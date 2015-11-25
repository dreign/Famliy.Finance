using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Famliy.Finance.BLL
{
    public class ActionFilter
    {
        /// <summary>
        /// 写一个过滤器，在需要做身份验证的action上加上过滤器就可以了
        ///  [CheckUserFilter]
        /// </summary>
        public class CheckUserFilter : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                base.OnActionExecuting(filterContext);
                //var user = (sys_user)Newtonsoft.Json.JsonConvert.DeserializeObject<sys_user>(filterContext.HttpContext.Session["SysUser"].ToString());
               if (filterContext.HttpContext.Session["SysUser"] != null)
                {
                    return;
                }
                //else if (CookieManage.GetCookie("login") != null)
                //{ }
            }
        }

        /// <summary>
        /// [LoggingFilter]
        /// </summary>
            public class LoggingFilterAttribute : ActionFilterAttribute
        {
            
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                filterContext.HttpContext.Trace.Write("Starting: " +
                filterContext.ActionDescriptor.ActionName);
            }

            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Exception != null)
                {
                    filterContext.HttpContext.Trace.Write("Exception thrown");
                }
            }
        }

    }

    /// <summary>
    ///  判断权限为超级管理员的情况下进入此控制器
    ///  [TestAuthorize(roleName = "超级管理员")]
    /// </summary>
    public class TestAuthorizeAttribute : AuthorizeAttribute
    {
        public string roleName = "";

        //权限进入
        public override void OnAuthorization(AuthorizationContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }
        //判断权限
        protected override bool AuthorizeCore(HttpContextBase actionContext)
        {
            if (roleName == "管理员")
                return true;
            return false;
        }

        //权限为false执行内容
        protected override void HandleUnauthorizedRequest(AuthorizationContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }
    }
}
