#!/usr/bin/env bash
# =============================================================================
# Shared helper for syncing a service subdirectory from this monorepo into
# its standalone GitHub distribution repo. Used by the per-service wrappers:
#
#   scripts/sync-nwgg-web-print-service.sh
#   scripts/sync-nwgg-temp-ticket-kiosk-service.sh
#   scripts/sync-nwgg-camera-service.sh
#   scripts/sync-nwgg-scale-reader-service.sh
#
# Each wrapper sets SOURCE_DIR + DIST_REPO and sources this file.
#
# What this does:
#   1. Snapshots <monorepo>/<SOURCE_DIR>/ at the current HEAD (clean — no
#      working-tree changes leak in).
#   2. Clones the standalone dist repo into a tempdir.
#   3. Replaces every file in the dist repo with the snapshot.
#   4. Commits with a message referencing the monorepo SHA.
#   5. Pushes to dist origin/master.
#
# The dist repo must already exist on GitHub (create it once, manually).
# If the repo is empty (no master branch yet), the script creates master
# from the first sync.
# =============================================================================

set -euo pipefail

if [[ -z "${SOURCE_DIR:-}" || -z "${DIST_REPO:-}" ]]; then
    echo "ERROR: SOURCE_DIR and DIST_REPO must be set by the wrapper." >&2
    exit 1
fi

DIST_BRANCH="${DIST_BRANCH:-master}"

# ---- Locate the monorepo root ----
MONO_ROOT="$(git rev-parse --show-toplevel 2>/dev/null || true)"
if [[ -z "$MONO_ROOT" ]]; then
    echo "ERROR: run this from inside the northwest-grain-growers monorepo." >&2
    exit 1
fi
cd "$MONO_ROOT"

if [[ ! -d "$SOURCE_DIR" ]]; then
    echo "ERROR: source directory '$SOURCE_DIR' not found in monorepo." >&2
    exit 1
fi

# ---- Refuse to sync if the working tree has unstaged changes in SOURCE_DIR ----
# Otherwise we'd silently publish someone's in-progress edits without their
# knowing the commit SHA the dist repo references doesn't actually include them.
if ! git diff --quiet -- "$SOURCE_DIR" || ! git diff --cached --quiet -- "$SOURCE_DIR"; then
    echo "ERROR: '$SOURCE_DIR' has uncommitted changes. Commit or stash first." >&2
    git status --short -- "$SOURCE_DIR" >&2
    exit 1
fi

MONO_SHA="$(git rev-parse --short HEAD)"
MONO_FULL_SHA="$(git rev-parse HEAD)"

echo "============================================"
echo "  Syncing $SOURCE_DIR"
echo "  → $DIST_REPO"
echo "  Monorepo commit: $MONO_SHA"
echo "============================================"

# ---- Snapshot the source from HEAD ----
SNAPSHOT=$(mktemp -d)
trap 'rm -rf "$SNAPSHOT" "$CLONE"' EXIT
git archive HEAD "$SOURCE_DIR" | tar -x -C "$SNAPSHOT"
# After this, files are at $SNAPSHOT/<SOURCE_DIR>/...

# ---- Clone the dist repo ----
CLONE=$(mktemp -d)
echo "Cloning $DIST_REPO..."
if ! git clone "https://github.com/${DIST_REPO}.git" "$CLONE" 2>&1 | tail -3; then
    echo "ERROR: failed to clone $DIST_REPO. Does the repo exist on GitHub?" >&2
    exit 1
fi
cd "$CLONE"

# Handle empty repo case (no commits yet → no branch exists)
if ! git rev-parse --verify "origin/$DIST_BRANCH" >/dev/null 2>&1; then
    echo "Dist repo is empty — initializing $DIST_BRANCH."
    git checkout -b "$DIST_BRANCH"
else
    git checkout "$DIST_BRANCH"
fi

# ---- Wipe everything except .git, copy in fresh snapshot ----
find . -mindepth 1 -maxdepth 1 ! -name '.git' -exec rm -rf {} +
# Snapshot has files under $SNAPSHOT/$SOURCE_DIR/... — flatten to the repo root.
shopt -s dotglob
cp -a "$SNAPSHOT/$SOURCE_DIR/." ./
shopt -u dotglob

# ---- Commit + push if anything changed ----
git add -A
if git diff --cached --quiet; then
    echo "No changes — dist repo already in sync with monorepo @ $MONO_SHA."
    exit 0
fi

git -c user.email="sync-bot@gtmichelli.dev" \
    -c user.name="NWGG sync" \
    commit -m "Sync from northwest-grain-growers @ ${MONO_SHA}

Source: ${SOURCE_DIR}
Monorepo commit: ${MONO_FULL_SHA}"

echo "Pushing to $DIST_REPO..."
git push origin "$DIST_BRANCH"

echo ""
echo "============================================"
echo "  Sync complete."
echo "  Monorepo @ $MONO_SHA → $DIST_REPO ($DIST_BRANCH)"
echo "============================================"
