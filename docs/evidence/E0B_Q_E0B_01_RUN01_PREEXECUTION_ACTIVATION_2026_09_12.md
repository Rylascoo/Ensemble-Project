# E0-B Q-E0B-01 Run 01 — Preexecution Activation

Date: 2026-09-12

Status: **ACTIVATION GATES PASS — ONE EXECUTION ONLY AFTER INTEGRATION + GREEN POST-MERGE VALIDATION — PROVIDER TRAFFIC NOT YET SENT**

Authority: `docs/evidence/E0B_Q_E0B_01_MIXED_CAST_DIRECTOR_DECISION_2026_09_12.md` approves `E0B-MIXED-CAST-01`; `docs/evidence/GEMINI_API_PROJECT_RELEVANCE_STANDING_DIRECTOR_AUTHORIZATION_2026_09_10.md` supplies standing project-relevant Gemini Free-tier authority after the exact activation gates pass.

## Exact run boundary

- RunId: `E0B-Q01-MIX-20260912-01`
- condition: `E0B-MIXED-CAST-01`
- executable: `d1073fe2c76e2e05f2daac47465f86b48b456a9a`
- validation tag: `validation/e0b-mixed-cast-implementation-native-arm64`
- tag object: `f7eadb838c4e01d9b4a01d17ff20b8d938046b03`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- canonical fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0B-Q01-MIX-20260912-01`
- accepted-turn cap: `12`
- attempts per role invocation: `1`; automatic retries: `0`; attempt timeout: `300s`
- visible output ceiling: `4096` tokens per role; total shadow-spend ceiling: `USD 5.00`

The reserved root, run claim, and root claim remain absent, no external process is using the RunId, and the detached validator remains clean at the exact executable. This activation creates no runtime claim/root.
## Fixed cast and frozen comparison

- VOSS Performer: `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL` / `minimal`.
- MARLOWE/WREN Performers: `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL` / `minimal`.
- Integrity every Turn: Run 08 3.5 route / `high`.
- Interpreter every Turn: Run 08 3.5 route / `minimal`.

Run 08 remains the selected reference. The blind scoring instrument was frozen before any E0-B transcript at `docs/evidence/E0B_Q_E0B_01_RUN01_BLIND_SCORING_INSTRUMENT_2026_09_12.json`, SHA-256 `36de1134d303072f4fb8aaa77fb82945bbfca130fd826aa9ec31ff98e567eb22`. No scoring/unblinding may occur before a contributing E0-B transcript exists and runtime/hard-gate evidence is sealed.

## Authenticated AI Studio gate

The Director supplied three authenticated Google AI Studio screenshots on 2026-09-12. The screenshots are not committed because account UI context is not required for executable reproduction and must not become repository credential material.

1. Rate-limit view SHA-256 `6476071a83837b8738b1531b379e56d714e9b3c00fadcc7031c8b1fcbb7b4a8d`: selected Kymaean project, Free tier; Gemini 3.5 Flash Lite shows peak `9 / 15 RPM`, `22.07K / 250K TPM`, `40 / 500 RPD`; Gemini 3.1 Flash Lite shows peak `4 / 15 RPM`, `7.12K / 250K TPM`, `8 / 500 RPD` over the selected 28-day window.
2. Project view SHA-256 `1bf3a8a8cfe10a2fe71ae72ac03eac32eadfd681c260689694a8560b0e2625f5`: `Gemini Project - Kymaean`, project ID `gen-lang-client-0490221700`, two keys, Free tier.
3. API-key view SHA-256 `65d381ea0c0896bb7712355358b8bee5a6acf103bd25d0cd668062d481bdd516`: `Gemini API Key - testing` is listed under `Gemini Project - Kymaean`, project ID `gen-lang-client-0490221700`, Free tier. No full key value is recorded here.

## Quota/capacity interpretation

The AI Studio rate-limit screen explicitly describes the displayed figures as **peak usage per model compared to its limit over the last 28 days**. They are therefore not mislabeled here as instantaneous counters. Because current usage lies within that window, it cannot exceed the displayed peak. At capture, the evidence therefore establishes conservative lower bounds of at least `460` RPD headroom for 3.5 and `492` RPD headroom for 3.1.

The reserved run can issue at most 72 provider operations total. Thus either route individually has more RPD headroom than the entire run could consume, even under an intentionally over-conservative all-operations-on-one-route bound. The displayed RPM/TPM peaks are likewise below the active 15 RPM / 250K input-TPM limits encoded by the validated routes. The Harness still enforces route-specific pacing and one conservative shared-project aggregate discipline; this snapshot does not guarantee provider availability or authorize concurrency, retry, fallback, or quota bypass.

No material account/profile discrepancy exists: both exact routes show `15 RPM / 250K TPM / 500 RPD`, matching the validated executable catalog. The authenticated project/key/tier/quota/capacity gate therefore passes.

## Public/native/credential boundaries

`docs/evidence/E0B_Q_E0B_01_RUN01_PUBLIC_FACT_AUDIT_2026_09_12.md` remains current for exact model lifecycle, thinking controls, pricing, data-use, and API-family facts. The executable freshness guard remains valid through 2026-09-14.

Implementation/native integration predecessor: docs/evidence/E0B_MIXED_CAST_IMPLEMENTATION_INTEGRATION_CLOSEOUT_2026_09_12.md. This activation does not transfer native runtime authority to later documentation or merge commits.

Fresh detached validator `C:\Users\Wiryl\Sol Dev\E0V-d1073fe` remains clean at exact executable `d1073fe2c76e2e05f2daac47465f86b48b456a9a`. Its native ARM64 Release build and canonical Missing Raft smoke passed during preactivation, and the exact credentialless `e0b-run` reached the missing-key edge without creating evidence or sending provider traffic.

The protected credential artifacts remain outside Git. This activation used only authenticated UI evidence and prior protected-container readiness; it did not read, decrypt, print, hash, persist, or export the API key. Plaintext may be injected only into the intended child process environment at the single authorized execution boundary.
## One-run execution law

After this activation package is integrated and exact-main post-merge Validation passes, standing Director authority covers exactly one execution of `E0B-Q01-MIX-20260912-01` through the fixed `e0b-run` path. No separate availability/key-health/quota probe is justified; the lowest-call useful provider action is the reserved run itself.

Any actual Harness namespace claim consumes this RunId/root. Any terminal result—success, provider error, cancellation, budget stop, invalid output, refusal, or other fail-closed terminal—must be preserved, entered in `docs/evidence/GEMINI_API_USAGE_LEDGER.md`, and never replayed. No automatic retry, replacement RunId, fallback route, paid/Priority switch, alternate key, or model substitution is authorized by this record.

A contributing run requires 12/12 accepted Turns, correct Character/role/route/model provenance, valid receipts/accounting, complete runtime seal, and independent hard-gate PASS. Only then may the frozen blind Run-08-vs-E0-B experiential scoring proceed, with scores sealed before unblinding.

## Disposition

All preexecution activation gates for reserved Run 01 now pass. **Provider traffic remains prohibited until this record and its continuity updates are merged and the resulting exact `main` commit has a green push-triggered Validation gate.** After that condition is durably satisfied, the next action is exactly one execution of the reserved run on the preserved native validator, followed by immediate evidence sealing/audit and usage-ledger accounting.