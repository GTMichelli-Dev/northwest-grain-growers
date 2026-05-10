# Camera Capture Service

A .NET 8 background worker + small Web API that connects to the GrainManagement web app over SignalR, captures snapshots from one or more IP / USB cameras, uploads them back to the web for ticket storage, and serves continuous MJPEG live feeds. Designed to run **one instance per physical location** — each instance handles every camera at that site.

## How it works

```
[Camera(s)] ──RTSP / HTTP / USB──▶  CameraService
                                        │
                                        ├── /api/stream/{cameraId}  (live MJPEG to browsers)
                                        ├── POST /api/ticket/{n}/image  (snapshot upload back to web)
                                        └── SignalR /hubs/camera (CRUD, capture commands, announcements)
                                                          │
                                                          ▼
                                                  GrainManagement Web
```

1. On startup the worker loads its settings + camera list from a local SQLite DB (`camera-service.db`)
2. Connects to the web's SignalR hub and joins a group keyed by `ServiceId`
3. Announces every active camera so the web's Camera admin page lists them
4. Listens for `CaptureImage`, `TestCapture`, `AddCamera`, `UpdateCamera`, `DeleteCamera`, `ReloadConfig`, `Reannounce`, `GetCameraList`, `GetCameraBrands`
5. Re-fetches brand definitions from a remote JSON URL on every restart and writes a local snapshot fallback so we still work when the internet is down

## Multiple cameras per service

The service holds a **list** of cameras — not just one. Each `CameraConfigEntity` row in SQLite has:

| Field           | Purpose                                                              |
|-----------------|----------------------------------------------------------------------|
| `CameraId`      | Stable id used by web / capture commands (e.g. `scale-cam-1`)         |
| `DisplayName`   | Friendly label for the admin UI                                       |
| `CameraBrand`   | Key into the brand definitions (e.g. `Hikvision`, `USB Camera (Windows)`, `Custom`) |
| `CameraIp`      | IP address for network cameras                                        |
| `CameraUser` / `CameraPassword` | Optional credentials                                    |
| `UsbDeviceName` | DirectShow or V4L2 device name for USB cameras                        |
| `CameraUrl`     | Custom snapshot URL when `CameraBrand = Custom`                       |
| `UsbCommand`    | Custom ffmpeg command override                                        |
| `TimeoutSeconds`| HTTP timeout for IP capture                                           |
| `Active`        | Skip inactive rows                                                    |
| `IsDefault`     | Used when a capture command arrives with no `CameraId`                |

Cameras are managed from the web app's **Camera admin** page over SignalR — every Add / Update / Delete writes to the local SQLite and re-announces the list. You can also edit `camera-service.db` directly with any SQLite tool (DB Browser for SQLite, sqlite3 CLI, etc.).

## Brand definitions (remote JSON)

Camera brand definitions are pulled at startup from a remote JSON file — the [GTMichelli-Dev/device-definitions](https://github.com/GTMichelli-Dev/device-definitions) repo (public). Each brand entry holds the snapshot URL template, USB command template, default port, etc. so the same code can drive any vendor with a single config edit on the server.

Two settings control this:

| Setting        | Purpose                                                      |
|----------------|--------------------------------------------------------------|
| `BrandsUrl`    | Raw URL to the JSON (e.g. `https://raw.githubusercontent.com/GTMichelli-Dev/device-definitions/main/camera-snapshot.json`) |
| `BrandsToken`  | Optional. Repo is public so no token is needed; set a `repo:read` PAT only if you fork it private or want to dodge anonymous rate limits |

On every startup (and on `ReloadConfig` over SignalR) the service:

1. `GET BrandsUrl` (with `Authorization: Bearer {BrandsToken}` if a token is set) and a 10s timeout
2. Deserializes to `List<CameraBrandDefinition>`
3. Writes a copy to `camera-snapshot.json` next to the binary as a fallback
4. Falls back to the local snapshot if the fetch fails — startup never blocks

Set the URL via the service's Swagger UI (`PUT /api/settings`) or directly with SQL:

```sql
UPDATE Settings SET
    BrandsUrl = 'https://raw.githubusercontent.com/GTMichelli-Dev/device-definitions/main/camera-snapshot.json'
WHERE Id = 1;
```

## Service settings (single-row `Settings` table)

| Field           | Default                                                | Purpose                                                          |
|-----------------|--------------------------------------------------------|------------------------------------------------------------------|
| `ServiceId`     | `default`                                              | Routes capture commands when multiple services connect (one per site) |
| `ServerUrl`     | `http://localhost:5110`                                | GrainManagement web base URL                                     |
| `SignalRHub`    | `/hubs/camera`                                         | Hub path on the web                                              |
| `StreamBaseUrl` | (empty)                                                | Externally reachable URL of THIS service (e.g. `http://192.168.1.50:5210`) — announced to the web so browsers can pull live feeds directly |
| `BrandsUrl`     | (empty)                                                | See above                                                        |
| `BrandsToken`   | (empty)                                                | See above                                                        |

## Endpoints

- `GET  /api/status` — operational status (web Camera page consumes this)
- `GET  /api/stream/{cameraId}` — continuous MJPEG live feed served by `MjpegStreamPool`
- `GET/PUT /api/settings` — read/write the SQLite `Settings` row
- Swagger UI: `/swagger`

## Adding a USB camera

Step-by-step for both Windows and Pi. IP cameras (Hikvision, Dahua, etc.) skip steps 1–2 entirely — they only need a brand, an IP, and a user/password.

### 1. Install ffmpeg

USB capture is driven by ffmpeg. Install it on the same machine the CameraService runs on — see the [Installing ffmpeg](#installing-ffmpeg) section below for Windows / Pi-host / Pi-Docker recipes. The Pi installer script `install-pi.sh` already handles this for you.

### 2. Find the device name

The CameraService's `UsbDeviceName` field has to match what ffmpeg sees on the host.

**Windows (DirectShow):**

```powershell
ffmpeg -list_devices true -f dshow -i dummy
```

Look for the `DirectShow video devices` block. Copy the exact name in quotes (case + spaces matter):

```
[dshow @ ...]  "USB Video Device"
[dshow @ ...]     Alternative name "@device_pnp_..."
```

→ Use **`USB Video Device`** as `UsbDeviceName`.

**Raspberry Pi (V4L2):**

```bash
v4l2-ctl --list-devices
```

You'll see entries like:

```
HD Pro Webcam C920 (usb-0000:01:00.0-1.2):
        /dev/video0
        /dev/video1
```

→ Use **`/dev/video0`** as `UsbDeviceName` (the first node is typically the one that produces frames; the second is often a metadata node).

### 3. Add the camera

Two paths — pick whichever fits your workflow:

**A. From the web's Camera admin page (recommended):**

The Camera admin page lists every announced CameraService and lets you Add / Edit / Delete cameras over SignalR. Fill in:

| Field            | Windows                                    | Raspberry Pi                |
|------------------|--------------------------------------------|-----------------------------|
| `CameraId`       | A stable id, e.g. `inbound-usb-1`          | Same                        |
| `DisplayName`    | "Inbound USB"                              | Same                        |
| `CameraBrand`    | `USB Camera (Windows)`                     | `USB Camera (Linux)`        |
| `UsbDeviceName`  | `USB Video Device` (from step 2)           | `/dev/video0` (from step 2) |
| `Active`         | ✔                                          | ✔                           |
| `IsDefault`      | ✔ if this is the only / primary camera     | Same                        |

Leave `CameraIp` / `CameraUser` / `CameraPassword` / `CameraUrl` blank — they're for IP cameras only.

**B. Direct SQL** (useful for first-time bootstrap before the web admin is reachable):

Edit `camera-service.db` (SQLite, next to the binary) with any tool. Windows:

```sql
INSERT INTO Cameras
    (CameraId, DisplayName, CameraBrand, UsbDeviceName, TimeoutSeconds, Active, IsDefault)
VALUES
    ('inbound-usb-1', 'Inbound USB', 'USB Camera (Windows)', 'USB Video Device', 10, 1, 1);
```

Raspberry Pi:

```sql
INSERT INTO Cameras
    (CameraId, DisplayName, CameraBrand, UsbDeviceName, TimeoutSeconds, Active, IsDefault)
VALUES
    ('inbound-usb-1', 'Inbound USB', 'USB Camera (Linux)', '/dev/video0', 10, 1, 1);
```

Restart the service so it picks up the new row and re-announces to the web.

### 4. Test capture

In the web's Camera admin page, click **Test Capture** on the new camera. The service captures a frame, base64s it back over SignalR, and the admin UI shows the image inline. Any error appears in the same panel.

CLI alternative — capture a frame manually using ffmpeg with the exact same arguments the service runs, to confirm the device name + driver:

```powershell
# Windows
ffmpeg -f dshow -i video="USB Video Device" -frames:v 1 -q:v 2 -y test.jpg
```

```bash
# Pi
ffmpeg -f v4l2 -i /dev/video0 -frames:v 1 -q:v 2 -y test.jpg
```

A successful run drops a 50–200 KB `test.jpg`. If ffmpeg errors with "Could not open device" the device name in step 2 was wrong; if it errors with "Permission denied" on the Pi, the service user isn't in the `video` group — `install-pi.sh` handles that automatically via `SupplementaryGroups=video` in the systemd unit.

### 5. (Optional) Custom ffmpeg command

The brand definition's `CaptureCommandTemplate` covers most USB cameras. If you need extra flags (a specific resolution, framerate, input format, etc.) set `CameraBrand = Custom` and put the full command in `UsbCommand`, using `{deviceName}` as the placeholder. Example for a webcam that defaults to YUYV but supports MJPEG at full HD:

```
ffmpeg -f v4l2 -input_format mjpeg -video_size 1920x1080 -i {deviceName} -frames:v 1 -q:v 2 -y -
```

## Installing ffmpeg

USB cameras need ffmpeg on the PATH. IP cameras (Hikvision, Dahua, etc.) do **not** — they use plain HTTP snapshot URLs.

### Windows

**Option A — winget (recommended on Win 11 / Win 10 with App Installer):**

```powershell
winget install --id Gyan.FFmpeg
```

**Option B — Chocolatey:**

```powershell
choco install ffmpeg -y
```

**Option C — manual download:**

1. Download a `release-essentials` build from <https://www.gyan.dev/ffmpeg/builds/>
2. Extract to `C:\ffmpeg\`
3. Add `C:\ffmpeg\bin` to the **System** `Path` (Settings → System → About → Advanced system settings → Environment Variables → System variables → Path → Edit → New)
4. Open a new PowerShell and verify: `ffmpeg -version`

Restart the CameraService Windows service after the PATH change so it picks up the new environment.

To list available DirectShow USB devices (for `UsbDeviceName`):

```powershell
ffmpeg -list_devices true -f dshow -i dummy
```

Copy the exact name (e.g. `USB Video Device`) into the camera's `UsbDeviceName` field.

### Raspberry Pi — recommended (native via `install-pi.sh`)

> 👉 **For the standard "one Pi, one USB camera" deployment, use [`install-pi.sh`](./install-pi.sh).** Skip Docker entirely. The script apt-installs ffmpeg + v4l-utils, publishes the service, registers a systemd unit, and starts it. Less moving parts, faster boot, easier `journalctl` debugging, and no Docker daemon overhead. Re-running it after a `git pull` upgrades the binaries while preserving `camera-service.db` and `camera-snapshot.json`.

If you'd rather wire it up by hand, the apt + manual systemd path is:

```bash
sudo apt update
sudo apt install -y ffmpeg v4l-utils
ffmpeg -version
```

`v4l-utils` is optional but gives you `v4l2-ctl --list-devices` to confirm USB camera nodes like `/dev/video0`. Use that as the `UsbDeviceName` (e.g. `/dev/video0`). Then drop a systemd unit per the [Install as a Linux systemd service manually](#install-as-a-linux-systemd-service-manually) section below.

### Raspberry Pi — Docker (only for special cases)

> **Don't do this on Windows.** Docker Desktop on Windows runs Linux containers inside a Hyper-V / WSL2 VM that can't see USB cameras without `usbipd-win` bind-and-attach for every device — and the binding resets on every reboot and USB hub renumeration. Even when the device is forwarded, the container sees a V4L2 node instead of the DirectShow stack the Windows ffmpeg brand definition expects. **Use [`install-windows.ps1`](./install-windows.ps1) on Windows** — it winget-installs ffmpeg cleanly into `%LOCALAPPDATA%` and registers a native Windows Service.

> ⚠ **Don't use Docker on a dedicated, single-purpose Pi either.** For the typical deployment (one Pi at one site running just this service + one USB camera) native is simpler in every operational dimension — fewer steps, no Docker daemon overhead, faster boot, easier debugging via `journalctl -u camera-service -f`. The "don't pollute the host" argument only matters when the host has other things on it to be polluted; on a flashed-from-image Pi that runs only this service, there's nothing to protect.
>
> The Docker recipe is here for: (a) read-only / immutable Pi OS images where apt-install can't persist, (b) fleets managed by a container deployment pipeline like Portainer / Balena, (c) hosts already running multiple services where you want clean per-service isolation. None of those describe a dedicated camera Pi.

Run the CameraService itself inside a container with ffmpeg already installed:

**`Dockerfile`** (next to `CameraService.csproj`):

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish CameraService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update \
 && apt-get install -y --no-install-recommends ffmpeg v4l-utils \
 && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5210
ENTRYPOINT ["dotnet", "CameraService.dll"]
```

**`docker-compose.yml`**:

```yaml
services:
  camera-service:
    build: .
    container_name: camera-service
    restart: unless-stopped
    network_mode: host          # easiest path for SignalR + MJPEG inbound
    devices:
      - /dev/video0:/dev/video0 # add a line per USB camera node
    volumes:
      - ./data:/app/data        # persists camera-service.db + camera-snapshot.json
    environment:
      - ASPNETCORE_URLS=http://0.0.0.0:5210
```

Build + run:

```bash
docker compose up --build -d
docker compose logs -f camera-service
```

To get an interactive ffmpeg-only image for testing (without rebuilding the service):

```bash
docker run --rm -it \
  --device /dev/video0 \
  jrottenberg/ffmpeg:6.1-ubuntu \
  -f v4l2 -i /dev/video0 -frames:v 1 -q:v 2 /dev/stdout > test.jpg
```

A successful run writes a 50–200 KB `test.jpg` to the current directory — proof that ffmpeg in the container can see the USB camera.

## SignalR hub contract

Web → service (the web sends, the service handles):

| Method                           | Args                              | Purpose                              |
|----------------------------------|-----------------------------------|--------------------------------------|
| `CaptureImage`                   | `{ Ticket, Direction, CameraId? }`| Capture a snapshot and POST it back  |
| `TestCapture`                    | `cameraId`                        | One-off capture echoed to the admin UI |
| `AddCamera` / `UpdateCamera` / `DeleteCamera` | `CameraConfigEntity`     | CRUD persisted to local SQLite       |
| `GetCameraList` / `GetCameraBrands` | —                              | Replies on `CameraListResponse` / `CameraBrandsResponse` |
| `ReloadConfig`                   | —                                 | Restart the SignalR connection + reload settings + re-fetch brands |
| `Reannounce`                     | —                                 | Re-send the camera-announcement list to the web |

Service → web:

| Method                  | Args                                  | Purpose                              |
|-------------------------|---------------------------------------|--------------------------------------|
| `JoinCameraGroup`       | `serviceId`                           | Joins the SignalR group keyed by site |
| `AnnounceCameras`       | `{ ServiceId, CameraCount, Cameras }, streamBaseUrl` | Tells web "these cameras live behind me" |
| `CameraCrudResult`      | `{ success, operation, cameraId, … }` | Reply for each CRUD command          |
| `TestCaptureResult`     | `{ success, cameraId, image (base64) }` | Reply for `TestCapture`            |

### Upload endpoint

The web app receives each snapshot as a multipart upload:

```
POST /api/ticket/{ticket}/image?direction={in|out|bol}
Content-Type: multipart/form-data
```

Saved as `{ticket}_{Direction}.jpg` under `TicketImages:PhysicalPath` on the web.

## Sparse checkout (single-service host)

A camera-only host doesn't need the rest of the monorepo. Use a git sparse + partial clone so only this folder lands on disk and `git pull` only touches it:

```bash
git clone --filter=blob:none --no-checkout https://github.com/GTMichelli-Dev/northwest-grain-growers.git
cd northwest-grain-growers
git sparse-checkout init --cone
git sparse-checkout set GrainManagement/CameraService
git checkout master
cd GrainManagement/CameraService
```

`--filter=blob:none` defers blob downloads, so blobs outside the sparse set are never fetched. `git sparse-checkout disable` reverts to a full checkout if you change your mind. Re-runs of `install-pi.sh` / `install-windows.ps1` work normally inside the sparse tree.

## Installation

> **Quick picker** — for the common single-camera deployments:
> - **Raspberry Pi (one Pi, one USB camera)** → run [`install-pi.sh`](./install-pi.sh) as root. Native systemd, no Docker.
> - **Windows kiosk / desktop** → run [`install-windows.ps1`](./install-windows.ps1) from an elevated PowerShell. Native Windows service, no Docker.
>
> Docker is documented under [Installing ffmpeg → Raspberry Pi — Docker](#raspberry-pi--docker-only-for-special-cases) for the narrow cases where it actually helps (immutable Pi OS images, container-fleet deployments). The default recommendation for both platforms is native.

### Run as console app

```bash
dotnet run
```

Logs print the Swagger URL on startup so you can edit settings the first time.

### Install as Windows Service (one-shot script)

[`install-windows.ps1`](./install-windows.ps1) automates the whole Windows install: installs the .NET 8 ASP.NET Core runtime + Gyan.FFmpeg via winget, publishes the project win-x64, drops the binaries under `C:\Services\CameraService`, registers the Windows service via `sc.exe` (auto-start, restart-on-failure mirroring the Pi systemd unit), sets `ASPNETCORE_URLS=http://0.0.0.0:5210` as a machine-level env var, and starts the service. Idempotent — re-run after a `git pull` to upgrade (the SQLite settings DB + brand snapshot survive the rebuild).

From an **elevated PowerShell** session, in the `CameraService` folder:

```powershell
.\install-windows.ps1
```

Optional parameters:

```powershell
.\install-windows.ps1 -Port 5310 -InstallDir D:\Services\CameraService
.\install-windows.ps1 -ServiceAccount "DOMAIN\camera" -ServicePassword "..."
```

After the service is up, hit `http://<this-host>:5210/swagger` to set `ServerUrl`, `ServiceId`, `StreamBaseUrl`, and `BrandsUrl` via `PUT /api/settings`.

### Install as Windows Service manually

```powershell
dotnet publish -c Release -o C:\Services\CameraService
sc.exe create "CameraCaptureService" binPath="C:\Services\CameraService\CameraService.exe" start=auto
sc.exe start CameraCaptureService
```

To stop / remove later:

```powershell
sc.exe stop CameraCaptureService
sc.exe delete CameraCaptureService
```

### Install as a Linux systemd service on a Raspberry Pi (one-shot script)

[`install-pi.sh`](./install-pi.sh) automates the whole Pi setup: installs the .NET 8 runtime + ffmpeg + v4l-utils, publishes the project linux-arm64, drops the binaries under `/opt/camera-service`, writes a systemd unit, and starts it. Idempotent — re-running upgrades the binaries and restarts the service.

Copy the repo onto the Pi and run:

```bash
cd GrainManagement/CameraService
chmod +x install-pi.sh
sudo ./install-pi.sh
sudo journalctl -u camera-service -f
```

Pass `--user <name>` if you'd like the service to run as something other than `pi`, and `--arch arm` if you're on a 32-bit Raspberry Pi OS.

After the service is up, hit `http://<pi-ip>:5210/swagger` from a browser on the LAN to set `ServerUrl`, `ServiceId`, `StreamBaseUrl`, and `BrandsUrl` via `PUT /api/settings`.

### Install as a Linux systemd service manually

```bash
sudo cp -r ./bin/Release/net8.0/publish /opt/camera-service
cat <<'EOF' | sudo tee /etc/systemd/system/camera-service.service
[Unit]
Description=Grain Management Camera Service
After=network-online.target

[Service]
WorkingDirectory=/opt/camera-service
ExecStart=/usr/bin/dotnet /opt/camera-service/CameraService.dll
Restart=always
RestartSec=5
User=pi
Group=pi

[Install]
WantedBy=multi-user.target
EOF
sudo systemctl daemon-reload
sudo systemctl enable --now camera-service
sudo journalctl -u camera-service -f
```

## Requirements

- .NET 8 runtime (or use the Docker image, which includes it)
- ffmpeg + v4l-utils on the PATH **only when using USB cameras**
- Outbound HTTPS to `raw.githubusercontent.com` for the brand-definitions JSON
- Inbound HTTP on the configured `StreamBaseUrl` port (default 5210) so browsers can pull live MJPEG feeds
- Network access to each camera and to the GrainManagement web app
