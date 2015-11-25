//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   系统配置处理类
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace GW.Utils
{
    /// <summary>
    /// 配置节读取类
    /// </summary>
    public class ConfigManager
    {
        /// <summary>
        /// 根据配置节名返回配置节内容
        /// </summary>
        /// <param name="sectionName">配置节名</param>
        /// <returns>某个名字的配置节内容</returns>
        public static NameValueCollection GetSection(string sectionName)
        {
            NameValueCollection cfg = ConfigurationManager.GetSection(sectionName) as NameValueCollection;

            if (cfg == null)
            {
                throw new Exception(string.Format("请检查配置是否包含{0}！", sectionName));
            }

            return cfg;
        }

        /// <summary>
        /// 返回AppSetting配置节值
        /// </summary>
        /// <param name="appSettingName">AppSetting名称</param>
        /// <returns>根据节点的key值获取节点的值</returns>
        public static string GetAppSettingValue(string appSettingName)
        {
            return ConfigurationManager.AppSettings[appSettingName];
        }

        /// <summary>
        /// 返回AppSetting配置节值
        /// </summary>
        /// <param name="appSettingName">配置节的Key值</param>
        /// <param name="defaultValue">配置节的默认值</param>
        /// <returns>根据节点的key值获取节点的值，为空则返回传入的默认值</returns>
        public static string GetAppSettingValue(string appSettingName, string defaultValue)
        {
            string value = ConfigurationManager.AppSettings[appSettingName];

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        }
        /// <summary>
        /// 返回ConnectionString
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ToString();
        }
        /// <summary>
        /// 返回ConnectionString
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetConnectionString(string key, string defaultValue)
        {
            string value = ConfigurationManager.ConnectionStrings[key].ToString(); 

            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return defaultValue;
            }
        } 
    }

    /// <summary>
    /// 配置节元素集合类
    /// </summary>
    public class ConfigCollection : ConfigurationElementCollection
    {
        #region Public Properties
        /// <summary>
        /// 集合类型
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }
        /// <summary>
        /// 根据索引获取元素
        /// </summary>
        /// <param name="index">集合索引</param>
        /// <returns>元素</returns>
        public ConfigElement this[int index]
        {
            get
            {
                return (ConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        /// <summary>
        /// 元素名称
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "item";
            }
        }

        /// <summary>
        /// 根据Key获取元素
        /// </summary>
        /// <param name="key">健名称</param>
        /// <returns>元素</returns>
        public new ConfigElement this[string key]
        {
            get { return (ConfigElement)BaseGet(key.ToUpper()); }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 元素索引
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>元素位置</returns>
        public int IndexOf(ConfigElement element)
        {
            return BaseIndexOf(element);
        }

        /// <summary>
        /// 删除元素
        /// </summary>
        /// <param name="element">元素</param>
        public void Remove(ConfigElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.Key);
        }

        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="element">元素</param>
        public void Add(ConfigElement element)
        {
            BaseAdd(element);
        }

        /// <summary>
        /// 按序号删除元素
        /// </summary>
        /// <param name="index">元素索引</param>
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        /// <summary>
        /// 按Key删除元素
        /// </summary>
        /// <param name="key">键名称</param>
        public void Remove(string key)
        {
            BaseRemove(key.ToUpper());
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            BaseClear();
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// 创建配置元素
        /// </summary>
        /// <param name="key">元素名称</param>
        /// <returns>配置元素</returns>
        protected override ConfigurationElement CreateNewElement(string key)
        {
            return new ConfigElement(key);
        }
        /// <summary>
        /// 创建配置元素
        /// </summary>
        /// <returns>配置元素</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigElement();
        }
        /// <summary>
        /// 获取配置元素的Key
        /// </summary>
        /// <param name="element">元素</param>
        /// <returns>对象</returns>
        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((ConfigElement)element).Key;
        }
        /// <summary>
        /// 添加配置元素
        /// </summary>
        /// <param name="element">元素</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }
        #endregion
    }

    /// <summary>
    /// 配置元素类
    /// </summary>
    public class ConfigElement : ConfigurationElement
    {
        #region Public Properties
        /// <summary>
        /// 元素键
        /// </summary>
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get
            {
                return this["key"].ToString().ToUpper();
            }
            set
            {
                this["key"] = value;
            }
        }

        /// <summary>
        /// 元素值
        /// </summary>
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return this["value"].ToString();
            }
            set
            {
                this["value"] = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigElement()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键名称</param>
        public ConfigElement(string key)
        {
            Key = key;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">值</param>
        public ConfigElement(string key, string value)
        {
            Key = key;
            Value = value;
        }
        #endregion
    }
}
