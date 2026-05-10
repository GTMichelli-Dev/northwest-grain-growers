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

### Raspberry Pi — host install (simplest)

```bash
sudo apt update
sudo apt install -y ffmpeg v4l-utils
ffmpeg -version
```

`v4l-utils` is optional but gives you `v4l2-ctl --list-devices` to confirm USB camera nodes like `/dev/video0`. Use that as the `UsbDeviceName` (e.g. `/dev/video0`).

### Raspberry Pi — Docker (when you don't want to pollute the host)

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

## Installation

### Run as console app

```bash
dotnet run
```

Logs print the Swagger URL on startup so you can edit settings the first time.

### Install as Windows Service

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
