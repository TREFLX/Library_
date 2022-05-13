namespace library.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class employees
    {
        public int id { get; set; }

        [Required]
        [StringLength(30)]
        public string first_name { get; set; }

        [Required]
        [StringLength(30)]
        public string last_name { get; set; }

        [Required]
        [StringLength(20)]
        public string post { get; set; }

        [Required]
        [StringLength(25)]
        public string dept { get; set; }

        public int account_id { get; set; }

        public virtual users users { get; set; }
    }
}
