using System;
using System.Configuration;
using GW.Utils;

namespace GW.Redis
{
    public sealed class RedisConfigInfo : ConfigurationSection
    {
        /// <summary>
        /// Redis配置信息类
        /// </summary>
        /// <author>kevin</author>
        /// <createDate>2014-04-14</createDate>
        public static RedisConfigInfo GetConfig()
        {
            try
            {
                return (RedisConfigInfo)ConfigurationManager.GetSection("RedisConfig");
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisConfigInfo.GetConfig() error. RedisConfig.config文件中缺失或配置节点信息错误;StackTrace:{0}",ex.StackTrace), ex);
                throw new ConfigurationErrorsException("RedisConfig.config文件中缺失或配置节点信息错误");
            }
           
        }

        /// <summary>
        /// 获取Redis配置信息
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <returns></returns>
        public static RedisConfigInfo GetConfig(string sectionName)
        {
            var section = (RedisConfigInfo)ConfigurationManager.GetSection(sectionName.Trim());
            if (section == null)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisConfigInfo.GetConfig() error.Section {0} is not found.", sectionName));
                throw new ConfigurationErrorsException("Section " + sectionName.Trim() + " is not found.");
            }
            return section;
        }

        /// <summary>
        /// 可写的Redis链接地址
        /// </summary>
        [ConfigurationProperty("WriteServerList", IsRequired = false)]
        public string WriteServerList
        {
            get
            {
                return (string)base["WriteServerList"];
            }
            set
            {
                base["WriteServerList"] = value;
            }
        }


        /// <summary>
        /// 可读的Redis链接地址
        /// </summary>
        [ConfigurationProperty("ReadServerList", IsRequired = false)]
        public string ReadServerList
        {
            get
            {
                return (string)base["ReadServerList"];
            }
            set
            {
                base["ReadServerList"] = value;
            }
        }


        /// <summary>
        /// 最大写链接数
        /// </summary>
        [ConfigurationProperty("MaxWritePoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxWritePoolSize
        {
            get
            {
                int _maxWritePoolSize = (int)base["MaxWritePoolSize"];
                return _maxWritePoolSize > 0 ? _maxWritePoolSize : 5;
            }
            set
            {
                base["MaxWritePoolSize"] = value;
            }
        }


        /// <summary>
        /// 最大读链接数
        /// </summary>
        [ConfigurationProperty("MaxReadPoolSize", IsRequired = false, DefaultValue = 5)]
        public int MaxReadPoolSize
        {
            get
            {
                int _maxReadPoolSize = (int)base["MaxReadPoolSize"];
                return _maxReadPoolSize > 0 ? _maxReadPoolSize : 5;
            }
            set
            {
                base["MaxReadPoolSize"] = value;
            }
        }


        /// <summary>
        /// 自动重启
        /// </summary>
        [ConfigurationProperty("AutoStart", IsRequired = false, DefaultValue = true)]
        public bool AutoStart
        {
            get
            {
                return (bool)base["AutoStart"];
            }
            set
            {
                base["AutoStart"] = value;
            }
        }



        /// <summary>
        /// 本地缓存到期时间，单位:秒
        /// </summary>
        [ConfigurationProperty("LocalCacheTime", IsRequired = false, DefaultValue = 36000)]
        public int LocalCacheTime
        {
            get
            {
                return (int)base["LocalCacheTime"];
            }
            set
            {
                base["LocalCacheTime"] = value;
            }
        }


        /// <summary>
        /// 是否记录日志,该设置仅用于排查redis运行时出现的问题,如redis工作正常,请关闭该项
        /// </summary>
        [ConfigurationProperty("RecordeLog", IsRequired = false, DefaultValue = false)]
        public bool RecordeLog
        {
            get
            {
                return (bool)base["RecordeLog"];
            }
            set
            {
                base["RecordeLog"] = value;
            }
        }

        /// <summary>
        /// Redis服务器访问密钥
        /// </summary>
        [ConfigurationProperty("RedisServerInfo", IsRequired = false, DefaultValue = "")]
        public string RedisServerInfo
        {
            get
            {
                return (string)base["RedisServerInfo"];
            }
            set
            {
                base["RedisServerInfo"] = value;
            }
        }

    }
}
