
namespace Seed25.DTO
{
   
 
    public class VarietyDescriptionDTO
    {
        public string Description { get; set; } = string.Empty;
        public int? Id { get; set; }
        public string  Lot { get; set; }
    }

    public class SeedTicketSummaryDTO
    {
        public Guid UID { get; set; }
       
        public long Ticket { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Status { get; set; } = Enumerations.TicketStatus.Incomplete.ToString();
        public int LocationId { get; set; }
        
        public string CustomerName { get; set; } = string.Empty;
        public string PO { get; set; } = string.Empty;
        public string BOL { get; set; } = string.Empty;
        public string TruckId { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public  string TicketType { get; set; }=Enumerations.TicketType.Unknown.ToString();

        public bool IsReturn { get; set; }

        public bool TreatedSeed { get; set; }

        public string StrTotallbs { get; set; }

        public List<string> ImagePaths { get; set; } = new();
        public List<VarietyDescriptionDTO> Varieties { get; set; } = new();
    }
}
