//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   用于基础类型的常量定义
//编写日期    :   2010-11-12
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace GW.Utils
{
    /// <summary>
    /// 基础类型的常量定义
    /// </summary>
    public static class BaseConstants
    {
        #region 系统常量

        /// <summary>
        /// 数值类型默认值
        /// </summary>
        public const int DEFAULT_NUM = 0;

        /// <summary>
        /// 字符类型默认值
        /// </summary>
        public static string DEFAULT_STRING = string.Empty;

        /// <summary>
        /// Bool类型默认值
        /// </summary>
        public const bool DEFAULT_BOOL = false;

        /// <summary>
        /// 日期类型默认值
        /// </summary>
        public static DateTime DEFAULT_DATETIME = new DateTime(1000, 1, 1);

        /// <summary>
        /// 日期类型Int默认值
        /// </summary>
        public const int DEFAULT_DATETIME2INT = 10000101;

        /// <summary>
        /// 月份类型Int默认值(100001)
        /// </summary>
        public const int DEFAULT_MONTHINT = 100001;

        /// <summary>
        /// 8位日期类型的默认格式(yyyyMMdd)
        /// </summary>
        public const string DEFAULT_SHORTDATETIME_FORMAT = "yyyyMMdd";
        public const string DEFAULT_LONGDATETIME_FORMAT = "yyyy-MM-dd hh:mm:ss.fff";
        public const string DEFAULT_FULLDATETIME_FORMAT = "yyyyMMddhhmmssfff";

        /// <summary>
        /// 默认日期字符串格式(yyyy-MM-dd)
        /// </summary>
        public const string DEFAULT_DATE_FORMAT = "yyyy-MM-dd";

        /// <summary>
        /// 默认月份字符串格式(yyyy-MM)
        /// </summary>
        public const string DEFAULT_MONTH_FORMAT = "yyyy-MM";

        /// <summary>
        /// 默认短月份字符串格式(yyyyMM)
        /// </summary>
        public const string DEFAULT_SHORTMONTH_FORMAT = "yyyyMM";
        
        /// <summary>
        /// 空Json对象的默认值，一般用于绑定Jcore的Datalist等数据展示控件
        /// </summary>
        public const string DEFAULT_EMPTYJOSNRESULT = "[]";

        #endregion
    }
}
