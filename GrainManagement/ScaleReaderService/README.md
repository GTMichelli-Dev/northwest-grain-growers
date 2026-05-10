# Scale Reader Service

A .NET 9 background worker that polls one or more TCP scale indicators (SMA protocol) on the local network and pushes the readings to the GrainManagement web app over SignalR. Designed to run **one instance per site** — each instance handles every scale at that location.

The same binary runs on **Windows** and **Linux / Raspberry Pi**; there's no platform-specific code path because it just speaks TCP to scale indicators.

## How it works

```
[SMA scale indicator]  ──TCP :3001──▶  ScaleReaderService
       (one per scale)                       │
                                             ├── Polls every PollIntervalMs (default 750 ms)
                                             ├── SignalR /hubs/scale on each ServerUrls entry
                                             │       │
                                             │       ▼
                                             │   GrainManagement Web — push weight to subscribed pages
                                             │
                                             └── Forever-retry policy (capped 30s) with heartbeat
                                                 warnings every retry so a long outage isn't silent
```

1. On startup the worker loads service-level settings from `appsettings.json` → `Service` block.
2. Loads the list of scales (IP, port, brand, encoding, request command) from local SQLite at `scalereaderservice.db`.
3. Opens a SignalR connection per entry in `ServerUrls` (you can fan out to multiple web servers — central + remote — at the same time).
4. Per-scale poll loop: opens a TCP socket → sends the `RequestCommand` (default `W\r\n`) → reads the SMA reply → parses the gross/tare/net weights → broadcasts on every connected hub.
5. Reconnects forever on disconnect — `ForeverRetryPolicy` is exponent-capped so `Math.Pow(2, n)` can't overflow `TimeSpan` (the previous bug parked the connection in Disconnected after ~40 retries). Logs `SignalR still disconnected from {HubUrl}…` on every retry.
6. SignalR-issued CRUD (`AddScale` / `UpdateScale` / `DeleteScale`) lets the GrainManagement web admin manage scales remotely; changes write to the local SQLite and re-announce.

## Sparse checkout (single-service host)

A scale-only host doesn't need the rest of the monorepo. Use a git sparse + partial clone so only this folder lands on disk and `git pull` only touches it:

```bash
git clone --filter=blob:none --no-checkout https://github.com/GTMichelli-Dev/northwest-grain-growers.git
cd northwest-grain-growers
git sparse-checkout init --cone
git sparse-checkout set GrainManagement/ScaleReaderService
git checkout master
cd GrainManagement/ScaleReaderService
```

`--filter=blob:none` defers blob downloads, so blobs outside the sparse set are never fetched. `git sparse-checkout disable` reverts to a full checkout if you change your mind. Re-runs of the install script work normally inside the sparse tree.

## Quick picker

| Platform                                     | Run                                                | What it does                                                  |
|----------------------------------------------|----------------------------------------------------|---------------------------------------------------------------|
| **Raspberry Pi** (or any Debian-derived host)| `sudo ./install-pi.sh`                             | Installs .NET 9 runtime, publishes for `linux-arm64`, registers a systemd service |
| **Windows desktop / kiosk**                  | `.\install-windows.ps1` (elevated PowerShell)      | Installs .NET 9 runtime via winget, registers a Windows service |

Re-run either script after a `git pull` to upgrade — your `appsettings.json` edits and `scalereaderservice.db` survive in place.

## Configuration

### Service-level (`appsettings.json` → `Service`)

| Field                  | Default                          | Purpose                                                                  |
|------------------------|----------------------------------|--------------------------------------------------------------------------|
| `ServiceId`            | `default`                        | Unique id for this instance (e.g. `endicott`, `colfax`). Used by the web to route commands. |
| `ServerUrls`           | `["http://localhost:5000"]`      | One or more GrainManagement web URLs. Each gets its own SignalR connection. |
| `SignalRHub`           | `/hubs/scale`                    | Hub path on the web app                                                  |
| `LocationId`           | `0`                              | Pairs the service with a `system.Locations` row                          |
| `LocationDescription`  | `""`                             | Friendly site label                                                      |
| `PollIntervalMs`       | `750`                            | Per-scale poll cadence                                                   |
| `TimeoutMs`            | `1000`                           | TCP request timeout                                                      |
| `ReconnectBackoffMs`   | `500`                            | Initial reconnect backoff (the policy doubles, capped at `MaxBackoffMs`) |
| `MaxBackoffMs`         | `5000`                           | Ceiling for the initial connect retry loop                               |

After editing `appsettings.json` restart the service: `sudo systemctl restart scale-reader-service` (Pi) or `Restart-Service ScaleReaderService` (Windows).

### Per-scale config (SQLite `ScaleConfigs` table)

Managed remotely from the GrainManagement web app's **System → Scales** admin page over SignalR. Each row has:

| Field                  | Purpose                                                                  |
|------------------------|--------------------------------------------------------------------------|
| `Description`          | Friendly name (e.g. "Truck Scale 1")                                     |
| `IpAddress` / `Port`   | TCP target on the LAN (default port `3001` for SMA)                      |
| `Brand`                | Scale brand key (see `ScaleBrandDefinition`)                             |
| `RequestCommand`       | Bytes sent per poll (default `W\r\n` — SMA's gross weight request)       |
| `Encoding`             | `ascii` or `utf-8`                                                       |
| `LocationId`           | `system.Locations.LocationId` the scale lives at                         |
| `Enabled`              | Inactive rows are skipped                                                |

You can also edit `scalereaderservice.db` directly with any SQLite tool (DB Browser for SQLite, sqlite3 CLI) for bootstrap / debugging.

## SignalR hub contract

Web → service (the web sends, the service handles):

| Method                                       | Args                          | Purpose                              |
|----------------------------------------------|-------------------------------|--------------------------------------|
| `AddScale` / `UpdateScaleConfig` / `DeleteScale` | scale CRUD payloads        | CRUD persisted to local SQLite       |
| `GetScaleList` / `GetScaleBrands`            | —                             | Replies with the current list        |
| `Announce`                                   | —                             | Re-send the scale-announcement to the web |
| `ReloadConfig`                               | —                             | Tear down + rebuild the SignalR conn |

Service → web:

| Method                  | Args                                  | Purpose                              |
|-------------------------|---------------------------------------|--------------------------------------|
| `JoinScaleGroup`        | `serviceId`                           | Join the SignalR group on connect    |
| `AnnounceScales`        | `{ ServiceId, Scales }`               | "These scales live behind me"        |
| `WeightUpdate`          | `{ scaleId, gross, tare, net, units, motion, …}` | Pushed every poll cycle      |
| `ScaleCrudResult`       | `{ success, operation, scaleId, … }`  | Reply per CRUD command               |

## Manual install (when you don't want to run the scripts)

### Linux / Pi

```bash
# .NET 9 ASP.NET Core runtime — apt path
wget https://packages.microsoft.com/config/$(. /etc/os-release && echo $ID)/$(. /etc/os-release && echo $VERSION_ID)/packages-microsoft-prod.deb -O /tmp/ms.deb
sudo dpkg -i /tmp/ms.deb && sudo apt update
sudo apt install -y aspnetcore-runtime-9.0

dotnet publish -c Release -r linux-arm64 --self-contained false -o /opt/scale-reader-service
# … then write /etc/systemd/system/scale-reader-service.service referencing
# /opt/scale-reader-service/ScaleReaderService.dll and `systemctl enable --now`.
```

### Windows

```powershell
winget install --id Microsoft.DotNet.AspNetCore.9 --silent
dotnet publish -c Release -r win-x64 --self-contained false -o C:\Services\ScaleReaderService
sc.exe create "ScaleReaderService" binPath="C:\Services\ScaleReaderService\ScaleReaderService.exe" start=auto
sc.exe start ScaleReaderService
```

## Operational notes

- **No public listener.** Unlike the camera + print services, ScaleReaderService doesn't expose any HTTP endpoints — there's nothing to open on the firewall. It only makes outbound SignalR connections (to the web app) and outbound TCP connections (to scale indicators).
- **No CUPS / printer dependency** — the service just speaks TCP to indicators.
- **Multiple scales per service** — the `ScaleConfigs` table is a list; one instance polls every active row in parallel.
- **Multiple servers per service** — `ServerUrls` is a list. Each URL gets its own SignalR connection, so the same weight reading can fan out to a Central HQ and a Remote site simultaneously.
- **Forever reconnect** — both the initial connect (in `ConnectWithRetryAsync`) and the auto-reconnect (`ForeverRetryPolicy`) retry indefinitely with capped backoff and per-retry warning logs.
- **Service survives unhandled exceptions** — `BackgroundServiceExceptionBehavior=Ignore` plus a `TaskScheduler.UnobservedTaskException` handler in `Program.cs` keep the host alive when a transport-level error escapes.

## Requirements

- .NET 9 ASP.NET Core runtime (installed by the scripts)
- TCP reach to each scale indicator on its configured port
- Outbound SignalR reach to every URL in `ServerUrls`
- Local SQLite (no separate DB server — the file lives next to the binary)
