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
    public class RecordController : BaseController
    {
        BankAccountService accountService = new BankAccountService();
        BankOperateLogService logService = new BankOperateLogService();
        SysSubjectService subjectService = new SysSubjectService();

        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "记账", Controll = "Record", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: Record       
        public ActionResult Index()
        {
            string user_name = User.Identity.Name;
            bank_operate_log log = new bank_operate_log();
            log.user_name = User.Identity.Name;
            log.create_date = DateTime.Now; 
            var list = subjectService.FindList<sys_subject>(s => s.subject_level == 3);
            ViewBag.SubjectList1 = new SelectList(list,"subject_id", "subject_name");
            return View(log);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "user_name,subject_id,subject_remark,money,pay_way,remark,create_date")] bank_operate_log operate_log)
        {
            string user_name = User.Identity.Name;
            bank_operate_log log = new bank_operate_log();
            log.user_name = user_name;
            log.create_date = DateTime.Now;
            //log.modify_date = DateTime.Now;//用来展示创建日期的时间部分
            var list = subjectService.FindList<sys_subject>(s => s.subject_level == 3);
            ViewBag.SubjectList1 = new SelectList(list, "subject_id", "subject_name");

            var subject = subjectService.Find(s => s.subject_id == operate_log.subject_id);
            if (string.IsNullOrEmpty(operate_log.subject_remark))
            {
                operate_log.subject_remark = subject.subject_name;
            }
            operate_log.user_name = user_name;
            operate_log.sys_subject = subject;
            //operate_log.create_date = GW.Utils.ParseHelper.Parse<DateTime>(operate_log.create_date.Value.ToString("yyyy-MM-dd")+Request["LogTime"]);
            operate_log.create_date = new DateTime(
                operate_log.create_date.Value.Year,
                operate_log.create_date.Value.Month,
                operate_log.create_date.Value.Day,
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                DateTime.Now.Second,
                DateTime.Now.Millisecond
                );
            operate_log.modify_date = DateTime.Now;
            operate_log.status=0;
          
            if (ModelState.IsValid)
            {
                if (operate_log.money == 0)
                {
                    ModelState.AddModelError("", "请输入金额");
                    return View(operate_log);
                }
                if (operate_log.create_date == null)
                {
                    ModelState.AddModelError("", "请选择时间");
                    return View(operate_log);
                }
                var mybank=accountService.Find(a=>a.user_name==user_name);
                if (mybank != null)
                {
                    operate_log.history_total = mybank.money;
                    mybank.money += operate_log.money;
                    bool result=accountService.Update(mybank, true);
                    if (result)
                    {
                        logService.Add(operate_log);
                        Response.Write("<script>alert('提交成功！');</script>");
                        //Response.Write("<script>showok();</script>");
                    }
                    else
                    {
                        ModelState.AddModelError("", "更新资金账户失败");
                        return View(operate_log);
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "找不到资金帐号");
                    return View(operate_log);
                }             
            }
            return View(log);
        }
    }
}