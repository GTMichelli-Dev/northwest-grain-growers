
using System;
using System.Collections.Generic;
namespace BinData
{
    public partial class LocationDTO
    {
        public Guid Uid { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public bool Active { get; set; }

        public string District { get; set; }

        public bool DoNotXfer { get; set; }

        public bool InProduction { get; set; }

        public string DefaultState { get; set; }
        
        
    }
}