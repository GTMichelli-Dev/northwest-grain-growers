#!/usr/bin/env bash
# install-pi.sh — install / upgrade the Grain Management Temp Ticket
# Kiosk Service on a Raspberry Pi (or any Debian-derived ARM/x64 host).
#
# What it does, idempotently:
#   1. Installs .NET 8 ASP.NET Core runtime via the Microsoft apt repo
#      (falls back to dotnet-install.sh when the repo lacks packages).
#   2. dotnet publishes the project (linux-arm64 by default).
#   3. Copies binaries to /opt/temp-ticket-kiosk, preserving
#      appsettings.json across upgrades.
#   4. Adds the service user to the 'gpio' group so it can open
#      /dev/gpiochip0 without sudo.
#   5. Writes /etc/systemd/system/temp-ticket-kiosk.service and starts it.
#   6. Opens UFW for the kiosk service port (5240/tcp) when UFW is active.
#
# Usage:
#   sudo ./install-pi.sh                     # default: user=pi, arch=arm64, port=5240
#   sudo ./install-pi.sh --user kiosk
#   sudo ./install-pi.sh --arch arm          # 32-bit Raspberry Pi OS
#   sudo ./install-pi.sh --port 5240

set -euo pipefail

SERVICE_USER="pi"
RUNTIME="linux-arm64"
PORT="5240"
INSTALL_DIR="/opt/temp-ticket-kiosk"
SERVICE_NAME="temp-ticket-kiosk"

while [[ $# -gt 0 ]]; do
    case "$1" in
        --user) SERVICE_USER="$2"; shift 2 ;;
        --arch)
            case "$2" in
                arm)   RUNTIME="linux-arm" ;;
                arm64) RUNTIME="linux-arm64" ;;
                x64)   RUNTIME="linux-x64" ;;
                *) echo "Unknown --arch: $2 (use arm, arm64, or x64)" >&2; exit 2 ;;
            esac
            shift 2 ;;
        --port) PORT="$2"; shift 2 ;;
        -h|--help)
            sed -n '1,/^set -euo pipefail/p' "$0" | sed 's/^# \{0,1\}//'
            exit 0 ;;
        *) echo "Unknown arg: $1" >&2; exit 2 ;;
    esac
done

if [[ "$EUID" -ne 0 ]]; then
    echo "This script needs root. Re-run with: sudo $0 $*" >&2
    exit 1
fi

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_FILE="$SCRIPT_DIR/TempTicketKioskService.csproj"

if [[ ! -f "$PROJECT_FILE" ]]; then
    echo "Can't find TempTicketKioskService.csproj at $PROJECT_FILE — run this from the TempTicketKioskService folder." >&2
    exit 1
fi

echo "=== Temp Ticket Kiosk Service install ==="
echo "Project:        $PROJECT_FILE"
echo "Runtime:        $RUNTIME"
echo "Service user:   $SERVICE_USER"
echo "Install dir:    $INSTALL_DIR"
echo "Service port:   $PORT"
echo

# ── 1. .NET 8 runtime ──────────────────────────────────────────────────
if ! command -v dotnet >/dev/null 2>&1 || ! dotnet --list-runtimes 2>/dev/null | grep -q "Microsoft.AspNetCore.App 8\."; then
    echo "==> Installing .NET 8 ASP.NET Core runtime…"
    OS_RELEASE_VERSION_ID=$(. /etc/os-release && echo "$VERSION_ID")
    OS_RELEASE_ID=$(. /etc/os-release && echo "$ID")
    wget -qO /tmp/packages-microsoft-prod.deb \
        "https://packages.microsoft.com/config/${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}/packages-microsoft-prod.deb" || true
    if [[ -s /tmp/packages-microsoft-prod.deb ]]; then
        dpkg -i /tmp/packages-microsoft-prod.deb
        rm -f /tmp/packages-microsoft-prod.deb
        apt-get update
        apt-get install -y aspnetcore-runtime-8.0 dotnet-sdk-8.0
    else
        echo "   apt-repo unavailable for ${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}, using dotnet-install.sh"
        wget -qO /tmp/dotnet-install.sh https://dot.net/v1/dotnet-install.sh
        chmod +x /tmp/dotnet-install.sh
        /tmp/dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet
        ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet
    fi
else
    echo "==> .NET 8 runtime already installed — skipping."
fi

# ── 2. GPIO group membership ──────────────────────────────────────────
# The service user must be in the 'gpio' group to open /dev/gpiochip0
# without sudo. Raspberry Pi OS pre-creates the group; on plain Debian
# we create it if missing.
if ! getent group gpio >/dev/null; then
    groupadd -r gpio || true
fi
if id "$SERVICE_USER" >/dev/null 2>&1; then
    usermod -aG gpio "$SERVICE_USER" || true
fi

# ── 3. dotnet publish ─────────────────────────────────────────────────
echo "==> Publishing TempTicketKioskService for $RUNTIME…"
PUBLISH_DIR="$SCRIPT_DIR/bin/Release/${RUNTIME}-publish"
rm -rf "$PUBLISH_DIR"
PUBLISH_USER="${SUDO_USER:-root}"
if [[ "$PUBLISH_USER" != "root" ]]; then
    sudo -u "$PUBLISH_USER" dotnet publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
else
    dotnet publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
fi

# ── 4. Sync binaries (preserve appsettings.json) ──────────────────────
if systemctl is-active --quiet "$SERVICE_NAME"; then
    echo "==> Stopping existing $SERVICE_NAME for upgrade…"
    systemctl stop "$SERVICE_NAME"
fi

mkdir -p "$INSTALL_DIR"
PRESERVE=("appsettings.json")
TMPSAVE=$(mktemp -d)
for f in "${PRESERVE[@]}"; do
    [[ -f "$INSTALL_DIR/$f" ]] && cp -a "$INSTALL_DIR/$f" "$TMPSAVE/$f"
done

echo "==> Copying publish output to $INSTALL_DIR…"
rsync -a --delete "$PUBLISH_DIR/" "$INSTALL_DIR/"

for f in "${PRESERVE[@]}"; do
    [[ -f "$TMPSAVE/$f" ]] && cp -a "$TMPSAVE/$f" "$INSTALL_DIR/$f"
done
rm -rf "$TMPSAVE"

chown -R "$SERVICE_USER":"$SERVICE_USER" "$INSTALL_DIR"

# ── 5. UFW (open service port) ─────────────────────────────────────────
if command -v ufw >/dev/null 2>&1 && ufw status | grep -q "Status: active"; then
    echo "==> Opening UFW for the service ($PORT/tcp)…"
    ufw allow "${PORT}/tcp" >/dev/null
fi

# ── 6. systemd unit ───────────────────────────────────────────────────
DOTNET_BIN=$(command -v dotnet)
echo "==> Writing /etc/systemd/system/$SERVICE_NAME.service…"
cat <<EOF | tee "/etc/systemd/system/$SERVICE_NAME.service" >/dev/null
[Unit]
Description=Grain Management Temp Ticket Kiosk Service
After=network-online.target
Wants=network-online.target

[Service]
WorkingDirectory=$INSTALL_DIR
ExecStart=$DOTNET_BIN $INSTALL_DIR/TempTicketKioskService.dll
Restart=always
RestartSec=5
User=$SERVICE_USER
Group=$SERVICE_USER
SupplementaryGroups=gpio
Environment=ASPNETCORE_URLS=http://0.0.0.0:$PORT
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable "$SERVICE_NAME"
systemctl restart "$SERVICE_NAME"
sleep 2

if systemctl is-active --quiet "$SERVICE_NAME"; then
    IP=$(hostname -I | awk '{print $1}')
    echo
    echo "temp-ticket-kiosk is running on $IP:$PORT."
    echo "   Swagger:        http://$IP:$PORT/swagger"
    echo "   Config:         $INSTALL_DIR/appsettings.json  (edit Kiosk block, then restart)"
    echo "   Test press:     curl -X POST http://$IP:$PORT/api/test/press"
    echo "   Logs:           sudo journalctl -u $SERVICE_NAME -f"
    echo "   Restart:        sudo systemctl restart $SERVICE_NAME"
    echo "   Stop:           sudo systemctl stop $SERVICE_NAME"
    echo "   Disable:        sudo systemctl disable $SERVICE_NAME"
    echo
else
    echo
    echo "Service failed to start. Last 30 log lines:"
    journalctl -u "$SERVICE_NAME" -n 30 --no-pager
    exit 1
fi
