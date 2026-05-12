#!/usr/bin/env bash
# =============================================================================
# NWGG Temp Ticket Kiosk Service — Self-Install Script for Raspberry Pi
# =============================================================================
# Run directly on the Pi:
#
#   git clone https://github.com/GTMichelli-Dev/nwgg-temp-ticket-kiosk-service.git /tmp/ttk
#   sudo bash /tmp/ttk/deploy/install.sh <web-server-url> --kiosk-id <id> --scale-id <n>
#   rm -rf /tmp/ttk
#
# Examples:
#   sudo bash install.sh http://waldv002:5000 --kiosk-id kiosk-1 --scale-id 3
#   sudo bash install.sh http://waldv002:5000 --kiosk-id loadout-a --scale-id 7 --gpio-pin 17
#
# What this script does, idempotently:
#   1. Installs .NET 8 ASP.NET Core runtime via the Microsoft apt repo
#      (falls back to dotnet-install.sh when the repo lacks packages).
#   2. Adds the service user to the 'gpio' group so it can open
#      /dev/gpiochip0 without sudo.
#   3. Publishes the project for the Pi's architecture.
#   4. Copies binaries to /opt/temp-ticket-kiosk. On first install, writes
#      a fresh appsettings.json from the flags. On upgrade, preserves the
#      existing appsettings.json so per-kiosk config isn't lost.
#   5. Opens UFW for the kiosk service port (5240/tcp) when UFW is active.
#   6. Writes /etc/systemd/system/temp-ticket-kiosk.service and starts it.
#
# Re-run with the same flags to upgrade — existing appsettings.json is
# preserved across upgrades. Pass --reset-config to overwrite it.
# =============================================================================

set -euo pipefail

# ---- Defaults ----
SERVICE_USER="admin"
KIOSK_ID=""
SCALE_ID="0"
GPIO_PIN="17"
DEBOUNCE_MS="75"
PULL_MODE="pullup"
PRINTER_TARGET=""
SERVICE_PORT="5240"
INSTALL_DIR="/opt/temp-ticket-kiosk"
SERVICE_NAME="temp-ticket-kiosk"
DOTNET_CHANNEL="8.0"
RUNTIME=""              # auto-detect if empty
WEB_URL=""
RESET_CONFIG="false"

# ---- Parse arguments ----
while [[ $# -gt 0 ]]; do
    case "$1" in
        --user)            SERVICE_USER="$2"; shift 2 ;;
        --kiosk-id)        KIOSK_ID="$2"; shift 2 ;;
        --scale-id)        SCALE_ID="$2"; shift 2 ;;
        --gpio-pin)        GPIO_PIN="$2"; shift 2 ;;
        --debounce-ms)     DEBOUNCE_MS="$2"; shift 2 ;;
        --pull-mode)       PULL_MODE="$2"; shift 2 ;;
        --printer-target)  PRINTER_TARGET="$2"; shift 2 ;;
        --port)            SERVICE_PORT="$2"; shift 2 ;;
        --install-dir)     INSTALL_DIR="$2"; shift 2 ;;
        --arch)
            case "$2" in
                arm)   RUNTIME="linux-arm" ;;
                arm64) RUNTIME="linux-arm64" ;;
                x64)   RUNTIME="linux-x64" ;;
                *) echo "Unknown --arch: $2 (use arm, arm64, or x64)" >&2; exit 2 ;;
            esac
            shift 2 ;;
        --reset-config)    RESET_CONFIG="true"; shift ;;
        --help|-h)
            cat <<HELP
Usage: install.sh <web-server-url> --kiosk-id <id> --scale-id <n> [options]

  <web-server-url>        Required. URL of the GrainManagement web app.
                          Example: http://waldv002:5000

Required (on first install):
  --kiosk-id <id>         Kiosk identifier (e.g. kiosk-1, loadout-a)
  --scale-id <n>          NWGG scale ID this kiosk pushes presses for

Optional:
  --user <name>           Linux service user (default: admin)
  --gpio-pin <n>          BCM GPIO pin for the press button (default: 17)
  --debounce-ms <n>       Software debounce in ms (default: 75)
  --pull-mode <mode>      pullup | pulldown (default: pullup)
  --printer-target <id>   Optional logical printer name (default: empty)
  --port <port>           Service port (default: 5240)
  --arch <arm|arm64|x64>  Build target (default: auto-detect from uname)
  --install-dir <path>    Install location (default: /opt/temp-ticket-kiosk)
  --reset-config          Overwrite existing appsettings.json on upgrade
  --help                  Show this help
HELP
            exit 0
            ;;
        -*)
            echo "Unknown option: $1 (use --help)" >&2
            exit 2 ;;
        *)
            if [ -z "$WEB_URL" ]; then
                WEB_URL="$1"
            else
                echo "Unknown argument: $1" >&2
                exit 2
            fi
            shift ;;
    esac
done

if [ "$EUID" -ne 0 ]; then
    echo "This script needs root. Re-run with: sudo $0 $*" >&2
    exit 1
fi

if [ -z "$WEB_URL" ]; then
    echo "ERROR: Web server URL is required." >&2
    echo "Usage: install.sh http://your-server:5000 --kiosk-id <id> --scale-id <n>" >&2
    echo "Run with --help for all options." >&2
    exit 1
fi

# Require kiosk/scale id on FRESH installs (not on upgrades, where they're
# already in the preserved appsettings.json).
EXISTING_CONFIG="${INSTALL_DIR}/appsettings.json"
if [ ! -f "$EXISTING_CONFIG" ] || [ "$RESET_CONFIG" = "true" ]; then
    if [ -z "$KIOSK_ID" ]; then
        echo "ERROR: --kiosk-id is required on first install." >&2
        exit 1
    fi
fi

echo "============================================"
echo "  NWGG Temp Ticket Kiosk Service — Install"
echo "============================================"
echo "  Web Server:     $WEB_URL"
echo "  Service User:   $SERVICE_USER"
echo "  Kiosk ID:       ${KIOSK_ID:-<preserved>}"
echo "  Scale ID:       ${SCALE_ID:-<preserved>}"
echo "  GPIO Pin:       $GPIO_PIN"
echo "  Service Port:   $SERVICE_PORT"
echo "  Install Dir:    $INSTALL_DIR"
echo "  Reset config:   $RESET_CONFIG"
echo "============================================"
echo ""

# ---- Detect architecture ----
if [ -z "$RUNTIME" ]; then
    ARCH=$(uname -m)
    case "$ARCH" in
        aarch64) RUNTIME="linux-arm64" ;;
        armv7l)  RUNTIME="linux-arm" ;;
        x86_64)  RUNTIME="linux-x64" ;;
        *)       echo "WARNING: Unknown arch '${ARCH}', defaulting to linux-arm64"; RUNTIME="linux-arm64" ;;
    esac
fi
echo "[1/6] Architecture: $RUNTIME"

# ---- Install .NET 8 ----
echo "[2/6] Checking .NET 8 runtime..."
if command -v dotnet >/dev/null 2>&1 && dotnet --list-runtimes 2>/dev/null | grep -q "Microsoft.AspNetCore.App 8\."; then
    echo "  .NET 8 already installed."
else
    echo "  Installing .NET 8..."
    OS_RELEASE_VERSION_ID=$(. /etc/os-release && echo "$VERSION_ID")
    OS_RELEASE_ID=$(. /etc/os-release && echo "$ID")
    wget -qO /tmp/packages-microsoft-prod.deb \
        "https://packages.microsoft.com/config/${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}/packages-microsoft-prod.deb" || true
    if [ -s /tmp/packages-microsoft-prod.deb ]; then
        dpkg -i /tmp/packages-microsoft-prod.deb
        rm -f /tmp/packages-microsoft-prod.deb
        apt-get update -qq
        apt-get install -y -qq aspnetcore-runtime-8.0 dotnet-sdk-8.0
    else
        echo "  apt-repo unavailable for ${OS_RELEASE_ID}/${OS_RELEASE_VERSION_ID}, using dotnet-install.sh"
        wget -qO /tmp/dotnet-install.sh https://dot.net/v1/dotnet-install.sh
        chmod +x /tmp/dotnet-install.sh
        /tmp/dotnet-install.sh --channel ${DOTNET_CHANNEL} --install-dir /usr/share/dotnet
        ln -sf /usr/share/dotnet/dotnet /usr/local/bin/dotnet
    fi
fi
DOTNET_BIN=$(command -v dotnet)

# ---- GPIO group ----
echo "[3/6] GPIO group setup..."
if ! getent group gpio >/dev/null; then
    groupadd -r gpio || true
fi
if id "$SERVICE_USER" >/dev/null 2>&1; then
    usermod -aG gpio "$SERVICE_USER" || true
    echo "  Added $SERVICE_USER to gpio group."
else
    echo "  WARNING: user '$SERVICE_USER' does not exist. Use --user to set the right service account." >&2
fi

# ---- Locate source: prefer this script's parent (cloned repo), else $PWD ----
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"
PROJECT_FILE="$PROJECT_DIR/TempTicketKioskService.csproj"

if [ ! -f "$PROJECT_FILE" ]; then
    echo "ERROR: TempTicketKioskService.csproj not found at $PROJECT_FILE." >&2
    echo "       This script must be run from inside the cloned repo, e.g.:" >&2
    echo "       sudo bash /tmp/ttk/deploy/install.sh <web-server-url> ..." >&2
    exit 1
fi

# ---- Stop service if running ----
sudo systemctl stop ${SERVICE_NAME} 2>/dev/null || true

# ---- Publish ----
echo "[4/6] Publishing for $RUNTIME..."
PUBLISH_DIR="$(mktemp -d)"
trap 'rm -rf "$PUBLISH_DIR"' EXIT

PUBLISH_USER="${SUDO_USER:-root}"
if [ "$PUBLISH_USER" != "root" ] && id "$PUBLISH_USER" >/dev/null 2>&1; then
    sudo -u "$PUBLISH_USER" "$DOTNET_BIN" publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
else
    "$DOTNET_BIN" publish "$PROJECT_FILE" \
        -c Release -r "$RUNTIME" --self-contained false -o "$PUBLISH_DIR"
fi

# ---- Sync binaries (preserve appsettings.json unless --reset-config) ----
echo "[5/6] Installing to $INSTALL_DIR..."
mkdir -p "$INSTALL_DIR"

TMPSAVE=""
if [ -f "$EXISTING_CONFIG" ] && [ "$RESET_CONFIG" != "true" ]; then
    TMPSAVE=$(mktemp -d)
    cp -a "$EXISTING_CONFIG" "$TMPSAVE/appsettings.json"
    echo "  Preserving existing appsettings.json."
fi

rsync -a --delete "$PUBLISH_DIR/" "$INSTALL_DIR/"

if [ -n "$TMPSAVE" ]; then
    cp -a "$TMPSAVE/appsettings.json" "$EXISTING_CONFIG"
    rm -rf "$TMPSAVE"
else
    # Fresh install (or --reset-config): write appsettings.json from flags.
    if command -v python3 >/dev/null 2>&1; then
        python3 - "$EXISTING_CONFIG" \
            "$KIOSK_ID" "$SCALE_ID" "$PRINTER_TARGET" "$WEB_URL" \
            "$GPIO_PIN" "$DEBOUNCE_MS" "$PULL_MODE" "$SERVICE_PORT" <<'PY'
import json, sys
path, kiosk_id, scale_id, printer_target, server_url, gpio_pin, debounce_ms, pull_mode, port = sys.argv[1:]
with open(path) as f:
    cfg = json.load(f)
cfg.setdefault('Kiosk', {})
cfg['Kiosk']['KioskId']       = kiosk_id
cfg['Kiosk']['ScaleId']       = int(scale_id)
cfg['Kiosk']['PrinterTarget'] = printer_target
cfg['Kiosk']['ServerUrl']     = server_url
cfg['Kiosk']['GpioPin']       = int(gpio_pin)
cfg['Kiosk']['DebounceMs']    = int(debounce_ms)
cfg['Kiosk']['PullMode']      = pull_mode
cfg.setdefault('Kestrel', {}).setdefault('Endpoints', {}).setdefault('Http', {})
cfg['Kestrel']['Endpoints']['Http']['Url'] = f"http://0.0.0.0:{port}"
with open(path, 'w') as f:
    json.dump(cfg, f, indent=2)
PY
        echo "  Wrote appsettings.json (KioskId=$KIOSK_ID, ScaleId=$SCALE_ID, ServerUrl=$WEB_URL)."
    else
        echo "  WARNING: python3 not found — appsettings.json left at the defaults from the build." >&2
    fi
fi

chown -R "$SERVICE_USER:$SERVICE_USER" "$INSTALL_DIR" 2>/dev/null || true

# ---- UFW ----
if command -v ufw >/dev/null 2>&1 && ufw status | grep -q "Status: active"; then
    echo "  Opening UFW for ${SERVICE_PORT}/tcp..."
    ufw allow "${SERVICE_PORT}/tcp" >/dev/null
fi

# ---- systemd unit ----
echo "[6/6] Writing systemd unit..."
cat <<EOF | tee "/etc/systemd/system/${SERVICE_NAME}.service" >/dev/null
[Unit]
Description=NWGG Temp Ticket Kiosk Service
After=network-online.target
Wants=network-online.target

[Service]
WorkingDirectory=${INSTALL_DIR}
ExecStart=${DOTNET_BIN} ${INSTALL_DIR}/TempTicketKioskService.dll
Restart=always
RestartSec=5
User=${SERVICE_USER}
Group=${SERVICE_USER}
SupplementaryGroups=gpio
Environment=ASPNETCORE_URLS=http://0.0.0.0:${SERVICE_PORT}
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

systemctl daemon-reload
systemctl enable ${SERVICE_NAME}
systemctl restart ${SERVICE_NAME}
sleep 2

if systemctl is-active --quiet ${SERVICE_NAME}; then
    IP=$(hostname -I | awk '{print $1}')
    echo ""
    echo "============================================"
    echo "  Install complete — service is running."
    echo "============================================"
    echo "  Address:      http://${IP}:${SERVICE_PORT}"
    echo "  Swagger:      http://${IP}:${SERVICE_PORT}/swagger"
    echo "  Config:       ${INSTALL_DIR}/appsettings.json"
    echo "  Test press:   curl -X POST http://${IP}:${SERVICE_PORT}/api/test/press"
    echo "  Logs:         sudo journalctl -u ${SERVICE_NAME} -f"
    echo "  Restart:      sudo systemctl restart ${SERVICE_NAME}"
    echo "============================================"
else
    echo ""
    echo "WARNING: Service didn't start. Last 30 log lines:" >&2
    journalctl -u ${SERVICE_NAME} -n 30 --no-pager >&2
    exit 1
fi
