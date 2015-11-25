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
    [Authorize]
    public class sys_subjectController : BaseController
    {
        private BankModel db = new BankModel();

        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "设置", Controll = "sys_subject", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "财务科目", Controll = "sys_subject", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: sys_subject
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

            return View(db.sys_subjects.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: sys_subject/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_subject sys_subject = db.sys_subjects.Find(id);
            if (sys_subject == null)
            {
                return HttpNotFound();
            }
            return View(sys_subject);
        }

        // GET: sys_subject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: sys_subject/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subject_id,subject_name,subject_level,subject_desc,money_unit,parents_id,create_date,modify_date,status")] sys_subject sys_subject)
        {
            if (ModelState.IsValid)
            {
                db.sys_subjects.Add(sys_subject);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sys_subject);
        }

        // GET: sys_subject/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_subject sys_subject = db.sys_subjects.Find(id);
            if (sys_subject == null)
            {
                return HttpNotFound();
            }
            return View(sys_subject);
        }

        // POST: sys_subject/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subject_id,subject_name,subject_level,subject_desc,money_unit,parents_id,create_date,modify_date,status")] sys_subject sys_subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sys_subject);
        }

        // GET: sys_subject/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_subject sys_subject = db.sys_subjects.Find(id);
            if (sys_subject == null)
            {
                return HttpNotFound();
            }
            return View(sys_subject);
        }

        // POST: sys_subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sys_subject sys_subject = db.sys_subjects.Find(id);
            db.sys_subjects.Remove(sys_subject);
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
