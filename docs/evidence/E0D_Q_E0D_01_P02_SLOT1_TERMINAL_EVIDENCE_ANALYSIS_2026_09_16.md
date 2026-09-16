# E0-D Q-E0D-01 — P02 Slot 1 Terminal Evidence Analysis

Date: 2026-09-16

Status: **SEALED — HARD GATES PASS — NONCONTRIBUTING INVALID OUTPUT 2/12 — SLOT 1 CONSUMED — P02 EXPERIENTIAL PAIR INELIGIBLE — SLOT 2 SEPARATELY UNAUTHORIZED**

## Authority and exact run

Project authority at launch was `main@2bfb0c1c3e8cb79acafe72d6fb80692bc818a407`; PR #159 had integrated the Director-amended P02 pair window and push-triggered exact-main Validation #900 passed. Timing authority is `docs/evidence/E0D_P02_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_16.md`; Slot-1 authority is `docs/evidence/E0D_P02_SLOT1_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_16.md` plus `docs/evidence/E0D_Q_E0D_01_P02_SLOT1_PREEXECUTION_ACTIVATION_2026_09_16.md`.

- pair: `E0D-P02-OMNISCIENT-CONTEXT`
- slot: `P02-ABLATION`
- variant: `E0D-OMNISCIENT-CONTEXT-01`
- RunId: `E0D-Q01-P02-OMNI-20260914-03`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P02-OMNI-20260914-03`
- executable commit: `8770a6361233e8a877a966c45eb6f62c5b3ca182`
- executable SHA-256: `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- model/profile: `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`
- amended P02 pair window: `2026-09-16T19:33:00Z..2026-09-17T04:33:00Z`

Immediately before launch, authenticated AI Studio evidence showed `Ensemble Testing`, Free tier, exact `Gemini 3.5 Flash Lite`, and live capacity `7 / 15` RPM, `10.83K / 250K` TPM, `17 / 500` RPD. The machine census at `2026-09-16T20:24:23.0595359Z` confirmed exact Project main, absent root/claims, zero Harness processes, exact executable/fixture presence, protected credential artifacts present, and absent ambient `GEMINI_API_KEY`.

## Consumption and terminal result

The Director crossed the protected-credential boundary locally on SurfSeven without placing plaintext key material in chat, command arguments, Git, evidence or the ambient environment. The run claim was created at `2026-09-16T20:27:49.6580887Z`, the root claim at `20:27:49.6600928Z`, and the evidence root at `20:27:49.6610922Z`. P02 Slot 1 is therefore permanently consumed. `run.final.json` sealed at `20:28:42.7243597Z`.

Terminal result: **`InvalidOutput` at 2/12 accepted turns**. Final state hash is `e5f37db15afb77572aae597ccbac150e9f80013c74342e1d338f7a62f90e8648`; final opportunity is `WREN`. Sealed shadow estimate is USD `0.00751730`; `hasUnknownProviderUsage=false`.

## Deterministic invalid-output boundary

Turns 1 and 2 completed Performer -> Integrity -> Interpreter -> State Authority -> opportunity -> atomic commit. Turn 3 reached the Performer only. The provider returned `Success` from exact model `gemini-3.5-flash-lite`, but structured control contained `addressedCharacterIds=["MARLO"]` instead of canonical roster ID `MARLOWE`.

`PerformerCandidateContract` deterministically rejects an addressed Character ID outside the roster, and `E0AReferenceRunDriver` maps that parser exception to terminal `InvalidOutput`. The rejection occurred before a Turn-3 `performer.candidate`, Integrity invocation, Interpreter invocation, Take, commit, or accepted-history entry. `transcript.json` contains exactly two accepted performances. The preserved evidence shows no transport, quota, credential, model-route, fixture, or source-regression signal; the deterministic terminal is the invalid canonical control ID.

## Provider accounting

Seven role invocations reached the provider: three Performer, two Integrity, and two Interpreter. Each invocation performed one `countTokens` preflight and one generation request, for **14 Gemini API operations** total.

Generation usage totals are:

- input tokens: `12116`;
- output tokens: `1553`;
- reasoning tokens: `958`;
- cached input tokens: `0`;
- cache-write tokens: `0`.

All seven generation receipts returned exact model `gemini-3.5-flash-lite`; all provider usage is known and the final shadow estimate remained within verified pricing assumptions.

## Omniscient variant conformance and hard-gate review

All three live Performer contexts carried exact `ensemble.e0.context.e0d.omniscient.v1` / `E0D-OMNISCIENT-CONTEXT-01` identity. Sealed request bodies for Turns 1-3 each contain cross-Character records from Marlowe, Voss and Wren plus directional relationship material, consistent with the frozen category-preserving union. The context contains Character-facing story state only; the evidence-wide credential-marker scan found zero `GEMINI_API_KEY`, `x-goog-api-key`, Authorization/Bearer, `AIza`, or `AQ.` hits.

Runtime evidence sealed 25 artifacts. Independent SHA-256 recomputation found zero mismatches and the root contained exactly the expected 26 runtime files including `run.final.json`. The exact admitted executable then sealed the omniscient hard-gate evaluation with reviewer `ENGINEERING-SOL`, method `SEALED-EVIDENCE-HARD-GATE-REVIEW-V1`, result **PASS**, findings `0`. Final evaluated evidence count is `29` files.

- runtime root: `6b7297be2a46ed81901c1891c3522e3fcdf4a7adc630dd49c05be29aa876b931`
- runtime seal identity: `096e7610d6b1c0ce9085e8806463ca5446406ca1b8479b1dfd83f51fb73db52a`
- `run.final.json` SHA-256: `d8a3b15b2f30cd4115abb2b134ab597e841a30611c8f4a59f6dbaf0c756a053d`
- `evaluation/hard-gates.json` SHA-256: `1dfe9b415d155c9d9ef921604cf38a8e10eda4409a37c16ab1c72a9a447e04f7`
- `evaluation.final.json` SHA-256: `a81b6ac308af9e1ad71486dad1d3b54f052b78debb2a175a53ed4e84d0700e2b`

## Contribution and successor boundary

P02 Slot 1 is **NONCONTRIBUTING** because it reached only 2/12 accepted turns. Under the frozen E0-D method, `InvalidOutput` earns no retry, replay, replacement, fallback, retune, alternate route/model/profile/fixture, or automatic window extension. Because the ablation member failed, matched pair `E0D-P02-OMNISCIENT-CONTEXT` is **ineligible for experiential scoring**, regardless of any later P02-FULL terminal.

The frozen method does not cancel later preregistered slots merely because an earlier slot is noncontributing. P02 Slot 2 / `P02-FULL` remains frozen and unconsumed but is **not authorized by this closeout**. Any Slot-2 launch still requires durable reconciliation of this terminal, a fresh post-predecessor authenticated capacity observation, separate Director authorization, every frozen identity/interlock, and execution inside the amended P02 pair window. P03 and E0-E remain separately gated.