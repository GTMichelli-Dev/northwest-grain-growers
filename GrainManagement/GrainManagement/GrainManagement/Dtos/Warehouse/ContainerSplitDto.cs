namespace GrainManagement.Dtos.Warehouse
{
    /// <summary>
    /// A single container split row — pairs a container with its percentage share.
    /// Used for both source and destination container assignments.
    /// </summary>
    public class ContainerSplitDto
    {
        /// <summary>Container receiving or supplying this share.</summary>
        public long ContainerId { get; set; }

        /// <summary>Percentage of the transaction allocated to this container (0 &lt; Percent &lt;= 100).</summary>
        public decimal Percent { get; set; }
    }
}
