using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Famliy.Finance.Models;
using PagedList;
using Famliy.Finance.Common;
using Famliy.Finance.BLL;

namespace Famliy.Finance.Controllers
{
    public class bank_operate_logController : BaseController
    {
        BankOperateLogService logService = new BankOperateLogService();
        SysSubjectService subjectService = new SysSubjectService();

        private BankModel db = new BankModel();
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "资金明细", Controll = "bank_account", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: bank_operate_log       
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
            if (string.IsNullOrEmpty(searchString) && !string.IsNullOrEmpty(CurrentUser.user_name))
                searchString = CurrentUser.user_name;
            ViewBag.CurrentFilter = searchString;

            return View(db.bank_operate_logs.Include(b => b.sys_subject).Where(l=>l.user_name==searchString).ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: bank_operate_log/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_operate_log bank_operate_log = db.bank_operate_logs.Find(id);
            if (bank_operate_log == null)
            {
                return HttpNotFound();
            }
            return View(bank_operate_log);
        }

        // GET: bank_operate_log/Create
        public ActionResult Create()
        {
            ViewBag.subject_id = new SelectList(db.sys_subjects, "subject_id", "subject_name");
            return View();
        }

        // POST: bank_operate_log/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,user_name,money,history_total,subject_id,subject_remark,order_id,product_name,product_number,order_amount,pay_way,remark,create_date,modify_date,status")] bank_operate_log bank_operate_log)
        {
            if (ModelState.IsValid)
            {
                db.bank_operate_logs.Add(bank_operate_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.subject_id = new SelectList(db.sys_subjects, "subject_id", "subject_name", bank_operate_log.subject_id);
            return View(bank_operate_log);
        }

        // GET: bank_operate_log/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var log = logService.Find((int)id);
            //bank_operate_log bank_operate_log = db.bank_operate_logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            ViewBag.subject_id = new SelectList(db.sys_subjects, "subject_id", "subject_name", log.subject_id);
            return View(log);
        }

        // POST: bank_operate_log/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,user_name,money,history_total,subject_id,subject_remark,order_id,product_name,product_number,order_amount,pay_way,remark,create_date,modify_date,status")] bank_operate_log log)
        {
            if (ModelState.IsValid)
            {
                bank_operate_log old = logService.Find((int)log.id);
                old.user_id = log.user_id;
                old.user_name = log.user_name;
                old.money = log.money;
                old.history_total = log.history_total;
                old.subject_id = log.subject_id;
                old.subject_remark = log.subject_remark;
                old.order_id = log.order_id;
                old.product_name = log.product_name;
                old.product_number = log.product_number;
                old.order_amount = log.order_amount;
                old.pay_way = log.pay_way;
                old.remark = log.remark;
                old.create_date = log.create_date;
                old.modify_date = DateTime.Now;
                old.status = log.status;

                old.sys_subject = subjectService.Find((int)log.subject_id);
                //logService.Update(log, true);
                db.Entry(old).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.subject_id = new SelectList(db.sys_subjects, "subject_id", "subject_name", log.subject_id);
            return View(log);
        }

        // GET: bank_operate_log/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_operate_log bank_operate_log = db.bank_operate_logs.Find(id);
            if (bank_operate_log == null)
            {
                return HttpNotFound();
            }
            return View(bank_operate_log);
        }

        // POST: bank_operate_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            bank_operate_log bank_operate_log = db.bank_operate_logs.Find(id);
            db.bank_operate_logs.Remove(bank_operate_log);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
