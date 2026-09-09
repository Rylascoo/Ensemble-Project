# Q-E0A-03 Reference-Evidence Planning Recursive Audit — 2026-09-09

Status: **PASS — PLAN FROZEN; PROVIDER TRAFFIC REMAINS UNAUTHORIZED**

Scope: recursive audit of the Q-E0A-03 planning package against current `main`, frozen E0-A architecture, Q-E0A-04 machine validation, provider/account decisions, evidence law, blind-review law, experiment order, and E0-E reference-binding requirements.

Primary contract: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.

## 1. Authority/state resolution

Authoritative engineering state before this package:

- `main` = `8842dac123d31851dee78dd91b760db61045655d`;
- Q-E0A-01, Q-E0A-02, Q-E0A-04 = DONE;
- Q-E0A-03 = BLOCKED;
- promoted native executable = `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`;
- validation tag = `validation/e0a-gemini-counttokens-input-projection-native-arm64`;
- tag object = `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`;
- provider authorization = NONE.

No source/test/fixture change is part of this planning package.

## 2. Audit findings and corrections

### A. Stale hypothesis continuity — CORRECTED

`docs/HYPOTHESIS_LEDGER.md` still described the pre-Q-E0A-04 state: 3.5 Flash-Lite paused, 3.1 deferred pending the bounded diagnostic, Attempt 03 as the operative compatibility evidence, and the exact rejected field as unknown.

That is inconsistent with Attempt 04 plus Q-E0A-04. The ledger is updated to distinguish:

- exact old failure diagnosed;
- input-projection correction machine-validated/tagged;
- full generation compatibility still unproven;
- historical pause/deferral prerequisite satisfied but no provider route automatically authorized;
- Q-E0A-03 remains the live network/evidence gate.

### B. Disposable compatibility probe — REJECTED

A standalone post-Q-E0A-04 live probe would consume provider traffic without producing full reference evidence. The corrected full Q-E0A-03 run already fails closed at the first provider boundary and therefore supplies compatibility evidence if it terminates early.

The plan freezes the next real 3.5 request as the first actual Q-E0A-03 full-reference attempt, subject to separate authorization.

### C. Comparison-order ambiguity — RESOLVED

The later Free-Tier RPD model-selection amendment supersedes the older comparison order where they conflict. Current order is:

1. 3.5 Flash-Lite Minimal — 12-turn primary full candidate;
2. 3.1 Flash-Lite Minimal — 12-turn full comparator;
3. 2.5 Flash-Lite None — three-turn exact-no-thinking control.

Each remains separately authorized. No earlier outcome silently activates a later run.

### D. Three-turn control promotion risk — CLOSED

The 2.5 Flash-Lite route is explicitly a three-turn control. It cannot become the E0-A full reference, even if both 3.x candidates fail. This prevents a quota-limited control arm from silently redefining the reference experiment.

### E. Success/failure permutation ambiguity — RESOLVED

The parent amendments define contribution and comparison but do not fully specify every later-provider failure permutation. The planning contract freezes:

- both full candidates contribute -> blind compare, unblind, frontier adjudication;
- exactly one contributes after the other produces a separately authorized terminal/noncontributing result -> the contributor may be selected, with failure retained as reliability evidence;
- comparator becomes externally inadmissible before authorization -> no call; any single-candidate selection requires explicit Director limitation record;
- neither contributes -> Q-E0A-03 remains open; no control substitution;
- control unavailable/fails -> report it; no silent skipping and no automatic invalidation of a justified full reference.

### F. Post-hoc quality weighting risk — CLOSED

The current comparison law calls for a quality/speed frontier but does not freeze a scalar utility function. Inventing weights after seeing transcripts would be methodologically unsafe.

The plan reuses the already-frozen E0-E experiential dimensions as a blind vector and forbids post-hoc weighted aggregation. If both full candidates are non-dominated, the Director selects after blind evidence is sealed/unblinded and records the tradeoff rationale.

### G. Blind-review contamination — CONTROLLED

Quality comparison uses only normalized blind Character-visible transcripts. Provider/model/RunId/hash/implementation metadata remains excluded. Left/right mapping is sealed separately before reviewer exposure; scoring seals before unblinding. Hard-gate review remains a distinct non-blind evidence layer and cannot rewrite runtime history.

### H. Fixture/configuration provenance — FROZEN

Canonical comparison Fixture is:

- `ensemble.e0.missing-raft@0.1.0`;
- SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- initial Opportunity `VOSS`.

The first profile's current manifest reference-configuration identity is deterministically produced by existing code from the RPD comparison identity plus `CREATIVE-MINIMAL` and `GEMINI-3.5-FLASH-LITE-MINIMAL`. The identity's 2026-09-07 naming is a frozen configuration namespace, not an execution-date claim, so it should not be renamed for Q-E0A-03.

### I. Provider-fact cadence — RECONCILED

The 2026-09-09 Director quota decision supersedes only repeated manual quota-limit rereads for the exact unchanged 3.5 route. It does not waive model/pricing/data-use/lifecycle checks, contrary-signal triggers, or separate authorization. It also does not extend to 3.1 or 2.5.

The current executable's source guard expires after 2026-09-14. Execution after that date requires a source snapshot refresh and renewed native validation/tagging; documentation cannot bypass the executable guard.

### J. Generation behavior remains genuinely unverified — PRESERVED

Q-E0A-04 proves only the corrected token-preflight implementation and native runtime behavior without provider network. No current comparison profile has yet completed a real successful generation path. HYP-004 therefore remains open and the first successful Q-E0A-03 generation is evidence, not assumed compatibility.

### K. Reference descriptor requirement — MADE EXPLICIT

E0-E preparation already requires a sealed E0-A reference configuration containing source run/seal/configuration/provider/model/settings/fixture/envelope facts. Q-E0A-03 closure therefore cannot stop at a 12-turn run or a selected model name. The plan requires a durable post-selection reference descriptor traceable to the immutable run and compatible with the prepared E0-E reference fields.

No new runtime source is required merely to state this closure requirement. If later implementation is needed to mechanize descriptor creation, that becomes a separate audited source task and must not be smuggled into a provider run.

## 3. Negative-space audit

The plan does not:

- authorize any provider request;
- authorize key entry or spend;
- alter the promoted executable or native validation authority;
- change Core, Fixture, prompts, schemas, generation bytes, structured output, rate pacing, token accounting, retry/fallback, cancellation, evidence sealing, hard-gate semantics, or causal authority;
- activate E0-B or E0-E;
- admit Gemini Free Tier for private/Production-derived material;
- treat the 2.5 control as a reference candidate;
- treat one failed run as authorization for another;
- invent a quality-weight formula after evidence exists.

## 4. Recursive pass

After applying the corrections above, the package was re-read against:

- `CURRENT_STATE.md`;
- `docs/PROJECT_AUTHORITY.md`;
- `docs/PROJECT_EXECUTION_QUEUE.md`;
- `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_FREE_TIER_RPD_MODEL_SELECTION_AMENDMENT.md`;
- `docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`;
- Attempt-04 archive audit;
- 3.5 quota-reuse Director decision;
- promoted source/evidence contracts;
- E0-E preparation reference-binding and blind scoring contract.

That pass found no additional material correction or worthwhile improvement to the plan.

## 5. Disposition

- Q-E0A-03 planning/closure method: **FROZEN**;
- Q-E0A-03 provider execution: **BLOCKED**;
- provider authorization: **NONE**;
- first prospective live profile: `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`;
- next lawful network step: only after applicable public provider facts are freshly checked and the Director separately authorizes one exact full-reference run.
