using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Famliy.Finance
{
    //    新建一个CustomAuthorizeAttribute类，使这个类继承于AuthorizeAttribute。
    //打开AuthorizeAttribute查看下方法说明，我们只需要重写AuthorizeCore和OnAuthorization就能达到我们的目的。
    //    当用户请求一个Action时，会调用OnAuthorization方法，该方法中GetRoles.GetActionRoles(actionName, controllerName);
    //根据Controller和Action去查找当前Action需要具有的角色类型，获得Action的Roles以后，
    //在AuthorizeCore中与用户的角色进行比对Roles.Any(httpContext.User.IsInRole)，
    //如果没有相应权限则返回false，程序就会自动跳转到登录页面。
    public class CustomAuthorizeAttribute: System.Web.Mvc.AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            if (Roles == null)
            {
                return true;
            }
            if (Roles.Length == 0)
            {
                return true;
            }
            if (Roles.Any(httpContext.User.IsInRole))
            {
                return true;
            }
            return false;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            string roles = "";// GetRoles.GetActionRoles(actionName, controllerName);
            if (!string.IsNullOrWhiteSpace(roles))
            {
                this.Roles = roles.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            base.OnAuthorization(filterContext);
        }
    }
}