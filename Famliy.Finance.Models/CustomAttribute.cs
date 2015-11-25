using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Famliy.Finance.Models
{
    public class CustomAttribute
    {
    }

    /// <summary>
    /// 自定义唯一约束
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class UniqueAttribute : ValidationAttribute
    {
        public override Boolean IsValid(Object value)
        {
            //校验数据库是否存在当前Key
            return true;
        }
    }

    //public class SomeMsgAttribute : FilterAttribute, IResultFilter
    //{
    //    public void OnResultExecuted(ResultExecutedContext filterContext)
    //    {
    //    }

    //    public void OnResultExecuting(ResultExecutingContext filterContext)
    //    {
    //        filterContext.Controller.ViewBag.Msg = "Hello";
    //    }
    //}
    public class MyFilterAttribute : ActionFilterAttribute
    {
        public string Message { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            filterContext.HttpContext.Response.Write("Action执行之前" + Message + "<br />");
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            filterContext.HttpContext.Response.Write("Action执行之后" + Message + "<br />");
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
            filterContext.HttpContext.Response.Write("返回Result之前" + Message + "<br />");
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            filterContext.HttpContext.Response.Write("返回Result之后" + Message + "<br />");
        }
    }

    public class CheckLogin : ActionFilterAttribute
    {
        //在Action执行之前　乱了点，其实只是判断Cookie用户名密码正不正确而已而已。
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //HttpCookieCollection CookieCollect = System.Web.HttpContext.Current.Request.Cookies; if (CookieCollect["username"] == null || CookieCollect["password"] == null)
            //{
            //    filterContext.Result = new RedirectResult("/Home/Login");
            //}
            //else
            //{
            //    if (CookieCollect["username"].Value != "admin" && CookieCollect["password"].Value != "123456")
            //    {
            //        filterContext.Result = new RedirectResult("/Home/Login");
            //    }
            //}
        }
    }//本示例贪图方便，将要跳转到的Action放在同一个Controller下了，如果将过滤器放到Controller类顶部，则永远也跳不到这个LoginAction。
}
