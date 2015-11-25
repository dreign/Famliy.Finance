using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using Famliy.Finance.Models;
using Famliy.Finance.BLL;
using Famliy.Finance.Common;
using System.Web.Security;
using System.Drawing;
using GW.Utils;

namespace Famliy.Finance.Controllers
{
    public class MemberController : Controller
    {
        #region 属性
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        #endregion

        #region 登录
        // GET: Member     

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.LoginState = "欢迎登录";
            //ViewBag.Title = "管理系统：登录";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            //BankModel bank = new BankModel();
            //DBInit.Instance.Init(bank);

            if (!ModelState.IsValid)
            {            
                return View(model);
            }
            //var user = await MemberBLL.Instance.Find_SysUser(model.Email);
            //if (user == null) ModelState.AddModelError("UserName", "用户名不存在");

            var user = await MemberBLL.Instance.Get_SysUser(model.Email, Utils.MD5Encrypt(model.Password));
            if (user != null )
            {
                ViewBag.LoginState = "已登录";
                //FormsAuthentication.SetAuthCookie(model.Email, model.RememberMe);
                var identity = MemberBLL.Instance.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = model.RememberMe }, identity);

                Session.Add(ConfigBusiness.Session_User_Key, user);

                sys_log logInfo = new sys_log();
                logInfo.user_id = user.user_id;
                logInfo.user_name = user.user_name;
                logInfo.interface_name = "Login";
                logInfo.interface_param = JsonHelper.ToJson(model);
                logInfo.result = ViewBag.LoginState;
                LogBusiness.Instance.WriteLog(logInfo, GW.Utils.LogMsgLevel.Info);

                return RedirectToLocal(returnUrl);

            }
            else { ViewBag.LoginState = "未登录"; }

            AddErrors(new IdentityResult(new string[] { "登录失败" }));

            sys_log logInfo2 = new sys_log();
            logInfo2.user_id = user.user_id;
            logInfo2.user_name = user.user_name;
            logInfo2.interface_name = "Login";
            logInfo2.interface_param = JsonHelper.ToJson(model);
            logInfo2.result = ViewBag.LoginState;
            LogBusiness.Instance.WriteLog(logInfo2,GW.Utils.LogMsgLevel.Info);        
            return View(model);
        }

        #endregion

        #region 注册
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            //ViewBag.Register = "欢迎注册";
            if (ModelState.IsValid)
            {
                if (TempData["VerificationCode"] == null || TempData["VerificationCode"].ToString() != model.VerificationCode.ToUpper())
                {
                    ModelState.AddModelError("VerificationCode", "验证码不正确");
                    return View(model);
                }

                var tuser = await MemberBLL.Instance.Find_SysUser(model.Email);
                if (tuser != null && tuser.user_name == model.Email)
                    ModelState.AddModelError("UserName", "用户名已存在");

                var user = new Famliy.Finance.Models.sys_user
                {
                    user_name = model.Email,
                    email = model.Email,
                    password = Utils.MD5Encrypt(model.Password),
                    nick_name = model.NickName,
                    sex = model.Sex,
                    birthday = model.Birthday
                    //,sys_user_roles = new List<sys_user_role>(),
                    //bank_family = new bank_family()
            };                

                var item = await MemberBLL.Instance.Add_SysUser(user);
                if (item != null)
                {
                    sys_log logInfo = new sys_log();
                    logInfo.user_id = user.user_id;
                    logInfo.user_name = user.user_name;
                    logInfo.interface_name = "Register";
                    logInfo.interface_param = JsonHelper.ToJson(model);
                    logInfo.result = "OK";
                    LogBusiness.Instance.WriteLog(logInfo, GW.Utils.LogMsgLevel.Info);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    sys_log logInfo = new sys_log();
                    logInfo.user_id = user.user_id;
                    logInfo.user_name = user.user_name;
                    logInfo.interface_name = "Register";
                    logInfo.interface_param = JsonHelper.ToJson(model);
                    logInfo.result = "Error";
                    LogBusiness.Instance.WriteLog(logInfo, GW.Utils.LogMsgLevel.Info);


                    AddErrors(new IdentityResult(new string[] { "注册失败" }));
                }
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        #endregion

        #region 登出
        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect(Url.Content("~/"));
        }
        #endregion

        /// <summary>
        /// 显示资料
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Details()
        {
            var user = await MemberBLL.Instance.Find_SysUser(User.Identity.Name);
            return View(user);
        }

        #region Index
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 私有方法

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerificationCode()
        {
            string verificationCode = Utils.CreateVerificationText(4);
            Bitmap _img = Utils.CreateVerificationImage(verificationCode, 160, 30);
            _img.Save(Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            TempData["VerificationCode"] = verificationCode.ToUpper();
            return null;
        }

        //[HttpPost]
        //public ActionResult Register(FormCollection fc)
        //{
        //    //获取表单数据
        //    string email = fc["inputEmail3"];
        //    string password = fc["inputPassword3"];

        //    //进行下一步处理，这里先改下文字
        //    ViewBag.LoginState = "注册账号 " + email;
        //    return View();
        //}
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        #endregion
    }
}