<#
.SYNOPSIS
    Install / upgrade the Grain Management Camera Service on Windows.

.DESCRIPTION
    Idempotent installer that mirrors install-pi.sh for Windows:
      1. Installs the .NET 8 ASP.NET Core runtime via winget (skipped when already present).
      2. Installs ffmpeg via winget — Gyan.FFmpeg (skipped when already on PATH).
      3. dotnet publishes this project for win-x64.
      4. Stops the existing Windows service (if any), syncs binaries to the
         install directory (preserving camera-service.db + camera-snapshot.json
         across upgrades), and re-registers the service via sc.exe.
      5. (Re)starts the service and prints the Swagger URL.

    Re-run after a git pull to upgrade — the script stops the service,
    republishes, copies the new binaries, and starts the service again.
    The SQLite settings DB and brand snapshot survive upgrades.

.PARAMETER InstallDir
    Where the published binaries live. Default: C:\Services\CameraService.

.PARAMETER ServiceName
    Windows service short name. Default: CameraCaptureService.

.PARAMETER Port
    Port the service listens on. Default: 5210.

.PARAMETER ServiceAccount
    Account the service runs under. Default: LocalSystem.
    Pass DOMAIN\user (and -ServicePassword) if you need a specific account.

.PARAMETER ServicePassword
    Password for ServiceAccount (only used when ServiceAccount is not LocalSystem).

.EXAMPLE
    # Run from an elevated PowerShell session, from the CameraService folder:
    .\install-windows.ps1

.EXAMPLE
    .\install-windows.ps1 -Port 5310 -InstallDir D:\Services\CameraService
#>

[CmdletBinding()]
param(
    [string] $InstallDir      = "C:\Services\CameraService",
    [string] $ServiceName     = "CameraCaptureService",
    [int]    $Port            = 5210,
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
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectFile = Join-Path $scriptDir "CameraService.csproj"
if (-not (Test-Path $projectFile)) {
    Write-Error "Can't find CameraService.csproj at $projectFile - run this from the CameraService folder."
    exit 1
}

Write-Host "=== Camera Service install ===" -ForegroundColor Cyan
Write-Host "Project:        $projectFile"
Write-Host "Install dir:    $InstallDir"
Write-Host "Service name:   $ServiceName"
Write-Host "Service port:   $Port"
Write-Host "Service user:   $ServiceAccount"
Write-Host ""

function Test-Command([string]$Name) {
    $null = Get-Command $Name -ErrorAction SilentlyContinue
    return $?
}

function Refresh-EnvPath {
    # winget / installer scripts modify the machine PATH but this session's
    # PATH is a snapshot from when PowerShell launched. Merge in the latest
    # machine + user PATH so freshly-installed CLIs (dotnet, ffmpeg) become
    # callable in this same session without restarting PowerShell.
    $machine = [Environment]::GetEnvironmentVariable("Path", "Machine")
    $user    = [Environment]::GetEnvironmentVariable("Path", "User")
    $env:Path = ($machine, $user | Where-Object { $_ }) -join ";"
}

# ── 1. .NET 8 runtime ─────────────────────────────────────────────────
function Test-DotNet8 {
    if (-not (Test-Command "dotnet")) { return $false }
    $runtimes = & dotnet --list-runtimes 2>$null
    return ($runtimes -match "^Microsoft\.AspNetCore\.App 8\." ).Count -gt 0
}

if (-not (Test-DotNet8)) {
    Write-Host "==> Installing .NET 8 ASP.NET Core runtime via winget..." -ForegroundColor Yellow
    if (-not (Test-Command "winget")) {
        Write-Error @"
winget is required to auto-install .NET 8. Install 'App Installer' from the
Microsoft Store (https://aka.ms/getwinget), or install the .NET 8 ASP.NET Core
Runtime manually from https://dotnet.microsoft.com/download/dotnet/8.0 and
re-run this script.
"@
        exit 1
    }
    winget install --id Microsoft.DotNet.AspNetCore.8 --accept-source-agreements --accept-package-agreements --silent
    Refresh-EnvPath
    if (-not (Test-DotNet8)) {
        Write-Error ".NET 8 install reported success but dotnet --list-runtimes still doesn't see it. Open a new PowerShell session and re-run."
        exit 1
    }
} else {
    Write-Host "==> .NET 8 runtime already installed - skipping." -ForegroundColor Green
}

# ── 2. ffmpeg + Gyan build ────────────────────────────────────────────
if (-not (Test-Command "ffmpeg")) {
    Write-Host "==> Installing ffmpeg (Gyan.FFmpeg) via winget..." -ForegroundColor Yellow
    if (-not (Test-Command "winget")) {
        Write-Warning "winget unavailable - install ffmpeg manually from https://www.gyan.dev/ffmpeg/builds/ and add bin\\ to PATH, then re-run."
        exit 1
    }
    winget install --id Gyan.FFmpeg --accept-source-agreements --accept-package-agreements --silent
    Refresh-EnvPath
    if (-not (Test-Command "ffmpeg")) {
        Write-Warning "ffmpeg installer reported success but is not on PATH yet. Open a new PowerShell after this script finishes; the next service restart will pick it up automatically."
    }
} else {
    Write-Host "==> ffmpeg already installed - skipping." -ForegroundColor Green
}

# ── 3. dotnet publish ─────────────────────────────────────────────────
$runtime    = "win-x64"
$publishDir = Join-Path $scriptDir "bin\Release\$runtime-publish"

Write-Host "==> Publishing CameraService for $runtime..." -ForegroundColor Yellow
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }

& dotnet publish $projectFile `
    -c Release `
    -r $runtime `
    --self-contained false `
    -o $publishDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet publish failed (exit $LASTEXITCODE)."
    exit 1
}

# ── 4. Stop existing service + sync binaries ──────────────────────────
$existing = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($existing) {
    if ($existing.Status -eq 'Running') {
        Write-Host "==> Stopping existing $ServiceName service for upgrade..." -ForegroundColor Yellow
        Stop-Service -Name $ServiceName -Force
        # sc.exe stop is async — wait until the process actually exits so the
        # exe isn't locked when we try to overwrite it.
        $deadline = (Get-Date).AddSeconds(20)
        do {
            Start-Sleep -Milliseconds 500
            $svc = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
        } while ($svc -and $svc.Status -ne 'Stopped' -and (Get-Date) -lt $deadline)
    }
}

if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir | Out-Null
}

# Preserve operator-set settings + cached brand snapshot across upgrades.
$preserve = @("camera-service.db", "camera-snapshot.json")
$tempSave = Join-Path $env:TEMP "camera-service-preserve-$(Get-Random)"
New-Item -ItemType Directory -Path $tempSave | Out-Null
foreach ($f in $preserve) {
    $src = Join-Path $InstallDir $f
    if (Test-Path $src) { Copy-Item -Path $src -Destination (Join-Path $tempSave $f) -Force }
}

Write-Host "==> Copying publish output to $InstallDir..." -ForegroundColor Yellow
# robocopy /MIR = mirror (delete extras), /NJH /NJS = quiet summary, exit
# codes 0-7 are "success" / "files copied" — only 8+ are errors.
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
$exePath  = Join-Path $InstallDir "CameraService.exe"
$binPath  = "`"$exePath`""

if (-not (Test-Path $exePath)) {
    Write-Error "Publish output is missing CameraService.exe at $exePath."
    exit 1
}

if ($existing) {
    Write-Host "==> Updating existing service registration..." -ForegroundColor Yellow
    # sc.exe config — the trailing space after each "key=" is required.
    sc.exe config $ServiceName binPath= $binPath start= auto | Out-Null
    if ($ServiceAccount -ne "LocalSystem") {
        sc.exe config $ServiceName obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
} else {
    Write-Host "==> Creating service $ServiceName..." -ForegroundColor Yellow
    if ($ServiceAccount -eq "LocalSystem") {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Camera Service" | Out-Null
    } else {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Camera Service" obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Error "sc create failed (exit $LASTEXITCODE)."
        exit 1
    }
}

# Restart-on-failure: restart after 5s on the 1st/2nd/3rd failure, reset
# the failure count after 1 day clean. Mirrors `Restart=always RestartSec=5`
# from install-pi.sh's systemd unit.
sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/5000/restart/5000 | Out-Null

# ASPNETCORE_URLS — set as a per-service environment variable so the
# service binds to $Port without us having to edit appsettings.json.
[Environment]::SetEnvironmentVariable("ASPNETCORE_URLS", "http://0.0.0.0:$Port", "Machine")

# ── 6. Start + report ─────────────────────────────────────────────────
Write-Host "==> Starting $ServiceName..." -ForegroundColor Yellow
Start-Service -Name $ServiceName
Start-Sleep -Seconds 2

$svc = Get-Service -Name $ServiceName
if ($svc.Status -eq 'Running') {
    $ip = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias 'Ethernet*','Wi-Fi*' -ErrorAction SilentlyContinue | Where-Object { $_.IPAddress -notmatch '^169\.' } | Select-Object -First 1).IPAddress
    if (-not $ip) { $ip = "<this-host>" }
    Write-Host ""
    Write-Host "Camera service is running on http://localhost:$Port" -ForegroundColor Green
    Write-Host "   Swagger UI:  http://${ip}:${Port}/swagger"
    Write-Host "   Logs:        Get-EventLog -LogName Application -Source CameraService -Newest 50"
    Write-Host "   Stop:        Stop-Service $ServiceName"
    Write-Host "   Uninstall:   Stop-Service $ServiceName; sc.exe delete $ServiceName"
} else {
    Write-Host ""
    Write-Error "Service failed to start. Status: $($svc.Status). Check the Application event log:"
    Write-Host "   Get-EventLog -LogName Application -Source CameraService -Newest 20"
    exit 1
}
