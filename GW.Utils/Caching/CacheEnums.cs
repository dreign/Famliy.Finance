//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   缓存对象超时模式枚举和时间委托
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
    /// 缓存对象超时模式枚举
    /// </summary>
    public enum CacheExpireModeEnum : byte
    {
        /// <summary>
        /// 最后访问时间
        /// </summary>
        IntervalAfterLastAccess = 0,
        /// <summary>
        /// 绝对时点
        /// </summary>
        AbsoluteTimePoint = 1,
        /// <summary>
        /// 永不超时
        /// </summary>
        Never = 2,
        /// <summary>
        /// 依赖于其他对象
        /// </summary>
        Depend = 3
    }

    /// <summary>
    /// 缓存对象被移除时的代理
    /// 由缓存加载类提供，主要用于在该缓存对象被删除时重新加载该对象
    /// </summary>
    /// <param name="key">键值</param>
    /// <param name="value">缓存的对象，如果此代理负责重新加载该对象，这把新加载的对象赋予此参数，否则赋予此参数为NULL</param>
    /// <param name="cache">当前缓存对象</param>
    /// <param name="reason">缓存过期原因</param>
    public delegate void CacheItemRemovedHandler(string key, object value, Cache cache, CacheExpireModeEnum reason);

    /// <summary>
    /// 缓存对象被移除时的代理
    /// 由缓存管理类提供，主要用于记录日志信息
    /// </summary>
    /// <param name="key">键值</param>
    public delegate void CacheRemoveHandler(string key);

    /// <summary>
    /// 缓存对象被加入时的代理
    /// 由缓存管理类提供，主要用于记录日志
    /// </summary>
    /// <param name="key">键值</param>
    public delegate void CacheAddedHandler(string key);
}
