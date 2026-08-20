# Camera Setup Guide

End-to-end instructions for getting cameras working with the GrainManagement web app. Covers Hikvision IP cameras, USB cameras on Windows, USB cameras on a Raspberry Pi, and Docker packaging for both Windows and remote (Linux) hosts.

---

## 1. Architecture in one diagram

```
┌──────────────────────────┐        SignalR /hubs/camera         ┌──────────────────────────┐
│  CameraService #1        │ ──── announces cameras ──────────▶ │  GrainManagement Web     │
│  (Windows or Pi)         │ ◀──── CaptureImage commands ──────  │  - CameraHub             │
│  - SQLite cameras db     │                                     │  - CameraRegistry        │
│  - MJPEG fan-out         │                                     │  - /api/cameras          │
└─────────┬────────────────┘                                     │  - /api/ticket/{n}/image │
          │ HTTP MJPEG                                           └─────────┬────────────────┘
          │ <img src="…/api/stream/{cam}">                                  │
          ▼                                                                 │
        Browser ◀─────────────── HTML / SignalR ──────────────────────── Browser
                                                                            │
                                                                  saves to TicketImages folder
                                                                            │
                                                                            ▼
                                                                  ┌──────────────────┐
                                                                  │  PictureUpsert   │
                                                                  │  (same host)     │
                                                                  └─────────┬────────┘
                                                                            │ POST /api/picture-upsert/ingest
                                                                            ▼
                                                                       Central HQ
```

The web is the **server**. Every CameraService is a SignalR **client** that:
1. Joins `/hubs/camera` with a `ServiceId` (host nickname).
2. Announces its cameras + an externally-reachable `StreamBaseUrl` for live view.
3. Receives `CaptureImage(ticket, direction, cameraId)` commands.
4. Pulls a snapshot, uploads it to `POST /api/ticket/{ticket}/image?direction=…`.

You can run **many** CameraServices at once (Windows server + several Pis) — each with its own SQLite. Role assignments (inbound/outbound/BOL) live in SQL Server (`system.CameraAssignments`) and are managed from the web's `/Camera` page.

---

## 2. Required ports

| Port  | Service                  | Direction                             |
|-------|--------------------------|---------------------------------------|
| 51791 | GrainManagement web      | Inbound from CameraServices + browsers |
| 5210  | CameraService            | Inbound from browsers (MJPEG)         |
| 5310  | PictureUpsert            | Inbound from browser (status/swagger) |
| 80/443 | IP cameras (Hikvision)  | Outbound from CameraService           |
| 554   | IP cameras (RTSP)        | Outbound from CameraService (if used) |

`StreamBaseUrl` on each CameraService must be the URL **a browser on the operator's PC can reach** — usually the LAN IP, not `localhost`.

---

## 3. Hikvision IP camera

### a) Camera-side setup

1. Power the camera and connect it to the LAN.
2. Find its IP with Hikvision SADP or your router's DHCP table.
3. Open `http://{cameraIp}` in a browser, log in (default `admin` / printed on the device).
4. Settings → System → Security: enable Web Service Authentication (`Digest/Basic`).
5. Settings → Network → Advanced → Integration Protocol: enable ONVIF, create an ONVIF user.
6. Confirm the snapshot URL works:

   ```
   http://{ip}/ISAPI/Streaming/channels/101/picture
   ```

   Authentication is Digest. In a browser you'll get a 401 prompt — that's expected.

### b) CameraService-side setup

1. Open the CameraService Swagger (`http://{host}:5210/swagger`).
2. `POST /api/cameras` with:

   ```json
   {
     "cameraId": "front-gate",
     "displayName": "Front Gate (Hikvision)",
     "cameraBrand": "Hikvision",
     "cameraIp":    "10.0.60.240",
     "cameraUser":  "admin",
     "cameraPassword": "Scales123",
     "timeoutSeconds": 10,
     "active": true
   }
   ```

3. Reload the web's `/Camera` page — the camera should appear under the service's `ServiceId`.

The CameraService uses Digest auth automatically for Hikvision (`CameraCaptureService.CaptureIpAsync`).

---

## 4. USB camera on Windows

### a) Install ffmpeg

```powershell
winget install Gyan.FFmpeg
```

After install, open a new shell and confirm:

```powershell
ffmpeg -version
ffmpeg -list_devices true -f dshow -i dummy
```

The second command prints the DirectShow video device names — pick the exact string for your camera (e.g. `Logitech HD Webcam C270`).

### b) CameraService config

```json
{
  "cameraId": "kiosk-usb",
  "displayName": "Kiosk USB",
  "cameraBrand": "USB Camera (Windows)",
  "usbDeviceName": "Logitech HD Webcam C270",
  "timeoutSeconds": 10,
  "active": true
}
```

The brand definitions JSON contains a `CaptureCommandTemplate` like:

```
ffmpeg -f dshow -i video="{deviceName}" -frames:v 1 -q:v 2 -y
```

If your installation needs a custom command, set `cameraBrand` to `"Custom"` and provide `usbCommand` directly.

---

## 5. USB camera on a Raspberry Pi

Pis are a great cheap host for USB cameras in the field.

### a) OS prep (Raspberry Pi OS 64-bit Lite)

```bash
sudo apt update
sudo apt install -y ffmpeg v4l-utils dotnet-runtime-8.0 dotnet-aspnetcore-runtime-8.0

# Find the USB camera device
v4l2-ctl --list-devices
# Example output: USB Camera (usb-0000:01:00.0-1.2): /dev/video0
```

### b) Deploy the CameraService

```bash
sudo mkdir -p /opt/cameraservice
sudo chown $USER /opt/cameraservice
# from your workstation, scp the publish output:
scp -r CameraService/* pi@<pi-ip>:/opt/cameraservice/
```

### c) systemd unit

```ini
# /etc/systemd/system/cameraservice.service
[Unit]
Description=GrainManagement Camera Service
After=network-online.target
Wants=network-online.target

[Service]
WorkingDirectory=/opt/cameraservice
ExecStart=/usr/bin/dotnet /opt/cameraservice/CameraService.dll
Restart=always
RestartSec=5
User=pi
Environment=ASPNETCORE_URLS=http://0.0.0.0:5210

[Install]
WantedBy=multi-user.target
```

```bash
sudo systemctl daemon-reload
sudo systemctl enable --now cameraservice
journalctl -u cameraservice -f
```

### d) Camera config on the Pi

In `appsettings.json` set `ServiceId` to something like `"shop-pi"` and `StreamBaseUrl` to `http://<pi-lan-ip>:5210`. Then add a camera with:

```json
{
  "cameraId": "yard-usb",
  "displayName": "Yard USB (Pi)",
  "cameraBrand": "USB Camera (Linux)",
  "usbDeviceName": "/dev/video0",
  "active": true
}
```

The Linux brand template is `ffmpeg -f v4l2 -i {deviceName} -frames:v 1 -q:v 2 -y`.

---

## 6. Docker — Windows host

Containers on Windows can't see USB cameras directly. Use Docker for **IP-only** CameraServices (Hikvision etc.) on the Windows host; USB cameras need to run on metal.

```dockerfile
# Cameras/CameraService.Dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish CameraService/CameraService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5210
EXPOSE 5210
ENTRYPOINT ["dotnet", "CameraService.dll"]
```

Build + run:

```powershell
docker build -f CameraService.Dockerfile -t gm/cameraservice .
docker run -d --name cameraservice `
  -p 5210:5210 `
  -v C:\cameraservice-data:/app `
  -e Camera__ServiceId=hq-windows `
  -e Camera__ServerUrl=http://host.docker.internal:51791 `
  -e Camera__StreamBaseUrl=http://<windows-lan-ip>:5210 `
  gm/cameraservice
```

> `host.docker.internal` lets the container reach the web app on the Windows host.

---

## 7. Docker — Remote (Linux) host

```bash
docker build -f Cameras/CameraService.Dockerfile -t gm/cameraservice .
docker run -d --name cameraservice \
  --restart unless-stopped \
  -p 5210:5210 \
  -e Camera__ServiceId=remote-1 \
  -e Camera__ServerUrl=https://your-grainmanagement.example.com \
  -e Camera__StreamBaseUrl=http://<linux-lan-ip>:5210 \
  --device=/dev/video0 \
  gm/cameraservice
```

`--device=/dev/video0` is what makes a USB camera visible inside the container on Linux. (Doesn't work on Windows.)

---

## 8. Assigning roles from the web

Once a CameraService is online and announcing, go to `/Camera` and either:

- Add an entry via `POST /api/cameras/assignments`:

  ```json
  {
    "serviceId": "shop-pi",
    "cameraId":  "yard-usb",
    "displayName": "Yard inbound (Pi USB)",
    "locationId": 1,
    "scaleId": 1,
    "role": "Inbound",   // or "Outbound", "BOL", "View"
    "isPrimary": true
  }
  ```

- Or wire the assignment UI on top of this API (one of the open follow-ups).

Rules:

- A camera with `Role = Inbound` and `ScaleId = X` fires on every load that comes across scale X at that location.
- A camera with `Role = Inbound` and `ScaleId = null` is **location-wide** — fires on every inbound load regardless of scale.
- Multiple cameras can share a role. Mark one `IsPrimary` and its photo becomes the ticket's main image; others save as `{load}_In_2.jpg`, `{load}_In_3.jpg`, etc.
- Any camera with `Role = BOL` makes the **Scan BOL** button visible on scale pages.

---

## 9. Troubleshooting checklist

| Symptom | First thing to check |
|---|---|
| Camera doesn't show on `/Camera` | CameraService logs — does it log "Joined camera groups"? If not, `ServerUrl` is wrong. |
| Camera shows but stream URL is empty | `StreamBaseUrl` in CameraService settings is empty or unreachable. |
| Live view box stays black | Mixed-content block — web is HTTPS but CameraService is HTTP. Either terminate TLS in front of CameraService or run the web on HTTP for that LAN. |
| Hikvision: 401 in logs | Wrong user/pass or Hikvision "Illegal login" lockout (wait 30 min). |
| USB on Windows: "device not found" | Re-run `ffmpeg -list_devices true -f dshow -i dummy` and copy the device name exactly (including quotes). |
| USB on Pi: empty file | Check `v4l2-ctl --list-formats-ext` — some cameras only emit MJPEG, not YUYV. Add `-input_format mjpeg` to the ffmpeg command. |
| Captures save but never reach Central | PictureUpsert `/api/status` → `remoteReachable: false`. Check the RemoteBaseUrl, auth token, and firewall. |
