
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SeedInvoiceItem
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int orderIdx { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Returned { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Location_ID { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime DateOfTransaction { get; set; }

        public int? Customer { get; set; }

        public int? Ticket { get; set; }

        [StringLength(4000)]
        public string TicketDate { get; set; }

        [StringLength(10)]
        public string Invoice { get; set; }

        public decimal? Quantity { get; set; }

        public int? Item { get; set; }
    }
