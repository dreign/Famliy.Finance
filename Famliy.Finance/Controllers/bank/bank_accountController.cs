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
    public class bank_accountController : BaseController
    {
        private BankModel db = new BankModel();
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "资金帐户", Controll = "bank_account", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: bank_account
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

            return View(db.bank_accounts.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: bank_account/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_account bank_account = db.bank_accounts.Find(id);
            if (bank_account == null)
            {
                return HttpNotFound();
            }
            return View(bank_account);
        }

        // GET: bank_account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bank_account/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_name,user_id,money,debt,freezing,create_date,modify_date,status")] bank_account bank_account)
        {
            if (ModelState.IsValid)
            {
                db.bank_accounts.Add(bank_account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bank_account);
        }

        // GET: bank_account/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_account bank_account = db.bank_accounts.Find(id);
            if (bank_account == null)
            {
                return HttpNotFound();
            }
            return View(bank_account);
        }

        // POST: bank_account/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_name,user_id,money,debt,freezing,create_date,modify_date,status")] bank_account bank_account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bank_account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bank_account);
        }

        // GET: bank_account/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_account bank_account = db.bank_accounts.Find(id);
            if (bank_account == null)
            {
                return HttpNotFound();
            }
            return View(bank_account);
        }

        // POST: bank_account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            bank_account bank_account = db.bank_accounts.Find(id);
            db.bank_accounts.Remove(bank_account);
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
