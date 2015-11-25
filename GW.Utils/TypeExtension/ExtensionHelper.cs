using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Utils.TypeExtension
{
    #region 基础类型扩展方法

    ///// <summary>
    ///// 基础类型扩展方法
    ///// </summary>
    //public static class ExtensionHelper
    //{
    //    /// <summary>
    //    /// 将【Guid】数组转换成字符串;
    //    /// </summary>
    //    /// <param name="guids">【Guid】数组</param>
    //    /// <param name="splitSymbol">分隔符默认为【,】</param>
    //    /// <returns></returns>
    //    public static string ConvertToString(this Guid[] guids, Char splitSymbol = ',')
    //    {
    //        if (guids == null || guids.Length == 0)
    //            return string.Empty;
    //        var strTemp = new StringBuilder(guids.Length * 36);
    //        guids.ToList().ForEach(p => strTemp.AppendFormat("{0}{1}", p, splitSymbol));
    //        return strTemp.ToString().TrimEnd(splitSymbol);
    //    }
    //    /// <summary>
    //    /// 将【Guid】列表对象转换成字符串;
    //    /// </summary>
    //    /// <param name="guids">【Guid】列表对象</param>
    //    /// <param name="splitSymbol">分隔符默认为【,】</param>
    //    /// <returns></returns>
    //    public static string ConvertToString(this List<Guid> guids, Char splitSymbol = ',')
    //    {
    //        if (guids == null || guids.Count == 0)
    //            return string.Empty;
    //        return guids.ToArray().ConvertToString();
    //    }

    //    /// <summary>
    //    /// 判断【Guid?】对象是否存在有效值;
    //    /// true:非null并且非Guid.Empty;
    //    /// </summary>
    //    /// <param name="guidParam">【Guid?】对象</param>
    //    /// <returns>true:非null并且非Guid.Empty;</returns>
    //    public static bool HasValidValue(this Guid? guidParam)
    //    {
    //        return guidParam.HasValue && guidParam.Value != Guid.Empty;
    //    }

    //    /// <summary>
    //    /// 判断【List】列表对象是否存在有效值;
    //    /// 返回值 true:非null并且List.Count>0;
    //    /// </summary>
    //    /// <param name="listParams">【List】列表对象</param>
    //    /// <returns>true:非null并且List.Count>0;</returns>
    //    public static bool HasValidValues<T>(this IList<T> listParams)
    //    {
    //        return listParams != null && listParams.Count > 0;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="array"></param>
    //    /// <returns></returns>
    //    public static bool HasValidValues(this Array array)
    //    {
    //        return array != null && array.Length > 0;
    //    }

    //    /// <summary>
    //    /// 判断【DateTime?】是否存在有效值;
    //    /// 返回值 true:非null并且DateTime.Year>2000
    //    /// </summary>
    //    /// <param name="dateTimeParams">日期对象</param>
    //    /// <returns>返回值 true:非null并且DateTime.Year>2000</returns>
    //    public static bool HasValidValues(this DateTime? dateTimeParams)
    //    {
    //        if (dateTimeParams != null && dateTimeParams.Value.Year > 2000)
    //            return true;
    //        return false;
    //    }

    //    /// <summary>
    //    /// 截取字节数组
    //    /// </summary>
    //    /// <param name="originalArray">原始字节数组</param>
    //    /// <param name="startIndex">开始截取下标</param>
    //    /// <param name="arrayLength">截取长度</param>
    //    /// <returns></returns>
    //    public static byte[] SubArray(this byte[] originalArray, int startIndex, int arrayLength)
    //    {
    //        var resultArray = new byte[arrayLength];
    //        for (var i = 0; i < arrayLength; i++)
    //            resultArray[i] = originalArray[i + startIndex];
    //        return resultArray;
    //    }

    //    /// <summary>
    //    /// 截取字节数组
    //    /// </summary>
    //    /// <param name="originalArray">原始字节数组</param>
    //    /// <param name="startIndex">开始截取下标</param>
    //    /// <returns></returns>
    //    public static byte[] SubArray(this byte[] originalArray, int startIndex)
    //    {
    //        var arrayLength = originalArray.Length - startIndex;
    //        var resultArray = new byte[arrayLength];
    //        for (var i = 0; i < arrayLength; i++)
    //            resultArray[i] = originalArray[i + startIndex];
    //        return resultArray;
    //    }

    //}

    #endregion
}
