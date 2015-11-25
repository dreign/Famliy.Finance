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
    public class ReportController : BaseController
    {
        private BankAccountService accountService = new BankAccountService();
        private BankFamilyService familyService = new BankFamilyService();
        BankModel db = new BankModel();

        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "财务报表", Controll = "Report", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: Record       
        public ActionResult Index()
        {
            string user_name = User.Identity.Name;
          
            if (CurrentUser != null && CurrentUser.bank_family !=null)
            {
                var familyId = CurrentUser.bank_family.family_id;
                //计算家庭成员的资金汇总
                var bank_family = familyService.Find(f=>f.family_id== familyId);
                var bank_account = accountService.Find(a=>a.user_name==user_name);

                List<report_family_user> fu = db.report_family_users.Where(f => f.bank_family_family_id == familyId).ToList();

                List<report_family> reportFamily = db.report_familys.Where(f=>f.bank_family_family_id==familyId).ToList();
                List<report_family_day> reportFamilyDay = db.report_family_days.Where(f => f.bank_family_family_id == familyId).ToList();
                List<report_family_subject_day> reportFamilySubjectDay = db.report_family_subject_days.Where(f => f.bank_family_family_id == familyId).ToList();

                ViewBag.bank_family = bank_family;
                ViewBag.bank_account = bank_account;

                ViewBag.Family = reportFamily;
                ViewBag.FamilyDay = reportFamilyDay;
                ViewBag.FamilySubjectDay = reportFamilySubjectDay;
            }
            if (CurrentUser != null)
            {
                var userName = CurrentUser.user_name;
                List<report_user> reportUser = db.report_users.Where(f => f.user_name == userName).ToList();
                List<report_user_day> reportUserDay = db.report_user_days.Where(f => f.user_name == userName).ToList();
                List<report_user_subject_day> reportUserSubjectDay = db.report_user_subject_days.Where(f => f.user_name == userName).ToList();
                ViewBag.User = reportUser;
                ViewBag.UserDay = reportUserDay;
                ViewBag.UserSubjectDay = reportUserSubjectDay; 
            }
            return View();
        }
    }
}