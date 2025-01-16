
using System;
using System.Collections.Generic;

namespace BinData
{
    public class BinDTO
    {
        public DateTime? AdjustedDate { get; set; }
        public int LocationId { get; set; }
        public string Bin { get; set; }
        public string Location { get; set; }
        public Guid BinUID { get; set; }
        public string District { get; set; }
        public decimal Bushels { get; set; }
        public float Protein { get; set; }
        public string Comment { get; set; }
        public Guid AdjustmentUID { get; set; }

        public List<BinAdjustmentDTO> BinAdjustmentDTOs { get; set; }
    }

}