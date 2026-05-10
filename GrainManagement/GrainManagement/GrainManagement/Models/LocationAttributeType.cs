#nullable disable
using System;

namespace GrainManagement.Models;

/// <summary>
/// Defines a known per-location attribute (e.g. REQUIRE_DUMP_TYPE).
/// Mirrors <see cref="TransactionAttributeType"/>.
/// </summary>
public partial class LocationAttributeType
{
    public int Id { get; set; }

    public string Code { get; set; }

    public string Description { get; set; }

    public string DataType { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
