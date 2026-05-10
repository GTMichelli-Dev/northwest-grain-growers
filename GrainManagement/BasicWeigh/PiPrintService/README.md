# Print Service (PiPrintService)

A .NET 8 background worker + Web API that connects to the GrainManagement web app over SignalR and renders ticket / weight-sheet PDFs to a real printer on the host. Despite the "Pi" name, the service runs on **both Linux (CUPS) and Windows (Print Spooler)** — the right driver is wired up at startup via `OperatingSystem.IsWindows()` in `Program.cs`.

## How it works

```
[GrainManagement Web]  ──SignalR /scaleHub──▶  PrintService
                                                    ├── IPrintClient (CUPS on Linux / Print Spooler on Windows)
                                                    │       │
                                                    │       ▼
                                                    │    Physical / network printer
                                                    │
                                                    ├── GET /api/printers              (printer list)
                                                    ├── POST /api/printers/{id}/test   (test page)
                                                    ├── GET/PUT /api/settings          (SQLite-backed config)
                                                    └── Swagger UI /swagger
```

1. On startup the worker loads `ServiceSettings` from `webprintservice.db` (SQLite, next to the binary).
2. Connects to the web's SignalR hub (`/scaleHub` by default) and joins a group keyed by `ServiceId`.
3. Announces every local printer so the web app's printer-picker shows them.
4. Listens for `PrintTicket` commands; on each one downloads the PDF from the web app and submits it to the local print system (CUPS or Print Spooler) for the named printer.
5. Reconnects forever on disconnect — `ForeverRetryPolicy` capped at 30s, with heartbeat warnings on every retry so a long outage isn't silent.

## Sparse checkout (single-service host)

A print-only host doesn't need the rest of the monorepo. Use a git sparse + partial clone so only this folder lands on disk and `git pull` only touches it:

```bash
git clone --filter=blob:none --no-checkout https://github.com/GTMichelli-Dev/northwest-grain-growers.git
cd northwest-grain-growers
git sparse-checkout init --cone
git sparse-checkout set GrainManagement/BasicWeigh/PiPrintService
git checkout master
cd GrainManagement/BasicWeigh/PiPrintService
```

`--filter=blob:none` defers blob downloads, so blobs outside the sparse set are never fetched. `git sparse-checkout disable` reverts to a full checkout if you change your mind. Re-runs of the install script work normally inside the sparse tree.

## Quick picker

| Platform                                     | Run                                                | What it does                                                  |
|----------------------------------------------|----------------------------------------------------|---------------------------------------------------------------|
| **Raspberry Pi** (or any Debian-derived host)| `sudo ./install-pi.sh`                             | Installs .NET 8 + CUPS, **configures CUPS to listen on the LAN at :631**, publishes + runs as a systemd service |
| **Windows desktop / kiosk**                  | `.\install-windows.ps1` (elevated PowerShell)      | Installs .NET 8 + SumatraPDF via winget, ensures Print Spooler is running, registers a Windows service |

Re-run either script after a `git pull` to upgrade — the SQLite settings DB and any operator config you've set via Swagger are preserved across upgrades.

## Service settings (single-row `Settings` table)

| Field         | Default                          | Purpose                                              |
|---------------|----------------------------------|------------------------------------------------------|
| `ServiceId`   | `default`                        | Routes print commands when multiple services connect |
| `ServerUrl`   | `http://localhost:5110`          | GrainManagement web base URL                         |
| `SignalRHub`  | `/scaleHub`                      | Hub path on the web                                  |

Edit via the Swagger UI (`PUT /api/settings`) or directly in SQLite. The service auto-restarts its hub connection after a settings change.

## Endpoints

| Method + path                              | Purpose                                          |
|--------------------------------------------|--------------------------------------------------|
| `GET  /api/status/health`                  | Liveness + print-system + printer count          |
| `GET  /api/printers`                       | Printers visible to the host                     |
| `GET  /api/printers/{id}/status`           | Detailed status of a single printer              |
| `POST /api/printers/{id}/test`             | Send a test page to a printer                    |
| `GET/PUT /api/settings`                    | Read/write `ServiceSettings`                     |
| `POST /api/status/restart`                 | Reload settings + reconnect the SignalR hub      |
| `GET  /api/readme`                         | This file, served raw                            |
| Swagger UI: `/swagger`                     |                                                  |

## CUPS — making the print server publicly reachable (Linux / Pi)

The Pi installer **configures CUPS to accept print jobs from the LAN** out of the box — this is the "make CUPS available publicly" part. Specifically it:

1. Installs `cups`, `cups-bsd`, and `cups-client` via apt
2. Adds the service user (default `pi`) to the `lpadmin` group so they can manage printers from the web UI at `http://<pi>:631/`
3. Edits `/etc/cups/cupsd.conf` to:
   - `Listen 0.0.0.0:631` (instead of `Listen localhost:631`)
   - `Browsing On` + `BrowseLocalProtocols dnssd`
   - In the `<Location />`, `<Location /admin>`, and `<Location /admin/conf>` blocks: `Allow @LOCAL` so any LAN client can submit jobs and `lpadmin` users on the LAN can manage printers
4. Restarts `cups.service` to apply the change
5. Opens UFW (when enabled) for **tcp/631** so the firewall doesn't reject the new listener

After install, CUPS is reachable from any LAN client at:

- **Web admin**: `http://<pi-ip>:631/` — log in as a `lpadmin` user (default `pi`)
- **IPP print target**: `ipp://<pi-ip>:631/printers/<printer-name>`

To **add a printer** through the CUPS web admin: Add Printer → log in as a `lpadmin` user → pick the discovered local device (USB / network) → pick the driver → name it (e.g. `bay-1-receipt`) → save. The printer immediately shows up in `GET /api/printers` from this service and gets announced to the GrainManagement web on the next refresh.

⚠ **CUPS public access is LAN-only by default.** `@LOCAL` matches RFC1918 / link-local addresses — the CUPS server is reachable from `192.168.x.x`, `10.x.x.x`, `172.16-31.x.x`, but **not** from the public internet. Don't broaden that without a VPN. CUPS authentication on the bare LAN is `BasicAuthentication` — fine for trusted networks, not for a public IP.

## Windows printing

Windows has no CUPS — the service uses the Windows Print Spooler via `WindowsPrintClient`. The installer ensures the `Spooler` service is running, enables it for auto-start, and installs **SumatraPDF** via winget so PDFs can be rendered silently (without the dreaded "What do you want to open this with?" dialog).

To **share a printer to the LAN** so other Windows / Linux clients can print to it, use Windows' built-in printer sharing:

```powershell
# Run as Administrator
Add-Printer -Name "bay-1-receipt" -DriverName "Generic / Text Only" -PortName "USB001"
Set-Printer -Name "bay-1-receipt" -Shared $true -ShareName "bay1-receipt"
```

The shared printer then shows up as `\\<host>\bay1-receipt` for SMB-based access. For IPP equivalency to CUPS, install the optional Windows feature *Print and Document Services → Internet Printing Client* and use `http://<host>/printers/<sharename>/.printer`.

## SignalR hub contract

Web → service:

| Method            | Args                                       | Purpose                                |
|-------------------|--------------------------------------------|----------------------------------------|
| `PrintTicket`     | `{ PrinterId, Url, JobTitle? }`            | Download PDF from `Url` and print to `PrinterId` |
| `TestPrint`       | `printerId`                                | One-off test page echoed to the admin UI |
| `GetPrinterList`  | —                                          | Reply on `PrinterListResponse`         |
| `ReloadConfig`    | —                                          | Reload settings + reconnect            |

Service → web:

| Method                 | Args                                  | Purpose                              |
|------------------------|---------------------------------------|--------------------------------------|
| `JoinPrintGroup`       | `serviceId`                           | Joins the SignalR group keyed by site|
| `PrintServiceReady`    | `{ ServiceId, Printers }`             | Tells web "these printers live here" |
| `PrinterListResponse`  | `{ serviceId, printers }`             | Reply for `GetPrinterList`           |
| `PrintResult`          | `{ success, message, jobId }`         | Reply for each `PrintTicket`         |
| `TestPrintResult`      | `{ success, printerId, message }`     | Reply for each `TestPrint`           |

## Manual install (when you don't want to run the scripts)

### Linux / Pi

```bash
sudo apt update
sudo apt install -y cups cups-bsd cups-client aspnetcore-runtime-8.0
sudo usermod -aG lpadmin pi
sudo cupsctl --remote-any --remote-admin --share-printers   # the CUPS-public step
sudo systemctl restart cups
dotnet publish -c Release -r linux-arm64 --self-contained false -o /opt/print-service
# … then write /etc/systemd/system/print-service.service and `systemctl enable --now print-service`.
```

### Windows

```powershell
winget install --id Microsoft.DotNet.AspNetCore.8 --silent
winget install --id SumatraPDF.SumatraPDF      --silent
dotnet publish -c Release -r win-x64 --self-contained false -o C:\Services\PrintService
sc.exe create "PrintService" binPath="C:\Services\PrintService\PiPrintService.exe" start=auto
sc.exe start PrintService
```

## Requirements

- .NET 8 ASP.NET Core runtime (installed by the scripts)
- CUPS on Linux (apt-installed by the Pi script) **or** Windows Print Spooler (already present) **or** macOS CUPS
- Network access to the GrainManagement web app
- Inbound **tcp/5230** (this service's API + Swagger) and on Linux **tcp/631** (CUPS) so the LAN can submit print jobs
- On Windows: SumatraPDF for silent PDF rendering (winget-installed by the script)
