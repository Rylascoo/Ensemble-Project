# Q-E0A-03 G35L Run 01 — Terminal Evidence Analysis

Date: 2026-09-10

Status: **TERMINAL EVIDENCE ANALYSIS COMPLETE — GENERATION-BOUNDARY FAILURE — DIAGNOSTIC CORRECTION WARRANTED — PROVIDER AUTHORITY NONE**

## Authority and scope

This record completes the local archive analysis required by `CURRENT_STATE.md` for exact Run `E0A-Q03-G35L-20260909-01`. It classifies only facts supported by the preserved Director-machine evidence. It does not authorize provider traffic, retry, rerun, fallback, alternate models, credentials, inference, or spend.

Run boundary:

```text
route       CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL
executable  3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
fixture     ensemble.e0.missing-raft@0.1.0
terminal    TechnicalFailure
accepted    0 turns
```

Exact Run 01 authorization remains **CONSUMED**. Provider authorization is **NONE**.

## Archive and seal verification

The preserved archive SHA-256 independently matches `8C82F7FBC465B66F1374B9199F6B0DFC4142A4F5AECDBE18E29B564BA26EE78D`; the console SHA-256 independently matches `51AEFE4321A04F207CA7271FAC6DA919314F813145D4B7DFB3816107FA029EC7`.

All eight artifacts rooted by `run.final.json` independently match their recorded SHA-256 values. The ZIP contains the same nine files as the evidence root under the expected top-level RunId directory; byte comparison after removing that wrapper prefix is exact.
Recomputed seal values match `run.final.json`:

```text
runtimeRoot         ac3a9a4fe4ef1679b5697942a27f2e7175abc5376e78c31fa432f58cb815ccec
runtimeSealIdentity 38b5f879d840febd32837a185a33e5a63089240c52c516e1dfd88f6128589b9e
```

A bounded credential-pattern scan of the preserved runtime/archive content found no credential match. The execution worktree had previously been preserved at the exact authorized checkout; this analysis does not alter the immutable evidence.

## Exact runtime sequence

`events.ndjson` establishes:

1. run start from state hash `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`, Opportunity `VOSS`;
2. deterministic turn-1 context composition for VOSS;
3. Gemini `countTokens` preflight **succeeded** with `inputTokens=649` in `588.4227 ms`;
4. generation rate gate became ready after `3428.5287 ms`;
5. USD `0.16403470` was conservatively reserved;
6. the first Performer provider generation call completed `TechnicalFailure` after `141.5875 ms` provider elapsed / `4179.7088 ms` role elapsed;
7. spend reconciled with unknown provider usage and reason `gemini-http-400`;
8. the run terminated `TechnicalFailure`, zero accepted turns, unchanged state hash and Opportunity.

The attempt terminal record has no response id, returned model, usage, structured output, or accepted Performance. No Integrity, Interpreter, Take, or causal commit occurred.
## Classification

The failure is conclusively localized to the first real Gemini **generation boundary**, after successful corrected token counting. It therefore falsifies any claim that Run 01 failed in `countTokens` or before provider generation invocation.

The preserved evidence does **not** identify the rejected generation field or establish that the current frozen generation request is semantically invalid. The terminal diagnostic is only `gemini-http-400`; arbitrary provider error prose/body was intentionally not persisted. The 2026-09-09 public-fact audit found no current provider-documentation contradiction sufficient to justify mutating the generation request.

## Proven Engineering defect

The Harness has asymmetric bounded-error observability:

- non-success `countTokens` responses parse Google RPC status and validated `BadRequest.fieldViolations[].field` under strict size/privacy/path bounds;
- non-success buffered and streaming generation responses discard those same bounded structured details and return only `gemini-http-<HTTP>`.

That asymmetry is a source defect because an immutable authorized experiment can terminate at generation without retaining the safe structured evidence required to distinguish request incompatibility from another HTTP 400 condition. It is an evidence/diagnostic defect, **not proof of a generation-payload defect**.

Disposition: the smallest warranted correction is `docs/blueprint/E0A_GEMINI_GENERATION_ERROR_DIAGNOSTIC_CORRECTION_AMENDMENT.md`. Do not alter generation bytes by hypothesis.

## Successor boundary

After that diagnostic correction is machine-validated, Q-E0A-03 remains open. Any additional real provider request requires a new explicit Director authorization and a fresh exact run/evidence root. No automatic retry, rerun, probe, fallback, or alternate-model authority is created by this analysis.

## Retained provenance lineage

The following current-surface records remain reachable as provenance for Q-E0A-03/Run 01. Their historical pre-execution statements do not recreate authorization or override this terminal analysis:

- `docs/evidence/E0A_FRESH_CHAT_CONTINUITY_RECONCILIATION_2026_09_09.md`;
- `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_PREAUTHORIZATION_PUBLIC_FACT_AUDIT_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_HISTORICAL_ROOT_FORENSIC_RESOLUTION_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_01_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_02_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_03_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_LOCAL_PREFLIGHT_ATTEMPT_04_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_TERMINAL_CONTINUITY_INTAKE_2026_09_09.md`;
- `docs/evidence/E0A_Q_E0A_03_REFERENCE_EVIDENCE_PLANNING_RECURSIVE_AUDIT_2026_09_09.md`.

They are retained for auditability only. Future execution must use current `CURRENT_STATE.md`, the Q-E0A-03 closure contract, the promoted validation ledger, current external facts, and a new exact Director authorization.
