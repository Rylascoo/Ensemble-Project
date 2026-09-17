# E0-D Q-E0D-01 — P02 Slot 2 Terminal Evidence Analysis

Date: 2026-09-17

Status: **SEALED — HARD GATES PASS — NONCONTRIBUTING INVALID OUTPUT 2/12 — SLOT 2 CONSUMED — P02 PAIR FULLY CONSUMED / EXPERIENTIALLY INELIGIBLE — NO RETRY / REPLACEMENT / SCORING**

## Authority and exact run

Project authority at launch was `main@526893a0f518b52bceddb311514de12ec826b9b3`; PR #161 had integrated exact P02 Slot-2 Director authorization and activation, exact-head Validation #905 and E0-E preparation #106 passed, and push-triggered exact-main Validation #906 passed. Timing authority remains `docs/evidence/E0D_P02_PAIR_WINDOW_DIRECTOR_AMENDMENT_2026_09_16.md`; live authority is `docs/evidence/E0D_P02_SLOT2_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_16.md` plus `docs/evidence/E0D_Q_E0D_01_P02_SLOT2_PREEXECUTION_ACTIVATION_2026_09_16.md`.

- pair: `E0D-P02-OMNISCIENT-CONTEXT`
- slot: `P02-FULL`
- variant: `E0D-FULL-REFERENCE-01`
- RunId: `E0D-Q01-P02-FULL-20260914-04`
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0D-Q01-P02-FULL-20260914-04`
- executable commit: `8770a6361233e8a877a966c45eb6f62c5b3ca182`
- executable SHA-256: `651aedf9e05800ea46ecc8a7c6565f9f91eeaf07794747dcb0a3c7697b946fac`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- model/profile: `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL`
- amended P02 pair window: `2026-09-16T19:33:00Z..2026-09-17T04:33:00Z`

Immediately before launch, fresh authenticated AI Studio evidence confirmed `Ensemble Testing`, project ID `gen-lang-client-0793779417`, credential label `Gemini API Key`, Free tier, exact `Gemini 3.5 Flash Lite`, and live capacity `7 / 15` RPM, `10.83K / 250K` TPM, `17 / 500` RPD. Final preclaim census at `2026-09-16T23:52:25Z` confirmed exact Project main, exact absent Slot-2 root/run claim/root claim, zero Harness processes, absent ambient `GEMINI_API_KEY`, clean admitted validator/tag/executable identity, canonical fixture presence, and protected credential readiness.

## Consumption and terminal result

The Director crossed the protected-credential boundary locally on SurfSeven without placing plaintext key material in chat, command arguments, Git, evidence, or the ambient environment. The run claim was created at `2026-09-17T00:00:50.4584510Z`, the root claim at `00:00:50.4609607Z`, and the evidence root at `00:00:50.4629594Z`. P02 Slot 2 is therefore permanently consumed. `run.final.json` sealed at `00:01:50.1906254Z`.

Terminal result: **`InvalidOutput` at 2/12 accepted turns**. Final state hash is `537d2d35042947f74b399e83ace40c55281dcbf88dcd891a2482bbcbeec4b8bb`; final opportunity is `VOSS`. Sealed shadow estimate is USD `0.00699480`; `hasUnknownProviderUsage=false`.

## Deterministic invalid-output boundary

Turns 1 and 2 completed Performer -> Integrity -> Interpreter -> State Authority -> least-intervention opportunity -> atomic commit. Turn 3 reached the Performer only. The provider returned `Success` from exact model `gemini-3.5-flash-lite`, but structured control contained `addressedCharacterIds=["MARLO"]` instead of canonical roster ID `MARLOWE`.

`PerformerCandidateContract` deterministically rejects an addressed Character ID outside the roster, and `E0AReferenceRunDriver` maps that parser exception to terminal `InvalidOutput`. The rejection occurred before a Turn-3 `performer.candidate`, Integrity invocation, Interpreter invocation, Take, commit, or accepted-history entry. `transcript.json` contains exactly the two accepted performances. The preserved evidence shows no transport, quota, credential, model-route, fixture, or source-regression signal; the deterministic terminal is the invalid canonical control ID.

## Provider accounting

Seven role invocations reached the provider: three Performer, two Integrity, and two Interpreter. Each invocation performed one `countTokens` preflight and one generation request, for **14 Gemini API operations** total.

Generation usage totals are:

- input tokens: `10641`;
- output tokens: `1521`;
- reasoning tokens: `856`;
- cached input tokens: `0`;
- cache-write tokens: `0`.

All seven generation receipts returned exact model `gemini-3.5-flash-lite`; all provider usage is known and the final shadow estimate remained within verified pricing assumptions.

## Runtime seal and hard-gate review

All three context-composition events retained the Full reference Character-bounded access contract: other-Character private records and Production-authority records remained denied. The two accepted turns have deterministic State Authority decisions and traceable atomic commits; Character claims remain subject to deterministic authority/disclosure rules. The rejected Turn-3 provider output never became a candidate or fiction/history. No provider failure became fiction and no deterministic access/cost/cancellation/eligibility rule was delegated to a model.

Runtime evidence sealed 25 artifacts. Before evaluation publication, the admitted executable's evidence authority recomputed every rooted runtime digest, runtime root, seal identity, artifact binding, and summary binding; evaluation sealing succeeded only after that verification passed. Evidence-wide searches found zero `GEMINI_API_KEY` or `AIza` credential-marker hits.

The exact admitted executable sealed the Full-reference hard-gate evaluation at `2026-09-17T00:09:36.2123729Z` with reviewer `ENGINEERING-SOL`, method `SEALED-EVIDENCE-HARD-GATE-REVIEW-V1`, result **PASS**, findings `0`. Final evaluated evidence count is `29` files.

- runtime root: `289bae9c6f429507e85498cc9d1f538c19e445c0cc22f87c6580d1c2997b8c1c`
- runtime seal identity: `7311544d5973c6d82a0aafde0e70dc7ce195e769d0a9927b2da0d8b06aa01e9d`
- `run.final.json` SHA-256: `1615afab96a51270331deda5756a19c74d0d4bb2f9d27c54267835226043c2d5`
- `evaluation/hard-gates.json` SHA-256: `be13aa87db411dcd95006e2b373516c12a21feba2c9c6ae597fd4767551fb65a`
- `evaluation.final.json` SHA-256: `562866b2e563a8d15b2517339a148283a70ec62ea6d3b193c77f34a950f75ec5`

## Contribution, pair closure, and successor boundary

P02 Slot 2 is **NONCONTRIBUTING** because it reached only 2/12 accepted turns. Under the frozen E0-D method, `InvalidOutput` earns no retry, replay, replacement, fallback, paid/Priority route, alternate model/profile/fixture, retune, or automatic window extension.

Both preregistered P02 members are now permanently consumed/noncontributing. Matched pair `E0D-P02-OMNISCIENT-CONTEXT` is therefore **fully consumed and experientially ineligible**; there is no blind experiential scoring, pair comparison, rescue run, or replacement to perform.

The frozen method does not cancel later preregistered pairs merely because P02 is noncontributing. P03 remains frozen and unconsumed but is **not authorized by this closeout**. Any P03 live execution requires its own separate Director authorization, fresh execution-sensitive provider/account/capacity evidence, exact identity/interlock verification, and execution inside its frozen `2026-09-18T14:30:00Z..23:30:00Z` window. Q-E0E-RUN remains blocked until E0-D closes.