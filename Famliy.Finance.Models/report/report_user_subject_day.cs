namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class report_user_subject_day
    {
        [Key]
        public int id { get; set; }

        [StringLength(50)]
        public string user_name { get; set; }

        public int? subject_id { get; set; }

        public decimal? total { get; set; }

        [StringLength(10)]
        public string dt { get; set; }

        [StringLength(50)]
        public string subject_name { get; set; }
    }
}
