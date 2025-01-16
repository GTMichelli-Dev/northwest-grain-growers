
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SeedTicketInfo")]
    public partial class SeedTicketInfo
    {
        [Key]
        public Guid UID { get; set; }

        public int? Ticket { get; set; }

      
        public int Location_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Ticket_Date { get; set; }

        public double? Net { get; set; }

        public double? Clean { get; set; }

        public double? Treated { get; set; }

        [StringLength(250)]
        public string Variety { get; set; }

        public int? Dept { get; set; }

        [StringLength(255)]
        public string CommodityDetails { get; set; }

        [StringLength(3)]
        public string Commodity { get; set; }

        public int? Grower_ID { get; set; }

        public int VarietyID { get; set; }
        public string Location { get; set; }
}

