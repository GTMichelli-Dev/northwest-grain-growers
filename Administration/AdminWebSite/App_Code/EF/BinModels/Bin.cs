
using System;
using System.Collections.Generic;

namespace BinData
{
    public partial class Bin
    {
        public Guid Uid { get; set; }

        public int LocationId { get; set; }

        public string Bin1 { get; set; }

        public int CropId { get; set; }

        public decimal BushelsFt { get; set; }

        public int Capacity { get; set; }

        public int Bushels { get; set; }

        public DateTime TestTimeDate { get; set; }

        public virtual ICollection<BinAdjustment> BinAdjustments { get; set; } = new List<BinAdjustment>();

        public virtual Crop Crop { get; set; }

        public virtual Location Location { get; set; }
    }
}