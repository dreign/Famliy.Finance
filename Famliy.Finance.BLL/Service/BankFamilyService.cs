using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class BankFamilyService : BaseService<bank_family>
    {
       //public static readonly BankFamilyService Instance = new BankFamilyService();

        //BankModel db = null;
        public BankFamilyService()
        {
            //if (db == null)
            //{
            //    db = base.DbContext;
            //}
        }

        public BankFamilyService(DbContext dbContext) : base(dbContext)
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
                result = DbContext.bank_familys.Include("sys_users")
                    .Where(f => f.sys_users.Where(u => u.user_name == user_name).ToList().Count > 0);
            }
            return result;
        }
    }
}
