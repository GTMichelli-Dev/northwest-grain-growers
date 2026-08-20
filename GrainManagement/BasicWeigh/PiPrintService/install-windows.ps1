<#
.SYNOPSIS
    Install / upgrade the Grain Management Print Service on Windows.

.DESCRIPTION
    Idempotent Windows installer. What it does:
      1. Installs the .NET 8 ASP.NET Core runtime via winget (skipped when present).
      2. Installs SumatraPDF via winget — required for silent PDF printing
         (without it, Windows opens an "open with…" dialog for each PDF).
      3. Ensures the Windows Print Spooler is set to auto-start and running.
      4. dotnet publishes the project for win-x64.
      5. Stops the existing service (if any), syncs binaries to the install
         directory (preserving webprintservice.db across upgrades), and
         re-registers the service via sc.exe with restart-on-failure.
      6. Starts the service and prints the Swagger URL.

    Note: Windows has no CUPS. This service uses the native Windows Print
    Spooler. If you want to share Windows printers to the LAN (the rough
    equivalent of CUPS-public), use the built-in Add-Printer + Set-Printer
    -Shared workflow — see the README.

.PARAMETER InstallDir
    Where the published binaries live. Default: C:\Services\PrintService.

.PARAMETER ServiceName
    Windows service short name. Default: PrintService.

.PARAMETER Port
    Port the service listens on. Default: 5230.

.PARAMETER ServiceAccount
    Account the service runs under. Default: LocalSystem. Use a real account
    when you need to print to authenticated network shares.

.PARAMETER ServicePassword
    Password for ServiceAccount (only used when not LocalSystem).

.EXAMPLE
    .\install-windows.ps1
    .\install-windows.ps1 -Port 5330 -InstallDir D:\Services\PrintService
    .\install-windows.ps1 -ServiceAccount "DOMAIN\print-svc" -ServicePassword "..."
#>

[CmdletBinding()]
param(
    [string] $InstallDir      = "C:\Services\PrintService",
    [string] $ServiceName     = "PrintService",
    [int]    $Port            = 5230,
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
$projectFile = Join-Path $scriptDir "PiPrintService.csproj"
if (-not (Test-Path $projectFile)) {
    Write-Error "Can't find PiPrintService.csproj at $projectFile - run this from the PiPrintService folder."
    exit 1
}

Write-Host "=== Print Service install ===" -ForegroundColor Cyan
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

# ── 2. SumatraPDF (silent PDF printing) ───────────────────────────────
$sumatraPath = "$env:LOCALAPPDATA\SumatraPDF\SumatraPDF.exe"
$sumatraSystem = "$env:ProgramFiles\SumatraPDF\SumatraPDF.exe"
if (-not (Test-Path $sumatraPath) -and -not (Test-Path $sumatraSystem) -and -not (Test-Command "SumatraPDF.exe")) {
    Write-Host "==> Installing SumatraPDF via winget (silent PDF printing)..." -ForegroundColor Yellow
    winget install --id SumatraPDF.SumatraPDF --accept-source-agreements --accept-package-agreements --silent
    Refresh-EnvPath
} else {
    Write-Host "==> SumatraPDF already installed - skipping." -ForegroundColor Green
}

# ── 3. Print Spooler service ──────────────────────────────────────────
$spooler = Get-Service -Name Spooler -ErrorAction SilentlyContinue
if (-not $spooler) {
    Write-Error "Windows Print Spooler service not found. This host can't run a print service without it."
    exit 1
}
if ($spooler.StartType -ne 'Automatic') {
    Write-Host "==> Setting Print Spooler to automatic start..." -ForegroundColor Yellow
    Set-Service -Name Spooler -StartupType Automatic
}
if ($spooler.Status -ne 'Running') {
    Write-Host "==> Starting Print Spooler..." -ForegroundColor Yellow
    Start-Service -Name Spooler
}

# ── 4. dotnet publish ─────────────────────────────────────────────────
$runtime    = "win-x64"
$publishDir = Join-Path $scriptDir "bin\Release\$runtime-publish"

Write-Host "==> Publishing PiPrintService for $runtime..." -ForegroundColor Yellow
if (Test-Path $publishDir) { Remove-Item -Recurse -Force $publishDir }
& dotnet publish $projectFile -c Release -r $runtime --self-contained false -o $publishDir
if ($LASTEXITCODE -ne 0) {
    Write-Error "dotnet publish failed (exit $LASTEXITCODE)."
    exit 1
}

# ── 5. Stop existing service + sync binaries ──────────────────────────
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

# Preserve operator-set settings across upgrades.
$preserve = @("webprintservice.db")
$tempSave = Join-Path $env:TEMP "print-service-preserve-$(Get-Random)"
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

# ── 6. Register service ───────────────────────────────────────────────
$exePath = Join-Path $InstallDir "PiPrintService.exe"
$binPath = "`"$exePath`""
if (-not (Test-Path $exePath)) {
    Write-Error "Publish output is missing PiPrintService.exe at $exePath."
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
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Print Service" | Out-Null
    } else {
        sc.exe create $ServiceName binPath= $binPath start= auto DisplayName= "Grain Management Print Service" obj= "$ServiceAccount" password= "$ServicePassword" | Out-Null
    }
    if ($LASTEXITCODE -ne 0) {
        Write-Error "sc create failed (exit $LASTEXITCODE)."
        exit 1
    }
}

# Restart-on-failure: 5s, 5s, 5s, reset failure count after 1 day clean.
sc.exe failure $ServiceName reset= 86400 actions= restart/5000/restart/5000/restart/5000 | Out-Null

# Per-service env vars so port + URL bind without touching appsettings.
[Environment]::SetEnvironmentVariable("Print__Port",     "$Port",                "Machine")
[Environment]::SetEnvironmentVariable("ASPNETCORE_URLS", "http://0.0.0.0:$Port", "Machine")

# Allow the service to talk through the firewall.
if (Get-Command New-NetFirewallRule -ErrorAction SilentlyContinue) {
    if (-not (Get-NetFirewallRule -DisplayName "Grain Management Print Service" -ErrorAction SilentlyContinue)) {
        New-NetFirewallRule -DisplayName "Grain Management Print Service" `
            -Direction Inbound -Action Allow -Protocol TCP -LocalPort $Port | Out-Null
    }
}

# ── 7. Start + report ─────────────────────────────────────────────────
Write-Host "==> Starting $ServiceName..." -ForegroundColor Yellow
Start-Service -Name $ServiceName
Start-Sleep -Seconds 2

$svc = Get-Service -Name $ServiceName
if ($svc.Status -eq 'Running') {
    $ip = (Get-NetIPAddress -AddressFamily IPv4 -InterfaceAlias 'Ethernet*','Wi-Fi*' -ErrorAction SilentlyContinue | Where-Object { $_.IPAddress -notmatch '^169\.' } | Select-Object -First 1).IPAddress
    if (-not $ip) { $ip = "<this-host>" }
    Write-Host ""
    Write-Host "✅ print-service is running." -ForegroundColor Green
    Write-Host "   Swagger:        http://${ip}:${Port}/swagger"
    Write-Host "   Printers:       Get-Printer        (shared printers show up automatically)"
    Write-Host "   Logs:           Get-EventLog -LogName Application -Source PrintService -Newest 50"
    Write-Host "   Stop:           Stop-Service $ServiceName"
    Write-Host "   Uninstall:      Stop-Service $ServiceName; sc.exe delete $ServiceName"
} else {
    Write-Host ""
    Write-Error "Service failed to start. Status: $($svc.Status). Check the Application event log:"
    Write-Host "   Get-EventLog -LogName Application -Source PrintService -Newest 20"
    exit 1
}
