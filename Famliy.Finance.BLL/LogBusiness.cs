using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW.Utils;
using System.Web;
using System.Net;
using System.Net.Sockets;
using Famliy.Finance.Models;

namespace Famliy.Finance.BLL
{
    /// <summary>
    /// 接口log
    /// </summary>
    public class LogBusiness
    {
        SysLogService logService = new SysLogService();
        public static readonly LogBusiness Instance = new LogBusiness();
        #region Basic Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(sys_log log)
        {
            string connStr = string.Empty;
            logService.Add(log);
        }

        #endregion

        #region Method

        /// <summary>
        /// 正常写日志；文件和数据库都写。
        /// </summary>
        /// <param name="logInfo"></param>
        public void WriteLog(sys_log logInfo, LogMsgLevel logMsgLevel)
        {
            #region logInfo参数补充
            if (logInfo == null)
                logInfo = new sys_log();

            logInfo.create_date = DateTime.Now;
            logInfo.modify_date = logInfo.create_date;
            logInfo.status = 0;
            logInfo.local_ip = GetLocalIP();
            logInfo.visitor_ip = GetClientIP();
            #endregion

            //log4net按配置等级来写文本日志
            LogHelper.Write(JsonHelper.ToJson(logInfo), logMsgLevel);

            //判断数据库日志级别，写数据库日志

            var task = Task.Factory.StartNew(
                            () =>
                            {
                                try
                                {
                                    logService.Add(logInfo);
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Write("Famliy.Finance.BLL.LogBusiness.WriteLog() error.", ex);
                                }
                            });
        }
             

        public static string GetClientIP()
        {
            if (HttpContext.Current != null)
            {
                string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                return result;
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetLocalIP()
        {
            try
            {
                string cacheKey = "KEY_LOCALIP";
                string cacheIp = GW.Utils.ParseHelper.Parse<string>(GW.Utils.Web.CacheHelper.GetCache(cacheKey), string.Empty);
                if (string.IsNullOrEmpty(cacheIp))
                {
                    string HostName = Dns.GetHostName(); //得到主机名
                    IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                    for (int i = 0; i < IpEntry.AddressList.Length; i++)
                    {
                        //从IP地址列表中筛选出IPv4类型的IP地址
                        //AddressFamily.InterNetwork表示此IP为IPv4,
                        //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                        if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            cacheIp = IpEntry.AddressList[i].ToString();
                            break;
                        }
                    }
                    GW.Utils.Web.CacheHelper.SetCache(cacheKey, cacheIp);
                }
                return cacheIp;
            }
            catch (Exception ex)
            {
                LogHelper.Write("GW.Bank.BLL.BankInterfaceLogBLL.GetLocalIP() error.", ex);
                return string.Empty;
            }
        }
       
        public static LogMsgLevel GetLogMsgLevel(GW.Utils.LogMsgLevel logLevel)
        {
            return (LogMsgLevel)ParseHelper.Parse<int>(logLevel);
        }

        #endregion


    }
}
