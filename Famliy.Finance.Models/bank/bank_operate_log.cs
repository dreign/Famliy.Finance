namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 个人账户流水
    /// </summary>
    public partial class bank_operate_log
    {
        public bank_operate_log()
        {
            sys_subject = new sys_subject();
        }
        public bank_operate_log(bool init = false)
        {
            sys_subject = new sys_subject();
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
        public long id { get; set; }

        [Required]
        [Display(Name = "用户Id")]
        public long user_id { get; set; }

        [Required]
        [Display(Name = "用户名")]
        [StringLength(50)]
        public string user_name { get; set; }
        
        [Display(Name = "金额")]
        public decimal money { get; set; }

        [Display(Name = "历史金额")]
        public decimal? history_total { get; set; }

        [Display(Name = "科目Id")]
        public int? subject_id { get; set; }

        /// <summary>
        /// 科目别名,不同于subject表的subject_name或subject_desc，是留作自定义科目描述文字的字段
        /// </summary>
        [Display(Name = "科目别名")]
        [StringLength(200)]
        public string subject_remark { get; set; }

        [Display(Name = "订单号")]
        [StringLength(200)]
        public string order_id { get; set; }

        [Display(Name = "产品名称")]
        [StringLength(200)]
        public string product_name { get; set; }

        [Display(Name = "产品数量")]
        public int? product_number { get; set; }

        [Display(Name = "产品总价")]
        public decimal? order_amount { get; set; }

        [Display(Name = "支付方式")]
        [StringLength(100)]
        public string pay_way { get; set; }

        [Display(Name = "备注")]
        [StringLength(500)]
        public string remark { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int? status { get; set; }

        public sys_subject sys_subject { get; set; }
    }
}
