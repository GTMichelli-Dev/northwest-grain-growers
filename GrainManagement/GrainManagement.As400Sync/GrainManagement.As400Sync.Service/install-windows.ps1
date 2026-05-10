<#
.SYNOPSIS
    Install / upgrade the Grain Management AS400 Sync Service on Windows.

.DESCRIPTION
    Idempotent Windows installer. What it does:
      1. Installs the .NET 8 ASP.NET Core runtime via winget (skipped when present).
      2. Checks for the IBM i Access ODBC Driver and warns (with the IBM
         download URL) if it's missing — the service can't talk to the
         AS/400 without it, but the driver isn't winget-installable.
      3. dotnet publishes the project for win-x64.
      4. Stops the existing service (if any), syncs binaries to the install
         directory (preserving appsettings.json across upgrades — that's
         where the SQL + ODBC connection strings, hub URL, and API key
         live), and re-registers the service via sc.exe with restart-on-failure.
      5. Opens a firewall rule for the service port (5080 by default) so
         the admin page can reach Swagger / /api/sync/info from another LAN host.
      6. Starts the service and prints the Swagger URL.

    Re-run after a git pull to upgrade — the script stops the service,
    republishes, copies the new binaries, and starts the service again.
    appsettings.json survives upgrades.

.PARAMETER InstallDir
    Where the published binaries live. Default: C:\Services\As400SyncService.

.PARAMETER ServiceName
    Windows service short name. Default: As400SyncService.

.PARAMETER Port
    Firewall port to open. Default: 5080. The service itself reads its bind
    URL from appsettings.json → Kestrel:Endpoints — this is only used to
    open the inbound firewall hole. If you change the appsettings port,
    re-run with -Port matching it.

.PARAMETER ServiceAccount
    Account the service runs under. Default: LocalSystem. Use a real domain
    account when the SQL connection string is set up for Windows auth.

.PARAMETER ServicePassword
    Password for ServiceAccount (only used when not LocalSystem).

.EXAMPLE
    .\install-windows.ps1
    .\install-windows.ps1 -Port 5180 -InstallDir D:\Services\As400SyncService
    .\install-windows.ps1 -ServiceAccount "DOMAIN\sync-svc" -ServicePassword "..."
#>

[CmdletBinding()]
param(
    [string] $InstallDir      = "C:\Services\As400SyncService",
    [string] $ServiceName     = "As400SyncService",
    [int]    $Port            = 5080,
    [string] $ServiceAccount  = "LocalSystem",
    [string] $ServicePassword = ""
)

$ErrorActionPreference = 'Stop'

# ── Elevation check ───────────────────────────────────────────────────
$currentUser = [Security.Principal.WindowsPrincipal]::new(
    [Security.Principal.WindowsIdentity]::GetCurrent())
if (-not $currentUser.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {
    Write-Error "This script needs to be run from an elevated (Administrator) PowerShell session."
    exit 1
}

# ── Locate the project ────────────────────────────────────────────────
$scriptDir   = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectFile = Join-Path $scriptDir "GrainManagement.As400Sync.csproj"
if (-not (Test-Path $projectFile)) {
    Write-Error "Can't find GrainManagement.As400Sync.csproj at $projectFile - run this from the GrainManagement.As400Sync.Service folder."
    exit 1
}

Write-Host "=== AS400 Sync Service install ===" -ForegroundColor Cyan
Write-Host "Project:        $projectFile"
Write-Host "Install dir:    $InstallDir"
Write-Host "Service name:   $ServiceName"
Write-Host "Firewall port:  $Port"
Write-Host "Service user:   $ServiceAccount"
Write-Host ""

function Test-Command([string]$Name) {
    $null = Get-Command $Name -ErrorAction SilentlyContinue
    return $?
}

function Refresh-EnvPath {
    $machine = [Environment]::GetEnvironmentVariable("Path", "Machine")
    $user    = [Environment]::GetEnvironmentVariable("Path", "User")
    $env:Path = ($machine, $user | Where-Object { $_ }) -join ";"
}

function Test-DotNet8 {
    if (-not (Test-Command "dotnet")) { return $false }
    $runtimes = & dotnet --list-runtimes 2>$null
    return ($runtimes -match "^Microsoft\.AspNetCore\.App 8\.").Count -gt 0
}

# ── 1. .NET 8 runtime ─────────────────────────────────────────────────
if (-not (Test-DotNet8)) {
    Write-Host "==> Installing .NET 8 ASP.NET Core runtime via winget..." -ForegroundColor Yellow
    if (-not (Test-Command "winget")) {
        Write-Error "winget is required. Install 'App Installer' from the Microsoft Store and re-run."
        exit 1
    }
    winget install --id Microsoft.DotNet.AspNetCore.8 --accept-source-agreements --accept-package-agreements --silent
    Refresh-EnvPath
    if (-not (Test-DotNet8)) {
        Write-Error ".NET 8 install reported success but dotnet --list-runtimes still doesn't see it. Open a new PowerShell and re-run."
        exit 1
    }
} else {
    Write-Host "==> .NET 8 runtime already installed - skipping." -ForegroundColor Green
}

# ── 2. IBM i Access ODBC Driver check ─────────────────────────────────
# The driver isn't winget-installable, so we can only check + warn. Look in
# both ODBC registry hives — 64-bit lives under HKLM:\SOFTWARE\ODBC\..., the
# 32-bit hive lives under HKLM:\SOFTWARE\WOW6432Node\ODBC\...
function Test-IbmIOdbcDriver {
    $hives = @(
        "HKLM:\SOFTWARE\ODBC\ODBCINST.INI",
        "HKLM:\SOFTWARE\WOW6432Node\ODBC\ODBCINST.INI"
    )
    foreach ($hive in $hives) {
        if (-not (Test-Path $hive)) { continue }
        $children = Get-ChildItem $hive -ErrorAction SilentlyContinue | Select-Object -ExpandProperty PSChildName
        if ($children -match "^IBM i Access ODBC Driver$") { return $true }
    }
    return $false
}

if (-not (Test-IbmIOdbcDriver)) {
    Write-Warning @"
IBM i Access ODBC Driver was not detected.
The AS400 sync service can't connect to Agvantage without it.
Download IBM i Access Client Solutions (free, IBM ID required) from:
   https://www.ibm.com/support/pages/ibm-i-access-client-solutions
Install the ODBC Driver component, then re-run this script (or just
restart the service afterward — the binary side is fine).
"@
} else {
    Write-Host "==> IBM i Access ODBC Driver detected." -ForegroundColor Green
}

# ── 3. dotnet publish ─────────────────────────────────────────────────
$runtime    = "win-x64"
$publishDir = Join-Path $scriptDir "bin\Release\$runtime-publish"

Write-Host "==> Publishing GrainManagement.As400Sync for $runtime..." -ForegroundColor Yellow
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }
& dotnet publish $projectFile -c Release -r $runtime --self-contained false -o $publishDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet publish failed (exit $LASTEXITCODE)."
    exit 1
}

# ── 4. Stop existing service + sync binaries ──────────────────────────
$existing = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($existing -and $existing.Status -eq 'Running') {
    Write-Host "==> Stopping existing $ServiceName for upgrade..." -ForegroundColor Yellow
    Stop-Service -Name $ServiceName -Force
    $deadline = (Get-Date).AddSeconds(20)
    do {
        Start-Sleep -Milliseconds 500
        $svc = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
    } while ($svc -and $svc.Status -ne 'Stopped' -and (Get-Date) -lt $deadline)
}

if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir | Out-Null
}

# Preserve appsettings.json (SQL + ODBC creds, HubUrl, ApiKey, Sync flags).
$preserve = @("appsettings.json")
$tempSave = Join-Path $env:TEMP "as400sync-preserve-$(Get-Random)"
New-Item -ItemType Directory -Path $tempSave | Out-Null
foreach ($f in $preserve) {
    $src = Join-Path $InstallDir $f
    if (Test-Path $src) { Copy-Item -Path $src -Destination (Join-Path $tempSave $f) -Force }
}

Write-Host "==> Copying publish output to $InstallDir..." -ForegroundColor Yellow
$rcArgs = @("$publishDir", "$InstallDir", "/MIR", "/NJH", "/NJS", "/NDL", "/NP")
$null = & robocopy @rcArgs
if ($LASTEXITCODE -ge 8) {
    Write-Error "robocopy failed (exit $LASTEXITCODE)."
    exit 1
}

foreach ($f in $preserve) {
    $src = Join-Path $tempSave $f
    if (Test-Path $src) { Copy-Item -Path $src -Destination (Join-Path $InstallDir $f) -Force }
}
Remove-Item -Recurse -Force $tempSave

# ── 5. Register service ───────────────────────────────────────────────
$exePath = Join-Path $InstallDir "GrainManagement.As400Sync.exe"
$binPath = "`"$exePath`""
if (-not (Test-Path $exePath)) {
    Write-Error "Publish output is missing GrainManagement.As400Sync.exe at $exePath."
    exit 1
}

if ($existing) {
    Write-Host "==> Updating existing service registration..." -ForegroundColor Yellow
    sc.exe config $ServiceName binPath= $binPath start= auto | Out-Null
    if ($ServiceAccount -ne "LocalSystem") {
        sc.exe config $ServiceName obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
} else {
    Write-Host "==> Creating service $ServiceName..." -ForegroundColor Yellow
    if ($ServiceAccount -eq "LocalSystem") {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management AS400 Sync Service" | Out-Null
    } else {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management AS400 Sync Service" obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Error "sc create failed (exit $LASTEXITCODE)."
        exit 1
    }
}

# Restart-on-failure: 5s, 5s, 5s, reset failure count after 1 day clean.
sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/5000/restart/5000 | Out-Null

# Firewall rule so the admin page on another host can reach /swagger + /api/sync/info.
if (Get-Command New-NetFirewallRule -ErrorAction SilentlyContinue) {
    if (-not (Get-NetFirewallRule -DisplayName "Grain Management AS400 Sync" -ErrorAction SilentlyContinue)) {
        New-NetFirewallRule -DisplayName "Grain Management AS400 Sync" `
            -Direction Inbound -Action Allow -Protocol TCP -LocalPort $Port | Out-Null
    }
}

# ── 6. Start + report ─────────────────────────────────────────────────
Write-Host "==> Starting $ServiceName..." -ForegroundColor Yellow
Start-Service -Name $ServiceName
Start-Sleep -Seconds 2

$svc = Get-Service -Name $ServiceName
if ($svc.Status -eq 'Running') {
    $ip = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias 'Ethernet*','Wi-Fi*' -ErrorAction SilentlyContinue | Where-Object { $_.IPAddress -notmatch '^169\.' } | Select-Object -First 1).IPAddress
    if (-not $ip) { $ip = "<this-host>" }
    Write-Host ""
    Write-Host "as400-sync-service is running." -ForegroundColor Green
    Write-Host "   Swagger:      http://${ip}:${Port}/swagger"
    Write-Host "   Info:         http://${ip}:${Port}/api/sync/info  (anonymous)"
    Write-Host "   Config:       $InstallDir\appsettings.json  (edit, then Restart-Service $ServiceName)"
    Write-Host "   Logs:         Get-EventLog -LogName Application -Source As400SyncService -Newest 50"
    Write-Host "   Restart:      Restart-Service $ServiceName"
    Write-Host "   Stop:         Stop-Service $ServiceName"
    Write-Host "   Uninstall:    Stop-Service $ServiceName; sc.exe delete $ServiceName"
    Write-Host ""
    Write-Host "Drive jobs from the GrainManagement web admin: /Admin/As400Sync"
} else {
    Write-Host ""
    Write-Error "Service failed to start. Status: $($svc.Status). Check the Application event log:"
    Write-Host "   Get-EventLog -LogName Application -Source As400SyncService -Newest 20"
    exit 1
}
