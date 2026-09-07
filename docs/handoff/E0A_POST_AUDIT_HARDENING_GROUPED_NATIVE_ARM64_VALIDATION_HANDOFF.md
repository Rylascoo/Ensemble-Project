# E0-A Post-Audit Hardening — Grouped Native Windows ARM64 Validation Handoff

Status: **READY FOR DIRECTOR-MACHINE EXECUTION — PROVIDER GATE CLOSED**

## Purpose

Validate the exact final executable/test checkpoint produced by E0-A Phase B post-audit hardening after Patch Groups 1–6 and final static closure.

Exact executable/test checkout:

`d18ec637bb8881a38db9cfeb1420f093a994ac17`

Audited `main` baseline / expected `origin/main` for this validation handoff:

`f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25`

Hardening branch:

`e0a-phase-b-post-audit-hardening`

The branch contains documentation commits after the exact executable/test checkpoint. Therefore the validation intentionally checks out `d18ec637...` detached. Later documentation commits do not need to be executable-tested unless they change source, tests, fixtures, project configuration, or executable behavior.

This handoff is generated from `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md` and preserves its host rules.

## Validation scope

Required:

- exact Git authority and cleanliness;
- native Windows ARM64 environment probes;
- complete Core test project;
- complete Harness test project, including Groups 1–6 regressions;
- native ARM64 Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- credentialless explicit `e0a-run` gate;
- post-validation Git authority/cleanliness.

Explicitly prohibited:

- setting or using `OPENAI_API_KEY`;
- real provider inference;
- provider-network execution;
- provider spend;
- `e0a-evaluate` against synthetic live-provider output;
- merge to `main`.

## Director-machine command set

Run from the local `Ensemble-Project` repository in **Windows PowerShell**. Paste the complete block as one command set.

```powershell
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$ExpectedCheckout = 'd18ec637bb8881a38db9cfeb1420f093a994ac17'
$ExpectedOriginMain = 'f9cb1a79ad7923b640ff1a97c46d8fd6ab9dac25'

function Require([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

# --- Resolve repository and exact checkout ---
$RepoRoot = (git rev-parse --show-toplevel).Trim()
$GitRootExit = $LASTEXITCODE
Require ($GitRootExit -eq 0 -and -not [string]::IsNullOrWhiteSpace($RepoRoot)) 'Repository root resolution failed.'
Set-Location $RepoRoot

& git fetch origin
$FetchExit = $LASTEXITCODE
Require ($FetchExit -eq 0) 'git fetch origin failed.'

$OriginMain = (git rev-parse origin/main).Trim()
$OriginMainExit = $LASTEXITCODE
Require ($OriginMainExit -eq 0) 'origin/main resolution failed.'
Require ($OriginMain -eq $ExpectedOriginMain) "origin/main moved from the audited baseline. Expected $ExpectedOriginMain; observed $OriginMain. Stop and return to engineering review."

& git switch --detach $ExpectedCheckout
$SwitchExit = $LASTEXITCODE
Require ($SwitchExit -eq 0) 'Could not switch to the exact hardening executable/test checkpoint.'

$Head = (git rev-parse HEAD).Trim()
$HeadExit = $LASTEXITCODE
Require ($HeadExit -eq 0 -and $Head -eq $ExpectedCheckout) "Wrong validation HEAD: $Head"

& git diff --quiet
$TrackedExit = $LASTEXITCODE
Require ($TrackedExit -eq 0) 'Tracked working tree is not clean.'

& git diff --cached --quiet
$StagedExit = $LASTEXITCODE
Require ($StagedExit -eq 0) 'Staged index is not clean.'

$Untracked = @(& git ls-files --others --exclude-standard --full-name)
$UntrackedExit = $LASTEXITCODE
Require ($UntrackedExit -eq 0) 'Could not inspect untracked files.'
$MaterialUntracked = @($Untracked | Where-Object { $_ -match '^(src|tests|fixtures)/' })
Require ($MaterialUntracked.Count -eq 0) ('Material untracked files exist: ' + ($MaterialUntracked -join ', '))

Write-Host "VALIDATION_HEAD=$Head"
Write-Host "ORIGIN_MAIN=$OriginMain"
if ($Untracked.Count -gt 0) { Write-Host ('NONMATERIAL_UNTRACKED=' + ($Untracked -join ';')) }

# --- Trusted native Windows ARM64 probes for this host ---
Require ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') "PROCESSOR_ARCHITECTURE is '$($env:PROCESSOR_ARCHITECTURE)', expected ARM64."
Write-Host "PROCESSOR_ARCHITECTURE=$env:PROCESSOR_ARCHITECTURE"

$DotnetInfo = (& dotnet --info | Out-String)
$DotnetInfoExit = $LASTEXITCODE
Require ($DotnetInfoExit -eq 0) 'dotnet --info failed.'
Require ($DotnetInfo -match '(?m)^\s*RID:\s+win-arm64\s*$') 'dotnet --info did not report RID: win-arm64.'
Require ($DotnetInfo -match '(?m)^\s*Architecture:\s+arm64\s*$') 'dotnet --info did not report Host Architecture: arm64.'
Write-Host $DotnetInfo

# Keep the provider/credential gate closed for the entire validation.
Remove-Item Env:OPENAI_API_KEY -ErrorAction SilentlyContinue
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY must be absent.'

# --- Core test project ---
& dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug --nologo
$CoreTestExit = $LASTEXITCODE
Require ($CoreTestExit -eq 0) "Core tests failed with exit $CoreTestExit."
Write-Host "CORE_TEST_EXIT=$CoreTestExit"

# --- Harness test project ---
& dotnet test .\tests\Ensemble.E0.Harness.Tests\Ensemble.E0.Harness.Tests.csproj -c Debug --nologo
$HarnessTestExit = $LASTEXITCODE
Require ($HarnessTestExit -eq 0) "Harness tests failed with exit $HarnessTestExit."
Write-Host "HARNESS_TEST_EXIT=$HarnessTestExit"

# --- Native ARM64 Harness build ---
& dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug -r win-arm64 --nologo
$HarnessBuildExit = $LASTEXITCODE
Require ($HarnessBuildExit -eq 0) "Harness ARM64 build failed with exit $HarnessBuildExit."
$HarnessDll = Join-Path $RepoRoot 'src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll'
Require (Test-Path $HarnessDll) "Harness build output missing: $HarnessDll"
Write-Host "HARNESS_BUILD_EXIT=$HarnessBuildExit"
Write-Host "HARNESS_DLL=$HarnessDll"

# --- Fixture smokes ---
& dotnet $HarnessDll .\fixtures\missing-raft\missing-raft-0.1.0.json
$MissingRaftExit = $LASTEXITCODE
Require ($MissingRaftExit -eq 0) "Missing Raft smoke failed with exit $MissingRaftExit."
Write-Host "MISSING_RAFT_EXIT=$MissingRaftExit"

& dotnet $HarnessDll .\fixtures\smoke\e0-fixture-v1.json
$GenericSmokeExit = $LASTEXITCODE
Require ($GenericSmokeExit -eq 0) "Generic fixture smoke failed with exit $GenericSmokeExit."
Write-Host "GENERIC_SMOKE_EXIT=$GenericSmokeExit"

# --- Credentialless explicit e0a-run gate ---
# This invocation is expected to fail before evidence-root creation. Native stderr is
# isolated because this Director host can surface expected stderr as NativeCommandError.
$EvidenceRoot = Join-Path $env:TEMP ('ensemble-e0a-credentialless-' + [Guid]::NewGuid().ToString('N'))
$StdoutPath = Join-Path $env:TEMP ('ensemble-e0a-credentialless-out-' + [Guid]::NewGuid().ToString('N') + '.txt')
$StderrPath = Join-Path $env:TEMP ('ensemble-e0a-credentialless-err-' + [Guid]::NewGuid().ToString('N') + '.txt')
$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet $HarnessDll e0a-run CREATIVE-NONE .\fixtures\missing-raft\missing-raft-0.1.0.json E0A-NATIVE-CREDENTIALLESS $EvidenceRoot $ExpectedCheckout 1> $StdoutPath 2> $StderrPath
    $CredentiallessExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}

$CredentiallessStdout = if (Test-Path $StdoutPath) { Get-Content $StdoutPath -Raw } else { '' }
$CredentiallessStderr = if (Test-Path $StderrPath) { Get-Content $StderrPath -Raw } else { '' }
$CredentiallessCombined = $CredentiallessStdout + "`n" + $CredentiallessStderr

Require ($CredentiallessExit -eq 1) "Credentialless e0a-run exit was $CredentiallessExit, expected 1."
Require ($CredentiallessCombined.Contains('OPENAI_API_KEY is required at the E0-A provider edge.')) 'Credentialless e0a-run did not emit the required provider-edge refusal.'
Require (-not (Test-Path $EvidenceRoot)) 'Credentialless e0a-run created an evidence root.'
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY became present during validation.'

Write-Host "CREDENTIALLESS_EXIT=$CredentiallessExit"
Write-Host 'CREDENTIALLESS_MESSAGE_OK=true'
Write-Host 'CREDENTIALLESS_EVIDENCE_ROOT_ABSENT=true'
Write-Host 'OPENAI_API_KEY_ABSENT=true'

Remove-Item $StdoutPath,$StderrPath -Force -ErrorAction SilentlyContinue

# --- Post-validation authority / cleanliness ---
$PostHead = (git rev-parse HEAD).Trim()
$PostHeadExit = $LASTEXITCODE
Require ($PostHeadExit -eq 0 -and $PostHead -eq $ExpectedCheckout) "Post-validation HEAD changed: $PostHead"

& git diff --quiet
$PostTrackedExit = $LASTEXITCODE
Require ($PostTrackedExit -eq 0) 'Post-validation tracked working tree is not clean.'

& git diff --cached --quiet
$PostStagedExit = $LASTEXITCODE
Require ($PostStagedExit -eq 0) 'Post-validation staged index is not clean.'

$PostUntracked = @(& git ls-files --others --exclude-standard --full-name)
$PostUntrackedExit = $LASTEXITCODE
Require ($PostUntrackedExit -eq 0) 'Post-validation untracked inspection failed.'
$PostMaterialUntracked = @($PostUntracked | Where-Object { $_ -match '^(src|tests|fixtures)/' })
Require ($PostMaterialUntracked.Count -eq 0) ('Post-validation material untracked files exist: ' + ($PostMaterialUntracked -join ', '))

Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY must remain absent at validation end.'

Write-Host "POST_VALIDATION_HEAD=$PostHead"
Write-Host "POST_TRACKED_DIFF_EXIT=$PostTrackedExit"
Write-Host "POST_STAGED_DIFF_EXIT=$PostStagedExit"
Write-Host 'VALIDATION_COMMAND_SET_COMPLETE'
```

## Return evidence

Return the complete PowerShell output to the engineering chat. Do not summarize failures manually; preserve the exact failing command/output and native exit code.

The engineering surface will then:

1. classify compiler/runtime evidence without overstating it;
2. record exact Core/Harness test counts from native output;
3. record native ARM64/build/fixture/credentialless results;
4. create the grouped native-validation evidence record if and only if the command set passes;
5. update `CURRENT_STATE.md`;
6. review PR #39 for promotion readiness without opening the provider gate.

## Gate after a PASS

A PASS would authorize only promotion/review of the hardened fake-only/credentialless E0-A executable/test surface.

It would **not** authorize `OPENAI_API_KEY`, real provider requests, provider-network execution, spend, E0-B..G, product persistence/UI, Windows AI/NPU, MSIX/WACK, Store/Partner Center, or later ODR scope.
