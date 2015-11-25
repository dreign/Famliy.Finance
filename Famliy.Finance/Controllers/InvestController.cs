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
    public class InvestController : BaseController
    {
        BankAccountService accountService = new BankAccountService();
        BankOperateLogService logService = new BankOperateLogService();
        SysSubjectService subjectService = new SysSubjectService();

        /// <summary>
        /// 初始化面包屑导航条
        /// </summary>
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "理财", Controll = "Home", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "理财投资", Controll = "Invest", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: Record       
        public ActionResult Index()
        {
            //面包屑导航条
            InitBreadCrumbs();
            string user_name = User.Identity.Name;
            bank_operate_log log = new bank_operate_log();
            log.user_name = User.Identity.Name;
            log.create_date = DateTime.Now;
            var list = subjectService.FindList<sys_subject>(s => s.parents_id == 4);
            ViewBag.SubjectList1 = new SelectList(list, "subject_id", "subject_name");
            return View(log);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "user_name,subject_id,subject_remark,product_name,product_number,money,pay_way,remark,create_date")] bank_operate_log operate_log)
        {
            //面包屑导航条
            //InitBreadCrumbs();
            string user_name = User.Identity.Name;
            bank_operate_log log = new bank_operate_log();
            log.user_name = user_name;
            log.create_date = DateTime.Now;
            //log.modify_date = DateTime.Now;//用来展示创建日期的时间部分
            var list = subjectService.FindList<sys_subject>(s => s.parents_id == 4);
            ViewBag.SubjectList1 = new SelectList(list, "subject_id", "subject_name");

            var subject = subjectService.Find(s => s.subject_id == operate_log.subject_id);
            if (string.IsNullOrEmpty(operate_log.subject_remark))
            {
                operate_log.subject_remark = subject.subject_name;
            }
            operate_log.user_name = user_name;
            operate_log.sys_subject = subject;
            operate_log.order_amount = operate_log.money;
            if (string.IsNullOrEmpty(operate_log.remark))
            {
                operate_log.remark = "投资";
            }
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
            operate_log.status = 0;

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
                var mybank = accountService.Find(a => a.user_name == user_name);
                if (mybank != null)
                {
                    operate_log.history_total = mybank.money;
                    mybank.money += operate_log.money;

                    bool result = accountService.Update(mybank, true);
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

        //string url = "http://apis.baidu.com/apistore/stockservice/hkstock";
        //string param = "stockid=00168&list=1";
        //string result = request(url, param);

        /// <summary>
        /// 发送HTTP请求
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="param">请求的参数</param>
        /// <returns>请求结果</returns>
        public ActionResult GetHQ(string market, string stockcodes)
        {
            string strData = string.Empty;
            string url = ConfigBusiness.StockAUrl;
            if(market.ToLower()=="hk")
                url = ConfigBusiness.StockHKUrl;
            else if (market.ToLower() == "us")
                url = ConfigBusiness.StockUSUrl;

            if(!string.IsNullOrEmpty(stockcodes))
            {
                url += "?stockid=" + stockcodes.ToLower() + "&list=1";
                strData = Common.Utils.GetDataByHttpRequest(url, "apikey", ConfigBusiness.StockApikey);
            }

            return Content(strData);
        }


    }
}