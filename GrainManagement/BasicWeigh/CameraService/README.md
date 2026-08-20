# Camera Capture Service

A .NET 8.0 Windows Service that captures snapshots from IP or USB cameras and uploads them to a web application via SignalR.

## How It Works

1. The service connects to your web application's SignalR hub
2. When the web app sends a `CaptureImage` command, the service grabs a snapshot
3. The image is uploaded back to the web app via its API

## Supported Cameras

Camera brand definitions are loaded from a remote JSON file ([device-definitions](https://github.com/SeanSolleder/device-definitions) repo) or a bundled local fallback.

### IP Cameras (no extra software needed)
- Hikvision, Dahua, Axis, Amcrest, Reolink, Foscam, Vivotek, Hanwha (Samsung), Uniview, ONVIF Generic, and any custom URL

### USB Cameras (requires ffmpeg)
- Windows (DirectShow) and Linux (V4L2)

#### Test a USB camera with ffmpeg

Use these commands directly from a terminal to confirm ffmpeg can see and grab a frame from the camera before configuring the service.

**Windows (DirectShow):**
```powershell
# 1. List available video devices — copy the exact device name in quotes
ffmpeg -list_devices true -f dshow -i dummy

# 2. Capture a single test frame (replace the device name with yours)
ffmpeg -f dshow -i video="USB2.0 HD UVC WebCam" -frames:v 1 -y test.jpg

# 3. Show supported resolutions / pixel formats for the device
ffmpeg -f dshow -list_options true -i video="USB2.0 HD UVC WebCam"

# 4. Live preview window (press q to quit) — handy to confirm focus / framing
ffplay -f dshow -i video="USB2.0 HD UVC WebCam"
```

**Linux (V4L2):**
```bash
# 1. List devices (install with: sudo apt install v4l-utils)
v4l2-ctl --list-devices

# 2. Capture a single test frame
ffmpeg -f v4l2 -i /dev/video0 -frames:v 1 -y test.jpg

# 3. Show supported formats / resolutions
v4l2-ctl --device=/dev/video0 --list-formats-ext

# 4. Live preview
ffplay -f v4l2 -i /dev/video0
```

If step 2 produces a valid `test.jpg`, put the same device name (`USB2.0 HD UVC WebCam` or `/dev/video0`) into the `CameraIp` / device-name field in `appsettings.json`. If ffmpeg fails here, the service will fail too — fix permissions, drivers, or device name before running the service.

## Configuration

Edit `appsettings.json`:

```json
{
  "Camera": {
    "ServerUrl": "http://localhost:5110",
    "CameraBrand": "Hikvision",
    "CameraIp": "192.168.1.100",
    "CameraUser": "admin",
    "CameraPassword": "password",
    "TimeoutSeconds": 10
  }
}
```

## SignalR Hub Contract

Your web application must implement these hub methods:

| Direction | Method | Description |
|-----------|--------|-------------|
| Server -> Service | `CaptureImage(ticket, direction)` | Tells the service to capture a snapshot |
| Service -> Server | `JoinCameraGroup()` | Registers the service in the camera group |
| Service -> Server | `ReloadConfig` | Triggers the service to reload settings from the web API |

### Upload Endpoint

The service uploads images via HTTP POST:
```
POST /api/ticket/{ticket}/image?direction={direction}
Content-Type: multipart/form-data
```

## Installation

### Run as console app
```bash
dotnet run
```

### Install as Windows Service
```bash
dotnet publish -c Release -o C:\Services\CameraService
sc create "CameraCaptureService" binPath="C:\Services\CameraService\CameraService.exe"
sc start CameraCaptureService
```

## Requirements

- .NET 8.0 Runtime
- For USB cameras: ffmpeg installed and on PATH
- Network access to the camera and the web application
