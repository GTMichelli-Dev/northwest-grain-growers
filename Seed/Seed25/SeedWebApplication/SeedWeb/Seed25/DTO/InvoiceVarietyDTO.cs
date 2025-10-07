namespace Seed25.DTO
{
    public class InvoiceVarietyDTO
    {
        public int ItemId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public decimal Percent { get; set; }
        public decimal Weight { get; set; }

    }
}
