# =============================================================================
# Web Print Service - Windows Deploy Script
# =============================================================================
# Installs the Web Print Service as a Windows Service on the local or
# remote machine. Downloads the latest release from GitHub, installs
# .NET runtime if needed, and registers as a Windows Service.
#
# Usage (local):
#   .\deploy-to-windows.ps1 -WebServerUrl "http://basicscale.scaledata.net"
#
# Usage (with options):
#   .\deploy-to-windows.ps1 -WebServerUrl "http://192.168.1.100:5110" `
#       -ServiceId "office" -Port 5230 -InstallDir "C:\Services\WebPrintService"
#
# Requirements:
#   - Windows 10/11 or Windows Server 2016+
#   - Run as Administrator
#   - Internet access (for downloading .NET if needed)
# =============================================================================

param(
    [Parameter(Mandatory=$true)]
    [string]$WebServerUrl,

    [string]$ServiceId = "default",
    [int]$Port = 5230,
    [string]$InstallDir = "C:\Services\WebPrintService",
    [string]$ServiceName = "WebPrintService",
    [string]$ServiceDisplayName = "Web Print Service",
    [string]$GitHubRepo = "GTMichelli-Dev/web-print-service",
    [string]$Branch = "main"
)

$ErrorActionPreference = "Stop"

# ---- Check admin privileges ----
$isAdmin = ([Security.Principal.WindowsPrincipal] [Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)
if (-not $isAdmin) {
    Write-Host "ERROR: This script must be run as Administrator." -ForegroundColor Red
    Write-Host "Right-click PowerShell and select 'Run as Administrator', then try again."
    exit 1
}

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Web Print Service - Windows Deploy" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  Web Server:   $WebServerUrl"
Write-Host "  Service ID:   $ServiceId"
Write-Host "  Port:         $Port"
Write-Host "  Install Dir:  $InstallDir"
Write-Host "  Service Name: $ServiceName"
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ---- Check .NET Runtime ----
Write-Host "[1/5] Checking .NET runtime..." -ForegroundColor Yellow

$dotnetInstalled = $false
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($dotnetVersion) {
        Write-Host "  .NET SDK found: $dotnetVersion"
        $dotnetInstalled = $true
    }
} catch {}

if (-not $dotnetInstalled) {
    # Check for just the runtime
    try {
        $runtimes = & dotnet --list-runtimes 2>$null
        if ($runtimes -match "Microsoft.AspNetCore.App 8") {
            Write-Host "  .NET ASP.NET Core 8.x runtime found."
            $dotnetInstalled = $true
        }
    } catch {}
}

if (-not $dotnetInstalled) {
    Write-Host "  .NET 8 runtime not found. Installing..." -ForegroundColor Yellow

    $installerUrl = "https://dot.net/v1/dotnet-install.ps1"
    $installerPath = "$env:TEMP\dotnet-install.ps1"

    Invoke-WebRequest -Uri $installerUrl -OutFile $installerPath
    & $installerPath -Channel 8.0 -Runtime aspnetcore

    Write-Host "  .NET ASP.NET Core runtime installed." -ForegroundColor Green
}

# ---- Stop existing service ----
Write-Host "[2/5] Stopping existing service (if running)..." -ForegroundColor Yellow

$existingService = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($existingService) {
    if ($existingService.Status -eq "Running") {
        Stop-Service -Name $ServiceName -Force
        Write-Host "  Stopped existing service."
    }
    # Give it a moment to fully stop
    Start-Sleep -Seconds 2
}

# ---- Build / Download the service ----
Write-Host "[3/5] Building service..." -ForegroundColor Yellow

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectDir = Split-Path -Parent $scriptDir
$csprojPath = Join-Path $projectDir "PiPrintService.csproj"

$publishDir = Join-Path $env:TEMP "webprintservice-publish"
if (Test-Path $publishDir) { Remove-Item $publishDir -Recurse -Force }

if (Test-Path $csprojPath) {
    Write-Host "  Building from local source: $projectDir"
    & dotnet publish $csprojPath -c Release -r win-x64 --self-contained true -o $publishDir -p:PublishSingleFile=false -p:PublishTrimmed=false
} else {
    Write-Host "  Cloning from GitHub: $GitHubRepo..."
    $cloneDir = Join-Path $env:TEMP "webprintservice-clone"
    if (Test-Path $cloneDir) { Remove-Item $cloneDir -Recurse -Force }
    & git clone --depth 1 --branch $Branch "https://github.com/$GitHubRepo.git" $cloneDir
    & dotnet publish (Join-Path $cloneDir "PiPrintService.csproj") -c Release -r win-x64 --self-contained true -o $publishDir -p:PublishSingleFile=false -p:PublishTrimmed=false
    Remove-Item $cloneDir -Recurse -Force
}

# ---- Install files ----
Write-Host "[4/5] Installing to $InstallDir..." -ForegroundColor Yellow

if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir -Force | Out-Null
}

# Copy published files (preserve existing SQLite DB if present)
$existingDb = $null
$dbPath = Join-Path $InstallDir "webprintservice.db"
if (Test-Path $dbPath) {
    $existingDb = Join-Path $env:TEMP "webprintservice-db-backup.db"
    Copy-Item $dbPath $existingDb
    Write-Host "  Backed up existing database."
}

Copy-Item "$publishDir\*" $InstallDir -Recurse -Force

# Restore database if it existed
if ($existingDb -and (Test-Path $existingDb)) {
    Copy-Item $existingDb $dbPath -Force
    Remove-Item $existingDb
    Write-Host "  Restored existing database."
}

# Update appsettings.json
$settingsPath = Join-Path $InstallDir "appsettings.json"
if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath | ConvertFrom-Json
    if (-not $settings.Print) { $settings | Add-Member -NotePropertyName "Print" -NotePropertyValue @{} }
    $settings.Print.ServerUrl = $WebServerUrl
    $settings.Print.Port = "$Port"
    $settings | ConvertTo-Json -Depth 10 | Set-Content $settingsPath
    Write-Host "  Updated appsettings.json."
}

# Clean up publish dir
Remove-Item $publishDir -Recurse -Force

# ---- Register as Windows Service ----
Write-Host "[5/5] Registering Windows Service..." -ForegroundColor Yellow

# Find the executable
$exePath = Join-Path $InstallDir "PiPrintService.exe"
if (-not (Test-Path $exePath)) {
    $exePath = Join-Path $InstallDir "WebPrintService.exe"
}

if ($existingService) {
    Write-Host "  Updating existing service..."
    & sc.exe config $ServiceName binPath= "`"$exePath`""
} else {
    Write-Host "  Creating new service..."
    & sc.exe create $ServiceName binPath= "`"$exePath`"" start= auto DisplayName= "$ServiceDisplayName"
    & sc.exe description $ServiceName "Web Print Service - Remote printing via SignalR"
}

# Configure service recovery (restart on failure)
& sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/10000/restart/30000

# Start the service
Start-Service -Name $ServiceName
Start-Sleep -Seconds 3

$svc = Get-Service -Name $ServiceName
if ($svc.Status -eq "Running") {
    Write-Host ""
    Write-Host "============================================" -ForegroundColor Green
    Write-Host "  Deployment Complete!" -ForegroundColor Green
    Write-Host "============================================" -ForegroundColor Green
    Write-Host "  Service URL:  http://localhost:$Port"
    Write-Host "  Swagger:      http://localhost:$Port/swagger"
    Write-Host "  Web Server:   $WebServerUrl"
    Write-Host "  Service ID:   $ServiceId"
    Write-Host ""
    Write-Host "  Useful commands:" -ForegroundColor Cyan
    Write-Host "    Get-Service $ServiceName"
    Write-Host "    Restart-Service $ServiceName"
    Write-Host "    Stop-Service $ServiceName"
    Write-Host "    Get-EventLog -LogName Application -Source $ServiceName -Newest 20"
    Write-Host ""
    Write-Host "  Update service ID via API:" -ForegroundColor Cyan
    Write-Host "    Invoke-RestMethod -Uri http://localhost:$Port/api/settings -Method Put ``"
    Write-Host "      -ContentType 'application/json' ``"
    Write-Host "      -Body '{`"serviceId`": `"$ServiceId`", `"serverUrl`": `"$WebServerUrl`"}'"
    Write-Host "============================================" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "WARNING: Service may not have started." -ForegroundColor Red
    Write-Host "Check logs: Get-EventLog -LogName Application -Source $ServiceName -Newest 20"
}
