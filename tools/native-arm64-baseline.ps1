[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$OutputRoot,
    [Parameter(Mandatory)][string]$Authorization,
    [ValidateRange(1, 9)][int]$LaunchCount = 3
)

$ErrorActionPreference = 'Stop'
if ([Runtime.InteropServices.RuntimeInformation]::OSArchitecture -ne 'Arm64' -or
    [Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -ne 'Arm64') {
    throw 'Native ARM64 baseline requires native ARM64 PowerShell on Windows ARM64.'
}

$repo = (Resolve-Path "$PSScriptRoot/..").Path
$nativePwsh = Join-Path $PSHOME 'pwsh.exe'
Push-Location $repo
try {
    $source = (& git rev-parse HEAD).Trim()
    if ($LASTEXITCODE -ne 0 -or $source -cnotmatch '^[0-9a-f]{40}$') {
        throw 'Unable to resolve exact source revision.'
    }
    if (& git status --porcelain) {
        throw 'Native ARM64 baseline requires a clean committed source.'
    }

    $runRoot = Join-Path ([IO.Path]::GetFullPath($OutputRoot)) (
        'native-arm64-' + $source.Substring(0, 12) + '-' + [Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $runRoot -Force | Out-Null
    $result = [ordered]@{
        schema = 1
        source = $source
        machine = [Environment]::MachineName
        osArchitecture = [Runtime.InteropServices.RuntimeInformation]::OSArchitecture.ToString()
        processArchitecture = [Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture.ToString()
        sdk = (& dotnet --version).Trim()
        authorization = $Authorization
        measurementNote = 'Wall-clock timings include harness/process verification overhead and are regression evidence, not product KPIs.'
    }
    $presentationLog = Join-Path $runRoot 'presentation-tests.txt'
    $sw = [Diagnostics.Stopwatch]::StartNew()
    & dotnet test --project tests/Kymaean.Windows.Presentation.Tests/Kymaean.Windows.Presentation.Tests.csproj -c Release -r win-arm64 *> $presentationLog
    $presentationExit = $LASTEXITCODE
    $sw.Stop()
    if ($presentationExit -ne 0) {
        throw "Presentation tests failed; preserved log $presentationLog"
    }
    $result.presentationTests = [ordered]@{
        result = 'PASS'
        elapsedMs = $sw.ElapsedMilliseconds
        log = $presentationLog
        sha256 = (Get-FileHash $presentationLog -Algorithm SHA256).Hash
    }

    $stageRoot = Join-Path $runRoot 'stages'
    New-Item -ItemType Directory -Path $stageRoot -Force | Out-Null
    $buildLog = Join-Path $runRoot 'preview-build.txt'
    $sw.Restart()
    $buildOutput = @(& $nativePwsh -NoProfile -File "$repo/tools/director-preview/build.ps1" -OutputRoot $stageRoot -Authorization $Authorization 2>&1)
    $buildExit = $LASTEXITCODE
    $sw.Stop()
    $buildOutput | Set-Content -LiteralPath $buildLog -Encoding utf8
    if ($buildExit -ne 0) {
        throw "Director Preview technical build failed; preserved log $buildLog"
    }
    $stage = ($buildOutput | Select-Object -Last 1).ToString().Trim()
    if (!(Test-Path -LiteralPath (Join-Path $stage 'build.json'))) {
        throw 'Director Preview technical build did not return a valid staged receipt.'
    }
    $result.previewBuild = [ordered]@{
        result = 'PASS'
        elapsedMs = $sw.ElapsedMilliseconds
        stage = $stage
        receipt = (Join-Path $stage 'build.json')
        receiptSha256 = (Get-FileHash (Join-Path $stage 'build.json') -Algorithm SHA256).Hash
    }
    $refreshLog = Join-Path $runRoot 'preview-refresh.txt'
    $sw.Restart()
    $refreshOutput = @(& $nativePwsh -NoProfile -File "$repo/tools/director-preview/preview.ps1" -Action Refresh -Stage $stage -ExpectedSource $source 2>&1)
    $refreshExit = $LASTEXITCODE
    $sw.Stop()
    $refreshOutput | Set-Content -LiteralPath $refreshLog -Encoding utf8
    if ($refreshExit -ne 0) {
        throw "Director Preview refresh failed; preserved log $refreshLog"
    }
    $result.previewRefresh = [ordered]@{
        result = 'PASS'
        elapsedMs = $sw.ElapsedMilliseconds
        log = $refreshLog
    }

    $launchSamples = @()
    for ($index = 1; $index -le $LaunchCount; $index++) {
        $sw.Restart()
        $launchOutput = @(& $nativePwsh -NoProfile -File "$repo/tools/director-preview/preview.ps1" -Action Launch 2>&1)
        $launchExit = $LASTEXITCODE
        $sw.Stop()
        if ($launchExit -ne 0) {
            throw "Verified Director Preview launch $index failed."
        }
        $launch = ($launchOutput -join [Environment]::NewLine) | ConvertFrom-Json
        $process = Get-Process -Id $launch.pid
        $sample = [ordered]@{
            ordinal = $index
            elapsedMs = $sw.ElapsedMilliseconds
            workingSetBytes = $process.WorkingSet64
            privateMemoryBytes = $process.PrivateMemorySize64
            pid = $launch.pid
            nativeMachine = $launch.nativeMachine
            processMachine = $launch.processMachine
            executable = $launch.executable
            receipt = $launch.receipt
        }
        if (!$process.CloseMainWindow() -or !$process.WaitForExit(10000)) {
            throw "Verified Director Preview launch $index did not close gracefully."
        }
        $launchSamples += $sample
    }
    $elapsedSorted = @($launchSamples.elapsedMs | Sort-Object)
    $workingSetSorted = @($launchSamples.workingSetBytes | Sort-Object)
    $middle = [int][Math]::Floor($LaunchCount / 2)
    $result.verifiedLaunches = [ordered]@{
        result = 'PASS'
        samples = $launchSamples
        medianElapsedMs = $elapsedSorted[$middle]
        medianWorkingSetBytes = $workingSetSorted[$middle]
    }

    $resultPath = Join-Path $runRoot 'baseline.json'
    $result | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $resultPath -Encoding utf8
    Write-Output ($result | ConvertTo-Json -Depth 10)
    Write-Output "NATIVE_ARM64_BASELINE_RESULT=$resultPath"
} finally {
    Pop-Location
}
