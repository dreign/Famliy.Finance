using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Famliy.Finance.Common;

namespace Famliy.Finance
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {              
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                
                // 初始化配置
                ConfigBusiness config = new ConfigBusiness();
                // 在应用程序启动时运行的代码
                log4net.Config.XmlConfigurator.Configure();

                GW.Utils.LogHelper.Write("Application start...", GW.Utils.LogMsgLevel.Info);

#warning 数据库初始化
                //TODO
                if (Common.ConfigBusiness.IsInitDB)
                {
                    System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<Famliy.Finance.Models.BankModel>());
                    using (var context = new BankModel())
                    {
                        context.Database.Initialize(true);
                        InitDb(context);
                    }
                }
            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write(ex.Message + "[][]" + ex.StackTrace.ToString(), ex);
            }
        }

        public void InitDb(BankModel context)
        {
            bank_family family = new bank_family(true);
            family.family_name = "董瑞军的家";
            family.family_nick_name = "My Sweet Home";
            family.assets_money = 0;
            family.assets_investment = 0;
            family.assets_real = 0;
            family.assets_total = 0;
            family.assets_debt = 0;
            family.assets_net = 0;
            bank_family tfamily = context.bank_familys.Add(family);

            context.SaveChanges();

            sys_permission permission = new sys_permission(true);
            permission.permission_name = "系统管理员";
            permission.permission_url = "/Home/Index";
            sys_permission tpermission = context.sys_permissions.Add(permission);

            context.SaveChanges();

            sys_role role = new sys_role(true);
            role.role_name = "管理员";
            role.role_desc = "管理员";
            sys_role trole = context.sys_roles.Add(role);

            context.SaveChanges();

            sys_role_permission role_permission = new sys_role_permission(true);
            role_permission.role_id = trole.role_id;
            role_permission.permission_id = tpermission.permission_id;

            role_permission.sys_role = trole;
            role_permission.sys_permission = tpermission;
            sys_role_permission trole_permission = context.sys_role_permissions.Add(role_permission);

            context.SaveChanges();

            sys_user user = new sys_user(true);
            user.email = user.user_name = "dongruijun@gw.com.cn";
            user.password = Utils.MD5Encrypt("dreign");
            user.nick_name = "董瑞军";
            user.birthday = DateTime.Now;
            user.sex = false;
            user.address = "上海市浦东新区";
            user.age = 35;
            user.family_name = "家长";
            user.interest = "电影，音乐，运动";
            user.last_login_ip = "127.0.0.1";
            user.last_login_time = DateTime.Now;
            user.phone = "1861xxxx091";
            user.post = "200126";
            user.qq = "27178846";
            user.remark = "好想给国家多庆祝几天生日啊~";
            user.weixin = "dreign";
            user.user_desc = @"上海交通大学继续教育学院网络教育（原网络教育学院）,位于上海市华山路1954号交大徐汇校区。开展专升本\高升专\高升本学历教育，招生专业涵盖艺术\经济管理\船舶\计算机\英语\机电等十余个领域，总机：021-52389900。";
            user.bank_family = tfamily;
            sys_user tuser1 = context.sys_users.Add(user);
            user = new sys_user(true);
            user.email = user.user_name = "dreign@qq.com";
            user.password = Utils.MD5Encrypt("dreign");
            user.nick_name = "魏晶晶";
            user.birthday = DateTime.Now;
            user.sex = true;
            user.address = "上海市浦东新区";
            user.age = 30;
            user.family_name = "家长";
            user.interest = "美食，音乐";
            user.last_login_ip = "127.0.0.1";
            user.last_login_time = DateTime.Now;
            user.phone = "1861xxxx091";
            user.post = "200126";
            user.qq = "27178846";
            user.remark = "好好学习天天向上~";
            user.weixin = "dreign";
            user.user_desc = @"东宫白庶子，南寺远禅师。何处遥相见，心无一事时。";
            user.bank_family = tfamily;
            sys_user tuser2 = context.sys_users.Add(user);

            context.SaveChanges();

            sys_user_role user_role = new sys_user_role(true);
            user_role.user_name = tuser1.user_name;
            user_role.role_id = trole.role_id;
            user_role.sys_role = trole;
            user_role.sys_user = tuser1;
            sys_user_role tuser_role1 = context.sys_user_roles.Add(user_role);
            user_role = new sys_user_role(true);
            user_role.user_name = tuser2.user_name;
            user_role.role_id = trole.role_id;
            user_role.sys_role = trole;
            user_role.sys_user = tuser2;
            sys_user_role tuser_role2 = context.sys_user_roles.Add(user_role);

            context.SaveChanges();

            bank_account account = new bank_account(true);
            account.user_id = tuser1.user_id;
            account.user_name = tuser1.user_name;
            account.money = 9500;
            account.freezing = 0;
            bank_account taccount1 = context.bank_accounts.Add(account);
            account = new bank_account(true);
            account.user_id = tuser2.user_id;
            account.user_name = tuser2.user_name;
            account.money = 0;
            account.freezing = 0;
            bank_account taccount2 = context.bank_accounts.Add(account);

            context.SaveChanges();

          
            //bank_family_user family_relation = new bank_family_user(true);
            //family_relation.family_id = tfamily.family_id;
            //family_relation.user_name = tuser1.user_name;
            //family_relation.sys_user = tuser1;
            //family_relation.bank_family = tfamily;
            //bank_family_user tfamily_user1 = context.bank_family_users.Add(family_relation);
            //family_relation = new bank_family_user(true);
            //family_relation.family_id = tfamily.family_id;
            //family_relation.user_name = tuser2.user_name;
            //family_relation.sys_user = tuser2;
            //family_relation.bank_family = tfamily;
            //bank_family_user tfamily_user2 = context.bank_family_users.Add(family_relation);

            context.SaveChanges();

            sys_subject subject = new sys_subject(true);
            subject.subject_id = 1;
            subject.subject_name = "资产";
            subject.subject_level = 1;
            subject.subject_desc = "资产";
            subject.parents_id = 0;
            sys_subject tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 2;
            subject.subject_name = "负债";
            subject.subject_level = 1;
            subject.subject_desc = "负债";
            subject.parents_id = 0;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 3;
            subject.subject_name = "货币资产";
            subject.subject_level = 2;
            subject.subject_desc = "资产";
            subject.parents_id = 1;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 4;
            subject.subject_name = "投资资产";
            subject.subject_level = 2;
            subject.subject_desc = "资产";
            subject.parents_id = 1;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 5;
            subject.subject_name = "实物资产";
            subject.subject_level = 2;
            subject.subject_desc = "资产";
            subject.parents_id = 1;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 6;
            subject.subject_name = "贷款";
            subject.subject_level = 2;
            subject.subject_desc = "负债";
            subject.parents_id = 2;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 7;
            subject.subject_name = "债务";
            subject.subject_level = 2;
            subject.subject_desc = "负债";
            subject.parents_id = 2;
            tsubject = context.sys_subjects.Add(subject);

            subject = new sys_subject(true);
            subject.subject_id = 8;
            subject.subject_name = "日常家用";
            subject.subject_level = 2;
            subject.subject_desc = "负债";
            subject.parents_id = 2;
            tsubject = context.sys_subjects.Add(subject);

            context.SaveChanges();

            List<sys_subject> subjectList = new List<sys_subject>();

            subject = new sys_subject(true) { subject_id = 9, subject_name = "现金与活期存款", subject_level = 3, subject_desc = "货币资产", parents_id = 3 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 10, subject_name = "活期存款", subject_level = 3, subject_desc = "货币资产", parents_id = 3 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 11, subject_name = "工资", subject_level = 3, subject_desc = "货币资产", parents_id = 3 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 12, subject_name = "其他货币资产", subject_level = 3, subject_desc = "货币资产", parents_id = 3 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 13, subject_name = "股票", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 14, subject_name = "债券", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 15, subject_name = "基金", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 16, subject_name = "银行理财", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 17, subject_name = "信托", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 18, subject_name = "互联网金融", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 19, subject_name = "保险", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 20, subject_name = "其他投资资产", subject_level = 3, subject_desc = "投资资产", parents_id = 4 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 21, subject_name = "自住房", subject_level = 3, subject_desc = "实物资产", parents_id = 5 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 22, subject_name = "投资房", subject_level = 3, subject_desc = "实物资产", parents_id = 5 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 23, subject_name = "机动车", subject_level = 3, subject_desc = "实物资产", parents_id = 5 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 24, subject_name = "家具", subject_level = 3, subject_desc = "实物资产", parents_id = 5 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 25, subject_name = "其他实物资产", subject_level = 3, subject_desc = "实物资产", parents_id = 5 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 26, subject_name = "住房贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 27, subject_name = "车辆贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 28, subject_name = "消费贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 29, subject_name = "助学贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 30, subject_name = "信用卡贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 31, subject_name = "其他贷款", subject_level = 3, subject_desc = "贷款", parents_id = 6 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 32, subject_name = "法人债务", subject_level = 3, subject_desc = "债务", parents_id = 7 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 33, subject_name = "个人债务", subject_level = 3, subject_desc = "债务", parents_id = 7 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 34, subject_name = "餐饮", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 35, subject_name = "水电煤", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 36, subject_name = "交通", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 37, subject_name = "通讯", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 38, subject_name = "医疗", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 39, subject_name = "衣服", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 40, subject_name = "社交", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 41, subject_name = "旅游", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 42, subject_name = "教育", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 43, subject_name = "日用品", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);
            subject = new sys_subject(true) { subject_id = 44, subject_name = "其他负债", subject_level = 3, subject_desc = "日常家用", parents_id = 8 }; subjectList.Add(subject);

            context.sys_subjects.AddRange(subjectList);

            context.SaveChanges();

            bank_operate_log operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 11;
            operate_log.sys_subject = tsubject;
            operate_log.money = 5000;
            operate_log.subject_remark = "奖金";
            operate_log.remark = "收入";
            bank_operate_log toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 35;
            operate_log.money = -200;
            operate_log.subject_remark = "水电煤";
            operate_log.remark = "支出";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 37;
            operate_log.money = -100;
            operate_log.subject_remark = "手机费";
            operate_log.remark = "支出";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 36;
            operate_log.money = -200;
            operate_log.subject_remark = "交通费";
            operate_log.remark = "支出";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 11;
            operate_log.money = 10000;
            operate_log.subject_remark = "工资";
            operate_log.remark = "收入";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 42;
            operate_log.money = -4000;
            operate_log.subject_remark = "学费";
            operate_log.remark = "支出";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);

            operate_log = new bank_operate_log(true);
            operate_log.user_id = tuser1.user_id;
            operate_log.user_name = tuser1.user_name;
            operate_log.subject_id = 44;
            operate_log.money = -1000;
            operate_log.subject_remark = "偿还信用卡";
            operate_log.remark = "支出";
            operate_log.sys_subject = tsubject;
            toperate_log1 = context.bank_operate_logs.Add(operate_log);           

            context.SaveChanges();

            sys_log log = new sys_log(true);
            log.user_id = tuser1.user_id;
            log.user_name = tuser1.user_name;
            log.visitor_ip = "127.0.0.1";
            log.local_ip = "127.0.0.1";
            log.result = "OK";
            var tlog = context.sys_logs.Add(log);

            log = new sys_log(true);
            log.user_id = tuser1.user_id;
            log.user_name = tuser1.user_name;
            log.visitor_ip = "127.0.0.1";
            log.local_ip = "127.0.0.1";
            log.result = "OK";

            tlog = context.sys_logs.Add(log);

            context.SaveChanges();

            sys_user_log userlog = new sys_user_log(true);
            userlog.user_id = tuser1.user_id;
            userlog.user_name = tuser1.user_name;
            userlog.login_time = DateTime.Now;
            userlog.action = "Login";
            userlog.action_desc = "登录";
            var tuserlog = context.sys_user_logs.Add(userlog);

            userlog = new sys_user_log(true);
            userlog.user_id = tuser1.user_id;
            userlog.user_name = tuser1.user_name;
            userlog.login_time = DateTime.Now;
            userlog.action = "Login";
            userlog.action_desc = "登录";
            tuserlog = context.sys_user_logs.Add(userlog);

            context.SaveChanges();

        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
        }

        void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            try
            {
                var statusCode = Context.Response.StatusCode;
                if (statusCode == 404)
                {
                    Response.Clear();
                    Response.RedirectToRoute("Default", new { controller = "Home", action = "Page404", id = UrlParameter.Optional });
                }
                if (statusCode == 500)
                {
                    Response.Clear();
                    Response.RedirectToRoute("Default", new { controller = "Home", action = "Page500", id = UrlParameter.Optional });
                }

                // 在出现未处理的错误时运行的代码
                Exception ex = Server.GetLastError();
                Exception objErr = ex.GetBaseException();
                string err = "Error in:" + Request.Url.ToString() +
                        "Error Message: " + objErr.Message.ToString() +
                        "Stack Trace:" + objErr.StackTrace.ToString();
                GW.Utils.LogHelper.Write("GW.Bank.Web:" + err, ex);
            }
            catch (Exception ex)
            {
                GW.Utils.LogHelper.Write("GW.Bank.Web.Global error.", ex);
            }
        }      
    }
}
