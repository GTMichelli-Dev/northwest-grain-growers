# Grain Management AS/400 Sync Service

A .NET 8 ASP.NET Core service that mirrors selected Agvantage (IBM i / AS400 / DB2-for-i) data into the local `GrainManagement` SQL Server, and uploads finished warehouse-transfer weight sheets back the other way. One instance per Central deployment.

## What it does

```
[Agvantage AS400 / DB2-for-i]                     [GrainManagement SQL Server]
        ▲                                                       ▲
        │ ODBC (IBM i Access)                                   │ Microsoft.Data.SqlClient
        │                                                       │
        └────────────── As400Sync.Service ──────────────────────┘
                              │
                              ├── HTTP :5080  → /swagger, /api/sync/*  (X-Api-Key)
                              │
                              └── SignalR client → web app /hubs/as400sync
                                       (RunAccounts / RunProducts / RunSplitGroups /
                                        RunWarehouseTransferUpload / RunClearU5Siload)
```

Three pull-from-Agvantage jobs (read via ODBC, upsert into SQL Server):

| Job          | Source query                       | Target table(s)                              |
|--------------|------------------------------------|----------------------------------------------|
| `Accounts`   | `Accounts.sql`                     | `dbo.Accounts` (+ phones / address columns)  |
| `Products`   | `AllProductItems.sql`              | `dbo.Items` and item attributes              |
| `SplitGroups`| `LandLordSplitPercentages.sql`     | `dbo.SplitGroups` + `dbo.SplitGroupMembers`  |

Two push-to-Agvantage commands:

| Command                     | What it does                                                                  |
|-----------------------------|-------------------------------------------------------------------------------|
| `RunWarehouseTransferUpload`| Streams the given weight-sheet IDs into Agvantage's `U5SILOAD` staging table. |
| `RunClearU5Siload`          | Truncates `U5SILOAD` — used after Agvantage has consumed the staged batch.    |

The service exposes the same jobs three ways:

1. **Manually** via the GrainManagement web admin (`/Admin/As400Sync`) → SignalR command.
2. **Manually** via REST (`POST /api/sync/accounts | /products | /splitgroups`) with the `X-Api-Key` header.
3. **Automatically** every `RunEveryMinutes` if the corresponding `Sync*` flag is `true`.

## Platform

**Windows only.** The IBM i Access ODBC Driver (used by the AS/400 connection string in `appsettings.json`) ships only on Windows as part of [IBM i Access Client Solutions](https://www.ibm.com/support/pages/ibm-i-access-client-solutions). There's no Linux / Pi installer for this service because the driver itself isn't available there.

## Sparse checkout (single-service host)

The HQ sync server doesn't need the rest of the monorepo. Use a git sparse + partial clone so only this folder lands on disk and `git pull` only touches it:

```powershell
git clone --filter=blob:none --no-checkout https://github.com/GTMichelli-Dev/northwest-grain-growers.git
cd northwest-grain-growers
git sparse-checkout init --cone
git sparse-checkout set GrainManagement/GrainManagement.As400Sync/GrainManagement.As400Sync.Service
git checkout master
cd GrainManagement\GrainManagement.As400Sync\GrainManagement.As400Sync.Service
```

`--filter=blob:none` defers blob downloads, so blobs outside the sparse set are never fetched. `git sparse-checkout disable` reverts to a full checkout if you change your mind. Re-runs of the install script work normally inside the sparse tree.

## Quick picker

| Platform                | Run                                            | What it does                                              |
|-------------------------|------------------------------------------------|-----------------------------------------------------------|
| **Windows (HQ server)** | `.\install-windows.ps1` (elevated PowerShell)  | Installs .NET 8 runtime via winget, registers a Windows service, opens TCP 5080 in the firewall, preserves `appsettings.json` across upgrades |

Re-run `install-windows.ps1` after a `git pull` to upgrade — the script stops the service, republishes, and starts it again. Your edited `appsettings.json` (which holds the SQL + ODBC connection strings, hub URL, and API key) survives the upgrade in place.

Before the first install make sure **IBM i Access Client Solutions → ODBC Driver** is installed on the host. The .NET service can't open the AS/400 connection without it.

## Configuration

All settings live in `appsettings.json` (next to the binary after install — default `C:\Services\As400SyncService\appsettings.json`).

### Connection strings

```jsonc
"ConnectionStrings": {
  "SqlServer":  "Server=.;Database=GrainManagement;User Id=sa;Password=...;Encrypt=True;TrustServerCertificate=True;",
  "As400Odbc":  "Driver={IBM i Access ODBC Driver};System=ASPE.AGVANTAGE.COM;Uid=...;Pwd=...;Naming=1;CommitMode=2;DefaultLibraries=COMDATA;"
}
```

`As400Odbc` is the exact ODBC string passed to `OdbcConnection` — same shape you'd put in `odbcad32.exe`. `Naming=1` selects SQL naming, `CommitMode=2` is autocommit.

**`DefaultLibraries` is the per-deployment knob.** The shipped SQL files (`Accounts.sql`, `AllProductItems.sql`, `LandLordSplitPercentages.sql`) reference the Agvantage tables *unqualified* (`FROM U4CSTMR`, `FROM U5SPLTS`, …). Whichever library the ODBC user can read these from goes here:

| Deployment        | `DefaultLibraries` |
|-------------------|--------------------|
| Production (ASPE) | `COMDATA`          |
| Test / dev (NWGG_TEST / Pstpwr9 / etc.) | `COPDATA` |

If the user can read multiple libraries, list the candidates in priority order separated by commas (e.g. `DefaultLibraries=COMDATA,COPDATA`). Moving the service between LPARs is then a connection-string edit — no SQL changes needed.

### Kestrel binding

```jsonc
"Kestrel": {
  "Endpoints": {
    "Http": { "Url": "http://0.0.0.0:5080" }
  }
}
```

Bound explicitly to **5080** so the service doesn't collide with the website's default `:5000`. Change this here if you need a different port; the installer also exposes a `-Port` parameter for first-time setup, but after install the appsettings value is authoritative.

### Sync block

```jsonc
"As400Sync": {
  "BatchSize": 2000,
  "RunEveryMinutes": 60,

  // Auto-update flags — leave false unless you want the worker loop
  // to drive each job on the RunEveryMinutes cadence. Defaults to false
  // so the service stays idle until an admin triggers a job from the website.
  "SyncAccounts": false,
  "SyncProducts": false,
  "SyncSplitGroups": false,

  // SignalR hub on the website. The service connects on startup as ServiceId.
  "HubUrl":    "http://localhost:5000/hubs/as400sync",
  "ServiceId": "as400sync"
}
```

### API key

`/api/sync/accounts | /products | /splitgroups` require an `X-Api-Key` header. Set the expected value under `Security:ApiKey`:

```jsonc
"Security": {
  "ApiKey": "long-random-string"
}
```

`/api/sync/status` and `/api/sync/info` are anonymous — the admin page polls `/api/sync/info` cheaply to render the "service info" panel.

After editing `appsettings.json` restart the service: `Restart-Service As400SyncService`.

## SignalR hub contract

Web → service (the web sends, the service handles):

| Method                         | Args                | Purpose                                            |
|--------------------------------|---------------------|----------------------------------------------------|
| `RunAccounts`                  | —                   | Kick off the Accounts sync                         |
| `RunProducts`                  | —                   | Kick off the Products / Items sync                 |
| `RunSplitGroups`               | —                   | Kick off the LandLord split-percentage sync        |
| `RunWarehouseTransferUpload`   | `long[] wsIds`      | Push these weight sheets into Agvantage `U5SILOAD` |
| `RunClearU5Siload`             | —                   | Truncate `U5SILOAD` after Agvantage consumes it    |
| `RequestSnapshot`              | —                   | Re-send the busy/idle snapshot                     |

Service → web:

| Method            | Args                                              | Purpose                                       |
|-------------------|---------------------------------------------------|-----------------------------------------------|
| `RegisterService` | `serviceId`                                       | Announce on (re)connect                       |
| `ReportStatus`    | `SyncProgress` (Stage, Current, Total, Message)   | Streamed every step so the UI can render a bar |
| `ReportError`     | `{ ServiceId, Job, Message, … }`                  | Per-item or fatal error                       |
| `ReportCompleted` | `{ ServiceId, Job, Message, … }`                  | Final ack                                     |

The hub client uses a custom `TenSecondForeverRetryPolicy` so a long website outage doesn't park the service in Disconnected. Reconnects are logged at Warning level so a multi-hour outage isn't silent.

## REST endpoints

| Verb  | Path                       | Auth         | Purpose                                                  |
|-------|----------------------------|--------------|----------------------------------------------------------|
| POST  | `/api/sync/accounts`       | `X-Api-Key`  | Same as `RunAccounts` from the hub                       |
| POST  | `/api/sync/products`       | `X-Api-Key`  | Same as `RunProducts`                                    |
| POST  | `/api/sync/splitgroups`    | `X-Api-Key`  | Same as `RunSplitGroups`                                 |
| GET   | `/api/sync/status`         | anonymous    | `{ accounts, products, splitGroups }` busy/started-at    |
| GET   | `/api/sync/info`           | anonymous    | Adds version + serviceId + hubUrl for the admin info panel |
| any   | `/swagger`                 | anonymous    | Swagger UI                                               |

All POST endpoints return `409 Conflict` if the matching job is already running — only one of each job at a time.

## Manual install (when you don't want to run the script)

```powershell
# 1. Install prerequisites
winget install --id Microsoft.DotNet.AspNetCore.8 --silent
# IBM i Access Client Solutions ODBC driver — download from IBM (no winget id):
#   https://www.ibm.com/support/pages/ibm-i-access-client-solutions

# 2. Publish
dotnet publish -c Release -r win-x64 --self-contained false -o C:\Services\As400SyncService

# 3. Register
sc.exe create As400SyncService binPath= "C:\Services\As400SyncService\GrainManagement.As400Sync.exe" start= auto
sc.exe failure As400SyncService reset= 86400 actions= restart/5000/restart/5000/restart/5000
sc.exe start As400SyncService

# 4. Open the firewall (admin page on a different LAN host needs to reach :5080)
New-NetFirewallRule -DisplayName "Grain Management AS400 Sync" `
    -Direction Inbound -Action Allow -Protocol TCP -LocalPort 5080
```

## Operational notes

- **One service per Central deployment.** The web app's `/hubs/as400sync` is single-tenant — multiple sync services pointing at the same hub would all fight for the same SignalR group. The admin page expects exactly one `ServiceId`.
- **Service survives website outages.** Indefinite 10-second SignalR reconnect with per-attempt warning logs.
- **Service survives AS/400 outages.** A failed sync logs the error and the next scheduled run picks back up; no crash-loop.
- **U5SILOAD is single-batch.** The pattern is: web → `RunWarehouseTransferUpload` (inserts) → Agvantage consumes → web → `RunClearU5Siload` (truncates). Don't run a second upload until Agvantage has finished with the previous batch.
- **No SQLite.** Unlike the camera / print / scale services, this one has nothing local to persist — all state lives in the GrainManagement SQL Server.
- **Auto-update flags are off by default.** Even if `RunEveryMinutes=60`, nothing runs until at least one `SyncAccounts | SyncProducts | SyncSplitGroups` is flipped to `true`. The recommended pattern is to leave them all `false` and drive jobs from `/Admin/As400Sync`.

## Requirements

- Windows host with .NET 8 ASP.NET Core runtime (installed by the script).
- **IBM i Access Client Solutions — ODBC Driver** installed on the same host (see above). Not auto-installable via winget; download from IBM.
- Network reach to the Agvantage AS/400 host on its standard DRDA / DDM port (typically `446`) — `telnet ASPE.AGVANTAGE.COM 446` from the service host is a useful smoke test.
- TCP 1433 reach to the `GrainManagement` SQL Server (usually the same host).
- Outbound HTTP to the `HubUrl` (the GrainManagement website's `/hubs/as400sync`).
