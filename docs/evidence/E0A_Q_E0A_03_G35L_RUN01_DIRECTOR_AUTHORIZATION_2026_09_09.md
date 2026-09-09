# E0-A Q-E0A-03 Gemini 3.5 Flash-Lite Run 01 — Director Authorization

Status: **AUTHORIZED — UNCONSUMED — LOCAL PREFLIGHT PENDING**

Date: **2026-09-09**

Authority: this record preserves the Director's explicit one-run provider authorization. `CURRENT_STATE.md` remains the sole phase/checkpoint/validation/next-action authority. The frozen planning/closure law remains `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.

## Exact authorized run

```text
RunId          E0A-Q03-G35L-20260909-01
executable     3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
validation tag validation/e0a-gemini-counttokens-input-projection-native-arm64
tag object     b9341c1c0ab48be5370ed22e7d2511f9e3be00d9
fixture        ensemble.e0.missing-raft@0.1.0
fixture SHA    5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703
arm/profile    CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL
model          gemini-3.5-flash-lite
accepted cap   12
evidence root  C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G35L-20260909-01
```

The native validation tag was freshly checked before this record was created and still dereferenced through annotated tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9` to exact executable commit `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`.

## Director authorization — verbatim

> I authorize exactly one Q-E0A-03 full-reference provider run with RunId `E0A-Q03-G35L-20260909-01`.
>
> The authorized run is limited to:
>
> * executable checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`;
> * annotated validation tag `validation/e0a-gemini-counttokens-input-projection-native-arm64`;
> * canonical fixture `ensemble.e0.missing-raft@0.1.0`, SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
> * arm/profile `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`;
> * model `gemini-3.5-flash-lite`;
> * accepted-turn cap 12;
> * evidence root `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0A-Q03-G35L-20260909-01`;
> * one attempt per probabilistic role invocation;
> * zero automatic retries;
> * no fallback, alternate model, alternate profile, alternate fixture, compatibility-only probe, or unrelated provider traffic;
> * the existing 300-second role-attempt timeout, deterministic rate discipline, USD 5 shadow-spend ceiling, synthetic-only privacy boundary, evidence immutability, cancellation, usage, and cleanup laws.
>
> A purely local preflight failure before the first provider-bound invocation does not consume this authorization. Once the run begins its first provider-bound invocation, any terminal result consumes this authorization and does not authorize a rerun.
>
> If the corrected first `countTokens` preflight succeeds, this same authorized run may continue through its frozen generation and deterministic commit/Opportunity path up to the configured 12-turn cap. If `countTokens` or any later provider boundary terminates the run, preserve the resulting evidence and do not retry.
>
> The API credential will be supplied only locally on the Director's Windows ARM64 machine and must never be pasted into chat, committed, or persisted in project evidence.
>
> This authorization applies only while the executable's existing provider snapshot freshness guard remains valid through September 14, 2026 and the already-audited provider conditions have not materially changed.

## Fresh activation audit

Before recording this authorization, repository and current official-provider surfaces were rechecked on 2026-09-09.

Repository evidence established:

- Q-E0A-03 planning/closure contract remains frozen;
- Q-E0A-04 corrected executable remains the promoted native authority;
- repository search found no prior occurrence of RunId `E0A-Q03-G35L-20260909-01`;
- provider authorization in the preauthorization audit was still NONE before this Director authorization;
- `E0AGeminiPricingPolicy.SnapshotValidThrough` remains `2026-09-14`;
- the Director-approved 3.5 quota snapshot remains 15 RPM / 250,000 input TPM / 500 RPD until a recorded contrary signal occurs.

Current official Google documentation recheck found no material contrary signal: `gemini-3.5-flash-lite` remains stable with structured outputs and thinking supported; the current GenerateContent surface still exposes `generationConfig.responseFormat.text.mimeType/schema`; `minimal` remains supported for 3.5 Flash-Lite; current Standard pricing remains Free-tier free with paid shadow-reference rates $0.30 input / $2.50 output / $0.03 cached input per million tokens; current Unpaid Services terms still permit Google product/model improvement use and human review and therefore preserve the project's synthetic-only boundary; no shutdown date is announced for `gemini-3.5-flash-lite`.

No manual quota lookup was performed because the route-specific Director reuse decision explicitly waives repeated quota rereads absent a recorded contrary signal. No such signal was found.

## Consumption law

Authorization is **UNCONSUMED** until the first provider-bound invocation of this exact run begins. A purely local preflight failure before that boundary does not consume it.

The Director-machine packet must emit:

```text
PROVIDER_INVOCATION_STARTED=YES
```

at the guarded provider-run boundary after all packet-level local checks and secure process-local credential entry have passed. Once that boundary is crossed, this authorization is treated as consumed for operational safety; any terminal result is final for this authorization. No retry, rerun, fallback, alternate model/profile/fixture, compatibility probe, or unrelated provider call is permitted.

If the validated Harness itself fails a local fail-closed guard before reaching a provider-bound invocation, terminal evidence must be inspected before consumption is durably classified; the Director authorization text above controls that distinction.

## Local gates still unresolved by repository inspection

Only the Director's Windows ARM64 machine can establish these immediately before execution:

- preserved historical root remains clean and unchanged;
- dedicated execution worktree is safe and exact at the validated checkout;
- canonical fixture bytes hash exactly to the frozen SHA-256;
- exact evidence root does not exist and no local archive for this RunId proves prior execution;
- `dotnet --info` reports native Windows ARM64 host facts;
- credential is entered securely and exists only in process environment scope;
- no unrelated provider traffic/capacity change creates a quota contrary signal.

Failure of any local gate before provider invocation stops the packet without provider traffic and leaves this authorization unconsumed.

## Current disposition

- exact RunId authorization: **AUTHORIZED / UNCONSUMED**;
- provider scope: **only** `E0A-Q03-G35L-20260909-01` as specified above;
- next action: guarded native Windows ARM64 local preflight, then exactly one provider run if every gate passes;
- Gemini 3.1 / Gemini 2.5: **NOT AUTHORIZED**;
- no provider traffic was sent while creating this record.
