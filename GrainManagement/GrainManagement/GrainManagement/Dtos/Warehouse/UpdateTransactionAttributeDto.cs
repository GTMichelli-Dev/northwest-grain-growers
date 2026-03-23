namespace GrainManagement.Dtos.Warehouse;

public class UpdateTransactionAttributeDto
{
    public long TransactionId { get; set; }
    public int AttributeTypeId { get; set; }
    public decimal? DecimalValue { get; set; }
    public string StringValue { get; set; }
}
