#!/usr/bin/env bash
# Sync GrainManagement/ScaleReaderService/ → GTMichelli-Dev/nwgg-scale-reader-service.
# See scripts/_sync-service-to-dist.sh for the implementation.
#
# NOTE: ScaleReaderService currently targets net9.0 (unlike the other
# NWGG services on net8.0) and its install scripts haven't been adapted
# to the standalone-repo / clone-and-install pattern. Both issues need
# resolving before this distribution repo is usable for fleet deploys.
# Treat this sync script as scaffolding for now.

set -euo pipefail
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

SOURCE_DIR="GrainManagement/ScaleReaderService"
DIST_REPO="GTMichelli-Dev/nwgg-scale-reader-service"

source "$SCRIPT_DIR/_sync-service-to-dist.sh"
