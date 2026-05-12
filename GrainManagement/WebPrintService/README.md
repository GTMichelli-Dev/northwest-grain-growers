# NWGG Web Print Service

A cross-platform .NET 8 print service that connects to a Northwest Grain Growers (NWGG) web app via SignalR and prints tickets, lot labels, and weight sheets to a locally-attached printer.

Distributed standalone from [`GTMichelli-Dev/nwgg-web-print-service`](https://github.com/GTMichelli-Dev/nwgg-web-print-service). Source of truth lives in the NWGG monorepo at `GrainManagement/WebPrintService/`; the standalone repo is kept in sync via `scripts/sync-nwgg-web-print-service.sh`.

> **BasicWeigh fleet**: do not install this service onto a BasicWeigh kiosk. BasicWeigh uses the legacy [`GTMichelli-Dev/web-print-service`](https://github.com/GTMichelli-Dev/web-print-service) repo, which speaks the older `/scaleHub` protocol. This repo targets the GrainManagement web app's `/hubs/print` hub.

## Architecture

```
GrainManagement web app (waldv002:5000, etc.)
    |
    +-- SignalR Hub  (/hubs/print)
            |
            +-- NWGG Web Print Service (this)  — listens on :5230 for Swagger/REST
                    |
                    +-- Windows:  SumatraPDF / PDFtoPrinter      (Windows Print)
                        Linux:    CUPS  (lp, lpstat, lpoptions)
                            |
                            +-- BIXOLON BK3-3  (USB)
```

The service connects outbound to one or more web-server URLs, joins a `print` SignalR group, announces its printer queue, then waits for `PrintTicket` / `TestPrint` / `GetPrinterList` / `ReloadConfig` events from the server. PDFs are downloaded from the web app via HTTP and handed to the OS print stack.

## Hub path

`SignalRHub` defaults to `/hubs/print` (GrainManagement convention). It can be overridden in `appsettings.json` for special cases, but the install scripts always write the default and there's no per-fleet override here — this repo is GrainManagement-only.

## Quick start — Raspberry Pi

**Prerequisite (one-time per Pi): install the BIXOLON BK3 CUPS driver pack.**

The driver pack is provided by BIXOLON as `Software_BxlPOSCupsDrv_Linux_v1.5.9.tgz` (or newer). It contains the `rastertoBixolon` CUPS filter and PPD files for every BIXOLON POS model. Without it, the install script falls back to a raw queue, and PDFs will not render.

```bash
# from your dev box:
scp Software_BxlPOSCupsDrv_Linux_v1.5.9.tgz admin@<pi-ip>:/tmp/

# on the Pi as admin:
cd /tmp
tar xzf Software_BxlPOSCupsDrv_Linux_v1.5.9.tgz
cd Software_BxlPOSCupsDrv_Linux_v1.5.9
sudo ./setup_v1.5.9.sh

# verify the BK3-3 PPD is registered with CUPS:
ls /usr/share/cups/model/Bixolon/BK33*
lpinfo -m 2>/dev/null | grep BK33
```

**Install the service:**

```bash
git clone https://github.com/GTMichelli-Dev/nwgg-web-print-service.git /tmp/wps
sudo bash /tmp/wps/deploy/install.sh http://waldv002:5000 \
    --server-id 1 \
    --printer-name Kiosk \
    --bk3-ppd /usr/share/cups/model/Bixolon/BK33_v1.0.3.ppd
rm -rf /tmp/wps
```

**Flags:**

| Flag | Default | Description |
|---|---|---|
| `<web-server-url>` | *required (positional)* | Base URL of the GrainManagement web app — written to `Print.ServerUrls[0]` |
| `--server-id <n>` | `1` | NWGG `ServerId` for this kiosk |
| `--printer-name <name>` | `Kiosk` | CUPS queue name for the BK3 printer |
| `--service-id <id>` | `default` | SignalR group ID |
| `--port <port>` | `5230` | Local API/Swagger port |
| `--bk3-ppd <path>` | *(none — raw queue)* | Path to the BIXOLON BK3 PPD on the Pi. Strongly recommended. Use `BK33_v1.0.3.ppd` for BK3-3, `BK32_v1.0.2.ppd` for BK3-2. |
| `--branch <branch>` | `master` | Git branch of `nwgg-web-print-service` to install |
| `--install-dir <path>` | `/opt/web-print-service` | Install location |

**Re-run the same command to upgrade** — the script stops the service, rebuilds, regenerates `appsettings.json`, and restarts.

**After install:**

- Service: `sudo systemctl status web-print-service`
- Logs: `sudo journalctl -u web-print-service -f`
- Swagger / REST: `http://<pi-ip>:5230/swagger`
- CUPS admin: `http://<pi-ip>:631`
- Healthy banner looks like:
  ```
  Web Print Service v1.2.0
  Connecting to http://waldv002:5000/hubs/print...
  Connected to http://waldv002:5000/hubs/print. Joining print group (ServiceId=default)...
  Announced 1 printer(s).
  ```

## Quick start — Windows

Run PowerShell as Administrator:

```powershell
.\deploy\deploy-to-windows.ps1 -WebServerUrl "http://waldv002:5000" -ServerId 1
```

| Parameter | Default | Description |
|---|---|---|
| `-WebServerUrl` | *(required)* | URL of the GrainManagement web app |
| `-ServerId` | `1` | NWGG ServerId |
| `-PrinterName` | `Kiosk` | Logical printer name to register the BK3 under |
| `-ServiceId` | `default` | SignalR group ID |
| `-Port` | `5230` | API/Swagger port |
| `-InstallDir` | `C:\Services\WebPrintService` | Install location |
| `-ServiceName` | `WebPrintService` | Windows service name |
| `-BixolonInfDir` | `deploy\drivers\bixolon-bk3` | Folder containing the BIXOLON `.inf` (drops the script into pnputil + Add-Printer mode) |

The script installs .NET 8 runtime if missing, publishes self-contained, copies to `C:\Services\WebPrintService\`, optionally installs the BIXOLON Windows driver from the supplied INF folder, and registers a Windows Service with auto-restart on failure. SumatraPDF is also auto-installed (via `winget`) for silent PDF printing.

## Configuration (`appsettings.json`)

```json
{
  "Print": {
    "ServerUrls":  ["http://waldv002:5000"],
    "SignalRHub":  "/hubs/print",
    "ServiceId":   "default",
    "ServerId":    1,
    "Port":        "5230"
  }
}
```

`ServerUrls` is an array — list multiple web servers if the Pi should accept print jobs from more than one (each gets its own SignalR connection).

## SignalR protocol

| Direction | Method | Description |
|-----------|--------|-------------|
| Service → Hub | `JoinPrintGroup(serviceId)` | Join the PrintClients group |
| Service → Hub | `PrintServiceReady(announcement)` | Announce printers on connect |
| Service → Hub | `PrinterListResponse(data)` | Reply to a `GetPrinterList` request |
| Service → Hub | `PrintResult(result)` | Report job result |
| Service → Hub | `TestPrintResult(result)` | Report test-print result |
| Hub → Service | `PrintTicket(data)` | Print a ticket PDF — `type` ∈ {`LotLabel`, `IntakeWeightSheet`, default (load ticket)} |
| Hub → Service | `GetPrinterList` | Request the printer list |
| Hub → Service | `TestPrint(printerId)` | Send a test page to a specific printer |
| Hub → Service | `ReloadConfig` | Recycle SignalR connections (re-read settings) |

PDF endpoints the service downloads from:

```
GET  {ServerUrl}/api/printjobs/load-ticket/{id}/pdf
GET  {ServerUrl}/api/printjobs/lot-label/{id}/pdf
GET  {ServerUrl}/api/printjobs/intake-weight-sheet/{id}/pdf
GET  {ServerUrl}/api/printjobs/test-page/pdf
```

## REST endpoints (Swagger at `http://<host>:5230/swagger`)

- `GET  /api/status/health` — overall health, printer count, print system type
- `GET  /api/printers` — enumerate all printers known to the OS print stack
- `GET  /api/printers/{id}/status` — single-printer status
- `POST /api/printers/{id}/test` — send a test page directly via REST (no SignalR)

## Troubleshooting

**Service shows `404 Not Found` on connect.** The web app at `ServerUrls[0]` isn't mapping `/hubs/print`. Confirm you're pointed at the GrainManagement web app (not BasicWeigh — see note at the top). Check the hub registration in the web app's `Program.cs` (`app.MapHub<PrintHub>("/hubs/print")`).

**Printer queue exists but jobs produce blank pages or garbage.** The CUPS queue was created as `raw` (no PPD). Run `lpstat -t` and check `lpoptions -p Kiosk` — if no driver is listed, re-install pointing `--bk3-ppd` at the BIXOLON PPD path.

**`Cannot find BIXOLON BK3 USB`.** The auto-detect looks at `lpinfo -v` for a `usb://` line matching `BIXOLON`. If the printer isn't powered on / connected when the install runs, the queue is created against a guessed URI. Re-plug the printer and re-run the install.

## Development

```bash
# Linux/macOS
sudo apt-get install cups   # if not already installed
dotnet run

# Windows
dotnet run
```

The dev banner prints `Listening on: http://localhost:5230`. Swagger comes up at `/swagger`.
