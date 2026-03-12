# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

GrainManagement is an ASP.NET Core 9.0 MVC + Web API application for Northwest Grain Growers. It manages grain intake, warehouse operations, lot/inventory tracking, pricing, sales, and scale hardware integration. The app is deployed to a Linux host (Vultr) behind a reverse proxy.

## Build & Run Commands

```bash
# Build
dotnet build GrainManagement.sln

# Run (development)
dotnet run --project GrainManagement.csproj

# Publish to Vultr output folder
dotnet publish GrainManagement.csproj -o publish_vultr
```

Swagger UI is available at `/swagger` in Development mode only.

## Database / EF Core

The app uses **EF Core Reverse Engineering** via the `EF Core Power Tools` Visual Studio extension (config: `efpt.config.json`). **Do not use `dotnet ef migrations`** — all models in `Models/` are scaffolded from the SQL Server database schema; modify the DB first, then re-scaffold.

```bash
# Install/restore local tools (includes dotnet-ef 10.x)
dotnet tool restore
```

Two connection strings exist:
- `DefaultConnection` — standard user (`tsssa`)
- `AdminConnection` — elevated operations, selected per-endpoint via `[UseAdminConnection]` attribute

The `[UseAdminConnection]` attribute on a controller action causes `dbContext` to be instantiated with the admin connection string (resolved in `Program.cs` via endpoint metadata).

## Configuration

- `appsettings.json` — base config (deployed)
- `appdevelopmentsettings.json` — development overrides, loaded only in `Development` environment
- `UseDemoData: true` in appsettings switches `ILocationService` to `DemoLocationService` instead of `sqlLocationService`

Key config sections:
- `AzureAd` — Microsoft Entra ID OIDC app registration
- `GrainSecurity` — Azure AD group IDs for `GrainAdmin`, `GrainManager`, `GrainUser` policies
- `Branding` — `ThemeKey` and `AllowedThemes` (server-controlled; users cannot override)
- `TicketImages` — physical path and request path for ticket image static files

## Architecture

### MVC Controllers (`Controllers/`)
Standard MVC controllers returning Views/PartialViews. Each maps to a feature area (Admin, Camera, Home, Kiosk, LoadType, Locations, Remote, Reports, Scales, Seed, Users, Warehouse).

### API Controllers (`API/`)
`[ApiController]` controllers under `api/` routes for AJAX/fetch calls from client JS. Key APIs:
- `ScaleController` — receives scale weight pushes from hardware, broadcasts via SignalR
- `WarehouseIntakeApiController` — warehouse intake data/snapshots
- `LocationsApiController`, `LookupsController`, `PrintingController`, `PrintJobsController`, `SystemApiController`, `UsersApiController`

### SignalR Hubs (`Hubs/`)
Three real-time hubs:
- `ScaleHub` (`/hubs/scale`) — typed hub (`IScaleClient`), clients join groups by `scaleId`, receives `ScaleUpdated` pushes
- `PrintHub` (`/hubs/print`) — print job notifications
- `WarehouseHub` (`/hubs/warehouse`) — warehouse intake snapshots on demand

### Services (`Services/`)
- `IScaleRegistry` / `ScaleRegistry` — **singleton** in-memory registry of scale states (ConcurrentDictionary), tracks last-update for health/staleness
- `IWarehouseIntakeDataService` / `DummyWarehouseIntakeDataService` — warehouse intake data (dummy implementation currently)
- `IWarehouseDashboardService` / `DummyWarehouseDashboardService` — warehouse dashboard data
- `ILocationService` — either `sqlLocationService` (real DB) or `DemoLocationService` based on `UseDemoData` config
- `ICurrentUser` / `CurrentUser` — scoped service exposing `IsAdmin`, `IsManager`, `IsUser` booleans derived from Azure AD group claims
- `IDeviceContext` / `DeviceContext` — scoped, reads `X-Device-Id` header or remote IP
- `ThemeMiddleware` — resolves active UI theme from config, stores in `HttpContext.Items["ThemeKey"]`
- `IJsonLog` / `JsonLog` — structured JSON logging helper

### Models (`Models/`)
EF Core entity classes scaffolded from SQL Server. The database uses multiple schemas:
- `account` — Accounts, Trucks, SplitGroups
- `chem` — ChemicalLots, ChemicalTransactions, SeedTreatmentApplications
- `container` — Containers, ContainerTypes, StorageLocations
- `Inventory` — Lots, LotTraits, LabResults, ContainerLotLayers, InventoryEvents/Movements
- `product` — Products, Categories, Traits, PriceLists, PriceListLines, PriceAdjustments
- `purchase` — PurchaseOrders, WeightSheets, WeightSheetLoads, ReceivedInventoryItems
- `sale` — SalesOrders, SalesInvoices, Payments
- `seed` — ReceivedGrades
- `system` — Locations, LocationDistricts, LocationKiosks, LocationCameras, Servers, Users, AuditTrail, UnitOfMeasures
- `users` — Users, Privileges, UserPrivileges

`dbContext.cs` is the main EF Core context. `dbContextProcedures.cs` and `dbContext.Functions.cs` contain stored procedure and SQL function mappings.

### Authorization
Three Azure AD group-based policies: `GrainAdmin`, `GrainManager`, `GrainUser`. Policies require the `groups` claim (injected via `GroupClaimsTransformation` when the group count exceeds the OIDC token limit). Inject `ICurrentUser` to check roles programmatically.

### Reporting (`Reporting/`)
DevExpress reporting used for PDF generation. `LoadTicketReport` is the main report; call `report.ExportToPdf(ms)` to generate bytes.

### Frontend
- **DevExtreme** components (grids, forms) via `DevExtreme.AspNet.Core` and `DevExtreme.AspNet.Data`
- Page-specific JavaScript lives in `wwwroot/js/pages/` (e.g., `warehouse.intake.js`, `scales.index.js`)
- SignalR client JS in `warehouse.signalr.js`
- JSON serialization is configured **without camelCase** (`PropertyNamingPolicy = null`) for both MVC and SignalR — property names must match C# casing on the client

### Static Files
Ticket images are served from an external physical path configured in `TicketImages:PhysicalPath`, mounted at `TicketImages:RequestPath` (`/ticket-images`). On Linux this is `/var/grainmanagement/ticket-images`; on dev Windows it's `C:\Images`.
