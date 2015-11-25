//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   缓存项处理类
//编写日期    :   2010-12-15
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace GW.Utils.Caching
{
    /// <summary>
    /// 缓存项处理类
    /// </summary>
    public class CacheItem
    {
        #region 成员变量

        private string _key;
        private object _value;
        private DateTime _lastAccess;
        private int _ttl;
        private DateTime _expire;
        private IDependency _dependency;
        private CacheExpireModeEnum _expireMode;

        /// <summary>
        /// 缓存移除后的事件
        /// </summary>
        public CacheItemRemovedHandler OnCacheItemRemoved;

        #endregion

        #region 构造函数

        /// <summary>
        /// 时间间隔类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="timeToLive">距最后一次访问的时间间隔（分钟）</param>
        public CacheItem(string key, object value, int timeToLive)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.IntervalAfterLastAccess;
            this._expire = DateTime.Now.AddYears(100);
            this._lastAccess = DateTime.Now;
            this._ttl = timeToLive;
            this.OnCacheItemRemoved = null;
        }

        /// <summary>
        /// 时间间隔类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="timeToLive">距最后一次访问的时间间隔（分钟）</param>
        /// <param name="onCacheItemRemoved">对象被从缓存中移除时的回调函数</param>
        public CacheItem(string key, object value, int timeToLive, CacheItemRemovedHandler onCacheItemRemoved)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.IntervalAfterLastAccess;
            this._expire = DateTime.Now.AddYears(100);
            this._lastAccess = DateTime.Now;
            this._ttl = timeToLive;
            this.OnCacheItemRemoved += new CacheItemRemovedHandler(onCacheItemRemoved);
        }

        /// <summary>
        /// 绝对时间点超时类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="expire">绝对时间点</param>
        public CacheItem(string key, object value, DateTime expire)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.AbsoluteTimePoint;
            this._expire = expire;
            this._lastAccess = DateTime.Now;
            this._ttl = -1;
            this.OnCacheItemRemoved = null;
        }

        /// <summary>
        /// 绝对时间点超时类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="expire">绝对时间点</param>
        /// <param name="onCacheItemRemoved">对象被从缓存中移除时的回调函数</param>
        public CacheItem(string key, object value, DateTime expire, CacheItemRemovedHandler onCacheItemRemoved)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.AbsoluteTimePoint;
            this._expire = expire;
            this._lastAccess = DateTime.Now;
            this._ttl = -1;
            this.OnCacheItemRemoved += new CacheItemRemovedHandler(onCacheItemRemoved);
        }

        /// <summary>
        /// 依赖于外部对象类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="dependency">依赖对象</param>
        public CacheItem(string key, object value, IDependency dependency)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.Depend;
            this._expire = DateTime.Now.AddYears(100);
            this._lastAccess = DateTime.Now;
            this._ttl = -1;
            this._dependency = dependency;
            this.OnCacheItemRemoved = null;
        }

        /// <summary>
        /// 依赖于外部对象类构造函数
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存的对象</param>
        /// <param name="dependency">依赖对象</param>
        /// <param name="onCacheItemRemoved">对象被从缓存中移除时的回调函数</param>
        public CacheItem(string key, object value, IDependency dependency, CacheItemRemovedHandler onCacheItemRemoved)
        {
            this._key = key;
            this._value = value;
            this._expireMode = CacheExpireModeEnum.Depend;
            this._expire = DateTime.Now.AddYears(100);
            this._lastAccess = DateTime.Now;
            this._ttl = -1;
            this._dependency = dependency;
            this.OnCacheItemRemoved += new CacheItemRemovedHandler(onCacheItemRemoved);
        } 

        #endregion

        #region 属性

        /// <summary>
        /// 键值
        /// </summary>
        public string Key
        {
            get { return this._key; }
        }

        /// <summary>
        /// 缓存对象
        /// </summary>
        public object Value
        {
            get
            {
                this._lastAccess = DateTime.Now;
                return this._value;
            }
            set
            {
                this._lastAccess = DateTime.Now;
                this._value = value;
            }
        }

        /// <summary>
        /// 超时模式
        /// </summary>
        public CacheExpireModeEnum ExpireMode
        {
            get { return this._expireMode; }
        }

        /// <summary>
        /// 是否超时
        /// </summary>
        public bool TimeOut
        {
            get
            {
                switch (this._expireMode)
                {
                    case CacheExpireModeEnum.AbsoluteTimePoint:
                        return this._expire.CompareTo(DateTime.Now) <= 0;
                    case CacheExpireModeEnum.IntervalAfterLastAccess:
                        return this._lastAccess.AddMinutes(this._ttl).CompareTo(DateTime.Now) <= 0;
                    case CacheExpireModeEnum.Depend:
                        if (this._dependency != null)
                            return this._dependency.ConditionMatched();
                        return false;
                    default:
                        return false;
                }
            }
        } 

        #endregion
    }
}
