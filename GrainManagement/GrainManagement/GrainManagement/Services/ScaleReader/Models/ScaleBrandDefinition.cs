namespace GrainManagement.Services.ScaleReader.Models
{
    /// <summary>
    /// Defines a scale brand/protocol and its default communication parameters.
    /// Used to provide sensible defaults when configuring new scale indicators.
    /// </summary>
    public sealed class ScaleBrandDefinition
    {
        /// <summary>
        /// Unique brand key (e.g. "SMA", "Rice_Lake", "Avery_Weigh_Tronix", "Cardinal").
        /// </summary>
        public string BrandKey { get; set; } = "";

        /// <summary>
        /// Display name shown in the UI.
        /// </summary>
        public string DisplayName { get; set; } = "";

        /// <summary>
        /// Default TCP port for this brand.
        /// </summary>
        public int DefaultPort { get; set; } = 3001;

        /// <summary>
        /// Default SMA request command (may include escape sequences like \r\n or \u0005).
        /// </summary>
        public string DefaultRequestCommand { get; set; } = "W\r\n";

        /// <summary>
        /// Default character encoding ("ascii" or "utf-8").
        /// </summary>
        public string DefaultEncoding { get; set; } = "ascii";

        /// <summary>
        /// Well-known scale brand definitions.
        /// </summary>
        public static readonly ScaleBrandDefinition[] KnownBrands = new[]
        {
            new ScaleBrandDefinition
            {
                BrandKey = "SMA",
                DisplayName = "SMA (Generic)",
                DefaultPort = 3001,
                DefaultRequestCommand = "W\r\n",
                DefaultEncoding = "ascii"
            },
            new ScaleBrandDefinition
            {
                BrandKey = "Rice_Lake",
                DisplayName = "Rice Lake",
                DefaultPort = 3001,
                DefaultRequestCommand = "W\r\n",
                DefaultEncoding = "ascii"
            },
            new ScaleBrandDefinition
            {
                BrandKey = "Avery_Weigh_Tronix",
                DisplayName = "Avery Weigh-Tronix",
                DefaultPort = 3001,
                DefaultRequestCommand = "W\r\n",
                DefaultEncoding = "ascii"
            },
            new ScaleBrandDefinition
            {
                BrandKey = "Cardinal",
                DisplayName = "Cardinal",
                DefaultPort = 3001,
                DefaultRequestCommand = "\u0005",
                DefaultEncoding = "ascii"
            }
        };
    }
}
