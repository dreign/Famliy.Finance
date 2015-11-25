//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   缓存处理类
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
using System.Collections;
using System.Web;

using GW.Utils.Caching;

namespace GW.Utils.Web
{
    /// <summary>
    /// 缓存处理类
    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 填充缓存的方法委托
        /// </summary>
        /// <returns>事件委托</returns>
        public delegate object WebCacheAction();

        #region --方法--

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">key名</param>
        /// <returns>缓存对象</returns>
        public static object GetCache(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey];
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="cacheKey">缓存ID</param>
        /// <param name="expire">过期时间</param>
        /// <param name="action">当缓存为空时的动作</param>
        /// <returns>从缓存中读取的对象</returns>
        public static object GetCache(string cacheKey, DateTime expire, WebCacheAction action)
        {
            object o = GetCache(cacheKey);
            if (o == null)
            {
                o = action.Invoke();
                SetCache(cacheKey, o, expire);
            }
            return o;
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey">key名</param>
        /// <param name="objObject">要被缓存的对象</param>
        public static void SetCache(string cacheKey, object objObject)
        {
            if (objObject != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, objObject);
            }
            else
            {
                RemoveCache(cacheKey);
            }
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="cacheKey">key名</param>
        /// <param name="objObject">要被缓存的对象</param>
        /// <param name="expire">过期时间</param>
        public static void SetCache(string cacheKey, object objObject, DateTime expire)
        {
            if (objObject != null)
            {
                HttpRuntime.Cache.Insert(cacheKey, objObject, null, expire, TimeSpan.Zero);
            }
            else
            {
                RemoveCache(cacheKey);
            }
        }

        /// <summary>
        /// 根据键名清除缓存
        /// </summary>
        /// <param name="cacheKey">key名</param>
        public static void RemoveCache(string cacheKey)
        {
            HttpRuntime.Cache.Remove(cacheKey);
        }

        /// <summary>
        /// 从缓存中去除前缀缓存名为参数的缓存
        /// </summary>
        /// <param name="prefixCacheKey">缓存名的前缀</param>
        public static void RemoveCacheByPrefix(string prefixCacheKey)
        {
            foreach (DictionaryEntry cache in HttpRuntime.Cache)
            {
                string cacheKey = cache.Key.ToString();

                if (cacheKey.StartsWith(prefixCacheKey))
                {
                    RemoveCache(cacheKey);
                }
            }
        }

        #endregion

        #region --Cache--

        /// <summary>
        /// 缓存更新的调度机制
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="initValue">原来的数据</param>
        /// <param name="loadedValue">更新后的数据</param>
        /// <param name="cache">缓存对象</param>
        /// <param name="expire">更新时间</param>
        /// <param name="reason">缓存对象超时模式枚举</param>
        /// <param name="cacheReloadEvent">cache重载回调</param>
        public virtual void ReloadCache(string key, object initValue, object loadedValue, Cache cache, DateTime expire, CacheExpireModeEnum reason, CacheItemRemovedHandler cacheReloadEvent)
        {
            if (cache != null)
            {
                if (loadedValue != null)
                {
                    SetCacheRefreshAfterLoad(key, loadedValue, cache, expire, cacheReloadEvent);
                }
                else
                {
                    SetCacheRefreshWhenFail(key, initValue, cache, cacheReloadEvent);
                }
            }
        }

        /// <summary>
        /// 正常更新机制
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="cache">缓存</param>
        /// <param name="expire">过期时间</param>
        /// <param name="cacheRemoveEvent">缓存删除回调事件</param>
        public virtual void SetCacheRefreshAfterLoad(string key, object value, Cache cache, DateTime expire, CacheItemRemovedHandler cacheRemoveEvent)
        {
            cache.Add(key, value, expire, cacheRemoveEvent);
        }

        /// <summary>
        /// 异常更新机制
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        /// <param name="cache">缓存</param>
        /// <param name="cacheRemoveEvent">缓存删除回调事件</param>
        public virtual void SetCacheRefreshWhenFail(string key, object value, Cache cache, CacheItemRemovedHandler cacheRemoveEvent)
        {
            cache.Add(key, value, DateTime.Now.AddMinutes(1), cacheRemoveEvent);
        }

        /// <summary>
        /// 不带缓存更新函数的调度机制
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="initValue">原来的数据</param>
        /// <param name="loadedValue">更新后的数据</param>
        /// <param name="cache">缓存对象</param>
        /// <param name="expire">更新时间</param>
        public virtual void ReloadCacheWithoutRemoveFunc(string key, object initValue, object loadedValue, Cache cache, DateTime expire)
        {
            if (cache != null)
            {
                if (loadedValue != null)
                {
                    cache.Add(key, loadedValue, expire);
                }
                else
                {
                    cache.Add(key, initValue, expire);
                }
            }
        }
         
        #endregion
    }
}
