# E0-C Q-E0C-01 — Two-Slot Recursive Audit and Process Hardening

Date: 2026-09-13

Status: **AUDIT COMPLETE — PRESERVED RUN OUTCOMES UNCHANGED — CONCURRENT-REVIEW EXPECTATION RECONCILED AS PROCESS GAP — NO PROVIDER TRAFFIC**

## Authority and scope

Audit baseline: `Rylascoo/Ensemble-Project@aceaa9db65e25eb993081b96aaf674aa8211585a`, after PR #127 and exact-main Validation #791.

The controlling E0-C Director ratification is Project Issue #107 comment `5650353003`. It defines exactly two fresh repeats of selected Run 08, freezes order as **Slot 1 then Slot 2**, and requires each slot to satisfy its own fresh execution-time gates inside one immutable UTC batch window. Canonical preregistration and both activation/terminal records implement that experiment role.

A post-execution Director clarification in the current Engineering audit stated that “slot 2 was supposed to be a concurrent audit.” Fresh repository/Issue search finds no durable `concurrent audit` requirement in the E0-C method, activation law, or Issue #107. Because the frozen experiment already defines Slot 2 as the second fresh repeat, this clarification must not retroactively relabel the consumed experimental Slot 2 or alter its outcome. The recoverable process meaning is instead treated as a missing **concurrent review/audit gate**: if a second Engineering/audit surface was intended to supervise Slot 2 activation, that requirement was not durably encoded as a launch prerequisite.

This audit therefore separates three questions: whether the two runs preserved the frozen condition; whether each launch obeyed execution law; and what process hardening is warranted. It does not rescore, replay, regenerate, replace, unblind, send provider traffic, or advance E0-D.

## Slot 1 — evidence and execution audit

Slot 1 `E0C-Q01-IDENT-20260912-01` is internally coherent. Its manifest preserves the exact fixture hash `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`, executable `bb869fb1c505603612bc718f739b3f1b358e5539`, `CREATIVE-MINIMAL`, `GEMINI-3.5-FLASH-LITE-MINIMAL`, 12-turn cap, one attempt per role invocation, zero retries, 300-second timeout, USD 5 shadow ceiling, and the frozen prompt/schema hashes.

The source-derived run/root claim files exist and bind the exact preregistered RunId/root. The launch ran once from `2026-09-13T20:59:27.8798874Z` through `21:04:34.8805953Z`, inside the frozen `20:30Z..23:30Z` window, and ended `AcceptedTurnCapReached` at 12/12 accepted turns. Fresh authenticated pre-Slot-1 rate-limit/tier/capacity evidence had been separately preserved before activation.

Raw evidence recheck found 102 runtime-sealed artifacts plus four evaluation/seal files (106 final files), zero contradiction with the preserved runtime/evaluation identities, and the recorded hard-gate PASS. Slot 1 made 72 Gemini API operations and returned exact `gemini-3.5-flash-lite` on all generation terminals. No new Slot 1 defect was found.

## Slot 2 — condition identity, terminal, and protocol separation

Slot 2 `E0C-Q01-IDENT-20260912-02` preserves the same fixture, executable, variant/profile, turn/attempt/retry/timeout/spend bounds, roles, prompt hashes, and schema hashes as Slot 1. Direct comparison of the first Performer request shows its `requestBodyUtf8` is byte-for-byte identical to Slot 1's first Performer request; only run-specific attempt/prepared identities differ. The `InvalidOutput` therefore cannot be attributed to a drift in the frozen first-turn request condition.

The source-derived Slot 2 claims exist and bind the preregistered RunId/root. Raw evidence contains nine runtime-sealed artifacts plus four evaluation/seal files (13 final files), consistent with the terminal record. The provider preflight and generation both succeeded; the exact returned model was `gemini-3.5-flash-lite`.

The generated performance text spelled “Marlowe” normally, but the typed control emitted `addressedCharacterIds=["MARLOE","WREN"]`. `MARLOE` is not canonical `MARLOWE`. The frozen prompt explicitly required exact case-sensitive IDs copied from `context.rosterCharacterIds`, and the deterministic parser correctly rejected the malformed control before a Candidate, Integrity call, Interpreter call, Take, commit, accepted performance, or state mutation entered Production. This is a genuine stochastic/model-output failure under the frozen condition, not evidence of parser, transport, credential, schema-transport, or source regression.

Separately, Slot 2 had an **execution-process breach**. Its activation reused the authenticated pre-Slot-1 `42/500 RPD`, `9/15 RPM`, `22.07K/250K TPM` snapshot plus conservative arithmetic and elapsed minute windows instead of obtaining the required fresh authenticated post-Slot-1 model/tier/quota/capacity observation. The standing association amendment kept quota/capacity execution-sensitive. Slot 2 therefore should have remained blocked even though the eventual provider call succeeded. Provider success does not cure that breach, and the breach does not causally explain `MARLOE`.

## Deterministic corrections made by this audit

The Slot 2 terminal evidence contained two text-integrity defects: a literal `` `r`n `` sequence had fused the hard-gate and evaluation SHA lines, and the three P01/P02/P03 pair lines contained mojibake in place of an em dash. Those encoding/formatting defects are corrected without changing evidence meaning, hashes, eligibility, or outcome.

No runtime evidence, sealed artifact, score, mapping, source, test, fixture, claim, credential, provider record, or experiment result is changed.

## Deferred hardening — no retroactive E0-C mutation

Three improvements are warranted at the next authority-appropriate implementation/convergence boundary:

1. **Execution-sensitive gate provenance must be machine-verifiable.** A fresh-capacity gate should carry authenticated-observation provenance and a timestamp demonstrably later than the predecessor terminal when the method requires post-predecessor freshness. Standing evidence or arithmetic must not be able to satisfy such a gate accidentally.
2. **Concurrent review must be explicit when intended.** A concurrent Engineering/audit surface is not a valid launch gate merely because another chat or agent exists. Future launch contracts that require independent/concurrent review should name the reviewer role, exact baseline, required evidence, race check, and PASS-before-launch condition durably.
3. **Consider roster-bounded structured-output constraints after the frozen E0 boundary.** The current Performer JSON schema permits arbitrary strings in `addressedCharacterIds`/`nominatedCharacterId`, while the prompt and deterministic parser enforce roster membership. A future implementation audit should evaluate generating schema enums from the bounded roster to reduce preventable ID typos while retaining deterministic validation. This is not an authorization to change the E0-C executable or reinterpret Slot 2.

These are hardening findings, not causal retrofits. Q-E0C-01 remains **ACTIVE / DIRECTOR HOLD**. Slot 1 remains contributing; Slot 2 remains permanently consumed/noncontributing with no replay/replacement/third slot. The already-sealed P01 score and mapping evidence remain immutable under the current hold. Q-E0D-01 remains blocked and provider traffic remains **ZERO**.

## Recursive-audit stop condition

One complete pass after the corrections found no additional Slot 1 evidence defect, no Slot 1/Slot 2 frozen-condition drift, no contradiction in either runtime/evaluation seal, and no basis to alter either consumed outcome. The remaining consequential question is the existing Director E0-C disposition; the hardening items above remain deferred and cannot be used to reopen the consumed batch.
