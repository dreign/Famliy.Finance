using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using GW.Utils;

namespace GW.Utils.TypeExtension
{
    public static class StringExtension
    {
        //        public static string NullSafe(this string target)
        //        {
        //            return (target ?? string.Empty).Trim();
        //        }

        //        public static Guid ToGuid(this string target)
        //        {
        //            if ((!string.IsNullOrEmpty(target)) && (target.Trim().Length == 22))
        //            {
        //                string encoded = string.Concat(target.Trim().Replace("-", "+").Replace("_", "/"), "==");

        //                byte[] base64 = Convert.FromBase64String(encoded);

        //                return new Guid(base64);
        //            }

        //            return Guid.Empty;
        //        }

        //        public static string Hash(this string target)
        //        {
        //            Check.Argument.IsNotEmpty(target, "target");

        //            using (MD5 md5 = MD5.Create())
        //            {
        //                byte[] data = Encoding.Unicode.GetBytes(target);
        //                byte[] hash = md5.ComputeHash(data);

        //                return Convert.ToBase64String(hash);
        //            }
        //        }

        //        public static T ToEnum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        //        {
        //            T convertedValue = defaultValue;

        //            if (!string.IsNullOrEmpty(target))
        //            {
        //                try
        //                {
        //                    convertedValue = (T)Enum.Parse(typeof(T), target.Trim(), true);
        //                }
        //                catch (ArgumentException)
        //                {
        //                }
        //            }

        //            return convertedValue;
        //        }

        //        public static string CleanQuoteTag(this string text)
        //        {
        //            if (string.IsNullOrEmpty(text))
        //                return text;

        //            RegexOptions options = RegexOptions.IgnoreCase;

        //            for (int i = 0; i < 20; i++)
        //                text = Regex.Replace(text, @"\[quote(?:\s*)user=""((.|\n)*?)""\]((.|\n)*?)\[/quote(\s*)\]", "", options);
        //            for (int i = 0; i < 20; i++)
        //                text = Regex.Replace(text, @"\[quote\]([^>]+?|.+?)\[\/quote\]", "", options);
        //            return text;
        //        }

        //        public static int TryToInt(this string text)
        //        {
        //            int i;
        //            int.TryParse(text, out i);
        //            return i;
        //        }

        //        public static DateTime TryToDatetime(this string text, DateTime defaultValue)
        //        {
        //            DateTime result;
        //            try
        //            {
        //                result = Convert.ToDateTime(text);
        //            }
        //            catch (Exception)
        //            {
        //                result = defaultValue;
        //            }
        //            return result;
        //        }

        //        public static string HtmlEncode(this string str)
        //        {
        //            if (string.IsNullOrEmpty(str))
        //            {
        //                return string.Empty;
        //            }
        //            return HttpUtility.HtmlEncode(str);
        //        }

        //        public static string HtmlDecode(this string str)
        //        {
        //            if (string.IsNullOrEmpty(str))
        //            {
        //                return string.Empty;
        //            }
        //            return HttpUtility.HtmlDecode(str);
        //        }
        public static string ToMd5(this string strText)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(strText))).ToLower().Replace("-", "");
        }
    }

}
