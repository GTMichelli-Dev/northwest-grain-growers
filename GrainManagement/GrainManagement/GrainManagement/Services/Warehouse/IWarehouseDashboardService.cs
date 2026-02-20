using GrainManagement.Models.DTO.Warehouse;


namespace GrainManagement.Services.Warehouse;

public interface IWarehouseDashboardService
{
    WarehouseDashboardVm GetDashboard(int? locationId = null);
}
