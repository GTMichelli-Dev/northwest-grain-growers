#!/usr/bin/env bash
# Sync GrainManagement/CameraService/ → GTMichelli-Dev/nwgg-camera-service.
# See scripts/_sync-service-to-dist.sh for the implementation.
#
# NOTE: the CameraService install scripts and README have not yet been
# adapted to the standalone-repo / clone-and-install pattern used by the
# other NWGG services. Until that work lands, syncing this repo will
# publish the in-repo install-pi.sh / install-windows.ps1 as-is — they
# assume the source is already on disk at the project root. Treat this
# sync script as scaffolding; don't expect a clean one-liner install
# until the service has been adapted and field-tested on a Pi.

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SOURCE_DIR="GrainManagement/CameraService"
DIST_REPO="GTMichelli-Dev/nwgg-camera-service"

source "$SCRIPT_DIR/_sync-service-to-dist.sh"
