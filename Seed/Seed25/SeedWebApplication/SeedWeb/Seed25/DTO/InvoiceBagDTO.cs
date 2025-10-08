namespace Seed25.DTO
{
    public class InvoiceBagDTO
    {
        public long ID { get; set; } = 0;
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "lbs";
    }
}
