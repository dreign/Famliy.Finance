using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class BankAccountService : BaseService<bank_account>
    {
        //public static readonly BankAccountService Instance = new BankAccountService();

        public BankAccountService()
        {           
        }

        public BankAccountService(DbContext dbContext) : base(dbContext)
        {
            //this.DbContext = dbContext;
        }

        /// <summary>
        /// 家庭成员资金流水列表
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public IQueryable<bank_operate_log> FindBankOperateLogByUserArray(string[] names, int pageSize)
        {
            IQueryable<bank_operate_log> result = null;
            if (names.Length > 0)
            {
                result = this.DbContext.bank_operate_logs.Where(f => names.Contains(f.user_name)).OrderByDescending(f => f.create_date).Take(pageSize);
            }
            return result;
        }
        /// <summary>
        /// 家庭成员资金帐号列表
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public Dictionary<string, bank_account> FindBankAccountDictByUsers(string[] names)
        {
            Dictionary<string, bank_account> result = new Dictionary<string, bank_account>();
            if (names.Length > 0)
            {
                var list = FindList<bank_account>(f => names.Contains(f.user_name)).OrderBy(f => f.user_name);
                foreach (var item in list)
                {
                    result.Add(item.user_name, item);
                }
            }
            return result;
        }
    }
}
