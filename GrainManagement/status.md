# Grower Delivery — Session Status (2026-04-07)

## What Was Built
Reworked the Grower Delivery page (`/GrowerDelivery`) to support multiple weight methods with a compact, single-screen layout.

## Key Changes

### Weight Method System
- **LocationQuantityMethod table** (`[Inventory].[LocationQuantityMethods]`) — configures which weight methods each location supports
- **Admin UI** on `/Locations` page — "Options" button opens popup to manage weight methods per location
- MANUAL is always present and cannot be deleted; new locations auto-get MANUAL + TRUCK_SCALE
- All existing locations seeded with MANUAL + TRUCK_SCALE
- NOT FOR REPLICATION on FK constraints

### Grower Delivery Page Layout
- **Weight Method dropdown** + **Bin dropdown** on same row at top of weight section
- **Scale mode** (TRUCK_SCALE): In Weight / Out Weight / Net in horizontal cards (fixed 1/3 width each)
  - Capture Weight modal with scale buttons showing status colors (green=stable, yellow=motion, pink=error)
  - Scales disabled if: in error, in motion, below 1000 lbs, or (for outbound) exceeding inbound weight
  - Manual entry option with PIN validation
  - "Manual" badge shown next to weight when manual entry used
- **DirectQty mode** (MANUAL/RAIL/BULKLOADER): Single card with Enter Amount modal + PIN
- **Save Delivery** button in weight sheet header (top-right), hidden until weight captured
- **Load Details** section: Protein + Moisture (narrow, right-justified) with In/Out/BOL image thumbnails
- **BOL** on second row with Scan button, Truck ID, Driver
- **Notes** full-width at bottom
- Bins preloaded for location at page init

### API Changes
- `TxnType` changed from `RECEIVE` to `WAREHOUSE_RECEIVE`
- Lot ProductId/ItemId must match delivery ProductId/ItemId
- DirectQty source tracking fields added to DTO and controller
- `GET /api/GrowerDelivery/ValidatePin?pin=` — new endpoint
- `GET /api/Lookups/QuantityMethods?locationId=` — returns methods for location (fallback to all if table missing)
- `GET /api/Lookups/QuantitySourceTypes` — all active source types
- `GET/POST/DELETE /api/locations/{locationId}/QuantityMethods` — CRUD for location methods
- `GET /api/locations/AllQuantityMethods` — all active methods

### Files Modified/Created
| Area | Files |
|------|-------|
| **New model** | `Models/LocationQuantityMethod.cs` |
| **New JS** | `wwwroot/js/pages/locations.quantitymethods.js` |
| **New SQL** | `SQL/CreateLocationQuantityMethods.sql` |
| **API** | `GrowerDeliveryApiController.cs`, `LookupsController.cs`, `LocationsApiController.cs` |
| **DTO** | `GrowerDeliveryDto.cs` (DirectQty source fields) |
| **Views** | `GrowerDelivery/Index.cshtml`, `Locations/Index.cshtml`, `ScaleConfig/Index.cshtml` |
| **JS** | `grower.delivery.js` (major rework), `scaleconfig.index.js` (reverted scale type) |
| **CSS** | `grower.delivery.css` (compact layout, weight cells, thumbnails) |
| **DB config** | `dbContext.cs` (LocationQuantityMethods entity) |
| **Scale** | `ScaleRegistry.cs`, `ScaleWorker.cs` (reverted QuantityMethodId) |

## Known Issues / TODO
- **Scan button** (`#gdBolScanBtn`) is wired as a placeholder — needs camera/barcode scanner integration
- **Image thumbnails** (In/Out/BOL) are placeholder UI — need upload/camera API
- **ContainerBins endpoint** (`/api/Lookups/ContainerBins`) returns 500 — using `/api/locations/{id}/Containers` instead
- **C# changes require app restart** — the running GrainManagement process (locked exe) must be restarted to pick up API changes
- Net weight shows `0 lbs` when In = Out (correct math, but may want to flag as warning)

## Database State
- `[Inventory].[LocationQuantityMethods]` table created and seeded (91 rows — all locations have MANUAL + TRUCK_SCALE)
- `[Inventory].[QuantityMethods]` has: BULKLOADER, TRUCK_SCALE, RAIL, MANUAL, PUMP, BARGE, BAGGED, TOTE

## Git
- Commit: `10f7fee` on `master`
- Pushed to: `https://github.com/GTMichelli-Dev/northwest-grain-growers`
