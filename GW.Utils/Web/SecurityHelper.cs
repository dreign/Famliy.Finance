//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   安全处理类，包括防SQL注入，防JS脚本注入和加解密
//                加密方法有对称加密和MD5加密，对称加密的加解密的密钥在程序中写死
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using System.Web;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace GW.Utils.Web
{
    /// <summary>
    /// 安全处理类
    /// </summary>
    public sealed class SecurityHelper
    {
        #region 成员变量

        //对称加密密钥
        private const string CONST_CRYPTOKEY = "!~oX@y$]2wiq3#Fj";
        private const string CONST_CRYPTOVI = "!~l7D@0#VI$w5%r&";
        //DES加解密用的Key
        private const string CONST_KEY = "gw.framework.security";
        //公钥
        private static string CONST_PUBLICKEY = "<RSAKeyValue>" +
                                                "<Modulus>6CdsXgYOyya/yQHTO96dB3gEurM2UQDDVGrZoe6RcAVTxAqDDf5LwPycZwtNOx3Cfy44/D5Mj86koPew5soFIz9sx" +
                                                "PAHRF5hcqJoG+q+UfUYTHYCsMH2cnqGVtnQiE/PMRMmY0RwEfMIo+TDpq3QyO03MaEsDGf13sPw9YRXiac=</Modulus>" +
                                                "<Exponent>AQAB</Exponent>" +
                                                "</RSAKeyValue>";
        //私钥
        private static string CONST_PRIVATEKEY ="<RSAKeyValue>" +
                                                "<Modulus>6CdsXgYOyya/yQHTO96dB3gEurM2UQDDVGrZoe6RcAVTxAqDDf5LwPycZwtNOx3Cfy44/D5Mj86koPew5soFI" +
                                                "z9sxPAHRF5hcqJoG+q+UfUYTHYCsMH2cnqGVtnQiE/PMRMmY0RwEfMIo+TDpq3QyO03MaEsDGf13sPw9YRXiac=</Modulus>" +
                                                "<Exponent>AQAB</Exponent>" +
                                                "<P>/aoce2r6tonjzt1IQI6FM4ysR40j/gKvt4dL411pUop1Zg61KvCm990M4uN6K8R/DUvAQdrRdVgzvvAxXD7ESw==</P>" +
                                                "<Q>6kqclrEunX/fmOleVTxG4oEpXY4IJumXkLpylNR3vhlXf6ZF9obEpGlq0N7sX2HBxa7T2a0WznOAb0si8FuelQ==</Q>" +
                                                "<DP>3XEvxB40GD5v/Rr4BENmzQW1MBFqpki6FUGrYiUd2My+iAW26nGDkUYMBdYHxUWYlIbYo6Tezc3d/oW40YqJ2Q==</DP>" +
                                                "<DQ>LK0XmQCmY/ArYgw2Kci5t51rluRrl4f5l+aFzO2K+9v3PGcndjAStUtIzBWGO1X3zktdKGgCLlIGDrLkMbM21Q==</DQ>" +
                                                "<InverseQ>GqC4Wwsk2fdvJ9dmgYlej8mTDBWg0Wm6aqb5kjncWK6WUa6CfD+XxfewIIq26+4Etm2A8IAtRdwPl4aPjSfWdA==</InverseQ>" +
                                                "<D>a1qfsDMY8DSxB2DCr7LX5rZHaZaqDXdO3GC01z8dHjI4dDVwOS5ZFZs7MCN3yViPsoRLccnVWcLzOkSQF4lgKfTq3IH40H5" +
                                                "N4gg41as9GbD0g9FC3n5IT4VlVxn9ZdW+WQryoHdbiIAiNpFKxL/DIEERur4sE1Jt9VdZsH24CJE=</D>" +
                                                "</RSAKeyValue>";

        #endregion

        #region 防止SQL注入

        /// <summary>
        /// 防止SQL注入,替换SQL敏感字符
        /// </summary>
        /// <param name="inputString">SQL字符串</param>
        /// <returns>替换SQL敏感字符后的值</returns>
        public static string ReplaceSQL(string inputString)
        {
            string pattern = "*('|''|--|).*/g";
            inputString = Regex.Replace(inputString, pattern, string.Empty);
            return inputString;
        }

        #endregion

        #region 防止XSS攻击

        /// <summary>
        /// 从输入文本中获取字符串，需要检查输入的有效性
        /// </summary>
        /// <param name="contents">输入文本</param>
        /// <returns>检查处理后的文本</returns>
        public static string FilterInvlidContents(string contents)
        {
            return FilterInvlidContents(contents, contents.Length);
        }

        /// <summary>
        /// 从输入文本中获取字符串，需要检查输入的有效性
        /// </summary>
        /// <param name="contents">输入文本</param>
        /// <param name="maxLength">文本的长度</param>
        /// <returns>检查处理后的文本</returns>
        public static string FilterInvlidContents(string contents, int maxLength)
        {
            //过滤js脚本和html 
            Regex regex1 = new Regex(@"<script[/s/S]+</script *>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex(@" href *= *[/s/S]*script *:", RegexOptions.IgnoreCase);
            Regex regex3 = new Regex(@" on[/s/S]*=", RegexOptions.IgnoreCase);
            Regex regex4 = new Regex(@"<iframe[/s/S]+</iframe *>", RegexOptions.IgnoreCase);
            Regex regex5 = new Regex(@"<frameset[/s/S]+</frameset *>", RegexOptions.IgnoreCase);

            contents = regex1.Replace(contents, string.Empty);        //过滤<script></script>标记  
            contents = regex2.Replace(contents, string.Empty);        //过滤href="/u/javascript: ";(<A>) 属性  
            contents = regex3.Replace(contents, " _disibledevent=");  //过滤其它控件的on...事件  
            contents = regex4.Replace(contents, string.Empty);        //过滤iframe  
            contents = regex5.Replace(contents, string.Empty);        //过滤frameset  

            //过滤危险字符
            StringBuilder retVal = new StringBuilder();

            if (!string.IsNullOrEmpty(contents))
            {
                contents = contents.Trim();
                if (contents.Length > maxLength)
                {
                    contents = contents.Substring(0, maxLength);
                }
                for (int index = 0; index < contents.Length; index++)
                {
                    switch (contents[index])
                    {
                        case '%':
                            retVal.Append(string.Empty);
                            break;
                        //case '/':
                        //    retVal.Append("");
                        //    break;
                        //case '*':
                        //    retVal.Append("");
                        //    break;
                        //case '-':
                        //    retVal.Append("");
                        //    break;
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(contents[index]);
                            break;
                    }
                }

                //Replace single quotes with white space 
                retVal.Replace("'", " ");
            }
            return retVal.ToString();
        }

        #endregion

        #region 加解密方法

        #region 对称式加解密

        /// <summary>
        /// 加密函数,根据密钥把字符串加密成流
        /// </summary>
        /// <param name="needEncryptString">需要加密的内容</param>
        /// <returns>密文</returns>
        public static string EncryptData(string needEncryptString)
        {
            byte[] cliperText;
            RijndaelManaged RP = new RijndaelManaged();
            MemoryStream ms = null;
            CryptoStream cs = null;

            try
            {
                RP.Key = Encoding.UTF8.GetBytes(CONST_CRYPTOKEY);
                RP.IV = Encoding.UTF8.GetBytes(CONST_CRYPTOVI);
                ICryptoTransform RE = RP.CreateEncryptor();
                cliperText = Encoding.UTF8.GetBytes(needEncryptString);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, RE, CryptoStreamMode.Write);
                cs.Write(cliperText, 0, cliperText.Length);
                cs.FlushFinalBlock();
                cliperText = ms.ToArray();
            }
            finally
            {
                cs.Close();
                ms.Close();
                RP.Clear();
            }

            return Convert.ToBase64String(cliperText, 0, cliperText.Length);
        }

        /// <summary>
        /// 解密函数，读取加密后的字符，根据密钥把密文还原成字符串
        /// </summary>
        /// <param name="needDecryString">密文</param>
        /// <returns>明文</returns>
        public static string DecrypData(string needDecryString)
        {
            byte[] receviData = Convert.FromBase64CharArray(needDecryString.ToCharArray(), 0, needDecryString.ToCharArray().Length);
            int receviLength = receviData.Length;
            string encoded = string.Empty;
            RijndaelManaged RP = new RijndaelManaged();
            StreamReader sr = null;
            MemoryStream MS = null;
            CryptoStream cs = null;

            try
            {
                RP.Key = Encoding.UTF8.GetBytes(CONST_CRYPTOKEY);
                RP.IV = Encoding.UTF8.GetBytes(CONST_CRYPTOVI);
                ICryptoTransform RD = RP.CreateDecryptor();
                MS = new MemoryStream(receviData, 0, receviLength);
                cs = new CryptoStream(MS, RD, CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                encoded = sr.ReadToEnd();
            }
            finally
            {
                sr.Close();
                cs.Close();
                MS.Close();
                RP.Clear();
            }
            return encoded;
        }

        #region DES加解密

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string dataToEncrypt)
        {
            return DESEncrypt(dataToEncrypt, CONST_KEY);
        }

        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <param name="keys">密钥</param>
        /// <returns>密文</returns>
        public static string DESEncrypt(string dataToEncrypt, string keys)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            //把字符串放到byte数组中
            byte[] inputByteArray = Encoding.UTF8.GetBytes(dataToEncrypt);

            //建立加密对象的密钥和偏移量,使得输入密码必须输入英文文本
            desProvider.Key = ASCIIEncoding.ASCII.GetBytes(keys);
            desProvider.IV = ASCIIEncoding.ASCII.GetBytes(keys);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desProvider.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder retEncrypt = new StringBuilder();

            foreach (byte b in ms.ToArray())
            {
                retEncrypt.AppendFormat("{0:X2}", b);
            }

            retEncrypt.ToString();

            return retEncrypt.ToString();
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="dataToDecrypt">密文</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string dataToDecrypt)
        {
            return DESDecrypt(dataToDecrypt, CONST_KEY);
        }

        /// <summary>
        /// 解密方法
        /// </summary>
        /// <param name="dataToDecrypt">密文</param>
        /// <param name="keys">密匙</param>
        /// <returns>明文</returns>
        public static string DESDecrypt(string dataToDecrypt, string keys)
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[dataToDecrypt.Length / 2];

            for (int index = 0; index < dataToDecrypt.Length / 2; index++)
            {
                int itemValue = (Convert.ToInt32(dataToDecrypt.Substring(index * 2, 2), 16));
                inputByteArray[index] = (byte)itemValue;
            }

            //建立加密对象的密钥和偏移量，此值重要，不能修改
            desProvider.Key = ASCIIEncoding.ASCII.GetBytes(keys);
            desProvider.IV = ASCIIEncoding.ASCII.GetBytes(keys);

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desProvider.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            //建立StringBuild对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象
            StringBuilder retDecrypt = new StringBuilder();

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion

        #endregion

        #region 不可逆加密

        /// <summary>
        /// MD5加密（采用系统的MD5实现方法，本方法简单封装）
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <returns>MD5加密算法加密后的密文</returns>
        public static string MD5Encode(string dataToEncrypt)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(provider.ComputeHash(Encoding.Default.GetBytes(dataToEncrypt)));
        }

        /// <summary>
        /// SHA1加密（采用系统的SHA1实现方法，本方法简单封装）
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <returns>SHA1加密算法加密后的密文</returns>
        public string EncryptToSHA1(string dataToEncrypt)
        {
            SHA1CryptoServiceProvider provider = new SHA1CryptoServiceProvider();
            return Convert.ToBase64String(provider.ComputeHash(Encoding.Default.GetBytes(dataToEncrypt)));
        }

        #endregion

        #region 简单加解密

        /// <summary>
        /// 将字节数组转换成十六进制字符串
        /// </summary>
        /// <param name="buffer">字节数组</param>
        /// <returns>密文</returns>
        public static string ByteArrayToHexString(byte[] buffer)
        {
            if ((buffer == null) || (0 == buffer.Length))
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < buffer.Length; index++)
            {
                builder.Append(string.Format("{0:X2}", buffer[index]));
            }

            return builder.ToString();
        }

        /// <summary>
        /// 将十六进制字符串转换成字节数组
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>字节数组格式的密文</returns>
        public static byte[] HexStringToByteArray(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                return null;
            }
            if ((hexString.Length % 2) != 0)
            {
                hexString = "0" + hexString;
            }

            int num = hexString.Length / 2;
            byte[] buffer = new byte[num];
            for (int index = 0; index < num; index++)
            {
                try
                {
                    buffer[index] = byte.Parse(hexString.Substring(index * 2, 2), NumberStyles.HexNumber, (IFormatProvider)null);
                }
                catch
                {
                    buffer[index] = 0xff;
                }
            }
            return buffer;
        }

        #endregion

        #region 不对称之RSA加解密

        /// <summary>
        /// 生成公钥和私钥信息
        /// </summary>
        /// <param name="publicKeyXmlFilePath">公钥xml文件地址</param>
        /// <param name="privateKeyXmlFilePath">私钥xml文件地址</param>
        public static void ExportKeyXml(string publicKeyXmlFilePath, string privateKeyXmlFilePath)
        {
            //ExportParameters和ToXmlString选true时表示公钥和私钥信息同时导出，选false则只导出公钥信息
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            StreamWriter swPublic = new StreamWriter(publicKeyXmlFilePath);
            swPublic.Write(rsa.ToXmlString(false));
            swPublic.Close();

            StreamWriter swPrivate = new StreamWriter(privateKeyXmlFilePath);
            swPrivate.Write(rsa.ToXmlString(true));
            swPrivate.Close();
        }

        /// <summary>
        /// RSA加密方法（默认使用系统提供的私钥加密）
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <returns>RSA加密算法加密后的密文</returns>
        public static string RSAEncrypt(string dataToEncrypt)
        {
            return RSAEncrypt(dataToEncrypt, CONST_PUBLICKEY);
        }

        /// <summary>
        /// RSA加密方法
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <param name="publicKey">公钥</param>
        /// <returns>RSA加密算法加密后的密文</returns>
        public static string RSAEncrypt(string dataToEncrypt,string publicKey)
        {
            //Create a UnicodeEncoder to convert between byte array and string.
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            //Create byte arrays to hold original, encrypted, and decrypted data.
            byte[] toEncryptString = ByteConverter.GetBytes(dataToEncrypt);
            byte[] encryptedData;

            //Create a new instance of RSACryptoServiceProvider to generate
            //public and private key data.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            RSA.FromXmlString(publicKey);

            //Pass the data to ENCRYPT, the public key information 
            //(using RSACryptoServiceProvider.ExportParameters(false),
            //and a boolean flag specifying no OAEP padding.
            encryptedData = RSAEncrypt(toEncryptString, RSA.ExportParameters(false), false);

            string base64code = Convert.ToBase64String(encryptedData);
            return base64code;
        }

        /// <summary>
        /// RSA解密方法（默认使用系统提供的私钥解密）
        /// </summary>
        /// <param name="dataToDecrypt">密文</param>
        /// <returns>RSA解密算法解密后的明文</returns>
        public static string RSADecrypt(string dataToDecrypt)
        {
            return RSADecrypt(dataToDecrypt, CONST_PRIVATEKEY);
        }

        /// <summary>
        /// RSA解密方法
        /// </summary>
        /// <param name="dataToDecrypt">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>RSA解密算法解密后的明文</returns>
        public static string RSADecrypt(string dataToDecrypt, string privateKey)
        {
            //Create a UnicodeEncoder to convert between byte array and string.
            UnicodeEncoding ByteConverter = new UnicodeEncoding();

            //Create a new instance of RSACryptoServiceProvider to generate
            //public and private key data.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            RSA.FromXmlString(privateKey);

            byte[] encryptedData;
            byte[] decryptedData;

            encryptedData = Convert.FromBase64String(dataToDecrypt);

            //Pass the data to DECRYPT, the private key information 
            //(using RSACryptoServiceProvider.ExportParameters(true),
            //and a boolean flag specifying no OAEP padding.
            decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

            //Display the decrypted plaintext to the console. 
            return ByteConverter.GetString(decryptedData);
        }

        /// <summary>
        /// 私有RSA加密方法
        /// </summary>
        /// <param name="dataToEncrypt">明文</param>
        /// <param name="RSAKeyInfo">RSA参数</param>
        /// <param name="isOAEPPadding">是否是OAEP方式</param>
        /// <returns>RSA加密算法加密后的密文</returns>
        private static byte[] RSAEncrypt(byte[] dataToEncrypt, RSAParameters RSAKeyInfo, bool isOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This only needs
            //toinclude the public key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Encrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP or
            //later.  
            return RSA.Encrypt(dataToEncrypt, isOAEPPadding);
        }

        /// <summary>
        /// 私有RSA解密方法
        /// </summary>
        /// <param name="dataToDecrypt">密文</param>
        /// <param name="RSAKeyInfo">RSA参数</param>
        /// <param name="isOAEPPadding">是否是OAEP方式</param>
        /// <returns>RSA解密算法解密后的明文</returns>
        private static byte[] RSADecrypt(byte[] dataToDecrypt, RSAParameters RSAKeyInfo, bool isOAEPPadding)
        {
            //Create a new instance of RSACryptoServiceProvider.
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

            //Import the RSA Key information. This needs
            //to include the private key information.
            RSA.ImportParameters(RSAKeyInfo);

            //Decrypt the passed byte array and specify OAEP padding.  
            //OAEP padding is only available on Microsoft Windows XP orlater.  
            return RSA.Decrypt(dataToDecrypt, isOAEPPadding);
        }

        #endregion

        #endregion

    }
}
