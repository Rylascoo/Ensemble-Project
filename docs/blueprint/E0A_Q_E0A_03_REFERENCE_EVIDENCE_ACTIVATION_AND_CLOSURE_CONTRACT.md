# E0-A Q-E0A-03 — Reference Evidence Activation and Closure Contract

Status: **FROZEN — DIRECTOR-APPROVED PLANNING CONTRACT 2026-09-09 — PROVIDER TRAFFIC NOT AUTHORIZED**

Authority: subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, Blueprint 0.1, the E0-A Phase-B reference envelope and its current Gemini amendments. This contract resolves Q-E0A-03 activation, run-order, contribution, comparison, reference-selection, and closure procedure. It does not authorize credentials, provider traffic, inference, spend, retry, fallback, phase advance, or source changes.

## 1. Objective

Q-E0A-03 completes the E0-A Same-Model Isolated Cast reference evidence package and seals the reference configuration required by E0-B onward and the prepared E0-E control.

The next real provider request is not a disposable compatibility probe. The first separately authorized Q-E0A-03 run is itself the compatibility test and, if the provider path succeeds, the reference experiment. A technical/provider failure remains immutable condition evidence and consumes only that exact run authorization; it never creates retry/fallback authority.

## 2. Frozen executable and Fixture baseline

The current eligible executable authority is exactly:

```text
checkout   3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2
tag        validation/e0a-gemini-counttokens-input-projection-native-arm64
tag object b9341c1c0ab48be5370ed22e7d2511f9e3be00d9
```

Native Windows ARM64 authority at that checkout is Core 622/622, Harness 131/131, fresh Harness build, Missing Raft/generic smokes, and credentialless current-profile gates PASS. Provider network was not performed during validation.

The comparison Fixture is frozen as:

```text
id/version ensemble.e0.missing-raft@0.1.0
SHA-256    5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703
scene       SCENE-MISSING-RAFT
roster      MARLOWE / VOSS / WREN
initial Opportunity VOSS
```

No fixture, Core, prompt, schema, deterministic authority, retry, cancellation, evidence, causal, or accepted-turn semantics may drift inside Q-E0A-03 without a separately audited amendment and renewed validation when executable bytes change.

## 3. Comparison sequence

Subject to the per-run activation gates below, the frozen comparison order is:

```text
1. CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL
   model: gemini-3.5-flash-lite
   accepted-turn cap: 12
   role controls: Performer minimal / Integrity high / Interpreter minimal
   purpose: primary full-reference candidate

2. CREATIVE-MINIMAL / GEMINI-3.1-FLASH-LITE-MINIMAL
   model: gemini-3.1-flash-lite
   accepted-turn cap: 12
   role controls: Performer minimal / Integrity high / Interpreter minimal
   purpose: full-reference efficiency/cost comparator

3. CREATIVE-NONE / GEMINI-2.5-FLASH-LITE-NONE
   model: gemini-2.5-flash-lite
   accepted-turn cap: 3
   role controls: Performer thinkingBudget=0 / Integrity thinkingBudget=3584 / Interpreter thinkingBudget=0
   purpose: exact-no-thinking control only
```

Each run requires its own explicit Director authorization. Completion or failure of an earlier run never authorizes the next. `GEMINI-2.5-FLASH-LITE-NONE` is a bounded control and **cannot be selected as the E0-A reference configuration** because it is not a full 12-turn reference candidate.

## 4. Per-run activation gate

Before each real run, all applicable conditions must hold:

1. exact intended executable checkout is already machine-validated and annotated-tagged;
2. checkout is detached/clean on native Windows ARM64 and the canonical Fixture hash matches;
3. the run uses a fresh RunId and a nonexistent evidence root;
4. provider/model/profile remains available and materially compatible with the frozen API surface;
5. current pricing, lifecycle, Free-tier/data-use terms, and other volatile public provider facts are reverified;
6. the executable pricing/data-use freshness guard remains valid; the current executable fails closed after 2026-09-14, so execution after that date requires an audited snapshot source change plus renewed native validation/tagging;
7. account/project/key association and sufficient quota remain admissible for the selected profile;
8. for `gemini-3.5-flash-lite`, the Director-approved 15 RPM / 250,000 input TPM / 500 RPD snapshot may be reused until one of its recorded contrary-signal triggers occurs; this reuse does not extend automatically to 3.1 or 2.5 Flash-Lite;
9. the API key is supplied only on the Director machine through the process environment and never persisted in evidence;
10. the Director explicitly authorizes exactly one named run, profile, Fixture, executable, retry/fallback boundary, and evidence root.

Provider authorization remains NONE until condition 10 is separately satisfied.

## 5. Real-run execution semantics

Every authorized run uses the existing frozen envelope:

```text
attempts per probabilistic role invocation  1
automatic retries                           0
automatic fallback                          none
attempt timeout                             300 seconds
visible max output                          4096 tokens/role
shadow spend ceiling                        USD 5.00/run
```

The corrected `countTokens` input-semantic projection is the first provider-bound preflight for each role. If it or any later provider boundary fails, the run terminates according to existing technical-failure law. If the first Performer token preflight succeeds, the same authorization permits the same run to proceed through generation and the frozen Performer -> Integrity -> Interpreter -> Take/commit -> immediate next-Opportunity cycle until its profile cap or another terminal condition.

There is no standalone compatibility probe, retry, replacement attempt, fallback model, or same-authorization rerun.

## 6. Runtime contribution law

A 12-turn full-reference candidate contributes only if all existing reference-envelope requirements hold, including:

- manifest exists before provider work and exact provenance is complete;
- provider receipts remain bound to the configured run/role/turn/context/profile/request identities;
- observable model identity stays fixed through the contributing run;
- all 12 Turns reach accepted Take, causal commit, and the following deterministic Opportunity;
- no technical, cancellation, budget, provider-usage-policy, cache, or Integrity terminal occurs;
- runtime artifact seal is complete and independently verifiable;
- post-run hard-gate evaluation is sealed and passes.

Any terminal/noncontributing run remains immutable evidence. Its failure mode, accepted-turn yield, latency, usage facts available before termination, and technical diagnosis are reported and cannot be hidden by selecting only clean runs.

The three-turn 2.5 Flash-Lite control may be valid control evidence if it reaches its exact configured cap and passes applicable gates, but it never becomes a full-reference candidate.

## 7. Post-run hard-gate evaluation

Runtime sealing precedes evaluation. For every candidate intended to contribute, the existing `ensemble.e0a.hard-gates.v1` checklist is reviewed independently against the sealed evidence and recorded through the existing evaluation seal.

A hard-gate failure invalidates experiential contribution without rewriting runtime history. Technical/noncontributing runs do not receive fictional completion credit. Hard-gate review and blind quality review are separate evidence layers.

## 8. Frozen blind quality comparison

If two full-reference candidates contribute, their accepted Character-visible transcripts are compared blind before provider/profile mapping is revealed.

Use the same E0-A blind-transcript normalization law already frozen for E0-E: no prose edits, no turn merging/splitting, no added stage directions, no removal of embarrassing-but-valid output, and no provider/model/run/hash metadata in the blind transcript.

The quality dimensions are frozen before new candidate output exists:

1. Character distinction;
2. Social causality;
3. Character agency;
4. Relationship expression;
5. Coherence;
6. Earned surprise;
7. Assistant convergence avoidance;
8. Anticipation;
9. Memorability;
10. Mechanical invisibility.

For each dimension the blind record is `LEFT`, `RIGHT`, `TIE`, or `INDETERMINATE`, with brief evidence. Review also records which transcript feels most like distinct people, which makes later actions feel most caused by earlier actions, which preserves asymmetric knowledge most convincingly, which creates stronger anticipation, and whether the experiential difference is meaningful.

The left/right mapping is sealed separately before reviewer exposure and withheld until the scoring record is sealed. No weighted aggregate score is invented after seeing results. Quality remains a vector of frozen judgments.

## 9. Speed/resource comparison

After blind quality records are sealed, unblinded comparison may use the runtime provenance already required by the Gemini comparison amendments:

- contract/structured-output success;
- accepted-turn yield and terminal status;
- `countTokens` latency;
- role/provider latency;
- committed-turn wall time and total run wall time;
- accepted turns per minute;
- input/output/reasoning tokens;
- shadow cost per accepted turn and per run;
- technical failure/reliability evidence.

No cheapest/newest/highest-RPM default winner exists. No post-hoc scalar weighting of quality versus latency is introduced.

## 10. Reference-selection disposition

Only a contributing 12-turn candidate may become the E0-A reference.

- **Both 3.5 and 3.1 contribute:** seal blind comparison first, unblind, place both on the quality/committed-turn-latency frontier, then the Director selects the reference with an explicit evidence-based rationale. Split/non-dominated tradeoffs remain visible rather than being forced through an invented weighted score.
- **Exactly one full candidate contributes after the other receives its separately authorized terminal/noncontributing result:** the contributing candidate may be selected as the reference; the failed candidate remains reliability/compatibility evidence.
- **A full comparator becomes externally inadmissible before authorization** because of model removal, terms, project/tier constraints, or another material provider fact: it is not called. A contributing remaining full candidate may be selected only after the Director explicitly records the comparator-unavailability limitation.
- **Neither full candidate contributes:** Q-E0A-03 remains open/blocked. The three-turn control cannot substitute. Diagnose the evidence and create a separately audited correction/route decision before any further provider traffic.
- **2.5 Flash-Lite control fails or becomes inadmissible:** preserve/report that fact. It does not by itself invalidate an otherwise justified full-reference selection, but omission of an admissible planned control requires explicit Director disposition rather than silent skipping.

Every provider run in these branches remains separately authorized. This contract is not standing network authority.

## 11. Sealed reference descriptor

Q-E0A-03 cannot close merely because a run reaches 12 turns. After final reference selection, create a durable sealed descriptor traceable to the selected E0-A runtime evidence containing at least:

```text
source E0-A RunId
source manifest SHA-256
source runtime-seal identity / run.final digest
referenceConfigurationIdentity
provider identity
requested model identity
provider-returned model/version identity when available
provider profile / service-tier identity
creative reasoning/thinking control + exact value
streaming mode
visible max-output ceiling
accepted-turn cap
attempts per invocation
automatic retry count
attempt timeout
estimated run spend ceiling
Fixture id / version / canonical hash
executable commit
annotated native-validation tag
hard-gate evaluation identity/digest
blind-comparison record identity when applicable
```

The descriptor must satisfy the prepared E0-E `E0EReferenceConfiguration` field requirements for the overlapping fields and must independently bind to the selected sealed E0-A evidence. It is post-run evidence, not a mutation of runtime history.

## 12. Q-E0A-03 closure package

Q-E0A-03 is DONE only when durable project evidence contains:

1. every separately authorized Q-E0A-03 run terminal record and sealed archive/audit;
2. independent runtime-seal verification for the selected reference run;
3. passing hard-gate evaluation for the selected run;
4. blind comparison/scoring record when two full candidates contribute, sealed before unblinding;
5. comparison summary that reports noncontributing/failed runs rather than suppressing them;
6. explicit Director reference-selection decision;
7. sealed reference descriptor;
8. `CURRENT_STATE.md` and `docs/PROJECT_EXECUTION_QUEUE.md` transition Q-E0A-03 to DONE and activate only its lawful successor.

Q-E0B-01 is the immediate experiment-order successor. E0-E execution remains blocked until E0-A -> E0-B -> E0-C -> E0-D have closed even though its preparation is already complete.

## 13. Exclusions

This contract does not authorize or require:

- provider traffic or credential use;
- another compatibility-only probe;
- Core/Fixture/prompt/schema/runtime redesign;
- automatic retry/backoff/fallback;
- profile substitution or new model admission;
- post-hoc prompt tuning between comparison runs;
- weighted aggregate quality scoring invented after output exists;
- E0-B/E0-C/E0-D/E0-E execution;
- Production-provider admission or private/user-derived material on the Free-tier route.

## 14. Current gate

Planning is frozen. Provider authorization is **NONE**. Q-E0A-03 remains **BLOCKED FOR PROVIDER EXECUTION** until the immediately applicable external provider facts are reverified and the Director separately authorizes the exact first `GEMINI-3.5-FLASH-LITE-MINIMAL` full-reference run.
