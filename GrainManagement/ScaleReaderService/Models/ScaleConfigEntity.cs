using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ScaleReaderService.Models
{
    /// <summary>
    /// Persistent scale configuration stored in the local SQLite database.
    /// </summary>
    [Table("ScaleConfigs")]
    public class ScaleConfigEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>Human-readable name (e.g. "Truck Scale 1").</summary>
        [MaxLength(100)]
        public string Description { get; set; } = "Unknown";

        /// <summary>IP address of the scale indicator.</summary>
        [MaxLength(50)]
        public string IpAddress { get; set; } = "127.0.0.1";

        /// <summary>TCP port.</summary>
        public int Port { get; set; } = 3001;

        /// <summary>Scale brand key (see ScaleBrandDefinition).</summary>
        [MaxLength(50)]
        public string Brand { get; set; } = "SMA";

        /// <summary>SMA request command sent each poll cycle.</summary>
        [MaxLength(50)]
        public string RequestCommand { get; set; } = "W\r\n";

        /// <summary>Character encoding ("ascii" or "utf-8").</summary>
        [MaxLength(10)]
        public string Encoding { get; set; } = "ascii";

        /// <summary>Location ID from system.Locations.</summary>
        public int LocationId { get; set; }

        /// <summary>Location description snapshot (e.g. "Endicott Elevator").</summary>
        [MaxLength(200)]
        public string LocationDescription { get; set; } = string.Empty;

        /// <summary>Whether this scale is actively polled.</summary>
        public bool Enabled { get; set; } = true;
    }
}
