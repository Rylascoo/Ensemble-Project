# Product regression actions; no Harness CLI/provider execution.
[CmdletBinding()]
param([switch]$Full, [string]$Filter)

$ErrorActionPreference = 'Stop'
Push-Location -LiteralPath (Split-Path -Parent $PSScriptRoot)
try {
    $projects = @('tests/Kymaean.Application.Tests/Kymaean.Application.Tests.csproj')
    if ($Full) { $projects += 'tests/Kymaean.Infrastructure.Persistence.Tests/Kymaean.Infrastructure.Persistence.Tests.csproj' }
    foreach ($project in $projects) {
        $testArgs = @('test', '--project', $project, '-c', 'Release', '-r', 'win-arm64')
        if ($Filter) { $testArgs += @('--filter', $Filter) }
        & dotnet @testArgs
        if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    }
} finally {
    Pop-Location
}
