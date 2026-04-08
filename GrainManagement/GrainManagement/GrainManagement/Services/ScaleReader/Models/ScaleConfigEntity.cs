using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrainManagement.Services.ScaleReader.Models
{
    /// <summary>
    /// Persistent scale configuration stored in the local SQLite database.
    /// Each row represents one physical scale indicator.
    /// </summary>
    [Table("ScaleConfigs")]
    public class ScaleConfigEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Logical scale identifier sent to the server (matches ScaleDto.Id).
        /// </summary>
        public int ScaleId { get; set; }

        /// <summary>
        /// Human-readable name for this scale (e.g. "Truck Scale 1").
        /// </summary>
        [MaxLength(100)]
        public string Description { get; set; } = "Unknown";

        /// <summary>
        /// IP address of the scale indicator or SMA bridge.
        /// </summary>
        [MaxLength(50)]
        public string IpAddress { get; set; } = "127.0.0.1";

        /// <summary>
        /// TCP port of the scale indicator or SMA bridge.
        /// </summary>
        public int Port { get; set; } = 3001;

        /// <summary>
        /// Scale brand key that selects the protocol/parser (see ScaleBrandDefinition).
        /// </summary>
        [MaxLength(50)]
        public string Brand { get; set; } = "SMA";

        /// <summary>
        /// SMA request command sent each poll cycle.
        /// Common values: "W\r\n", "\u0005" (ENQ).
        /// </summary>
        [MaxLength(50)]
        public string RequestCommand { get; set; } = "W\r\n";

        /// <summary>
        /// Character encoding for the TCP stream ("ascii" or "utf-8").
        /// </summary>
        [MaxLength(10)]
        public string Encoding { get; set; } = "ascii";

        /// <summary>
        /// Whether this scale is actively polled.
        /// </summary>
        public bool Enabled { get; set; } = true;

    }
}
