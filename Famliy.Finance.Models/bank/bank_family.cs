namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    /// <summary>
    /// 家庭
    /// </summary>
    public partial class bank_family
    {
        public bank_family()
        {
            sys_users = new HashSet<sys_user>();
            //bank_accounts=new HashSet<bank_account>();
        }

        public bank_family(bool init = false)
        {
            sys_users = new HashSet<sys_user>();
            //bank_accounts = new HashSet<bank_account>();
            if (init)
            {
                this.create_date = DateTime.Now;
                this.modify_date = DateTime.Now;
                this.status = 0;
            }
        }

        [Key]
        [Display(Name = "家庭Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long family_id { get; set; }

        [Display(Name = "家庭名称")]
        [StringLength(50)]
        public string family_name { get; set; }

        [Display(Name = "家庭昵称")]
        [StringLength(50)]
        public string family_nick_name { get; set; }

        [Display(Name = "总资产")]
        public decimal assets_total { get; set; }

        [Display(Name = "货币型资产")]
        public decimal assets_money { get; set; }

        [Display(Name = "投资型资产")]
        public decimal assets_investment { get; set; }

        [Display(Name = "实物型资产")]
        public decimal assets_real { get; set; }

        [Display(Name = "总负债")]
        public decimal assets_debt { get; set; }

        [Display(Name = "净资产")]
        public decimal assets_net { get; set; }

        [Display(Name = "创建时间")]
        public DateTime? create_date { get; set; }
        [Display(Name = "修改时间")]
        public DateTime? modify_date { get; set; }

        [Display(Name = "状态")]
        public int status { get; set; }

        public virtual ICollection<sys_user> sys_users { get; set; }
        //public virtual ICollection<bank_account> bank_accounts { get; set; }

        public static bank_family Clone(bank_family family)
        {
            bank_family item = new bank_family();
            item.assets_debt = family.assets_debt;
            item.assets_investment = family.assets_debt;
            item.assets_money = family.assets_money;
            item.assets_net = family.assets_net;
            item.assets_real = family.assets_real;
            item.assets_total = family.assets_total;
            item.create_date = family.create_date;
            item.family_id = family.family_id;
            item.family_name = family.family_name;
            item.family_nick_name = family.family_nick_name;
            item.modify_date = family.modify_date;
            item.status = family.status;            
            return item;
        }
    }
}
