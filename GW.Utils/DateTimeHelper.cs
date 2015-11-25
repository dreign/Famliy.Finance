
//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   日期处理，提供各种日期处理方法
//编写日期    :   2010-11-12
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace GW.Utils
{
    /// <summary>
    /// 日期常用操作函数集合
    /// </summary>
    public sealed class DateTimeHelper
    {
        #region Constants

        private const string CAST_ERROR_MSG = "ParserHelper.{0} Error.object:{1},type:{2}.";
        private const string NULL_ERROR_MSG = "ParserHelper.{0} Error.Object is null or dbnull.";

        #endregion

        #region Date

        /// <summary>
        /// 根据默认格式取得今天日期字符串，如:"yyyy-MM-dd"
        /// </summary>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string GetTodayStr()
        {
            return ParseDateToStr(DateTime.Now);
        }

        /// <summary>
        /// 根据默认格式取得和今天相差diffDay的日期字符串，如:"yyyy-MM-dd"
        /// </summary>
        /// <param name="diffDay">相差的天数</param>
        /// <returns>yyyy-MM-dd格式的相差某些天的日期字符串</returns>
        public static string GetTodayDiffStr(int diffDay)
        {
            return ParseDateToStr(DateTime.Now.AddDays(diffDay));
        }

        /// <summary>
        /// 根据短日期格式要求取得今天日期字符串
        /// </summary>
        /// <returns>今天日期字符串</returns>
        public static string GetTodayShortDateStr()
        {
            return ParseDateToShortDateStr(DateTime.Now);
        }

        /// <summary>
        /// 根据格式要求取得今天日期字符串
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <returns>特定格式的今天日期字符串</returns>
        public static string GetTodayStr(string format)
        {
            return ParseDateToStr(DateTime.Now, format);
        }

        /// <summary>
        /// 获取本周周一日期并转化为数字
        /// </summary>
        /// <returns>数字格式的本周周一日期</returns>
        public static int GetFirstDayOfWeek()
        {
            return ParseDateToInt(DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1));
        }

        /// <summary>
        /// 获取本周周未日期并转化为数字
        /// </summary>
        /// <returns>数字格式的本周周未日期</returns>
        public static int GetLastDayOfWeek()
        {
            return ParseDateToInt(DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek));
        }

        #endregion

        #region Month

        /// <summary>
        /// 根据默认格式取得当月日期字符串，如:"yyyy-MM"
        /// </summary>
        /// <returns>yyyy-MM格式的月份字符串</returns>
        public static string GetMonthStr()
        {
            return GetMonthStr(DateTime.Now);
        }

        /// <summary>
        /// 根据默认短月份格式取得当月日期字符串，如:"yyyyMM"
        /// </summary>
        /// <returns>yyyyMM格式的月份字符串</returns>
        public static string GetMonthShortDateStr()
        {
            return GetMonthStr(DateTime.Now);
        }

        /// <summary>
        /// 根据格式取得当月日期字符串
        /// </summary>
        /// <param name="format">格式</param>
        /// <returns>传入格式的月份字符串</returns>
        public static string GetMonthStrByFormat(string format)
        {
            return GetMonthStr(DateTime.Now, format);
        }

        /// <summary>
        /// 根据默认格式取得当月月份数字
        /// </summary>
        /// <returns>当月月份数字</returns>
        public static int GetCurMonth()
        {
            return DateTime.Now.Month;
        }

        /// <summary>
        /// 根据默认格式取得日期的月份字符串，如:"yyyy-MM"
        /// </summary>
        /// <param name="dateTime">需要格式化的日期</param>
        /// <returns>传入日期的yyyy-MM格式的月份字符串</returns>
        public static string GetMonthStr(DateTime dateTime)
        {
            return GetMonthStr(dateTime, BaseConstants.DEFAULT_MONTH_FORMAT);
        }

        /// <summary>
        /// 根据默认格式取得日期的月份字符串，如:"yyyyMM"
        /// </summary>
        /// <param name="date">传入的日期</param>
        /// <returns>传入日期的yyyyMM格式的月份字符串</returns>
        public static string GetMonthShortDateStr(DateTime date)
        {
            return GetMonthStr(date, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        /// <summary>
        /// 根据格式取得日期的月份字符串
        /// </summary>
        /// <param name="dateTime">需要格式化的日期</param>
        /// <param name="format">格式化的格式</param>
        /// <returns>传入日期的传入格式的月份字符串</returns>
        public static string GetMonthStr(DateTime dateTime, string format)
        {
            return ParseDateToStr(dateTime, format);
        }

        /// <summary>
        /// 根据默认格式取得日期的月份字符串，如:"yyyy-MM",转化不成功，返回空字符串
        /// </summary>
        /// <param name="obj">需要格式化的日期对象</param>
        /// <returns>取得日期的月份字符串，如:"yyyy-MM",转化不成功，返回空字符串</returns>
        public static string GetMonthStr(object obj)
        {
            try
            {
                return GetMonthStr(ParseFromShortDateEx(obj));
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        #region ToDateTime

        #region OADateToDateTime

        /// <summary>
        /// OA类型数据转换为DateTime,当对象为空或转换错误时返回指定默认值
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>DateTime类型数据，发生异常则返回默认日期</returns>
        public static DateTime ParseFromOADate(object obj, DateTime defaultVal)
        {
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    return DateTime.FromOADate(Convert.ToDouble(obj));
                }
                catch { }
            }
            return defaultVal;
        }

        /// <summary>
        /// OA类型数据转换为DateTime,当对象为空或转换错误时返回默认值
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>日期格式数据</returns>
        public static DateTime ParseFromOADate(object obj)
        {
            return ParseFromOADate(obj, BaseConstants.DEFAULT_DATETIME);
        }

        /// <summary>
        /// OA类型数据转换为DateTime,当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>日期格式数据</returns>
        public static DateTime ParseFromOADateEx(object obj)
        {
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    return DateTime.FromOADate(Convert.ToDouble(obj));
                }
                catch { }
                throw new InvalidCastException(string.Format(CAST_ERROR_MSG, "ParseFromOADateEx", obj.ToString(), "DateTime"));
            }
            throw new InvalidCastException(string.Format(NULL_ERROR_MSG, "ParseFromOADateEx"));
        }

        #endregion

        #region ShortDateToDateTime

        /// <summary>
        /// 将短日期格式数据转换为DateTime,形如：20090121,支持4位以上字符串,当对象为空或转换错误时返回指定默认值
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>日期格式数据</returns>
        public static DateTime ParseFromShortDate(object obj, DateTime defaultVal)
        {
            DateTime val = defaultVal;
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    ParseFromShortDate(obj, ref val);
                }
                catch { }
            }
            return val;
        }

        /// <summary>
        /// 将紧格式数据转换为DateTime,形如：20090121,支持4位以上字符串,当对象为空或转换错误时返回默认值
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>日期格式数据</returns>
        public static DateTime ParseFromShortDate(object obj)
        {
            return ParseFromShortDate(obj, BaseConstants.DEFAULT_DATETIME);
        }

        /// <summary>
        /// 将紧格式数据转换为DateTime,形如：20090121,支持4位以上字符串,当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>日期格式数据</returns>
        public static DateTime ParseFromShortDateEx(object obj)
        {
            DateTime val = BaseConstants.DEFAULT_DATETIME;
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    if (ParseFromShortDate(obj, ref val))
                        return val;
                }
                catch { }
                throw new InvalidCastException(string.Format(CAST_ERROR_MSG, "ParseFromShortDateEx", obj.ToString(), "DateTime"));
            }
            throw new InvalidCastException(string.Format(NULL_ERROR_MSG, "ParseFromShortDateEx"));
        }

        #endregion

        #endregion

        #region ToStr

        #region DateTimeToStr
        /// <summary>
        /// 根据格式取得日期字符串
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>传入格式的日期字符串</returns>
        public static string ParseToStr(DateTime date, string format)
        {
            return date.ToString(format);
        }

        /// <summary>
        /// 根据默认格式取得日期字符串，如:"yyyy-MM-dd",默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string ParseToStr(DateTime date)
        {
            return ParseToStr(date, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 取得短类型日期字符串，如:"yyyyMMdd"
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>yyyyMMdd格式的日期字符串</returns>
        public static string ParseToShortDateStr(DateTime date)
        {
            return ParseToStr(date, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        #endregion

        #region DateTimeObjToStr

        /// <summary>
        /// 根据格式要求取得日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <param name="format">格式字符串</param>
        /// <returns>传入格式的日期字符串，转换过程中发生错误则返回传入的默认值</returns>
        public static string ParseDateToStr(object obj, DateTime defaultVal, string format)
        {
            return ParseHelper.Parse<DateTime>(obj, defaultVal).ToString(format);
        }

        /// <summary>
        /// 根据默认格式要求取得日期字符串,如:"yyyy-MM-dd",默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns>yyyy-MM-dd格式的日期字符串，转换过程中发生错误则返回传入的默认值</returns>
        public static string ParseDateToStr(object obj, DateTime defaultVal)
        {
            return ParseDateToStr(obj, defaultVal, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式要求取得日期字符串,如:"yyyyMMdd",默认为DEFAULT_DATETIME的短日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns>yyyyMMdd格式的日期字符串，转换过程中发生错误则返回传入的默认值</returns>
        public static string ParseDateToShortDateStr(object obj, DateTime defaultVal)
        {
            return ParseDateToStr(obj, defaultVal, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        /// <summary>
        /// 根据格式要求取得日期字符串,默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>传入特定格式的日期字符串，如果转换过程中发生错误则按系统默认值返回</returns>
        public static string ParseDateToStr(object obj, string format)
        {
            return ParseDateToStr(obj, BaseConstants.DEFAULT_DATETIME, format);
        }

        /// <summary>
        /// 根据默认格式取得日期字符串,如:"yyyy-MM-dd",默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyy-MM-dd格式的日期字符串，如果转换过程中发生错误则按系统默认值返回</returns>
        public static string ParseDateToStr(object obj)
        {
            return ParseDateToStr(obj, BaseConstants.DEFAULT_DATETIME, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式取得日期字符串,如:"yyyyMMdd",默认为DEFAULT_DATETIME的短日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyyMMdd格式的日期字符串，如果转换过程中发生错误则按系统默认值返回</returns>
        public static string ParseDateToShortDateStr(object obj)
        {
            return ParseDateToStr(obj, BaseConstants.DEFAULT_DATETIME, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        /// <summary>
        /// 根据格式要求取得日期字符串,当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>特定格式的日期字符串</returns>
        public static string ParseDateToStrEx(object obj, string format)
        {
            return ParseHelper.Parse<DateTime>(obj).ToString(format);
        }

        /// <summary>
        /// 根据默认格式取得日期字符串，如:"yyyy-MM-dd",当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string ParseDateToStrEx(object obj)
        {
            return ParseDateToStrEx(obj, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式取得日期字符串，如:"yyyyMMdd",当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyyMMdd格式的日期字符串</returns>
        public static string ParseDateToShortDateStrEx(object obj)
        {
            return ParseDateToStrEx(obj, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        #endregion

        #region ShortDateObjToStr

        /// <summary>
        /// 根据格式要求取得短类型日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <param name="format">格式字符串</param>
        /// <returns>特定格式的日期字符串，发生错误将返回传入的默认日期</returns>
        public static string ParseShortDateToStr(object obj, DateTime defaultVal, string format)
        {
            if (obj == null || obj == DBNull.Value)
                return string.Empty;
            return ParseFromShortDate(obj, defaultVal).ToString(format);
        }

        /// <summary>
        /// 根据默认格式要求取得短类型日期字符串，如:"yyyy-MM-dd",
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string ParseShortDateToStr(object obj, DateTime defaultVal)
        {
            return ParseShortDateToStr(obj, defaultVal, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式要求取得短类型日期字符串，如:"yyyyMMdd",
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns>yyyyMMdd格式的日期字符串</returns>
        public static string ParseShortDateToShortDateStr(object obj, DateTime defaultVal)
        {
            return ParseShortDateToStr(obj, defaultVal, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        /// <summary>
        /// 根据格式要求取得短类型日期字符串,默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>传入格式的日期字符串</returns>
        public static string ParseShortDateToStr(object obj, string format)
        {
            return ParseShortDateToStr(obj, BaseConstants.DEFAULT_DATETIME, format);
        }

        /// <summary>
        /// 根据默认格式取得短类型日期字符串,如:"yyyy-MM-dd",默认为DEFAULT_DATETIME的日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string ParseShortDateToStr(object obj)
        {
            return ParseShortDateToStr(obj, BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式取得短类型日期字符串,如:"yyyyMMdd",默认为DEFAULT_DATETIME的短格式日期字符串
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyyMMdd格式的日期字符串</returns>
        public static string ParseShortDateToShortDateStr(object obj)
        {
            return ParseShortDateToStr(obj, BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        /// <summary>
        /// 根据格式取得短类型日期字符串,当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="format">格式字符串</param>
        /// <returns>特定格式的日期字符串</returns>
        public static string ParseShortDateToStrEx(object obj, string format)
        {
            return ParseShortDateToStrEx(obj, format);
        }

        /// <summary>
        /// 根据默认格式取得短类型日期字符串,如:"yyyy-MM-dd",当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyy-MM-dd格式的日期字符串</returns>
        public static string ParseShortDateToStrEx(object obj)
        {
            return ParseFromShortDateEx(obj).ToString(BaseConstants.DEFAULT_DATE_FORMAT);
        }

        /// <summary>
        /// 根据默认短日期格式取得短类型日期字符串,如:"yyyyMMdd",当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>yyyyMMdd格式的日期字符串</returns>
        public static string ParseShortDateToShortDateStrEx(object obj)
        {
            return ParseFromShortDateEx(obj).ToString(BaseConstants.DEFAULT_SHORTDATETIME_FORMAT);
        }

        #endregion

        #endregion

        #region ToInt

        #region DateToInt

        /// <summary>
        /// 取得日期数字,如:"yyyyMMdd"
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns>日期数字</returns>
        public static int ParseDateToInt(DateTime date)
        {
            return ParseHelper.Parse<int>(date.ToString(BaseConstants.DEFAULT_SHORTDATETIME_FORMAT));
        }

        #endregion

        #region DateTimeObjToInt

        /// <summary>
        /// 将日期对象转换为数字,如:"yyyyMMdd"
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns>日期对象转换后的数字</returns>
        public static int ParseDateToInt(object obj, DateTime defaultVal)
        {
            return ParseDateToInt(ParseHelper.Parse<DateTime>(obj, defaultVal));
        }

        /// <summary>
        /// 将日期对象转换为数字,如:"yyyyMMdd",默认为DEFAULT_DATETIME2INT
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>日期对象转换后的数字</returns>
        public static int ParseDateToInt(object obj)
        {
            return ParseDateToInt(obj, BaseConstants.DEFAULT_DATETIME);
        }

        /// <summary>
        /// 将日期对象转换为数字,如:"yyyyMMdd",当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">日期</param>
        /// <returns>int日期对象转换后的数字</returns>
        public static int ParseDateToIntEx(object obj)
        {
            return ParseDateToInt(ParseHelper.Parse<DateTime>(obj));
        }

        #endregion

        #region ShortDateToInt
        /// <summary>
        /// 将短日期对象转换为数字,如:"yyyyMMdd"
        /// </summary>
        /// <param name="obj">日期</param>
        /// <param name="defaultVal">转换错误的默认值</param>
        /// <returns></returns>
        //private static int ParseShortDateToInt(object obj, DateTime defaultVal)
        //{
        //    return ParseShortDateToInt(ParseFromShortDate(obj, defaultVal));
        //}

        /// <summary>
        /// 将短日期对象转换为数字,默认为DEFAULT_DATETIME2INT
        /// </summary>
        /// <param name="obj">数字类型的日期</param>
        /// <returns></returns>
        //private static int ParseShortDateToInt(object obj)
        //{
        //    return ParseShortDateToInt(obj, BaseTypeConstants.DEFAULT_DATETIME);
        //}

        /// <summary>
        /// 将短日期对象转换为数字,当对象为空或转换错误时丢出InvalidCastException
        /// </summary>
        /// <param name="obj">数字类型的日期</param>
        /// <returns></returns>
        //private static int ParseShortDateToIntEx(object obj)
        //{
        //    return ParseDateToInt(ParseFromShortDateEx(obj));
        //}
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// 转换为日期型
        /// </summary>
        /// <param name="obj">object类型</param>
        /// <param name="val">输出DateTime类型</param>
        /// <returns>是否为短日期格式转换</returns>
        private static bool ParseFromShortDate(object obj, ref DateTime val)
        {
            bool result = true;
            string objStr = obj.ToString();
            int length = objStr.Length;
            if (length >= 8)
                val = Convert.ToDateTime(objStr.Substring(0, 4) + "-" + objStr.Substring(4, 2) + "-" + objStr.Substring(6, 2));
            else if (length >= 6)
                val = Convert.ToDateTime(objStr.Substring(0, 4) + "-" + objStr.Substring(4, 2));
            else if (length >= 4)
                val = Convert.ToDateTime(objStr.Substring(0, 4) + "-01-01");
            else
                result = false;
            return result;
        }

        #endregion
    }
}
