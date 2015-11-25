using System;
using System.Collections.Generic;
using System.Linq;
using GW.Utils;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;

namespace GW.Redis
{
    /// <summary>
    /// Redis管理类
    /// </summary>
    /// <author>kevin</author>
    /// <createDate>2014-04-14</createDate>
    public static class RedisManager
    {
        /// <summary>
        /// Redis配置文件信息
        /// </summary>
        private static readonly RedisConfigInfo RedisConfigInfo = RedisConfigInfo.GetConfig();

        /// <summary>
        /// RedisServer集群信息
        /// </summary>
        public static readonly Dictionary<String, String> RedisServerPwd = GetRedisServerPwd();

        private static IRedisNativeClient NativeClient = null;
      
        /// <summary>
        /// RedisSharding一致性哈希
        /// </summary>
        //public static readonly RedisSharding RedisSharding = new RedisSharding(RedisServerPwd.Keys.ToList());

        private static Dictionary<String, String> GetRedisServerPwd()
        {
            if (string.IsNullOrEmpty(RedisConfigInfo.RedisServerInfo))
            {
                LogHelper.Write("GW.Redis.RedisManager.GetRedisServerPwd() error. RedisConfig.config文件中缺失[RedisServerInfo]信息.");
                throw new Exception("RedisConfig.config文件中缺失[RedisServerInfo]信息.报错方法：GetRedisServerPwd");
            }
            var result = new Dictionary<String, String>();
            var ipPwdPairs = RedisConfigInfo.RedisServerInfo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ipPwdPairs.ForEach(p =>
            {
                var ipPwdPair = p.Split (new[] { ':' });
                result.Add(string.Format("{0}:{1}", ipPwdPair[0], ipPwdPair[1]), ipPwdPair[2]);
            });
            return result;
        }


        /// <summary>
        /// Redis缓存池信息
        /// </summary>
        private static readonly PooledRedisClientManager _prcm = CreateManager();

        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static PooledRedisClientManager CreateManager()
        {
            #region
            // var writeServerList = RedisServerPwd.Keys;// RedisConfigInfo.WriteServerList.Split(',');
            //var readServerList = RedisServerPwd.Keys;// RedisConfigInfo.ReadServerList.Split(',');
            #endregion
            return new PooledRedisClientManager(RedisServerPwd.Keys, RedisServerPwd.Keys,
                              new RedisClientManagerConfig
                              {
                                  MaxWritePoolSize = RedisConfigInfo.MaxWritePoolSize,
                                  MaxReadPoolSize = RedisConfigInfo.MaxReadPoolSize,
                                  AutoStart = RedisConfigInfo.AutoStart,
                              });
        }
        private static IRedisNativeClient RedisClientInstance()
        {
            if (NativeClient == null)
            {
                NativeClient = new RedisNativeClient("10.15.89.60", 6379, "");
            }
            return NativeClient;
        }
        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetRedisClient()
        {

            var redisClient = _prcm.GetClient();
            if (RedisServerPwd.Count > 0)
            {
                redisClient.Password = string.IsNullOrEmpty(RedisServerPwd.Values.First())
                    ? null
                    : RedisServerPwd.Values.First().Trim();
            }
            return redisClient;
        }



        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisManager()
        {
            // CreateManager();
        }



        #region 封装底层RedisClient对象方法

        #endregion

        #region 非功能性方法

        #endregion

        #region 注释掉的代码

        /// <summary>
        /// Redis缓存池集合信息
        /// </summary>
        //private static readonly Dictionary<string, PooledRedisClientManager> _dicPrcm = new Dictionary<string, PooledRedisClientManager>();



        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        //private static void CreateManager2()
        //{
        //    if (RedisServerPwd.Count > 0)
        //    {
        //        RedisServerPwd.Keys.ForEach(
        //            p => _dicPrcm.Add(p, new PooledRedisClientManager(new[] { p }, new[] { p },
        //                    new RedisClientManagerConfig
        //                    {
        //                        MaxWritePoolSize = RedisConfigInfo.MaxWritePoolSize,
        //                        MaxReadPoolSize = RedisConfigInfo.MaxReadPoolSize,
        //                        AutoStart = RedisConfigInfo.AutoStart,
        //                    })));
        //    }
        //    else
        //        throw new Exception("RedisConfig.config文件中缺失[RedisServerInfo]信息.报错方法：CreateManager");
        //}






        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <param name="cacheKey">缓存键字符串</param>
        /// <returns></returns>
        //public static IRedisClient GetRedisClient(string cacheKey)
        //{
        //    var strHostPort = RedisSharding.GetNodeByCacheKey(cacheKey);
        //    if (RedisServerPwd != null)
        //    {
        //        var redisClient = _dicPrcm[strHostPort].GetClient();
        //        redisClient.Password = string.IsNullOrEmpty(RedisServerPwd[strHostPort].Trim())
        //            ? null
        //            : RedisServerPwd[strHostPort].Trim();
        //        return redisClient;
        //    }
        //    else
        //    {
        //        throw new Exception("初始化[RedisClient]对象失败,[RedisServerPwd]值为null.报错方法： GetRedisClient(string cacheKey)");
        //    }
        //}


        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        /// <param name="cacheKey">缓存键字符串</param>
        /// <returns></returns>
        //public static IRedisClient GetRedisClient2(string cacheKey)
        //{
        //    var strHostPort = RedisSharding.GetNodeByCacheKey(cacheKey);
        //    var arrHostPort = strHostPort.Split(':');
        //    if (RedisServerPwd != null)
        //    {
        //        var redisClient = new RedisClient(arrHostPort[0], Convert.ToInt32(arrHostPort[1]),
        //             string.IsNullOrEmpty(RedisServerPwd[strHostPort].Trim())
        //                 ? null
        //                 : RedisServerPwd[strHostPort].Trim());
        //        return redisClient;
        //    }
        //    else
        //    {
        //        throw new Exception("初始化[RedisClient]对象失败,[RedisServerPwd]值为null.");
        //    }
        //}

        #endregion
    }
}
