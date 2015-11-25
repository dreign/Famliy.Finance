namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 系统权限
    /// </summary>
    public partial class sys_permission
    {
        public sys_permission()
        {
            sys_role_permissions = new HashSet<sys_role_permission>();
        }
        public sys_permission(bool init = false)
        {
            sys_role_permissions = new HashSet<sys_role_permission>();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "权限Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
      
        public int permission_id { get; set; }

        [Required]
        [Display(Name = "权限名称")]
        [StringLength(50)]
        public string permission_name { get; set; }

        [Required]
        [Display(Name = "权限地址")]
        [StringLength(200)]
        public string permission_url { get; set; }
   
      
        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }
        [Display(Name = "状态")]
        public int status { get; set; }

        public virtual ICollection<sys_role_permission> sys_role_permissions { get; set; }
       
    }
}
