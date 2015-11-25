using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.Collections.Specialized;
using System.Globalization;
//using System.Runtime.Serialization.Json;

namespace GW.Utils
{
    public static class SerializerUtility
    {
        #region Bin

        public static T BinDeserialize<T>(byte[] b)
        {
            if (b == null)
            {
                throw new ArgumentNullException("b");
            }
            T local = default(T);
            using (var stream = new MemoryStream(b))
            {
                stream.Position = 0L;
                var formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }

        public static T BinDeserializeFromFile<T>(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            return BinDeserialize<T>(File.ReadAllBytes(path));
        }

        public static byte[] BinSerialize(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                stream.Position = 0L;
                return stream.ToArray();
            }
        }

        public static void BinSerializeToFile(object o, string path)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            byte[] bytes = BinSerialize(o);
            File.WriteAllBytes(path, bytes);
        }

        #endregion

        #region XML

        #region DataTable

        public static DataTable GetDataTableFromXmlString(string xmlString)
        {
            if (xmlString == null)
            {
                throw new ArgumentNullException("xmlString");
            }
            var table = new DataTable();
            using (var reader = new StringReader(xmlString))
            {
                table.ReadXml(reader);
                return table;
            }
        }

        public static string GetXmlStringFromDataTable(DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }
            using (var writer = new StringWriter())
            {
                table.WriteXml(writer, XmlWriteMode.WriteSchema);
                return writer.ToString();
            }
        }

        #endregion

        public static object XmlDeserialize(string xml, Type objectType)
        {
            object convertedObject = null;

            if (!string.IsNullOrEmpty(xml))
            {
                using (var reader = new StringReader(xml))
                {
                    var ser = new XmlSerializer(objectType);
                    convertedObject = ser.Deserialize(reader);
                    reader.Close();
                }
            }
            return convertedObject;
        }

        public static string XmlSerialize(object objectToConvert)
        {
            string xml = null;

            if (objectToConvert != null)
            {
                Type t = objectToConvert.GetType();

                var ser = new XmlSerializer(t);
                using (var writer = new StringWriter(CultureInfo.InvariantCulture))
                {
                    ser.Serialize(writer, objectToConvert);
                    xml = writer.ToString();
                    writer.Close();
                }
            }

            return xml;
        }


        public static T XmlDeserialize<T>(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(data))
            {
                return (T)serializer.Deserialize(stream);
            }
        }

        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            T local;
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("s");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(encoding.GetBytes(s)))
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    local = (T)serializer.Deserialize(reader);
                }
            }
            return local;
        }

        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            return XmlDeserialize<T>(File.ReadAllText(path, encoding), encoding);
        }


        public static byte[] XmlSerializeToStream(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            var serializer = new XmlSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize((Stream)stream, obj);
                return stream.ToArray();
            }
        }

        public static string XmlSerialize(object o, Encoding encoding)
        {
            byte[] bytes = XmlSerializeInternal(o, encoding);
            return encoding.GetString(bytes);
        }

        private static byte[] XmlSerializeInternal(object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            var serializer = new XmlSerializer(o.GetType());
            using (var stream = new MemoryStream())
            {
                using (var writer = new XmlTextWriter(stream, encoding))
                {
                    writer.Formatting = Formatting.Indented;
                    serializer.Serialize((XmlWriter)writer, o);
                    writer.Close();
                }
                return stream.ToArray();
            }
        }

        public static string XmlSerializeObject(object o, Encoding encoding)
        {
            if (o == null)
            {
                throw new ArgumentNullException("o");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            var serializer = new XmlSerializer(o.GetType());
            using (var stream = new MemoryStream())
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineChars = "\r\n";
                settings.Encoding = encoding;
                settings.OmitXmlDeclaration = true;
                settings.IndentChars = "\t";
                var namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                XmlWriter xmlWriter = XmlWriter.Create(stream, settings);
                serializer.Serialize(xmlWriter, o, namespaces);
                xmlWriter.Close();
                return encoding.GetString(stream.ToArray());
            }
        }

        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException("path");
            }
            byte[] bytes = XmlSerializeInternal(o, encoding);
            File.WriteAllBytes(path, bytes);
        }

        #region NameValueCollection

        public static void ConvertFromNameValueCollection(NameValueCollection nvc
            , ref string keys, ref string values)
        {
            if (nvc == null || nvc.Count == 0)
                return;

            var sbKey = new StringBuilder();
            var sbValue = new StringBuilder();

            int index = 0;
            foreach (string key in nvc.AllKeys)
            {
                if (key.IndexOf(':') != -1)
                    throw new ArgumentException("ExtendedAttributes Key can not contain the character \":\"");

                string v = nvc[key];
                if (!string.IsNullOrEmpty(v))
                {
                    sbKey.AppendFormat("{0}:S:{1}:{2}:", key, index, v.Length);
                    sbValue.Append(v);
                    index += v.Length;
                }
            }
            keys = sbKey.ToString();
            values = sbValue.ToString();
        }

        public static NameValueCollection ConvertToNameValueCollection(string keys, string values)
        {
            var nvc = new NameValueCollection();

            if (keys != null && values != null && keys.Length > 0 && values.Length > 0)
            {
                char[] splitter = new char[1] { ':' };
                string[] keyNames = keys.Split(splitter);

                for (int i = 0; i < (keyNames.Length / 4); i++)
                {
                    int start = int.Parse(keyNames[(i * 4) + 2], CultureInfo.InvariantCulture);
                    int len = int.Parse(keyNames[(i * 4) + 3], CultureInfo.InvariantCulture);
                    string key = keyNames[i * 4];

                    //Future version will support more complex types	
                    if (((keyNames[(i * 4) + 1] == "S") && (start >= 0)) && (len > 0) && (values.Length >= (start + len)))
                    {
                        nvc[key] = values.Substring(start, len);
                    }
                }
            }

            return nvc;
        }

        #endregion

        #endregion

        #region JSON

        //public static string JsonSerialize<T>(T jsonObject)
        //{
        //    var serializer = new DataContractJsonSerializer(typeof(T));
        //    var ms = new MemoryStream();
        //    serializer.WriteObject(ms, jsonObject);
        //    string json = Encoding.UTF8.GetString(ms.GetBuffer());
        //    ms.Close();
        //    ms.Dispose();
        //    ms = null;
        //    return json;
        //}

        //public static T JsonDeserialize<T>(string json)
        //{
        //    var serializer = new DataContractJsonSerializer(typeof(T));
        //    var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
        //    T jto = (T)serializer.ReadObject(ms);
        //    ms.Close();
        //    ms.Dispose();
        //    ms = null;
        //    return jto;
        //}

        //public static string JsonSerialize(object jsonObject)
        //{
        //    var serializer = new DataContractJsonSerializer(jsonObject.GetType());
        //    var ms = new MemoryStream();
        //    serializer.WriteObject(ms, jsonObject);
        //    string json = Encoding.UTF8.GetString(ms.GetBuffer());
        //    ms.Close();
        //    ms.Dispose();
        //    ms = null;
        //    return json;
        //}

        public static string JsonSerializeByJNet(object jsonObject)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject);
        }

        public static string JsonSerializeByJNetForDate(object jsonObject)
        {
            return JsonSerializeByJNet(jsonObject, "yyyy-MM-dd");
        }

        public static string JsonSerializeByJNet(object jsonObject, string dateFormate)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = dateFormate;
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject, Newtonsoft.Json.Formatting.Indented, timeConverter);
        }

        public static T JsonDeserializeByJNet<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static T JsonDeserializeByJNet<T>(string json, string dateFormate)
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter timeConverter = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            timeConverter.DateTimeFormat = dateFormate;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, timeConverter);
        }

        #endregion

        #region ProtoBuf-net

        public static byte[] ProtoBufSerialize(object protoObject)
        {
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, protoObject);
                return stream.ToArray();
            }
        }

        public static T ProtoBufDeserialize<T>(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        public static string ProtoBufBase64Serialize<T>(T protoObject)
        {
            using (var stream = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(stream, protoObject);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        public static T ProtoBufBase64Deserialize<T>(string buffer)
        {
            using (var stream = new MemoryStream(Convert.FromBase64String(buffer)))
            {
                return ProtoBuf.Serializer.Deserialize<T>(stream);
            }
        }

        #endregion
    }
}
