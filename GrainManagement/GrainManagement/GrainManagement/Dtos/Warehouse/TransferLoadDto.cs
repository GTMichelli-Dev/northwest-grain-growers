using System.Collections.Generic;

namespace GrainManagement.Dtos.Warehouse
{
    /// <summary>
    /// Payload for creating a single transfer load on a transfer weight sheet.
    /// Maps to an InventoryTransaction with TxnType = 'TRANSFER' and a Direction
    /// flag derived from whether the load is inbound (+1) or outbound (-1) at the
    /// current location. No LotId — the variety, source, and destination ride on
    /// the WeightSheet header.
    /// </summary>
    public class TransferLoadDto
    {
        // ── Linkage ─────────────────────────────────────────────────────────────
        /// <summary>The transfer weight sheet RowUid this load belongs to.</summary>
        public Guid? WeightSheetUid { get; set; }

        // ── Quantity — scale readings OR direct entry (not both) ────────────────
        public decimal? StartQty { get; set; }
        public decimal? EndQty { get; set; }
        public decimal? DirectQty { get; set; }

        // ── Quantity source tracking (mirrors GrowerDeliveryDto) ────────────────
        public int? DirectQtyMethodId { get; set; }
        public int? DirectQtySourceTypeId { get; set; }
        public string DirectQtyLocation { get; set; }
        public string DirectQtySourceDescription { get; set; }

        public int? StartQtyMethodId { get; set; }
        public int? StartQtySourceTypeId { get; set; }
        public string StartQtyLocation { get; set; }
        public string StartQtySourceDescription { get; set; }

        public int? EndQtyMethodId { get; set; }
        public int? EndQtySourceTypeId { get; set; }
        public string EndQtyLocation { get; set; }
        public string EndQtySourceDescription { get; set; }

        public int?    StartQtyLocationQuantityMethodId { get; set; }
        public string  StartQtyLocationQuantityMethodDescription { get; set; }
        public int?    EndQtyLocationQuantityMethodId { get; set; }
        public string  EndQtyLocationQuantityMethodDescription { get; set; }
        public int?    DirectQtyLocationQuantityMethodId { get; set; }
        public string  DirectQtyLocationQuantityMethodDescription { get; set; }

        // ── Timing ──────────────────────────────────────────────────────────────
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // ── Where ───────────────────────────────────────────────────────────────
        /// <summary>Location at which the load is being weighed/captured (the current location).</summary>
        public int LocationId { get; set; }

        /// <summary>Container splits for the destination side — only meaningful for inbound transfers.</summary>
        public List<ContainerSplitDto> ToContainers { get; set; }

        // ── Who ─────────────────────────────────────────────────────────────────
        public int? CreatedByUserId { get; set; }

        // ── Source document reference ────────────────────────────────────────────
        public string RefType { get; set; }
        public Guid? RefId { get; set; }

        // ── Notes ───────────────────────────────────────────────────────────────
        public string Notes { get; set; }

        // ── PIN for any post-creation weight edits ─────────────────────────────
        public int? WeightEditPin { get; set; }

        // ── Grain quality attributes ───────────────────────────────────────────
        public decimal? Moisture { get; set; }
        public decimal? Protein { get; set; }
        public decimal? Oil { get; set; }
        public decimal? Starch { get; set; }
        public decimal? TestWeight { get; set; }
        public decimal? Dockage { get; set; }
        public string Grade { get; set; }
        public decimal? ForeignMatter { get; set; }
        public decimal? Splits { get; set; }
        public decimal? Damaged { get; set; }

        // ── Transport / load-level ─────────────────────────────────────────────
        public string BOL { get; set; }
        public string TruckId { get; set; }
        public string Driver { get; set; }
    }
}
