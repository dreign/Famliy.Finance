using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class SysLogService : BaseService<sys_log>
    {
        //public static readonly SysLogService Instance = new SysLogService();

        //BankModel db = null;
        public SysLogService()
        {
            //if (db == null)
            //{
            //    db = base.DbContext;
            //}
        }

        public SysLogService(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
