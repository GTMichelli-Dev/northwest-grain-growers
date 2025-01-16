using System;
using System.Collections.Generic;
namespace BinData
{

    public partial class BinAdjustmentDTO
    {
        public Guid Uid { get; set; }

        public Guid BinUid { get; set; }

        public DateTime AdjustedDate { get; set; }

        public int Bushels { get; set; }

        public float? Protein { get; set; } = 0;

        public string Comment { get; set; }

    }
}