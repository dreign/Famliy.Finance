//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   字符串处理类，包括基本的字符串处理方法，常用的验证方法和HTML格式化操作
//编写日期    :   2010-11-16
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace GW.Utils
{
    /// <summary>
    /// 字符操作函数集合类
    /// </summary>
    public sealed class StringHelper
    {

        #region 字符串的拼接和截断

        /// <summary>
        /// 获取指定分隔符第一次出现以后的子字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="separator">分隔符</param>
        /// <returns>指定分隔符第一次出现以后的子字符串</returns>
        public static string SubstringAfter(string str, string separator)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(separator))
                return string.Empty;
            int _index = str.IndexOf(separator);
            return _index == -1 ? string.Empty : str.Substring(_index + separator.Length);
        }

        /// <summary>
        /// 将集合中的项通过Separator组合成字符串
        /// </summary>
        /// <param name="strList">集合参数</param>
        /// <param name="separator">分隔符</param>
        /// <returns>通过Separator组合成的字符串</returns>
        public static string Join(List<string> strList, string separator)
        {
            StringBuilder sb = new StringBuilder();
            int i = strList.Count;
            foreach (string str in strList)
            {
                i--;
                sb.Append(str);
                if (i > 0)
                    sb.Append(separator);
            }
            return sb.ToString();
        }

        #endregion

        #region 在集合中检查字符是否存在

        /// <summary>
        /// 判断指定字符串是否在集合中,如果在,返回Index,不在,返回-1,不区分大小写
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="strList">字符串集合参数</param>
        /// <returns>指定字符串在集合中的索引,如果在,返回Index,不在,返回-1,不区分大小写</returns>
        public static int IndexOf(string val, List<string> strList)
        {
            int i = 0;
            foreach (string str in strList)
            {
                if (string.Compare(val, str, true) == 0)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串是否在集合中,如果在,返回Index,不在,返回-1,可设置是否区分大小写
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="strs">字符串数组</param>
        /// <param name="isIgnoreCase">忽略大小写</param>
        /// <returns>可以区分大小写的指定字符串在集合中的索引,如果在,返回Index,不在,返回-1</returns>
        public static int IndexOf(string val, string[] strs, bool isIgnoreCase)
        {
            int i = 0;
            foreach (string str in strs)
            {
                if (string.Compare(val, str, isIgnoreCase) == 0)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串是否包含在集合的某项中,如果在,返回Index,不在,返回-1,可区分大小写
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="strs">字符串数组</param>
        /// <param name="isIgnoreCase">忽略大小写</param>
        /// <returns>可以区分大小写的指定字符串在数组中的索引,如果在,返回Index,不在,返回-1</returns>
        public static int IndexOfPartial(string val, string[] strs, bool isIgnoreCase)
        {
            string item;
            int i = 0;
            string tempVal = isIgnoreCase ? val.ToUpper() : val;
            foreach (string str in strs)
            {
                item = isIgnoreCase ? str.ToUpper() : str;
                if (item.IndexOf(tempVal) >= 0)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串是否包含在集合的某项中,如果在,返回Index,不在,返回-1,可区分大小写
        /// </summary>
        /// <param name="val">字符串</param>
        /// <param name="strList">字符串数组</param>
        /// <param name="isIgnoreCase">忽略大小写</param>
        /// <returns>可以区分大小写的指定字符串在集合中的某一项的索引,如果在,返回Index,不在,返回-1</returns>
        public static int IndexOfPartial(string val, List<string> strList, bool isIgnoreCase)
        {
            string item;
            int i = 0;
            string tempVal = isIgnoreCase ? val.ToUpper() : val;
            foreach (string str in strList)
            {
                item = isIgnoreCase ? str.ToUpper() : str;
                if (item.IndexOf(tempVal) >= 0)
                    return i;
                i++;
            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串是否在字典中,如果在,返回true,并设置Key,不在,返回false,可设置是否区分大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">字符串</param>
        /// <param name="strDict">字典集合</param>
        /// <param name="isIgnoreCase">忽略大小写</param>
        /// <param name="key">key值</param>
        /// <returns>可以区分大小写的指定字符串是否等于字典的key,如果在,返回true,并设置Key,不在,返回false</returns>
        public static bool InDict<T>(string val, Dictionary<T, string> strDict, bool isIgnoreCase, ref T key)
        {
            foreach (KeyValuePair<T, string> pair in strDict)
            {
                if (string.Compare(val, pair.Value, isIgnoreCase) == 0)
                {
                    key = pair.Key;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定字符串是否包含在字典的某项中,如果在,返回true,并设置Key,不在,返回false,可设置是否区分大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val">字符串</param>
        /// <param name="strDict">字典集合</param>
        /// <param name="isIgnoreCase">忽略大小写</param>
        /// <param name="key">key值</param>
        /// <returns>可以区分大小写的指定字符串是否在字典中,如果在,返回true,并设置Key,不在,返回false</returns>
        public static bool InDictPartial<T>(string val, Dictionary<T, string> strDict, bool isIgnoreCase, ref T key)
        {
            string item;
            string tempVal = isIgnoreCase ? val.ToUpper() : val;
            foreach (KeyValuePair<T, string> pair in strDict)
            {
                item = isIgnoreCase ? pair.Value.ToUpper() : pair.Value;
                if (item.IndexOf(tempVal) >= 0)
                {
                    key = pair.Key;
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 按长度截取字符串

        /// <summary>
        /// 根据汉字占两个字节，英文占一个字节的规则截取指定长度字符串（超过部分用"..."替换）
        /// </summary>
        /// <param name="originalString">要传入的字符串</param>
        /// <param name="Length">截取长度</param>
        /// <returns>根据汉字占两个字节，英文占一个字节的规则截取指定长度字符串（超过部分用"..."替换）后的值</returns>
        public static string SubStrBySingleByte(string originalString, int Length)
        {
            string ReturnString = "";
            if (Length < 1)
                Length = 1;
            int SingleByteLengthOfString = GetSingleByteLengthOfString(originalString);

            if (SingleByteLengthOfString > Length)
            {
                int temp_length = 1;
                for (int i = 0; i < originalString.Length; i++)
                {
                    if (GetSingleByteLengthOfString(originalString.Substring(0, i + 1)) >= Length)
                        break;
                    temp_length++;
                }
                ReturnString = originalString.Substring(0, temp_length) + "...";
            }
            else
                ReturnString = originalString;

            return ReturnString;
        }

        /// <summary>
        /// 获取字符串长度
        /// </summary>
        /// <param name="ss">字符串</param>
        /// <returns>字符串长度</returns>
        private static int GetSingleByteLengthOfString(String ss)
        {
            Char[] cc = ss.ToCharArray();
            int intLen = ss.Length;
            int i;
            if ("豆腐".Length == 4)
            {
                //是非中文的平台
                return intLen;
            }
            for (i = 0; i < cc.Length; i++)
            {
                if (Convert.ToInt32(cc[i]) > 255)
                {
                    intLen++;
                }
            }
            return intLen;
        }

        #endregion

        #region 字符串验证

        /// <summary>
        /// Email地址
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的字符串，true为合法，false为非法</returns>
        public static bool IsValidEmail(string input)
        {
            return Regex.IsMatch(input, @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$");
        }

        /// <summary>
        /// 验证是否为小数
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的小数，true为合法，false为非法</returns>
        public static bool IsValidDecimal(string input)
        {
            return Regex.IsMatch(input, @"^(-?\d+)(\.\d+)?$");
        }

        /// <summary>
        /// 验证是否为电话号码 
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的电话号码，true为合法，false为非法</returns>
        public static bool IsValidTel(string input)
        {
            return Regex.IsMatch(input, @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?");
        }

        /// <summary>
        /// 验证年月日，YYYY-MM-DD格式 
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的日期，true为合法，false为非法</returns>
        public static bool IsValidDate(string input)
        {
            return Regex.IsMatch(input, @"^(([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8]))))|((([0-9]{2})(0[48]|[2468][048]|[13579][26])|((0[48]|[2468][048]|[3579][26])00))-02-29)");
        }

        /// <summary>
        /// 验证是否是身份证号码
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的身份证号码，true为合法，false为非法</returns>
        public static bool IsIDCard(string input)
        {
            return Regex.IsMatch(input, @"(^\d{15}$)|(^\d{17}([0-9]|X)$)");
        }

        /// <summary>
        /// 验证后缀名的格式是否为gif和jpg
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>验证文件的后缀名是否为gif或者jpg格式，true为合法，false为非法</returns>
        public static bool IsValidPostfix(string input)
        {
            return Regex.IsMatch(input, @"\.(?i:gif|jpg)$");
        }

        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的IP地址，true为合法，false为非法</returns>
        public static bool IsValidIp(string input)
        {
            return Regex.IsMatch(input, @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }

        /// <summary>
        /// 验证数字格式是否合法
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的数字，true为合法，false为非法</returns>
        public static bool IsNum(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]+$");
        }

        /// <summary>
        /// 验证英文字符是否为合法的英文字母
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的英文字母，true为合法，false为非法</returns>
        public static bool IsChar(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 验证输入是否为汉字
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为汉字，true为是，false为非汉字</returns>
        public static bool IsChinese(string input)
        {
            return Regex.IsMatch(input, @"^[\u4e00-\u9fa5]{1,}$");
        }

        /// <summary>
        /// 验证网址是否合法
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为合法的网址，true为合法，false为非法</returns>
        public static bool IsUrl(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$");
        }

        /// <summary>
        /// 验证输入是否为HTML标签
        /// </summary>
        /// <param name="input">需要验证的内容</param>
        /// <returns>是否为html标签，true为合法，false为非法</returns>
        public static bool IsHTML(string input)
        {
            return Regex.IsMatch(input, @"<(.*)>.*<\/\1>|<(.*) \/>");
        }

        #endregion

        #region Html格式化

        /// <summary>
        /// 格式化为html，将\t\n等等内容替换成相应的html标签
        /// </summary>
        /// <param name="text">需要格式化的内容</param>
        /// <returns>格式化后的html</returns>
        public static string FormatToHtml(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;

            text = text.Replace("\t", "&nbsp;&nbsp;");
            text = text.Replace("\n", "<br>");
            text = text.Replace("\r", "<br>");
            text = text.Replace(" ", "&nbsp;");

            return (text);
        }

        /// <summary>
        /// 格式化为正常的文本，将&nbsp;等等内容替换成相应的文本格式标签，如\n
        /// </summary>
        /// <param name="htmlContent">需要格式化的内容</param>
        /// <returns>格式化后的正常文本</returns>
        public static string FormatToText(string htmlContent)
        {
            if (string.IsNullOrEmpty(htmlContent)) return string.Empty;

            htmlContent = htmlContent.Replace("&nbsp;", " ");
            htmlContent = htmlContent.Replace("<br>", "\n");
            htmlContent = htmlContent.Replace("<br />", "\n");
            htmlContent = htmlContent.Replace("<p>", "\n");
            htmlContent = htmlContent.Replace("<p />", "\n");

            return (htmlContent);
        }

        /// <summary>
        /// 清除字符串HTML
        /// </summary>
        /// <param name="text">字符串内容</param>
        /// <returns>清除html标签后的结果</returns>
        public static string ClearHtmlCode(string text)
        {
            text = text.Trim();
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = Regex.Replace(text, @"[\s]{2,}", " ");    //two or more spaces
            text = Regex.Replace(text, @"(<[b|B][r|R]/*>)+|(<[p|P](.|\n)*?>)", " ");    //<br>
            text = Regex.Replace(text, @"(\s*&[n|N][b|B][s|S][p|P];\s*)+", " ");    //&nbsp;
            text = Regex.Replace(text, @"<(.|\n)*?>", string.Empty);    //any other tags
            text = Regex.Replace(text, @"/<\/?[^>]*>/g", string.Empty);    //any other tags
            text = Regex.Replace(text, @"/[    | ]* /g", string.Empty);    //any other tags
            text = text.Replace("'", "''");
            text = Regex.Replace(text, @"/ [\s| |    ]* /g", string.Empty);
            return text;
        }

        #endregion
    }
}
