$ServiceName = "AgvantageTransfer"
$ExePath     = "C:\Agvantage\Service\AgvantageTransfer.exe"

if (-not (Test-Path $ExePath)) { throw "EXE not found at $ExePath" }

# Delete old, if any
$svc = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($svc) { sc.exe stop $ServiceName | Out-Null; sc.exe delete $ServiceName | Out-Null; Start-Sleep 2 }

# Create with safe quoting
& sc.exe @(
  'create', $ServiceName,
  ('binPath= "{0}"' -f $ExePath),
  'start= auto',
  ('obj= {0}' -f 'NT AUTHORITY\LocalService'),
  'password= ""'
)
if ($LASTEXITCODE -ne 0) { throw "sc create failed ($LASTEXITCODE)" }

& sc.exe config  AgvantageTransfer start= delayed-auto
& sc.exe qc      AgvantageTransfer
& sc.exe start   AgvantageTransfer
