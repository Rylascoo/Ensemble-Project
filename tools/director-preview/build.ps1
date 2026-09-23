[CmdletBinding()]
param([Parameter(Mandatory)][string]$OutputRoot, [Parameter(Mandatory)][string]$Authorization)
. "$PSScriptRoot/common.ps1"
$repo = (Resolve-Path "$PSScriptRoot/../..").Path
Push-Location $repo
try {
    $source = (& git rev-parse HEAD).Trim()
    if ($LASTEXITCODE -ne 0 -or (& git status --porcelain)) { throw 'Build requires an exact clean committed source.' }
    if ([string]::IsNullOrWhiteSpace($Authorization)) { throw 'Record the Director/package authorization; a build is not authority.' }
    $stage = Join-Path ([IO.Path]::GetFullPath($OutputRoot)) ($source.Substring(0,12) + '-' + [Guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $stage | Out-Null
    $commands = @(
        @('test','--project','tools/director-preview/Tests/Tests.csproj','-c','Release','-r','win-arm64'),
        @('test','--project','tests/Kymaean.Application.Tests/Kymaean.Application.Tests.csproj','-c','Release','-r','win-arm64'),
        @('test','--project','tests/Kymaean.Infrastructure.Persistence.Tests/Kymaean.Infrastructure.Persistence.Tests.csproj','-c','Release','-r','win-arm64'),
        @('test','--project','tests/Ensemble.E0.Core.Tests/Ensemble.E0.Core.Tests.csproj','-c','Release','-r','win-arm64'),
        @('test','--project','tests/Ensemble.E0.Harness.Tests/Ensemble.E0.Harness.Tests.csproj','-c','Release','-r','win-arm64'),
        @('build','src/Kymaean.Windows/Kymaean.Windows.csproj','-c','Release','-p:Platform=ARM64','-r','win-arm64','-p:KymaeanDirectorPreview=true',"-p:PreviewSourceRevision=$source"),
        @('build','src/Kymaean.Windows/Kymaean.Windows.csproj','-c','Release','-p:Platform=ARM64','-r','win-arm64','-p:KymaeanDirectorPreview=false'),
        @('build','tools/director-preview/Profiles/Profiles.csproj','-c','Release','-r','win-arm64'))
    $checks = @(); $number = 0
    foreach ($arguments in $commands) {
        $number++; $log = Join-Path $stage "check-$number.txt"
        & dotnet @arguments *> $log
        if ($LASTEXITCODE -ne 0) { throw "Technical check failed; preserved log $log" }
        $checks += [ordered]@{ command = 'dotnet ' + ($arguments -join ' '); result = 'PASS'; log = "check-$number.txt"; sha256 = (Get-FileHash $log -Algorithm SHA256).Hash }
    }
    $layout = Join-Path $stage 'layout'; New-Item -ItemType Directory -Path $layout | Out-Null
    $output = Join-Path $repo 'src/Kymaean.Windows/bin/DirectorPreview/ARM64/Release/net10.0-windows10.0.26100.0/win-arm64'
    if (!(Test-Path $output)) { throw "Preview output missing: $output" }
    Get-ChildItem -LiteralPath $output -File | Copy-Item -Destination $layout
    Copy-Item -LiteralPath "$repo/src/Kymaean.Windows/Assets" -Destination $layout -Recurse
    $manifestPath = Join-Path $layout 'AppxManifest.xml'
    [xml]$manifest = Get-Content -LiteralPath $manifestPath -Raw
    $manifest.Package.Identity.Name = $PreviewIdentity
    $manifest.Package.Properties.DisplayName = $PreviewName
    $manifest.Package.Applications.Application.VisualElements.DisplayName = $PreviewName
    $manifest.Package.Applications.Application.VisualElements.Description = 'Local development preview; no release or Design acceptance claim.'
    $manifest.Save($manifestPath)
    Copy-Item -LiteralPath "$PSScriptRoot/Profiles/bin/Release/net10.0/win-arm64" -Destination (Join-Path $stage 'profiles') -Recurse
    $ordinary = Join-Path $repo 'src/Kymaean.Windows/bin/ARM64/Release/net10.0-windows10.0.26100.0/win-arm64'
    & dotnet (Join-Path $stage 'profiles/Profiles.dll') inspect-build-boundary (Join-Path $ordinary 'Kymaean.Windows.dll') (Join-Path $layout 'Kymaean.Windows.dll') *> (Join-Path $stage 'build-boundary.txt')
    if ($LASTEXITCODE -ne 0) { throw 'Preview leaked into ordinary assembly or is absent from Preview assembly.' }
    foreach ($path in @('src/Kymaean.Windows/App.xaml.cs','src/Kymaean.Windows/MainWindow.xaml.cs')) {
        $original = ((& git show "69c68001836ed984df672587cecd41d340a31337:$path") -join "`n").Trim()
        $current = (Get-Content -LiteralPath $path -Raw).Replace("`r`n","`n")
        $ordinarySource = [regex]::Replace($current, '(?ms)^#if KYMAEAN_DIRECTOR_PREVIEW\n.*?^#endif\n', '').Trim()
        if ($ordinarySource -cne $original) { throw "Ordinary startup/title source differs from admitted baseline: $path" }
    }
    [xml]$ordinaryManifest = Get-Content (Join-Path $ordinary 'AppxManifest.xml') -Raw
    if ($ordinaryManifest.Package.Identity.Name -cne 'D2F0C54F-1F72-4CFE-A5F5-546A1918F94E' -or $ordinaryManifest.Package.Properties.DisplayName -cne 'Kymaean') { throw 'Ordinary identity changed.' }
    # No source is allowed to change underneath the staged build.
    if ((& git rev-parse HEAD).Trim() -cne $source -or (& git status --porcelain)) { throw 'Source moved during build.' }
    $receipt = [ordered]@{ schema = 1; identity = $PreviewIdentity; name = $PreviewName; source = $source; authorization = $Authorization; compatibility = $PreviewCompatibility; status = 'TECHNICAL_BUILD_CHECKS_PASS'; checks = $checks; layout = @(Get-Inventory $layout); profiles = @(Get-Inventory (Join-Path $stage 'profiles')); nonAuthority = 'Not merge approval, Design acceptance, Alpha, provider or release authority.' }
    Write-NewJson (Join-Path $stage 'build.json') $receipt
    Write-Output $stage
} finally { Pop-Location }
