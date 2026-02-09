# GMScaleService – Linux systemd Setup

This guide explains how to install **GMScaleService** as a Linux **systemd** service so it starts automatically on boot and restarts if it stops.

Tested on:
- Debian / Ubuntu
- Raspberry Pi OS
- Headless kiosk and cloud VMs

---

## Install Location

Place the application in a stable directory:

/home/admin/apps/GMScaleService

Example:

sudo mkdir -p /home/admin/apps/GMScaleService
sudo cp -a /home/admin/Documents/GMScaleService/* /home/admin/apps/GMScaleService/
sudo chown -R admin:admin /home/admin/apps/GMScaleService

Keep `appsettings.json` in the same directory as the executable or `.dll`.

---

## Runtime Type

### Framework-dependent (.dll)
File present:
GMScaleService.dll

Requires:
/usr/bin/dotnet

### Self-contained executable
File present:
GMScaleService

No .NET runtime required.

---

## Create the systemd Service

Create the service file:

sudo nano /etc/systemd/system/gmscaleservice.service

### Framework-dependent (.dll)

[Unit]
Description=GMScaleService
After=network-online.target
Wants=network-online.target

[Service]
Type=simple
User=admin
WorkingDirectory=/home/admin/apps/GMScaleService
ExecStart=/usr/bin/dotnet /home/admin/apps/GMScaleService/GMScaleService.dll
Restart=always
RestartSec=3
Environment=DOTNET_ENVIRONMENT=Production
UMask=0002

[Install]
WantedBy=multi-user.target

---

### Self-contained executable

[Unit]
Description=GMScaleService
After=network-online.target
Wants=network-online.target

[Service]
Type=simple
User=admin
WorkingDirectory=/home/admin/apps/GMScaleService
ExecStart=/home/admin/apps/GMScaleService/GMScaleService
Restart=always
RestartSec=3

[Install]
WantedBy=multi-user.target

Make executable if needed:

chmod +x /home/admin/apps/GMScaleService/GMScaleService

---

## Enable and Start

sudo systemctl daemon-reload
sudo systemctl enable gmscaleservice
sudo systemctl start gmscaleservice

Check status:

sudo systemctl status gmscaleservice -l --no-pager

---

## Logs

Live logs:

sudo journalctl -u gmscaleservice -f

Recent logs:

sudo journalctl -u gmscaleservice -n 200 --no-pager

---

## Common Fixes

### Service fails to start
- Verify ExecStart path
- Ensure file exists
- Check permissions

ls -la /home/admin/apps/GMScaleService

---

### USB / Serial devices
If using scales or USB adapters:

sudo usermod -aG dialout,plugdev admin

Log out and back in after running this.

---

## Updating the App

sudo systemctl stop gmscaleservice
sudo cp -a ./publish/* /home/admin/apps/GMScaleService/
sudo systemctl start gmscaleservice

---

## Notes

- Automatically restarts on failure
- Starts after network is online
- Designed
