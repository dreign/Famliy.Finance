//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   Remoting处理类
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Reflection;

namespace GW.Utils
{
    /// <summary>
    /// Remoting帮助类
    /// </summary>
    public sealed class RemotingHelper
    {
        #region Remoting

        /// <summary>
        /// 根据对象类型名称从配置项中获取远程对象URL
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <returns>获取远程对象URL</returns>
        public static string GetObjectUrl(string typeName)
        {
            WellKnownClientTypeEntry[] registeredWellKnownClientTypes = RemotingConfiguration.GetRegisteredWellKnownClientTypes();
            if (registeredWellKnownClientTypes != null)
            {
                foreach (WellKnownClientTypeEntry entry in registeredWellKnownClientTypes)
                {
                    if (entry.TypeName.CompareTo(typeName) == 0)
                    {
                        return entry.ObjectUrl;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取远程Remoting对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="serverAddress">Remoting服务地址</param>
        /// <returns>Remoting对象</returns>
        public static T GetObject<T>(string serverAddress)
        {
            return (T)Activator.GetObject(typeof(T), serverAddress);
        }

        #endregion
    }

}
