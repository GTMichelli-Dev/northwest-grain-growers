namespace Seed25.DTO
{
    public class InvoiceWeightDTO
    {
        public string TruckId { get; set; }=string.Empty;
        public long ID { get; set; } = 0;
        public decimal Gross { get; set; }
        public decimal Tare { get; set; }
        public decimal Net { get; set; }
        public string Unit { get; set; } = "lbs";
    }
}
