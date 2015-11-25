namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 家庭-用户关系
    /// </summary>
    public partial class bank_family_user
    {
        public bank_family_user()
        {
            sys_user = new sys_user();
            bank_family = new bank_family();           
        }
        public bank_family_user(bool init = false)
        {
            sys_user = new sys_user();
            bank_family = new bank_family();
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

        [Required(ErrorMessage = "必填")]
        [Display(Name = "家庭Id")] 
        public long family_id { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "用户名称")]
        public string user_name { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }
        public virtual sys_user sys_user { get; set; }
        public virtual bank_family bank_family { get; set; }  
    }
}
