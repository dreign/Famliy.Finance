//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   XML文件操作类
//编写日期    :   2010-11-17
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Data;
using System.Xml.Serialization;
using System.Xml.Xsl;

using GW.Utils.Web;

namespace GW.Utils
{
    /// <summary>
    /// XML处理类
    /// </summary>
    public sealed class XmlHelper
    {
        #region 成员变量

        /*
         XML文件中需要待处理的特殊字符
          *小于(<) --> &lt;
           大于(>) --> &gt;
          *和号(&) --> &amp;
         单引号(') --> &apos;
           引号(") --> &quot;
         */

        /// <summary>
        /// xml文件名
        /// </summary>
        private string _xmlFileName;

        /// <summary>
        /// XmlDocument
        /// </summary>
        private XmlDocument _objXmlDoc = new XmlDocument();

        #endregion

        #region 构造函数

        /// <summary>
        /// 类构造函数
        /// </summary>
        /// <param name="xmlFileName">XML文件路径及文件名</param>
        public XmlHelper(string xmlFileName)
        {
            try
            {
                _objXmlDoc.Load(xmlFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _xmlFileName = xmlFileName;
        }

        #endregion

        #region XML文件操作

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <param name="xmlPathNode">目标节点</param>
        /// <returns>以表格形式返回节点下的内容</returns>
        public DataTable GetNodeData(string xmlPathNode)
        {
            DataTable dtResult = null;

            XmlNode xmlNode = _objXmlDoc.SelectSingleNode(xmlPathNode);
            if (xmlNode != null)
            {
                DataSet ds = new DataSet();
                StringReader read = new StringReader(xmlNode.OuterXml);
                ds.ReadXml(read);

                if (ds != null && ds.Tables.Count > 0)
                {
                    dtResult = ds.Tables[0];
                }
            }
            return dtResult;
        }

        /// <summary>
        /// 根据节点，取属性值
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名称</param>
        /// <returns>xml节点的属性值，如果不存在该节点和属性，则返回空</returns>
        public string GetNodeAttribute(string node, string attribute)
        {
            if (string.IsNullOrEmpty(node))
                return string.Empty;

            XmlNode xmlNode = _objXmlDoc.SelectSingleNode(node);
            if (xmlNode != null && xmlNode.Attributes[attribute] != null)
            {
                return FilterInvalidContent(xmlNode.Attributes[attribute].Value);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据xml的节点名返回节点
        /// </summary>
        /// <param name="xmlNodeName">节点名</param>
        /// <returns>xml的节点</returns>
        public XmlNode GetXmlNode(string xmlNodeName)
        {
            XmlNode xmlNode = _objXmlDoc.SelectSingleNode(xmlNodeName);
            return xmlNode;
        }

        /// <summary>
        /// 更新节点内容
        /// </summary>
        /// <param name="xmlPathNode">目标节点</param>
        /// <param name="content">需要更新的内容</param>
        public void Replace(string xmlPathNode, string content)
        {
            XmlNode xmlNode = _objXmlDoc.SelectSingleNode(xmlPathNode);

            if (xmlNode != null)
            {
                xmlNode.InnerText = content;
            }
        }

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="xmlNodeName">目标节点</param>
        public void Delete(string xmlNodeName)
        {
            int parentIndex = xmlNodeName.LastIndexOf("/");

            if (parentIndex > 0)
            {
                string mainNode = xmlNodeName.Substring(0, xmlNodeName.LastIndexOf("/"));
                XmlNode xmlNode = _objXmlDoc.SelectSingleNode(xmlNodeName);

                if (xmlNode != null)
                {
                    _objXmlDoc.SelectSingleNode(mainNode).RemoveChild(_objXmlDoc.SelectSingleNode(xmlNodeName));
                }
            }
        }

        /// <summary>
        /// 插入一节点和此节点的一子节点。
        /// </summary>
        /// <param name="mainNode">目标节点</param>
        /// <param name="childNode">需要插入的子节点</param>
        /// <param name="element">需要插入的子节点的属性</param>
        /// <param name="content">需要插入的子节点的属性值</param>
        public void InsertNode(string mainNode, string childNode, string element, string content)
        {
            XmlNode objRootNode = _objXmlDoc.SelectSingleNode(mainNode);
            XmlElement objChildNode = _objXmlDoc.CreateElement(childNode);
            objRootNode.AppendChild(objChildNode);

            XmlElement objElement = _objXmlDoc.CreateElement(element);

            objElement.InnerText = content;
            objChildNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个带属性的节点
        /// </summary>
        /// <param name="mainNode">目标节点</param>
        /// <param name="element">需插入的元素</param>
        /// <param name="attribName">属性名</param>
        /// <param name="attribContent">属性内容</param>
        /// <param name="content">元素内容</param>
        public void InsertElement(string mainNode, string element, string attribName, string attribContent, string content)
        {
            XmlNode objNode = _objXmlDoc.SelectSingleNode(mainNode);
            XmlElement objElement = _objXmlDoc.CreateElement(element);

            objElement.SetAttribute(attribName, attribContent);
            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }

        /// <summary>
        /// 插入一个节点，不带属性
        /// </summary>
        /// <param name="mainNode">目标节点</param>
        /// <param name="element">需插入的元素</param>
        /// <param name="content">内容</param>
        public void InsertElement(string mainNode, string element, string content)
        {
            XmlNode objNode = _objXmlDoc.SelectSingleNode(mainNode);
            XmlElement objElement = _objXmlDoc.CreateElement(element);

            objElement.InnerText = content;
            objNode.AppendChild(objElement);
        }

        /// <summary>
        /// 保存XML文档
        /// </summary>
        public void Save()
        {
            try
            {
                _objXmlDoc.Save(_xmlFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _objXmlDoc = null;
        }

        /// <summary>
        /// 过滤无效字符
        /// </summary>
        /// <param name="replaceString">需要替换的内容</param>
        /// <returns>过滤无效字符后的值</returns>
        public string FilterInvalidContent(string replaceString)
        {
            return replaceString.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty).Trim();
        }

        #region 读取xml文件并返回DataSet

        /// <summary>
        /// 将xml文件流格式化成DataTable
        /// </summary>
        /// <param name="stream">xml文件</param>
        /// <returns>DataTable格式数据</returns>
        public static DataTable GetDataTable(Stream stream)
        {
            DataTable dataTable = new DataTable();

            dataTable.ReadXml(stream);

            return dataTable;
        }

        /// <summary>
        /// 将某个文件路径下的xml文件格式化成DataTable
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>DataTable格式数据</returns>
        public static DataTable GetDataTable(string fileName)
        {
            DataTable dataTable = new DataTable();

            dataTable.ReadXml(fileName);

            return dataTable;
        }

        /// <summary>
        /// 将textReader对象格式化成DataTable
        /// </summary>
        /// <param name="textReader">TextReader对象</param>
        /// <returns>DataTable格式数据</returns>
        public static DataTable GetDataTable(TextReader textReader)
        {
            DataTable dataTable = new DataTable();

            dataTable.ReadXml(textReader);

            return dataTable;
        }

        /// <summary>
        /// 将xmlReader对象格式化成DataTable
        /// </summary>
        /// <param name="xmlReader">XmlReader对象</param>
        /// <returns>DataTable格式数据</returns>
        public static DataTable GetDataTable(XmlReader xmlReader)
        {
            DataTable dataTable = new DataTable();

            dataTable.ReadXml(xmlReader);

            return dataTable;
        }

        /// <summary>
        /// 将xml文件流格式化成DataSet
        /// </summary>
        /// <param name="stream">xml文件流</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(Stream stream)
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXml(stream);

            return dataSet;
        }

        /// <summary>
        /// 将xml文件流按照一定的模式格式化成DataSet
        /// </summary>
        /// <param name="stream">xml文件流</param>
        /// <param name="xmlReadMode">XmlReadMode</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(Stream stream, XmlReadMode xmlReadMode)
        {
            DataSet dataSet = new DataSet();
            DataTable DT = new DataTable();
            dataSet.ReadXml(stream, xmlReadMode);

            return dataSet;
        }

        /// <summary>
        /// 将xml文件格式化成DataSet
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(string fileName)
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXml(fileName);

            return dataSet;
        }

        /// <summary>
        /// 将xml文件按照一定的模式格式化成DataSet
        /// </summary>
        /// <param name="fileName">xml文件名</param>
        /// <param name="xmlReadMode">XmlReadMode</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(string fileName, XmlReadMode xmlReadMode)
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXml(fileName, xmlReadMode);

            return dataSet;
        }

        /// <summary>
        /// 将textReader对象格式化成DataSet
        /// </summary>
        /// <param name="textReader">TextReader对象</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(TextReader textReader)
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXml(textReader);

            return dataSet;
        }

        /// <summary>
        /// 将XmlReader对象格式化成DataSet
        /// </summary>
        /// <param name="xmlReader">XmlReader对象</param>
        /// <returns>DataSet格式数据</returns>
        public static DataSet GetDataSet(XmlReader xmlReader)
        {
            DataSet dataSet = new DataSet();

            dataSet.ReadXml(xmlReader);

            return dataSet;
        }

        #endregion

        #endregion

        #region 序列化操作

        /// <summary>
        /// 序列化对象到内存流
        /// </summary>
        /// <param name="obj">实例化的对象</param>
        /// <returns>对象的内存流</returns>
        public static MemoryStream Serializer(object obj)
        {
            if (obj == null) throw new Exception("Parameter is null!");

            XmlTextWriter writer = null;
            try
            {
                Type t = obj.GetType();
                XmlSerializer ser = new XmlSerializer(t);
                MemoryStream ms = new MemoryStream();
                writer = new XmlTextWriter(ms, Encoding.Default);
                ser.Serialize(writer, obj);

                return ms;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                writer.Close();
            }
        }

        /// <summary>
        /// 将内存流反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="memoryStream">内存流</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(MemoryStream memoryStream)
        {
            string contents = Encoding.Default.GetString(memoryStream.ToArray());

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamReader sr = new StreamReader(new MemoryStream(Encoding.Default.GetBytes(contents)), Encoding.Default);

            T myObject = (T)mySerializer.Deserialize(sr);
            sr.Close();
            sr.Dispose();
            return myObject;
        }

        /// <summary>
        /// 序列化对象并将内容输出xml文件
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="fileName">序列化后的xml文件路径文件名，形如：C:\test.xml</param>
        /// <param name="obj">需要序列化的对象</param>
        public static void Serializer<T>(string fileName, T obj)
        {
            if (obj == null) throw new Exception("Parameter is null!");

            Stream stream = null;
            try
            {
                stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(stream, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// 将xml文件的内容反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="fileName">xml文件路径文件名，形如：C:\test.xml</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string fileName)
        {
            T obj;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    obj = (T)xs.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write("GW.Utils.XmlHelper error.", ex);
                throw ex;
            }
            return obj;
        }

        /// <summary>
        /// 序列化对象成字符串
        /// </summary>
        /// <param name="obj">实例化的对象</param>
        /// <returns>字符串格式的对象</returns>
        public static string SerializerToString(object obj)
        {
            if (obj == null) throw new Exception("Parameter is null!");

            using (StringWriter sw = new StringWriter())
            {
                XmlSerializer xz = new XmlSerializer(obj.GetType());
                xz.Serialize(sw, obj);
                return sw.ToString();
            }
        }

        /// <summary>
        /// 将字符串反序列化成对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="contents">对象序列化后的Xml字符串</param>
        /// <returns>对象值</returns>
        public static T DeserializeFromString<T>(string contents)
        {
            using (StringReader sr = new StringReader(contents))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(T));

                T myObject = (T)mySerializer.Deserialize(sr);

                return myObject;
            }
        }

        #endregion

        #region XSLT转换
        /// <summary>
        /// XSLT转换
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <param name="xsltFilePath">xslt路径</param>
        /// <returns>转换后的值</returns>
        public static string ConvertXSLT(string xmlFilePath, string xsltFilePath)
        {
            return ConvertXSLT(xmlFilePath, xsltFilePath, false);
        }
        /// <summary>
        /// XSLT转换
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <param name="xsltFilePath">xslt路径</param>
        /// <param name="isCacheXSLT">是否启用缓存,黙认不启用</param>
        /// <returns>转换后的值，文件不存在则抛出异常。</returns>
        public static string ConvertXSLT(string xmlFilePath, string xsltFilePath, bool isCacheXSLT)
        {
            if (FileHelper.IsFileExist(xmlFilePath) && FileHelper.IsFileExist(xsltFilePath))
            {
                StringWriter output = new StringWriter();
                XslCompiledTransform transform = new XslCompiledTransform();

                if (isCacheXSLT)
                {
                    FileInfo xsltFilePathInfo = null;
                    if (xsltFilePathInfo == null)
                    {
                        transform.Load(xsltFilePath);
                        CacheHelper.SetCache("XSLTCache", FileHelper.GetFileInfo(xsltFilePath), DateTime.Now.AddMinutes(60));
                    }
                    else
                    {
                        xsltFilePathInfo = (FileInfo)CacheHelper.GetCache("XSLTCache");
                    }
                }
                else
                {
                    transform.Load(xsltFilePath);
                    transform.Transform(xmlFilePath, null, output);
                }
                transform.Transform(xmlFilePath, null, output);

                return output.ToString();
            }
            else
            {
                throw new Exception("传入的xslt或者xml文件不存在！");
            }
        }

        #endregion
    }
}
