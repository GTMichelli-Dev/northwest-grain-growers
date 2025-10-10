namespace Seed25.DTO
{
    public class InvoiceDTO
    {
        public Guid UID { get; set; }

        public long Ticket { get; set; }
        public DateTime InvoiceDate { get; set; }
   
        public string CustomerName { get; set; }
        public string  PO { get; set; }
        public string BOL { get; set; }
        public string TruckId { get; set; }
        public string Weighmaster  { get; set; }
       
        public string  Comments { get; set; }
        public string Location { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }
        public decimal TotalWeight { get; set; }
        public bool TreatedSeed { get; set; }
        public bool Coaxium  { get; set; }
        public bool Clearfield { get; set; }

        public string RequestedAmount { get; set; }

        public List< InvoiceLotDTO> invoiceLotDTOs { get; set; }= new List<InvoiceLotDTO>();
        public List<InvoiceVarietyDTO> InvoiceVarietyDTOs { get; set; }= new List<InvoiceVarietyDTO>();
        public List<InvoiceTreatmentDTO> invoiceTreatmentDTOs { get; set; } = new List<InvoiceTreatmentDTO>();
        public List<InvoiceAnalysisDTO> invoiceAnalysisDTOs { get; set; } = new List<InvoiceAnalysisDTO>();
        public List<InvoiceWeightDTO> invoiceWeightDTOs { get; set; } = new List<InvoiceWeightDTO>();
        public List<InvoiceBagDTO> invoiceBagsDTOs { get; set; } = new List<InvoiceBagDTO>();
        public List<InvoiceMiscDTO> invoiceMiscDTOs { get; set; }= new List<InvoiceMiscDTO>();
       


    }
}
