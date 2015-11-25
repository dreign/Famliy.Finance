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
    public class sys_user_roleController : BaseController
    {
        private BankModel db = new BankModel();

        // GET: sys_user_role
        public ActionResult Index()
        {
            var sys_user_roles = db.sys_user_roles.Include(s => s.sys_role).Include(s => s.sys_user);
            return View(sys_user_roles.ToList());
        }

        // GET: sys_user_role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_role sys_user_role = db.sys_user_roles.Find(id);
            if (sys_user_role == null)
            {
                return HttpNotFound();
            }
            return View(sys_user_role);
        }

        // GET: sys_user_role/Create
        public ActionResult Create()
        {
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name");
            ViewBag.user_name = new SelectList(db.sys_users, "user_name", "password");
            return View();
        }

        // POST: sys_user_role/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,role_id,user_name,create_date,modify_date,status")] sys_user_role sys_user_role)
        {
            if (ModelState.IsValid)
            {
                db.sys_user_roles.Add(sys_user_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_user_role.role_id);
            ViewBag.user_name = new SelectList(db.sys_users, "user_name", "password", sys_user_role.user_name);
            return View(sys_user_role);
        }

        // GET: sys_user_role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_role sys_user_role = db.sys_user_roles.Find(id);
            if (sys_user_role == null)
            {
                return HttpNotFound();
            }
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_user_role.role_id);
            ViewBag.user_name = new SelectList(db.sys_users, "user_name", "password", sys_user_role.user_name);
            return View(sys_user_role);
        }

        // POST: sys_user_role/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,role_id,user_name,create_date,modify_date,status")] sys_user_role sys_user_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_user_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_user_role.role_id);
            ViewBag.user_name = new SelectList(db.sys_users, "user_name", "password", sys_user_role.user_name);
            return View(sys_user_role);
        }

        // GET: sys_user_role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_user_role sys_user_role = db.sys_user_roles.Find(id);
            if (sys_user_role == null)
            {
                return HttpNotFound();
            }
            return View(sys_user_role);
        }

        // POST: sys_user_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sys_user_role sys_user_role = db.sys_user_roles.Find(id);
            db.sys_user_roles.Remove(sys_user_role);
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
