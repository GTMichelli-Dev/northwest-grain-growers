using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UpdateZipCodes.Models
{

    public class DownloadedZipCodes
    {
        public int zip_code { get; set; }


        [JsonIgnore]
        public double latitude { get; set; }


        [JsonIgnore]
        public double longitude { get; set; }
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public string county { get; set; } = string.Empty;
    }


}
