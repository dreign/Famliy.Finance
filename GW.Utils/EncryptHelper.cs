using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace GW.Utils
{
    public class EncryptHelper
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public EncryptHelper()
        {
   
        }

        /// <summary>
        /// 从适用于URL的Base64编码字符串转换为普通字符串 
        /// Replace('.', '=').Replace('*', '+').Replace('-', '/')
        /// </summary>
        public static string FromBase64StringForUrl(string base64String)
        {
            string temp = base64String.Replace('.', '=').Replace('*', '+').Replace('-', '/');
            return Encoding.UTF8.GetString(Convert.FromBase64String(temp));
        }

        /// <summary>
        /// 从普通字符串转换为适用于URL的Base64编码字符串 
        /// Replace('+', '*').Replace('/', '-').Replace('=', '.')
        /// </summary>
        public static string ToBase64StringForUrl(string normalString)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(normalString)).Replace('+', '*').Replace('/', '-').Replace('=', '.');
        }
        #region AES加密
        /*
        //默认密钥向量   
        private static byte[] _key1 = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private static string str1 = Encoding.UTF8.GetString(_key1);
        private static string str2 = Convert.ToBase64String(_key1);
           
        /// <summary>  
        /// AES加密算法  
        /// </summary>  
        /// <param name="plainText">明文字符串</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回加密后的密文字节数组</returns>  
        public static byte[] AESEncrypt(string plainText, string strKey)
        {
            //分组加密算法  
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);//得到需要加密的字节数组      
            //设置密钥及密钥向量  
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            byte[] cipherBytes = ms.ToArray();//得到加密后的字节数组  
            cs.Close();
            ms.Close();
            return cipherBytes;
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="cipherText">密文字节数组</param>  
        /// <param name="strKey">密钥</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static byte[] AESDecrypt(byte[] cipherText, string strKey)
        {
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(strKey);
            des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            MemoryStream ms = new MemoryStream(cipherText);
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read);
            cs.Read(decryptBytes, 0, decryptBytes.Length);
            cs.Close();
            ms.Close();
            return decryptBytes;
        }  
         */
        /// <summary>
        /// AES加密 UTF8
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static string AESEncryptBase64(string strData, string Key, byte[] Vector)
        {
            //SymmetricAlgorithm.GenerateIV(); 
            byte[] bDatas = Encoding.UTF8.GetBytes(strData);
            byte[] bResults = AESEncrypt(bDatas, Key, Vector);
            //return Encoding.UTF8.GetString(bResults);
            return Convert.ToBase64String(bResults);
        }
        /// <summary>
        /// AES解密 UTF8
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static string AESDecryptBase64(string strData, String Key, byte[] Vector)
        {
            byte[] inputByteArray = Convert.FromBase64String(strData);
            //byte[] inputByteArray = Encoding.UTF8.GetBytes(strData);
            byte[] bResults = AESDecrypt(inputByteArray, Key, Vector);
            return Encoding.UTF8.GetString(bResults);
        }
        /// <summary>
        /// AES加密 UTF8
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static string AESEncrypt(string strData, string Key, byte[] Vector)
        {
            //SymmetricAlgorithm.GenerateIV(); 
            byte[] bDatas = Encoding.UTF8.GetBytes(strData);
            byte[] bResults = AESEncrypt(bDatas, Key, Vector);
            return Encoding.UTF8.GetString(bResults);
        }
        /// <summary>
        /// AES解密 UTF8
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static string AESDecrypt(string strData, String Key, byte[] Vector)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(strData);
            byte[] bResults = AESDecrypt(inputByteArray, Key, Vector);
            return Encoding.UTF8.GetString(bResults);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data">被加密的明文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>密文</returns>
        public static byte[] AESEncrypt(byte[] Data, String Key, byte[] bVector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            //byte[] bVector = new Byte[16];
            //Byte[] bVector = new Byte[16];
            //Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] Cryptograph = null; // 加密后的密文

            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流
                using (MemoryStream Memory = new MemoryStream())
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Encryptor = new CryptoStream(Memory,
                    Aes.CreateEncryptor(bKey, bVector),
                    CryptoStreamMode.Write))
                    {
                        // 明文数据写入加密流
                        Encryptor.Write(Data, 0, Data.Length);
                        Encryptor.FlushFinalBlock();

                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }

            return Cryptograph;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">被解密的密文</param>
        /// <param name="Key">密钥</param>
        /// <param name="Vector">向量</param>
        /// <returns>明文</returns>
        public static byte[] AESDecrypt(byte[] Data, string Key, byte[] bVector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            //Byte[] bVector = new Byte[16];
            //Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] original = null; // 解密后的明文

            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流，存储密文
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    // 把内存流对象包装成加密流对象
                    using (CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read))
                    {
                        // 明文存储区
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            Byte[] Buffer = new Byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }

            return original;
        }
       
        #endregion

        #region DES加密

        /// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public string DESEncrypt(string pToEncrypt, string sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public string DESDecrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
        #endregion

        #region MD5加密
        
        public string MD5Encrypt(string content)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] s = m.ComputeHash(Encoding.Default.GetBytes(content));
            return Encoding.Default.GetString(s);
        }

        public string MD5Encrypt(Encoding encoding,string content)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] s = m.ComputeHash(encoding.GetBytes(content));
            return encoding.GetString(s);
            //return BitConverter.ToString(s);
        }
         #endregion
    }
}