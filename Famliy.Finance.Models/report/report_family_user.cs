namespace Famliy.Finance.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class report_family_user
    {
        [Key]
        public int id { get; set; }
        public long? bank_family_family_id { get; set; }

        [StringLength(50)]
        public string family_name { get; set; }

        [StringLength(50)]
        public string user_name { get; set; }
    }
}
