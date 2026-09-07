# E0-A Pre-Restructure Closure — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS — CREDENTIALLESS / FAKE-ONLY SCOPE; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Date: **2026-09-07**

## Exact validated checkout

`cc395a25162a0a682796bffb44060c799df0db32`

Validation branch:

`repo-pre-restructure-closure`

Observed `origin/main` baseline:

`64ca69be681c0f549f94bc2d1038eb76bfd28884`

The validated checkout includes the pre-restructure closure package: completed E0-A handoff reconciliation, required x64 Core regression CI, removal of superseded OpenAI executable/test lineage, deterministic documentation-reference census, and archival of superseded E0-A transition evidence.

## Checkout and working-tree authority

Observed before native execution:

- exact detached HEAD: `cc395a25162a0a682796bffb44060c799df0db32`;
- `origin/main`: `64ca69be681c0f549f94bc2d1038eb76bfd28884`, exactly matching the expected baseline;
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

PASS.

## Provider gate state

Before test/build/provider-edge validation:

- `OPENAI_API_KEY` removed and asserted absent;
- `GEMINI_API_KEY` removed and asserted absent.

No provider credential was supplied during validation.

## Native test and build results

Core tests:

```text
total:     622
succeeded: 622
failed:    0
skipped:   0
exit:      0
```

Harness tests:

```text
total:     103
succeeded: 103
failed:    0
skipped:   0
exit:      0
```

The prior composed Gemini-on-hardened checkpoint recorded 125 Harness tests. The reduction to 103 was audited against the pre-restructure source/test delta before this validation was classified. The removed coverage belongs to the deliberately retired OpenAI transport lineage and OpenAI-specific live-host/readiness compatibility surface. Provider-neutral invariants were retained on the active Gemini/fake path, including configured request/receipt identity, spend and reservation behavior, evidence authority, cancellation, malformed provider handling, streaming closure, and deterministic run behavior. The lower count is therefore an intentional deletion result, not an unexplained validation regression.

Fresh explicit native ARM64 Harness build:

```text
configuration: Debug
RID:           win-arm64
exit:          0
```

Fresh output existed at `src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64/Ensemble.E0.Harness.dll` only after target output was cleared and the exact-checkout build returned success.

PASS.

## Fixture smokes

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

The exact-checkout host executed explicit `e0a-run CREATIVE-NONE` with both provider credentials absent and a fresh nonexistent evidence root.

Expected-failure gate:

```text
native exit                    1
required refusal               GEMINI_API_KEY is required at the E0-A provider edge.
evidence root                  absent
OPENAI_API_KEY                 absent
GEMINI_API_KEY                 absent
```

PASS.

The Director host presented native stderr as PowerShell `NativeCommandError`, matching the established host-apparatus behavior. That presentation does not invalidate the result because stderr was isolated, `$LASTEXITCODE` was captured immediately, and exit code, exact refusal text, evidence-root absence, and credential absence were asserted independently.

No `countTokens` request, provider inference, provider-network execution, or spend occurred.

## Post-validation authority

Observed after all native steps:

- HEAD remained exactly `cc395a25162a0a682796bffb44060c799df0db32`;
- tracked diff exit `0`;
- staged diff exit `0`;
- no material untracked file under `src/`, `tests/`, or `fixtures/`;
- both provider credentials remained absent.

PASS.

## Classification

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS 622/622
Harness tests                            PASS 103/103
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

The complete Director-machine transcript was checked against `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`, the exact repository diff from the previously promoted runtime checkpoint, and the intended pre-restructure closure boundary.

The audit found no material anomaly that invalidates the PASS:

- exact checkout and expected `origin/main` matched;
- source/test/build outputs were cleared before native execution;
- both test suites completed with zero failures;
- the Harness executable was rebuilt fresh for `win-arm64` before fixture/provider-edge smokes;
- the 22-test Harness count reduction is explained by intentional removal of superseded OpenAI-specific coverage rather than a failed or skipped test condition;
- both provider credentials remained absent;
- the expected Gemini refusal was verified by exit, exact message, and filesystem effects;
- post-validation checkout and tracked/staged cleanliness remained exact.

One complete recursive audit pass found no further material correction, inconsistency, ambiguity, regression, or worthwhile improvement inside this validation boundary.

## Validation boundary

This PASS establishes native compiler/runtime authority only for exact checkout `cc395a25162a0a682796bffb44060c799df0db32` and the credentialless/fake-only command scope above. It does **not** establish or authorize:

- live Gemini API compatibility;
- successful `models.countTokens` network behavior;
- model/project/account availability or quota;
- current provider pricing or data-use terms;
- live implicit-cache behavior;
- provider usage-accounting correctness under real responses;
- inference quality/correctness;
- provider spend;
- E0-B or later product scope;
- Windows AI/NPU authority;
- packaging, WACK, or Store certification.

## Repository Surface gate

Before `CURRENT_STATE.md` may promote this checkout as durable machine-tested runtime authority, Repository Surface Law 5 requires an annotated validation tag pointing exactly to:

`cc395a25162a0a682796bffb44060c799df0db32`

The tag must record the validation level, credentialless/fake-only scope, and this evidence path.
