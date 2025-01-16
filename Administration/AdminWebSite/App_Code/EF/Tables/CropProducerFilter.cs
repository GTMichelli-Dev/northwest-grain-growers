
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CropProducerFilter")]
    public partial class CropProducerFilter
    {
        [Key]
        public Guid UID { get; set; }

        public Guid ProducerUID { get; set; }

        public Guid CropUID { get; set; }
    }
