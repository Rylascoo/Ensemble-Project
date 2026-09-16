# E0-D Q-E0D-01 - P01 Slot 1 Terminal Evidence Analysis

Date: 2026-09-15

Status: **SEALED - HARD GATES PASS - NONCONTRIBUTING INVALID OUTPUT 2/12 - SLOT 1 CONSUMED - P01 EXPERIENTIAL PAIR INELIGIBLE - SLOT 2 SEPARATELY UNAUTHORIZED**

## Authority and exact run

Current Project authority at launch was `main@fc42f5441b820d0dfd82654c37dbf4d2ddd025e2`; push-triggered exact-main Validation #869 passed. `docs/evidence/E0D_P01_SLOT1_PRECLAIM_CHECKOUT_GUARD_REFUSAL_2026_09_15.md` records the prior non-consuming checkout-guard refusal and Director disposition authorizing exactly one corrected preclaim launch. Timing authority remains `docs/evidence/E0D_P01_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_15.md`; all non-timing activation/readiness/provider-association predecessors retained by the refusal evidence remain in force except where this terminal consumes Slot 1.

- pair: `E0D-P01-RELATIONSHIP-OMISSION`
- slot: `P01-FULL`
- variant: `E0D-FULL-REFERENCE-01`
- RunId: `E0D-Q01-P01-FULL-20260914-01`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P01-FULL-20260914-01`
- executable commit: `0dacdbf6bd5453c192568cd4718207e145dfcf40`
- executable SHA-256: `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- canonical fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- model/profile: `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`
- amended P01 pair window: `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`

Immediately before the corrected launch, authenticated Google AI Studio showed project `Ensemble Testing`, Free tier, exact `Gemini 3.5 Flash Lite`, and live capacity `0 / 15` RPM, `0 / 250K` TPM, `0 / 500` RPD at `2026-09-15T23:58:39.8428899Z`. The machine census at `23:54:04Z` confirmed exact Project main, validator/tag/executable identity, clean validator, canonical fixture presence, zero Harness processes, absent ambient `GEMINI_API_KEY`, and successful in-memory decryption of the new CurrentUser-protected credential.

## Consumption and terminal result

The source-derived run claim was created at `2026-09-15T23:59:48.3087196Z`; the root claim followed at `23:59:48.3129447Z`; the evidence root was created at `23:59:48.3149479Z`. Slot 1 is therefore permanently consumed. `run.final.json` sealed at `2026-09-16T00:00:43.0833165Z`.

Terminal result: **`InvalidOutput` at 2/12 accepted turns**. Final state hash is `7496a4c2921ee6f4564b2c234f08fefd24566fd174c1146ac55490d5077e5f0d`; final opportunity is `VOSS`. Sealed shadow estimate is USD `0.008278`; `hasUnknownProviderUsage=false`.

## Deterministic invalid-output boundary

Turns 1 and 2 completed the full Performer -> Integrity -> Interpreter -> State Authority -> opportunity -> atomic commit path. Turn 3 reached the Performer only. The provider returned `Success` from exact model `gemini-3.5-flash-lite`, but structured control contained `addressedCharacterIds=["MARLO"]` instead of canonical roster ID `MARLOWE`.

The deterministic candidate parser rejected that control before a Turn-3 `performer.candidate`, Integrity invocation, Interpreter invocation, Take, commit, or history entry could occur. `transcript.json` contains exactly the two accepted performances. This is an invalid canonical-control-ID terminal, not a transport, quota, credential, schema, model-route, fixture, or source-regression failure.

## Provider accounting

Seven role invocations reached the provider: three Performer, two Integrity, and two Interpreter. Each invocation performed one `countTokens` preflight and one generation request, for **14 Gemini API operations** total.

Generation usage totals are:

- input tokens: `10860`;
- output tokens: `2008`;
- reasoning tokens: `1293`;
- cached input tokens: `0`;
- cache-write tokens: `0`.

All seven generation receipts returned exact model `gemini-3.5-flash-lite`; all provider usage is known and the final spend estimate remained within verified pricing assumptions.

## Runtime seal and hard-gate review

Runtime evidence sealed 25 artifacts before evaluation. Independent review against `ensemble.e0a.hard-gates.v1` found zero violations: inaccessible/Production-authority records remained denied, model claims stayed Character claims until deterministic State Authority approval, both accepted turns have traceable atomic commits, and the rejected Turn-3 output never entered history. No provider failure became fiction and no deterministic access/cost/cancellation/eligibility rule was delegated to a model.

The exact validated executable sealed the evaluation with reviewer `ENGINEERING-SOL`, method `SEALED-EVIDENCE-HARD-GATE-REVIEW-V1`, result **PASS**, findings `0`. Final evidence count is `29` files.

- runtime root: `f65f57bc8d3efb6c649fe07e2f85d456cbb09972f209b48ee8ceb8b85f68d220`
- runtime seal identity: `9a165859efa67e6f7944c334371c83cb406965f8552663a447e7379448684de0`
- `run.final.json` SHA-256: `ce7a10c504830c43c8624f8723d6c95de8a6dd43beeb8a27017402112db8fdc1`
- `hard-gates.json` SHA-256: `be13aa87db411dcd95006e2b373516c12a21feba2c9c6ae597fd4767551fb65a`
- `evaluation.final.json` SHA-256: `a4eb86f7763e217adb9f0760f79774f371feaae89e69b232fd43572eb53f3af1`

## Contribution and successor boundary

Slot 1 is **NONCONTRIBUTING** because it reached only 2/12 accepted turns. Under the frozen E0-D method, its clean hard-gate PASS preserves reliability/process evidence but earns no experiential score, no replacement, and no retry. Because the Full member failed, matched pair `E0D-P01-RELATIONSHIP-OMISSION` is **ineligible for experiential scoring** regardless of any later Slot-2 terminal.

The frozen method does not cancel later preregistered slots merely because an earlier slot is noncontributing. P01 Slot 2 remains frozen and unconsumed, but is **not authorized by this closeout**. Any Slot-2 launch still requires a separate Director authorization, fresh execution-sensitive authenticated capacity observation after this terminal, every frozen identity/interlock, and execution inside the amended P01 window. No retry/replay/replacement of Slot 1 exists.
