//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   日志处理，提供有关日志处理的方法
//编写日期    :   2010-11-22
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期   修改者  修改内容
//2010-11-23  jguo  如果写日志的时候发生异常，就将日志异常和需要记录日志中的信息一并写入系统日志中。
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2013,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using log4net;

namespace GW.Utils
{
    /// <summary>
    /// log类型
    /// </summary>
    public enum LogType
    { 
        /// <summary>
        /// 一般Log服务
        /// </summary>
        Information = 0,
        /// <summary>
        /// 错误处理
        /// </summary>
        Exception = 1
    }

    /// <summary>
    /// 日志级别
    /// </summary>
    public enum LogMsgLevel
    {
        /// <summary>
        /// 致命错误信息
        /// </summary>
        Fatal,
        /// <summary>
        /// 错误信息
        /// </summary>
        Error,
        /// <summary>
        /// 警告信息
        /// </summary>
        Warn, 
        /// <summary>
        /// 调试信息
        /// </summary>
        Debug, 
        /// <summary>
        /// 通知信息
        /// </summary>
        Info
    }

    /// <summary>
    /// 日志处理，提供有关日志处理的方法
    /// </summary>
    public class LogHelper
    {
        #region 类成员变量

        //日志名称，对应web.config里的log4net里的name
        private const string CONST_LOGSERVERNAME = "Service";

        #endregion

        #region 写Log

        /// <summary>
        /// 写Log
        /// </summary>
        /// <param name="message">log内容</param>
        public static void Write(string message)
        {
            Write(message, LogMsgLevel.Error);
        }

        /// <summary>
        /// 写Log
        /// </summary>
        /// <param name="message">log内容</param>
        /// <param name="logMsgLevel">log类型</param>
        public static void Write(string message, LogMsgLevel logMsgLevel)
        {
            try
            {
                switch (logMsgLevel)
                {
                    case LogMsgLevel.Debug:
                        LogsHelper().Debug(message);
                        break;
                    case LogMsgLevel.Warn:
                        LogsHelper().Warn(message);
                        break;
                    case LogMsgLevel.Info:
                        LogsHelper().Info(message);
                        break;
                    case LogMsgLevel.Error:
                        LogsHelper().Error(message);
                        break;
                    case LogMsgLevel.Fatal:
                        LogsHelper().Fatal(message);
                        break;
                }
            }
            catch (Exception ex)
            {
                WriteSystemLog(string.Format("写日志的时候发生错误，错误信息：{0}。需要写入日志的信息是：{1}！", ex.Message, message));
            }
        }

        /// <summary>
        /// 将异常错误信息写Log
        /// </summary>
        /// <param name="message">log内容</param>
        /// <param name="ex">异常对象</param>
        public static void Write(string message, Exception ex)
        {
            Write(message, ex, LogMsgLevel.Error);
        }

        /// <summary>
        /// 将异常错误信息写Log
        /// </summary>
        /// <param name="message">log内容</param>
        /// <param name="ex">异常对象</param>
        /// <param name="logMsgLevel">log类型</param>
        public static void Write(string message, Exception ex, LogMsgLevel logMsgLevel)
        {
            try
            {
                switch (logMsgLevel)
                {
                    case LogMsgLevel.Debug:
                        LogsHelper().Debug(message, ex);
                        break;
                    case LogMsgLevel.Warn:
                        LogsHelper().Warn(message, ex);
                        break;
                    case LogMsgLevel.Info:
                        LogsHelper().Info(message, ex);
                        break;
                    case LogMsgLevel.Error:
                        LogsHelper().Error(message, ex);
                        break;
                    case LogMsgLevel.Fatal:
                        LogsHelper().Fatal(message, ex);
                        break;
                }
            }
            catch (Exception exception)
            {
                WriteSystemLog(string.Format("写日志的时候发生错误，错误信息：{0}。需要写入日志的信息是：{1}！", exception.Message, ex.Message));
            }
        }

        /// <summary>
        /// 写系统日志
        /// </summary>
        /// <param name="message">需要写入的信息</param>
        public static void WriteSystemLog(string message)
        {
            WriteSystemLog(message, EventLogEntryType.Error);
        }

        /// <summary>
        /// 写系统日志
        /// </summary>
        /// <param name="message">需要写入的信息</param>
        /// <param name="eventLogEntryType">日志类型</param>
        public static void WriteSystemLog(string message, EventLogEntryType eventLogEntryType)
        {
            EventLog eventLog = new EventLog();
            eventLog.Source = "WebFramework";
            eventLog.WriteEntry(message, eventLogEntryType);
        }

        #endregion

        #region Log操作的私有方法

        /// <summary>
        /// 初始化log4net
        /// </summary>
        /// <returns>ILog</returns>
        private static log4net.ILog LogsHelper()
        {
            return log4net.LogManager.GetLogger(CONST_LOGSERVERNAME);
        }

        #endregion
    }
}
