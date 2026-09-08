# E0-A Gemini Free-Tier RPD Model Selection — Native Windows ARM64 Validation

Status: **PASS — MACHINE-TESTED CHECKOUT ESTABLISHED — VALIDATION TAG/PROMOTION PENDING**

Date: **2026-09-08**

## Exact checkout

`3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`

Branch at validation time:

`e0a-gemini-rate-discipline-model-comparison`

`origin/main` observed by the Director packet:

`7490de24bfd2a9829f6afc1ae4b3831c98c50837`

The checkout was detached to the exact validation commit and remained tracked/material-untracked clean before and after validation.

## Host

Director Windows ARM64 machine.

Observed host facts:

- Windows version `10.0.26200`;
- `PROCESSOR_ARCHITECTURE=ARM64`;
- .NET SDK `9.0.317`;
- runtime host `10.0.11`, architecture `arm64`;
- `dotnet --info` RID `win-arm64`.

## Native tests

```text
Core tests      622/622 PASS
Harness tests   122/122 PASS
```

The Harness result supersedes failed native attempt 01 at `31436ee52238c2de96e9bcbe9d61ece173f81dd3`, which reached only 121/122 because a stale readiness test treated the retired 2.5 Flash route as live. That failed checkout is not promoted.

## Fresh native build and fixture smokes

- stale Core/Harness/test build outputs cleared before execution;
- fresh Debug Harness build: `net9.0/win-arm64` PASS;
- frozen Missing Raft fixture smoke: PASS;
- generic E0 smoke fixture: PASS.

## Credentialless live-profile boundary

Before provider-edge probes, both `OPENAI_API_KEY` and `GEMINI_API_KEY` were explicitly absent.

All current live profiles returned native exit 1 with the exact expected refusal:

`GEMINI_API_KEY is required at the E0-A provider edge.`

Profiles checked:

- `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`.

For every probe, the designated evidence root remained absent.

The retired historical route `GEMINI-2.5-FLASH-NONE` was also probed and rejected before credential access/evidence creation with:

`E0-A Gemini provider profile is not approved for live selection.`

## Post-validation authority

The packet re-resolved `HEAD` as exactly:

`3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`

Tracked and material-untracked repository surfaces remained clean. Both provider-key environment variables remained absent.

The packet reached:

`RPD_MODEL_SELECTION_NATIVE_GATE=PASS`

## Provider/network scope

```text
Gemini provider network           NOT PERFORMED
Gemini countTokens network call   NOT PERFORMED
Gemini inference                  NOT PERFORMED
Gemini credential use             NOT PERFORMED
Gemini spend                      NOT PERFORMED
```

This evidence proves native Windows ARM64 execution of the amended Harness/test/credentialless boundary only. It does not prove live Gemini API compatibility, account service availability at execution time, provider response semantics, live usage accounting, latency, quality, or successful inference.

## Promotion gate

Native machine evidence is complete for this checkout. Promotion requires an annotated validation tag that dereferences exactly to `3d6d8a7f1caf548c15f0f2393d0fc7b50ac0cd99`, after which `CURRENT_STATE.md` and the validation ledger may promote it as current machine-tested authority.
