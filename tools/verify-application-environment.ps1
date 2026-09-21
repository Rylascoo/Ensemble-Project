# Cheap, offline readiness check. Does not restore, build, test or install.
[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
Push-Location -LiteralPath $repoRoot
try {
    if (-not $IsWindows) { throw 'Use Windows 11 ARM64 for the native application environment.' }
    $osArch = [Runtime.InteropServices.RuntimeInformation]::OSArchitecture
    $processArch = [Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture
    Write-Output "OS=$([Environment]::OSVersion.Version) OS_ARCH=$osArch PROCESS_ARCH=$processArch"
    if ($osArch -ne 'Arm64' -or $processArch -ne 'Arm64') {
        throw 'Use native ARM64 PowerShell and .NET; x64 emulation is not native validation.'
    }
    if ([Environment]::OSVersion.Version.Build -lt 22000) { throw 'Windows 11 is required for this development environment.' }
    $sdkPolicy = Get-Content -LiteralPath global.json -Raw | ConvertFrom-Json
    if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
        throw "Install the ARM64 .NET SDK compatible with global.json ($($sdkPolicy.sdk.version))."
    }
    $selectedSdk = & dotnet --version
    if ($LASTEXITCODE -ne 0) { throw 'dotnet could not select the SDK required by global.json.' }
    Write-Output "DOTNET_SDK=$selectedSdk POLICY=$($sdkPolicy.sdk.version)/$($sdkPolicy.sdk.rollForward)"
    $dotnetInfo = & dotnet --info
    if ($LASTEXITCODE -ne 0) { throw 'dotnet --info failed.' }
    # RID is stable across localized dotnet output; version selection is enforced by dotnet/global.json.
    if (($dotnetInfo -join "`n") -notmatch '(?m)^\s*RID:\s+win-arm64\s*$') {
        throw 'The selected dotnet host must report RID win-arm64.'
    }
    [xml]$windowsProject = Get-Content -LiteralPath src/Kymaean.Windows/Kymaean.Windows.csproj -Raw
    $tfm = $windowsProject.SelectSingleNode('//TargetFramework').InnerText
    if ($tfm -notmatch '-windows(?<sdk>\d+\.\d+\.\d+\.\d+)$') { throw "Unrecognized Windows target framework: $tfm" }
    $windowsSdk = $Matches.sdk
    $kits = Join-Path ${env:ProgramFiles(x86)} 'Windows Kits/10'
    foreach ($required in @("Include/$windowsSdk/um/Windows.h", "bin/$windowsSdk/arm64/makepri.exe", "bin/$windowsSdk/arm64/makeappx.exe")) {
        if (-not (Test-Path -LiteralPath (Join-Path $kits $required))) {
            throw "Install Windows SDK $windowsSdk with ARM64 tools; missing $required. No install was attempted."
        }
    }
    Write-Output "WINDOWS_TARGET=$tfm WINDOWS_SDK=$windowsSdk"
    $solution = Get-Content -LiteralPath Ensemble.sln -Raw
    $projects = [regex]::Matches($solution, '"([^"\r\n]+\.csproj)"')
    if ($projects.Count -eq 0) { throw 'Ensemble.sln contains no C# projects.' }
    foreach ($match in $projects) {
        $projectPath = $match.Groups[1].Value
        if (-not (Test-Path -LiteralPath $projectPath)) { throw "Missing solution project: $projectPath" }
        [xml]$project = Get-Content -LiteralPath $projectPath -Raw
        foreach ($reference in $project.SelectNodes('//ProjectReference')) {
            $target = Join-Path (Split-Path $projectPath -Parent) $reference.Include
            if (-not (Test-Path -LiteralPath $target)) { throw "Missing project reference: $target" }
        }
        $assets = Join-Path (Split-Path $projectPath -Parent) 'obj/project.assets.json'
        $restoreState = if (Test-Path -LiteralPath $assets) { 'PRESENT_NOT_VALIDATED' } else { 'REQUIRED' }
        Write-Output "PROJECT=$projectPath RESTORE=$restoreState SDK=$($project.Project.Sdk)"
        foreach ($package in $project.SelectNodes('//PackageReference')) {
            Write-Output "PACKAGE=$($package.Include) VERSION=$($package.Version)"
        }
    }
    Write-Output "ENVIRONMENT_PREREQUISITES=PASS PROJECTS=$($projects.Count)"
    Write-Output 'Restore is explicit on first test/build and may need NuGet network access. No lockfile reproducibility claim.'
    Write-Output 'Native UI launch/runtime registration, packaging, WACK and Store readiness are NOT TESTED.'
} finally {
    Pop-Location
}
