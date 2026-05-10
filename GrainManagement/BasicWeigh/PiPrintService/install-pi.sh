#!/usr/bin/env bash
# install-pi.sh — install / upgrade the Grain Management Print Service on a
# Raspberry Pi (or any Debian-derived ARM/x64 host running systemd).
#
# What it does, idempotently:
#   1. Installs .NET 8 ASP.NET Core runtime via the Microsoft apt repo
#      (falls back to dotnet-install.sh when the repo lacks packages).
#   2. Installs CUPS + cups-bsd + cups-client.
#   3. Configures CUPS to listen on the LAN at 0.0.0.0:631 with @LOCAL
#      Allow rules — this is the "make CUPS available publicly" step.
#      Adds the service user to the lpadmin group.
#   4. Opens UFW for tcp/631 + tcp/5230 when UFW is enabled.
#   5. dotnet publishes the project (linux-arm64 by default).
#   6. Copies binaries to /opt/print-service, preserving webprintservice.db
#      across upgrades.
#   7. Writes /etc/systemd/system/print-service.service and starts it.
#
# Usage:
#   sudo ./install-pi.sh                     # default: user=pi, arch=arm64, port=5230
#   sudo ./install-pi.sh --user kiosk
#   sudo ./install-pi.sh --arch arm          # 32-bit Raspberry Pi OS
#   sudo ./install-pi.sh --port 5230
#
# Re-run after a git pull to upgrade — the SQLite DB survives the redeploy.

set -euo pipefail

SERVICE_USER="pi"
RUNTIME="linux-arm64"
PORT="5230"
INSTALL_DIR="/opt/print-service"
SERVICE_NAME="print-service"

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
PROJECT_FILE="$SCRIPT_DIR/PiPrintService.csproj"

if [[ ! -f "$PROJECT_FILE" ]]; then
    echo "Can't find PiPrintService.csproj at $PROJECT_FILE — run this from the PiPrintService folder." >&2
    exit 1
fi

echo "=== Print Service install ==="
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

# ── 2. CUPS ───────────────────────────────────────────────────────────
if ! command -v cupsd >/dev/null 2>&1; then
    echo "==> Installing CUPS…"
    apt-get install -y cups cups-bsd cups-client
else
    echo "==> CUPS already installed — skipping."
fi

# ── 3. CUPS — open to the LAN ─────────────────────────────────────────
echo "==> Configuring CUPS to accept LAN print jobs (publicly reachable on the LAN)…"

# Add the service user to lpadmin so they can manage printers via the web UI.
if id "$SERVICE_USER" >/dev/null 2>&1; then
    usermod -aG lpadmin "$SERVICE_USER" || true
fi

# cupsctl handles the cupsd.conf edit atomically. The three flags here are
# the canonical "share this CUPS server with the LAN" combo:
#   --remote-any      → Listen on 0.0.0.0:631 instead of just localhost
#   --remote-admin    → Allow admin pages to be reached from the LAN
#   --share-printers  → Browse-broadcast local printers to LAN peers
cupsctl --remote-any --remote-admin --share-printers

# Belt-and-braces: edit cupsd.conf in case cupsctl on this version doesn't
# touch all three Location blocks the way newer versions do. Idempotent.
CUPSD_CONF="/etc/cups/cupsd.conf"
if [[ -f "$CUPSD_CONF" ]] && ! grep -q "^# install-pi.sh applied @LOCAL grants$" "$CUPSD_CONF"; then
    cp -a "$CUPSD_CONF" "${CUPSD_CONF}.bak.$(date +%s)"
    python3 - "$CUPSD_CONF" <<'PY'
import re, sys, pathlib
path = pathlib.Path(sys.argv[1])
text = path.read_text()
# Ensure the three Location blocks (/, /admin, /admin/conf) allow @LOCAL.
def grant(block):
    # If the block already mentions @LOCAL we leave it alone.
    if "@LOCAL" in block: return block
    return re.sub(r"(Order [a-z,]+)", r"\1\n  Allow @LOCAL", block, count=1, flags=re.I)
text = re.sub(r"<Location />.*?</Location>",        lambda m: grant(m.group(0)), text, flags=re.S)
text = re.sub(r"<Location /admin>.*?</Location>",   lambda m: grant(m.group(0)), text, flags=re.S)
text = re.sub(r"<Location /admin/conf>.*?</Location>", lambda m: grant(m.group(0)), text, flags=re.S)
text += "\n# install-pi.sh applied @LOCAL grants\n"
path.write_text(text)
PY
fi

systemctl restart cups

# ── 4. UFW (open 631 + service port) ──────────────────────────────────
if command -v ufw >/dev/null 2>&1 && ufw status | grep -q "Status: active"; then
    echo "==> Opening UFW for CUPS (631/tcp) and the service ($PORT/tcp)…"
    ufw allow 631/tcp >/dev/null
    ufw allow "${PORT}/tcp" >/dev/null
fi

# ── 5. dotnet publish ─────────────────────────────────────────────────
echo "==> Publishing PiPrintService for $RUNTIME…"
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

# ── 6. Sync binaries (preserve SQLite) ────────────────────────────────
if systemctl is-active --quiet "$SERVICE_NAME"; then
    echo "==> Stopping existing $SERVICE_NAME for upgrade…"
    systemctl stop "$SERVICE_NAME"
fi

mkdir -p "$INSTALL_DIR"
PRESERVE=("webprintservice.db")
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
# The service user needs to be in the lp group so it can talk to CUPS.
usermod -aG lp "$SERVICE_USER" || true

# ── 7. systemd unit ───────────────────────────────────────────────────
DOTNET_BIN=$(command -v dotnet)
echo "==> Writing /etc/systemd/system/$SERVICE_NAME.service…"
cat <<EOF | tee "/etc/systemd/system/$SERVICE_NAME.service" >/dev/null
[Unit]
Description=Grain Management Print Service
After=network-online.target cups.service
Wants=network-online.target

[Service]
WorkingDirectory=$INSTALL_DIR
ExecStart=$DOTNET_BIN $INSTALL_DIR/PiPrintService.dll
Restart=always
RestartSec=5
User=$SERVICE_USER
Group=$SERVICE_USER
Environment=Print__Port=$PORT
Environment=ASPNETCORE_URLS=http://0.0.0.0:$PORT
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
SupplementaryGroups=lp lpadmin
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
    echo "✅ print-service is running."
    echo "   Swagger:        http://$IP:$PORT/swagger"
    echo "   CUPS admin:     http://$IP:631/  (log in as a user in the lpadmin group)"
    echo "   Logs:           sudo journalctl -u $SERVICE_NAME -f"
    echo "   Stop:           sudo systemctl stop $SERVICE_NAME"
    echo "   Disable:        sudo systemctl disable $SERVICE_NAME"
    echo
    echo "Add a printer via the CUPS web UI, then GET /api/printers will return it."
else
    echo
    echo "❌ Service failed to start. Last 30 log lines:"
    journalctl -u "$SERVICE_NAME" -n 30 --no-pager
    exit 1
fi
