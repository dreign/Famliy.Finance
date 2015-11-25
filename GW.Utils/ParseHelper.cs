//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   通用类型转换，提供类型转换处理方法
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
using System.Collections;

namespace GW.Utils
{
    /// <summary>
    /// 通用类型转换，提供类型转换处理方法
    /// </summary>
    public sealed class ParseHelper
    {
        #region 常量

        //转换出错信息
        private const string CAST_ERROR_MSG = "ParserHelper.{0} Error.object:{1},type:{2}.";
        //不能为空
        private const string NULL_ERROR_MSG = "ParserHelper.{0} Error.Object is null or dbnull.";

        #endregion

        #region 公共静态方法

        /// <summary>
        /// 对Object进行指定类型的转换,当对象为空或转换错误时返回指定默认值
        /// 注意：当Obj为字符类型但内容包含小数且T为整型时，转换错误。如："20.22" 转换为Int错误;而20.22转换为Int=20
        /// 当Obj为数字且以科学计数法表示时转换为String会变为科学计数法，且精度丢失，所以建议用Decimal
        /// </summary>
        /// <typeparam name="T">支持类型：double,int,long,ushort,decimal,float,short,string,datetime</typeparam>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>T型的数据</returns>
        public static T Parse<T>(object obj, T defaultVal)
        {
            if (obj != null && obj != DBNull.Value)
            {
                Type type = typeof(T);
                try
                {
                    return (T)Convert.ChangeType(obj, type);
                }
                catch { }
            }
            return defaultVal;
        }

        /// <summary>
        /// 对Object进行指定类型的转换,当对象为空或转换错误时返回默认值
        /// 注意：当Obj为字符类型但内容包含小数且T为整型时，转换错误。如："20.22" 转换为Int错误;而20.22转换为Int=20
        /// 当Obj为数字且以科学计数法表示时转换为String会变为科学计数法，且精度丢失，所以建议用Decimal
        /// </summary>
        /// <typeparam name="T">支持类型：double,int,long,ushort,decimal,float,short,string,datetime</typeparam>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>T型的数据</returns>
        public static T Parse<T>(object obj)
        {
            return Parse<T>(obj, GetDefaultVal<T>());
        }

        /// <summary>
        /// 对object进行指定类型的转换
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="covertToType">转换的类型</param>
        /// <returns>object</returns>
        public static object Parse(object obj, Type covertToType)
        {
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    return Convert.ChangeType(obj, covertToType);
                }
                catch { }
            }
            return null;
        }

        /// <summary>
        /// 对Object进行指定类型的转换,并判断转换结果是否为空，当对象为空或转换错误或与指定的NullVal相等时,返回true,Val为类型默认值
        /// </summary>
        /// <typeparam name="T">支持类型：double,int,long,ushort,decimal,float,short,string,datetime</typeparam>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="nullVal">空值标示</param>
        /// <param name="outValue">输出值</param>
        /// <returns>对象是否为空，true为空，false为非空</returns>
        public static bool ParseIsNull<T>(object obj, T nullVal, ref T outValue)
        {
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    outValue = (T)Convert.ChangeType(obj, typeof(T));
                    if (!outValue.Equals(nullVal))
                        return false;
                }
                catch { }
            }
            return true;
        }

        /// <summary>
        /// 枚举数据类型转换,可设置是否区分大小写,当对象为空或转换错误时返回指定默认值
        /// </summary>
        /// <typeparam name="T">支持枚举类型</typeparam>
        /// <param name="obj">要转换的对象</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>T型值</returns>
        public static T ParseEnum<T>(object obj, bool ignoreCase, T defaultVal)
        {
            T val = defaultVal;
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    ParseEnum<T>(obj, ignoreCase, ref val);
                }
                catch { }
            }
            return val;
        }

        /// <summary>
        /// 日期换化为字符串，失败返回0
        /// </summary>
        /// <typeparam name="DateTime">DateTime类型</typeparam>
        /// <param name="date">日期参数</param>
        /// <returns>将日期字符串，转换失败后返回0</returns>
        public static string PareDateTimeToStr<DateTime>(DateTime date)
        {
            string result = "0";
            if (date != null)
            {
                result = Parse<string>(date, result);
            }

            return result;
        }

        /// <summary>
        /// 日期换化为字符串
        /// </summary>
        /// <param name="obj">要转化的日期</param>
        /// <param name="format">日期格式:d,D,F,f,G,g,M,m,o,R,r,s,T,t,U,u,y,Y</param>
        /// <returns>特定日期格式的日期字符串</returns>
        public static string PareDateTimeToStr(DateTime obj, string format)
        {
            return Parse<DateTime>(obj).ToString(format);
        }

        /// <summary>
        /// 字符串转化日期类型
        /// </summary>
        /// <typeparam name="String">string类型参数</typeparam>
        /// <param name="value">要转化的值</param>
        /// <returns>日期类型的值</returns>
        public static DateTime PareStrToDateTime<String>(string value)
        {
            DateTime result = new DateTime();
            if (!string.IsNullOrEmpty(value))
            {
                result = Parse<DateTime>(value, result);
            }
            return result;
        }

        /// <summary>
        /// 将特定格式的字符串转换成日期型数据
        /// </summary>
        /// <typeparam name="String">String类型参数</typeparam>
        /// <param name="value">要转化的值</param>
        /// <param name="format">日期格式</param>
        /// <returns>用特定格式的字符串转换成的日期型数据</returns>
        public static DateTime PareStrToDateTime<String>(string value, string format)
        {
            DateTime result = new DateTime();
            if (!string.IsNullOrEmpty(value))
            {
                result = DateTime.ParseExact(value, format, null);
            }
            return result;
        }

        /// <summary>
        /// 枚举数据类型转换,不区分大小写,当对象为空或转换错误时返回指定默认值
        /// </summary>
        /// <typeparam name="T">支持枚举类型</typeparam>
        /// <param name="obj">要转化的对象</param>
        /// <param name="defaultVal">默认值</param>
        /// <returns>T型数据</returns>
        public static T ParseEnum<T>(object obj, T defaultVal)
        {
            return ParseEnum<T>(obj, true, defaultVal);
        }

        /// <summary>
        /// 根据基本类型，返回系统类型
        /// </summary>
        /// <param name="baseType">基本类型，如：int，string</param>
        /// <returns>System.String、System.Boolean等系统格式的类型</returns>
        public static string GetBaseDataType(string baseType)
        {
            string systemType = string.Empty;

            switch (baseType.ToLower())
            {
                case "short":
                    systemType = "System.UInt16";
                    break;
                case "int":
                    systemType = "System.Int32";
                    break;
                case "string":
                    systemType = "System.String";
                    break;
                case "float":
                    systemType = "System.Single";
                    break;
                case "double":
                    systemType = "System.Double";
                    break;
                case "decimal":
                    systemType = "System.Decimal";
                    break;
                case "long":
                    systemType = "System.Int64";
                    break;
                case "bool":
                    systemType = "System.Boolean";
                    break;
                case "byte":
                    systemType = "System.Byte";
                    break;
                case "char":
                    systemType = "System.Char";
                    break;
            }

            return systemType;
        }

        /// <summary>
        /// 将数据库的表字段类型转换成.Net中的数据类型
        /// </summary>
        /// <param name="sqlBaseType">数据库的表字段类型</param>
        /// <returns>.Net中的数据类型</returns>
        public static string ParseSqlType(string sqlBaseType)
        {
            switch (sqlBaseType.ToLower())
            {
                case "bigint": return "int64";
                case "binary": return "byte[]";
                case "bit": return "bool";
                case "char": return "string";
                case "date": return "string";
                case "datetime": return "DateTime";
                case "datetime2": return "string";
                case "datetimeoffset": return "string";
                case "decimal": return "decimal";
                case "float": return "double";
                case "geography": return "byte[]";
                case "geometry": return "byte[]";
                case "hierarchyid": return "Byte[]";
                case "image": return "byte[]";
                case "int": return "int";
                case "money": return "decimal";
                case "nchar": return "string";
                case "ntext": return "string";
                case "numeric": return "decimal";
                case "nvarchar": return "string";
                case "real": return "single";
                case "smalldatetime": return "DateTime";
                case "smallint": return "int16";
                case "smallmoney": return "decimal";
                case "sql_variant": return "object";
                case "text": return "string";
                case "time": return "string";
                case "timestamp": return "byte[]";
                case "tinyint": return "byte";
                case "uniqueidentifier": return "Guid";
                case "varbinary": return "byte[]";
                case "varchar": return "string";
                case "xml": return "string";
                default: return "object";
            }
        }

        #endregion

        #region 私有静态方法

        /// <summary>
        /// 获取默认值
        /// </summary>        
        /// <returns>T型数据的默认值</returns>
        private static T GetDefaultVal<T>()
        {
            Type type = typeof(T);
            if (IsNumType(type))
                return Parse<T>(BaseConstants.DEFAULT_NUM, type);
            if (IsStringType(type))
                return Parse<T>(BaseConstants.DEFAULT_STRING, type);
            if (IsDateTimeType(type))
                return Parse<T>(BaseConstants.DEFAULT_DATETIME, type);
            if (IsBoolType(type))
                return Parse<T>(BaseConstants.DEFAULT_BOOL, type);

            throw new InvalidCastException("Parser get default value error.Type=" + type.Name + ".");
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>是否为数据类型的值，true为是，false为不是</returns>
        private static bool IsNumType(Type type)
        {
            return type.Equals(typeof(double)) || type.Equals(typeof(int)) || type.Equals(typeof(long)) || type.Equals(typeof(ushort)) || type.Equals(typeof(decimal)) || type.Equals(typeof(float)) || type.Equals(typeof(short)) || type.Equals(typeof(byte));
        }

        /// <summary>
        /// 测试是否是string类型
        /// </summary>
        /// <param name="type">传入的type</param>
        /// <returns>true标示为string类型，否则不是</returns>
        private static bool IsStringType(Type type)
        {
            return type.Equals(typeof(string));
        }

        /// <summary>
        /// 测试是否是DateTime类型
        /// </summary>
        /// <param name="type">传入的type</param>
        /// <returns>true标示为datetime类型，否则不是</returns>
        private static bool IsDateTimeType(Type type)
        {
            return type.Equals(typeof(DateTime));
        }

        /// <summary>
        /// 测试是否是Bool类型
        /// </summary>
        /// <param name="type">传入的type</param>
        /// <returns>true标示为bool类型，否则不是</returns>
        private static bool IsBoolType(Type type)
        {
            return type.Equals(typeof(bool));
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="obj">要转换的对象</param>
        /// <param name="type">类型</param>
        /// <returns>T型数据</returns>
        private static T Parse<T>(object obj, Type type)
        {
            return (T)Convert.ChangeType(obj, type);
        }

        /// <summary>
        /// 枚举类型转换
        /// </summary>
        /// <typeparam name="T">要转换的枚举类型</typeparam>
        /// <param name="obj">要转换的枚举对象</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <param name="val">输出值</param>
        /// <returns>true为转换成功，否则不成功</returns>
        private static bool ParseEnum<T>(object obj, bool ignoreCase, ref T val)
        {
            bool result = false;
            Type type = typeof(T);
            object tempVal = Enum.Parse(type, obj.ToString(), ignoreCase);
            if (Enum.IsDefined(type, tempVal))
            {
                val = (T)tempVal;
                result = true;
            }
            return result;
        }

        #endregion
    }
}
