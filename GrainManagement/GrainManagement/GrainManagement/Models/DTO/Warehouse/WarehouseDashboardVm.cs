namespace GrainManagement.Models.DTO.Warehouse
{
  
    public sealed class WarehouseDashboardVm
    {
        public List<YardTruckRow> TrucksInYard { get; set; } = new();
        public List<IntakeWeightSheetRow> IntakeWeightSheets { get; set; } = new();
        public List<TransferWeightSheetRow> TransferWeightSheets { get; set; } = new();
        public List<OutboundLoadRow> OutboundLoads { get; set; } = new();
    }

    public sealed class YardTruckRow
    {
        public int Id { get; set; }
        public bool IsOpen { get; set; }
        public string TicketNumber { get; set; } = "";
        public string Location { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string Hauler { get; set; } = "";
        public string InTimeLocal { get; set; } = "";
        public string Comment { get; set; } = "";
    }

    public sealed class IntakeWeightSheetRow
    {
        public int WeightSheetId { get; set; }
        public bool IsOpen { get; set; }
        public string WeightSheetNumber { get; set; } = "";
        public string Location { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string Lot { get; set; } = "";
        public string Hauler { get; set; } = "";
        public string Landlord { get; set; } = "";
        public int TotalTrucks { get; set; }
        public int TrucksInYard { get; set; }
        public string Comment { get; set; } = "";
    }

    public sealed class TransferWeightSheetRow
    {
        public int WeightSheetId { get; set; }
        public bool IsOpen { get; set; }
        public string Direction { get; set; } = "Inbound"; // "Inbound" or "Outbound"
        public string WeightSheetNumber { get; set; } = "";
        public string Source { get; set; } = "";
        public string Destination { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string Hauler { get; set; } = "";
        public int TotalTrucks { get; set; }
        public int TrucksInYard { get; set; }
        public string Comment { get; set; } = "";
    }

    public sealed class OutboundLoadRow
    {
        public int BolId { get; set; }
        public bool IsOpen { get; set; }
        public string BolNumber { get; set; } = "";
        public string Commodity { get; set; } = "";
        public string Hauler { get; set; } = "";
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
    }

}
