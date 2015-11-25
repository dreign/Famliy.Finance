using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famliy.Finance.BLL
{
    public class BankOperateLogService : BaseService<bank_operate_log>
    {
        //public static readonly BankOperateLogService Instance = new BankOperateLogService();

        //BankModel db = null;
        public BankOperateLogService()
        {
            //if (db == null)
            //{
            //    db = base.DbContext;
            //}
        }

        public BankOperateLogService(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
