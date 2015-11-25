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
    public class sys_user_logController : BaseController
    {
        private BankModel db = new BankModel();

        // GET: sys_user_log
        public ActionResult Index()
        {
            return View(db.sys_user_logs.ToList());
        }

        // GET: sys_user_log/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_log sys_user_log = db.sys_user_logs.Find(id);
            if (sys_user_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_user_log);
        }

        // GET: sys_user_log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: sys_user_log/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "log_id,user_id,user_name,action,action_desc,login_ip,login_time,create_date,modify_date,status")] sys_user_log sys_user_log)
        {
            if (ModelState.IsValid)
            {
                db.sys_user_logs.Add(sys_user_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sys_user_log);
        }

        // GET: sys_user_log/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_log sys_user_log = db.sys_user_logs.Find(id);
            if (sys_user_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_user_log);
        }

        // POST: sys_user_log/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "log_id,user_id,user_name,action,action_desc,login_ip,login_time,create_date,modify_date,status")] sys_user_log sys_user_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_user_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sys_user_log);
        }

        // GET: sys_user_log/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_log sys_user_log = db.sys_user_logs.Find(id);
            if (sys_user_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_user_log);
        }

        // POST: sys_user_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sys_user_log sys_user_log = db.sys_user_logs.Find(id);
            db.sys_user_logs.Remove(sys_user_log);
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
