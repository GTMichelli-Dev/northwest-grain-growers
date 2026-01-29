namespace GrainManagement.As400Sync;

public sealed record As400LandlordSplitPercentRow(
    int SplitGroupId,
    long As400AccountId,
    string Description,
    bool IsPrimaryGrower,
    decimal SplitPercent
);
