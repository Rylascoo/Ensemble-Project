Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$PreviewIdentity = 'Kymaean.DirectorPreview'
$PreviewName = 'KYMÆAN Director Preview'
$PreviewCompatibility = 'qprod08-journal-v1'

function Assert-PlainTree([string]$Path) {
    $item = Get-Item -LiteralPath $Path -Force
    for ($ancestor = [IO.DirectoryInfo]::new([IO.Path]::GetFullPath($Path)); $null -ne $ancestor; $ancestor = $ancestor.Parent) {
        if ($ancestor.Exists -and ($ancestor.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { throw "Reparse ancestor refused: $Path" }
    }
    if (($item.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { throw "Reparse path refused: $Path" }
    if ($item.PSIsContainer) {
        foreach ($child in Get-ChildItem -LiteralPath $Path -Force) { Assert-PlainTree $child.FullName }
    }
}
function Get-Inventory([string]$Path) {
    Assert-PlainTree $Path
    @(Get-ChildItem -LiteralPath $Path -File -Recurse -Force | Sort-Object FullName | ForEach-Object {
        [ordered]@{ path = [IO.Path]::GetRelativePath($Path, $_.FullName).Replace('\','/'); bytes = $_.Length; sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant() }
    })
}
function Assert-Inventory([string]$Path, $Expected) {
    $actual = @(Get-Inventory $Path) | ConvertTo-Json -Depth 6 -Compress
    $wanted = @($Expected) | ConvertTo-Json -Depth 6 -Compress
    if ($actual -cne $wanted) { throw "Inventory mismatch: $Path" }
}
function Get-AuthoritativeInventory($Inventory) {
    @($Inventory | Where-Object { $_.path -cnotmatch '/data/production-catalog/entry-[^/]+/(\.projection\.snapshot|\.pending-projection-snapshot-[^/]+)$' })
}
function Assert-PreservedProfiles([string]$Path, $Expected) {
    $actual = @(Get-AuthoritativeInventory @(Get-Inventory $Path)) | ConvertTo-Json -Depth 6 -Compress
    $wanted = @(Get-AuthoritativeInventory $Expected) | ConvertTo-Json -Depth 6 -Compress
    if ($actual -cne $wanted) { throw 'Authoritative profile data changed; refuse resume/refresh.' }
}
function Set-ActivationGate([string]$LocalState, [string]$Stage, [string]$Source, [string]$Mode, [string]$Nonce = '') {
    $root = Join-Path $LocalState 'DirectorPreview'
    New-Item -ItemType Directory -Path $root -Force | Out-Null
    $temporary = Join-Path $root ('activation-' + [Guid]::NewGuid().ToString('N') + '.tmp')
    Write-NewJson $temporary ([ordered]@{ source=$Source; executable=[IO.Path]::GetFullPath((Join-Path $Stage 'layout/Kymaean.Windows.exe')); mode=$Mode; nonce=$Nonce })
    [IO.File]::Move($temporary, (Join-Path $root 'activation.json'), $true)
}
function Write-NewJson([string]$Path, $Value) {
    $stream = [IO.File]::Open($Path, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write, [IO.FileShare]::None)
    try { $bytes = [Text.Encoding]::UTF8.GetBytes(($Value | ConvertTo-Json -Depth 12)); $stream.Write($bytes) } finally { $stream.Dispose() }
}
function Get-PreviewPackage {
    $packages = @(Get-AppxPackage -Name $PreviewIdentity)
    if ($packages.Count -gt 1) { throw 'Ambiguous Preview registration.' }
    if ($packages.Count -eq 1) {
        if ($packages[0].Name -cne $PreviewIdentity) { throw 'Wrong package identity.' }
        return $packages[0]
    }
    return $null
}
function Get-PreviewLocalState($Package) {
    Join-Path $env:LOCALAPPDATA "Packages/$($Package.PackageFamilyName)/LocalState"
}
function Assert-Closed($Package) {
    if (!$Package) { return }
    $expected = [IO.Path]::GetFullPath((Join-Path $Package.InstallLocation 'Kymaean.Windows.exe'))
    foreach ($process in Get-Process -Name 'Kymaean.Windows' -ErrorAction SilentlyContinue) {
        # Other preserved Kymaean identities can be inaccessible. Storage quiescence
        # is established by the Preview-only exclusive session lock, not by name.
        if (!$process.Path) { continue }
        if ([IO.Path]::GetFullPath($process.Path) -eq $expected) { throw 'Close Director Preview before maintenance.' }
    }
}
function Open-PreviewLock([string]$LocalState) {
    $root = Join-Path $LocalState 'DirectorPreview'
    New-Item -ItemType Directory -Path $root -Force | Out-Null
    Assert-PlainTree $root
    [IO.File]::Open((Join-Path $root 'session.lock'), [IO.FileMode]::OpenOrCreate, [IO.FileAccess]::ReadWrite, [IO.FileShare]::None)
}
function Invoke-Profiles([string]$Stage, [string[]]$Arguments) {
    $output = & dotnet (Join-Path $Stage 'profiles/Profiles.dll') @Arguments
    if ($LASTEXITCODE -ne 0) { throw 'Preview profile operation failed.' }
    $output | ConvertFrom-Json
}
function Assert-Stage([string]$Stage) {
    $Stage = [IO.Path]::GetFullPath($Stage)
    Assert-PlainTree $Stage
    $receipt = Get-Content -LiteralPath (Join-Path $Stage 'build.json') -Raw | ConvertFrom-Json
    if ($receipt.identity -cne $PreviewIdentity -or $receipt.compatibility -cne $PreviewCompatibility -or $receipt.source -cnotmatch '^[a-f0-9]{40}$' -or $receipt.status -cne 'TECHNICAL_BUILD_CHECKS_PASS') { throw 'Not a technically checked Preview stage.' }
    Assert-Inventory (Join-Path $Stage 'layout') $receipt.layout
    Assert-Inventory (Join-Path $Stage 'profiles') $receipt.profiles
    [xml]$manifest = Get-Content -LiteralPath (Join-Path $Stage 'layout/AppxManifest.xml') -Raw
    if ($manifest.Package.Identity.Name -cne $PreviewIdentity -or $manifest.Package.Properties.DisplayName -cne $PreviewName) { throw 'Preview manifest identity mismatch.' }
    return $receipt
}

function Start-VerifiedPreview($Package, [string]$Source, [string]$Nonce = '') {
    if (-not ('DirectorPreviewNative' -as [type])) {
        Add-Type -TypeDefinition @'
using System;
using System.Runtime.InteropServices;
[ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")] class PreviewActivationManager { }
[ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
interface IPreviewActivationManager {
 [PreserveSig] int ActivateApplication([MarshalAs(UnmanagedType.LPWStr)] string id, [MarshalAs(UnmanagedType.LPWStr)] string args, uint options, out uint pid);
}
public static class DirectorPreviewNative {
 public static uint Activate(string id, string args) { uint pid; var m=(IPreviewActivationManager)new PreviewActivationManager(); Marshal.ThrowExceptionForHR(m.ActivateApplication(id,args,2,out pid)); return pid; }
 [DllImport("kernel32.dll", SetLastError=true)] public static extern bool IsWow64Process2(IntPtr process, out ushort processMachine, out ushort nativeMachine);
}
'@
    }
    $localState = Get-PreviewLocalState $Package
    $launches = Join-Path $localState 'DirectorPreview/launches'
    $before = if (Test-Path -LiteralPath $launches) { @(Get-ChildItem -LiteralPath $launches -File | ForEach-Object Name) } else { @() }
    $previewProcessId = [DirectorPreviewNative]::Activate($Package.PackageFamilyName + '!App', $Nonce)
    $launchFile = $null
    for ($attempt = 0; $attempt -lt 100; $attempt++) {
        Start-Sleep -Milliseconds 100
        if (Test-Path -LiteralPath $launches) {
            $launchFile = Get-ChildItem -LiteralPath $launches -File | Where-Object Name -NotIn $before | Select-Object -Last 1
            if ($launchFile) { break }
        }
    }
    if (!$launchFile) { throw 'No fresh native Preview launch receipt.' }
    $launch = Get-Content -LiteralPath $launchFile.FullName -Raw | ConvertFrom-Json
    $process = Get-Process -Id $previewProcessId
    [ushort]$pm = 0; [ushort]$nm = 0
    if (![DirectorPreviewNative]::IsWow64Process2($process.Handle, [ref]$pm, [ref]$nm) -or $pm -ne 0 -or $nm -ne 0xAA64) { throw 'Preview is not native ARM64.' }
    if ($launch.source -cne $Source -or !$launch.success -or $launch.pid -ne $previewProcessId -or $launch.architecture -cne 'Arm64' -or [IO.Path]::GetFullPath($process.Path) -ne [IO.Path]::GetFullPath((Join-Path $Package.InstallLocation 'Kymaean.Windows.exe')) -or $process.MainWindowTitle -cne $PreviewName) { throw 'Native Preview admission mismatch.' }
    return [ordered]@{ pid = $previewProcessId; source = $Source; profile = $launch.profile; executable = $process.Path; nativeMachine = $nm; processMachine = $pm; title = $process.MainWindowTitle; receipt = $launchFile.FullName }
}
