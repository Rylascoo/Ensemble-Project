# E0-A Gemini Comparison — Native Windows ARM64 Validation

Status: **DIRECTOR-MACHINE-SOURCED — PASS — CREDENTIALLESS / FAKE-ONLY SCOPE; REAL GEMINI NETWORK EXECUTION NOT AUTHORIZED**

Date: **2026-09-07**

## Exact validated checkout

`c9f706b42350c8b6cfc462e09cf71db6bc3a2355`

Validation branch at execution time:

`e0a-gemini-rate-discipline-model-comparison`

Observed `origin/main` baseline:

`7490de24bfd2a9829f6afc1ae4b3831c98c50837`

The checkout contains the approved rate-discipline/model-comparison implementation, Gemini 3.5 thought-signature compatibility correction, regression coverage, and the documentation state used to construct the native packet.

This evidence record does not by itself promote validation authority. Repository Surface Law requires an annotated validation tag at the exact validated checkout before `CURRENT_STATE.md` may first name it as machine-validated.

## Checkout and working-tree authority

Observed before native execution:

- exact detached HEAD: `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`;
- `origin/main`: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`, exactly matching the expected baseline;
- tracked working tree clean;
- staged index clean;
- no material untracked file under `src/`, `tests/`, or `fixtures/`.

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

- `OPENAI_API_KEY` absent;
- `GEMINI_API_KEY` absent.

No provider credential was supplied during validation.

## Native test and build results

Core tests:

```text
total:     622
succeeded: 622
failed:    0
skipped:   0
```

Harness tests:

```text
total:     117
succeeded: 117
failed:    0
skipped:   0
```

The Harness count increased from the previously promoted pre-comparison native checkpoint because the comparison/rate-discipline and Gemini-signature work added active regression coverage. No failure or skip was observed.

Fresh explicit native ARM64 Harness build:

```text
configuration: Debug
RID:           win-arm64
result:        PASS
```

Fresh output existed at:

`src/Ensemble.E0.Harness/bin/Debug/net9.0/win-arm64/Ensemble.E0.Harness.dll`

PASS.

## Fixture smokes

Missing Raft:

```text
Fixture validated: ensemble.e0.missing-raft@0.1.0
```

Generic smoke:

```text
Fixture validated: ensemble.e0.smoke@0.1.0
```

Both PASS.

## Credentialless three-profile provider-edge gate

With both provider credentials absent, the exact-checkout host invoked all three approved arm/profile pairings using distinct nonexistent evidence roots.

### `GEMINI-2.5-FLASH-LITE-NONE`

```text
arm:            CREATIVE-NONE
native exit:    1
required text:  GEMINI_API_KEY is required at the E0-A provider edge.
evidence root:  absent
```

PASS expected refusal.

### `GEMINI-3.5-FLASH-LITE-MINIMAL`

```text
arm:            CREATIVE-MINIMAL
native exit:    1
required text:  GEMINI_API_KEY is required at the E0-A provider edge.
evidence root:  absent
```

PASS expected refusal.

### `GEMINI-2.5-FLASH-NONE`

```text
arm:            CREATIVE-NONE
native exit:    1
required text:  GEMINI_API_KEY is required at the E0-A provider edge.
evidence root:  absent
```

PASS expected refusal.

PowerShell presented native stderr as `NativeCommandError`, matching the established Director-host behavior. This presentation does not invalidate the gate because the native exit code, exact refusal text, and evidence-root absence were asserted independently for every profile.

## Post-validation authority

Observed after all native steps:

- HEAD remained exactly `c9f706b42350c8b6cfc462e09cf71db6bc3a2355`;
- tracked/staged/material-untracked cleanliness remained PASS;
- `OPENAI_API_KEY` remained absent;
- `GEMINI_API_KEY` remained absent.

PASS.

## Classification

```text
Checkout / working-tree authority            PASS
Native Windows ARM64 host probes             PASS
Core tests                                   PASS 622/622
Harness tests                                PASS 117/117
Fresh native ARM64 Harness build             PASS
Missing Raft smoke                           PASS
Generic fixture smoke                        PASS
2.5 Flash-Lite credentialless provider edge  PASS expected refusal
3.5 Flash-Lite credentialless provider edge  PASS expected refusal
2.5 Flash credentialless provider edge       PASS expected refusal
Evidence-root absence for all three          PASS
Post-validation checkout/cleanliness         PASS
OpenAI credential use                        NOT PERFORMED
Gemini credential use                        NOT PERFORMED
Gemini token-count network request            NOT PERFORMED
Gemini inference                              NOT PERFORMED
Gemini provider-network execution             NOT PERFORMED
Gemini spend                                  NOT PERFORMED
Overall credentialless native validation     PASS
```

## Recursive validation audit

The Director-machine transcript was checked against the exact repository checkout, current live-host argument contract, comparison profiles, credential boundary, and `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`.

The audit found no material anomaly that invalidates the PASS:

- exact checkout and expected `origin/main` matched;
- stale source/test/build outputs were cleared before execution;
- the selected SDK/runtime were native ARM64;
- Core completed 622/622;
- Harness completed 117/117;
- the Harness executable was rebuilt fresh before fixture/provider-edge smokes;
- both fixtures validated successfully;
- all three approved profile pairings failed at the same credential boundary with exit `1` and the exact required message;
- every probe proved evidence-root absence;
- both provider credentials remained absent throughout;
- post-validation checkout and cleanliness remained exact.

One complete recursive audit pass found no further material correction, inconsistency, ambiguity, regression, or worthwhile improvement inside this validation boundary.

## Validation boundary

This PASS establishes native compiler/runtime authority only for exact checkout `c9f706b42350c8b6cfc462e09cf71db6bc3a2355` and the credentialless/fake-only command scope above, once repository validation-tag law is satisfied.

It does **not** establish or authorize:

- live Gemini API compatibility;
- successful `models.countTokens` network behavior;
- current selected-project/model/account availability or quota beyond separately supplied external evidence;
- live usage/reasoning accounting;
- live implicit-cache behavior;
- inference quality/correctness;
- provider spend;
- any second or subsequent comparison run;
- E0-B or later product scope;
- Windows AI/NPU authority;
- packaging, WACK, or Store certification.

## Repository Surface gate

Before `CURRENT_STATE.md` may promote this checkout as machine-validated, create and push an annotated tag at exact commit:

`c9f706b42350c8b6cfc462e09cf71db6bc3a2355`

Recommended tag:

`validation/e0a-gemini-comparison-native-arm64`

The tag message must record native Windows ARM64 validation, credentialless/fake-only scope, and this evidence path.
