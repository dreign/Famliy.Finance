namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 用户
    /// </summary>
    public partial class sys_user
    {
        public sys_user()
        {
            sys_user_roles = new HashSet<sys_user_role>();
            bank_family = new bank_family();
        }

        public sys_user(bool init = false)
        {
            sys_user_roles = new HashSet<sys_user_role>();
            bank_family = new bank_family();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "用户Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 user_id { get; set; }

        //[Unique]    
        [Required(ErrorMessage="必填")]
        [Display(Name = "用户名称")]
        [StringLength(50)]
        public string user_name { get; set; }

        [Display(Name = "密码")]
        [StringLength(200, MinimumLength = 4, ErrorMessage = "{1}到{0}个字符")]
        [DataType(DataType.Password)]
               
        public string password { get; set; }

        [Display(Name = "昵称")]
        [StringLength(50)]
        public string nick_name { get; set; }

        [Display(Name = "家庭Id")]
        public int family_id { get; set; }

        [Display(Name = "家庭昵称")]
        [StringLength(50)]
        public string family_name { get; set; }

        [Display(Name = "签名")]
        [StringLength(200)]
        public string remark { get; set; }

        /// <summary>
        /// true:男  false:女
        /// </summary>
        [Display(Name = "性别")]
        
        public bool? sex { get; set; }
        [Display(Name = "年龄")]

        public int? age { get; set; }


        [Display(Name = "生日")]
        public DateTime? birthday { get; set; }

        [Display(Name = "地址")]
        [StringLength(200)]
        public string address { get; set; }

        [Display(Name = "邮编")]
        [StringLength(50)]
        public string post { get; set; }

        [Display(Name = "电话")]
        [StringLength(50)]
        public string phone { get; set; }

        [Required]
        [Display(Name = "邮件")]
        [StringLength(50)]
        public string email { get; set; }

        [Display(Name = "QQ")]
        [StringLength(50)]
        public string qq { get; set; }

        [Display(Name = "微信")]
        [StringLength(50)]
        public string weixin { get; set; }

        [Display(Name = "兴趣")]
        [StringLength(500)]
        public string interest { get; set; }

        [Display(Name = "备注")]
        [StringLength(500)]
        public string user_desc { get; set; }

        [Display(Name = "最后登录时间")]
        public DateTime? last_login_time { get; set; }

        [Display(Name = "最后登录IP")]
        [StringLength(50)]
        public string last_login_ip { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }

        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int? status { get; set; }

        public virtual ICollection<sys_user_role> sys_user_roles { get; set; }
        public virtual bank_family bank_family { get; set; }

      
        /// <summary>
        /// sex=true:男  sex=false:女
        /// </summary>
        /// <returns></returns>
        public string IsMale()
        {
            if ((bool)this.sex)
                return "女";
            else
                return "男";
        }
        public static sys_user Clone(sys_user user)
        {
            sys_user olduser = new sys_user();
            if (olduser != null)
            {
                olduser.user_id = user.user_id;
                olduser.user_name = user.user_name;
                olduser.email = user.email;
                olduser.create_date = user.create_date;
                olduser.modify_date = user.modify_date;
                olduser.status = user.status;

                olduser.nick_name = user.nick_name;
                olduser.password = user.password;
                olduser.birthday = user.birthday;
                olduser.sex = user.sex;
                olduser.address = user.address;
                olduser.age = user.age;
                olduser.family_id = user.family_id;
                olduser.family_name = user.family_name;
                olduser.interest = user.interest;
                olduser.last_login_ip = user.last_login_ip;
                olduser.last_login_time = user.last_login_time;
                olduser.phone = user.phone;
                olduser.post = user.post;
                olduser.qq = user.qq;
                olduser.remark = user.remark;
                olduser.weixin = user.weixin;
                olduser.user_desc = user.user_desc;
            }
            return olduser;
        }
    }
}
