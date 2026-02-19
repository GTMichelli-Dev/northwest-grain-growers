#nullable enable
namespace GrainManagement.Dtos.Warehouse
{
    public sealed class NewTruckRequestDto
    {
        public string Mode { get; set; } = "";
        public int? ScaleId { get; set; }
        public string? ScaleName { get; set; }
        public int Weight { get; set; }
    }
}
