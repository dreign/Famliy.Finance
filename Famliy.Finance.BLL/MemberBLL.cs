using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Famliy.Finance.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Famliy.Finance.BLL
{
    public class MemberBLL
    {
        public static readonly MemberBLL Instance = new MemberBLL();
        private static BankModel db;
        public MemberBLL()
        {
            if (db == null)
            {
                db = new BankModel();
            }
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public async Task<sys_user> Find_SysUser(string user_name)
        {
            sys_user user = null;
            await Task.Run(() =>
            {
                var userlist = db.sys_users.Where(u => u.user_name == user_name).ToList();
                if (userlist.Count() > 0)
                {
                    user = userlist[0];
                }
            });
            return user;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user_name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<sys_user> Get_SysUser(string user_name, string password)
        {
           // BankModel bank = new BankModel();
            sys_user user=null;
            await Task.Run(() =>
            {
                user = db.sys_users.Where(u => u.user_name == user_name & u.password == password).FirstOrDefault();                
            });
            return user;           
        }
        public async Task<List<sys_user>> Get_SysUserList(int pageSize,int pageNo)
        {
            // BankModel bank = new BankModel();
            List<sys_user> userlist = null;
            await Task.Run(() =>
            {
                userlist = db.sys_users.OrderBy(u => u.user_name).Skip(pageNo).Take(pageSize).ToList(); //函数式方式;
            });
            return userlist;            
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<sys_user> Add_SysUser(sys_user user)
        {
            //BankModel bank = new BankModel();
            sys_user tuser=null;
            await Task.Run(() =>
            {
                tuser=db.sys_users.Add(user);
                db.SaveChanges();
            });
            if (tuser.user_id >= 0)
            {
                return tuser;
            }
            else
            {
                return null;
            }
        }

        public ClaimsIdentity CreateIdentity(sys_user user, string authenticationType)
        {
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, user.user_name));
            _identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.user_id.ToString()));
            _identity.AddClaim(new Claim(ClaimTypes.Surname, user.nick_name));
            _identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("NickName", user.nick_name));
            return _identity;
        }
    }
}
