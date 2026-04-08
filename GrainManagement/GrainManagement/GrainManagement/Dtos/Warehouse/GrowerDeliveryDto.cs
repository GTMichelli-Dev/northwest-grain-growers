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

        // ── Quantity source tracking ────────────────────────────────────────────

        /// <summary>Measurement method for DirectQty (e.g. QuantityMethodId for MANUAL, BULKLOADER, RAIL).</summary>
        public int? DirectQtyMethodId { get; set; }

        /// <summary>Source type for DirectQty (e.g. QuantitySourceTypeId for MANUAL). Must be valid for the method.</summary>
        public int? DirectQtySourceTypeId { get; set; }

        /// <summary>Location where DirectQty was captured.</summary>
        public string DirectQtyLocation { get; set; }

        /// <summary>Description of the DirectQty source — user full name (manual entry).</summary>
        public string DirectQtySourceDescription { get; set; }

        /// <summary>Measurement method for StartQty (e.g. QuantityMethodId for BULKLOADER, TRUCK_SCALE, PUMP, etc.).</summary>
        public int? StartQtyMethodId { get; set; }

        /// <summary>Source type for StartQty (e.g. QuantitySourceTypeId for MANUAL, SCALE, PUMP). Must be valid for the method.</summary>
        public int? StartQtySourceTypeId { get; set; }

        /// <summary>Location where StartQty was captured (e.g. "Endicott Elevator").</summary>
        public string StartQtyLocation { get; set; }

        /// <summary>Description of the StartQty source — user full name (manual), scale description, or pump description.</summary>
        public string StartQtySourceDescription { get; set; }

        /// <summary>Measurement method for EndQty (e.g. QuantityMethodId for BULKLOADER, TRUCK_SCALE, PUMP, etc.).</summary>
        public int? EndQtyMethodId { get; set; }

        /// <summary>Source type for EndQty (e.g. QuantitySourceTypeId for MANUAL, SCALE, PUMP). Must be valid for the method.</summary>
        public int? EndQtySourceTypeId { get; set; }

        /// <summary>Location where EndQty was captured (e.g. "Endicott Elevator").</summary>
        public string EndQtyLocation { get; set; }

        /// <summary>Description of the EndQty source — user full name (manual), scale description, or pump description.</summary>
        public string EndQtySourceDescription { get; set; }

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

        /// <summary>User ID (from users.Users) who created this transaction. Required when any quantity source is Manual.</summary>
        public int? CreatedByUserId { get; set; }

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

        // ── Transport / load-level attributes ──────────────────────────────────

        /// <summary>Bill of Lading number (BOL, AttributeTypeId=11).</summary>
        public string BOL { get; set; }

        /// <summary>Truck identifier (TRUCK_ID, AttributeTypeId=12).</summary>
        public string TruckId { get; set; }

        /// <summary>Driver name (DRIVER, AttributeTypeId=13).</summary>
        public string Driver { get; set; }
    }
}
