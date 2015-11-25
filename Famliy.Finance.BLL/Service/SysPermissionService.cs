using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class SysPermissionService : BaseService<sys_permission>
    {
        //public static readonly SysUserService Instance = new SysUserService();
        //public static SysUserService Instance {
        //    get {
        //        return new SysUserService();
        //    }
        //}

        //BankModel db = null;
        public SysPermissionService()
        {
            //if (DbContext == null)
            //{
            //    DbContext = ContextFactory.GetCurrentContext();
            //}
        }

        public SysPermissionService(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
