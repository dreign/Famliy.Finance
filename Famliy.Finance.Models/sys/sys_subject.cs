namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 财务科目
    /// </summary>
    public partial class sys_subject
    {
        public sys_subject()
        {
            bank_operate_logs = new HashSet<bank_operate_log>();

        }
        public sys_subject(bool init = false)
        {
            bank_operate_logs = new HashSet<bank_operate_log>();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]       
        [Required]
        [Display(Name = "科目Id")]      
        public int subject_id { get; set; }

        [Required]
        [Display(Name = "科目名称")]
        [StringLength(50)]
        public string subject_name { get; set; }

        [Display(Name = "科目等级")]
        public int? subject_level { get; set; }

        [Display(Name = "科目描述")]
        [StringLength(200)]
        public string subject_desc { get; set; }

        [Display(Name = "科目单位")]
        [StringLength(50)]
        public string money_unit { get; set; }

        [Display(Name = "父科目Id")]
        public int? parents_id { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }

        public virtual ICollection<bank_operate_log> bank_operate_logs { get; set; }       
    }
}
