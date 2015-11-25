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
    [Authorize]
    public class sys_roleController : BaseController
    {
        SysRoleService roleService = new SysRoleService();
        SysPermissionService permissionService = new SysPermissionService();
        SysRolePermissionService rolePermissionService = new SysRolePermissionService();

        private BankModel db = new BankModel();

        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "角色管理", Controll = "sys_role", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: sys_role
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
            return View(db.sys_roles.ToList().ToPagedList(pageNumber, pageSize));
        }
        // GET: sys_role/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role sys_role = db.sys_roles.Find(id);
            if (sys_role == null)
            {
                return HttpNotFound();
            }
            return View(sys_role);
        }

        // GET: sys_role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: sys_role/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "role_id,role_name,role_desc,create_date,modify_date,status")] sys_role sys_role)
        {
            if (ModelState.IsValid)
            {
                db.sys_roles.Add(sys_role);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sys_role);
        }

        // GET: sys_role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role sys_role = db.sys_roles.Find(id);
            if (sys_role == null)
            {
                return HttpNotFound();
            }
            //权限列表
            var PermissionList = permissionService.FindAll().ToList();
            var RolePermissionList = rolePermissionService.FindList<sys_role_permission>(f=>f.role_id==id).ToList();
            ViewBag.PermissionList = PermissionList;
            ViewBag.RolePermissionList = RolePermissionList;
            return View(sys_role);
        }

        // POST: sys_role/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "role_id,role_name,role_desc,create_date,modify_date,status")] sys_role sys_role)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sys_role).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sys_role);
        }
        public ActionResult Save(int rid,string pids)
        {
            string strData = "ok";
            if (!string.IsNullOrEmpty(pids))
            {
                string[] flag = {","};
                string[] ids = pids.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                int id = 0;
                for (var i = 0; i < ids.Length; i++)
                {
                    id = GW.Utils.ParseHelper.Parse<int>(ids[i]);
                    var item =rolePermissionService.FindList<sys_role_permission>(f => f.sys_role.role_id == rid).Where(f => f.sys_permission.permission_id == id).FirstOrDefault();
                    //没有则添加
                    if (item == null) {
                        //rolePermissionService.Add()
                    }
                }
            }
            return Content(strData);
        }
        // GET: sys_role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sys_role sys_role = db.sys_roles.Find(id);
            if (sys_role == null)
            {
                return HttpNotFound();
            }
            return View(sys_role);
        }

        // POST: sys_role/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sys_role sys_role = db.sys_roles.Find(id);
            db.sys_roles.Remove(sys_role);
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
