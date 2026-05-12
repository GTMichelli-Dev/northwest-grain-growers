#!/bin/bash
# =============================================================================
# Web Print Service - Raspberry Pi Deploy Script
# =============================================================================
# Deploys the Web Print Service to a Raspberry Pi from a Windows/Mac/Linux
# development machine. Downloads the latest release from GitHub, installs
# .NET runtime if needed, configures CUPS, sets up a BIXOLON BK3 USB
# printer, and registers a systemd service.
#
# Usage:
#   ./deploy-to-pi.sh <pi-ip> <web-server-url> [options]
#
# Examples:
#   ./deploy-to-pi.sh 192.168.1.50 http://basicscale.scaledata.net
#   ./deploy-to-pi.sh 192.168.1.50 http://192.168.1.100:5110 --server-id 2 --printer-name Kiosk
#   ./deploy-to-pi.sh 192.168.1.50 http://basicscale.scaledata.net --port 5230 --arch arm64
#
# Requirements:
#   - SSH access to the Pi (key-based auth recommended)
#   - Pi must have internet access (for downloading .NET and packages)
# =============================================================================

set -e

# ---- Defaults ----
PI_USER="pi"
SERVICE_ID="default"
SERVER_ID="1"
PRINTER_NAME="Kiosk"
SERVICE_PORT="5230"
INSTALL_DIR="/opt/web-print-service"
SERVICE_NAME="web-print-service"
DOTNET_CHANNEL="8.0"
ARCH=""  # auto-detect if empty
GITHUB_REPO="GTMichelli-Dev/nwgg-web-print-service"
BRANCH="master"
BK3_PPD=""  # optional PPD on the Pi

# ---- Parse arguments ----
PI_IP="${1:?Usage: $0 <pi-ip> <web-server-url> [--server-id N] [--printer-name Kiosk] [--user pi]}"
WEB_URL="${2:?Usage: $0 <pi-ip> <web-server-url> [--server-id N] [--printer-name Kiosk] [--user pi]}"
shift 2

while [[ $# -gt 0 ]]; do
    case "$1" in
        --user)         PI_USER="$2"; shift 2 ;;
        --service-id)   SERVICE_ID="$2"; shift 2 ;;
        --server-id)    SERVER_ID="$2"; shift 2 ;;
        --printer-name) PRINTER_NAME="$2"; shift 2 ;;
        --port)         SERVICE_PORT="$2"; shift 2 ;;
        --arch)         ARCH="$2"; shift 2 ;;
        --branch)       BRANCH="$2"; shift 2 ;;
        --bk3-ppd)      BK3_PPD="$2"; shift 2 ;;
        *) echo "Unknown option: $1"; exit 1 ;;
    esac
done

SSH_TARGET="${PI_USER}@${PI_IP}"

echo "============================================"
echo "  Web Print Service - Deploy to Pi"
echo "============================================"
echo "  Pi:           ${SSH_TARGET}"
echo "  Web Server:   ${WEB_URL}"
echo "  Server ID:    ${SERVER_ID}"
echo "  Service ID:   ${SERVICE_ID}"
echo "  Printer Name: ${PRINTER_NAME}"
echo "  Port:         ${SERVICE_PORT}"
echo "  Install Dir:  ${INSTALL_DIR}"
echo "  Branch:       ${BRANCH}"
echo "============================================"
echo ""

# ---- Test SSH connection ----
echo "[1/8] Testing SSH connection..."
ssh -o ConnectTimeout=5 -o BatchMode=yes "${SSH_TARGET}" "echo 'SSH OK'" 2>/dev/null || {
    echo "ERROR: Cannot connect to ${SSH_TARGET}"
    echo "Make sure:"
    echo "  1. The Pi is powered on and connected to the network"
    echo "  2. SSH is enabled (sudo raspi-config > Interface Options > SSH)"
    echo "  3. You have SSH key access (ssh-copy-id ${SSH_TARGET})"
    exit 1
}

# ---- Detect architecture ----
if [ -z "$ARCH" ]; then
    echo "[2/8] Detecting Pi architecture..."
    PI_ARCH=$(ssh "${SSH_TARGET}" "uname -m")
    case "$PI_ARCH" in
        aarch64) ARCH="linux-arm64" ;;
        armv7l)  ARCH="linux-arm" ;;
        x86_64)  ARCH="linux-x64" ;;
        *)       echo "WARNING: Unknown arch '${PI_ARCH}', defaulting to linux-arm64"; ARCH="linux-arm64" ;;
    esac
    echo "  Detected: ${PI_ARCH} -> ${ARCH}"
else
    echo "[2/8] Using specified architecture: ${ARCH}"
fi

# ---- Install prerequisites on Pi ----
echo "[3/8] Installing prerequisites on Pi..."
ssh "${SSH_TARGET}" << 'PREREQ_EOF'
set -e

if ! command -v lpstat &> /dev/null; then
    echo "  Installing CUPS..."
    sudo apt-get update -qq
    sudo apt-get install -y -qq cups
    sudo usermod -aG lpadmin $USER
    echo "  CUPS installed."
else
    echo "  CUPS already installed."
fi

sudo systemctl enable cups
sudo systemctl start cups

sudo apt-get install -y -qq curl git unzip
PREREQ_EOF

# ---- Install .NET runtime on Pi ----
echo "[4/8] Installing .NET runtime on Pi..."
ssh "${SSH_TARGET}" << DOTNET_EOF
set -e

if command -v dotnet &> /dev/null; then
    DOTNET_VER=\$(dotnet --version 2>/dev/null || echo "unknown")
    echo "  .NET already installed: \${DOTNET_VER}"
else
    echo "  Installing .NET ${DOTNET_CHANNEL} ASP.NET Core runtime..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \\
        --channel ${DOTNET_CHANNEL} \\
        --runtime aspnetcore \\
        --install-dir /home/${PI_USER}/.dotnet

    if ! grep -q '.dotnet' /home/${PI_USER}/.bashrc; then
        echo 'export DOTNET_ROOT=/home/${PI_USER}/.dotnet' >> /home/${PI_USER}/.bashrc
        echo 'export PATH=\$PATH:\$DOTNET_ROOT' >> /home/${PI_USER}/.bashrc
    fi
    export DOTNET_ROOT=/home/${PI_USER}/.dotnet
    export PATH=\$PATH:\$DOTNET_ROOT
    echo "  .NET installed: \$(\$DOTNET_ROOT/dotnet --version)"
fi
DOTNET_EOF

# ---- Build and deploy the service ----
echo "[5/8] Building and deploying service..."

PUBLISH_DIR=$(mktemp -d)
echo "  Publishing for ${ARCH}..."

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

if [ -f "${PROJECT_DIR}/WebPrintService.csproj" ]; then
    echo "  Building from local source: ${PROJECT_DIR}"
    dotnet publish "${PROJECT_DIR}/WebPrintService.csproj" \
        -c Release \
        -r "${ARCH}" \
        --self-contained true \
        -o "${PUBLISH_DIR}" \
        -p:PublishSingleFile=false \
        -p:PublishTrimmed=false
else
    echo "  Cloning from GitHub: ${GITHUB_REPO}..."
    CLONE_DIR=$(mktemp -d)
    git clone --depth 1 --branch "${BRANCH}" "https://github.com/${GITHUB_REPO}.git" "${CLONE_DIR}"
    dotnet publish "${CLONE_DIR}/WebPrintService.csproj" \
        -c Release \
        -r "${ARCH}" \
        --self-contained true \
        -o "${PUBLISH_DIR}" \
        -p:PublishSingleFile=false \
        -p:PublishTrimmed=false
    rm -rf "${CLONE_DIR}"
fi

echo "  Uploading to Pi..."

ssh "${SSH_TARGET}" "sudo systemctl stop ${SERVICE_NAME} 2>/dev/null || true"

ssh "${SSH_TARGET}" "sudo mkdir -p ${INSTALL_DIR} && sudo chown ${PI_USER}:${PI_USER} ${INSTALL_DIR}"
rsync -az --delete "${PUBLISH_DIR}/" "${SSH_TARGET}:${INSTALL_DIR}/"

rm -rf "${PUBLISH_DIR}"

# ---- Configure the service ----
echo "[6/8] Configuring service..."

ssh "${SSH_TARGET}" << CONFIG_EOF
set -e

chmod +x ${INSTALL_DIR}/WebPrintService || chmod +x ${INSTALL_DIR}/PiPrintService || true

cd ${INSTALL_DIR}
if [ -f appsettings.json ]; then
    python3 - <<'PY'
import json
with open('appsettings.json', 'r') as f:
    config = json.load(f)
config.setdefault('Print', {})
config['Print']['ServiceId']  = '${SERVICE_ID}'
config['Print']['ServerId']   = int('${SERVER_ID}')
config['Print']['ServerUrls'] = ['${WEB_URL}']
config['Print']['Port']       = '${SERVICE_PORT}'
with open('appsettings.json', 'w') as f:
    json.dump(config, f, indent=2)
print('  appsettings.json updated.')
PY
fi
CONFIG_EOF

# ---- BIXOLON BK3 USB printer setup on Pi ----
echo "[7/8] Setting up BIXOLON BK3 USB printer '${PRINTER_NAME}' on Pi..."

ssh "${SSH_TARGET}" PRINTER_NAME="${PRINTER_NAME}" BK3_PPD="${BK3_PPD}" bash <<'PRINTER_EOF'
set -e

BK3_USB=""
if command -v lpinfo &> /dev/null; then
    BK3_USB=$(lpinfo -v 2>/dev/null | awk '/usb:\/\// && /BIXOLON/ {print $2; exit}')
    if [ -z "$BK3_USB" ]; then
        BK3_USB=$(lpinfo -v 2>/dev/null | awk '/usb:\/\// {print $2; exit}')
    fi
fi
[ -z "$BK3_USB" ] && BK3_USB="usb://BIXOLON/BK3-3"

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
        echo "  (Pass --bk3-ppd /path/on/pi/BK3.ppd for proper driver support.)"
    fi
fi

if lpstat -p "$PRINTER_NAME" &> /dev/null; then
    echo "  Printer '$PRINTER_NAME' exists — updating device URI..."
    sudo lpadmin -p "$PRINTER_NAME" -v "$BK3_USB" $DRIVER_FLAG -E
else
    echo "  Adding printer '$PRINTER_NAME' on $BK3_USB..."
    sudo lpadmin -p "$PRINTER_NAME" -v "$BK3_USB" $DRIVER_FLAG -E
fi
sudo cupsenable "$PRINTER_NAME" 2>/dev/null || true
sudo cupsaccept "$PRINTER_NAME" 2>/dev/null || true
sudo lpadmin -d "$PRINTER_NAME" 2>/dev/null || true
echo "  Printer '$PRINTER_NAME' configured."
PRINTER_EOF

# ---- Create systemd service ----
echo "[8/8] Setting up systemd service..."

ssh "${SSH_TARGET}" << SERVICE_EOF
set -e

if [ -f ${INSTALL_DIR}/WebPrintService ]; then
    EXEC_NAME="WebPrintService"
elif [ -f ${INSTALL_DIR}/PiPrintService ]; then
    EXEC_NAME="PiPrintService"
else
    EXEC_NAME="dotnet WebPrintService.dll"
fi

sudo tee /etc/systemd/system/${SERVICE_NAME}.service > /dev/null << UNIT
[Unit]
Description=Web Print Service
After=network.target cups.service
Wants=cups.service

[Service]
Type=notify
ExecStart=${INSTALL_DIR}/\${EXEC_NAME}
WorkingDirectory=${INSTALL_DIR}
Restart=always
RestartSec=5
User=${PI_USER}
Environment=DOTNET_ROOT=/home/${PI_USER}/.dotnet
Environment=ASPNETCORE_URLS=http://0.0.0.0:${SERVICE_PORT}
Environment=DOTNET_ENVIRONMENT=Production

NoNewPrivileges=true
ProtectSystem=strict
ReadWritePaths=${INSTALL_DIR}

[Install]
WantedBy=multi-user.target
UNIT

sudo systemctl daemon-reload
sudo systemctl enable ${SERVICE_NAME}
sudo systemctl start ${SERVICE_NAME}

sleep 3
if sudo systemctl is-active --quiet ${SERVICE_NAME}; then
    echo "  Service started successfully!"
else
    echo "  WARNING: Service may not have started. Check logs:"
    echo "  sudo journalctl -u ${SERVICE_NAME} -n 20 --no-pager"
fi
SERVICE_EOF

echo ""
echo "============================================"
echo "  Deployment Complete!"
echo "============================================"
echo "  Service URL:  http://${PI_IP}:${SERVICE_PORT}"
echo "  Swagger:      http://${PI_IP}:${SERVICE_PORT}/swagger"
echo "  Web Server:   ${WEB_URL}"
echo "  Server ID:    ${SERVER_ID}"
echo "  Printer:      ${PRINTER_NAME}"
echo ""
echo "  Useful commands (SSH to Pi):"
echo "    sudo systemctl status ${SERVICE_NAME}"
echo "    sudo systemctl restart ${SERVICE_NAME}"
echo "    sudo journalctl -u ${SERVICE_NAME} -f"
echo "    lpstat -p ${PRINTER_NAME}"
echo ""
echo "  CUPS admin: http://${PI_IP}:631"
echo "============================================"
