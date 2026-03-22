#Requires -Version 5.1
<#
.SYNOPSIS
    Builds a GitHub-uploadable zip: Buu/Buu.dll, Buu/Buu.json, and Buu/Buu.pck (if has_pck).

.PARAMETER ModFolder
    Folder that already contains the built mod files (usually .../Slay the Spire 2/mods/Buu).
    If omitted, uses environment variable BUU_MOD_RELEASE_DIR.

.EXAMPLE
    .\package-release.ps1 -ModFolder "D:\SteamLibrary\steamapps\common\Slay the Spire 2\mods\Buu"
#>
param(
    [Parameter(Mandatory = $false)]
    [string] $ModFolder = $env:BUU_MOD_RELEASE_DIR
)

$ErrorActionPreference = "Stop"
Set-StrictMode -Version Latest

$repoRoot = $PSScriptRoot
$manifestPath = Join-Path $repoRoot "Buu.json"

if (-not $ModFolder -or -not (Test-Path -LiteralPath $ModFolder -PathType Container)) {
    throw @"
Mod folder not found. Pass -ModFolder or set BUU_MOD_RELEASE_DIR to your game mods path, e.g.
  ...\Slay the Spire 2\mods\Buu
(after a successful build / publish so Buu.dll, Buu.json, and Buu.pck are there).
"@
}

if (-not (Test-Path -LiteralPath $manifestPath)) {
    throw "Buu.json not found at: $manifestPath"
}

$manifest = Get-Content -LiteralPath $manifestPath -Raw -Encoding UTF8 | ConvertFrom-Json
$version = $manifest.version
if (-not $version) {
    throw "Buu.json is missing a 'version' field."
}

$requiredFiles = @("Buu.dll", "Buu.json")
if ($manifest.has_pck) {
    $requiredFiles += "Buu.pck"
}

foreach ($name in $requiredFiles) {
    $p = Join-Path $ModFolder $name
    if (-not (Test-Path -LiteralPath $p -PathType Leaf)) {
        throw "Missing release file: $p"
    }
}

$outDir = Join-Path $repoRoot "releases"
if (-not (Test-Path -LiteralPath $outDir -PathType Container)) {
    New-Item -ItemType Directory -Path $outDir | Out-Null
}

$zipName = "Buu-mod-$version.zip"
$zipPath = Join-Path $outDir $zipName

$stagingRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("buu-release-" + [guid]::NewGuid().ToString("n"))
$stagingBuu = Join-Path $stagingRoot "Buu"
try {
    New-Item -ItemType Directory -Path $stagingBuu -Force | Out-Null
    foreach ($name in $requiredFiles) {
        Copy-Item -LiteralPath (Join-Path $ModFolder $name) -Destination (Join-Path $stagingBuu $name) -Force
    }

    if (Test-Path -LiteralPath $zipPath) {
        Remove-Item -LiteralPath $zipPath -Force
    }

    Compress-Archive -Path $stagingBuu -DestinationPath $zipPath -CompressionLevel Optimal -Force
}
finally {
    Remove-Item -LiteralPath $stagingRoot -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host "Created: $zipPath"
