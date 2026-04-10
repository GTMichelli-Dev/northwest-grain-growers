namespace GrainManagement.Dtos.Warehouse;

public class VoidDeliveryDto
{
    /// <summary>User PIN for authorization.</summary>
    public int Pin { get; set; }

    /// <summary>Reason for voiding the delivery ticket.</summary>
    public string VoidReason { get; set; }
}
