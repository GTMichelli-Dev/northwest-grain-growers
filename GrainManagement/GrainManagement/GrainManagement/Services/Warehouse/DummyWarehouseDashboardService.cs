namespace GrainManagement.Services.Warehouse
   
{

    using GrainManagement.Models.DTO.Warehouse;
    public sealed class DummyWarehouseDashboardService : IWarehouseDashboardService
    {
        public WarehouseDashboardVm GetDashboard(int? locationId = null)
        {
            // NOTE: Later swap this class for a real implementation (DB / API).
            return new WarehouseDashboardVm
            {
                TrucksInYard = new()
            {
                new YardTruckRow { Id = 101, IsOpen = true, TicketNumber="Y-10021", Location="Moses Lake", Commodity="HRW Wheat", Hauler="Bob's Trucking", InTimeLocal="07:42", Comment="Waiting on probe" },
                new YardTruckRow { Id = 102, IsOpen = true, TicketNumber="Y-10022", Location="Moses Lake", Commodity="Barley", Hauler="Arrow Transport", InTimeLocal="08:05", Comment="" },
                new YardTruckRow { Id = 103, IsOpen = true, TicketNumber="Y-10023", Location="Kahlotus", Commodity="Corn", Hauler="Independent", InTimeLocal="08:33", Comment="Short on docs" },
            },

                IntakeWeightSheets = new()
            {
                new IntakeWeightSheetRow { WeightSheetId=2001, IsOpen=true,  WeightSheetNumber="WS-245901", Location="Moses Lake", Commodity="HRW Wheat", Lot="W-24-ML-07", Hauler="Bob's Trucking", Landlord="Smith Farms", TotalTrucks=12, TrucksInYard=2, Comment="Protein pending" },
                new IntakeWeightSheetRow { WeightSheetId=2002, IsOpen=true,  WeightSheetNumber="WS-245902", Location="Kahlotus",  Commodity="Barley",    Lot="B-24-KH-02", Hauler="Arrow Transport", Landlord="N/A", TotalTrucks=8,  TrucksInYard=1, Comment="" },
                new IntakeWeightSheetRow { WeightSheetId=2003, IsOpen=false, WeightSheetNumber="WS-245880", Location="Moses Lake", Commodity="Corn",      Lot="C-24-ML-11", Hauler="Independent", Landlord="Jones", TotalTrucks=5,  TrucksInYard=0, Comment="Closed yesterday" },
            },

                TransferWeightSheets = new()
            {
                new TransferWeightSheetRow { WeightSheetId=3001, IsOpen=true,  Direction="Inbound",  WeightSheetNumber="TR-77120", Source="Lyons Ferry", Destination="Moses Lake", Commodity="HRW Wheat", Hauler="Arrow Transport", TotalTrucks=6, TrucksInYard=1, Comment="" },
                new TransferWeightSheetRow { WeightSheetId=3002, IsOpen=true,  Direction="Outbound", WeightSheetNumber="TR-77121", Source="Moses Lake", Destination="Port Kelley", Commodity="Barley",    Hauler="Bob's Trucking", TotalTrucks=4, TrucksInYard=0, Comment="Needs seal" },
                new TransferWeightSheetRow { WeightSheetId=3003, IsOpen=false, Direction="Inbound",  WeightSheetNumber="TR-77098", Source="Kahlotus",   Destination="Moses Lake", Commodity="Corn",      Hauler="Independent", TotalTrucks=3, TrucksInYard=0, Comment="Completed" },
            },

                OutboundLoads = new()
            {
                new OutboundLoadRow { BolId=4001, IsOpen=true,  BolNumber="BOL-558901", Commodity="HRW Wheat", Hauler="Bob's Trucking", Origin="Moses Lake", Destination="Columbia Export", },
                new OutboundLoadRow { BolId=4002, IsOpen=true,  BolNumber="BOL-558902", Commodity="Barley",    Hauler="Arrow Transport", Origin="Moses Lake", Destination="Feed Mill - Pasco", },
                new OutboundLoadRow { BolId=4003, IsOpen=false, BolNumber="BOL-558850", Commodity="Corn",      Hauler="Independent", Origin="Kahlotus", Destination="Ethanol Plant", },
            }
            };
        }
    }

}
