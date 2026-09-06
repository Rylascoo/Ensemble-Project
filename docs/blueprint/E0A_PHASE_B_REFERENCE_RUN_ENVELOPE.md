# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.12 — RECURSIVE AUDIT COMPLETE; DIRECTOR APPROVAL REQUIRED; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question / authority

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through Same-Model Isolated Cast while provider execution/review/spend/cancellation/credentials/evidence stay outside Core and every semantic result is coupled to its configured attempt?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0018 Integrity
 -> Interpreter -> State Authority -> Accepted Take/commit -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; three Characters use one frontier model; provider/model/settings stay fixed within a batch; E0-C/D/E return to reference unless explicitly varied; provenance is complete; technical failure never becomes fiction; LLMs never own access/cost/cancel/retry/state/Take/commit authority.

## 2. Fixed probabilistic roles

```text
Provider              OpenAI official API
Model                 gpt-5.6-sol
Performer reasoning   none
Interpreter reasoning none
Integrity reasoning   high
```

Performers use `none`; Interpreter also `none`. Integrity uses the same model/provider at `high` to apply a rubric, not portray a Character. It emits concern evidence only; Patch 0018/Patch 0008 own disposition. Director, Access, State Authority, Take, commit and Opportunity remain deterministic Core authority.

## 3. Reasoning characterization

Reference is `PERFORMER-NONE`. First sweep varies Performer reasoning only:

```text
PERFORMER-NONE    Performer none    / Integrity high / Interpreter none
PERFORMER-LOW     Performer low     / Integrity high / Interpreter none
PERFORMER-MEDIUM  Performer medium  / Integrity high / Interpreter none
PERFORMER-HIGH    Performer high    / Integrity high / Interpreter none
```

Everything else stays matched; `xhigh`/`max` deferred. A later Interpreter-only sweep may hold Performers at `none`; neither replaces NONE as E0-C/D/E reference. Future E0-B records `LeastDeliberativeSupported` plus exact native control; different providers' low/off modes are never claimed equivalent.

## 4. Harness boundary

Core stays unchanged. Harness gains logical components:

```text
Run        policy/driver + fakeable provider-role port
Integrity  bounded assessment packet + response parser
Evidence   local experimental evidence
OpenAI     HTTP adapter
Host       CLI + credential/cancellation edge
```

Harness references Core only; network/filesystem/provider/secrets never enter Core. Add a Harness test project. No product Application layer/post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API via `HttpClient`; reverify API/model before implementation and contributing batch. No provider conversation/`previous_response_id`; no tools; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs; request/store no hidden reasoning.

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=4096
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

Sweep changes only Performer reasoning. Freeze a snapshot only if an immutable provider identifier is exposed/usable; otherwise record requested alias + returned identifiers and claim no snapshot stability. Observable in-batch identity change fails.

Provider schemas stay inside the supported Structured Outputs subset. Shape enforcement never replaces Core semantic parsers/binders.

## 6. Prompt / disclosure law

Freeze exact UTF-8 + SHA-256 for `PerformerPromptContract`, `IntegrityAssessorPromptContract`, `StateInterpreterPromptContract`; no in-batch tuning.

Fixed provider instructions are separate from dynamic data. Every fixture/Context/Candidate/record value is deterministically serialized as **untrusted data** and explicitly non-instructional; dynamic content never enters the instruction channel.

Performer gets exact Character-bounded rendered Context + Candidate schema. Interpreter gets that Context + exact Candidate Performance/control + mutation schema; no full Production, denied/private state, Authority locks/snapshot or credentials.

Integrity gets separate `E0AIntegrityAssessmentPacket`:

```text
source Context identity + CharacterId
Candidate visible text + typed control
Character-visible rendered Context
active denied Production records + AccessReason
active SystemImmutable/CreatorLocked records
five Patch 0008 concern definitions
```

Denied records derive from exact current Production + Access decisions; inactive excluded; protected deduplicated. No unrelated provenance graph/commit history/credentials/creator metadata. Integrity disclosure never becomes Character knowledge.

Integrity output is strict root object `{"concerns":[...]}` limited to the five Patch 0008 concern names. Do not depend on provider array uniqueness/order; Core owns duplicate/defined/canonical concern semantics. No rationale/confidence/free text.

## 7. Configured-path receipts

Provenance is prerequisite to semantic consumption. Each invocation starts as closed `PreparedRoleAttempt`:

```text
RunId / attempt / role / turn / CharacterId if applicable
provider / requested model / reasoning
ContextPacketId / structured+rendered hashes
CandidateContentHash for Integrity/Interpreter
prompt + schema hashes / request-body SHA-256 / max output / service tier
Integrity packet hash + denied/protected RecordIds when applicable
```

For Integrity, `IntegrityCandidateInput.Bind` may run before disclosure only to obtain Candidate identity/prove zero reject codes; it creates no evaluation. Impossible reject-coded Candidate fails without semantic review.

Write request evidence before disclosure. Configured provider port returns only closed `RoleAttemptReceipt` bound to the prepared attempt: terminal outcome, response ID, observable model identity, usage, raw-output hash, raw structured bytes only on success.

Driver consumes bytes only when receipt RunId/role/turn/source/profile/request identity matches current attempt. Cross-run/turn/Context/profile receipts fail; technical/cancelled receipts carry no semantic payload. Performer/Integrity/Interpreter parsing respectively requires receipts bound to current Context, Candidate+assessment packet, and InterpretationSource+Candidate. This is Harness provenance coupling, not Core cryptographic attestation. Secrets never enter receipts.

## 8. Identity / fixed envelope

Manifest supplies one Core `RunId`; fail if run directory exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId for Core's 128-character CanonicalId limit.

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 establish next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; experimental stop only, ODR-13 open. Any role technical/refusal/timeout/invalid/cancel outcome terminates without replacement. E0-F may later vary failure/retry.

Cancellation is checked before/while provider work and before another Turn. Once `CommitAccepted` succeeds, attempt immediate deterministic `EstablishOpportunity` before honoring cancellation; if establishment itself fails, Patch 0016 valid-postcommit authority remains and run is noncontributing.

## 9. Spend gate

Manifest freezes assumptions for **estimated model-token cost**; provider billing remains external authority. Before inference: prepare exact request; obtain supported Responses input-token count; reserve worst-case uncached input + 4096 output; refuse if cumulative estimate exceeds USD 5.00. After response replace reservation with estimated actual usage charge, distinguishing cached/reasoning classes where supported. Token-count calls are preflight evidence, not role attempts. `BudgetExceeded` is non-fictional terminal evidence.

## 10. Integrity / State Authority

After successful Integrity receipt, parse `concerns`, verify receipt/packet identity, then call only:

```text
DeterministicE0TurnOrchestrator.EvaluateIntegrity(candidateReady, concerns)
```

Turn re-binds Input/evidence, runs Patch 0008 validation and constructs InterpretationSource on Accept. Harness has no parallel evaluation path. No concerns -> Accept. Any concern -> `RequestAnotherTake`; zero retries terminate before Interpreter/Take/commit. Assessor technical/schema failure produces no Integrity disposition. Synthetic empty evidence stays test-only.

Reference State Authority policy contains Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 owns hard rejection/mandatory review. On `ReviewRequired`, explicitly Reject each unresolved review mutation through Patch 0018; record `E0AReferenceDeterministicReject`, never human/creator review. Performance may still be Accepted; rejected proposals/reasons remain provenance.

## 11. Mapping / loop

Successful Performer receipt -> diagnostics -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 BindCandidate -> Turn GateAttempt. Technical/refusal/timeout/invalid -> TechnicalFailure; explicit cancel -> Cancelled; partial output diagnostic only.

Successful Interpreter receipt -> diagnostics -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn EvaluateAuthority. Technical/cancel/refusal/timeout/schema failure stops before Take. Provider schemas use required nullable fields where Core permits null; invalid semantic combinations remain parser failures.

Each terminal Approved Add gets one derived RecordId/materialization; rejected gets none; Supersede/Deactivate use existing targets. Provider never chooses IDs.

```text
Cycle.ComposeContext
 -> Performer spend/receipt -> Patch 0017/Turn.GateAttempt
 -> Integrity packet + spend/receipt -> Turn.EvaluateIntegrity
 -> Interpreter spend/receipt -> strict parse
 -> Turn.EvaluateAuthority -> deterministic review rejects if required
 -> Turn.BindAcceptedTake -> materializations -> Turn.CommitAccepted
 -> VALID POSTCOMMIT -> Patch 0016 EstablishOpportunity -> next state
```

## 12. Immutable evidence / blindness

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

Manifest precedes inference/immutable; attempts write-once; events append-only; finalization hashes artifacts. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history; hidden reasoning neither requested nor stored.

Record Blueprint/fixture/hash; executable Git commit + host OS/arch/.NET; variant; prompt/schema hashes; Access/Context; every role profile/disclosure/receipt; Integrity packet/concerns; Performer/control; Director/Integrity/Interpreter/Authority; mutations; Take/commit/Opportunity; latency; usage/cost; failures; blind mapping.

Initial fixture + accepted Take/commit/Opportunity evidence must replay supported H1 transitions; `run.final.json` is checkpoint only. Blind transcript contains evaluation-appropriate Character-visible accepted Performance material with neutral speakers; never denied/protected Integrity material, assessor diagnostics, provider/reasoning/RunId/hashes/implementation metadata. Mapping stays separate/private.

## 13. Contribution / sweep

Reference contributes only if manifest preceded execution; receipts complete; observable model identity stayed frozen; all 12 Turns reached accepted commit + next Opportunity; no technical/cancel/budget/Integrity terminal; evidence finalized; independent post-run hard-gate review finds no Blueprint violation. Failure invalidates contribution but never rewrites it.

Failed/noncontributing runs remain immutable; RunId never reused. PERFORMER LOW/MEDIUM/HIGH reuse exact envelope except Performer reasoning; Integrity high/Interpreter none fixed. Same ceilings/$5 remain; extra reasoning cost/truncation/timeout is observable, not compensated.

## 14. Credentials / pre-spend validation

Adapter reads `OPENAI_API_KEY` only as process-injected secret; missing secret fails before execution. Never persist/echo/hash/pass below provider edge. E0 only, not product secret storage.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; role profiles/reasoning isolation; instruction/data separation; exact receipt coupling + stale/cross-run rejection; Integrity packet/provenance; no parallel Integrity path; one attempt/zero retry; technical outcomes never fiction; cost refusal precedes inference; mandatory-review reject-only behavior; deterministic IDs/materializations; commit→Opportunity cancellation law; 12-turn synchronization; partial streams diagnostic; secret-free replay evidence; blind output excludes denied material.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 15. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B; E0-E/F implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; Performers NONE + Interpreter NONE + Integrity HIGH; Performer LOW/MEDIUM/HIGH sweep; bounded instruction-separated prompts + Integrity packet; configured receipts; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s; 4096 max output each role; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy/IDs; commit→Opportunity cancellation boundary; immutable replay evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.

Final recursive pass: 0 material corrections, 0 authority/privacy/dependency/scope contradictions, 0 worthwhile in-scope simplifications.
