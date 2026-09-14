# E0-C Q-E0C-01 - Slot 2 Terminal Protocol Reconciliation and Director Input

Date: 2026-09-13

Status: **SLOT 2 CONSUMED / NONCONTRIBUTING - INVALID OUTPUT AT 0/12 - POST-LAUNCH CAPACITY-GATE COMPLIANCE DEFECT - NO REPLAY OR REPLACEMENT - DIRECTOR DISPOSITION REQUIRED BEFORE BLIND SCORING OR E0-D**

## Immutable execution result

Predecessor activation is `docs/evidence/E0C_Q_E0C_01_RUN02_PREEXECUTION_ACTIVATION_2026_09_13.md`. Canonical Slot 2 `E0C-Q01-IDENT-20260912-02` launched once at `2026-09-13T23:15:47.8715145Z` after PR #119 merged as `50c6e53a8abfd40f2af55deae22d7ddbf683ab6a` and push-triggered exact-main Validation #769 passed. The child exited `3` at `2026-09-13T23:15:53.5433138Z`. The evidence root and both authoritative claims exist, so the slot is permanently consumed.

Authoritative claims are `run-83d68e99cd8b304d958b67b66ef555a8551602068783c07c943128717c83e89b.claim` and `root-622000c79f2a5ff649979ab3cdd641a03cf1e6b62b41025696c0dcc66d9f6847.claim`. These are exactly the source-derived and canonical-preregistration claim filenames.

Harness terminal is `InvalidOutput` with `0/12` accepted turns, final StateHash `30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104`, final Opportunity `VOSS`, known usage, and sealed shadow spend USD `0.00057720`.

The run made exactly two Gemini API operations: one `models.countTokens(generateContentRequest)` preflight and one generation. The generation itself succeeded with nonblank response ID and exact returned model `gemini-3.5-flash-lite`; usage was 724 input / 144 output / 0 reasoning / 0 cached-input tokens.

## Exact InvalidOutput cause

The successful Performer structured output named addressed Character IDs `MARLOE` and `WREN`. The canonical roster contains `MARLOWE`, not `MARLOE`. `PerformerCandidateContract` requires every `addressedCharacterIds` value to be a roster Character and throws `PerformerCandidateException` for an out-of-roster ID; `E0AReferenceRunDriver` catches that parse exception and terminates `InvalidOutput`. No Candidate was accepted, and Integrity/Interpreter were never invoked.

The terminal is therefore a schema/typed-control invalid-output result after a successful provider response, not a provider availability, quota, credential, or route failure. It is noncontributing and cannot enter experiential comparison.
## Seal verification

`run.final.json` SHA-256 is `4061c8984b1a81a79aa16553bb3cf1e98f647ec90056af6ee43e32826244aea3`. Independent recomputation found 9 sealed artifacts and 0 hash mismatches. Runtime root is `6c590aa15a7979a6dd332cf4d663c7c6ff22d4f7c27ee0bc9289a01adb275b57`; runtime-seal identity is `37682840a71db982b08e7724221f2385232b7b8772cbe4dcb62673898ea2694b`.

No hard-gate experiential PASS can be earned because the contribution law requires 12 accepted turns. Under the frozen instrument, noncontributing Slot 2 removes pairs P02 (Run 08 / Slot 2) and P03 (Slot 1 / Slot 2). P01 (Run 08 / Slot 1) is the only otherwise eligible experiential pair.

## Post-launch activation-compliance defect

A post-launch independent audit identified, and Engineering independently confirmed, that `docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md` explicitly keeps fresh model/tier/quota/capacity evidence execution-sensitive. Slot 2 activation instead reused the authenticated pre-Slot-1 `42/500 RPD`, `9/15 RPM`, `22.07K/250K TPM` snapshot and inferred post-Slot-1 headroom arithmetically.

That arithmetic was conservative and the actual provider generation succeeded, but it was not a new authenticated post-Slot-1 quota observation. Hosted Validation cannot override the frozen method law. Slot 2 therefore launched with an unresolved execution-sensitive capacity gate and should have remained blocked. This is a protocol-compliance defect, not a causal explanation for the `InvalidOutput` terminal.

The defect creates no retry, replacement, third slot, alternate route, model/key/tier substitution, or window extension authority. Any namespace claim consumes the slot, and both authoritative claims exist.

## Claim-history correction

Canonical two-slot preregistration correctly predicted the exact Slot 1 and Slot 2 claim filenames. A later launch-boundary checker used different Slot 1 claim paths; that checker was wrong. The Harness source-defined claim guard and canonical preregistration were correct. The Slot 1 terminal record is corrected in the same reconciliation package.

## Director boundary

The two planned fresh namespaces are now consumed: Slot 1 is sealed/hard-gate PASS/contributing; Slot 2 is sealed/noncontributing with the protocol defect above. No further provider traffic is authorized. Before blind pair mapping/scoring, unblinding, E0-C closure, or E0-D advancement, return this deviation to the Director.

Recommended disposition is to preserve Slot 2 exactly as consumed/noncontributing protocol-deviation evidence, authorize no replacement, and decide explicitly whether the unaffected P01 Run 08 / Slot 1 pair may proceed through the already-frozen blind comparison law or whether E0-C should close without experiential scoring because of the execution-process breach.