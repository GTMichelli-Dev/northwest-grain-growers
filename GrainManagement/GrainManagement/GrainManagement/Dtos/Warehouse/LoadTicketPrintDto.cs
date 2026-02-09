

namespace GrainManagement.Dtos.Warehouse
{
    public class LoadTicketPrintDto
    {
        public string Ticket { get; set; } = "";
        public string Location { get; set; } = "";
        public DateTime DateTimeIn { get; set; }
        public DateTime DateTimeOut { get; set; }
        public string Customer { get; set; } = "";
        public int WeightSheetId { get; set; }
        public string Commodity { get; set; } = "";
        public string Hauler { get; set; } = "";
        public string TruckId { get; set; } = "";

      
        public string Bin { get; set; } = "";
        public decimal Protein { get; set; }
        public int Gross { get; set; }
        public int Tare { get; set; }
        public int Net { get; set; }
    }

}
