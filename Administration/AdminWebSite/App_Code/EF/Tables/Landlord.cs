
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


public partial class Landlord
{
        public Guid Uid { get; set; }

    public string Description { get; set; }

}
