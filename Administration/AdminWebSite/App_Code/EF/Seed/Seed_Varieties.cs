    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Seed_Varieties
    {
        [Key]
        [Column(Order = 0)]
        public Guid Item_Location_UID { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Item_UID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Item_Id { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Location_ID { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal Price { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool ItemLocation_Inactive { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool ItemLocation_NotInUSe { get; set; }

        [StringLength(50)]
        public string Lot { get; set; }

        public string Comment { get; set; }

        public decimal? DefaultValue { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(250)]
        public string Description { get; set; }

        public int? Dept { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(255)]
        public string Item_Department { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string Item_Type { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string Item_Class { get; set; }

        [Key]
        [Column(Order = 11)]
        public bool Item_Inactive { get; set; }

        [Key]
        [Column(Order = 12)]
        public bool Item_NotInUse { get; set; }
    }
