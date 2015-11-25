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
using GW.Mail;
using GW.Utils;

namespace Famliy.Finance.Controllers
{
    [Authorize]
    public class sys_userController : BaseController
    {
        SysUserService userService = new SysUserService();
        SysRoleService roleService = new SysRoleService();
        SysUserRoleService userRoleService = new SysUserRoleService();
        BankFamilyService familyService = new BankFamilyService();
        BankAccountService accountService = new BankAccountService();

        public override void InitBreadCrumbs()
        {
            List<BreadCrumbs> navList = new List<BreadCrumbs>();
            navList.Add(new BreadCrumbs() { ID = 1, Text = "管理", Controll = "bank_family", Action = "Index" });
            navList.Add(new BreadCrumbs() { ID = 2, Text = "成员管理", Controll = "sys_user", Action = "Index" });
            ViewData["NavList"] = navList;
        }
        // GET: sys_user
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
            return View(userService.FindAll().ToList().ToPagedList(pageNumber, pageSize));
        }

        // GET: sys_user/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            long userId = GW.Utils.ParseHelper.Parse<long>(id);
            sys_user sys_user = userService.Find(u => u.user_id == userId);
            if (sys_user == null)
            {
                return HttpNotFound();
            }
            return View(sys_user);
        }

        // GET: sys_user/Create
        public ActionResult Create()
        {
            //List<bank_family> familyList = new List<bank_family>();
            //familyList.Add(CurrentUser.bank_family);
            //ViewData["FamilyList"] = new SelectList(familyList, "family_id", "family_name", CurrentUser.bank_family.family_id);

            List<bank_family> familyList = familyService.FindAll().ToList();
            ViewBag.FamilyList = new SelectList(familyList, "family_id", "family_name");
            return View();
        }

        // POST: sys_user/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_name,password,nick_name,family_id,family_name,remark,sex,age,birthday,address,post,phone,email,qq,weixin,interest,user_desc,last_login_time,last_login_ip")] sys_user user)
        {
            List<bank_family> familyList = familyService.FindAll().ToList();
            ViewBag.FamilyList = new SelectList(familyList, "family_id", "family_name");

            sys_user olduser = userService.Find(u => u.user_name == user.user_name);
            if (olduser != null) 
            {
                ModelState.AddModelError("Msg", "用户已经存在");
                return View(user);
            }
            if (ModelState.IsValid)
            {               
                user.password = Utils.MD5Encrypt(user.password);
                user.create_date = DateTime.Now;
                user.modify_date = DateTime.Now;
                user.status = 0;

                //当前家长用户创建的用户，默认是本家庭的成员用户                
                user.bank_family = familyList.Where(f=>f.family_id==user.family_id).FirstOrDefault();
                userService.Add(user,true);

                var oldUser=accountService.Find(a=>a.user_name==user.user_name);
                if (olduser == null)
                {                    //同步添加资金帐号
                    bank_account account = new bank_account();
                    account.user_id = user.user_id;
                    account.user_name = user.user_name;
                    account.money = 0;
                    account.debt = 0;
                    account.create_date = DateTime.Now;
                    account.modify_date = DateTime.Now;
                    account.status = 0;

                    accountService.Add(account, true);
                }
                else
                {
                    oldUser.money = 0;
                    accountService.Update(oldUser,true);
                }

                GW.Mail.Entity.MailInfo mailInfo = new GW.Mail.Entity.MailInfo();
                if (!string.IsNullOrEmpty(user.email))
                {
                    MailManager.SendTo(user.email, "家庭理财", "恭喜您！帐号" + user.user_name + "已创建");
                    ///MailManager.Send("EMailSection", "家庭理财", "恭喜您！帐号" + user.user_name + "已创建");
                }        
              

                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: sys_user/Edit/5
        public ActionResult Edit(string id)
        {
            sys_user user = null;
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                long userId = GW.Utils.ParseHelper.Parse<long>(id);
                user = userService.Find(u => u.user_id == userId);

                if (user == null)
                {
                    return HttpNotFound();
                }
                List<bank_family> familyList = familyService.FindAll().ToList();
                ViewBag.FamilyList = new SelectList(familyList, "family_id", "family_name");

                //角色列表
                int uid = GW.Utils.ParseHelper.Parse<int>(id);
                var RoleList = roleService.FindAll().ToList();
                var UserRoleList = userRoleService.FindList<sys_user_role>(f => f.sys_user.user_id == uid).ToList();
                ViewBag.RoleList = RoleList;
                ViewBag.UserRoleList = UserRoleList;
            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write("Famliy.Finance.Controllers.sys_user.Edit error.", ex);
            }
            return View(user);
        }

        // POST: sys_user/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_name,password,nick_name,family_id,family_name,remark,sex,age,birthday,address,post,phone,email,qq,weixin,interest,user_desc,last_login_time,last_login_ip,status")] sys_user user)
        {
            try
            {
                List<bank_family> familyList = familyService.FindAll().ToList();
                ViewBag.FamilyList = new SelectList(familyList, "family_id", "family_name");

                #region Way1

                //sys_user olduser = service.Find(u => u.user_id == user.user_id);

                //olduser = sys_user.Clone(user);
                //olduser.user_id = 0;
                //olduser.password = Utils.MD5Encrypt(user.password);
                //olduser.modify_date = DateTime.Now;
                //olduser.bank_family = bank_family.Clone(CurrentUser.bank_family);

                //service.Update(olduser);
                //return RedirectToAction("Index");
                #endregion

                #region Way2

                sys_user olduser = userService.Find(u => u.user_id == user.user_id);

                if (olduser == null) ModelState.AddModelError("", "用户不存在");
                else
                {
                    //this.Request["PublisherID"].
                    //olduser.user_name = user.user_name;
                    olduser.email = user.email;
                    olduser.nick_name = user.nick_name;
                    olduser.birthday = user.birthday;
                    olduser.sex = user.sex;
                    olduser.address = user.address;
                    olduser.age = user.age;
                    olduser.family_id = user.family_id;
                    olduser.family_name = user.family_name;
                    olduser.interest = user.interest;
                    olduser.phone = user.phone;
                    olduser.post = user.post;
                    olduser.qq = user.qq;
                    olduser.remark = user.remark;
                    olduser.weixin = user.weixin;
                    olduser.user_desc = user.user_desc;
                    olduser.password = Utils.MD5Encrypt(user.password);
                    //olduser.create_date = user.create_date;
                    olduser.modify_date = DateTime.Now;
                    //olduser.status = user.status;

                    olduser.bank_family = familyList.Where(f => f.family_id == user.family_id).FirstOrDefault();

                    if (TryUpdateModel(olduser))
                    {
                        if (ModelState.IsValid)
                        {
                            //db.Entry(olduser).State = EntityState.Modified;
                            //db.SaveChanges();
                            //if (olduser != null)
                            //{
                            //    service.DbContext.Entry<sys_user>(olduser).State = EntityState.Modified;
                            //}
                            if (userService.Update(olduser))
                                ModelState.AddModelError("", "修改成功！");
                            else
                                ModelState.AddModelError("", "无需要修改的资料");
                            return RedirectToAction("Index");
                        }
                    }
                    else ModelState.AddModelError("", "更新模型数据失败");
                }
                #endregion

            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write("Famliy.Finance.Controllers.sys_user.Edit(Post) error.", ex);
            }
            return View(user);
        }
        public ActionResult Save(int uid, string roleids)
        {
            string strData = "ok";
            if (!string.IsNullOrEmpty(roleids))
            {
                string[] flag = { "," };
                string[] ids = roleids.Split(flag, StringSplitOptions.RemoveEmptyEntries);
                int rid = 0;
                for (var i = 0; i < ids.Length; i++)
                {
                    rid = GW.Utils.ParseHelper.Parse<int>(ids[i]);
                    var item = userRoleService.FindList<sys_user_role>(f => f.sys_user.user_id == uid).Where(f => f.sys_role.role_id == rid).FirstOrDefault();
                    //没有则添加
                    if (item == null)
                     {
                        // :warning:
                        //userRoleService.Add()
                    }
                }
            }
            return Content(strData);
        }
        // GET: sys_user/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            long userId = GW.Utils.ParseHelper.Parse<long>(id);
            sys_user sys_user = userService.Find(u => u.user_id == userId);
            if (sys_user == null)
            {
                return HttpNotFound();
            }
            return View(sys_user);
        }

        // POST: sys_user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            long userId = GW.Utils.ParseHelper.Parse<long>(id);
            sys_user sys_user = userService.Find(u => u.user_id == userId);
            userService.Delete(sys_user, true);
            //db.sys_users.Remove(sys_user);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
