
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Seed_Items
    {
        [Key]
        [Column(Order = 0)]
        public Guid UID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(250)]
        public string Description { get; set; }

        public int? Dept { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Item_Class { get; set; }

        public int? Store_Location { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string ItemType { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FLC { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(1)]
        public string UOMCode { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool Inactive { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool NotInUse { get; set; }

        public string Comment { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string Class_Description { get; set; }

        [Key]
        [Column(Order = 10)]
        public decimal Pure_Seed { get; set; }

        [Key]
        [Column(Order = 11)]
        public decimal Other_Crop { get; set; }

        [Key]
        [Column(Order = 12)]
        public decimal Inert_Matter { get; set; }

        [Key]
        [Column(Order = 13)]
        public decimal Germination { get; set; }

        [Key]
        [Column(Order = 14)]
        public decimal Weed_Seed { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(255)]
        public string Dept_Description { get; set; }
    }
