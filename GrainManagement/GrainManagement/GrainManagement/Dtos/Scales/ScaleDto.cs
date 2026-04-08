# nullable enable
using System.ComponentModel.DataAnnotations;


namespace GrainManagement.Dtos.Scales
{
    public sealed class ScaleDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Id must be >= 1.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MaxLength(100)]
        public string Description { get; set; } = string.Empty;

        public decimal Weight { get; set; }

        public bool Ok { get; set; }

        public bool Motion { get; set; }

        [MaxLength(100)]
        public string? Status { get; set; }

        /// <summary>Raw TCP response from the scale indicator.</summary>
        public string? RawResponse { get; set; }

        public DateTime LastUpdate { get; set; }

        /// <summary>Service ID that owns this scale.</summary>
        public string? ServiceId { get; set; }

        /// <summary>
        /// Location ID from system.Locations — identifies which facility the scale is at.
        /// </summary>
        public int LocationId { get; set; }

        /// <summary>
        /// Human-readable location description (e.g. "Endicott Elevator").
        /// </summary>
        [MaxLength(200)]
        public string LocationDescription { get; set; } = string.Empty;

    }
}
