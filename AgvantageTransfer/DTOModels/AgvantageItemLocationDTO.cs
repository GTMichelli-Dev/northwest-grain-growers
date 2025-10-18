using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agvantage_Transfer.DTOModels;
    public class AgvantageItemLocationDTO
    {
        public int Id { get; set; }

        public int LocationId { get; set; }

        public decimal Price { get; set; }

        public bool Inactive { get; set; }

        public bool NotInUse { get; set; }

        public string Lot { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public decimal? DefaultValue { get; set; }
    }
