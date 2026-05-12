# NWGG Temp Ticket Kiosk Service

A .NET 8 background worker + Web API that monitors a GPIO button on a Raspberry Pi kiosk and POSTs a temp-ticket press to the GrainManagement web app on every debounced press. One instance per kiosk — one Pi, one button, one scale, one printer.

Cross-platform: boots cleanly on Windows for testing (no real GPIO; use Swagger `POST /api/test/press` to simulate). On a Pi the GPIO pin is monitored via `System.Device.Gpio` (libgpiod under the hood).

Distributed standalone from [`GTMichelli-Dev/nwgg-temp-ticket-kiosk-service`](https://github.com/GTMichelli-Dev/nwgg-temp-ticket-kiosk-service). Source of truth lives in the NWGG monorepo at `GrainManagement/TempTicketKioskService/`; the standalone repo is kept in sync via `scripts/sync-nwgg-temp-ticket-kiosk-service.sh`.

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

Everything else — the motion wait, the database row, the print, the camera, the kiosk display banner — lives on the web side. The web-side display state reaches the kiosk browser via SignalR, not through this service.

## Quick start — Raspberry Pi

```bash
git clone https://github.com/GTMichelli-Dev/nwgg-temp-ticket-kiosk-service.git /tmp/ttk
sudo bash /tmp/ttk/deploy/install.sh http://waldv002:5000 \
    --kiosk-id kiosk-1 \
    --scale-id 3 \
    --gpio-pin 17
rm -rf /tmp/ttk
```

**Flags:**

| Flag | Default | Description |
|---|---|---|
| `<web-server-url>` | *required (positional)* | GrainManagement web app base URL — written to `Kiosk.ServerUrl` |
| `--kiosk-id <id>` | *required on first install* | Friendly id stamped on every temp ticket (e.g. `kiosk-1`, `bay-2-loadout`) |
| `--scale-id <n>` | `0` | NWGG scale id this kiosk reads from (must match a row in the web's scale registry) |
| `--gpio-pin <n>` | `17` | BCM pin number. Default 17 = physical pin 11 on a 40-pin Pi header |
| `--debounce-ms <n>` | `75` | Software debounce window in ms |
| `--pull-mode <mode>` | `pullup` | `pullup` (button shorts pin to GND when pressed) or `pulldown` |
| `--printer-target <id>` | *(empty)* | Optional logical printer name to print the temp ticket to. `serviceId:printerId` or just `printerId` |
| `--user <name>` | `admin` | Linux service user (NWGG kiosks run as `admin`) |
| `--port <port>` | `5240` | Service port for local Swagger / REST |
| `--install-dir <path>` | `/opt/temp-ticket-kiosk` | Install location |
| `--arch <arm\|arm64\|x64>` | *(auto-detected)* | Build target |
| `--reset-config` | *(off)* | On upgrade, overwrite the existing `appsettings.json` instead of preserving it |

**Re-run the same command to upgrade** — the script stops the service, rebuilds, copies binaries, and restarts. Your existing `appsettings.json` is preserved across upgrades unless you pass `--reset-config`.

**After install:**

- Service: `sudo systemctl status temp-ticket-kiosk`
- Logs: `sudo journalctl -u temp-ticket-kiosk -f`
- Swagger / REST: `http://<pi-ip>:5240/swagger`
- Test press: `curl -X POST http://<pi-ip>:5240/api/test/press`

## Configuration (`appsettings.json`)

Written by the install script from the flags above, or edited in place:

```json
{
  "Kiosk": {
    "KioskId":       "kiosk-1",
    "ScaleId":       3,
    "PrinterTarget": "",
    "ServerUrl":     "http://waldv002:5000",
    "GpioPin":       17,
    "DebounceMs":    75,
    "PullMode":      "pullup"
  },
  "Kestrel": {
    "Endpoints": { "Http": { "Url": "http://0.0.0.0:5240" } }
  }
}
```

After hand-editing, restart the service: `sudo systemctl restart temp-ticket-kiosk`.

| Field | Purpose |
|---|---|
| `KioskId` | Friendly id stamped on every temp ticket |
| `ScaleId` | Scale to read the weight from |
| `PrinterTarget` | Where the temp ticket prints. `serviceId:printerId` or just `printerId` |
| `ServerUrl` | GrainManagement web base URL |
| `GpioPin` | BCM pin number |
| `DebounceMs` | Software debounce window. Edges within this many ms of the previous accepted edge are ignored |
| `PullMode` | `pullup` (button shorts pin to GND when pressed — most common) or `pulldown` |

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

Every edge transition is logged at **Information** level — no debug flag needed. Tail the journal to confirm your wiring before bolting the button onto the kiosk:

```bash
sudo journalctl -u temp-ticket-kiosk -f
```

You'll see lines like:

```
GPIO monitor watching pin 17 (InputPullUp, trigger Falling, debounce 75 ms). Idle value = High.
GPIO pin 17: Falling PRESS (Δ=1842 ms — firing press to http://waldv002:5000)
GPIO pin 17: Rising (other edge — ignored)        ← button released
GPIO pin 17: Falling BOUNCE (Δ=12 ms < debounce 75 ms — ignored)  ← contact bounce
```

For a one-shot read of the current pin state, hit `/api/status` — the response includes `gpio.currentValue` (`High` / `Low` / `n/a`):

```bash
curl http://<pi>:5240/api/status | python3 -m json.tool
```

## Local HTTP API

The service hosts a small Swagger surface at the configured port (default **5240**).

| Method + path | Purpose |
|---|---|
| `GET  /api/status` | Service health, version, GPIO state, current Kiosk options |
| `POST /api/test/press` | Simulate a button press. Useful on Windows / macOS where there's no GPIO, and on the Pi for end-to-end smoke tests without crouching at the kiosk |
| `GET  /api/config` | Read the current `KioskOptions` (no PUT yet — edit appsettings.json + restart) |
| `GET  /swagger` | Swagger UI |

## Operational notes

- **No SignalR client (yet).** The kiosk service speaks HTTP only. The web-side display state flows directly to the kiosk's browser tab over a separate SignalR connection — this service doesn't sit in that path.
- **One short retry on POST failure**, then drop. Queuing presses through a long web outage would replay ghost tickets the operator never intended once the web came back.
- **Service survives unhandled exceptions** — `BackgroundServiceExceptionBehavior=Ignore` plus a `TaskScheduler.UnobservedTaskException` handler in `Program.cs` keep the host alive when a GPIO transport error escapes.
- **No public listener beyond `:5240`.** The service makes outbound HTTP only; the inbound port hosts the local Swagger / test endpoints.

## Windows install (for development / testing)

GPIO is disabled on Windows. The service still runs; use `POST /api/test/press` to simulate.

```powershell
winget install --id Microsoft.DotNet.AspNetCore.8 --silent
dotnet publish -c Release -r win-x64 --self-contained false -o C:\Services\TempTicketKioskService
sc.exe create "TempTicketKioskService" binPath="C:\Services\TempTicketKioskService\TempTicketKioskService.exe" start=auto
sc.exe start TempTicketKioskService
```

A dedicated Windows install script may be added later if production Windows deployments come up.

## Requirements

- .NET 8 ASP.NET Core runtime (installed by `deploy/install.sh`)
- A button wired to a GPIO pin (Pi only — Windows hosts use the simulate endpoint)
- Outbound HTTP to the GrainManagement web app
- On Linux, the service user must be in the `gpio` group so it can open `/dev/gpiochip0` (the install script handles this)
