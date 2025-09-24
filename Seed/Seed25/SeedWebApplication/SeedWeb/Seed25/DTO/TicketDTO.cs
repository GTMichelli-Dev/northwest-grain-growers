namespace Seed25.DTO
{
    public class TicketDTO
    {
        public Guid Uid { get; set; }

        public bool Void { get; set; }

        public bool ReadOnly { get; set; }

        public bool Pending { get; set; }

        public int? Ticket { get; set; }

        public int LocationId { get; set; }

        public DateTime TicketDate { get; set; }

        public string TicketType { get; set; } = string.Empty;

        public bool Manual { get; set; }

        public string TruckId { get; set; } = string.Empty;

        public int? GrowerId { get; set; }

        public string Bol { get; set; } = string.Empty;

        public string Po { get; set; } = string.Empty;

        public int? BagCnt { get; set; }

        public int? BagSize { get; set; }

        public string Weighmaster { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;

        public string GrowerName { get; set; } = string.Empty;

        public bool Returned { get; set; }
        public List< TicketTreatmentDTO> TicketTreatmentDTOs { get; set; }= new List<TicketTreatmentDTO>();

        public List< TicketVarietyDTO> TicketVarietyDTOs { get; set; }= new List<TicketVarietyDTO>();
    }
}
