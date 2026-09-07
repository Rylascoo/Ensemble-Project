# E0-A Post-Audit Hardening — Native Windows ARM64 Rerun 02 Handoff

Status: **READY FOR DIRECTOR-MACHINE EXECUTION — PROVIDER GATE CLOSED**

Date: **2026-09-07**

## Purpose

Run the complete native Windows ARM64 validation sequence from the beginning against the repaired hardening source/test checkpoint after Attempt 01 exposed a stale diagnostic test oracle.

Exact source/test checkout:

`5c70f619d6e951d89bb527a5945b014998573dab`

Promoted `main` expected when this packet was generated:

`67de40f4b76075dbf7feb3ad00aee8ed7f0bbbed`

Hardening branch:

`e0a-phase-b-post-audit-hardening`

Attempt 01 evidence and diagnosis:

`docs/evidence/E0A_POST_AUDIT_HARDENING_NATIVE_ARM64_VALIDATION_ATTEMPT_01.md`

Attempt 01 proved the earlier MSTEST0032 compiler blocker closed but failed one of 88 Harness tests because `BufferedWrongJsonType_FailsClosedAsTechnicalReceipt` retained the older E-03 `provider-incomplete` diagnostic oracle after E-06 had intentionally refined malformed response handling to `malformed-provider-response`. Repair `5c70f619...` changes that one expected test string only; no product source or runtime behavior changed.

A focused rerun of the one failed test is insufficient. This packet re-runs every native gate from the beginning.

## Validation scope

Required:

- exact Git checkout and clean tracked/staged state;
- no material untracked files under `src/`, `tests/`, or `fixtures/`;
- trusted Windows ARM64 host probes;
- complete Core native test project;
- complete Harness native test project;
- fresh clean-output native ARM64 Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- credentialless explicit OpenAI-edge `e0a-run` refusal;
- no evidence root from the credentialless refusal;
- post-validation exact checkout and cleanliness.

Explicitly prohibited:

- setting or using `OPENAI_API_KEY`;
- setting or using `GEMINI_API_KEY`;
- provider-network execution;
- provider inference;
- provider spend;
- merge to `main`.

## Windows PowerShell command set

Run from the local `Ensemble-Project` repository. Preserve complete output.

```powershell
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$ExpectedCheckout = '5c70f619d6e951d89bb527a5945b014998573dab'
$ExpectedOriginMain = '67de40f4b76075dbf7feb3ad00aee8ed7f0bbbed'

function Require([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

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
Require ($OriginMain -eq $ExpectedOriginMain) "origin/main moved. Expected $ExpectedOriginMain; observed $OriginMain. Stop and return to engineering review."
Write-Host "ORIGIN_MAIN=$OriginMain"

& git switch --detach $ExpectedCheckout
$SwitchExit = $LASTEXITCODE
Require ($SwitchExit -eq 0) 'Could not switch to the exact hardening rerun checkpoint.'

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
if ($Untracked.Count -gt 0) { Write-Host ('NONMATERIAL_UNTRACKED=' + ($Untracked -join ';')) }

Require ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') "PROCESSOR_ARCHITECTURE is '$($env:PROCESSOR_ARCHITECTURE)', expected ARM64."
Write-Host "PROCESSOR_ARCHITECTURE=$env:PROCESSOR_ARCHITECTURE"

$DotnetInfo = (& dotnet --info | Out-String)
$DotnetInfoExit = $LASTEXITCODE
Require ($DotnetInfoExit -eq 0) 'dotnet --info failed.'
Require ($DotnetInfo -match '(?m)^\s*RID:\s+win-arm64\s*$') 'dotnet --info did not report RID: win-arm64.'
Require ($DotnetInfo -match '(?m)^\s*Architecture:\s+arm64\s*$') 'dotnet --info did not report Host Architecture: arm64.'
Write-Host $DotnetInfo

Remove-Item Env:OPENAI_API_KEY -ErrorAction SilentlyContinue
Remove-Item Env:GEMINI_API_KEY -ErrorAction SilentlyContinue
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY must be absent.'
Require (-not (Test-Path Env:GEMINI_API_KEY)) 'GEMINI_API_KEY must be absent.'

# Clear Harness/test outputs before native test execution so the test rung cannot
# depend on stale binaries from a prior checkout.
$HarnessBinRoot = Join-Path $RepoRoot 'src\Ensemble.E0.Harness\bin'
$HarnessObjRoot = Join-Path $RepoRoot 'src\Ensemble.E0.Harness\obj'
$HarnessTestsBinRoot = Join-Path $RepoRoot 'tests\Ensemble.E0.Harness.Tests\bin'
$HarnessTestsObjRoot = Join-Path $RepoRoot 'tests\Ensemble.E0.Harness.Tests\obj'
Remove-Item $HarnessBinRoot,$HarnessObjRoot,$HarnessTestsBinRoot,$HarnessTestsObjRoot -Recurse -Force -ErrorAction SilentlyContinue

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug --nologo
    $CoreTestExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}
Require ($CoreTestExit -eq 0) "Core tests failed with exit $CoreTestExit."
Write-Host "CORE_TEST_EXIT=$CoreTestExit"

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet test .\tests\Ensemble.E0.Harness.Tests\Ensemble.E0.Harness.Tests.csproj -c Debug --nologo
    $HarnessTestExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}
Require ($HarnessTestExit -eq 0) "Harness tests failed with exit $HarnessTestExit."
Write-Host "HARNESS_TEST_EXIT=$HarnessTestExit"

# Clear the target Harness output again. Fixture/credentialless smokes count only
# against the immediately following successful explicit native build.
$HarnessTargetBin = Join-Path $RepoRoot 'src\Ensemble.E0.Harness\bin\Debug\net9.0\win-arm64'
$HarnessTargetObj = Join-Path $RepoRoot 'src\Ensemble.E0.Harness\obj\Debug\net9.0\win-arm64'
Remove-Item $HarnessTargetBin,$HarnessTargetObj -Recurse -Force -ErrorAction SilentlyContinue
Require (-not (Test-Path $HarnessTargetBin)) 'Previous target Harness output was not removed.'

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug -r win-arm64 --nologo
    $HarnessBuildExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}
Require ($HarnessBuildExit -eq 0) "Harness native ARM64 build failed with exit $HarnessBuildExit."

$HarnessDll = Join-Path $HarnessTargetBin 'Ensemble.E0.Harness.dll'
Require (Test-Path $HarnessDll) "Fresh Harness build output missing: $HarnessDll"
Write-Host "HARNESS_BUILD_EXIT=$HarnessBuildExit"
Write-Host "HARNESS_DLL=$HarnessDll"

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet $HarnessDll .\fixtures\missing-raft\missing-raft-0.1.0.json
    $MissingRaftExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}
Require ($MissingRaftExit -eq 0) "Missing Raft smoke failed with exit $MissingRaftExit."
Write-Host "MISSING_RAFT_EXIT=$MissingRaftExit"

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet $HarnessDll .\fixtures\smoke\e0-fixture-v1.json
    $GenericSmokeExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}
Require ($GenericSmokeExit -eq 0) "Generic fixture smoke failed with exit $GenericSmokeExit."
Write-Host "GENERIC_SMOKE_EXIT=$GenericSmokeExit"

$EvidenceRoot = Join-Path $env:TEMP ('ensemble-e0a-hardening-rerun02-' + [Guid]::NewGuid().ToString('N'))
$StdoutPath = Join-Path $env:TEMP ('ensemble-e0a-hardening-rerun02-out-' + [Guid]::NewGuid().ToString('N') + '.txt')
$StderrPath = Join-Path $env:TEMP ('ensemble-e0a-hardening-rerun02-err-' + [Guid]::NewGuid().ToString('N') + '.txt')
$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet $HarnessDll e0a-run CREATIVE-NONE .\fixtures\missing-raft\missing-raft-0.1.0.json E0A-HARDENING-RERUN02-CREDENTIALLESS $EvidenceRoot $ExpectedCheckout 1> $StdoutPath 2> $StderrPath
    $CredentiallessExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}

$CredentiallessStdout = if (Test-Path $StdoutPath) { Get-Content $StdoutPath -Raw } else { '' }
$CredentiallessStderr = if (Test-Path $StderrPath) { Get-Content $StderrPath -Raw } else { '' }
$CredentiallessCombined = $CredentiallessStdout + "`n" + $CredentiallessStderr

Require ($CredentiallessExit -eq 1) "Credentialless e0a-run exit was $CredentiallessExit, expected 1."
Require ($CredentiallessCombined.Contains('OPENAI_API_KEY is required at the E0-A provider edge.')) 'Credentialless e0a-run did not emit the required OpenAI provider-edge refusal.'
Require (-not (Test-Path $EvidenceRoot)) 'Credentialless e0a-run created an evidence root.'
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY became present during validation.'
Require (-not (Test-Path Env:GEMINI_API_KEY)) 'GEMINI_API_KEY became present during validation.'

Write-Host "CREDENTIALLESS_EXIT=$CredentiallessExit"
Write-Host 'CREDENTIALLESS_MESSAGE_OK=true'
Write-Host 'CREDENTIALLESS_EVIDENCE_ROOT_ABSENT=true'
Write-Host 'OPENAI_API_KEY_ABSENT=true'
Write-Host 'GEMINI_API_KEY_ABSENT=true'

Remove-Item $StdoutPath,$StderrPath -Force -ErrorAction SilentlyContinue

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

Write-Host "POST_VALIDATION_HEAD=$PostHead"
Write-Host "POST_TRACKED_DIFF_EXIT=$PostTrackedExit"
Write-Host "POST_STAGED_DIFF_EXIT=$PostStagedExit"
Write-Host 'HARDENING_NATIVE_RERUN_02_COMPLETE'
```

## Evidence return

Return the complete PowerShell output to the engineering chat. Do not summarize or repair failures locally.

A PASS authorizes only the next engineering review/promotion decision for the hardened fake-only/credentialless surface. It does not authorize provider credentials, network inference, provider spend, E0-B+, product UI/persistence, NPU, packaging, WACK, or Store work.
