# E0-A Gemini countTokens Correction — Native Windows ARM64 Validation

Status: **PASS — DIRECTOR-MACHINE EXECUTION COMPLETE — VALIDATION TAG/PROMOTION PENDING**

Date: **2026-09-08**

## Exact checkout

`689655eed677b789ab3ee395f1c65b4f2cb72cc8`

Branch authority observed before checkout:

`e0a-gemini-rate-discipline-model-comparison` at `12dd70af11b6ccd39603efb0a3b5d5c4e312d738`.

`origin/main` observed:

`7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

The Director packet detached to the exact correction checkout and confirmed tracked/staged/material-untracked cleanliness before and after validation.

## Host

Director Windows ARM64 machine:

- Windows `10.0.26200`;
- .NET SDK `9.0.317`;
- runtime host `10.0.11`, architecture `arm64`;
- RID `win-arm64`;
- `PROCESSOR_ARCHITECTURE=ARM64`.

## Native tests

```text
Core tests      622/622 PASS
Harness tests   125/125 PASS
```

The Harness count increases from the previously promoted 122 because the countTokens correction added targeted regressions.

## Fresh native build and fixture smokes

- stale Core/Harness/test build outputs cleared before execution;
- fresh Debug Harness build: `net9.0/win-arm64` PASS;
- frozen Missing Raft fixture smoke: PASS;
- generic E0 smoke fixture: PASS.

## Credentialless provider boundary

Before provider-edge probes, both `GEMINI_API_KEY` and `OPENAI_API_KEY` were explicitly absent.

Current live profiles each returned native exit 1 with the exact expected missing-key refusal and created no evidence root:

- `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL`;
- `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE`.

The retired `GEMINI-2.5-FLASH-NONE` route returned native exit 1 with the expected pre-credential live-selection rejection and created no evidence root.

The PowerShell `NativeCommandError` presentation is host-shell formatting only; the packet independently asserted the native exit code, expected diagnostic, and absent evidence root for each probe.

## Post-validation authority

The packet re-resolved `HEAD` as exactly:

`689655eed677b789ab3ee395f1c65b4f2cb72cc8`

Tracked/staged/material-untracked repository surfaces remained clean. Both provider-key environment variables remained absent.

The packet reached:

`COUNT_TOKENS_CORRECTION_NATIVE_GATE=PASS`

## Provider/network scope

```text
Gemini provider network           NOT PERFORMED
Gemini countTokens network call   NOT PERFORMED
Gemini inference                  NOT PERFORMED
Gemini spend                      NOT PERFORMED
Real-run authorization            NONE
```

This evidence validates native Windows ARM64 execution of the countTokens correction source/test/credentialless boundary only. It does not establish successful live countTokens behavior, provider account availability, inference, live usage accounting, latency, quality, or spend.

## Promotion gate

Native machine evidence is complete for this checkout. Promotion requires an annotated validation tag that dereferences exactly to `689655eed677b789ab3ee395f1c65b4f2cb72cc8`. The tag message must record native Windows ARM64 validation, credentialless/no-provider-network scope, and this evidence document. Only after that tag exists may `CURRENT_STATE.md` and `docs/VALIDATION_LEDGER.md` promote this checkout as current machine-tested authority.
