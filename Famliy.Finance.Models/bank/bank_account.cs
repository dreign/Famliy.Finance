namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 个人账户
    /// </summary>
    public partial class bank_account
    {
        public bank_account()
        {
        }

        public bank_account(bool init=false)
        {
            if (init)
            {
                this.money = 0;
                this.freezing = 0;
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Required]
        [Display(Name = "用户Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]           
        public long user_id { get; set; }

        [Key]
        [Required(ErrorMessage = "必填")]
        [Display(Name = "用户名")]
        [StringLength(50)]
        public string user_name { get; set; }

        [Display(Name = "总资产")]
        public decimal money { get; set; }       

        [Display(Name = "总负债")]
        public decimal debt { get; set; }       

        [Display(Name = "冻结金额")]
        public decimal freezing { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }
    }
}
