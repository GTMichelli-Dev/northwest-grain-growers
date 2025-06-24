
using System;
using System.Collections.Generic;

namespace EFOptions.Dto
{

    public partial class SourceLocationFilters
    {
        public string SourceDescription { get; set; }

        public int SourceId { get; set; }


        public string  DestinationDescription { get; set; }

        public int DestinationId { get; set; }
        public bool Filtered { get; set; }


    }
}