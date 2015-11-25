namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class report_user_day
    {
        [Key]
        public int id { get; set; }

        [StringLength(50)]
        public string user_name { get; set; }

        public decimal? total { get; set; }

        [StringLength(10)]
        public string dt { get; set; }
    }
}
