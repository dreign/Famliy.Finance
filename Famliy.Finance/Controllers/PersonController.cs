using Famliy.Finance.BLL;
using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Famliy.Finance.Common;

namespace Famliy.Finance.Controllers
{
    [Authorize]
    public class PersonController : BaseController
    {
        SysUserService userService = new SysUserService();
        BankAccountService accountService = new BankAccountService();
        BankOperateLogService logService = new BankOperateLogService();
       
        /// <summary>
        /// 初始化面包屑导航条
        /// </summary>
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "我的账户", Controll = "Person", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: Person
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            int pageSize = ConfigBusiness.PageSize;
            int pageNumber = (page ?? 1);
            int pageTotal = 0;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "last" ? "last_desc" : "last";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            string user_name = User.Identity.Name;
            //成员账户
            //var account = BankAccountService.Instance.FindBankAccountByUsers(user_name).FirstOrDefault();
            var account = accountService.FindList<bank_account>(a => a.user_name == user_name).FirstOrDefault();
            ViewData["bank_account"] = account;
            //流水
            /*
            //查询语法   
            var lquery =
                from e in db.bank_accounts
                where(e.user_name==user_name)
                select e;
            //方法语法   
            var lsort =
                db.bank_accounts
                .OrderBy(e => e.create_date)
                .Select(e => e);
*/
            //List<bank_operate_log> orperateList = BankAccountService.Instance.FindBankOperateLogByUser(user_name).ToList();
            var orperateList = logService.FindList<DateTime>(pageTotal,f => f.user_name == user_name, OrderType.Desc, f => f.create_date.Value);
            //var orperateList = BankOperateLogService.Instance.FindPageList<DateTime>(pageNumber,
            //   pageSize,
            //   out pageTotal,
            //   f => f.user_name == user_name,
            //   OrderType.Desc,
            //   f => f.create_date.Value);
            //账户信息
            var user = userService.Find(u => u.user_name == user_name);
            ViewData["sys_user"] = user;
            ViewBag.PageTotal = pageTotal;

            return View(orperateList.ToPagedList(pageNumber, pageSize));
            //return View(orperateList);
        }
    }
}