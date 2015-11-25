using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Famliy.Finance.Models;

namespace Famliy.Finance.Controllers
{
    public class bank_recordController : BaseController
    {
        private BankModel db = new BankModel();

        // GET: bank_record
        public ActionResult Index()
        {
            return View(db.bank_records.ToList());
        }

        // GET: bank_record/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_record bank_record = db.bank_records.Find(id);
            if (bank_record == null)
            {
                return HttpNotFound();
            }
            return View(bank_record);
        }

        // GET: bank_record/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bank_record/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,user_id,user_name,family_id,period_date,opening_balance,closed_balance,balance,closed,closing_time,create_date,modify_date,status")] bank_record bank_record)
        {
            if (ModelState.IsValid)
            {
                db.bank_records.Add(bank_record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bank_record);
        }

        // GET: bank_record/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_record bank_record = db.bank_records.Find(id);
            if (bank_record == null)
            {
                return HttpNotFound();
            }
            return View(bank_record);
        }

        // POST: bank_record/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,user_id,user_name,family_id,period_date,opening_balance,closed_balance,balance,closed,closing_time,create_date,modify_date,status")] bank_record bank_record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank_record).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bank_record);
        }

        // GET: bank_record/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_record bank_record = db.bank_records.Find(id);
            if (bank_record == null)
            {
                return HttpNotFound();
            }
            return View(bank_record);
        }

        // POST: bank_record/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            bank_record bank_record = db.bank_records.Find(id);
            db.bank_records.Remove(bank_record);
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
