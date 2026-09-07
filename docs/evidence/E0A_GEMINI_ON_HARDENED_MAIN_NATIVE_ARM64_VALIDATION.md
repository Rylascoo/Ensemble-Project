# E0-A Gemini on Hardened Main — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS — CREDENTIALLESS / FAKE-ONLY SCOPE; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Date: **2026-09-07**

This record preserves the Director-machine native Windows ARM64 validation of the semantically composed Gemini-on-hardened E0-A Phase B executable/test checkpoint. It establishes native compiler/runtime authority only for the exact checkout and command scope recorded below. It does not authorize a Gemini credential, `countTokens`, inference, provider-network execution, or spend.

## Exact validated checkout

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Audited promoted `origin/main` observed by the validation command set:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Integration branch:

`e0a-gemini-on-hardened-main`

Static closure:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`

Validation command authority:

`docs/handoff/E0A_GEMINI_ON_HARDENED_MAIN_NATIVE_ARM64_VALIDATION_HANDOFF.md`

## Checkout and working-tree authority

Observed before native execution:

- exact detached HEAD: `5f286e8cfa896d38d85d4611f69a224fae5b55fd`;
- `origin/main`: `aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`, exactly matching the audited expected baseline;
- tracked working tree clean;
- staged index clean;
- no material untracked file under `src/`, `tests/`, or `fixtures/`;
- only observed nonmaterial untracked file: root-level `patch0012-local-edit.txt`.

PASS.

## Native Windows ARM64 host authority

Trusted host probes observed:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- Windows `10.0.26200`;
- repository-selected .NET SDK `9.0.317`;
- `dotnet --info` RID `win-arm64`;
- .NET Host Architecture `arm64`.

The installed .NET 10 SDK/runtime was visible on the machine but was not selected by the repository build. Repository `global.json` selected SDK `9.0.317`.

PASS.

## Provider gate state

Before test/build/provider-edge validation:

- `OPENAI_API_KEY` removed and asserted absent;
- `GEMINI_API_KEY` removed and asserted absent.

No provider credential was supplied at any point in this validation.

## Core tests

Native Windows ARM64 execution PASS:

```text
total:     622
succeeded: 622
failed:    0
skipped:   0
exit:      0
```

## Harness tests

Before Harness-native test execution, prior Harness and Harness-test `bin`/`obj` outputs were deleted so the run could not inherit executable/test binaries from an earlier checkout.

Native Windows ARM64 execution PASS:

```text
total:     125
succeeded: 125
failed:    0
skipped:   0
exit:      0
```

The combined Harness test count is larger than either prior independent hardening or original-Gemini checkpoint because it includes the composed Gemini surface plus dedicated hardening-preservation and cross-branch regression coverage.

## Fresh native ARM64 Harness build

After test execution, the target-specific Harness output was deleted again:

- `src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64`;
- corresponding target-specific `obj` output.

The validation asserted the prior target output was absent before rebuilding.

Fresh explicit build:

```text
configuration: Debug
RID:           win-arm64
exit:          0
```

Fresh DLL present at:

`src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64/Ensemble.E0.Harness.dll`

PASS.

## Fixture smokes

Executed only from the freshly built exact-checkout Harness DLL.

Missing Raft:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
exit: 0
```

Generic smoke:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
exit: 0
```

Both PASS.

## Credentialless Gemini provider-edge gate

The explicit live-host command used:

- `e0a-run CREATIVE-NONE`;
- exact executable checkout identity `5f286e8cfa896d38d85d4611f69a224fae5b55fd`;
- approved Missing Raft fixture;
- fresh RunId `E0A-GEMINI-HARDENED-CREDENTIALLESS`;
- a fresh nonexistent temporary evidence root;
- the immediately preceding fresh native ARM64 Harness build;
- both provider credentials absent.

Expected-failure gate PASS:

```text
native exit                    1
required refusal               GEMINI_API_KEY is required at the E0-A provider edge.
evidence root                  absent
OPENAI_API_KEY                 absent
GEMINI_API_KEY                 absent
```

Native stderr surfaced through the known PowerShell `NativeCommandError` presentation on this host. That presentation is non-defective here because stderr was intentionally isolated, `$LASTEXITCODE` was captured immediately, and the command set independently asserted the required exit code, exact refusal text, absent evidence root, and absent credentials.

This proves only that the exact compiled native host reaches the Gemini credential boundary and fails closed before evidence creation when the credential is absent. It is not evidence of `countTokens`, provider-network compatibility, inference, model availability, provider usage accounting, or spend.

## Post-validation authority

Observed after all native steps:

- HEAD remained exactly `5f286e8cfa896d38d85d4611f69a224fae5b55fd`;
- tracked diff exit `0`;
- staged diff exit `0`;
- no material untracked files under `src/`, `tests/`, or `fixtures/`;
- `OPENAI_API_KEY` absent;
- `GEMINI_API_KEY` absent.

PASS.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness tests                            PASS 125/125
Fresh native ARM64 Harness build         PASS
Missing Raft smoke                       PASS
Generic fixture smoke                    PASS
Credentialless Gemini provider edge      PASS expected refusal
Evidence-root absence                    PASS
Post-validation checkout/cleanliness     PASS
OpenAI credential use                    NOT PERFORMED
Gemini credential use                    NOT PERFORMED
Gemini token-count network request        NOT PERFORMED
Gemini inference                          NOT PERFORMED
Gemini provider-network execution         NOT PERFORMED
Gemini spend                              NOT PERFORMED
Overall credentialless native validation PASS
```

## Recursive validation audit

The returned Director-machine transcript was checked against the authoritative validation handoff and host-behavior contract.

No material anomaly invalidates the PASS:

- exact checkout and `origin/main` baseline matched;
- stale Harness/test outputs were cleared before native test execution;
- target Harness output was cleared again before the authoritative explicit native build;
- fixture and provider-edge smokes therefore ran only from the fresh current-checkout build;
- both provider credentials remained absent throughout;
- the expected Gemini refusal was verified by exit, exact message, and filesystem effects rather than by stderr presentation alone;
- post-validation checkout and tracked/staged cleanliness remained exact;
- the known root-level untracked file is outside `src/`, `tests/`, and `fixtures/` and did not participate in executable/test authority.

One complete audit pass found no material correction, inconsistency, ambiguity, or worthwhile improvement to the classification.

## Validation boundary

This PASS does **not** establish:

- live Gemini API compatibility;
- successful `models.countTokens` network behavior;
- current Gemini model/project availability;
- account tier, quota, or key type;
- current provider pricing or data-use terms;
- implicit-cache live behavior;
- provider usage-accounting correctness under real responses;
- inference quality/correctness;
- provider spend;
- Windows AI/NPU authority;
- MSIX/WACK or Store certification.

Those remain separately gated.

## Repository Surface gate

Before `CURRENT_STATE.md` may promote this checkout as durable machine-tested runtime authority, Repository Surface Law 5 requires an annotated validation tag at exactly:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

The tag must record validation level, credentialless/fake-only scope, and this evidence path.
