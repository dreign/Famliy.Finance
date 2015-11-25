namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 用户-角色关系
    /// </summary>
    public partial class sys_user_role
    {
        public sys_user_role()
        {
            sys_user = new sys_user();
            sys_role = new sys_role();
        }
        public sys_user_role(bool init = false)
        {
            sys_user = new sys_user();
            sys_role = new sys_role();
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

        [Required]
        [Display(Name = "角色Id")]
        public int role_id { get; set; }

        [Required]
        [Display(Name = "用户名称")]
        public string user_name { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }

        public virtual sys_user sys_user { get; set; }
        public virtual sys_role sys_role { get; set; }
       
    }
}
