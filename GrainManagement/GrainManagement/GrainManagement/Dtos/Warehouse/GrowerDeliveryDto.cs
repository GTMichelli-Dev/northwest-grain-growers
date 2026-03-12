namespace GrainManagement.Dtos.Warehouse
{
    /// <summary>
    /// Payload for creating a grower grain delivery.
    /// Maps to an InventoryTransaction with TxnType = 'RECEIVE', Direction = +1.
    /// Grain quality attributes are flattened from the TransactionAttributes EAV table.
    /// </summary>
    public class GrowerDeliveryDto
    {
        // ── What ────────────────────────────────────────────────────────────────

        /// <summary>Lot that receives this delivery.</summary>
        public long LotId { get; set; }

        /// <summary>Product being delivered.</summary>
        public int ProductId { get; set; }

        /// <summary>Optional item variant of the product.</summary>
        public long? ItemId { get; set; }

        // ── Quantity — scale readings OR direct entry (not both) ────────────────

        /// <summary>Gross scale reading (truck + grain). Populate with EndQty for scale-based entry.</summary>
        public decimal? StartQty { get; set; }

        /// <summary>Tare scale reading (empty truck). Populate with StartQty for scale-based entry.</summary>
        public decimal? EndQty { get; set; }

        /// <summary>Net quantity for non-scale entry (hours, gallons, direct weight). Mutually exclusive with StartQty/EndQty.</summary>
        public decimal? DirectQty { get; set; }

        // ── Timing ──────────────────────────────────────────────────────────────

        /// <summary>When the truck pulled onto the scale (scale-in time).</summary>
        public DateTime? StartedAt { get; set; }

        /// <summary>When the transaction was completed (scale-out time).</summary>
        public DateTime? CompletedAt { get; set; }

        // ── Where ───────────────────────────────────────────────────────────────

        /// <summary>Receiving location (elevator / facility).</summary>
        public int LocationId { get; set; }

        /// <summary>Container (bin, tank, etc.) grain is deposited into.</summary>
        public long? ToContainerId { get; set; }

        // ── Who ─────────────────────────────────────────────────────────────────

        /// <summary>Grower account making the delivery.</summary>
        public long? AccountId { get; set; }

        /// <summary>Optional split-ownership group for this delivery.</summary>
        public int? SplitGroupId { get; set; }

        // ── Source document reference ────────────────────────────────────────────

        /// <summary>Originating document type (e.g. 'WeightSheetLoad'). Null for manual entries.</summary>
        public string RefType { get; set; }

        /// <summary>Originating document ID. Null for manual entries.</summary>
        public Guid? RefId { get; set; }

        /// <summary>Optional weight sheet to link this delivery to via WeightSheetLoad.</summary>
        public Guid? WeightSheetUid { get; set; }

        // ── Notes ───────────────────────────────────────────────────────────────

        public string Notes { get; set; }

        // ── Grain quality attributes (TransactionAttributeTypes seeds) ───────────

        /// <summary>Moisture percentage (MOISTURE).</summary>
        public decimal? Moisture { get; set; }

        /// <summary>Protein percentage (PROTEIN).</summary>
        public decimal? Protein { get; set; }

        /// <summary>Oil percentage (OIL).</summary>
        public decimal? Oil { get; set; }

        /// <summary>Starch percentage (STARCH).</summary>
        public decimal? Starch { get; set; }

        /// <summary>Test weight in lbs/bushel (TEST_WEIGHT).</summary>
        public decimal? TestWeight { get; set; }

        /// <summary>Dockage percentage (DOCKAGE).</summary>
        public decimal? Dockage { get; set; }

        /// <summary>Grade designation string (GRADE).</summary>
        public string Grade { get; set; }

        /// <summary>Foreign matter percentage (FOREIGN_MATTER).</summary>
        public decimal? ForeignMatter { get; set; }

        /// <summary>Splits percentage (SPLITS).</summary>
        public decimal? Splits { get; set; }

        /// <summary>Damaged kernels percentage (DAMAGED).</summary>
        public decimal? Damaged { get; set; }
    }
}
