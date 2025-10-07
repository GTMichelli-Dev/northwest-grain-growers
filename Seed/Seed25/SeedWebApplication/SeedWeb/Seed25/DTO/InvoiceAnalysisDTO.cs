namespace Seed25.DTO
{
    public class InvoiceAnalysisDTO
    {
        public int ItemId { get; set; }
        public string ItemDescription { get; set; }
        public  decimal PureSeed { get; set; }
        public decimal OtherCrop  { get; set; }
        public decimal  InertMatter { get; set; }
        public decimal Germination { get; set; }
        public decimal WeedSeed { get; set; }
        public DateTime DateTested { get; set; }

    }
}
