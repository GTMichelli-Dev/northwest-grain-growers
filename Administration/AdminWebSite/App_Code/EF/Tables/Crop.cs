
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Crop
    {
        [Key]
        public Guid UID { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        public bool Use_At_Elevator { get; set; }

        public bool Use_At_Seed_Mill { get; set; }

        [Required]
        [StringLength(1)]
        public string Unit_Of_Measure { get; set; }

        public bool Active { get; set; }

        public float Pound_Per_Bushel { get; set; }

        public int? Color_Index { get; set; }

        public int? Secondary_Color_Index { get; set; }
    }
