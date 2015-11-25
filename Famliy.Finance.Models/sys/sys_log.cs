namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 系统日志
    /// </summary>
    public partial class sys_log
    {
        public sys_log()
        { }
        public sys_log(bool init = false)
        {            
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }
        /// <summary>
        /// 日志Id
        /// </summary>
        [Key]
        [Display(Name = "日志Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        /// <summary>
        /// 日志名称
        /// </summary>
        [Display(Name = "日志名称")]
        [StringLength(50)]
        public string log_name { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Display(Name = "用户Id")]
        public long? user_id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [StringLength(50)]
        public string user_name { get; set; }

        /// <summary>
        /// 接口名称
        /// </summary>
        [Display(Name = "接口名称")]
        [StringLength(50)]
        public string interface_name { get; set; }

        [Display(Name = "接口参数")]
        [StringLength(2000)]
        public string interface_param { get; set; }

        [Display(Name = "接口类型")]
        [StringLength(50)]
        public string interface_type { get; set; }

        [Display(Name = "调用方向")]
        public int? interface_dir { get; set; }

        [Display(Name = "本机Ip")]
        [StringLength(50)]
        public string local_ip { get; set; }

        [Display(Name = "调用方Ip")]
        [StringLength(50)]
        public string visitor_ip { get; set; }

        [Display(Name = "接口结果")]
        [StringLength(2000)]
        public string result { get; set; }

        [Display(Name = "接口状态")]
        [StringLength(50)]
        public string interface_status { get; set; }

        [Display(Name = "请求时间")]
        [StringLength(50)]
        public string request_time { get; set; }

        [Display(Name = "开始时间")]
        public DateTime? start_time { get; set; }

        [Display(Name = "结束时间")]
        public DateTime? end_time { get; set; }

        [Display(Name = "执行时间")]
        public long? execute_time { get; set; }

        [Display(Name = "备注")]
        [Column(TypeName = "text")]
        public string mem { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date  { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }
    }
}
