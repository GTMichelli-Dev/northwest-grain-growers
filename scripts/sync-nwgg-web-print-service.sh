#!/usr/bin/env bash
# Sync GrainManagement/WebPrintService/ → GTMichelli-Dev/nwgg-web-print-service.
# See scripts/_sync-service-to-dist.sh for the implementation.

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SOURCE_DIR="GrainManagement/WebPrintService"
DIST_REPO="GTMichelli-Dev/nwgg-web-print-service"

source "$SCRIPT_DIR/_sync-service-to-dist.sh"
