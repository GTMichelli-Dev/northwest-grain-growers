namespace GrainManagement.As400Sync;

public sealed record As400AccountRow(
    long As400AccountId,        // CSCNO -> BIGINT
    bool Active,
    string EntityName,
    string LookupName,
    string OwnerFirstName,
    string OwnerLastName,
    bool IsProducer,
    bool IsWholeSale,
    bool IsAutoPriced,
    bool PaysRoyalities,
    DateTime? TaxExemptDate,
    string? TaxId,
    DateTime CreatedAt,
    string? Email,
    bool EmailWeightSheet,
    bool PrintWeightSheet,
    bool EmailStatement,
    bool PrintStatement,
    string? Address1,
    string? Address2,
    string? City,
    string? State,
    string? Zip,
    string? Country,
    string? Phone1,
    string? Phone2,
    string? MobilePhone,
    bool CustomerPaysRoyalties,
    string Contact,
    string Notes
);
