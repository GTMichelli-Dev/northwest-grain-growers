#!/bin/bash
# =============================================================================
# Web Print Service - Raspberry Pi Deploy Script
# =============================================================================
# Deploys the Web Print Service to a Raspberry Pi from a Windows/Mac/Linux
# development machine. Downloads the latest release from GitHub, installs
# .NET runtime if needed, configures CUPS, and sets up as a systemd service.
#
# Usage:
#   ./deploy-to-pi.sh <pi-ip> <web-server-url> [options]
#
# Examples:
#   ./deploy-to-pi.sh 192.168.1.50 http://basicscale.scaledata.net
#   ./deploy-to-pi.sh 192.168.1.50 http://192.168.1.100:5110 --user pi --service-id office
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
SERVICE_PORT="5230"
INSTALL_DIR="/opt/web-print-service"
SERVICE_NAME="web-print-service"
DOTNET_CHANNEL="8.0"
ARCH=""  # auto-detect if empty
GITHUB_REPO="GTMichelli-Dev/web-print-service"
BRANCH="main"

# ---- Parse arguments ----
PI_IP="${1:?Usage: $0 <pi-ip> <web-server-url> [--user pi] [--service-id default] [--port 5230] [--arch arm64]}"
WEB_URL="${2:?Usage: $0 <pi-ip> <web-server-url> [--user pi] [--service-id default] [--port 5230] [--arch arm64]}"
shift 2

while [[ $# -gt 0 ]]; do
    case "$1" in
        --user)       PI_USER="$2"; shift 2 ;;
        --service-id) SERVICE_ID="$2"; shift 2 ;;
        --port)       SERVICE_PORT="$2"; shift 2 ;;
        --arch)       ARCH="$2"; shift 2 ;;
        --branch)     BRANCH="$2"; shift 2 ;;
        *) echo "Unknown option: $1"; exit 1 ;;
    esac
done

SSH_TARGET="${PI_USER}@${PI_IP}"

echo "============================================"
echo "  Web Print Service - Deploy to Pi"
echo "============================================"
echo "  Pi:          ${SSH_TARGET}"
echo "  Web Server:  ${WEB_URL}"
echo "  Service ID:  ${SERVICE_ID}"
echo "  Port:        ${SERVICE_PORT}"
echo "  Install Dir: ${INSTALL_DIR}"
echo "  Branch:      ${BRANCH}"
echo "============================================"
echo ""

# ---- Test SSH connection ----
echo "[1/7] Testing SSH connection..."
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
    echo "[2/7] Detecting Pi architecture..."
    PI_ARCH=$(ssh "${SSH_TARGET}" "uname -m")
    case "$PI_ARCH" in
        aarch64) ARCH="linux-arm64" ;;
        armv7l)  ARCH="linux-arm" ;;
        x86_64)  ARCH="linux-x64" ;;
        *)       echo "WARNING: Unknown arch '${PI_ARCH}', defaulting to linux-arm64"; ARCH="linux-arm64" ;;
    esac
    echo "  Detected: ${PI_ARCH} -> ${ARCH}"
else
    echo "[2/7] Using specified architecture: ${ARCH}"
fi

# ---- Install prerequisites on Pi ----
echo "[3/7] Installing prerequisites on Pi..."
ssh "${SSH_TARGET}" << 'PREREQ_EOF'
set -e

# Install CUPS if not already installed
if ! command -v lpstat &> /dev/null; then
    echo "  Installing CUPS..."
    sudo apt-get update -qq
    sudo apt-get install -y -qq cups
    sudo usermod -aG lpadmin $USER
    echo "  CUPS installed."
else
    echo "  CUPS already installed."
fi

# Ensure CUPS is running
sudo systemctl enable cups
sudo systemctl start cups

# Install required tools
sudo apt-get install -y -qq curl git unzip
PREREQ_EOF

# ---- Install .NET runtime on Pi ----
echo "[4/7] Installing .NET runtime on Pi..."
ssh "${SSH_TARGET}" << DOTNET_EOF
set -e

# Check if dotnet is already installed
if command -v dotnet &> /dev/null; then
    DOTNET_VER=\$(dotnet --version 2>/dev/null || echo "unknown")
    echo "  .NET already installed: \${DOTNET_VER}"
else
    echo "  Installing .NET ${DOTNET_CHANNEL} ASP.NET Core runtime..."
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \\
        --channel ${DOTNET_CHANNEL} \\
        --runtime aspnetcore \\
        --install-dir /home/${PI_USER}/.dotnet

    # Add to PATH permanently
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
echo "[5/7] Building and deploying service..."

# Build locally (cross-compile for the Pi's architecture)
PUBLISH_DIR=$(mktemp -d)
echo "  Publishing for ${ARCH}..."

# Check if we have the source locally
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
PROJECT_DIR="$(dirname "$SCRIPT_DIR")"

if [ -f "${PROJECT_DIR}/PiPrintService.csproj" ]; then
    echo "  Building from local source: ${PROJECT_DIR}"
    dotnet publish "${PROJECT_DIR}/PiPrintService.csproj" \
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
    dotnet publish "${CLONE_DIR}/PiPrintService.csproj" \
        -c Release \
        -r "${ARCH}" \
        --self-contained true \
        -o "${PUBLISH_DIR}" \
        -p:PublishSingleFile=false \
        -p:PublishTrimmed=false
    rm -rf "${CLONE_DIR}"
fi

echo "  Uploading to Pi..."

# Stop existing service if running
ssh "${SSH_TARGET}" "sudo systemctl stop ${SERVICE_NAME} 2>/dev/null || true"

# Create install directory and copy files
ssh "${SSH_TARGET}" "sudo mkdir -p ${INSTALL_DIR} && sudo chown ${PI_USER}:${PI_USER} ${INSTALL_DIR}"
rsync -az --delete "${PUBLISH_DIR}/" "${SSH_TARGET}:${INSTALL_DIR}/"

# Clean up local temp
rm -rf "${PUBLISH_DIR}"

# ---- Configure the service ----
echo "[6/7] Configuring service..."

ssh "${SSH_TARGET}" << CONFIG_EOF
set -e

# Set execute permission on the binary
chmod +x ${INSTALL_DIR}/PiPrintService || chmod +x ${INSTALL_DIR}/WebPrintService || true

# Update appsettings.json with the web server URL and port
cd ${INSTALL_DIR}
if [ -f appsettings.json ]; then
    # Use python3 to update JSON (available on most Pi images)
    python3 -c "
import json
with open('appsettings.json', 'r') as f:
    config = json.load(f)
config.setdefault('Print', {})
config['Print']['ServerUrl'] = '${WEB_URL}'
config['Print']['Port'] = '${SERVICE_PORT}'
with open('appsettings.json', 'w') as f:
    json.dump(config, f, indent=2)
print('  appsettings.json updated.')
"
fi
CONFIG_EOF

# ---- Create systemd service ----
echo "[7/7] Setting up systemd service..."

ssh "${SSH_TARGET}" << SERVICE_EOF
set -e

# Find the executable name
if [ -f ${INSTALL_DIR}/PiPrintService ]; then
    EXEC_NAME="PiPrintService"
elif [ -f ${INSTALL_DIR}/WebPrintService ]; then
    EXEC_NAME="WebPrintService"
else
    EXEC_NAME="dotnet PiPrintService.dll"
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

# Security hardening
NoNewPrivileges=true
ProtectSystem=strict
ReadWritePaths=${INSTALL_DIR}

[Install]
WantedBy=multi-user.target
UNIT

sudo systemctl daemon-reload
sudo systemctl enable ${SERVICE_NAME}
sudo systemctl start ${SERVICE_NAME}

# Wait for startup
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
echo "  Service ID:   ${SERVICE_ID}"
echo ""
echo "  Useful commands (SSH to Pi):"
echo "    sudo systemctl status ${SERVICE_NAME}"
echo "    sudo systemctl restart ${SERVICE_NAME}"
echo "    sudo journalctl -u ${SERVICE_NAME} -f"
echo ""
echo "  Configure printers via CUPS:"
echo "    http://${PI_IP}:631"
echo ""
echo "  Update service ID via API:"
echo "    curl -X PUT http://${PI_IP}:${SERVICE_PORT}/api/settings \\"
echo "      -H 'Content-Type: application/json' \\"
echo "      -d '{\"serviceId\": \"${SERVICE_ID}\", \"serverUrl\": \"${WEB_URL}\"}'"
echo "============================================"
