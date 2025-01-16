
using System;
using System.Collections.Generic;
namespace BinData
{

    public partial class LocationDistrict
    {
        public Guid Uid { get; set; }

        public string District { get; set; }

        public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
    }
}