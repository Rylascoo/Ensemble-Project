# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.6 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through a real Same-Model Isolated Cast while provider execution, semantic review, spend, cancellation, credentials and evidence remain outside Core and every disclosure/result remains attributable?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0008/0018
 -> Interpreter -> State Authority -> Accepted Take/commit
 -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; all three Characters use the same frontier model; provider/model/settings stay fixed within a batch; E0-C/D/E return to that reference unless explicitly varied; provenance is complete; technical failure never becomes fiction; LLMs never own access/cost/cancel/retry/state/Take/commit authority.

## 2. Fixed probabilistic roles

Primary reference:

```text
Provider            OpenAI official API
Model               gpt-5.6-sol
Performer reasoning none
Interpreter reasoning none
Integrity reasoning high
```

All three Performers use the same `none` profile. Interpreter is also `none`. Integrity uses the same model/provider at `high` because its job is strict rubric application, not Character portrayal. Integrity remains concern evidence only: deterministic Patch 0008 owns disposition and cannot be waived by the assessor.

Director, Access, State Authority, Take, commit and Opportunity remain deterministic Core authority.

## 3. Reasoning characterization

`PERFORMER-NONE` is normative. First characterization varies **Performer reasoning only**:

```text
PERFORMER-NONE    Performer none    / Integrity high / Interpreter none
PERFORMER-LOW     Performer low     / Integrity high / Interpreter none
PERFORMER-MEDIUM  Performer medium  / Integrity high / Interpreter none
PERFORMER-HIGH    Performer high    / Integrity high / Interpreter none
```

Fixture, prompts, Context, model, assessor, Interpreter, deterministic authority, service tier, timeouts, output/spend ceilings and run protocol stay matched. `xhigh`/`max` deferred. A later Interpreter-only characterization may hold Performers at `none`; neither replaces NONE as E0-C/D/E reference.

For later cross-provider E0-B record both semantic intent `LeastDeliberativeSupported` and the exact provider-native control. `none`, disabled, `minimal`, or low are never claimed equivalent.

## 4. Harness boundary

Core stays unchanged. Harness gains logical components only:

```text
Run        provider-neutral policy/driver + fakeable role adapter
Integrity  bounded assessment-packet builder/parser
Evidence   local experimental evidence
OpenAI     HTTP provider adapter
Host       CLI + credential/cancellation edge
```

Harness references Core only. Provider/network/filesystem/secrets stay outside Core. Add a Harness test project. No product Application layer or post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API via `HttpClient`; reverify API/model immediately before implementation and contributing batch. No provider conversation/`previous_response_id`; no tools; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs; request/store no reasoning summary/hidden reasoning.

Role settings:

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=2048
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

Characterization changes only Performer reasoning.

Freeze an immutable model snapshot if OpenAI exposes one suitable for the batch. Otherwise record requested alias plus every returned model/version identifier available; never claim unobservable snapshot stability. Observable identity change in-batch fails closed.

## 6. Versioned prompts and exact disclosure

Freeze exact UTF-8 + SHA-256 for three prompt contracts before contributing execution:

```text
PerformerPromptContract
IntegrityAssessorPromptContract
StateInterpreterPromptContract
```

No in-batch tuning.

Performer receives exact Character-bounded rendered Context + Candidate schema only.

Interpreter receives the same bounded source Context + exact Candidate Performance/control + mutation-proposal schema; no full Production, State Authority locks/snapshot, denied/private state or credentials.

Integrity receives a separately constructed `E0AIntegrityAssessmentPacket`, never the Performer request. It contains exactly:

```text
Source Context identity + CharacterId
Candidate visible text + typed control
Character-visible rendered Context
active denied Production records with AccessReason
active SystemImmutable/CreatorLocked records
five Patch 0008 concern definitions
```

Denied records are selected from the exact current Production state by current Access decisions. Inactive records are excluded. Protected records are deduplicated by RecordId. The assessor receives no unrelated provenance graph, commit history, provider credentials or creator metadata.

This extra disclosure is trusted experimental Integrity input, never Character knowledge. It is recorded separately from the Character request.

Integrity strict output is only an ordered unique array of Patch 0008 concern enum names; no rationale/confidence/free text. The Harness binds those concerns through canonical `IntegrityConcernEvidence.Bind`, then Patch 0008 deterministic validation/Turn progression. Assessor technical failure produces **no concern evidence and no Integrity disposition**; the run terminates.

## 7. Prepared attempts and identity

Before any provider request write an immutable prepared-attempt record containing:

```text
RunId / attempt / role / turn / CharacterId when applicable
provider / requested model / role reasoning
ContextPacketId / structured + rendered hashes
prompt + schema hashes
request-body SHA-256 / max output / service tier
```

For Integrity also record assessment-packet SHA-256 plus included denied/protected RecordIds and AccessReasons. Exact request body is written before disclosure. Never record auth headers/secrets. Couple response ID, observable model identity, service tier, status, usage and terminal metadata to that attempt.

Manifest supplies one Core `RunId`; run must fail if its directory exists. Derive IDs without clock/randomness:

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId length for Core CanonicalId limits. Attempt identity is Harness metadata.

## 8. Fixed envelope / spend

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 still establish next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; this is experimental, not Scene ending (ODR-13 remains open).

Timeout/refusal/provider/transport/incomplete-invalid output/cancellation of Performer, Integrity or Interpreter terminates without replacement. E0-F may later vary failures/retries.

Manifest freezes rate assumptions for **estimated model-token cost**; provider billing remains external authority. Before each inference: prepare exact request; obtain supported Responses input-token count; reserve worst-case uncached input + that role's max output; refuse before inference if cumulative estimate exceeds USD 5.00. After response replace reservation with estimated actual cost from returned usage, distinguishing cached/reasoning classes where supported. Token-count calls are preflight evidence, not role attempts. `BudgetExceeded` is non-fictional terminal evidence.

## 9. Integrity / State Authority behavior

For a correctly bound Candidate:

```text
Integrity assessor completed
 -> strict concern array
 -> IntegrityConcernEvidence.Bind
 -> Patch 0008 deterministic Validate / Patch 0018 EvaluateIntegrity
```

No concerns -> Accept. Any concern -> `RequestAnotherTake`; because reference retries are zero, run terminates before Interpreter/Take/commit. Deterministic Reject remains earlier hard short-circuit and does not invoke semantic review. Technical assessor failure gives no disposition and terminates. This satisfies Patch 0008's requirement that effective E0 progression authenticate configured concern-review provenance rather than use synthetic empty evidence.

Reference `StateAuthorityPolicy` includes all Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 still owns hard rejection/mandatory review. On `ReviewRequired`, supply explicit Reject choices for every unresolved review mutation through Patch 0018, recorded as `E0AReferenceDeterministicReject`, never human/creator review. Mandatory-review mutations never auto-approve; Performance may still become Accepted under Patch 0011; rejected proposals/reasons remain provenance.

## 10. Provider mapping / loop

Performer completed strict output -> raw/stream diagnostics -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 BindCandidate -> Turn GateAttempt. Provider/refusal/timeout/incomplete/invalid -> TechnicalFailure; explicit cancel -> Cancelled; partial output diagnostics only.

Interpreter completed strict output -> raw/stream diagnostics -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn EvaluateAuthority. Any technical/cancel/refusal/timeout/incomplete/schema failure stops before Take; no partial proposal reaches authority.

Each terminal Approved Add gets one derived RecordId/materialization; rejected mutations get none; Supersede/Deactivate use existing targets. Provider output never chooses IDs.

For accepted turn 1..12:

```text
Cycle.ComposeContext
 -> Performer spend gate/attempt -> Patch 0017/Turn.GateAttempt
 -> deterministic Integrity hard check
 -> bounded Integrity spend gate/attempt -> Patch 0008/Turn.EvaluateIntegrity
 -> Interpreter spend gate/attempt -> strict parse
 -> Turn.EvaluateAuthority -> reject unresolved mandatory-review items
 -> Turn.BindAcceptedTake(derived TakeId)
 -> materializations -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT -> Patch 0016 EstablishOpportunity
 -> next synchronized state
```

No provider call occurs between accepted commit and Opportunity establishment.

## 11. Immutable evidence

Evidence is local experimental provenance, **not Production persistence**:

```text
manifest.json
attempts/<id>/request.json
attempts/<id>/terminal.json
attempts/<streaming-id>/stream.ndjson
events.ndjson
run.final.json
transcript.json
blind/transcript.json
blind/mapping.json
```

Manifest precedes inference and is immutable; attempts write-once; events append-only; finalization SHA-256 hashes all artifacts. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history. Hidden reasoning is neither requested nor stored.

Record Blueprint/fixture/hash; executable Git commit + host OS/arch/.NET; variant; prompt/schema hashes; Access/Context; every role's provider/model/reasoning/disclosure hash; Integrity packet membership/concerns; Performer/control; Director/Integrity/Interpreter/Authority; mutation decisions; Take/commit/Opportunity; latency; usage/cost estimate; failures; blind mapping.

Initial fixture + accepted Take/commit/Opportunity evidence must replay the supported H1 transition chain; `run.final.json` is only a checkpoint/projection.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels; mapping remains separate.

## 12. Contribution / reasoning sweep

Reference contributes experiential evidence only if manifest preceded execution; observable model identity stayed frozen; all 12 Turns reached accepted commit + next Opportunity; no technical/cancel/budget/Integrity terminal occurred; evidence finalized; independent post-run hard-gate review finds no Blueprint violation. A post-run failure invalidates the run as contributing evidence but never rewrites it.

Failed/noncontributing runs remain immutable; RunId is never silently reused.

PERFORMER LOW/MEDIUM/HIGH reuse exact envelope except Performer reasoning; Integrity stays high and Interpreter none. Same role-specific output ceilings and USD 5.00 run ceiling remain fixed. Additional reasoning cost/truncation/timeout is observable experimental behavior, not silently compensated.

## 13. Credentials / pre-spend validation

Adapter may read `OPENAI_API_KEY` only as process-injected secret. Missing secret fails before provider execution. Never persist/echo/hash/pass it below Harness provider edge. This is E0, not product secret storage.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; role profiles and isolated Performer reasoning mappings; exact Context/request and Integrity-packet derivation; assessor cannot waive deterministic Reject; concern provenance is required before progression; one attempt/zero retry; technical outcomes never fiction; cost refusal precedes inference; mandatory-review reject-only policy; deterministic IDs/materializations; 12-turn synchronization; partial streams diagnostic only; no-overwrite/replay-capable/secret-free evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 14. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B mixed model; E0-E playwright implementation; E0-F failure implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; Performers NONE + Interpreter NONE + fixed Integrity HIGH; isolated Performer LOW/MEDIUM/HIGH characterization; three bounded role prompts; separate bounded Integrity packet; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s per role; Performer/Interpreter 4096 and Integrity 2048 output ceilings; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy and IDs; immutable replay-capable evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.
