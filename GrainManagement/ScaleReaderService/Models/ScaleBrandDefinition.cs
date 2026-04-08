namespace ScaleReaderService.Models
{
    public sealed class ScaleBrandDefinition
    {
        public string BrandKey { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public int DefaultPort { get; set; } = 3001;
        public string DefaultRequestCommand { get; set; } = "W\r\n";
        public string DefaultEncoding { get; set; } = "ascii";

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
