//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   缓存管理类
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
using System.Collections;
using System.Timers;
using System.Configuration;

namespace GW.Utils.Caching
{
    /// <summary>
    /// 缓存管理类
    /// </summary>
    public class Cache
    {
        #region 成员变量

        private static int MINIMUMCACHECHECKINTERVAL = 5000;
        private static int DEFAULTCACHECHECKINTERVAL = 60000;

        private bool _isEnableMemoryCheck = false;
        private double _availableMemoryPct = 20.0;

        private Hashtable _objectRepository;
        private Hashtable _objectRemoving;
        private Timer _checkTimer;

        /// <summary>
        /// 加入缓存对象回调函数
        /// </summary>
        public event CacheAddedHandler OnCacheAdded;

        /// <summary>
        /// 移除缓存对象回调函数
        /// </summary>
        public event CacheRemoveHandler OnCacheRemoved;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public Cache()
        {
            this._objectRepository = new Hashtable();
            this._objectRemoving = new Hashtable();
            this._checkTimer = new Timer();
            this._checkTimer.Enabled = false;
            this._checkTimer.Interval = GetCacheCheckInterval();
            this._checkTimer.Elapsed += new ElapsedEventHandler(_checkTimer_Elapsed);
            this._checkTimer.Enabled = true;
            this._availableMemoryPct = GetAvailableMemoryPctConfig();
            this._isEnableMemoryCheck = GetIsEnableMemoryCheck();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取缓存检测间隔
        /// </summary>
        /// <returns>时间间隔</returns>
        private static int GetCacheCheckInterval()
        {
            try
            {
                int v = Convert.ToInt32(ConfigurationManager.AppSettings["CacheCheckInterval"]);
                if (v < MINIMUMCACHECHECKINTERVAL)
                    v = MINIMUMCACHECHECKINTERVAL;
                return v;
            }
            catch
            {
                return DEFAULTCACHECHECKINTERVAL;
            }
        }

        /// <summary>
        /// 获得是否需要内存检测
        /// </summary>
        /// <returns>true | False</returns>
        private static bool GetIsEnableMemoryCheck()
        {
            string v = ConfigurationManager.AppSettings["EnableMemoryCheck"];
            try
            {
                return v.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                    v.Equals("TRUE", StringComparison.OrdinalIgnoreCase) ||
                    v.Equals("YES", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取可用的内存配置极限值
        /// </summary>
        /// <returns>可用极限值</returns>
        private static double GetAvailableMemoryPctConfig()
        {
            string v = ConfigurationManager.AppSettings["MemoryThreshold"];
            try
            {
                return Convert.ToDouble(v);
            }
            catch 
            {
                return 20.0;
            }
        }

        /// <summary>
        /// 判断内存是否是ThresholdExceeded的
        /// </summary>
        /// <returns>true | False</returns>
        private bool IsMemoryThresholdExceeded()
        {
            if (!this._isEnableMemoryCheck) return false;
            MemoryInfo memInfo = SystemInfo.GetMemoryInfo();
            return memInfo.dwAvailPhys * 100.0 / memInfo.dwTotalPhys < this._availableMemoryPct;
        }

        /// <summary>
        /// 时间间隔触发时间
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件参数</param>
        private void _checkTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this._checkTimer.Enabled = false;
            try
            {
                this._objectRemoving.Clear();
                List<CacheItem> removed = new List<CacheItem>();
                bool isMemoryThresholdExceeded = this.IsMemoryThresholdExceeded();
                lock (this._objectRepository.SyncRoot)
                {
                    foreach (CacheItem item in this._objectRepository.Values)
                        if (item.TimeOut || (isMemoryThresholdExceeded && item.OnCacheItemRemoved == null))
                        {
                            removed.Add(item);
                        }
                }

                try
                {
                    foreach (CacheItem item in removed)
                    {
                        try
                        {
                            this._objectRemoving.Add(item.Key, item);
                            this._objectRepository.Remove(item.Key);
                            if (item.OnCacheItemRemoved != null) item.OnCacheItemRemoved(item.Key, item.Value, this, item.ExpireMode);
                            if (OnCacheRemoved != null) OnCacheRemoved(item.Key);
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    removed.Clear();
                }
            }
            finally
            {
                this._checkTimer.Enabled = true;
            }
        }

        /// <summary>
        /// 添加CacheItem
        /// </summary>
        /// <param name="item">缓存项</param>
        private void AddItem(CacheItem item)
        {
            this._objectRepository[item.Key] = item;
            if (OnCacheAdded != null) OnCacheAdded(item.Key);
        }

        #endregion

        #region 公共处理方法

        /// <summary>
        /// 添加缓存对象
        /// 缺省添加函数，根据距最后访问的时间间隔计算是否超时，缺省超时时间60分钟
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        public void Add(string key, object value)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, 60);
                this.AddItem(item);
            }
            catch 
            {
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// 根据距最后访问时间间隔计算是否超时
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="timeToLive">距最后访问的时间间隔（分钟）</param>
        public void Add(string key, object value, int timeToLive)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, timeToLive);
                this.AddItem(item);
            }
            catch 
            {
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// 根据绝对时间点计算是否超时
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="expire">绝对时间点</param>
        public void Add(string key, object value, DateTime expire)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, expire);
                this.AddItem(item);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// 根据绝对时间点计算是否超时
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="expire">绝对时间点</param>
        /// <param name="onCacheItemRemoved">缓存对象被移除时的回调函数</param>
        public void Add(string key, object value, DateTime expire, CacheItemRemovedHandler onCacheItemRemoved)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, expire, onCacheItemRemoved);
                this.AddItem(item);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// 根据依赖对象计算是否超时
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dependency">依赖对象</param>
        public void Add(string key, object value, IDependency dependency)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, dependency);
                this.AddItem(item);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 添加缓存对象
        /// 根据依赖对象计算是否超时
        /// </summary>
        /// <param name="key">键值</param>
        /// <param name="value">缓存对象</param>
        /// <param name="dependency">依赖对象</param>
        /// <param name="onCacheItemRemoved">缓存对象被移除时的回调函数</param>
        public void Add(string key, object value, IDependency dependency, CacheItemRemovedHandler onCacheItemRemoved)
        {
            try
            {
                CacheItem item = new CacheItem(key, value, dependency, onCacheItemRemoved);
                this.AddItem(item);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 从缓存中清除一个缓存对象
        /// </summary>
        /// <param name="key">缓存对象键值</param>
        public void Remove(string key)
        {
            this._objectRepository.Remove(key);
        }

        /// <summary>
        /// 缓存对象总数
        /// </summary>
        public int Count
        {
            get { return this._objectRepository.Count; }
        }

        /// <summary>
        /// 获取一个缓存的对象
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns>缓存的对象</returns>
        public object this[string key]
        {
            get
            {
                CacheItem item = this._objectRepository[key] as CacheItem;
                if (item == null)
                {
                    item = this._objectRemoving[key] as CacheItem;
                    if (item == null) return null;
                    return item.Value;
                }
                return item.Value;
            }
        }

        #endregion
    }
}
