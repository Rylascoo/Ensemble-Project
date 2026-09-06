# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.10 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question / inherited law

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through real Same-Model Isolated Cast while provider execution/review/spend/cancellation/credentials/evidence remain outside Core and every semantic result is coupled to its exact configured attempt?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0018 Integrity
 -> Interpreter -> State Authority -> Accepted Take/commit
 -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; three Characters use the same frontier model; provider/model/settings stay fixed within a batch; E0-C/D/E return to that reference unless explicitly varied; provenance is complete; technical failure never becomes fiction; LLMs never own access/cost/cancel/retry/state/Take/commit authority.

## 2. Fixed probabilistic roles

```text
Provider              OpenAI official API
Model                 gpt-5.6-sol
Performer reasoning   none
Interpreter reasoning none
Integrity reasoning   high
```

Three Performers use `none`; Interpreter also `none`. Integrity uses the same model/provider at `high` because it applies a rubric rather than portraying a Character. It emits concern evidence only; Patch 0018/Patch 0008 remain deterministic disposition authority.

Director, Access, State Authority, Take, commit and Opportunity remain deterministic Core authority.

## 3. Reasoning characterization

Normative reference: `PERFORMER-NONE`. First sweep varies Performer reasoning only:

```text
PERFORMER-NONE    Performer none    / Integrity high / Interpreter none
PERFORMER-LOW     Performer low     / Integrity high / Interpreter none
PERFORMER-MEDIUM  Performer medium  / Integrity high / Interpreter none
PERFORMER-HIGH    Performer high    / Integrity high / Interpreter none
```

Everything else stays matched; `xhigh`/`max` deferred. A later Interpreter-only sweep may hold Performers at `none`; neither replaces NONE as E0-C/D/E reference.

Future E0-B records `LeastDeliberativeSupported` plus exact provider-native control; `none`, disabled, `minimal`, or low are never claimed equivalent.

## 4. Harness boundary

Core stays unchanged. Harness gains logical components:

```text
Run        policy/driver + fakeable provider-role port
Integrity  bounded assessment packet + response parser
Evidence   local experimental evidence
OpenAI     HTTP adapter
Host       CLI + credential/cancellation edge
```

Harness references Core only; network/filesystem/provider/secrets never enter Core. Add a Harness test project. No product Application layer or post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API via `HttpClient`; reverify API/model before implementation and contributing batch. No provider conversation/`previous_response_id`; no tools; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs; request/store no hidden reasoning.

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=4096
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

Sweep changes only Performer reasoning. Freeze a snapshot only if an immutable provider identifier is actually exposed/usable; otherwise record requested alias + all returned model/version identifiers and make no snapshot-stability claim. Observable in-batch identity change fails closed.

Provider schemas stay inside the supported Structured Outputs subset. Schema enforcement proves shape only; existing Core parsers/binders retain semantic authority.

## 6. Prompts / bounded disclosure

Freeze exact UTF-8 + SHA-256 for `PerformerPromptContract`, `IntegrityAssessorPromptContract`, `StateInterpreterPromptContract`; no in-batch tuning.

Performer receives exact Character-bounded rendered Context + Candidate schema only.

Interpreter receives that bounded Context + exact Candidate Performance/control + mutation schema; no full Production, denied/private state, Authority locks/snapshot or credentials.

Integrity receives separate `E0AIntegrityAssessmentPacket`:

```text
source Context identity + CharacterId
Candidate visible text + typed control
Character-visible rendered Context
active denied Production records + AccessReason
active SystemImmutable/CreatorLocked records
five Patch 0008 concern definitions
```

Denied records derive from exact current Production + current Access decisions; inactive excluded; protected deduplicated. No unrelated provenance graph/commit history/credentials/creator metadata. Integrity disclosure never becomes Character knowledge.

Integrity provider output is strict root object `{"concerns":[...]}`; items are the five Patch 0008 concern names. Do not depend on provider array uniqueness/order constraints. Core binding remains authority for duplicate/defined/canonical concern semantics. No rationale/confidence/free text.

## 7. Configured-path receipts

Provenance is prerequisite to semantic consumption.

Each invocation starts as closed `PreparedRoleAttempt` containing:

```text
RunId / attempt / role / turn / CharacterId if applicable
provider / requested model / reasoning
ContextPacketId / structured+rendered hashes
CandidateContentHash for Integrity/Interpreter
prompt + response-schema hashes
request-body SHA-256 / max output / service tier
Integrity packet hash + denied/protected RecordIds when applicable
```

For Integrity, canonical `IntegrityCandidateInput.Bind` may be used before disclosure only to obtain Candidate content identity and prove the current H1 candidate has zero deterministic reject codes; it creates no evaluation. Any impossible reject-coded Candidate fails closed without semantic review.

Write exact request evidence before disclosure. Configured provider port returns only closed `RoleAttemptReceipt` bound to the prepared attempt: terminal outcome, response ID, observable model identity, usage, raw-output hash, and raw structured bytes only on success.

Driver consumes bytes only if receipt RunId/role/turn/source/profile/request identity equals current prepared attempt. Cross-run/turn/Context/profile receipts fail; technical/cancelled receipts carry no semantic payload.

Candidate parsing requires Performer receipt; Integrity concerns require receipt bound to exact Candidate identity + assessment packet; proposal parsing requires Interpreter receipt bound to current InterpretationSource/Candidate. This is Harness configured-path coupling, not Core cryptographic attestation. Secrets never enter receipts.

## 8. Identity / fixed envelope

Manifest supplies one Core `RunId`; fail if run directory already exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId for Core 128-character CanonicalId limit.

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 establish next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; experimental stop only, ODR-13 open. Any role timeout/refusal/provider/transport/incomplete-invalid/cancellation terminates without replacement. E0-F may later vary failure/retry.

## 9. Spend gate

Manifest freezes assumptions for **estimated model-token cost**; provider billing remains external authority. Before inference: prepare exact request; obtain supported Responses input-token count; reserve worst-case uncached input + 4096 output; refuse before inference if cumulative estimate would exceed USD 5.00. After response replace reservation with estimated actual usage charge, distinguishing cached/reasoning classes where supported. Token-count calls are preflight evidence, not role attempts. `BudgetExceeded` is non-fictional terminal evidence.

## 10. Integrity / State Authority

After successful configured Integrity receipt, parse `concerns`, verify receipt/packet identity, then call only canonical:

```text
DeterministicE0TurnOrchestrator.EvaluateIntegrity(candidateReady, concerns)
```

Turn re-binds `IntegrityCandidateInput`, binds `IntegrityConcernEvidence`, runs Patch 0008 deterministic validation, and constructs the InterpretationSource on Accept. Harness does not create a parallel evaluation path.

No concerns -> Accept. Any concern -> `RequestAnotherTake`; zero retries terminate before Interpreter/Take/commit. Assessor technical/schema failure produces no concern set/Integrity disposition and terminates. Synthetic empty concern evidence remains test-only, never contributing-run provenance.

Reference State Authority policy includes Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 owns hard rejection/mandatory review. On `ReviewRequired`, explicitly Reject each unresolved review mutation through Patch 0018; record `E0AReferenceDeterministicReject`, never human/creator review. Mandatory-review mutations never auto-approve; Performance may still be Accepted; rejected proposals/reasons remain provenance.

## 11. Mapping / loop

Successful Performer receipt -> diagnostics -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 BindCandidate -> Turn GateAttempt. Failure/refusal/timeout/incomplete/invalid -> TechnicalFailure; explicit cancel -> Cancelled; partial output diagnostic only.

Successful Interpreter receipt -> diagnostics -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn EvaluateAuthority. Technical/cancel/refusal/timeout/incomplete/schema failure stops before Take. Provider schemas use required nullable fields where Core transport permits null; semantically invalid combinations remain canonical-parser failures.

Each terminal Approved Add gets one derived RecordId/materialization; rejected gets none; Supersede/Deactivate use existing targets. Provider never chooses IDs.

```text
Cycle.ComposeContext
 -> Performer spend/receipt -> Patch 0017/Turn.GateAttempt
 -> Integrity packet + spend/receipt -> Turn.EvaluateIntegrity
 -> Interpreter spend/receipt -> strict parse
 -> Turn.EvaluateAuthority -> deterministic review rejects if required
 -> Turn.BindAcceptedTake(derived TakeId)
 -> materializations -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT -> Patch 0016 EstablishOpportunity
 -> next synchronized state
```

No provider call occurs between accepted commit and Opportunity establishment.

## 12. Immutable evidence

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

Manifest precedes inference/immutable; attempts write-once; events append-only; finalization hashes every artifact. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history; hidden reasoning neither requested nor stored.

Record Blueprint/fixture/hash; executable Git commit + host OS/arch/.NET; variant; prompt/schema hashes; Access/Context; every role profile/disclosure/receipt; Integrity packet/concerns; Performer/control; Director/Integrity/Interpreter/Authority; mutation decisions; Take/commit/Opportunity; latency; usage/cost; failures; blind mapping.

Initial fixture + accepted Take/commit/Opportunity evidence must replay supported H1 transitions; `run.final.json` is checkpoint/projection only. Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speakers; mapping separate.

## 13. Contribution / reasoning sweep

Reference contributes only if manifest preceded execution; configured receipts complete; observable model identity stayed frozen; all 12 Turns reached accepted commit + next Opportunity; no technical/cancel/budget/Integrity terminal; evidence finalized; independent post-run hard-gate review finds no Blueprint violation. Post-run failure invalidates contribution but never rewrites it.

Failed/noncontributing runs remain immutable; RunId never reused.

PERFORMER LOW/MEDIUM/HIGH reuse exact envelope except Performer reasoning; Integrity high/Interpreter none fixed. Same role ceilings and USD 5.00 run ceiling remain; extra reasoning cost/truncation/timeout is observable, not compensated.

## 14. Credentials / pre-spend validation

Adapter reads `OPENAI_API_KEY` only as process-injected secret; missing secret fails before execution. Never persist/echo/hash/pass below provider edge. E0 only, not product secret storage.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; role profiles/reasoning isolation; exact Context/request/receipt coupling; stale/cross-run receipt rejection; Integrity packet derivation + configured concern provenance; no parallel Integrity evaluation; one attempt/zero retry; technical outcomes never fiction; cost refusal precedes inference; mandatory-review reject-only behavior; deterministic IDs/materializations; 12-turn synchronization; partial streams diagnostic only; no-overwrite/replay-capable/secret-free evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 15. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B; E0-E implementation; E0-F implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; Performers NONE + Interpreter NONE + Integrity HIGH; isolated Performer LOW/MEDIUM/HIGH sweep; bounded prompts + Integrity packet; closed configured receipts; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s per role; 4096 max output per role; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy and IDs; immutable replay-capable evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.
