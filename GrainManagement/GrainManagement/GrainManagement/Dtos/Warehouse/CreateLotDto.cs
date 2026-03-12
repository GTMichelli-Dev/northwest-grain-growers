namespace GrainManagement.Dtos.Warehouse
{
    public class CreateLotDto
    {
        public int ProductId { get; set; }
        public string LotDescription { get; set; }
        public int? SplitGroupId { get; set; }
    }
}
