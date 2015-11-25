//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   JSON对象操作工具
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Data;
using Newtonsoft.Json;

namespace GW.Utils
{
    /// <summary>
    /// JSON对象操作工具
    /// </summary>
    public sealed class JsonHelper
    {
        #region 私有成员

        private const string JSON_FORMAT = "\"{0}\":{1},";

        #endregion

        #region 公共方法

        /// <summary>
        /// 将对象转换成JSON格式数据，不支持值类型直接传入
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>json格式的对象</returns>
        public static string ToJson(object obj)
        {
            if (obj == null)
                return "null";
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //return JavaScriptConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 对象根据属性列表转化Json,不支持值类型直接传入
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <param name="propertyNames">属性名</param>
        /// <returns>json格式的对象</returns>
        public static string ToJson(object obj, string[] propertyNames)
        {
            if (obj == null)
                return "null";
            if (propertyNames.Length == 0)
                return ToJson(obj);
            Type objType = obj.GetType();
            PropertyInfo info;
            StringBuilder sb = new StringBuilder();
            sb.Append("[{");
            foreach (string propertyName in propertyNames)
            {
                info = objType.GetProperty(propertyName);
                if (info != null)
                    sb.Append(string.Format(JSON_FORMAT, info.Name, GetValue(obj, info)));
            }
            if (sb.Length > 1)
                sb.Remove(sb.Length - 1, 1);
            sb.Append("}]");
            return sb.ToString();
        }

        /// <summary>  
        /// DataTable转换成Json格式  
        /// </summary>  
        /// <param name="dtSource">数据源</param>  
        /// <returns>json格式的数据表</returns>  
        public static string ToJson(DataTable dtSource)
        {
            if (dtSource == null || dtSource.Rows.Count == 0) return string.Empty;

            StringBuilder jsonBuilder = new StringBuilder();

            for (int rowIndex = 0; rowIndex < dtSource.Rows.Count; rowIndex++)
            {
                if (rowIndex == 0)
                {
                    jsonBuilder.Append("[");
                }

                jsonBuilder.Append("{");

                for (int columnsIndex = 0; columnsIndex < dtSource.Columns.Count; columnsIndex++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dtSource.Columns[columnsIndex].ColumnName);
                    string itemContent = dtSource.Rows[rowIndex][columnsIndex].ToString().Replace("\"", "\\\"").Replace("\r\n", "\\r\\n");

                    if (dtSource.Columns[columnsIndex].DataType == Type.GetType("System.Int32"))
                    {
                        jsonBuilder.Append("\":");
                        jsonBuilder.Append(itemContent);
                        jsonBuilder.Append(",");
                    }
                    else
                    {
                        jsonBuilder.Append("\":\"");
                        jsonBuilder.Append(itemContent);
                        jsonBuilder.Append("\",");
                    }
                }

                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");

            }

            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]");

            return jsonBuilder.ToString();

        }

        /// <summary>
        /// 将JSON字符串反序列化成对象
        /// </summary>
        /// <param name="value">要反序列化的JSON字符串</param>
        /// <returns>对象</returns>
        public static object FromJson(string value)
        {

            // return JavaScriptConvert.DeserializeObject(value);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value);
        }

        /// <summary>
        /// 将JSON字符串反序列化成对象
        /// </summary>
        /// <param name="value">要反序列化的JSON字符串</param>
        /// <param name="type">类型</param>
        /// <returns>传入类型的对象</returns>
        public static object FromJson(string value, Type type)
        {
            // return JavaScriptConvert.DeserializeObject(value, type);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value, type);

        }

        /// <summary>
        /// 将JSON字符串反序列化成对象,注意：对象必须有无参数的构造函数，否则会丢出异常
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonString">要反序列化的JSON字符串</param>
        /// <returns>反序列化的结果对象</returns>
        public static T FromJson<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                return default(T);

            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取对象的属性值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="propertyInfo">属性信息</param>
        /// <returns>属性值</returns>
        private static string GetValue(object obj, PropertyInfo propertyInfo)
        {
            object value = propertyInfo.GetValue(obj, null);
            StringWriter sw = new StringWriter(CultureInfo.InvariantCulture);
            JsonWriter jw = new JsonTextWriter(sw);
            JsonSerializer serializer = new JsonSerializer();
            if (value is IConvertible)
                sw.Write(Newtonsoft.Json.JsonConvert.ToString(value));
            else
                serializer.Serialize(jw, value);
            return sw.ToString();
        }
        #endregion
    }

}
