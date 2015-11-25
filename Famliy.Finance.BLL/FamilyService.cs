using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class FamilyService : BaseService<bank_family>
    {
       // public static readonly FamilyService Instance = new FamilyService();

        BankModel db = null;
        public FamilyService()
        {
            if (db == null)
            {
                db = base.DbContext;
            }
        }

        public FamilyService(DbContext dbContext) : base(dbContext)
        {
        }
        /// <summary>
        /// 通过家庭表关联查找家庭信息和家庭成员
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public IQueryable<bank_family> FindFamliysByUser(string user_name)
        {
            IQueryable<bank_family> result = null;
            if (!string.IsNullOrEmpty(user_name))
            {               
                result = db.bank_familys.Include("sys_users")
                    .Where(f => f.sys_users.Where(u => u.user_name == user_name).ToList().Count>0);
            }
            return result;
        }
        /// <summary>
        /// 实体关系修改后，取不到值
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public bank_family FindFamliysByUser2(string user_name)
        {
            bank_family result = null;
            if (!string.IsNullOrEmpty(user_name))
            {
                db.bank_familys.Include("sys_users");
                result = db.sys_users
                    .Where(u => u.user_name == user_name)
                    .FirstOrDefault().bank_family;
            }
            return result;
        }
    }
}
