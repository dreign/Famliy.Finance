using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famliy.Finance.Models;
using System.Data.Entity;

namespace Famliy.Finance.BLL
{
    public partial class SysRolePermissionService : BaseService<sys_role_permission>
    {
        //public static readonly SysUserService Instance = new SysUserService();
        //public static SysUserService Instance {
        //    get {
        //        return new SysUserService();
        //    }
        //}

        //BankModel db = null;
        public SysRolePermissionService()
        {
            //if (DbContext == null)
            //{
            //    DbContext = ContextFactory.GetCurrentContext();
            //}
        }

        public SysRolePermissionService(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
