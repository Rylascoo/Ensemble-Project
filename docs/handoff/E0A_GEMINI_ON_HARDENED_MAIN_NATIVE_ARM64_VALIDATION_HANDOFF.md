# E0-A Gemini on Hardened Main — Native Windows ARM64 Validation Handoff

Status: **READY FOR DIRECTOR-MACHINE EXECUTION — PROVIDER NETWORK GATE CLOSED**

Date: **2026-09-07**

## Validation target

Repository: `Rylascoo/Ensemble-Project`

Integration branch: `e0a-gemini-on-hardened-main`

Exact executable/test checkout to validate:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Expected promoted `origin/main` when this packet was generated:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Static closure:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`

The target passed GitHub Actions ARM64-target cross-compile/compiler validation with 0 warnings and 0 errors in all four project builds. That is compiler authority only, not native Windows ARM64 runtime authority.

The integration branch contains documentation commits after the exact executable/test checkpoint. Validation intentionally checks out `5f286e8c...` detached. Those later documentation-only commits do not alter source, tests, fixtures, project configuration, or executable behavior.

## Native scope

Required:

- exact checkout and clean tracked/staged state;
- no material untracked files under `src/`, `tests/`, or `fixtures/`;
- trusted Windows ARM64 host probes;
- complete native Core tests;
- complete native Harness tests;
- fresh clean-output native ARM64 Harness build;
- Missing Raft fixture smoke;
- generic fixture smoke;
- credentialless explicit Gemini `e0a-run CREATIVE-NONE` expected refusal;
- absent evidence root and absent provider credentials;
- post-validation exact checkout and cleanliness.

Explicitly prohibited:

- setting or using `OPENAI_API_KEY`;
- setting or using `GEMINI_API_KEY`;
- Gemini `countTokens` network execution;
- provider inference;
- provider-network execution;
- provider spend;
- merge/promotion based on partial output.

## Host apparatus authority

This packet is generated from:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

It preserves the known Director-host rules:

- trust `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` (`RID: win-arm64`, Host `Architecture: arm64`), not the known blank PowerShell `RuntimeInformation` probes;
- capture `$LASTEXITCODE` immediately after native commands used as oracles;
- use temporary `$ErrorActionPreference='Continue'` around native commands whose stderr must not be promoted into a PowerShell wrapper failure;
- clear Harness/test outputs before native test execution;
- clear target Harness output again before the explicit native build;
- hard-stop all executable smokes if that exact-checkout build fails;
- keep complete interactive `if ... else ...` expressions in one submitted statement.

## Director-machine Windows PowerShell command set

Run from the local `Ensemble-Project` repository. Paste the complete block as one command set and preserve all output.

```powershell
$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$ExpectedCheckout = '5f286e8cfa896d38d85d4611f69a224fae5b55fd'
$ExpectedOriginMain = 'aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd'

function Require([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

# --- Repository and exact checkout authority ---
$RepoRoot = (git rev-parse --show-toplevel).Trim()
$GitRootExit = $LASTEXITCODE
Require ($GitRootExit -eq 0 -and -not [string]::IsNullOrWhiteSpace($RepoRoot)) 'Repository root resolution failed.'
Set-Location $RepoRoot

& git fetch origin --tags
$FetchExit = $LASTEXITCODE
Require ($FetchExit -eq 0) 'git fetch origin --tags failed.'

$OriginMain = (git rev-parse origin/main).Trim()
$OriginMainExit = $LASTEXITCODE
Require ($OriginMainExit -eq 0) 'origin/main resolution failed.'
Require ($OriginMain -eq $ExpectedOriginMain) "origin/main moved. Expected $ExpectedOriginMain; observed $OriginMain. Stop and return to engineering review."
Write-Host "ORIGIN_MAIN=$OriginMain"

& git switch --detach $ExpectedCheckout
$SwitchExit = $LASTEXITCODE
Require ($SwitchExit -eq 0) 'Could not switch to the exact combined Gemini executable/test checkpoint.'

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

# --- Trusted native Windows ARM64 probes ---
Require ($env:PROCESSOR_ARCHITECTURE -eq 'ARM64') "PROCESSOR_ARCHITECTURE is '$($env:PROCESSOR_ARCHITECTURE)', expected ARM64."
Write-Host "PROCESSOR_ARCHITECTURE=$env:PROCESSOR_ARCHITECTURE"
Write-Host ('WINDOWS_VERSION=' + [Environment]::OSVersion.VersionString)

$DotnetInfo = (& dotnet --info | Out-String)
$DotnetInfoExit = $LASTEXITCODE
Require ($DotnetInfoExit -eq 0) 'dotnet --info failed.'
Require ($DotnetInfo -match '(?m)^\s*RID:\s+win-arm64\s*$') 'dotnet --info did not report RID: win-arm64.'
Require ($DotnetInfo -match '(?m)^\s*Architecture:\s+arm64\s*$') 'dotnet --info did not report Host Architecture: arm64.'
Write-Host $DotnetInfo

# Keep all provider gates closed for the entire validation.
Remove-Item Env:OPENAI_API_KEY -ErrorAction SilentlyContinue
Remove-Item Env:GEMINI_API_KEY -ErrorAction SilentlyContinue
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY must be absent.'
Require (-not (Test-Path Env:GEMINI_API_KEY)) 'GEMINI_API_KEY must be absent.'

# --- Clear relevant build/test outputs before native tests ---
$OutputRoots = @(
    (Join-Path $RepoRoot 'src\Ensemble.E0.Harness\bin'),
    (Join-Path $RepoRoot 'src\Ensemble.E0.Harness\obj'),
    (Join-Path $RepoRoot 'tests\Ensemble.E0.Harness.Tests\bin'),
    (Join-Path $RepoRoot 'tests\Ensemble.E0.Harness.Tests\obj')
)
Remove-Item $OutputRoots -Recurse -Force -ErrorAction SilentlyContinue

# --- Complete native Core tests ---
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

# --- Complete native Harness tests ---
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

# --- Fresh native ARM64 Harness build ---
# The fixture/provider-edge smokes count only against the immediately following
# successful explicit build, never an older DLL left by a prior checkout.
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

# --- Missing Raft fixture smoke ---
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

# --- Generic fixture smoke ---
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

# --- Credentialless explicit Gemini provider-edge gate ---
# Expected to fail before evidence-root creation. Native stderr is isolated because
# this Director host can surface expected stderr as NativeCommandError.
$EvidenceRoot = Join-Path $env:TEMP ('ensemble-e0a-gemini-hardened-' + [Guid]::NewGuid().ToString('N'))
$StdoutPath = Join-Path $env:TEMP ('ensemble-e0a-gemini-hardened-out-' + [Guid]::NewGuid().ToString('N') + '.txt')
$StderrPath = Join-Path $env:TEMP ('ensemble-e0a-gemini-hardened-err-' + [Guid]::NewGuid().ToString('N') + '.txt')
Require (-not (Test-Path $EvidenceRoot)) 'Fresh credentialless evidence root unexpectedly already exists.'

$PriorEap = $ErrorActionPreference
try {
    $ErrorActionPreference = 'Continue'
    & dotnet $HarnessDll e0a-run CREATIVE-NONE .\fixtures\missing-raft\missing-raft-0.1.0.json E0A-GEMINI-HARDENED-CREDENTIALLESS $EvidenceRoot $ExpectedCheckout 1> $StdoutPath 2> $StderrPath
    $CredentiallessExit = $LASTEXITCODE
}
finally {
    $ErrorActionPreference = $PriorEap
}

$CredentiallessStdout = if (Test-Path $StdoutPath) { Get-Content $StdoutPath -Raw } else { '' }
$CredentiallessStderr = if (Test-Path $StderrPath) { Get-Content $StderrPath -Raw } else { '' }
$CredentiallessCombined = $CredentiallessStdout + "`n" + $CredentiallessStderr

Require ($CredentiallessExit -eq 1) "Credentialless e0a-run exit was $CredentiallessExit, expected 1."
Require ($CredentiallessCombined.Contains('GEMINI_API_KEY is required at the E0-A provider edge.')) 'Credentialless e0a-run did not emit the required Gemini provider-edge refusal.'
Require (-not (Test-Path $EvidenceRoot)) 'Credentialless e0a-run created an evidence root.'
Require (-not (Test-Path Env:OPENAI_API_KEY)) 'OPENAI_API_KEY became present during validation.'
Require (-not (Test-Path Env:GEMINI_API_KEY)) 'GEMINI_API_KEY became present during validation.'

Write-Host "CREDENTIALLESS_EXIT=$CredentiallessExit"
Write-Host 'CREDENTIALLESS_MESSAGE_OK=true'
Write-Host 'CREDENTIALLESS_EVIDENCE_ROOT_ABSENT=true'
Write-Host 'OPENAI_API_KEY_ABSENT=true'
Write-Host 'GEMINI_API_KEY_ABSENT=true'
Write-Host 'CREDENTIALLESS_STDOUT_BEGIN'
Write-Host $CredentiallessStdout
Write-Host 'CREDENTIALLESS_STDOUT_END'
Write-Host 'CREDENTIALLESS_STDERR_BEGIN'
Write-Host $CredentiallessStderr
Write-Host 'CREDENTIALLESS_STDERR_END'

Remove-Item $StdoutPath,$StderrPath -Force -ErrorAction SilentlyContinue

# --- Post-validation checkout / cleanliness authority ---
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
Require (-not (Test-Path Env:GEMINI_API_KEY)) 'GEMINI_API_KEY must remain absent at validation end.'

Write-Host "POST_VALIDATION_HEAD=$PostHead"
Write-Host "POST_TRACKED_DIFF_EXIT=$PostTrackedExit"
Write-Host "POST_STAGED_DIFF_EXIT=$PostStagedExit"
Write-Host 'GEMINI_ON_HARDENED_NATIVE_VALIDATION_COMPLETE'
```

## Required classification after a complete PASS

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS
Harness tests                            PASS
Fresh native ARM64 Harness build         PASS
Missing Raft smoke                       PASS
Generic fixture smoke                    PASS
Credentialless Gemini provider edge      PASS expected refusal
Evidence-root absence                    PASS
Gemini credential use                    NOT PERFORMED
Gemini token-count network request        NOT PERFORMED
Gemini inference                          NOT PERFORMED
Gemini provider-network execution         NOT PERFORMED
Gemini spend                              NOT PERFORMED
Overall credentialless native validation PASS
```

Do not infer live Gemini API compatibility, model availability, account quota/tier, pricing/data-use freshness, cache behavior, provider usage accounting, or inference correctness from this validation.

## Evidence return

Return the complete PowerShell output to the engineering chat. Do not summarize or repair a failure locally; preserve the exact failing command/output and native exit status.

Engineering will classify the exact test counts and runtime evidence, file the Director-machine evidence if and only if the complete packet passes, then issue the Repository Surface annotated-tag command.

## Repository Surface gate after PASS

A successful native validation must be followed by an annotated validation tag at exactly:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

The tag message must record validation level, fake-only/credentialless scope, and the resulting evidence-document path. Only after that durable tag exists may `CURRENT_STATE.md` promote the combined checkpoint as machine-validated runtime authority.

A PASS does **not** authorize a Gemini credential, `countTokens`, inference, provider-network execution, spend, E0-B+, product UI/persistence, NPU, packaging/WACK, or Store work.
