namespace Seed25.DTO
{
    public class TicketVarietyDTO
    {
        public Guid Uid { get; set; }

        public Guid SeedTicketUid { get; set; }

        public string PcAddress { get; set; }

        public int? VarietyId { get; set; }

        public float? PercentOfLoad { get; set; }

        public string Comment { get; set; } = string.Empty;

        public string CustomName { get; set; } = string.Empty;

        public string Lot { get; set; }

        public int? Bin { get; set; }

        public string BinName { get; set; }

        public bool? SentToPlc { get; set; }

        public string LabNumber { get; set; }

    }
}
