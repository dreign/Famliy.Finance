//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   环境处理，提供有关环境的属性和处理方法
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
using System.Web;
using System.Web.Configuration;

namespace GW.Utils
{
    /// <summary>
    /// 环境处理，提供有关环境的属性和处理方法
    /// </summary>
    public class EnvironmentHelper
    {
        /// <summary>
        /// 获取真实IP
        /// </summary>
        public static string IPAddress
        {
            get
            {
                string result = string.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(result))
                {
                    //可能有代理
                    if (result.IndexOf(".") == -1)
                    {
                        //没有"."肯定是非IPv4格式
                        result = null;
                    }
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有","估计有多个代理，取第一个不是内网的IP
                            result = result.Replace(" ", "").Replace("'", ""); 
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (StringHelper.IsValidIp(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    //找到不是内网的地址
                                    return temparyip[i];
                                }
                            }
                        }
                        else if (StringHelper.IsValidIp(result)) //代理即是IP格式 
                        {
                            return result; 
                        }
                        else
                            result = null;  //代理中的内容 非IP，取IP  
                    }
                }
                string IpAddress = (
                    !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] )) 
                    ? 
                    HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] 
                    : 
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;
                return result;
            }
        }

        /// <summary>
        /// 获取WEB应用程序虚拟目录
        /// </summary>
        public static string RootVPath
        {
            get
            {
                return HttpRuntime.AppDomainAppVirtualPath;
            }
        }

        /// <summary>
        ///  获取Web应用程序物理根目录
        /// </summary>
        public static string PhyAppPath
        {
            get
            {
                return HttpRuntime.AppDomainAppPath;
            }
        }

        /// <summary>
        /// 获取服务器的机器名
        /// </summary>
        public static string ComputeName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        /// <summary>
        /// 获取bin目录路径
        /// </summary>
        public static string BinPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 当使用基本验证模式时，获取客户在密码对话框中输入的密码
        /// </summary>
        public static string GetAuthPwd
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["Auth_Password"];
            }
        }
        

        /// <summary>
        /// 获取浏览器类型
        /// </summary>
        /// <returns>Firefox,IE,Opera,Safari,Wap</returns>
        public static string GetBrowseType()
        {
            string browseType = HttpContext.Current.Request.ServerVariables["Http_User_Agent"];

            if (browseType.IndexOf("Firefox") != -1)
                browseType = "Firefox";
            else if (browseType.IndexOf("MSIE") != -1)
                browseType = "IE";
            else if (browseType.ToLower().IndexOf("opera") != -1)
                browseType = "Opera";
            else if (browseType.ToLower().IndexOf("safari") != -1)
                browseType = "Safari";
            else if (browseType.ToLower().IndexOf("wap") != -1)
                browseType = "Wap";
            return browseType;
        }

        /// <summary>
        /// 获取自定义错误配置节
        /// </summary>
        /// <returns>CustomErrorsSection</returns>
        public static CustomErrorsSection GetCustomErrorSection()
        {
            return (CustomErrorsSection)HttpContext.Current.GetSection("system.web/customErrors");
        }

        /// <summary>
        /// 根据CustomeErrors配置获取错误处理页面URL
        /// </summary>
        /// <param name="customErrorsSection">错误配置节</param>
        /// <returns>错误处理页面URL</returns>
        public static string GetCustomErrorURL(CustomErrorsSection customErrorsSection)
        {
            string redirectURL = string.Empty;

            if (customErrorsSection.Mode == CustomErrorsMode.On || (customErrorsSection.Mode == CustomErrorsMode.RemoteOnly && !HttpContext.Current.Request.IsLocal))
            {
                redirectURL = customErrorsSection.DefaultRedirect;
                CustomError error = customErrorsSection.Errors[Convert.ToString(HttpContext.Current.Response.StatusCode)];
                if (null != error)
                    redirectURL = error.Redirect;
            }
            return redirectURL;
        }

        /// <summary>
        /// 把错误信息发送并跳转到错误页面
        /// </summary>
        /// <param name="ex">异常</param>
        public static void GoToErrURLAndSendErrMsg(Exception ex)
        {
            CustomErrorsSection cs = EnvironmentHelper.GetCustomErrorSection();
            string url = EnvironmentHelper.GetCustomErrorURL(cs);
            //System.Web.HttpContext.Current.Application["errmsginfo"] = ex.Message;
            System.Web.HttpContext.Current.Response.Redirect("ErrTest.aspx?errmsginfo=" + ex.Message);
        }

        /// <summary>
        /// 把错误信息发送并跳转到错误页面
        /// </summary>
        /// <param name="ex">异常</param>
        public static void GoToErrURLAndSendErrMsg(string ex)
        {
            CustomErrorsSection cs = EnvironmentHelper.GetCustomErrorSection();
            string url = EnvironmentHelper.GetCustomErrorURL(cs);
            //System.Web.HttpContext.Current.Application["errmsginfo"] = ex;
            System.Web.HttpContext.Current.Response.Redirect("ErrTest.aspx?errmsginfo=" + ex);
        }

        /// <summary>
        /// 获取URL中的参数值
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>传入的URL中的参数值</returns>
        public static string GetQueryParameter(string parameterName)
        {
            string result = string.Empty;

            if (HttpContext.Current.Request.Params[parameterName] == null)
            {
                throw new Exception("非法的参数名：" + parameterName);
            }
            else
            {
                result = HttpContext.Current.Request.Params[parameterName].ToString();
            }

            return result;
        }
    }   
}
