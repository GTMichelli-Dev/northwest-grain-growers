
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Producer
    {
        [Key]
        public Guid UID { get; set; }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Email_Address { get; set; }

        public bool Email_WS { get; set; }

        public bool Print_WS { get; set; }

        public bool Active { get; set; }

        [StringLength(255)]
        public string Company_Name { get; set; }

        [StringLength(255)]
        public string Customer_Name1 { get; set; }

        [StringLength(255)]
        public string Customer_Name2 { get; set; }

        [StringLength(255)]
        public string Address1 { get; set; }

        [StringLength(255)]
        public string Address2 { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(50)]
        public string State { get; set; }

        [StringLength(20)]
        public string Zip1 { get; set; }

        [StringLength(20)]
        public string Zip2 { get; set; }

        [StringLength(16)]
        public string Home_Phone { get; set; }

        [StringLength(16)]
        public string Work_Phone { get; set; }

        [StringLength(16)]
        public string Mobile_Phone { get; set; }

        [StringLength(16)]
        public string Phone { get; set; }

        [StringLength(10)]
        public string Member { get; set; }
    }
