namespace GrainManagement.Models;

public partial class LocationQuantityMethod
{
    public int LocationId { get; set; }

    public int QuantityMethodId { get; set; }

    public virtual Location Location { get; set; }

    public virtual QuantityMethod QuantityMethod { get; set; }
}
