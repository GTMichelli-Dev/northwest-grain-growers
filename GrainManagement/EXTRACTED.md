# GrainManagement extracted

The GrainManagement web app and the NWGG kiosk services have been
extracted from this monorepo into their own repository:

**https://github.com/GTMichelli-Dev/northwest-grain-management**

The new repo is the source of truth for:

- `GrainManagement/` — the web app
- `WebPrintService/`, `TempTicketKioskService/`, `CameraService/`,
  `ScaleReaderService/` — NWGG kiosk services
- `GrainManagement.As400Sync/`, `MultiScaleSimulator/`, `PictureUpsert/`
- `scripts/sync-nwgg-*.sh` — dist publish scripts for the Pi fleet

Only the legacy components remain in this folder:

- `BasicWeigh/` — the older `/scaleHub` stack
- `Windows_Application/` — legacy WinForms client
