namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 系统角色
    /// </summary>
    public partial class sys_role
    {
        public sys_role()
        {
            sys_user_roles = new HashSet<sys_user_role>();
            sys_role_permissions = new HashSet<sys_role_permission>();
        }
        public sys_role(bool init = false)
        {
            sys_user_roles = new HashSet<sys_user_role>();
            sys_role_permissions = new HashSet<sys_role_permission>();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "角色Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int role_id { get; set; }

        [Display(Name = "角色名称")]
        [StringLength(50)]
        public string role_name { get; set; }

        [Display(Name = "角色描述")]
        [StringLength(200)]
        public string role_desc { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }
        [Display(Name = "修改时间")]

        public DateTime? modify_date  { get; set; }
        [Display(Name = "状态")]
        public int status { get; set; }

        public virtual ICollection<sys_user_role> sys_user_roles { get; set; }
        public virtual ICollection<sys_role_permission> sys_role_permissions { get; set; }
       
    }
}
