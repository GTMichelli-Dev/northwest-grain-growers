namespace Seed25.DTO
{
    public class InvoiceMiscDTO
    {
        public int ItemID { get; set; }
        public string Description { get; set; } =string.Empty;
        public decimal Amount { get; set; }
        public string Unit { get; set; } = "each";

    }
}
