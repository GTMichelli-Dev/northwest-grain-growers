#!/usr/bin/env bash
# Sync GrainManagement/TempTicketKioskService/ → GTMichelli-Dev/nwgg-temp-ticket-kiosk-service.
# See scripts/_sync-service-to-dist.sh for the implementation.

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SOURCE_DIR="GrainManagement/TempTicketKioskService"
DIST_REPO="GTMichelli-Dev/nwgg-temp-ticket-kiosk-service"

source "$SCRIPT_DIR/_sync-service-to-dist.sh"
