# =============================================================================
# Web Print Service - Windows Deploy Script
# =============================================================================
# Installs the Web Print Service as a Windows Service on the local machine.
# Builds locally if the WebPrintService source is alongside this script;
# otherwise clones the repo from GitHub. Installs .NET runtime, registers as
# a Windows Service, installs the BIXOLON BK3 driver (when provided), and
# creates a USB printer queue.
#
# Usage:
#   .\deploy-to-windows.ps1 -WebServerUrl "http://basicscale.scaledata.net"
#
# With options:
#   .\deploy-to-windows.ps1 -WebServerUrl "http://192.168.1.100:5110" `
#       -ServerId 2 -PrinterName "Kiosk" -Port 5230 `
#       -BixolonDriverPath "C:\drivers\BIXOLON_BK3"
#
# Requirements:
#   - Windows 10/11 or Windows Server 2016+
#   - Run as Administrator
#   - Internet access (for downloading .NET if needed)
# =============================================================================

param(
    [Parameter(Mandatory=$true)]
    [string]$WebServerUrl,

    [int]$ServerId           = 1,
    [string]$PrinterName     = "Kiosk",
    [string]$ServiceId       = "default",
    [int]$Port               = 5230,
    [string]$InstallDir      = "C:\Services\WebPrintService",
    [string]$ServiceName     = "WebPrintService",
    [string]$ServiceDisplayName = "Web Print Service",
    [string]$GitHubRepo      = "GTMichelli-Dev/nwgg-web-print-service",
    [string]$Branch          = "master",

    # BIXOLON BK3 driver install (optional). Provide a folder containing the
    # BIXOLON .inf file. If empty, the script looks in deploy\drivers\bixolon-bk3
    # next to this file; if still not found and the driver isn't already
    # installed, it warns and skips printer creation.
    [string]$BixolonDriverPath = "",
    [string]$BixolonDriverName = "BIXOLON BK3-3"
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
Write-Host "  Server ID:    $ServerId"
Write-Host "  Service ID:   $ServiceId"
Write-Host "  Printer Name: $PrinterName"
Write-Host "  Port:         $Port"
Write-Host "  Install Dir:  $InstallDir"
Write-Host "  Service Name: $ServiceName"
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# ---- Check .NET Runtime ----
Write-Host "[1/6] Checking .NET runtime..." -ForegroundColor Yellow

$dotnetInstalled = $false
try {
    $dotnetVersion = & dotnet --version 2>$null
    if ($dotnetVersion) {
        Write-Host "  .NET SDK found: $dotnetVersion"
        $dotnetInstalled = $true
    }
} catch {}

if (-not $dotnetInstalled) {
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
Write-Host "[2/6] Stopping existing service (if running)..." -ForegroundColor Yellow

$existingService = Get-Service -Name $ServiceName -ErrorAction SilentlyContinue
if ($existingService) {
    if ($existingService.Status -eq "Running") {
        Stop-Service -Name $ServiceName -Force
        Write-Host "  Stopped existing service."
    }
    Start-Sleep -Seconds 2
}

# ---- Build / Download the service ----
Write-Host "[3/6] Building service..." -ForegroundColor Yellow

$scriptDir  = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectDir = Split-Path -Parent $scriptDir
$csprojPath = Join-Path $projectDir "WebPrintService.csproj"

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
    & dotnet publish (Join-Path $cloneDir "WebPrintService.csproj") -c Release -r win-x64 --self-contained true -o $publishDir -p:PublishSingleFile=false -p:PublishTrimmed=false
    Remove-Item $cloneDir -Recurse -Force
}

# ---- Install files ----
Write-Host "[4/6] Installing to $InstallDir..." -ForegroundColor Yellow

if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir -Force | Out-Null
}

$existingDb = $null
$dbPath = Join-Path $InstallDir "webprintservice.db"
if (Test-Path $dbPath) {
    $existingDb = Join-Path $env:TEMP "webprintservice-db-backup.db"
    Copy-Item $dbPath $existingDb
    Write-Host "  Backed up existing database."
}

Copy-Item "$publishDir\*" $InstallDir -Recurse -Force

if ($existingDb -and (Test-Path $existingDb)) {
    Copy-Item $existingDb $dbPath -Force
    Remove-Item $existingDb
    Write-Host "  Restored existing database."
}

# Update appsettings.json (Print section)
$settingsPath = Join-Path $InstallDir "appsettings.json"
if (Test-Path $settingsPath) {
    $settings = Get-Content $settingsPath -Raw | ConvertFrom-Json
    if (-not $settings.Print) {
        $settings | Add-Member -NotePropertyName "Print" -NotePropertyValue ([PSCustomObject]@{})
    }
    $settings.Print | Add-Member -NotePropertyName "ServiceId"   -NotePropertyValue $ServiceId       -Force
    $settings.Print | Add-Member -NotePropertyName "ServerId"    -NotePropertyValue $ServerId        -Force
    $settings.Print | Add-Member -NotePropertyName "ServerUrls"  -NotePropertyValue @($WebServerUrl) -Force
    $settings.Print | Add-Member -NotePropertyName "Port"        -NotePropertyValue "$Port"          -Force
    $settings | ConvertTo-Json -Depth 10 | Set-Content $settingsPath -Encoding UTF8
    Write-Host "  Updated appsettings.json (ServerId=$ServerId, ServerUrls=[$WebServerUrl])."
}

Remove-Item $publishDir -Recurse -Force

# ---- Install BIXOLON BK3 driver + printer ----
Write-Host "[5/6] Installing BIXOLON BK3 driver and '$PrinterName' printer..." -ForegroundColor Yellow

$driverInstalled = $null -ne (Get-PrinterDriver -Name $BixolonDriverName -ErrorAction SilentlyContinue)

if (-not $driverInstalled) {
    # Resolve driver path: explicit -BixolonDriverPath > deploy\drivers\bixolon-bk3 next to this script
    $driverDir = $BixolonDriverPath
    if (-not $driverDir) {
        $driverDir = Join-Path $scriptDir "drivers\bixolon-bk3"
    }

    if (Test-Path $driverDir) {
        $inf = Get-ChildItem -Path $driverDir -Filter *.inf -Recurse -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($inf) {
            Write-Host "  Installing driver from $($inf.FullName)..."
            & pnputil.exe /add-driver $inf.FullName /install | Out-Null
            try {
                Add-PrinterDriver -Name $BixolonDriverName -ErrorAction Stop
                $driverInstalled = $true
                Write-Host "  Driver '$BixolonDriverName' installed." -ForegroundColor Green
            } catch {
                Write-Warning "  pnputil reported success but Add-PrinterDriver -Name '$BixolonDriverName' failed: $($_.Exception.Message)"
                Write-Warning "  Open Print Management and confirm the driver name; pass it via -BixolonDriverName if it differs."
            }
        } else {
            Write-Warning "  No .inf found under $driverDir — skipping driver install."
        }
    } else {
        Write-Warning "  BIXOLON BK3 driver not installed and no driver folder found."
        Write-Warning "  Download the BK3 Windows driver from https://www.bixolon.com, extract it,"
        Write-Warning "  and either place it at $(Join-Path $scriptDir 'drivers\bixolon-bk3') or"
        Write-Warning "  re-run with -BixolonDriverPath '<extracted-folder>'."
    }
} else {
    Write-Host "  Driver '$BixolonDriverName' already installed."
}

if ($driverInstalled) {
    $existingPrinter = Get-Printer -Name $PrinterName -ErrorAction SilentlyContinue
    if ($existingPrinter) {
        Write-Host "  Printer '$PrinterName' already exists — leaving as-is."
    } else {
        # For a USB plug-and-play printer Windows auto-creates the port. Pick one
        # that looks like a BIXOLON / USB virtual port; fall back to USB001.
        $usbPort = Get-PrinterPort -ErrorAction SilentlyContinue |
            Where-Object { $_.Name -match '^(USB|BIXOLON|BK3)' } |
            Select-Object -First 1

        $portName = if ($usbPort) { $usbPort.Name } else { "USB001" }

        try {
            Add-Printer -Name $PrinterName -DriverName $BixolonDriverName -PortName $portName -ErrorAction Stop
            Write-Host "  Added printer '$PrinterName' on port $portName (driver: $BixolonDriverName)." -ForegroundColor Green
        } catch {
            Write-Warning "  Add-Printer failed: $($_.Exception.Message)"
            Write-Warning "  Plug in the BK3 via USB and re-run, or add the printer manually in Print Management."
        }
    }
}

# ---- Register as Windows Service ----
Write-Host "[6/6] Registering Windows Service..." -ForegroundColor Yellow

$exePath = Join-Path $InstallDir "WebPrintService.exe"
if (-not (Test-Path $exePath)) {
    $exePath = Join-Path $InstallDir "PiPrintService.exe"  # legacy fallback
}

if ($existingService) {
    Write-Host "  Updating existing service..."
    & sc.exe config $ServiceName binPath= "`"$exePath`""
} else {
    Write-Host "  Creating new service..."
    & sc.exe create $ServiceName binPath= "`"$exePath`"" start= auto DisplayName= "$ServiceDisplayName"
    & sc.exe description $ServiceName "Web Print Service - Remote printing via SignalR"
}

& sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/10000/restart/30000

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
    Write-Host "  Server ID:    $ServerId"
    Write-Host "  Printer:      $PrinterName"
    Write-Host ""
    Write-Host "  Useful commands:" -ForegroundColor Cyan
    Write-Host "    Get-Service $ServiceName"
    Write-Host "    Restart-Service $ServiceName"
    Write-Host "    Get-Printer -Name '$PrinterName' | Format-List"
    Write-Host "============================================" -ForegroundColor Green
} else {
    Write-Host ""
    Write-Host "WARNING: Service may not have started." -ForegroundColor Red
    Write-Host "Check logs: Get-EventLog -LogName Application -Source $ServiceName -Newest 20"
}
