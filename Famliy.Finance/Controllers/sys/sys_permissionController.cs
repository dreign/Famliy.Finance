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

namespace Famliy.Finance.Controllers
{
    public class sys_permissionController : BaseController
    {
        private BankModel db = new BankModel();
        public void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "权限管理", Controll = "sys_permission", Action = "Index" });
            ViewData["NavList"] = navList;
        }
       
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
            return View(db.sys_permissions.ToList().ToPagedList(pageNumber, pageSize));
        }
        // GET: sys_permission/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_permission sys_permission = db.sys_permissions.Find(id);
            if (sys_permission == null)
            {
                return HttpNotFound();
            }
            return View(sys_permission);
        }

        // GET: sys_permission/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: sys_permission/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "permission_id,permission_name,permission_url,create_date,modify_date,status")] sys_permission sys_permission)
        {
            if (ModelState.IsValid)
            {
                db.sys_permissions.Add(sys_permission);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sys_permission);
        }

        // GET: sys_permission/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_permission sys_permission = db.sys_permissions.Find(id);
            if (sys_permission == null)
            {
                return HttpNotFound();
            }
            return View(sys_permission);
        }

        // POST: sys_permission/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "permission_id,permission_name,permission_url,create_date,modify_date,status")] sys_permission sys_permission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_permission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sys_permission);
        }

        // GET: sys_permission/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_permission sys_permission = db.sys_permissions.Find(id);
            if (sys_permission == null)
            {
                return HttpNotFound();
            }
            return View(sys_permission);
        }

        // POST: sys_permission/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sys_permission sys_permission = db.sys_permissions.Find(id);
            db.sys_permissions.Remove(sys_permission);
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
