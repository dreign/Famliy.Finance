namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 个人账户清算
    /// </summary>
    public partial class bank_record
    {
        public bank_record()
        {           
        }
        public bank_record(bool init = false)
        {          
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "清算Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Display(Name = "用户Id")]
        public long user_id { get; set; }

        [Required]
        [Display(Name = "用户名称")]
        [StringLength(50)]
        public string user_name { get; set; }

        [Required]
        [Display(Name = "家庭Id")]
        public long family_id { get; set; }


        [Display(Name = "清算日期")]
        public DateTime? period_date { get; set; }

        [Display(Name = "期初值")]
        public decimal opening_balance { get; set; }

        [Display(Name = "期末值")]
        public decimal closed_balance { get; set; }

        [Display(Name = "余额")]
        public decimal balance { get; set; }

        [Display(Name = "是否关账")]
        public byte closed { get; set; }

        [Display(Name = "关账时间")]
        public DateTime? closing_time { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }
    }
}
