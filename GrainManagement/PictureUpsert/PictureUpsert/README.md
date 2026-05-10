# Picture Upsert Service

A small ASP.NET Core 8 worker + Web API that lives alongside the GrainManagement web app. It watches a local folder of ticket images and pushes each one to a remote ingestion API (e.g. Central HQ). If the remote is unreachable, the file stays queued in SQLite and resumes automatically when the remote comes back — nothing is dropped.

## What runs where

```
[CameraService(s)]                  [GrainManagement Web]            [PictureUpsert]                 [Central API]
      ┃                                    ┃                              ┃                                 ┃
      ┗ POST /api/ticket/{n}/image ──────▶ saves to TicketImages.PhysicalPath
                                           (= same folder PictureUpsert watches)
                                                                          ┣── FileSystemWatcher picks it up
                                                                          ┗── HTTP POST /api/picture-upsert/ingest ─▶
                                                                                (retries forever until 2xx)
```

Run PictureUpsert on the **same machine as the web app** so the local file path is reachable.

## Configuration

Settings live in `pictureupsert.db` (SQLite, next to the binary). Edit via Swagger (`PUT /api/settings`) or directly with any SQLite tool. Defaults:

| Field             | Default                                    |
|-------------------|--------------------------------------------|
| LocalFolder       | `C:\Images`                                |
| RemoteBaseUrl     | `http://localhost:51791`                   |
| RemoteUploadPath  | `/api/picture-upsert/ingest`               |
| AuthToken         | (empty — set when the remote requires auth) |
| DeleteAfterUpload | `false`                                    |
| RetryDelaySeconds | `30`                                       |
| Filter            | `*.jpg;*.jpeg;*.png`                       |

## Endpoints

- `GET /api/status` — live operational status (consumed by the web's Camera page).
- `GET /api/health` — flat 200/OK heartbeat.
- `GET /api/settings` / `PUT /api/settings` — read/write the SQLite config.
- `GET /api/queue?status=Pending|Failed|Sent&take=100` — inspect the queue.
- `POST /api/queue/retry-failed` — reset all `Failed` rows back to `Pending`.
- Swagger UI: `/swagger`.

## Install as a Windows Service

```bash
dotnet publish -c Release -o C:\Services\PictureUpsert
sc create "PictureUpsert" binPath="C:\Services\PictureUpsert\PictureUpsert.exe"
sc start PictureUpsert
```
