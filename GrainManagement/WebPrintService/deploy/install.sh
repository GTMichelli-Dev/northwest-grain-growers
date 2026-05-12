#!/bin/bash
# =============================================================================
# Web Print Service - Self-Install Script for Raspberry Pi / Linux
# =============================================================================
# Run directly on the Pi:
#
#   git clone https://github.com/GTMichelli-Dev/web-print-service.git /tmp/wps
#   bash /tmp/wps/deploy/install.sh <web-server-url>
#   rm -rf /tmp/wps
#
# Examples:
#   bash install.sh http://basicscale.scaledata.net
#   bash install.sh http://basicscale.scaledata.net --server-id 2 --printer-name Kiosk
#   bash install.sh http://basicscale.scaledata.net --bk3-ppd /opt/bixolon/BK3.ppd
#
# To update an existing install, run the same command again — it stops the
# service, updates files, preserves the database, and restarts.
# =============================================================================

set -e

# ---- Defaults ----
SERVICE_ID="default"
SERVER_ID="1"
PRINTER_NAME="Kiosk"
SERVICE_PORT="5230"
INSTALL_DIR="/opt/web-print-service"
SERVICE_NAME="web-print-service"
DOTNET_CHANNEL="8.0"
GITHUB_REPO="GTMichelli-Dev/web-print-service"
BRANCH="master"
WEB_URL=""
BK3_PPD=""  # optional PPD file path; falls back to lpinfo -m match, then raw

# ---- Parse arguments ----
while [[ $# -gt 0 ]]; do
    case "$1" in
        --service-id)   SERVICE_ID="$2"; shift 2 ;;
        --server-id)    SERVER_ID="$2"; shift 2 ;;
        --printer-name) PRINTER_NAME="$2"; shift 2 ;;
        --port)         SERVICE_PORT="$2"; shift 2 ;;
        --branch)       BRANCH="$2"; shift 2 ;;
        --install-dir)  INSTALL_DIR="$2"; shift 2 ;;
        --bk3-ppd)      BK3_PPD="$2"; shift 2 ;;
        --help|-h)
            cat <<HELP
Usage: install.sh <web-server-url> [options]

  <web-server-url>        Required. URL of the BasicWeigh web server.
                          Example: http://basicscale.scaledata.net

Options:
  --server-id <id>        Server ID for this kiosk (default: 1)
  --printer-name <name>   CUPS printer queue name (default: Kiosk)
  --service-id <id>       SignalR group ID for this service (default: default)
  --port <port>           API port (default: 5230)
  --branch <branch>       Git branch to install (default: master)
  --install-dir <path>    Install location (default: /opt/web-print-service)
  --bk3-ppd <path>        Path to BIXOLON BK3 PPD file (optional; defaults to raw)
  --help                  Show this help
HELP
            exit 0
            ;;
        -*)
            echo "Unknown option: $1 (use --help for usage)"
            exit 1
            ;;
        *)
            if [ -z "$WEB_URL" ]; then
                WEB_URL="$1"
            else
                echo "Unknown argument: $1"
                exit 1
            fi
            shift
            ;;
    esac
done

if [ -z "$WEB_URL" ]; then
    echo "ERROR: Web server URL is required."
    echo ""
    echo "Usage: install.sh http://your-server:5110 [--server-id 1] [--printer-name Kiosk]"
    echo "Run with --help for all options."
    exit 1
fi

echo ""
echo "============================================"
echo "  Web Print Service - Install"
echo "============================================"
echo "  Web Server:    ${WEB_URL}"
echo "  Server ID:     ${SERVER_ID}"
echo "  Service ID:    ${SERVICE_ID}"
echo "  Printer Name:  ${PRINTER_NAME}"
echo "  Port:          ${SERVICE_PORT}"
echo "  Install Dir:   ${INSTALL_DIR}"
echo "  Branch:        ${BRANCH}"
echo "============================================"
echo ""

# ---- Detect architecture ----
echo "[1/7] Detecting system..."
ARCH=$(uname -m)
case "$ARCH" in
    aarch64) RID="linux-arm64" ;;
    armv7l)  RID="linux-arm" ;;
    x86_64)  RID="linux-x64" ;;
    *)       echo "WARNING: Unknown arch '${ARCH}', trying linux-arm64"; RID="linux-arm64" ;;
esac
echo "  OS: $(uname -s) $(uname -r)"
echo "  Architecture: ${ARCH} (${RID})"

# ---- Install CUPS ----
echo "[2/7] Installing CUPS..."
if command -v lpstat &> /dev/null; then
    echo "  CUPS already installed."
else
    echo "  Installing CUPS..."
    sudo apt-get update -qq
    sudo apt-get install -y -qq cups
    echo "  CUPS installed."
fi

if ! groups | grep -q lpadmin; then
    sudo usermod -aG lpadmin "$USER"
    echo "  Added $USER to lpadmin group."
fi

sudo systemctl enable cups 2>/dev/null || true
sudo systemctl start cups 2>/dev/null || true
echo "  CUPS running. Admin UI: http://localhost:631"

# ---- Install .NET 8 ----
echo "[3/7] Installing .NET runtime..."
DOTNET_ROOT="$HOME/.dotnet"

if [ -x "$DOTNET_ROOT/dotnet" ]; then
    DOTNET_VER=$("$DOTNET_ROOT/dotnet" --version 2>/dev/null || echo "unknown")
    echo "  .NET already installed: ${DOTNET_VER}"
elif command -v dotnet &> /dev/null; then
    DOTNET_VER=$(dotnet --version 2>/dev/null || echo "unknown")
    DOTNET_ROOT=$(dirname "$(which dotnet)")
    echo "  .NET already installed: ${DOTNET_VER}"
else
    echo "  Downloading .NET ${DOTNET_CHANNEL} ASP.NET Core runtime..."
    sudo apt-get install -y -qq curl libicu-dev 2>/dev/null || true
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \
        --channel ${DOTNET_CHANNEL} \
        --runtime aspnetcore \
        --install-dir "$DOTNET_ROOT"
    echo "  .NET installed: $($DOTNET_ROOT/dotnet --version)"
fi

export PATH="$DOTNET_ROOT:$PATH"
export DOTNET_ROOT

if ! grep -q 'DOTNET_ROOT' "$HOME/.bashrc" 2>/dev/null; then
    echo "" >> "$HOME/.bashrc"
    echo "# .NET" >> "$HOME/.bashrc"
    echo "export DOTNET_ROOT=$DOTNET_ROOT" >> "$HOME/.bashrc"
    echo 'export PATH=$DOTNET_ROOT:$PATH' >> "$HOME/.bashrc"
    echo "  Added .NET to PATH in .bashrc"
fi

# ---- Install SDK for building (if not present) ----
echo "[4/7] Downloading and building Web Print Service..."

HAS_SDK=false
if dotnet --list-sdks 2>/dev/null | grep -q "8\."; then
    HAS_SDK=true
fi

sudo systemctl stop ${SERVICE_NAME} 2>/dev/null || true

sudo mkdir -p "${INSTALL_DIR}"
sudo chown "$USER:$USER" "${INSTALL_DIR}"

DB_BACKUP=""
if [ -f "${INSTALL_DIR}/webprintservice.db" ]; then
    DB_BACKUP="/tmp/webprintservice-db-backup.db"
    cp "${INSTALL_DIR}/webprintservice.db" "$DB_BACKUP"
    echo "  Backed up existing database."
fi

CLONE_DIR=$(mktemp -d)
echo "  Cloning from GitHub: ${GITHUB_REPO} (${BRANCH})..."
sudo apt-get install -y -qq git 2>/dev/null || true
git clone --depth 1 --branch "${BRANCH}" "https://github.com/${GITHUB_REPO}.git" "${CLONE_DIR}"

if [ "$HAS_SDK" = true ]; then
    echo "  .NET SDK already installed."
else
    echo "  Installing .NET SDK permanently (reused on future updates)..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \
        --channel ${DOTNET_CHANNEL} \
        --install-dir "$DOTNET_ROOT"
    echo "  .NET SDK installed to $DOTNET_ROOT"
fi

echo "  Building..."
dotnet publish "${CLONE_DIR}/WebPrintService.csproj" \
    -c Release \
    -r "${RID}" \
    --self-contained true \
    -o "${INSTALL_DIR}" \
    -p:PublishSingleFile=false \
    -p:PublishTrimmed=false

rm -rf "${CLONE_DIR}"

if [ -n "$DB_BACKUP" ] && [ -f "$DB_BACKUP" ]; then
    cp "$DB_BACKUP" "${INSTALL_DIR}/webprintservice.db"
    rm "$DB_BACKUP"
    echo "  Restored existing database."
fi

chmod +x "${INSTALL_DIR}/WebPrintService" 2>/dev/null || true

# ---- Configure ----
echo "[5/7] Configuring..."

# Update appsettings.json with web URL, server-id, service-id, port
if [ -f "${INSTALL_DIR}/appsettings.json" ] && command -v python3 &> /dev/null; then
    python3 - "${INSTALL_DIR}/appsettings.json" "${WEB_URL}" "${SERVER_ID}" "${SERVICE_ID}" "${SERVICE_PORT}" <<'PY'
import json, sys
path, web_url, server_id, service_id, port = sys.argv[1:]
with open(path) as f:
    config = json.load(f)
config.setdefault('Print', {})
config['Print']['ServiceId'] = service_id
config['Print']['ServerId'] = int(server_id)
config['Print']['ServerUrls'] = [web_url]
config['Print']['Port'] = port
with open(path, 'w') as f:
    json.dump(config, f, indent=2)
PY
    echo "  Updated appsettings.json (ServerId=${SERVER_ID}, ServerUrls=[${WEB_URL}])."
fi

# ---- BIXOLON BK3 USB printer setup ----
echo "[6/7] Setting up BIXOLON BK3 USB printer '${PRINTER_NAME}'..."

# Find a USB device URI — prefer a Bixolon match, otherwise first USB device.
BK3_USB=""
if command -v lpinfo &> /dev/null; then
    BK3_USB=$(lpinfo -v 2>/dev/null | awk '/usb:\/\// && /BIXOLON/ {print $2; exit}')
    if [ -z "$BK3_USB" ]; then
        BK3_USB=$(lpinfo -v 2>/dev/null | awk '/usb:\/\// {print $2; exit}')
    fi
fi
[ -z "$BK3_USB" ] && BK3_USB="usb://BIXOLON/BK3-3"

# Pick a driver: explicit PPD > lpinfo BIXOLON-BK3 model match > raw queue.
DRIVER_FLAG="-m raw"
if [ -n "$BK3_PPD" ] && [ -f "$BK3_PPD" ]; then
    DRIVER_FLAG="-P $BK3_PPD"
    echo "  Using PPD: $BK3_PPD"
elif command -v lpinfo &> /dev/null; then
    BK3_MODEL=$(lpinfo -m 2>/dev/null | awk '/BIXOLON.*BK3/ {print $1; exit}')
    if [ -n "$BK3_MODEL" ]; then
        DRIVER_FLAG="-m $BK3_MODEL"
        echo "  Using CUPS model: $BK3_MODEL"
    else
        echo "  No BIXOLON BK3 PPD found in CUPS — using raw queue."
        echo "  (Pass --bk3-ppd /path/to/BK3.ppd to use the BIXOLON driver.)"
    fi
fi

if lpstat -p "$PRINTER_NAME" &> /dev/null; then
    echo "  Printer '$PRINTER_NAME' already exists — updating device URI..."
    sudo lpadmin -p "$PRINTER_NAME" -v "$BK3_USB" $DRIVER_FLAG -E
else
    echo "  Adding printer '$PRINTER_NAME' on $BK3_USB..."
    sudo lpadmin -p "$PRINTER_NAME" -v "$BK3_USB" $DRIVER_FLAG -E
fi
sudo cupsenable "$PRINTER_NAME" 2>/dev/null || true
sudo cupsaccept "$PRINTER_NAME" 2>/dev/null || true
sudo lpadmin -d "$PRINTER_NAME" 2>/dev/null || true
echo "  Printer '$PRINTER_NAME' configured (default queue)."

# ---- Create systemd service ----
echo "[7/7] Setting up systemd service..."

if [ -f "${INSTALL_DIR}/WebPrintService" ]; then
    EXEC="${INSTALL_DIR}/WebPrintService"
elif [ -f "${INSTALL_DIR}/PiPrintService" ]; then
    EXEC="${INSTALL_DIR}/PiPrintService"  # legacy fallback
else
    EXEC="${DOTNET_ROOT}/dotnet ${INSTALL_DIR}/WebPrintService.dll"
fi

sudo tee /etc/systemd/system/${SERVICE_NAME}.service > /dev/null << UNIT
[Unit]
Description=Web Print Service
After=network.target cups.service
Wants=cups.service

[Service]
Type=simple
ExecStart=${EXEC}
WorkingDirectory=${INSTALL_DIR}
Restart=always
RestartSec=5
User=${USER}
Environment=DOTNET_ROOT=${DOTNET_ROOT}
Environment=ASPNETCORE_URLS=http://0.0.0.0:${SERVICE_PORT}
Environment=DOTNET_ENVIRONMENT=Production

NoNewPrivileges=true

[Install]
WantedBy=multi-user.target
UNIT

sudo systemctl daemon-reload
sudo systemctl enable ${SERVICE_NAME}
sudo systemctl start ${SERVICE_NAME}

sleep 3

echo ""
if sudo systemctl is-active --quiet ${SERVICE_NAME}; then
    echo "============================================"
    echo "  Install Complete!"
    echo "============================================"
    echo "  Service URL:  http://$(hostname -I | awk '{print $1}'):${SERVICE_PORT}"
    echo "  Swagger:      http://$(hostname -I | awk '{print $1}'):${SERVICE_PORT}/swagger"
    echo "  Web Server:   ${WEB_URL}"
    echo "  Server ID:    ${SERVER_ID}"
    echo "  Printer:      ${PRINTER_NAME}"
    echo ""
    echo "  CUPS Admin:   http://localhost:631"
    echo ""
    echo "  Commands:"
    echo "    sudo systemctl status ${SERVICE_NAME}"
    echo "    sudo systemctl restart ${SERVICE_NAME}"
    echo "    sudo journalctl -u ${SERVICE_NAME} -f"
    echo "    lpstat -p ${PRINTER_NAME}"
    echo "    echo 'BK3 test' | lp -d ${PRINTER_NAME}"
    echo ""
    echo "  To update later, run this command again."
    echo "============================================"
else
    echo "============================================"
    echo "  WARNING: Service may not have started."
    echo "============================================"
    echo "  Check logs:"
    echo "    sudo journalctl -u ${SERVICE_NAME} -n 30 --no-pager"
    echo "============================================"
fi
