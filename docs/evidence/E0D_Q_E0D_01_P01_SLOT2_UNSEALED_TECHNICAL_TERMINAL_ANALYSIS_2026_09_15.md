# E0-D Q-E0D-01 — P01 Slot 2 Unsealed Technical Terminal Analysis

Date: 2026-09-15

Status: **CONSUMED — NONCONTRIBUTING — PROVIDER HTTP 503 / UNAVAILABLE AFTER 3/12 — RUNTIME FINALIZATION DEFECT — NO RETRY — LATER E0-D LIVE EXECUTION HOLD**

## Authority and exact identity

Project predecessor authority is `main@46c5d7fc8597b33012e2f083fd42113a47b83e77`, where PR #153 integrated `docs/evidence/E0D_P01_SLOT2_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_15.md` and `docs/evidence/E0D_Q_E0D_01_P01_SLOT2_PREEXECUTION_ACTIVATION_2026_09_15.md`; push-triggered exact-main Validation #881 passed.

- pair / slot: `E0D-P01-RELATIONSHIP-OMISSION` / `P01-ABLATION`;
- variant: `E0D-RELATIONSHIPS-OMITTED-01`;
- RunId: `E0D-Q01-P01-REL-20260914-02`;
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P01-REL-20260914-02`;
- renewed native checkout: `0dacdbf6bd5453c192568cd4718207e145dfcf40`;
- validation tag: `validation/e0d-snapshot-refresh-native-arm64`;
- executable SHA-256: `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`;
- fixture: `ensemble.e0.missing-raft@0.1.0`, canonical SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- provider/model/profile: Google Gemini API / `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`.

The launch-boundary authenticated observation showed `Ensemble Testing` / Free tier / exact `Gemini 3.5 Flash Lite` at `4/15` RPM, `6.18K/250K` TPM and `7/500` RPD. Machine interlocks were clean before launch.
## Consumption and observed terminal

The deterministic run/root claims and evidence root were created at `2026-09-16T02:16:12Z`; Slot 2 is therefore permanently consumed. No retry, replay, replacement, fallback, model/profile/fixture substitution or retune exists.

Turns 1–3 completed Performer → Integrity → Interpreter → State Authority → commit. Turn 4 Performer then completed its `countTokens` preflight (`943` projected input tokens) but its generation returned `TechnicalFailure` with exact bounded diagnostic `gemini-http-503;status=UNAVAILABLE`.

The failed attempt terminal has no response ID, returned model, usage receipt or structured output. No Turn-4 `performer.candidate` or `turn.committed` event exists; the technical failure did not become fictional action or Production history.

Provider accounting established by the preserved evidence:

- accepted turns: `3 / 12`;
- `countTokens` preflights: `10`, projected input total `15,310` tokens;
- generation attempts: `10` = `9` success + `1` technical failure;
- total Gemini API operations: `20`;
- known successful-generation usage: `14,367` input / `2,449` output / `1,776` reasoning / `0` cached-input tokens;
- final conservative shadow estimate: USD `0.17455550`;
- failed-call usage is unknown, so terminal accounting remains conservative rather than exact.

## Runtime-finalization defect

The provider 503 was followed by an unhandled `Ensemble.E0.Core.Turn.E0TurnOrchestrationException: E0 turn attempt gate failed.` The Harness child exited nonzero before normal finalization. Consequently the root has no `run.terminal` event, `run.final.json`, `run.summary.json`, runtime seal or `evaluation.final.json`.
The exact defect is source-defined. For a Performer terminal, `E0AReferenceRunDriver.RunAsync` binds a technical result to the already-composed context and calls baseline `DeterministicE0TurnOrchestrator.GateAttempt(...)` before `Finish(...)`. `GateAttempt(...)` recomposes the ordinary baseline context with `DeterministicE0CausalCycle.ComposeContext(source)` and rejects the E0-D relationship-omitted packet ID as stale. By contrast, the successful E0-D candidate path uses variant-aware `E0DExperimentalTurnGate.GateCandidate(...)`.

`E0DExperimentalTurnGate` currently exposes only `GateCandidate`; there is no corresponding context-ablation-aware technical/cancelled gate. This is therefore a real executable defect in E0-D technical-terminal handling, not evidence that the 503 did not occur.

Do **not** synthesize or manually add `run.final.json` to the immutable consumed root. Do **not** invoke `e0d-evaluate` against an unsealed runtime. Any future repair must preserve this root exactly and be validated as a successor executable.

## Evidence integrity and forensic preservation

The consumed root contains `28` files at audit time. Bounded scans for Google API-key prefixes/header names, the ambient Gemini-key environment-variable name and Bearer-token patterns found `0` matches.

- `manifest.json` SHA-256: `308cca4c782ca696ecf91728734500ba54289bd25b9f2565b4bdf1cc12cd5dc8`;
- `events.ndjson` SHA-256: `1d94bcc341950c011b57c27be9f64c338fb0e2e3c53aeed84978199397f09680`;
- failed Turn-4 terminal SHA-256: `04a636c686653e80a670e6d13a16e031bfa7a357df6ee64a70ab0700b9aa6563`;
- deterministic forensic root digest over the 28 preserved path/hash pairs: `bf9f72331f9cf7dbf28ae37b814428da3f1594d08205961cc7f0d808ffed0991`;
- adjacent read-only forensic ZIP: `E0D-Q01-P01-REL-20260914-02-unsealed-terminal-evidence.zip`, SHA-256 `e9bf5b6340cd5689ce435203191b4856ae0e4bd6fa2e305fecfcff18672fe2bc`.

The ZIP is outside the consumed evidence root and does not alter it.

## Forensic hard-gate review

A read-only Engineering review of the preserved partial evidence found no hard-gate breach in the three committed turns or the failed Turn 4. All Production-authority records remained denied in the recorded access decisions; other-character exclusions remained denied; Performer request scans contained zero relationship-record or Production-authority tokens under the relationship-omitted composition; each committed turn has a Performer candidate, accepted Integrity evaluation and completed State Authority evaluation; Turn 4 has neither candidate nor commit; and no technical failure entered fiction.
That forensic review is **not** a formal sealed E0-D evaluation: the runtime seal prerequisite is absent, so no `evaluation.final.json` is published and no sealed PASS claim is made for Slot 2.

## Contribution and successor boundary

Slot 2 is permanently **NONCONTRIBUTING**: only 3/12 accepted turns completed and the consumed run ended in a provider technical failure followed by an executable finalization crash. Slot 1 was already noncontributing, so P01 remains experiential-ineligible and no pair score exists.

The defect is a material executable/evidence-preservation contrary signal. **Hold all later E0-D live execution.** Before P02 or P03 provider traffic, Engineering must produce a narrow technical-terminal handling repair (or an equally bounded successor), add regression coverage for E0-D context-ablation Performer technical/cancelled outcomes, complete renewed native Windows ARM64 validation and evidence-sealing smokes, and obtain Director disposition on the changed executable and remaining frozen-slot admissibility.

No later slot, E0-E execution, retry or provider probe is authorized by this analysis.