using System.Numerics;

namespace Seed25.DTO
{

    
    public class InboundTicketDTO
    {
      

        public Guid UID { get; set; }
        public long  Ticket  { get; set; }
        public int Weight { get; set; }
        public DateTime TimeIn { get; set; }
        public string Plant { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    
        public string Prompt1 { get; set; } = string.Empty;

        public string Prompt2 { get; set; } = string.Empty;

    }
}
