
using System;
using System.Collections.Generic;
namespace BinData
{
    public partial class Location
    {
        public Guid Uid { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string District { get; set; }

        public bool DoNotXfer { get; set; }

        public bool InProduction { get; set; }

        public string DefaultState { get; set; }

        public virtual ICollection<Bin> Bins { get; set; } = new List<Bin>();

        public virtual LocationDistrict DistrictNavigation { get; set; }
    }
}