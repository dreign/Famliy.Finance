using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class SysSubjectService : BaseService<sys_subject>
    {
        //public static readonly SysSubjectService Instance = new SysSubjectService();

        //BankModel db = null;
        public SysSubjectService()
        {
            //if (db == null)
            //{
            //    db = base.DbContext;
            //}
        }

        public SysSubjectService(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
