using GW.Utils.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Famliy.Finance.BLL;
using Famliy.Finance.Models;
using Famliy.Finance.Common;
using GW.Utils;

namespace Famliy.Finance.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private BankAccountService accountService = new BankAccountService();
        private BankFamilyService familyService = new BankFamilyService();
        private FamilyService fService = new FamilyService();
        /// <summary>
        /// 初始化面包屑导航条
        /// </summary>
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "我的家庭", Controll = "Home", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        public ActionResult Index()
        {
            try
            {
                string user_name = User.Identity.Name;              
                //家庭信息
                List<bank_family> familyList = fService.FindFamliysByUser(user_name).ToList();
                //家庭成员
                List<sys_user> userList = new List<sys_user>();
                if (familyList.Count > 0)
                {                      
                    userList = familyList[0].sys_users.ToList();
                }
                else
                {
                    ViewData["bank_family"] = new bank_family();
                    return View(familyList);
                }

                ViewData["UserList"] = userList;
                string[] names = new string[userList.Count];
                for (var i = 0; i < userList.Count; i++)
                {
                    names[i] = userList[i].user_name;
                }
                //计算家庭成员的资金汇总
                var accountList = accountService.FindList<bank_account>(f => names.Contains(f.user_name));
           
                //家庭成员账户列表
                //Dictionary<string,bank_account> accountDict = accountService.FindBankAccountDictByUsers(names);
                Dictionary<string, bank_account> accountDict = new Dictionary<string, bank_account>();
                decimal total = 0;
                decimal money = 0;
                decimal dept = 0;

                foreach (var item in accountList)
                {
                    accountDict.Add(item.user_name, item);
                    total += item.money;//资产合计
                }
                familyList[0].assets_net = total;
                ViewData["bank_family"] = familyList[0];
                ViewData["bank_account"] = accountDict;
                //家庭成员混合流水
                List<bank_operate_log> orperateList = accountService.FindBankOperateLogByUserArray(names,ConfigBusiness.PageSize).ToList();
                if (familyList.Count > 0)
                {
                    ViewData["bank_operate_log"] = orperateList;
                }
                return View(familyList);
            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write("Famliy.Finance.Controllers.HomeController.Index", ex, LogMsgLevel.Info);
            }
            return View();
        }
        [ChildActionOnly]
        public ActionResult ShowOrperateLogWidget()
        {
            return PartialView("~/View/Shared/_OperateLogPartial.cshtml");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }       

        public ActionResult Page500()
        {           
            return View();
        }
        public ActionResult Page404()
        {          
            return View();
        }
    }
}