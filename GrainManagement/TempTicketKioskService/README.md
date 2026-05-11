# Temp Ticket Kiosk Service

A .NET 8 background worker + Web API that monitors a GPIO button on a Raspberry Pi kiosk and POSTs a temp-ticket press to the GrainManagement web app on every debounced press. Designed to run **one instance per kiosk** — one Pi, one button, one scale, one printer.

The service is cross-platform — it boots cleanly on Windows for testing (no real GPIO; use Swagger `POST /api/test/press` to simulate). On a Pi the GPIO pin is monitored via `System.Device.Gpio` (libgpiod under the hood).

## How it works

```
[Pi: button → GPIO 17]
        │  (debounced 75 ms, falling edge)
        ▼
TempTicketKioskService
        │  POST http://web/api/temp-tickets/press
        │       { KioskId, ScaleId, PrinterTarget }
        ▼
GrainManagement Web
        ├── waits for scale Motion = false (up to 8 s)
        ├── inserts Inventory.TempWeightTickets row
        ├── dispatches print → kiosk's configured printer
        └── fires camera capture → TempTicket-role cameras
```

The kiosk-side service is intentionally thin:

- Single button-press monitor (GPIO interrupt, software debounce)
- One outbound HTTP POST per press (~5 s timeout, one retry, then drop)
- Local Swagger for status / simulated-press / config-read

Everything else — the motion wait, the database row, the print, the camera, the kiosk display banner — lives on the web side. The web-side display state (Phase 4) reaches the kiosk browser via SignalR, not through this service.

## Quick picker

| Platform                                     | Run                                                | What it does                                                  |
|----------------------------------------------|----------------------------------------------------|---------------------------------------------------------------|
| **Raspberry Pi** (or any Debian-derived host)| `sudo ./install-pi.sh`                             | Installs .NET 8 runtime, publishes for `linux-arm64`, registers a systemd service, adds the service user to the `gpio` group |
| **Windows desktop / kiosk**                  | `.\install-windows.ps1` (elevated PowerShell)      | Installs .NET 8 runtime via winget, registers a Windows service. GPIO is disabled on Windows — Swagger's `POST /api/test/press` simulates a press |

Re-run either script after a `git pull` to upgrade — your `appsettings.json` survives in place.

## Sparse checkout (single-service host)

A kiosk-only Pi doesn't need the rest of the monorepo. Use a git sparse + partial clone so only this folder lands on disk and `git pull` only touches it:

```bash
git clone --filter=blob:none --no-checkout https://github.com/GTMichelli-Dev/northwest-grain-growers.git
cd northwest-grain-growers
git sparse-checkout init --cone
git sparse-checkout set GrainManagement/TempTicketKioskService
git checkout master
cd GrainManagement/TempTicketKioskService
```

`--filter=blob:none` defers blob downloads, so blobs outside the sparse set are never fetched. `git sparse-checkout disable` reverts to a full checkout if you change your mind.

## Configuration

All settings live in `appsettings.json → Kiosk`. Edit the file on the installed host and restart the service (`sudo systemctl restart temp-ticket-kiosk` on Pi, `Restart-Service TempTicketKioskService` on Windows).

| Field             | Default                | Purpose                                                                          |
|-------------------|------------------------|----------------------------------------------------------------------------------|
| `KioskId`         | `kiosk-1`              | Friendly id stamped on every temp ticket (e.g. `bay-1-kiosk`).                   |
| `ScaleId`         | `0`                    | Scale to read the weight from. Must match a row in the web's scale registry.     |
| `PrinterTarget`   | `""`                   | Where the temp ticket prints. `serviceId:printerId` or just `printerId`.         |
| `ServerUrl`       | `http://localhost:5000`| GrainManagement web base URL.                                                    |
| `GpioPin`         | `17`                   | BCM pin number. Default 17 = physical pin 11 on a 40-pin Pi header.              |
| `DebounceMs`      | `75`                   | Software debounce window. Edges within this many ms of the previous accepted edge are ignored. |
| `PullMode`        | `pullup`               | `pullup` (button shorts pin to GND when pressed — most common) or `pulldown`.    |

### Wire-up — typical "pullup" button

```
       3.3 V ───────[ internal pull-up ]──┐
                                          │
                            GPIO pin ─────┤
                                          │
                          BUTTON ─────────┤
                                          │
       GND  ──────────────────────────────┘
```

When pressed, the pin is shorted to GND, so the pin transitions from HIGH → LOW (falling edge). Use `PullMode: pullup` and the service triggers on the falling edge.

If your hardware does the opposite (button shorts the pin to 3.3 V, idle LOW), set `PullMode: pulldown` and the service triggers on the rising edge instead.

## Watching the GPIO from the console

Every edge transition is logged at **Information** level — no debug log
flag needed. Tail the journal to confirm your wiring before bolting the
button onto the kiosk:

```bash
sudo journalctl -u temp-ticket-kiosk -f
```

You'll see lines like:

```
GPIO monitor watching pin 17 (InputPullUp, trigger Falling, debounce 75 ms). Idle value = High.
GPIO pin 17: Falling PRESS (Δ=1842 ms — firing press to http://10.0.0.5:5000)
GPIO pin 17: Rising (other edge — ignored)        ← button released
GPIO pin 17: Falling BOUNCE (Δ=12 ms < debounce 75 ms — ignored)  ← contact bounce
```

For a one-shot read of the current pin state, hit `/api/status` — the
response includes `gpio.currentValue` (`High` / `Low` / `n/a`):

```bash
curl http://<pi>:5240/api/status | python3 -m json.tool
```

## Local HTTP API

The service hosts a small Swagger surface at the configured port (default **5240**).

| Method + path             | Purpose                                                                  |
|---------------------------|--------------------------------------------------------------------------|
| `GET  /api/status`        | Service health, version, GPIO state, current Kiosk options.              |
| `POST /api/test/press`    | Simulate a button press. Useful on Windows / macOS where there's no GPIO, and on the Pi for end-to-end smoke tests without crouching at the kiosk. |
| `GET  /api/config`        | Read the current `KioskOptions` (no PUT yet — edit appsettings.json + restart). |
| `GET  /swagger`           | Swagger UI.                                                              |

## Operational notes

- **No SignalR client (yet).** The kiosk service speaks HTTP only. The web-side display state (Phase 4) flows directly to the kiosk's browser tab over a separate SignalR connection — this service doesn't sit in that path.
- **One short retry on POST failure**, then drop. Queuing presses through a long web outage would replay ghost tickets the operator never intended once the web came back.
- **Service survives unhandled exceptions** — `BackgroundServiceExceptionBehavior=Ignore` plus a `TaskScheduler.UnobservedTaskException` handler in `Program.cs` keep the host alive when a GPIO transport error escapes.
- **No public listener beyond `:5240`.** The service makes outbound HTTP only; the inbound port hosts the local Swagger / test endpoints.

## Manual install (when you don't want to run the scripts)

### Linux / Pi

```bash
sudo apt update
sudo apt install -y aspnetcore-runtime-8.0
sudo usermod -aG gpio $USER  # so the service can open /dev/gpiochip0

dotnet publish -c Release -r linux-arm64 --self-contained false -o /opt/temp-ticket-kiosk
# … then write /etc/systemd/system/temp-ticket-kiosk.service referencing
# /opt/temp-ticket-kiosk/TempTicketKioskService.dll and `systemctl enable --now`.
```

### Windows

```powershell
winget install --id Microsoft.DotNet.AspNetCore.8 --silent
dotnet publish -c Release -r win-x64 --self-contained false -o C:\Services\TempTicketKioskService
sc.exe create "TempTicketKioskService" binPath="C:\Services\TempTicketKioskService\TempTicketKioskService.exe" start=auto
sc.exe start TempTicketKioskService
```

## Requirements

- .NET 8 ASP.NET Core runtime (installed by the scripts)
- A button wired to a GPIO pin (Pi only — Windows hosts use the simulate endpoint)
- Outbound HTTP to the GrainManagement web app
- On Linux, the service user must be in the `gpio` group so it can open `/dev/gpiochip0`
