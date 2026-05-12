# NWGG monorepo sync scripts

These scripts publish individual service subdirectories from this monorepo into their standalone GitHub distribution repos. Pi hosts in the field clone from the distribution repos, not from this monorepo, so deploys stay small and per-service.

## Usage

Each script is a one-shot sync. Run from anywhere inside the monorepo:

```bash
bash scripts/sync-nwgg-web-print-service.sh
bash scripts/sync-nwgg-temp-ticket-kiosk-service.sh
bash scripts/sync-nwgg-camera-service.sh         # scaffolding — see note below
bash scripts/sync-nwgg-scale-reader-service.sh   # scaffolding — see note below
```

Each script:

1. Refuses to run if the source subtree (`GrainManagement/<Service>/`) has uncommitted changes.
2. Snapshots the subtree at the monorepo's `HEAD`.
3. Clones the target dist repo, wipes its tree (except `.git`), copies the snapshot to the repo root.
4. Commits with a message referencing the monorepo SHA, then pushes to `master`.

The dist repos must already exist on GitHub. Create them once, manually, then the sync scripts handle the rest. The first sync into an empty repo creates `master` for you.

## Status

| Service | Sync ready? | Notes |
|---|---|---|
| `nwgg-web-print-service` | ✅ Full | install + deploy-to-pi + deploy-to-windows scripts adapted, README rewritten, BIXOLON BK3 driver-pack prerequisite documented. |
| `nwgg-temp-ticket-kiosk-service` | ✅ Full | install.sh restructured into `deploy/`, README rewritten. GPIO/UFW setup preserved. |
| `nwgg-camera-service` | ⚠️ Scaffolding | sync script in place, but the service's `install-pi.sh` / `install-windows.ps1` haven't been adapted to the clone-and-install pattern. Don't publish for fleet use until tested on a Pi. |
| `nwgg-scale-reader-service` | ⚠️ Scaffolding | same caveat. Also still on net9.0 (everything else is net8.0) — install script needs to install the 9.0 runtime instead of 8.0, or the project itself bumped down. |

## Shared helper

All four wrappers `source` `_sync-service-to-dist.sh`, which holds the cloning, snapshotting, and pushing logic. Per-service wrappers just set `SOURCE_DIR` and `DIST_REPO`.

If you ever need to add another service, copy one of the existing wrappers and change those two lines.
