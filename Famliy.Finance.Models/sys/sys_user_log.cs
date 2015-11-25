namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 用户日志
    /// </summary>
    public partial class sys_user_log
    {
        public sys_user_log()
        { }
        public sys_user_log(bool init = false)
        {
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "日志Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long log_id { get; set; }

        [Display(Name = "用户Id")]
        public long user_id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        [StringLength(50)]
        public string user_name { get; set; }

        [Display(Name = "动作")]
        [StringLength(50)]
        public string action { get; set; }

        [Display(Name = "动作描述")]
        [StringLength(200)]
        public string action_desc { get; set; }


        [Display(Name = "登录IP")]
        [StringLength(50)]
        public string login_ip { get; set; }

        [Display(Name = "登录时间")]
        public DateTime? login_time { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }
    }
}
