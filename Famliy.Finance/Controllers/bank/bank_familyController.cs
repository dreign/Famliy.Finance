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
    public class bank_familyController : BaseController
    {
        private BankModel db = new BankModel();
        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "家庭管理", Controll = "bank_family", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: bank_family
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

            return View(db.bank_familys.ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: bank_family/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_family bank_family = db.bank_familys.Find(id);
            if (bank_family == null)
            {
                return HttpNotFound();
            }
            return View(bank_family);
        }

        // GET: bank_family/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: bank_family/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "family_id,family_name,family_nick_name,assets_total,assets_money,assets_investment,assets_real,assets_debt,assets_net,create_date,modify_date,status")] bank_family bank_family)
        {
            if (ModelState.IsValid)
            {
                bank_family tfamily = db.bank_familys.Where(f => f.family_name == bank_family.family_name).FirstOrDefault();
                if (tfamily != null)
                {
                    ModelState.AddModelError("Msg", "用户已经存在");
                    return View(bank_family);
                }
                db.bank_familys.Add(bank_family);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bank_family);
        }

        // GET: bank_family/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_family bank_family = db.bank_familys.Find(id);
            if (bank_family == null)
            {
                return HttpNotFound();
            }
            return View(bank_family);
        }

        // POST: bank_family/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "family_id,family_name,family_nick_name,assets_total,assets_money,assets_investment,assets_real,assets_debt,assets_net,create_date,modify_date,status")] bank_family family)
        {
            if (ModelState.IsValid)
            {
                bank_family old = db.bank_familys.Find(family.family_id);
                old.family_name = family.family_name;
                old.family_nick_name = family.family_nick_name;
                old.assets_total = family.assets_total;
                old.assets_money = family.assets_money;
                old.assets_investment = family.assets_investment;
                old.assets_real = family.assets_real;
                old.assets_debt = family.assets_debt;
                old.assets_net = family.assets_net;
                old.create_date = family.create_date;
                old.modify_date = family.modify_date;
                old.status = family.status;

                db.Entry(family).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(family);
        }

        // GET: bank_family/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            bank_family bank_family = db.bank_familys.Find(id);
            if (bank_family == null)
            {
                return HttpNotFound();
            }
            return View(bank_family);
        }

        // POST: bank_family/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {           
            bank_family bank_family = db.bank_familys.Find(id);
            //if (bank_family != null)
            //{
            //    db.Entry<bank_family>(bank_family).State = EntityState.Deleted; 
            //}
            db.bank_familys.Remove(bank_family);
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
