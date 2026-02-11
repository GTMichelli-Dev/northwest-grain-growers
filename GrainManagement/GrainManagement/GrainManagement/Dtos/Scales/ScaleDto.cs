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

        public DateTime LastUpdate { get; set; }
    }
}
