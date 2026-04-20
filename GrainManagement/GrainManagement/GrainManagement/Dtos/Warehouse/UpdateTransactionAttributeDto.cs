namespace GrainManagement.Dtos.Warehouse;

public class UpdateTransactionAttributeDto
{
    public long TransactionId { get; set; }
    public int AttributeTypeId { get; set; }
    /// <summary>Optional attribute code (e.g. "PROTEIN", "MOISTURE"). When provided, server resolves it to the current AttributeTypeId — preferred over hardcoded IDs.</summary>
    public string AttributeCode { get; set; }
    public decimal? DecimalValue { get; set; }
    public string StringValue { get; set; }
}
