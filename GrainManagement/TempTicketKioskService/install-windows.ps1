<#
.SYNOPSIS
    Install / upgrade the Grain Management Temp Ticket Kiosk Service on Windows.

.DESCRIPTION
    Idempotent Windows installer. What it does:
      1. Installs the .NET 8 ASP.NET Core runtime via winget (skipped when present).
      2. dotnet publishes the project for win-x64.
      3. Stops the existing service (if any), syncs binaries to the install
         directory (preserving appsettings.json across upgrades), and
         re-registers the service via sc.exe with restart-on-failure.
      4. Opens a firewall rule for the kiosk port (5240 by default).
      5. Starts the service.

    Note: GPIO is not available on Windows. The service still hosts its
    Swagger surface so you can drive the rest of the temp-ticket flow
    end-to-end using `POST /api/test/press`. On a real kiosk you'd run
    this on a Raspberry Pi via install-pi.sh.

.PARAMETER InstallDir
    Where the published binaries live. Default: C:\Services\TempTicketKioskService.

.PARAMETER ServiceName
    Windows service short name. Default: TempTicketKioskService.

.PARAMETER Port
    Port the service listens on. Default: 5240.

.PARAMETER ServiceAccount
    Account the service runs under. Default: LocalSystem.

.PARAMETER ServicePassword
    Password for ServiceAccount (only used when not LocalSystem).

.EXAMPLE
    .\install-windows.ps1
    .\install-windows.ps1 -Port 5340 -InstallDir D:\Services\TempTicketKioskService
#>

[CmdletBinding()]
param(
    [string] $InstallDir      = "C:\Services\TempTicketKioskService",
    [string] $ServiceName     = "TempTicketKioskService",
    [int]    $Port            = 5240,
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
$projectFile = Join-Path $scriptDir "TempTicketKioskService.csproj"
if (-not (Test-Path $projectFile)) {
    Write-Error "Can't find TempTicketKioskService.csproj at $projectFile - run this from the TempTicketKioskService folder."
    exit 1
}

Write-Host "=== Temp Ticket Kiosk Service install ===" -ForegroundColor Cyan
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

# ── 2. dotnet publish ─────────────────────────────────────────────────
$runtime    = "win-x64"
$publishDir = Join-Path $scriptDir "bin\Release\$runtime-publish"

Write-Host "==> Publishing TempTicketKioskService for $runtime..." -ForegroundColor Yellow
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }
& dotnet publish $projectFile -c Release -r $runtime --self-contained false -o $publishDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet publish failed (exit $LASTEXITCODE)."
    exit 1
}

# ── 3. Stop existing service + sync binaries ──────────────────────────
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

$preserve = @("appsettings.json")
$tempSave = Join-Path $env:TEMP "temp-ticket-kiosk-preserve-$(Get-Random)"
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

# ── 4. Register service ───────────────────────────────────────────────
$exePath = Join-Path $InstallDir "TempTicketKioskService.exe"
$binPath = "`"$exePath`""
if (-not (Test-Path $exePath)) {
    Write-Error "Publish output is missing TempTicketKioskService.exe at $exePath."
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
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Temp Ticket Kiosk Service" | Out-Null
    } else {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Temp Ticket Kiosk Service" obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Error "sc create failed (exit $LASTEXITCODE)."
        exit 1
    }
}

sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/5000/restart/5000 | Out-Null

[Environment]::SetEnvironmentVariable("ASPNETCORE_URLS", "http://0.0.0.0:$Port", "Machine")

if (Get-Command New-NetFirewallRule -ErrorAction SilentlyContinue) {
    if (-not (Get-NetFirewallRule -DisplayName "Grain Management Temp Ticket Kiosk" -ErrorAction SilentlyContinue)) {
        New-NetFirewallRule -DisplayName "Grain Management Temp Ticket Kiosk" `
            -Direction Inbound -Action Allow -Protocol TCP -LocalPort $Port | Out-Null
    }
}

# ── 5. Start + report ─────────────────────────────────────────────────
Write-Host "==> Starting $ServiceName..." -ForegroundColor Yellow
Start-Service -Name $ServiceName
Start-Sleep -Seconds 2

$svc = Get-Service -Name $ServiceName
if ($svc.Status -eq 'Running') {
    $ip = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias 'Ethernet*','Wi-Fi*' -ErrorAction SilentlyContinue | Where-Object { $_.IPAddress -notmatch '^169\.' } | Select-Object -First 1).IPAddress
    if (-not $ip) { $ip = "<this-host>" }
    Write-Host ""
    Write-Host "temp-ticket-kiosk is running." -ForegroundColor Green
    Write-Host "   Swagger:      http://${ip}:${Port}/swagger"
    Write-Host "   Test press:   Invoke-RestMethod -Method Post http://${ip}:${Port}/api/test/press"
    Write-Host "   Config:       $InstallDir\appsettings.json  (edit, then Restart-Service $ServiceName)"
    Write-Host "   Logs:         Get-EventLog -LogName Application -Source TempTicketKioskService -Newest 50"
    Write-Host "   Restart:      Restart-Service $ServiceName"
    Write-Host "   Stop:         Stop-Service $ServiceName"
    Write-Host "   Uninstall:    Stop-Service $ServiceName; sc.exe delete $ServiceName"
} else {
    Write-Host ""
    Write-Error "Service failed to start. Status: $($svc.Status). Check the Application event log:"
    Write-Host "   Get-EventLog -LogName Application -Source TempTicketKioskService -Newest 20"
    exit 1
}
