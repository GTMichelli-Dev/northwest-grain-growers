using DevExpress.Portable;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Seed25.DTO
{
    public class ScaleDTO
    {
        public Guid UID { get; set; }
        public int LocationId { get; set; }
        public string ScaleDescription { get; set; }
        public bool Motion { get; set; }
        public bool Ok { get; set; }
        public  int Weight{ get; set; }
        public string Status { get; set; }=string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }=DateTime.Now.AddDays(-100);

        public string ServerMessage { get; set; } = string.Empty;
        public int MessageTimeOut { get; set; } = 3000;
        public string BackColor { get; set; } = "white";
        public string ForeColor { get; set; } = "black";


    }
}
