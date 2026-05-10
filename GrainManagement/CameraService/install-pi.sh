#!/usr/bin/env bash
# install-pi.sh — install / upgrade the Grain Management Camera Service on a
# Raspberry Pi (or any Debian-derived ARM/x64 host running systemd).
#
# What it does, idempotently:
#   1. Installs .NET 8 runtime via the Microsoft apt repo (skipped if already on PATH).
#   2. Installs ffmpeg + v4l-utils (skipped if already installed).
#   3. dotnet publishes this project for the chosen runtime (linux-arm64 by default).
#   4. Copies the publish output to /opt/camera-service (existing service is
#      stopped first; the local SQLite DB and brand-snapshot file are preserved).
#   5. Writes /etc/systemd/system/camera-service.service and enables it on boot.
#   6. (Re)starts the service and tails the first few seconds of journal output
#      so you can see it boot.
#
# Usage:
#   sudo ./install-pi.sh                    # default: user=pi, arch=arm64
#   sudo ./install-pi.sh --user kiosk       # run service as a different user
#   sudo ./install-pi.sh --arch arm         # 32-bit Raspberry Pi OS
#   sudo ./install-pi.sh --port 5210        # override the service port
#
# Re-run after a git pull to upgrade — the script stops the service, republishes,
# copies the new binaries, and starts the service again. The SQLite settings DB
# (camera-service.db) and the brand-definitions snapshot survive upgrades.

set -euo pipefail

# ── Parse args ─────────────────────────────────────────────────────────
SERVICE_USER="pi"
RUNTIME="linux-arm64"
PORT="5210"
INSTALL_DIR="/opt/camera-service"
SERVICE_NAME="camera-service"

while [[ $# -gt 0 ]]; do
    case "$1" in
        --user)  SERVICE_USER="$2"; shift 2 ;;
        --arch)
            case "$2" in
                arm)        RUNTIME="linux-arm" ;;
                arm64)      RUNTIME="linux-arm64" ;;
                x64)        RUNTIME="linux-x64" ;;
                *) echo "Unknown --arch: $2 (use arm, arm64, or x64)" >&2; exit 2 ;;
            esac
            shift 2 ;;
        --port)  PORT="$2"; shift 2 ;;
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
PROJECT_FILE="$SCRIPT_DIR/CameraService.csproj"

if [[ ! -f "$PROJECT_FILE" ]]; then
    echo "Can't find CameraService.csproj at $PROJECT_FILE — run this from the CameraService folder." >&2
    exit 1
fi

echo "=== Camera Service install ==="
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
    # Microsoft's apt repo covers Debian / Ubuntu / Raspbian.
    wget -qO /tmp/packages-microsoft-prod.deb \
        "https://packages.microsoft.com/config/${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}/packages-microsoft-prod.deb" || true
    if [[ -s /tmp/packages-microsoft-prod.deb ]]; then
        dpkg -i /tmp/packages-microsoft-prod.deb
        rm -f /tmp/packages-microsoft-prod.deb
        apt-get update
        apt-get install -y aspnetcore-runtime-8.0 dotnet-sdk-8.0
    else
        # Some Raspberry Pi OS releases aren't on packages.microsoft.com —
        # use the official dotnet-install.sh fallback.
        echo "   apt-repo unavailable for ${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}, using dotnet-install.sh"
        wget -qO /tmp/dotnet-install.sh https://dot.net/v1/dotnet-install.sh
        chmod +x /tmp/dotnet-install.sh
        /tmp/dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet
        ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet
    fi
else
    echo "==> .NET 8 runtime already installed — skipping."
fi

# ── 2. ffmpeg + v4l-utils (USB cameras) ────────────────────────────────
if ! command -v ffmpeg >/dev/null 2>&1; then
    echo "==> Installing ffmpeg + v4l-utils…"
    apt-get install -y ffmpeg v4l-utils
else
    echo "==> ffmpeg already installed — skipping."
fi

# ── 3. dotnet publish ──────────────────────────────────────────────────
echo "==> Publishing CameraService for $RUNTIME…"
PUBLISH_DIR="$SCRIPT_DIR/bin/Release/${RUNTIME}-publish"
rm -rf "$PUBLISH_DIR"
# Publish under the invoking user so dotnet's NuGet cache + obj/ end up
# owned by that user (re-running as a regular dev account stays clean).
# Falls back to root when the script is invoked as root directly.
PUBLISH_USER="${SUDO_USER:-root}"
if [[ "$PUBLISH_USER" != "root" ]]; then
    sudo -u "$PUBLISH_USER" dotnet publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
else
    dotnet publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
fi

# ── 4. Stop existing service + sync new binaries ───────────────────────
if systemctl is-active --quiet "$SERVICE_NAME"; then
    echo "==> Stopping existing $SERVICE_NAME service for upgrade…"
    systemctl stop "$SERVICE_NAME"
fi

mkdir -p "$INSTALL_DIR"

# Preserve the local SQLite settings DB + cached brand snapshot across
# upgrades — they hold runtime configuration the operator set via Swagger.
PRESERVE=("camera-service.db" "camera-snapshot.json")
TMPSAVE=$(mktemp -d)
for f in "${PRESERVE[@]}"; do
    if [[ -f "$INSTALL_DIR/$f" ]]; then
        cp -a "$INSTALL_DIR/$f" "$TMPSAVE/$f"
    fi
done

echo "==> Copying publish output to $INSTALL_DIR…"
rsync -a --delete "$PUBLISH_DIR/" "$INSTALL_DIR/"

for f in "${PRESERVE[@]}"; do
    if [[ -f "$TMPSAVE/$f" ]]; then
        cp -a "$TMPSAVE/$f" "$INSTALL_DIR/$f"
    fi
done
rm -rf "$TMPSAVE"

chown -R "$SERVICE_USER":"$SERVICE_USER" "$INSTALL_DIR"

# ── 5. systemd unit ────────────────────────────────────────────────────
DOTNET_BIN=$(command -v dotnet)
echo "==> Writing /etc/systemd/system/$SERVICE_NAME.service…"
cat <<EOF | tee "/etc/systemd/system/$SERVICE_NAME.service" >/dev/null
[Unit]
Description=Grain Management Camera Service
After=network-online.target
Wants=network-online.target

[Service]
WorkingDirectory=$INSTALL_DIR
ExecStart=$DOTNET_BIN $INSTALL_DIR/CameraService.dll
Restart=always
RestartSec=5
User=$SERVICE_USER
Group=$SERVICE_USER
Environment=ASPNETCORE_URLS=http://0.0.0.0:$PORT
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
# USB camera access: the user running the service needs to be in the
# 'video' group. The grouping is applied here at the unit level so we
# don't have to re-login the SSH session.
SupplementaryGroups=video
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable "$SERVICE_NAME"

# ── 6. Start + tail ────────────────────────────────────────────────────
echo "==> Starting $SERVICE_NAME…"
systemctl restart "$SERVICE_NAME"
sleep 2

if systemctl is-active --quiet "$SERVICE_NAME"; then
    echo
    echo "✅ camera-service is running on http://localhost:$PORT"
    echo "   Swagger UI:  http://$(hostname -I | awk '{print $1}'):$PORT/swagger"
    echo "   Logs:        sudo journalctl -u $SERVICE_NAME -f"
    echo "   Stop:        sudo systemctl stop $SERVICE_NAME"
    echo "   Disable:     sudo systemctl disable $SERVICE_NAME"
else
    echo
    echo "❌ Service failed to start. Last 30 log lines:"
    journalctl -u "$SERVICE_NAME" -n 30 --no-pager
    exit 1
fi
