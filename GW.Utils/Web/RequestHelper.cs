using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Utils
{
    public class RequestHelper
    {
       public static string GetWebClientIp()  
       {  
           string userIP = "未获取用户IP";  
  
           try  
           {  
               if (System.Web.HttpContext.Current == null  
           || System.Web.HttpContext.Current.Request == null  
           || System.Web.HttpContext.Current.Request.ServerVariables == null)  
                   return "";  
  
               string CustomerIP = "";  
  
               //CDN加速后取到的IP   
               CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];  
               if (!string.IsNullOrEmpty(CustomerIP))  
               {  
                   return CustomerIP;  
               }  
  
               CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];  
  
  
               if (!String.IsNullOrEmpty(CustomerIP))  
                   return CustomerIP;  
  
               if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  
               {  
                   CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];  
                   if (CustomerIP == null)  
                       CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];  
               }  
               else  
               {
                   CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];  
  
               }  
  
               if (string.Compare(CustomerIP, "unknown", true) == 0)  
                   return System.Web.HttpContext.Current.Request.UserHostAddress;  
               return CustomerIP;  
           }  
           catch { }  
  
           return userIP;  
       }  
    }
}
