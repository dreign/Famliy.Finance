
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Famliy.Finance.Models;
using Famliy.Finance.BLL;
using GW.Utils;
using GW.Utils.Web;
using Famliy.Finance.Common;

namespace Famliy.Finance.Controllers
{    
    public class BaseController : Controller
    {
        private sys_user _user;
        private string userCacheKey = "Base.UserCache.Key.";
        private string logCacheKey = "Base.LogCache.Key.";
        //private BankFamilyService familyService = new BankFamilyService();
        FamilyService fservice = new FamilyService();
        BankOperateLogService logService = new BankOperateLogService();

        public sys_user CurrentUser
        {
            get
            {
                if (_user == null)
                {
                    if (User != null)
                    {
                        _user = (sys_user)CacheHelper.GetCache(userCacheKey + User.Identity.Name);
                    }
                }
                //if (_user == null)
                //{
                //    RedirectToAction("Index", "Login");
                //}
                return _user;
            }
            set
            {
                if (value != null)
                {
                    CacheHelper.SetCache(userCacheKey + User.Identity.Name, value);
                }
                _user = value;
            }
        }
      
        public BaseController()
        {
            InitBreadCrumbs();
        }

        public virtual void InitBreadCrumbs()
        { }
        //protected internal HttpNotFoundResult HttpNotFound();
        //public new HttpNotFoundResult HttpNotFound()
        //{
        //    HttpNotFoundResult result = new HttpNotFoundResult();
        //    return RedirectToAction("Page404", "Home");
        //}
        /// <summary>
        /// 重写基类在Action之前执行的方法
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            #region -----校验用户是否登录进入网站的-----
            base.OnActionExecuting(filterContext);
            CurrentUser = Session[ConfigBusiness.Session_User_Key] as sys_user;

            //检验用户是否已经登录，如果登录则不执行，否则则执行下面的跳转代码
            if (CurrentUser == null)
            {
                RedirectToAction("Index", "Login");
                //Response.Redirect("/Login/Index"); 
            }
            else
            {
                if (string.IsNullOrEmpty(CurrentUser.bank_family.family_name))
                {
                    CurrentUser.bank_family = fservice.FindFamliysByUser(CurrentUser.user_name).FirstOrDefault();
                }
                CacheHelper.SetCache(userCacheKey + User.Identity.Name, CurrentUser);
                ViewData["sys_user"] = CurrentUser;               
            }

            #endregion

            #region some

            //#region ------//留个接口------
            //if (CurrentUserInfo.UName == "admin")
            //{
            //    return;
            //}
            //#endregion

            /*

            #region -------检验用户是否有访问此地址的权利----
            //先将当前的请求，到权限表里面去找对应的数据
            //拿到当前请求的URL地址
            string requestUrl = filterContext.HttpContext.Request.Path;
            //拿到当前请求的类型
            string requestType = filterContext.HttpContext.Request.RequestType.ToLower().Equals("get") ? "HttpGet" : "HttpPost";
            //然后和权限表进行对比，如果取出来则通过请求，否则不通过
            //取出当前权限的数据
            var currentAction = _actioninfoService.LoadEntities(c => c.RequestUrl.Equals(requestUrl, StringComparison.InvariantCultureIgnoreCase) && c.RequestHttpType.Equals(requestType)).FirstOrDefault();
            //如果没有权限对应当前请求的话，直接干掉
            if (currentAction == null)
            {
                EndRequest();
            }
            //想去用户权限表里面查询有没有数据
            //分析第一条线路 UserInfo->R_UserInfo_ActionInfo->ActionInfo
            //拿到当前的用户信息
            var userCurrent = _userInfoService.LoadEntities(u => u.ID == CurrentUserInfo.ID).FirstOrDefault();
            var temp = (from r in userCurrent.R_UserInfo_ActionInfo
                        where r.ActionInfoID == currentAction.ID
                        select r).FirstOrDefault();
            if (temp != null)
            {
                if (temp.HasPermation)
                {
                    return;
                }
                else
                {
                    EndRequest();
                }
            }

            //分析第二条线路 UserInfo->ActionGroup->ActionInfo
            var groups = from n in userCurrent.ActionGroup //拿到当前用户所有的组
                         select n;
            //根据组信息遍历出权限信息  
            bool isPass = (from g in groups
                           from a in g.ActionInfo
                           select a.ID).Contains(currentAction.ID);
            if (isPass)   //11，23，34不包含4
            {
                return;
            }

            //分析第三条线路 分为两个
            //1)UserInfo->R_UserInfo_Role->Role->ActionInfo

            //先拿到用户对应的所有的角色
            var UserRoles = from r in userCurrent.R_UserInfo_Role
                            select r.Role;
            //拿到角色对应的所有权限
            var Rolesaction = (from r in UserRoles
                               from a in r.ActionInfo
                               select a.ID);
            if (Rolesaction.Contains(currentAction.ID))
            {
                return;
            }

            //2)UserInfo->R_UserInfo_Role->Role->ActionGroup->ActionInfo
            //拿到组信息
            var RoleGroupActions = from r in UserRoles
                                   from g in r.ActionGroup
                                   select g;
            //拿到所有的组信息
            var groupActions = from r in RoleGroupActions
                               from g in r.ActionInfo
                               select g.ID;
            if (groupActions.Contains(currentAction.ID))
            {
                return;
            }
            #endregion

    */
            #endregion

        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var logs = (List<bank_operate_log>)CacheHelper.GetCache(logCacheKey + User.Identity.Name);
            if (logs == null)
            {
                logs = logService.FindAll().OrderByDescending(f => f.create_date).Take(5).ToList();
                CacheHelper.SetCache(logCacheKey + User.Identity.Name, logs,DateTime.Now.AddMinutes(1));
            }
            ViewBag.LogList = logs;
        }

        public void EndRequest()
        {
            Response.Redirect("/Error.html");
        }
               
        public sys_user CheckUser()
        {
            var result = new sys_user();
            if (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                if (Session[ConfigBusiness.Session_User_Key] != null)
                {
                    //result = (sys_user)Newtonsoft.Json.JsonConvert.DeserializeObject<sys_user>(Session["SysUser"].ToString());
                    result = (sys_user)Session[ConfigBusiness.Session_User_Key];
                }
            }
            return result;
        }
        /*
        public sys_user CheckUserInCache()
        {
            var result = new sys_user();
            if (User != null && User.Identity != null && !string.IsNullOrEmpty(User.Identity.Name))
            {
                if (!string.IsNullOrEmpty(User.Identity.Name))
                {
                    string cacheKey = "Famliy.Finance.Base.UserCache.Key";
                    var user = (sys_user)CacheHelper.GetCache(cacheKey);
                    if (user == null)
                    {
                        var tuser = MemberBLL.Instance.Find_SysUser(User.Identity.Name);
                        if (tuser.Result != null)
                        {
                            CacheHelper.SetCache(cacheKey, tuser.Result);
                            ViewBag.BaseUser = tuser.Result;
                        }
                        return tuser.Result;
                    }
                }
            }
            return result;
        }       
        */
    }
}