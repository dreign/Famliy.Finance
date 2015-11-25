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
    public class sys_role_permissionController : BaseController
    {
        private BankModel db = new BankModel();

        // GET: sys_role_permission
        public ActionResult Index()
        {
            var sys_role_permissions = db.sys_role_permissions.Include(s => s.sys_permission).Include(s => s.sys_role);
            return View(sys_role_permissions.ToList());
        }

        // GET: sys_role_permission/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role_permission sys_role_permission = db.sys_role_permissions.Find(id);
            if (sys_role_permission == null)
            {
                return HttpNotFound();
            }
            return View(sys_role_permission);
        }

        // GET: sys_role_permission/Create
        public ActionResult Create()
        {
            ViewBag.permission_id = new SelectList(db.sys_permissions, "permission_id", "permission_name");
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name");
            return View();
        }

        // POST: sys_role_permission/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,role_id,permission_id,create_date,modify_date,status")] sys_role_permission sys_role_permission)
        {
            if (ModelState.IsValid)
            {
                db.sys_role_permissions.Add(sys_role_permission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.permission_id = new SelectList(db.sys_permissions, "permission_id", "permission_name", sys_role_permission.permission_id);
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_role_permission.role_id);
            return View(sys_role_permission);
        }

        // GET: sys_role_permission/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role_permission sys_role_permission = db.sys_role_permissions.Find(id);
            if (sys_role_permission == null)
            {
                return HttpNotFound();
            }
            ViewBag.permission_id = new SelectList(db.sys_permissions, "permission_id", "permission_name", sys_role_permission.permission_id);
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_role_permission.role_id);
            return View(sys_role_permission);
        }

        // POST: sys_role_permission/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,role_id,permission_id,create_date,modify_date,status")] sys_role_permission sys_role_permission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_role_permission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.permission_id = new SelectList(db.sys_permissions, "permission_id", "permission_name", sys_role_permission.permission_id);
            ViewBag.role_id = new SelectList(db.sys_roles, "role_id", "role_name", sys_role_permission.role_id);
            return View(sys_role_permission);
        }

        // GET: sys_role_permission/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role_permission sys_role_permission = db.sys_role_permissions.Find(id);
            if (sys_role_permission == null)
            {
                return HttpNotFound();
            }
            return View(sys_role_permission);
        }

        // POST: sys_role_permission/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sys_role_permission sys_role_permission = db.sys_role_permissions.Find(id);
            db.sys_role_permissions.Remove(sys_role_permission);
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
