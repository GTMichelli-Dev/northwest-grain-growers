namespace GrainManagement.As400Sync;

public sealed record As400ProductItemRow(
    long ItemId,
    string ItemDescription,
    bool IsActive,
    string ProductCode,
    string Category,
    string? ProductGroup,
    long? CropId,
    string? SystemUsage,
    string? HerbicideSystem,
    string? Season,
    string? LandProgram,
    string? CertClass,
    string? Condition
);
