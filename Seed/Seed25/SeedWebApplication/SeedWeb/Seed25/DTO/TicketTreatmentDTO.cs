namespace Seed25.DTO
{
    public class TicketTreatmentDTO
    {
        public Guid Uid { get; set; }

        public Guid SeedTicketUid { get; set; }

        public string PcAddress { get; set; }

        public int? TreatmentId { get; set; }

        public string CustomName { get; set; } = string.Empty;

        public float? Rate { get; set; }

        public string Comment { get; set; } = string.Empty;

        public bool? SentToPlc { get; set; }
    }
}
