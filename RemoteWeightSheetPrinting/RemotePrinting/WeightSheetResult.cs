using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemotePrinting
{
    public class WeightSheetResult
    {
        public Guid UID { get; set; }
        public long WeightSheetId { get; set; }

        public  bool  Transfer { get; set; }
        public string WSType { get; set; }
        public string ServerName { get; set; }
        public int LocationId { get; set; }
        public string LocationDescription { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
