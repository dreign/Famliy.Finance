namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 角色-权限关系
    /// </summary>
    public partial class sys_role_permission
    {
        public sys_role_permission()
        {
            sys_role = new sys_role();
            sys_permission = new sys_permission();
        }
        public sys_role_permission(bool init = false)
        {
            sys_role = new sys_role();
            sys_permission = new sys_permission();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "关系Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [Display(Name = "角色Id")]
        public int role_id { get; set; }

        [Display(Name = "权限Id")]
        public int permission_id { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }

        public sys_role sys_role { get; set; }     
        public sys_permission sys_permission { get; set; }
    }
}
