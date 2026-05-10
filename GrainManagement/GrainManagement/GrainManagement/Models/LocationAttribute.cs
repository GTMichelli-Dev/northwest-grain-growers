#nullable disable
using System;

namespace GrainManagement.Models;

/// <summary>
/// Per-location key/value attribute, modeled after
/// <see cref="TransactionAttribute"/>. The current set of types lives in
/// <see cref="LocationAttributeType"/> — e.g. REQUIRE_DUMP_TYPE (bool).
/// </summary>
public partial class LocationAttribute
{
    public Guid LocationAttributesUid { get; set; }

    public int LocationId { get; set; }

    public int AttributeTypeId { get; set; }

    public decimal? DecimalValue { get; set; }

    public string StringValue { get; set; }

    public bool? BoolValue { get; set; }

    public int? IntValue { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual LocationAttributeType AttributeType { get; set; }

    public virtual Location Location { get; set; }
}
