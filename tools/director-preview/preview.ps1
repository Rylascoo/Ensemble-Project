[CmdletBinding()]
param(
    [Parameter(Mandatory)][ValidateSet('Refresh','Launch','Status','NewProfile','SelectProfile','VerifyProfiles')][string]$Action,
    [string]$Stage, [string]$ExpectedSource, [string]$ProfileId,
    [ValidateSet('director','empty','rich','falsifier','performance-success','performance-empty','performance-incompatible','performance-invalid','performance-io','performance-access','performance-render')][string]$Kind = 'empty', [string]$Name,
    [switch]$Resume,
    [ValidateSet('None','BeforeRegistration','AfterRegistration','AfterReceipt')][string]$TestFailure = 'None')
. "$PSScriptRoot/common.ps1"
if ([Runtime.InteropServices.RuntimeInformation]::OSArchitecture -ne 'Arm64' -or [Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -ne 'Arm64') { throw 'Use native ARM64 PowerShell on Windows ARM64.' }
$control = Join-Path $env:LOCALAPPDATA 'KymaeanDirectorPreview'
New-Item -ItemType Directory -Path $control -Force | Out-Null
Assert-PlainTree $control
$pendingPath = Join-Path $control 'refresh-pending.json'
$controlLock = [IO.File]::Open((Join-Path $control 'control.lock'), [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None)
$dataLock = $null
try {
    $package = Get-PreviewPackage
    if ($Action -eq 'Status') {
        [ordered]@{ identity=$PreviewIdentity; name=$PreviewName; registered=[bool]$package; install=if($package){$package.InstallLocation}else{$null}; pending=(Test-Path $pendingPath); receipts=@(Get-ChildItem $control -Filter 'refresh-*.json' | ForEach-Object FullName) } | ConvertTo-Json -Depth 5
        return
    }
    if ((Test-Path $pendingPath) -and !($Action -eq 'Refresh' -and $Resume)) { throw 'Interrupted refresh retained. Inspect it, then explicitly resume the same stage; no implicit rollback.' }
    if ($Action -ne 'Refresh') {
        if (!$package) { throw 'Preview is not registered. Refresh a validated stage first.' }
        $Stage = Split-Path -Parent $package.InstallLocation
        $build = Assert-Stage $Stage
        if ($Action -eq 'Launch') { Start-VerifiedPreview $package $build.source | ConvertTo-Json; return }
        Assert-Closed $package
        $localState = Get-PreviewLocalState $package
        switch ($Action) {
            'NewProfile' { Invoke-Profiles $Stage @('create',$localState,$Kind,$Name,$build.source) | ConvertTo-Json }
            'SelectProfile' { Invoke-Profiles $Stage @('select',$localState,$ProfileId) | ConvertTo-Json }
            'VerifyProfiles' { Invoke-Profiles $Stage @('verify',$localState) | ConvertTo-Json -Depth 8 }
        }
        return
    }
    $Stage = [IO.Path]::GetFullPath($Stage)
    $build = Assert-Stage $Stage
    if ($ExpectedSource -cnotmatch '^[a-f0-9]{40}$' -or $ExpectedSource -cne $build.source) { throw 'Explicit admitted source does not match staged source.' }
    [xml]$stagedManifest = Get-Content (Join-Path $Stage 'layout/AppxManifest.xml') -Raw
    if ($package -and [version]$stagedManifest.Package.Identity.Version -lt [version]$package.Version) { throw 'Preview binary downgrade refused; persisted data is never rolled back implicitly.' }
    if ($package -and [version]$stagedManifest.Package.Identity.Version -eq [version]$package.Version -and [IO.Path]::GetFullPath($package.InstallLocation) -ne [IO.Path]::GetFullPath((Join-Path $Stage 'layout'))) { throw 'A different staged layout requires a newer development version.' }
    Assert-Closed $package
    $pending = $null
    if (Test-Path $pendingPath) {
        $pending = Get-Content $pendingPath -Raw | ConvertFrom-Json
        if ($pending.stage -cne $Stage -or $pending.buildHash -cne (Get-FileHash (Join-Path $Stage 'build.json')).Hash) { throw 'Resume must use the exact retained stage and receipt.' }
    } else {
        $run = [Guid]::NewGuid().ToString('N')
        $localState = if($package){ Get-PreviewLocalState $package }else{ $null }
        if (!$package -and @(Get-ChildItem (Join-Path $env:LOCALAPPDATA 'Packages') -Directory -Filter 'Kymaean.DirectorPreview_*').Count -gt 0) { throw 'Unregistered Preview data exists; reconcile it before initial registration.' }
        $before = @(); $selection = $null; $backup = $null
        if ($localState) {
            $dataLock = Open-PreviewLock $localState
            $profilesPath = Join-Path $localState 'DirectorPreview/profiles'
            $before = @(Get-Inventory $profilesPath)
            $selection = Get-Content (Join-Path $localState 'DirectorPreview/active-profile.json') -Raw
            $backup = Join-Path $control "preserved-$run"
            Copy-Item -LiteralPath $profilesPath -Destination $backup -Recurse
            Assert-Inventory $backup $before
        }
        $pending = [ordered]@{ run=$run; stage=$Stage; source=$build.source; buildHash=(Get-FileHash (Join-Path $Stage 'build.json')).Hash; previous=if($package){$package.InstallLocation}else{$null}; localState=$localState; before=$before; selection=$selection; backup=$backup; authorization=$build.authorization }
        Write-NewJson $pendingPath $pending
    }
    if ($pending.localState) {
        if (!$dataLock) { $dataLock = Open-PreviewLock $pending.localState }
        Assert-PreservedProfiles (Join-Path $pending.localState 'DirectorPreview/profiles') $pending.before
        if ((Get-Content (Join-Path $pending.localState 'DirectorPreview/active-profile.json') -Raw) -cne $pending.selection) { throw 'Selected profile changed after interrupted refresh.' }
    }
    $completedReceipt = Join-Path $control "refresh-$($pending.run).json"
    if (Test-Path $completedReceipt) {
        $completed = Get-Content $completedReceipt -Raw | ConvertFrom-Json
        if ($completed.status -cne 'PASS' -or $completed.source -cne $build.source -or $completed.buildHash -cne $pending.buildHash -or !$package -or [IO.Path]::GetFullPath($package.InstallLocation) -ne [IO.Path]::GetFullPath((Join-Path $Stage 'layout'))) { throw 'Existing completion receipt does not match this transaction.' }
        $localState = Get-PreviewLocalState $package
        if (!$dataLock) { $dataLock = Open-PreviewLock $localState }
        Assert-PreservedProfiles (Join-Path $localState 'DirectorPreview/profiles') $completed.after
        Move-Item -LiteralPath $pendingPath -Destination (Join-Path $control "completed-$($pending.run).json")
        Set-ActivationGate $localState $Stage $build.source 'admitted'
        Write-Output 'RESUMED_COMPLETION=PASS'
        return
    }
    if ($pending.localState) { Set-ActivationGate $pending.localState $Stage $build.source 'blocked' }
    if ($TestFailure -eq 'BeforeRegistration') { throw 'TEST: interrupted before registration; pending receipt preserved.' }
    # Same identity development registration only. No uninstall or persisted-data rollback path exists.
    Add-AppxPackage -Register ([IO.Path]::GetFullPath((Join-Path $Stage 'layout/AppxManifest.xml'))) -ErrorAction Stop
    $package = Get-PreviewPackage
    if (!$package -or [IO.Path]::GetFullPath($package.InstallLocation) -ne [IO.Path]::GetFullPath((Join-Path $Stage 'layout'))) { throw 'Registration did not select the exact staged layout.' }
    if ($TestFailure -eq 'AfterRegistration') { throw 'TEST: interrupted after registration; pending receipt preserved.' }
    if ($dataLock) { $dataLock.Dispose(); $dataLock=$null }
    $localState = Get-PreviewLocalState $package
    if (!$pending.localState) {
        $profilesPath = Join-Path $localState 'DirectorPreview/profiles'
        if (!(Test-Path $profilesPath)) {
            $director = Invoke-Profiles $Stage @('create',$localState,'director','Director workspace',$build.source)
            Invoke-Profiles $Stage @('select',$localState,$director.id) | Out-Null
        } elseif (!(Test-Path (Join-Path $localState 'DirectorPreview/active-profile.json'))) { throw 'Existing profile data has no selection; reconcile explicitly.' }
    }
    $replayBefore = Invoke-Profiles $Stage @('verify',$localState)
    $nonce = [Guid]::NewGuid().ToString('N')
    Set-ActivationGate $localState $Stage $build.source 'smoke' $nonce
    $launch = Start-VerifiedPreview $package $build.source $nonce
    $process = Get-Process -Id $launch.pid
    if (!$process.CloseMainWindow() -or !$process.WaitForExit(10000)) { throw 'Preview smoke did not close gracefully; no force termination.' }
    $dataLock = Open-PreviewLock $localState
    if ($pending.localState) {
        Assert-PreservedProfiles (Join-Path $localState 'DirectorPreview/profiles') $pending.before
        if ((Get-Content (Join-Path $localState 'DirectorPreview/active-profile.json') -Raw) -cne $pending.selection) { throw 'Refresh changed selected profile.' }
    }
    $after = @(Get-Inventory (Join-Path $localState 'DirectorPreview/profiles'))
    $dataLock.Dispose(); $dataLock=$null
    $replayAfter = Invoke-Profiles $Stage @('verify',$localState)
    if (($replayBefore | ConvertTo-Json -Depth 8 -Compress) -cne ($replayAfter | ConvertTo-Json -Depth 8 -Compress)) { throw 'Profile replay changed during refresh.' }
    Write-NewJson (Join-Path $control "refresh-$($pending.run).json") ([ordered]@{ status='PASS'; source=$build.source; identity=$PreviewIdentity; stage=$Stage; buildHash=$pending.buildHash; previous=$pending.previous; backup=$pending.backup; before=$pending.before; after=$after; launch=$launch; replay=$replayAfter; authority='Development Preview only; not merge approval or Design acceptance.' })
    if ($TestFailure -eq 'AfterReceipt') { throw 'TEST: interrupted after success receipt; completion must resume idempotently.' }
    # Retain the transaction record; completion moves only the small marker, never application data.
    Move-Item -LiteralPath $pendingPath -Destination (Join-Path $control "completed-$($pending.run).json")
    Set-ActivationGate $localState $Stage $build.source 'admitted'
    [ordered]@{ result='PASS'; source=$build.source; stage=$Stage; identity=$PreviewIdentity; localState=$localState } | ConvertTo-Json
} finally { if($dataLock){$dataLock.Dispose()}; $controlLock.Dispose() }
