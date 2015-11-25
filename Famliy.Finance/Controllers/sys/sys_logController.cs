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
    public class sys_logController : BaseController
    {
        private BankModel db = new BankModel();

        public void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "设置", Controll = "sys_subject", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "系统日志", Controll = "sys_log", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: sys_log
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
            return View(db.sys_logs.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: sys_log/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_log sys_log = db.sys_logs.Find(id);
            if (sys_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_log);
        }

        // GET: sys_log/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: sys_log/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,log_name,user_id,user_name,interface_name,interface_param,interface_type,interface_dir,local_ip,visitor_ip,result,interface_status,request_time,start_time,end_time,execute_time,mem,create_date,modify_date,status")] sys_log sys_log)
        {
            if (ModelState.IsValid)
            {
                db.sys_logs.Add(sys_log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sys_log);
        }

        // GET: sys_log/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_log sys_log = db.sys_logs.Find(id);
            if (sys_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_log);
        }

        // POST: sys_log/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,log_name,user_id,user_name,interface_name,interface_param,interface_type,interface_dir,local_ip,visitor_ip,result,interface_status,request_time,start_time,end_time,execute_time,mem,create_date,modify_date,status")] sys_log sys_log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sys_log);
        }

        // GET: sys_log/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_log sys_log = db.sys_logs.Find(id);
            if (sys_log == null)
            {
                return HttpNotFound();
            }
            return View(sys_log);
        }

        // POST: sys_log/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            sys_log sys_log = db.sys_logs.Find(id);
            db.sys_logs.Remove(sys_log);
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
