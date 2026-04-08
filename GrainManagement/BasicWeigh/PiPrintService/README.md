# Web Print Service

A cross-platform .NET 8.0 print service that connects to a web application via SignalR and provides remote printing. Automatically detects the operating system and uses **Windows Print** (PowerShell/WMI) on Windows or **CUPS** (`lpstat`, `lp`) on Linux/macOS.

## Architecture

### Windows
```
Web App (BasicWeigh, etc.)
    |
    +-- SignalR Hub (/scaleHub)
            |
            +-- Web Print Service (this)
                    |
                    +-- SumatraPDF (silent PDF printing)
                            |
                            +-- Physical Printer (USB, Network, Shared)
```

#### Windows PDF Printing Requirements

The service needs a silent PDF printing tool to send tickets to printers. It auto-detects in this order:

1. **SumatraPDF** (recommended) — auto-installed via `winget` if not found
2. **PDFtoPrinter.exe** — if placed in the service directory
3. **Fallback** — uses `Start-Process -Verb PrintTo` (may open a dialog, not recommended for unattended use)

To manually install SumatraPDF:
```
winget install SumatraPDF.SumatraPDF
```

Or download from https://www.sumatrapdfreader.org/free-pdf-reader

### Linux / macOS / Raspberry Pi
```
Web App (BasicWeigh, etc.)
    |
    +-- SignalR Hub (/scaleHub)
            |
            +-- Web Print Service (this)
                    |
                    +-- CUPS (lpstat, lp, lpoptions)
                            |
                            +-- Physical Printer (USB, Network, IPP)
```

## Features

- **Cross-Platform** — automatically uses Windows Print or CUPS based on the OS
- **Windows Integration** — uses PowerShell `Get-Printer`, `Start-Process`, `Out-Printer` to manage and print to local and network printers
- **CUPS Integration** — uses `lpstat`, `lp`, `lpoptions` to manage and print to local printers on Linux/macOS
- **SignalR Connection** — connects to any web app's SignalR hub (outbound only, works behind firewalls)
- **Printer Discovery** — announces available CUPS or Windows printers to the web app on connect/reconnect
- **PDF Printing** — downloads ticket PDFs from the web app and prints locally
- **Test Print** — send a test page to any connected printer from the web UI
- **Swagger API** — local REST API for configuration, testing, and diagnostics
- **SQLite Settings** — persistent configuration stored locally (no appsettings.json edits needed)
- **Forever Retry** — exponential backoff, never gives up reconnecting

## Quick Start

```bash
# Windows
cd WebPrintService
dotnet run

# Linux / Raspberry Pi
sudo apt-get install cups
dotnet run
```

## Endpoints (Swagger at http://<your-ip>:<your-port>/swagger)

### Health & Status

#### `GET /api/status/health`
Returns service health, print system type, and printer count.

**Response:**
```json
{
  "status": "ok",
  "printSystem": "Windows",
  "printSystemAvailable": true,
  "printerCount": 3,
  "printers": [
    { "printerId": "HP LaserJet", "displayName": "HP LaserJet", "status": "idle", "enabled": true, "isDefault": true }
  ]
}
```

#### `GET /api/readme`
Returns API documentation as JSON including all endpoints, parameters, response formats, and SignalR methods.

---

### Printers

#### `GET /api/printers`
List all printers with their status.

**Response:**
```json
[
  {
    "printerId": "HP_LaserJet_Pro",
    "displayName": "HP LaserJet Pro",
    "status": "idle",
    "isDefault": true,
    "enabled": true
  }
]
```

#### `GET /api/printers/{printerId}/status`
Get detailed status of a specific printer.

**Example:** `GET /api/printers/HP_LaserJet_Pro/status`

**Response:**
```json
{
  "printerId": "HP_LaserJet_Pro",
  "status": "printer HP_LaserJet_Pro is idle. enabled since Mon 24 Mar 2026 08:00:00 AM CDT"
}
```

#### `POST /api/printers/{printerId}/test`
Send a test page to a printer. No request body needed.

**Example:** `POST /api/printers/HP_LaserJet_Pro/test`

**Response:**
```json
{
  "success": true,
  "message": "Sent to HP_LaserJet_Pro"
}
```

---

### Settings

#### `GET /api/settings`
Get current service settings.

**Response:**
```json
{
  "id": 1,
  "serviceId": "default",
  "serverUrl": "http://localhost:5110",
  "signalRHub": "/scaleHub"
}
```

#### `PUT /api/settings`
Update service settings. Triggers a reconnect to the web app.

**Request:**
```json
{
  "serviceId": "office-printer",
  "serverUrl": "http://192.168.1.100:5110"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Settings saved. Service restarting..."
}
```

---

## SignalR Methods

| Direction | Method | Description |
|-----------|--------|-------------|
| Service -> Hub | `JoinPrintGroup(serviceId)` | Join the PrintClients group |
| Service -> Hub | `PrintServiceReady(announcement)` | Announce printers on connect |
| Service -> Hub | `PrinterListResponse(data)` | Respond to printer list request |
| Service -> Hub | `PrintResult(result)` | Report print job result |
| Service -> Hub | `TestPrintResult(result)` | Report test print result |
| Hub -> Service | `PrintTicket(data)` | Print a ticket PDF |
| Hub -> Service | `GetPrinterList` | Request printer list |
| Hub -> Service | `TestPrint(printerId)` | Send test page to printer |
| Hub -> Service | `ReloadConfig` | Restart the service |

## Platform Details

### Windows
Uses PowerShell commands:
- `Get-Printer` — enumerate all local and network printers
- `Start-Process -Verb PrintTo` — print PDFs (or SumatraPDF if installed)
- `Out-Printer` — print text files
- `rundll32 shimgvw.dll` — print images
- Optional: [SumatraPDF](https://www.sumatrapdfreader.org/) for better PDF printing

### Linux / macOS (CUPS)
Uses CUPS command-line tools:
- `lpstat -p -d` — enumerate printers and default
- `lp -d <printer>` — print files
- `lpoptions -p <printer>` — get printer options/description
- CUPS web interface at https://localhost:631

## Deployment

### Install on Raspberry Pi / Linux

SSH into the Pi and run:

```bash
git clone https://github.com/GTMichelli-Dev/web-print-service.git /tmp/wps
bash /tmp/wps/deploy/install.sh https://basicscale.scaledata.net
rm -rf /tmp/wps
```

With options:

```bash
git clone https://github.com/GTMichelli-Dev/web-print-service.git /tmp/wps
bash /tmp/wps/deploy/install.sh https://basicscale.scaledata.net \
    --service-id office --port 5230
rm -rf /tmp/wps
```

For private repos, git will prompt for credentials. You can also use a deploy key or GitHub token.

Options:
| Option | Default | Description |
|--------|---------|-------------|
| `--service-id <id>` | `default` | Unique ID for this service instance |
| `--port <port>` | `5230` | API/Swagger port |
| `--branch <branch>` | `master` | Git branch to install |
| `--install-dir <path>` | `/opt/web-print-service` | Install location |

**What the script does automatically:**
1. Detects Pi architecture (arm64, armv7l, x64)
2. Installs CUPS (printer system)
3. Installs .NET 8 SDK and runtime permanently (skips download on future updates)
4. Downloads latest source from GitHub
5. Builds for the Pi's architecture
6. Installs to `/opt/web-print-service`
7. Preserves existing database on updates
8. Registers and starts as a systemd service

**Prerequisites:** Just internet access and `git` (pre-installed on Raspberry Pi OS). No .NET, no CUPS — the script installs everything. The .NET SDK is installed permanently so future updates skip the download.

**To update:** Run the same command again. The script stops the service, updates files, preserves your database, and restarts.

**After install:**
- Swagger: `http://<pi-ip>:5230/swagger`
- CUPS Admin: `http://<pi-ip>:631`
- Logs: `sudo journalctl -u web-print-service -f`
- Restart: `sudo systemctl restart web-print-service`

### Install on Windows

**Option A — Automated (as a Windows Service):**

Run PowerShell as Administrator:

```powershell
.\deploy\deploy-to-windows.ps1 -WebServerUrl "https://basicscale.scaledata.net"

# With options
.\deploy\deploy-to-windows.ps1 -WebServerUrl "https://basicscale.scaledata.net" `
    -ServiceId "office" -Port 5230
```

| Parameter | Default | Description |
|-----------|---------|-------------|
| `-WebServerUrl` | *(required)* | BasicWeigh web server URL |
| `-ServiceId` | `default` | Unique ID for this instance |
| `-Port` | `5230` | API/Swagger port |
| `-InstallDir` | `C:\Services\WebPrintService` | Install location |
| `-ServiceName` | `WebPrintService` | Windows service name |

**What the script does:** Installs .NET if needed, builds, installs to `C:\Services\WebPrintService`, preserves existing database, registers as a Windows Service with auto-restart on failure.

**After install:**
- Swagger: `http://localhost:5230/swagger`
- Manage: `services.msc` > "Web Print Service"
- Restart: `Restart-Service WebPrintService`

**Option B — Manual (for development):**

```powershell
cd WebPrintService
dotnet run
```

Windows printers are auto-detected — no additional setup needed.

## Configuration

Initial config via `appsettings.json`:
```json
{
  "Print": {
    "ServerUrl": "https://your-server",
    "Port": "5230"
  }
}
```

After first run, use the API to update settings (persists to SQLite):
```bash
curl -X PUT http://localhost:5230/api/settings \
  -H "Content-Type: application/json" \
  -d '{"serverUrl": "https://your-server", "serviceId": "office-printer"}'
```
