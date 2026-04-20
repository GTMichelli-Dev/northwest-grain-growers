namespace GrainManagement.Models;

public enum LotType
{
    Seed = 0,
    Warehouse = 1
}

public partial class Lot
{
    public string CreatedByUserName { get; set; }
    public LotType LotType { get; set; }
}
