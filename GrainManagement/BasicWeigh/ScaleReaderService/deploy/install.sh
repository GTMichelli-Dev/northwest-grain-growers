#!/bin/bash
# =============================================================================
# Scale Reader Service - Self-Install Script for Raspberry Pi / Linux
# =============================================================================
# Run directly on the target machine:
#
#   git clone https://github.com/GTMichelli-Dev/scale-reader-service.git /tmp/srs
#   bash /tmp/srs/deploy/install.sh <web-server-url>
#   rm -rf /tmp/srs
#
# Examples:
#   git clone https://github.com/GTMichelli-Dev/scale-reader-service.git /tmp/srs
#   bash /tmp/srs/deploy/install.sh http://basicscale.scaledata.net
#   bash /tmp/srs/deploy/install.sh http://basicscale.scaledata.net --service-id plant-1 --port 5220
#   rm -rf /tmp/srs
#
# To update an existing install, run the same command again — it will
# stop the service, update files, preserve the database, and restart.
# =============================================================================

set -e

# ---- Defaults ----
SERVICE_ID="default"
SERVICE_PORT="5220"
INSTALL_DIR="/opt/scale-reader-service"
SERVICE_NAME="scale-reader-service"
DOTNET_CHANNEL="8.0"
GITHUB_REPO="GTMichelli-Dev/scale-reader-service"
BRANCH="master"
WEB_URL=""

# ---- Parse arguments ----
while [[ $# -gt 0 ]]; do
    case "$1" in
        --service-id) SERVICE_ID="$2"; shift 2 ;;
        --port)       SERVICE_PORT="$2"; shift 2 ;;
        --branch)     BRANCH="$2"; shift 2 ;;
        --install-dir) INSTALL_DIR="$2"; shift 2 ;;
        --help|-h)
            echo "Usage: install.sh <web-server-url> [options]"
            echo ""
            echo "  <web-server-url>       Required. URL of the BasicWeigh web server"
            echo "                         Example: http://basicscale.scaledata.net"
            echo ""
            echo "Options:"
            echo "  --service-id <id>      Unique ID for this service (default: default)"
            echo "  --port <port>          API port (default: 5220)"
            echo "  --branch <branch>      Git branch to install (default: master)"
            echo "  --install-dir <path>   Install location (default: /opt/scale-reader-service)"
            echo "  --help                 Show this help"
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
    echo "Usage:"
    echo "  git clone https://github.com/${GITHUB_REPO}.git /tmp/srs"
    echo "  bash /tmp/srs/deploy/install.sh http://your-server:5110"
    echo ""
    echo "Run with --help for all options."
    exit 1
fi

echo ""
echo "============================================"
echo "  Scale Reader Service - Install"
echo "============================================"
echo "  Web Server:   ${WEB_URL}"
echo "  Service ID:   ${SERVICE_ID}"
echo "  Port:         ${SERVICE_PORT}"
echo "  Install Dir:  ${INSTALL_DIR}"
echo "  Branch:       ${BRANCH}"
echo "============================================"
echo ""

# ---- Detect architecture ----
echo "[1/5] Detecting system..."
ARCH=$(uname -m)
case "$ARCH" in
    aarch64) RID="linux-arm64" ;;
    armv7l)  RID="linux-arm" ;;
    x86_64)  RID="linux-x64" ;;
    *)       echo "WARNING: Unknown arch '${ARCH}', trying linux-x64"; RID="linux-x64" ;;
esac
echo "  OS: $(uname -s) $(uname -r)"
echo "  Architecture: ${ARCH} (${RID})"

# ---- Install .NET 8 ----
echo "[2/5] Installing .NET runtime..."
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
    sudo apt-get update -qq 2>/dev/null || true
    sudo apt-get install -y -qq curl libicu-dev 2>/dev/null || true
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin \
        --channel ${DOTNET_CHANNEL} \
        --runtime aspnetcore \
        --install-dir "$DOTNET_ROOT"
    echo "  .NET installed: $($DOTNET_ROOT/dotnet --version)"
fi

# Ensure dotnet is on PATH
export PATH="$DOTNET_ROOT:$PATH"
export DOTNET_ROOT

# Add to .bashrc if not already there
if ! grep -q 'DOTNET_ROOT' "$HOME/.bashrc" 2>/dev/null; then
    echo "" >> "$HOME/.bashrc"
    echo "# .NET" >> "$HOME/.bashrc"
    echo "export DOTNET_ROOT=$DOTNET_ROOT" >> "$HOME/.bashrc"
    echo 'export PATH=$DOTNET_ROOT:$PATH' >> "$HOME/.bashrc"
    echo "  Added .NET to PATH in .bashrc"
fi

# ---- Download and Build ----
echo "[3/5] Downloading and building Scale Reader Service..."

# Stop existing service if running
sudo systemctl stop ${SERVICE_NAME} 2>/dev/null || true

# Create install directory
sudo mkdir -p "${INSTALL_DIR}"
sudo chown "$USER:$USER" "${INSTALL_DIR}"

# Backup existing database
DB_BACKUP=""
if [ -f "${INSTALL_DIR}/scalereaderservice.db" ]; then
    DB_BACKUP="/tmp/scalereaderservice-db-backup.db"
    cp "${INSTALL_DIR}/scalereaderservice.db" "$DB_BACKUP"
    echo "  Backed up existing database."
fi

# Clone and build
CLONE_DIR=$(mktemp -d)
echo "  Cloning from GitHub: ${GITHUB_REPO} (${BRANCH})..."
sudo apt-get install -y -qq git 2>/dev/null || true
git clone --depth 1 --branch "${BRANCH}" "https://github.com/${GITHUB_REPO}.git" "${CLONE_DIR}"

# Check if SDK is available
HAS_SDK=false
if dotnet --list-sdks 2>/dev/null | grep -q "8\."; then
    HAS_SDK=true
fi

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
dotnet publish "${CLONE_DIR}/ScaleReaderService.csproj" \
    -c Release \
    -r "${RID}" \
    --self-contained true \
    -o "${INSTALL_DIR}" \
    -p:PublishSingleFile=false \
    -p:PublishTrimmed=false

rm -rf "${CLONE_DIR}"

# Restore database if it existed
if [ -n "$DB_BACKUP" ] && [ -f "$DB_BACKUP" ]; then
    cp "$DB_BACKUP" "${INSTALL_DIR}/scalereaderservice.db"
    rm "$DB_BACKUP"
    echo "  Restored existing database."
fi

# Set execute permission
chmod +x "${INSTALL_DIR}/ScaleReaderService" 2>/dev/null || true

# ---- Configure ----
echo "[4/5] Configuring..."

# Update appsettings.json
if [ -f "${INSTALL_DIR}/appsettings.json" ] && command -v python3 &> /dev/null; then
    python3 -c "
import json
with open('${INSTALL_DIR}/appsettings.json', 'r') as f:
    config = json.load(f)
config.setdefault('Scale', {})
config['Scale']['ServerUrl'] = '${WEB_URL}'
config['Scale']['Port'] = '${SERVICE_PORT}'
with open('${INSTALL_DIR}/appsettings.json', 'w') as f:
    json.dump(config, f, indent=2)
"
    echo "  Updated appsettings.json"
fi

# ---- Create systemd service ----
echo "[5/5] Setting up systemd service..."

# Find executable
if [ -f "${INSTALL_DIR}/ScaleReaderService" ]; then
    EXEC="${INSTALL_DIR}/ScaleReaderService"
else
    EXEC="${DOTNET_ROOT}/dotnet ${INSTALL_DIR}/ScaleReaderService.dll"
fi

sudo tee /etc/systemd/system/${SERVICE_NAME}.service > /dev/null << UNIT
[Unit]
Description=Scale Reader Service
After=network.target

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

# Wait for startup
sleep 3

echo ""
if sudo systemctl is-active --quiet ${SERVICE_NAME}; then
    echo "============================================"
    echo "  Install Complete!"
    echo "============================================"
    echo "  Service URL:  http://$(hostname -I | awk '{print $1}'):${SERVICE_PORT}"
    echo "  Swagger:      http://$(hostname -I | awk '{print $1}'):${SERVICE_PORT}/swagger"
    echo "  Web Server:   ${WEB_URL}"
    echo "  Service ID:   ${SERVICE_ID}"
    echo ""
    echo "  Commands:"
    echo "    sudo systemctl status ${SERVICE_NAME}"
    echo "    sudo systemctl restart ${SERVICE_NAME}"
    echo "    sudo journalctl -u ${SERVICE_NAME} -f"
    echo ""
    echo "  Configure scales via Swagger:"
    echo "    http://$(hostname -I | awk '{print $1}'):${SERVICE_PORT}/swagger"
    echo ""
    echo "  Update service ID:"
    echo "    curl -X PUT http://localhost:${SERVICE_PORT}/api/settings \\"
    echo "      -H 'Content-Type: application/json' \\"
    echo "      -d '{\"serviceId\": \"${SERVICE_ID}\", \"serverUrl\": \"${WEB_URL}\"}'"
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
