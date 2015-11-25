using System;
using System.Text;
using System.Linq;

namespace GW.Utils.TypeExtension
{
    public static class GuidExtension
    {
        public static string Shrink(this Guid target)
        {
            string base64 = Convert.ToBase64String(target.ToByteArray());

            string encoded = base64.Replace("/", "_").Replace("+", "-");

            return encoded.Substring(0, 22);
        }

        public static bool IsEmpty(this Guid target)
        {
            return target == Guid.Empty;
        }

        /// <summary>
        /// 将【Guid】数组转换成字符串;
        /// </summary>
        /// <param name="guids">【Guid】数组</param>
        /// <param name="splitSymbol">分隔符默认为【,】</param>
        /// <returns></returns>
        public static string ConvertToString(this Guid[] guids, Char splitSymbol = ',')
        {
            if (guids == null || guids.Length == 0)
                return string.Empty;
            var strTemp = new StringBuilder(guids.Length * 36);
            guids.ToList().ForEach(p => strTemp.AppendFormat("{0}{1}", p, splitSymbol));
            return strTemp.ToString().TrimEnd(splitSymbol);
        }

        /// <summary>
        /// 判断【Guid?】对象是否存在有效值;
        /// true:非null并且非Guid.Empty;
        /// </summary>
        /// <param name="guidParam">【Guid?】对象</param>
        /// <returns>true:非null并且非Guid.Empty;</returns>
        public static bool HasValidValue(this Guid? guidParam)
        {
            if (guidParam.HasValue && guidParam.Value != Guid.Empty)
                return true;
            return false;
        }
    }

}
