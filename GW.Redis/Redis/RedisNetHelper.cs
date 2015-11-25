using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using GW.Utils;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;
using GW.Redis.Common;

namespace GW.Redis
{

    /// <summary>
    /// RedisNetHelper 辅助类
    /// <para> 方法返回值类型均为 Tuple[T,Int32]</para>
    /// <para> Tuple.Item1为 业务逻辑返回值</para>
    /// <para> Tuple.Item2为 Redis底层组件是否交互成功(0:成功;非0:失败)</para>
    /// </summary>
    /// <author>kevin</author>
    /// <createDate>2014-04-14</createDate>
    /// <remarks>
    /// --***************【Redis String 数据结构】***************************--
    ///[set] 操作: key   为null时  运行报错
    ///[set] 操作: value 为null时  编译报错
    ///[get] 操作: key   为null时  引用类型返回 null
    ///[get] 操作: key   为null时  值类型返回 0
    /// 
    /// --***************【Redis List 数据结构】*****************************--
    ///[set] 操作: key   为null时  运行报错
    ///[set] 操作: value 为null时  运行报错
    ///[get] 操作: key   为null时  运行报错
    /// </remarks>
    public static class RedisNetHelper
    {
        #region List Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>第一部分结果集；第二部分,0成功，1失败。</returns>
        public static Tuple<List<string>, Int32> GetAllItemsFromList(string listId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<List<string>, Int32>(redisClient.GetAllItemsFromList(listId), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<List<string>, Int32>(null, 1);
            }
        }

        public static Tuple<List<T>, bool> GetRangeFromList<T>(string listId, int start, int end)
            where T : new()
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    var list = redisClient.GetRangeFromList(listId, start, end);
                    var source = new List<T>();
                    list.ForEach((t) =>
                    {
                        source.Add(JsonHelper.FromJson<T>(t));
                    });
                    return new Tuple<List<T>, bool>(source, true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<List<T>, bool>(null, false);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redisPrefixNameEnum"></param>
        /// <param name="listId"></param>
        /// <returns>第一部分结果集；第二部分,0成功，1失败。</returns>
        public static Tuple<List<string>, Int32> GetAllItemsFromList(Int32 redisPrefixNameEnum, string listId)
        {
            return GetAllItemsFromList(GenerateCacheKey(redisPrefixNameEnum, listId));
        }

        public static Tuple<long, Int32> GetListCount(string listId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<long, Int32>(redisClient.GetListCount(listId), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<long, Int32>(0, 1);
            }
        }


        /// <summary>
        /// 获取 Redis List集合数据
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="returnRecordCount">返回记录数量</param>
        /// <returns></returns>
        public static ApiResponseEntity<List<T>> GetAllItemsFromList<T>(Int32 redisPrefixNameEnum, string listId, Int32 returnRecordCount)
        {
            var resultList = new List<T>();
            var removeList = new List<string>();
            try
            {
                var key = GenerateCacheKey(redisPrefixNameEnum, listId);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    var cacheList = redisClient.GetAllItemsFromList(key);
                    if (cacheList.HasValidValues())
                    {
                        cacheList.Reverse();
                        for (int i = 0; i < cacheList.Count; i++)
                        {
                            if (i < returnRecordCount)
                                resultList.Add(JsonHelper.FromJson<T>(cacheList[i]));//SerializerUtility.ProtoBufBase64Deserialize<T>(cacheList[i])
                            else
                                removeList.Add(cacheList[i]);
                        }
                    }
                }
                //清除多余10条的记录数据（插入时不限制，读取时处理只剩十条）
                if (removeList.HasValidValues())
                {
                    var task = System.Threading.Tasks.Task.Factory.StartNew(() => removeList.ForEach(p => RemoveItemFromList(key, p)));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<List<T>>.GetFaileEntity(ex.Message);
            }
            return ApiResponseEntity<List<T>>.GetSuccessEntity(resultList);
        }


        public static Tuple<long, Int32> GetListCount(Int32 redisPrefixNameEnum, string listId)
        {
            return GetListCount(GenerateCacheKey(redisPrefixNameEnum, listId));
        }



        public static Tuple<Boolean, Int32> AddItemToList(string listId, string value)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {

                    redisClient.AddItemToList(listId, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }


        public static Tuple<Boolean, Int32> AddItemToList(Int32 redisPrefixNameEnum, string listId, string value)
        {
            return AddItemToList(GenerateCacheKey(redisPrefixNameEnum, listId), value);
        }
        /// <summary>
        /// 向Redis List集合中添加数据(列表尾部)
        /// </summary>
        /// <param name="listId">Redis缓存键值</param>
        /// <param name="value">Redis缓存值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddItemToList(string listId, string value, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.AddItemToList(listId, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(listId, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }

        public static Tuple<Boolean, Int32> AddItemToList(Int32 redisPrefixNameEnum, string listId, string value, long expireTime)
        {
            return AddItemToList(GenerateCacheKey(redisPrefixNameEnum, listId), value, expireTime);
        }

        public static Tuple<Boolean, T> LPop<T>(string listid)
            where T : new()
        {
            T t = default(T);
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string value = redisClient.RemoveStartFromList(listid);
                    if (value != null && value.Length > 0)
                    {
                        t = JsonHelper.FromJson<T>(value);
                    }
                    else
                    {
                        return new Tuple<bool, T>(false, t);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.LPop() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, T>(false, t);
            }
            return new Tuple<bool, T>(true, t);
        }

        public static void RPop<T>(string listid)
           where T : new()
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveEndFromList(listid);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RPop() error.StackTrace:{0}", ex.StackTrace), ex);
            }
        }

        /// <summary>
        /// 向Redis List集合中添加数据(列表尾部)
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    //var value = SerializerUtility.ProtoBufBase64Serialize(cacheValue);//加密对象
                    var value = JsonHelper.ToJson(cacheValue);//序列化成字符串
                    var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                    redisClient.AddItemToList(key, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }



        public static ApiResponseEntity<Boolean> AddItemToList<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    var value = JsonHelper.ToJson(cacheValue);//序列化成字符串
                    var key = GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey);
                    redisClient.AddItemToList(key, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        /// <summary>
        /// 向Redis List集合中添加数据(列表尾部)
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string value = cacheValue.ToString();
                    if (typeof(T) != typeof(string))
                        value = JsonHelper.ToJson(cacheValue);//序列化成字符串

                    var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                    redisClient.AddItemToList(key, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(key, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        /// <summary>
        /// 向Redis List集合中添加数据(列表尾部)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisPrefixNameEnum"></param>
        /// <param name="cacheKeyTemplate"></param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToList<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, string cacheKey, T cacheValue, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string value = cacheValue.ToString();
                    if (typeof(T) != typeof(string))
                        value = JsonHelper.ToJson(cacheValue);//序列化成字符串

                    var key = GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey);
                    redisClient.AddItemToList(key, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(key, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToList<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        public static Tuple<Boolean, Int32> RemoveItemFromList(string listId, string value)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveItemFromList(listId, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveItemFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }

        public static Tuple<Boolean, Int32> RemoveItemFromList(Int32 redisPrefixNameEnum, string listId, string value)
        {
            return RemoveItemFromList(GenerateCacheKey(redisPrefixNameEnum, listId), value);
        }

        public static Tuple<Boolean, Int32> AddRangeToList(string listId, List<string> value)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.AddRangeToList(listId, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddRangeToList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listId">Key</param>
        /// <param name="value">列表值</param>
        /// <param name="expireTime">过期时间（单位：秒）</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddRangeToList(string listId, List<string> value, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.AddRangeToList(listId, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(listId, new TimeSpan(expireTime * 10000000));//TimeSpan单位：100毫微秒
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddRangeToList(string listId, List<string> value, long expireTime) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listId">Key</param>
        /// <param name="value">列表值</param>
        /// <param name="expireTime">过期时间（单位：秒）</param>
        /// <returns></returns>
        public static bool AddRangeToList<T>(string listId, List<T> value, long expireTime)
        {
            try
            {
                var values = value.Select(p => JsonHelper.ToJson(p)).ToList();
                var key = listId;
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.AddRangeToList(key, values);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(key, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddRangeToList<T>(string listId, List<T> value) error.[{1}]StackTrace:{0}", ex.StackTrace, ex.Message), ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="typeId">币种编号</param>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="expireTime">过期时间(单位：秒)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddRangeToList<T>(int typeId, string cacheKey, List<T> cacheValue, long expireTime)
        {
            try
            {
                var values = cacheValue.Select(p => JsonHelper.ToJson(p)).ToList();
                var key = GenerateCacheKey(typeId, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.AddRangeToList(key, values);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(key, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddRangeToList<T>(string cacheKeyTemplate, string cacheKey, List<T> cacheValue) error.[{1}]StackTrace:{0}", ex.StackTrace, ex.Message), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        public static Tuple<Boolean, Int32> AddRangeToList(Int32 redisPrefixNameEnum, string listId, List<string> value)
        {
            return AddRangeToList(GenerateCacheKey(redisPrefixNameEnum, listId), value);
        }

        public static Tuple<Boolean, Int32> RemoveStartFromList(string listId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveStartFromList(listId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveStartFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }

        public static Tuple<Boolean, Int32> RemoveStartFromList(Int32 redisPrefixNameEnum, string listId)
        {
            return RemoveStartFromList(GenerateCacheKey(redisPrefixNameEnum, listId));
        }
        public static Tuple<Boolean, Int32> RemoveEndFromList(string listId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveEndFromList(listId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveEndFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }


        public static Tuple<Boolean, Int32> RemoveEndFromList(Int32 redisPrefixNameEnum, string listId)
        {
            return RemoveEndFromList(GenerateCacheKey(redisPrefixNameEnum, listId));
        }

        public static Tuple<Boolean, Int32> RemoveAllFromList(string listId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveAllFromList(listId);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveAllFromList() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }

        public static Tuple<Boolean, Int32> RemoveAllFromList(Int32 redisPrefixNameEnum, string listId)
        {
            return RemoveAllFromList(GenerateCacheKey(redisPrefixNameEnum, listId));
        }
        #endregion

        #region Set Overloaded Methods

        [Obsolete("数据量大时,此方法不建议使用,会产生Redis服务器无法响应情况", true)]
        public static bool SetAll<T>(IDictionary<string, T> values)
        {
            #region
            //var dicCatalog = new Dictionary<string, Dictionary<string, T>>();
            //values.ForEach(p =>
            //{
            //    var strHostPort = RedisManager.RedisSharding.GetNodeByCacheKey(p.Key);
            //    if (dicCatalog.ContainsKey(strHostPort))
            //        dicCatalog[strHostPort].Add(p.Key, p.Value);
            //    else
            //        dicCatalog.Add(strHostPort, new Dictionary<string, T>() { { p.Key, p.Value } });
            //});

            //foreach (var key in dicCatalog.Keys)
            //{
            //    using (var redisClient = RedisManager.GetRedisClient(dicCatalog[key].FirstOrDefault().Key))
            //    {
            //        redisClient.SetAll<T>(dicCatalog[key]);
            //    }
            //}
            #endregion

            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.SetAll<T>(values);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.SetAll<T>(IDictionary<string, T> values) error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
            return true;

        }
        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns>成功时:true;失败时:false;</returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(cacheKey, cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }



        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKey, T cacheValue)
        {
            return Set<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKey), cacheValue);
        }

        public static bool CoverSet<T>(string key, T value, DateTime expireTime)
        {
            try
            {
                string json = JsonHelper.ToJson(value);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.Set(key, json, expireTime);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
        }

        public static bool CoverSet<T>(string key, T value)
        {
            try
            {
                string json = JsonHelper.ToJson(value);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.Set(key, json);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
        }

        /// <summary>
        /// 将 key 中储存的数字值减少
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public static long Decrement(string cacheKey, uint dataValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.Decrement(cacheKey, dataValue);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Decrement() error.StackTrace:{0}", ex.StackTrace), ex);
                return 0;
            }
        }

        public static long Decrement(Int32 redisPrefixNameEnum, string cacheKey, uint dataValue)
        {
            return Decrement(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue);
        }

        public static long DecrementValueBy(string cacheKey, int dataValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.DecrementValueBy(cacheKey, dataValue);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Decrement() error.StackTrace:{0}", ex.StackTrace), ex);
                return 0;
            }
        }



        public static long DecrementValueBy(Int32 redisPrefixNameEnum, string cacheKey, int dataValue)
        {
            return DecrementValueBy(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue);
        }
        public static bool DecrementValueBy(string cacheKey, int dataValue, out long lResult)
        {
            lResult = 0;
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    lResult = redisClient.DecrementValueBy(cacheKey, dataValue);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Decrement() error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
        }

        public static bool DecrementValueBy(Int32 redisPrefixNameEnum, string cacheKey, int dataValue, out long lResult)
        {
            return DecrementValueBy(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue, out lResult);
        }
        /// <summary>
        /// 将 key 中储存的数字值增加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <returns></returns>
        public static long Increment(string cacheKey, uint dataValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.Increment(cacheKey, dataValue);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Increment() error.StackTrace:{0}", ex.StackTrace), ex);
                return 0;
            }
        }



        public static long Increment(Int32 redisPrefixNameEnum, string cacheKey, uint dataValue)
        {
            return Increment(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue);
        }
        public static long IncrementValueBy(string cacheKey, int dataValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.IncrementValueBy(cacheKey, dataValue);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Increment() error.StackTrace:{0}", ex.StackTrace), ex);
                return 0;
            }
        }


        public static long IncrementValueBy(Int32 redisPrefixNameEnum, string cacheKey, int dataValue)
        {
            return IncrementValueBy(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue);
        }

        public static bool IncrementValueBy(string cacheKey, int dataValue, out long lresult)
        {
            lresult = 0;
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    lresult = redisClient.IncrementValueBy(cacheKey, dataValue);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Increment() error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
        }



        public static bool IncrementValueBy(Int32 redisPrefixNameEnum, string cacheKey, int dataValue, out long lresult)
        {
            return IncrementValueBy(GenerateCacheKey(redisPrefixNameEnum, cacheKey), dataValue, out  lresult);
        }

        /// <summary>
        /// 设置缓存,经过时间段立即失效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKey, T cacheValue, TimeSpan timeSpan)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(cacheKey, cacheValue, timeSpan), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue, TimeSpan timeSpan) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }



        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKey, T cacheValue, TimeSpan timeSpan)
        {
            return Set<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKey), cacheValue, timeSpan);
        }

        /// <summary>
        /// 设置缓存,到达时间段立即失效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cacheKey"></param>
        /// <param name="cacheValue"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKey, T cacheValue, DateTime dateTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(cacheKey, cacheValue, dateTime), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue, DateTime dateTime) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }

        }


        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKey, T cacheValue, DateTime dateTime)
        {
            return Set<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKey), cacheValue, dateTime);

        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns>成功时:true;失败时:false;</returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKeyTemplate, string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(GenerateCacheKey(cacheKeyTemplate, cacheKey), cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }
        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey), cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns>成功时:true;失败时:false;</returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKeyTemplate, object cacheKey, T cacheValue)
        {
            return Set(cacheKeyTemplate, cacheKey.ToString(), cacheValue);
        }

        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, object cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey), cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKeyTemplate, string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns>成功时:true;失败时:false;</returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKeyTemplate, List<String> cacheKey, T cacheValue)
        {
            try
            {
                var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(key, cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.(string cacheKeyTemplate, List<String> cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }
        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<String> cacheKey, T cacheValue)
        {
            try
            {
                var key = GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(key, cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.(string cacheKeyTemplate, List<String> cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }

        /// <summary>
        /// Sets an item into the cache at the cache key specified regardless if it already exists or not.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <param name="cacheValue">Redis缓存值</param>
        /// <returns>成功时:true;失败时:false;</returns>
        public static Tuple<Boolean, Int32> Set<T>(string cacheKeyTemplate, List<object> cacheKey, T cacheValue)
        {
            try
            {
                var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(key, cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKeyTemplate, List<object> cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }
        public static Tuple<Boolean, Int32> Set<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<object> cacheKey, T cacheValue)
        {
            try
            {
                var key = GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<bool, int>(redisClient.Set(key, cacheValue), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKeyTemplate, List<object> cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, int>(false, 1);
            }
        }

        #endregion

        #region Get Overloaded Methods
        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns>成功时:泛型实体;失败时:null;</returns>
        public static Tuple<T, Boolean> Get<T>(string cacheKey)
        {
            try
            {
                using (IRedisClient redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<T, bool>(redisClient.Get<T>(cacheKey), true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Get<T>(string cacheKey) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<T, bool>(default(T), false);
            }
        }



        public static Tuple<T, Boolean> Get<T>(Int32 redisPrefixNameEnum, string cacheKey)
        {
            return Get<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKey));
        }
        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns>成功时:泛型实体;失败时:null;</returns>
        public static Tuple<T, Boolean> Get<T>(string cacheKeyTemplate, string cacheKey)
        {
            try
            {
                var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<T, bool>(redisClient.Get<T>(key), true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Get<T>(string cacheKeyTemplate, string cacheKey) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<T, bool>(default(T), false);
            }
        }


        public static Tuple<T, Boolean> Get<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, string cacheKey)
        {
            return Get<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey));
        }

        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns>成功时:泛型实体;失败时:null;</returns>
        public static Tuple<T, Boolean> Get<T>(string cacheKeyTemplate, Object cacheKey)
        {
            return Get<T>(cacheKeyTemplate, cacheKey.ToString());
        }


        public static Tuple<T, Boolean> Get<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, Object cacheKey)
        {
            return Get<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey));
        }
        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns>成功时:泛型实体;失败时:null;</returns>
        public static Tuple<T, Boolean> Get<T>(string cacheKeyTemplate, List<String> cacheKey)
        {
            try
            {
                var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<T, bool>(redisClient.Get<T>(key), true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Get<T>(string cacheKeyTemplate, List<String> cacheKey) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<T, bool>(default(T), false);
            }
        }


        public static Tuple<T, Boolean> Get<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<String> cacheKey)
        {
            return Get<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey));
        }
        /// <summary>
        /// Retrieves the specified item from the cache.
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="cacheKeyTemplate">Redis缓存键模板</param>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns>成功时:泛型实体;失败时:null;</returns>
        public static Tuple<T, bool> Get<T>(string cacheKeyTemplate, List<Object> cacheKey)
        {
            try
            {
                var key = GenerateCacheKey(cacheKeyTemplate, cacheKey);
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<T, bool>(redisClient.Get<T>(key), true);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Get<T>(string cacheKeyTemplate, List<Object> cacheKey) error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<T, bool>(default(T), false);
            }
        }

        public static Tuple<T, bool> Get<T>(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<Object> cacheKey)
        {
            return Get<T>(GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey));
        }


        public static T CoverGet<T>(string key) where T : new()
        {
            T t = default(T);
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string json = redisClient.Get<string>(key);
                    if(json != null && json.Length > 0)
                    {
                        t = JsonHelper.FromJson<T>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Set<T>(string cacheKey, T cacheValue) error.StackTrace:{0}", ex.StackTrace), ex);
            }
            return t;
        }
        #endregion

        public static bool IsExist(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return redisClient.ContainsKey(key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Get<T>(string cacheKeyTemplate, List<Object> cacheKey) error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
        }

        #region 示例代码

        /// <summary>
        /// Redis实现事务 示例代码
        /// </summary>
        /// <param name="cacheKey">Redis缓存键值</param>
        /// <returns></returns>
        private static bool ExampleForRedisTransaction(string cacheKey)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                //创建IRedisTransaction接口的实现实例
                using (var iRedisTransaction = redisClient.CreateTransaction())
                {
                    var age = 0;
                    iRedisTransaction.QueueCommand(r => age = r.Get<Int32>("age"));//事务步骤 1
                    iRedisTransaction.QueueCommand(r => r.Set("age", age + 15));   //事务步骤 2
                    return iRedisTransaction.Commit();                             //提交事务 
                }
            }
        }


        #endregion

        [Obsolete("数据量大时,此方法不建议使用,会产生Redis服务器无法响应情况", true)]
        private static bool RemoveKeys(IEnumerable<string> keys)
        {
            #region
            //var dicCatalog = new Dictionary<string, List<string>>();
            //keys.ForEach(p =>
            //{
            //    var strHostPort = RedisManager.RedisSharding.GetNodeByCacheKey(p);
            //    if (dicCatalog.ContainsKey(strHostPort))
            //        dicCatalog[strHostPort].Add(p);
            //    else
            //        dicCatalog.Add(strHostPort, new List<string> { p });
            //});

            //foreach (var key in dicCatalog.Keys)
            //{
            //    using (var redisClient = RedisManager.GetRedisClient(dicCatalog[key].FirstOrDefault()))
            //    {
            //        redisClient.RemoveAll(dicCatalog[key]);
            //    }
            //}
            #endregion

            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveAll(keys);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveKeys(IEnumerable<string> keys) error.StackTrace:{0}", ex.StackTrace), ex);
                return false;
            }
            return true;
        }

        [Obsolete("数据量大时,此方法不建议使用,会产生Redis服务器无法响应情况", true)]
        private static List<string> SearchKeys(string pattern)
        {
            try
            {
                var result = new List<string>();
                if (RedisManager.RedisServerPwd.Count > 0)
                {
                    RedisManager.RedisServerPwd.ForEach(p =>
                    {
                        var arrHostPort = p.Key.Split(':');
                        using (var redisClient = new RedisClient(arrHostPort[0], Convert.ToInt32(arrHostPort[1]),
                         string.IsNullOrEmpty(p.Value) ? null : p.Value.Trim(), long.MaxValue))
                        {
                            result.AddRange(redisClient.SearchKeys(pattern.Trim()));
                        }
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.SearchKeys(string pattern) error.StackTrace:{0}", ex.StackTrace), ex);
                return null;
            }

            #region
            //List<string> list = new List<string>();
            //using (var redisClient = RedisManager.GetRedisClient())
            //{
            //    list = redisClient.SearchKeys(pattern);
            //}
            //return list;
            #endregion
        }

        #region 非功能性方法

        private static string GenerateCacheKey(string cacheKeyTemplate, string cacheKey)
        {
            return String.Format(cacheKeyTemplate, cacheKey.ToLower().Trim());
        }
        private static string GenerateCacheKey(string cacheKeyTemplate, object cacheKey)
        {
            return GenerateCacheKey(cacheKeyTemplate, cacheKey.ToString());
        }

        private static string GenerateCacheKey(string cacheKeyTemplate, List<String> cacheKey)
        {
            var cacheObjectKey = cacheKey.ConvertAll(new Converter<string, object>(p => (object)(p.ToLower().Trim())));
            return String.Format(cacheKeyTemplate, cacheObjectKey.ToArray());
        }

        private static string GenerateCacheKey(string cacheKeyTemplate, List<object> cacheKey)
        {
            cacheKey.ForEach(p => p = p.ToString().ToLower().Trim());
            return String.Format(cacheKeyTemplate, cacheKey.ToArray());
        }
        //========================================================================================================================









        //========================================================================================================================

        private static string GenerateCacheKey(Int32 redisPrefixNameEnum, string cacheKeyTemplate, string cacheKey)
        {
            return string.Format("{0}:{1}", redisPrefixNameEnum, String.Format(cacheKeyTemplate, cacheKey.ToLower().Trim()));
        }
        private static string GenerateCacheKey(Int32 redisPrefixNameEnum, string cacheKeyTemplate, object cacheKey)
        {
            return GenerateCacheKey(redisPrefixNameEnum, cacheKeyTemplate, cacheKey.ToString());
        }

        private static string GenerateCacheKey(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<String> cacheKey)
        {
            var cacheObjectKey = cacheKey.ConvertAll(new Converter<string, object>(p => (object)(p.ToLower().Trim())));
            return string.Format("{0}:{1}", redisPrefixNameEnum, String.Format(cacheKeyTemplate, cacheObjectKey.ToArray()));

        }

        private static string GenerateCacheKey(Int32 redisPrefixNameEnum, string cacheKeyTemplate, List<object> cacheKey)
        {
            cacheKey.ForEach(p => p = p.ToString().ToLower().Trim());
            return string.Format("{0}:{1}", redisPrefixNameEnum, String.Format(cacheKeyTemplate, cacheKey.ToArray()));
        }

        /// <summary>
        /// redis KEY 加前缀
        /// </summary>
        /// <param name="redisPrefixNameEnum">前缀的 money_type_id</param>
        /// <param name="cacheKey">旧的key值</param>
        /// <returns></returns>
        public static string GenerateCacheKey(Int32 redisPrefixNameEnum, string cacheKey)
        {
            return string.Format("{0}:{1}", redisPrefixNameEnum, cacheKey.ToLower().Trim());
        }
        #endregion


        #region HashTable Methods

        #region GetAllItemsFromHash

        /// <summary>
        /// 返回哈希表key中，所有的域和值
        /// </summary>
        /// <param name="listId"></param>
        /// <returns>第一部分结果集；第二部分,0成功，1失败。</returns>
        public static Tuple<List<string>, Int32> GetAllItemsFromHash(string hashId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<List<string>, Int32>(redisClient.GetHashValues(hashId), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromHash() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<List<string>, Int32>(null, 1);
            }
        }

        /// <summary>
        /// 返回哈希表key中，所有的域和值
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀（typeId）</param>
        /// <param name="hashId">key键名</param>
        /// <param name="listId"></param>
        /// <returns>第一部分结果集；第二部分,0成功，1失败。</returns>
        public static Tuple<List<string>, Int32> GetAllItemsFromHash(Int32 redisPrefixNameEnum, string hashId)
        {
            return GetAllItemsFromHash(GenerateCacheKey(redisPrefixNameEnum, hashId));
        }

        //TODO:方法试试能不能重写，无法重写就删除。
        /// <summary>
        /// 获取 Redis Hash 域 值数据
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <param name="hashKey">HASH键</param>
        /// <param name="returnRecordCount">返回记录数量</param>
        /// <returns></returns>
        //public static ApiResponseEntity<List<T>> GetAllItemsFromHash<T>(Int32 redisPrefixNameEnum, string hashKey, Int32 returnRecordCount)
        //{
        //    var resultList = new List<T>();
        //    var removeList = new List<string>();
        //    try
        //    {
        //        var key = GenerateCacheKey(redisPrefixNameEnum, hashKey);
        //        using (var redisClient = RedisManager.GetRedisClient())
        //        {
        //            var cacheList = redisClient.GetAllEntriesFromHash(key);
        //            if (cacheList.Count > 0)
        //            {
        //                cacheList.Reverse();//反转序列中元素的顺序
        //                for (int i = 0; i < cacheList.Count; i++)
        //                {
        //                    if (i < returnRecordCount)
        //                        resultList.Add(JsonHelper.FromJson<T>(cacheList[i]));//SerializerUtility.ProtoBufBase64Deserialize<T>(cacheList[i])
        //                    else
        //                        removeList.Add(cacheList[i]);
        //                }
        //            }
        //        }
        //        //清除多余10条的记录数据（插入时不限制，读取时处理只剩十条）
        //        if (removeList.HasValidValues())
        //        {
        //            var task = System.Threading.Tasks.Task.Factory.StartNew(() => removeList.ForEach(p => RemoveItemFromList(key, p)));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromHash() error.StackTrace:{0}", ex.StackTrace), ex);
        //        return ApiResponseEntity<List<T>>.GetFaileEntity(ex.Message);
        //    }
        //    return ApiResponseEntity<List<T>>.GetSuccessEntity(resultList);
        //}

        #endregion

        #region GetHashCount

        /// <summary>
        /// 返回哈希表key 中域的数量
        /// </summary>
        /// <param name="hashId"></param>
        /// <returns></returns>
        public static Tuple<long, Int32> GetHashCount(string hashId)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    return new Tuple<long, Int32>(redisClient.GetHashCount(hashId), 0);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetHashCount() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<long, Int32>(0, 1);
            }
        }
        /// <summary>
        /// 返回哈希表key 中域的数量
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static Tuple<long, Int32> GetHashCount(Int32 redisPrefixNameEnum, string hashId)
        {
            return GetHashCount(GenerateCacheKey(redisPrefixNameEnum, hashId));
        }

        #endregion

        #region AddItemToHashSet 单个记录插入

        /// <summary>
        /// 添加hash表记录
        /// </summary>
        /// <param name="hashId">hash Id</param>
        /// <param name="key">hash中的域</param>
        /// <param name="value">hash中的域的值</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddItemToHashSet(string hashId, string key, string value)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.SetEntryInHash(hashId, key, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToHashSet() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }
        /// <summary>
        /// 添加hash表记录
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <param name="hashId">hash Id</param>
        /// <param name="key">hash中的域</param>
        /// <param name="value">hash中的域的值</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddItemToHashSet(Int32 redisPrefixNameEnum, string hashId, string key, string value)
        {
            return AddItemToHashSet(GenerateCacheKey(redisPrefixNameEnum, hashId), key, value);
        }

        /// <summary>
        /// 向Redis Hash集合中添加数据(列表尾部)(已存在就覆盖，未存在就新建)
        /// </summary>
        /// <param name="listId">Redis缓存键值</param>
        /// <param name="value">Redis缓存值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddItemToHashSet(string hashId, string key, string value, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.SetEntryInHash(hashId, key, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(hashId, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToHashSet() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }

        /// <summary>
        /// 向Redis Hash集合中添加数据(列表尾部)(已存在就覆盖，未存在就新建)
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <param name="listId">Redis缓存键值</param>
        /// <param name="value">Redis缓存值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> AddItemToHashSet(Int32 redisPrefixNameEnum, string hashId, string key, string value, long expireTime)
        {
            return AddItemToHashSet(GenerateCacheKey(redisPrefixNameEnum, hashId), key, value, expireTime);
        }


        /// <summary>
        /// 向Redis Hasj集合中添加数据(列表尾部)
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="hashId">hash记录键</param>
        /// <param name="cacheKey">hash的记录的域</param>
        /// <param name="cacheValue">hash的记录的域的值</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToHashSet<T>(string hashId, string cacheKey, T cacheValue)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string value = JsonHelper.ToJson(cacheValue);//对象序列化成字符串
                    redisClient.SetEntryInHash(hashId, cacheKey, value);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToHashSet<T> error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        /// <summary>
        /// 向Redis Hasj集合中添加数据(列表尾部)
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="hashId">hash记录键</param>
        /// <param name="cacheKey">hash的记录的域</param>
        /// <param name="cacheValue">hash的记录的域的值</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToHashSet<T>(Int32 redisPrefixNameEnum, string hashId, string cacheKey, T cacheValue)
        {
            var hashKey = GenerateCacheKey(redisPrefixNameEnum, hashId);
            return AddItemToHashSet(hashId, cacheKey, cacheValue);
        }

        /// <summary>
        /// 向Redis Hash记录中添加数据(列表尾部--待测)
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="hashId">Hash记录ID</param>
        /// <param name="cacheKey">Hash记录的域</param>
        /// <param name="cacheValue">Hash记录的域的值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToHashSet<T>(string hashId, string cacheKey, T cacheValue, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    string value = cacheValue.ToString();
                    if (typeof(T) != typeof(string))
                        value = JsonHelper.ToJson(cacheValue);//序列化成字符串

                    redisClient.SetEntryInHash(hashId, cacheKey, value);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(hashId, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.AddItemToHashSet<T> error.StackTrace:{0}", ex.StackTrace), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }

        /// <summary>
        /// 向Redis List集合中添加数据(列表尾部)
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="redisPrefixNameEnum"></param>
        /// <param name="hashId">Hash记录ID</param>
        /// <param name="cacheKey">Hash记录的域</param>
        /// <param name="cacheValue">Hash记录的域的值</param>
        /// <param name="expireTime">过期时间(单位秒)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> AddItemToHashSet<T>(Int32 redisPrefixNameEnum, string hashId, string cacheKey, T cacheValue, long expireTime)
        {
            var key = GenerateCacheKey(redisPrefixNameEnum, hashId);
            return AddItemToHashSet(key, cacheKey, cacheValue, expireTime);
        }

        #endregion

        #region 多个记录插入
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="hashId">Hash ID</param>
        /// <param name="keyValuePairs">键值对</param>
        /// <param name="expireTime">过期时间(单位：秒)(小于等于0，忽略此参数；大于0，参数生效)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> SetRangeInHash(string hashId, IEnumerable<KeyValuePair<string, string>> keyValuePairs, long expireTime)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.SetRangeInHash(hashId, keyValuePairs);
                    //添加过期时间
                    if (expireTime > 0)
                        redisClient.ExpireEntryIn(hashId, new TimeSpan(expireTime * 10000000));
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.SetRangeInHash error.[{1}]StackTrace:{0}", ex.StackTrace, ex.Message), ex);
                return ApiResponseEntity<Boolean>.GetFaileEntity();
            }
            return ApiResponseEntity<Boolean>.GetSuccessEntity(true);
        }
        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="typeId">币种编号</param>
        /// <param name="hashId">Hash ID</param>
        /// <param name="keyValuePairs">键值对</param>
        /// <param name="expireTime">过期时间(单位：秒)(小于等于0，忽略此参数；大于0，参数生效)</param>
        /// <returns></returns>
        public static ApiResponseEntity<Boolean> SetRangeInHash(int typeId, string hashId, IEnumerable<KeyValuePair<string, string>> keyValuePairs, long expireTime)
        {
            var key = GenerateCacheKey(typeId, hashId);
            return SetRangeInHash(key, keyValuePairs, expireTime);
        }


        #endregion

        #region RemoveEntryFromHash
        /// <summary>
        /// 删除Rdeis Hash中的指定的域
        /// </summary>
        /// <param name="hashId">Hash ID</param>
        /// <param name="key">域名称(记录中的key)</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> RemoveEntryFromHash(string hashId, string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.RemoveEntryFromHash(hashId, key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.RemoveEntryFromHash() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }
        /// <summary>
        /// 删除Rdeis Hash中的指定的域
        /// </summary>
        /// <param name="redisPrefixNameEnum">币种前缀</param>
        /// <param name="hashId">Hash ID</param>
        /// <param name="key">域名称(记录中的key)</param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> RemoveEntryFromHash(Int32 redisPrefixNameEnum, string hashId, string key)
        {
            return RemoveEntryFromHash(GenerateCacheKey(redisPrefixNameEnum, hashId), key);
        }

        /// <summary>
        /// 删除给定的一个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true成功；false失败</returns>
        public static Tuple<Boolean, Int32> Remove(string key)
        {
            try
            {
                using (var redisClient = RedisManager.GetRedisClient())
                {
                    redisClient.Remove(key);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.Remove() error.StackTrace:{0}", ex.StackTrace), ex);
                return new Tuple<bool, Int32>(false, 1);
            }
            return new Tuple<bool, Int32>(true, 0);
        }
        /// <summary>
        /// 删除给定的一个key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Tuple<Boolean, Int32> Remove(int typeId, string key)
        {
            var hashId = GenerateCacheKey(typeId, key);
            return Remove(hashId);
        }
        #endregion

        #endregion


        #region Hash  Methods 2

        //public static Tuple<List<string>, Int32> GetAllItemsFromHash(string listId)
        //{
        //    try
        //    {
        //        return new Tuple<List<string>, Int32>(HashGetAll<string>(listId), 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Write(string.Format("GW.Redis.RedisNetHelper.GetAllItemsFromList() error.StackTrace:{0}", ex.StackTrace), ex);
        //        return new Tuple<List<string>, Int32>(null, 1);
        //    }
        //}

        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        public static bool HashExist<T>(string hashId, string key)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                return redisClient.HashContainsEntry(hashId, key);
            }
        }
        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        public static bool HashSet<T>(string hashId, string key, T t)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                //var value = JsonSerializer.SerializeToString<T>(t);
                var value = JsonHelper.ToJson(t);
                return redisClient.SetEntryInHash(hashId, key, value);
            }
        }
        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        public static bool HashRemove(string hashId, string key)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                return redisClient.RemoveEntryFromHash(hashId, key);
            }
        }
        /// <summary>
        /// 移除整个hash
        /// </summary>
        public static bool HashRemoveAll(string hashId)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                return redisClient.Remove(hashId);
            }
        }
        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        public static T HashGet<T>(string hashId, string key)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                string value = redisClient.GetValueFromHash(hashId, key);
                return JsonHelper.FromJson<T>(value);
            }
        }
        /// <summary>
        /// 获取整个hash的数据
        /// </summary>
        public static List<T> HashGetAll<T>(string hashId)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                var result = new List<T>();
                var list = redisClient.GetHashValues(hashId);
                if (list != null && list.Count > 0)
                {
                    list.ForEach(x =>
                    {
                        var value = JsonHelper.FromJson<T>(x);
                        result.Add(value);
                    });
                }
                return result;
            }
        }
        /// <summary>
        /// 设置缓存过期
        /// </summary>
        public static void HashSetExpire(string key, DateTime datetime)
        {
            using (var redisClient = RedisManager.GetRedisClient())
            {
                redisClient.ExpireEntryAt(key, datetime);
            }
        }

        #endregion
    }
}
