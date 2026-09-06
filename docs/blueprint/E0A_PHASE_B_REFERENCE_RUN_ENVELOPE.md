# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.8 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question and inherited law

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through real Same-Model Isolated Cast while provider execution/review/spend/cancellation/credentials/evidence remain outside Core and every semantic result is coupled to its exact configured attempt?

```text
fixture -> Cycle/Context -> Performer
 -> Patch 0017 -> bounded Integrity assessor -> Patch 0008/0018
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

All three Performers use `none`; Interpreter also uses `none`. Integrity uses the same model/provider at `high` because it applies a rubric rather than portraying a Character. It emits concern evidence only; deterministic Patch 0008 owns disposition and cannot be waived.

Director, Access, State Authority, Take, commit and Opportunity remain deterministic Core authority.

## 3. Reasoning characterization

Normative reference: `PERFORMER-NONE`. First sweep varies Performer reasoning only:

```text
PERFORMER-NONE    Performer none    / Integrity high / Interpreter none
PERFORMER-LOW     Performer low     / Integrity high / Interpreter none
PERFORMER-MEDIUM  Performer medium  / Integrity high / Interpreter none
PERFORMER-HIGH    Performer high    / Integrity high / Interpreter none
```

Everything else stays matched; `xhigh`/`max` deferred. Interpreter-only characterization, if later justified, holds Performers at `none`. Neither replaces NONE as E0-C/D/E reference.

For future E0-B record semantic intent `LeastDeliberativeSupported` plus exact provider-native control. `none`, disabled, `minimal`, or low are never claimed equivalent.

## 4. Harness boundary

Core stays unchanged. Harness gains logical components:

```text
Run        policy/driver + fakeable provider-role port
Integrity  bounded assessment packet + concern parser
Evidence   local experimental evidence
OpenAI     HTTP adapter
Host       CLI + credential/cancellation edge
```

Harness references Core only; network/filesystem/provider/secrets never enter Core. Add a Harness test project. No product Application layer/post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API through `HttpClient`; reverify API/model immediately before implementation and contributing batch. No provider conversation/`previous_response_id`; no tools; `store=false`; `service_tier=default`; `truncation=disabled`; omit temperature/top-p; strict Structured Outputs; request/store no hidden reasoning.

```text
Performer    stream=true   reasoning=none   max_output_tokens=4096
Integrity    stream=false  reasoning=high   max_output_tokens=2048
Interpreter  stream=true   reasoning=none   max_output_tokens=4096
```

The sweep changes only Performer reasoning. Freeze a provider snapshot only if an immutable identifier is actually exposed/usable; otherwise record requested alias and all returned model/version identifiers, with no snapshot-stability claim. Observable in-batch identity change fails closed.

## 6. Versioned prompts and bounded disclosure

Freeze exact UTF-8 + SHA-256 for `PerformerPromptContract`, `IntegrityAssessorPromptContract`, and `StateInterpreterPromptContract`; no in-batch tuning.

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

Denied records come from exact current Production + current Access decisions; inactive records excluded; protected records deduplicated. No unrelated provenance graph/commit history/credentials/creator metadata. This Integrity-only disclosure never becomes Character knowledge.

Integrity strict output is only an ordered unique Patch 0008 concern-name array; no rationale/confidence/free text.

## 7. Configured-path attempt receipts

Provenance is a precondition to semantic consumption, not paperwork afterward.

Every invocation starts as closed `PreparedRoleAttempt` containing:

```text
RunId / attempt / role / turn / CharacterId if applicable
provider / requested model / reasoning
ContextPacketId / structured+rendered hashes
CandidateContentHash for Integrity/Interpreter when available
prompt + response-schema hashes
request-body SHA-256 / max output / service tier
Integrity packet hash + included denied/protected RecordIds when applicable
```

Write exact request evidence before disclosure. Configured provider port returns only a closed `RoleAttemptReceipt` bound to that prepared attempt, carrying terminal outcome, response ID, observable model identity, usage, raw-output hash, and raw structured bytes only on success.

Run driver consumes semantic bytes only when receipt RunId/role/turn/source/profile/request identity exactly equals the current prepared attempt. Cross-run/turn/Context/profile receipts fail closed; technical/cancelled receipts carry no semantic payload.

Candidate parsing requires Performer receipt; concern binding requires Integrity receipt bound to exact Candidate content identity + assessment packet; proposal parsing requires Interpreter receipt bound to current InterpretationSource/Candidate identity. This is configured-path provenance coupling, not Core cryptographic attestation. Auth headers/secrets never enter receipts.

## 8. Run identity / fixed envelope

Manifest supplies one Core `RunId`; fail if its run directory already exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId for Core CanonicalId limits. Attempt identity is Harness metadata.

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds each
estimated model-token spend ceiling     USD 5.00/run
```

After Turn 12 establish next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`; experimental stop only, not Scene ending (ODR-13 open). Any role timeout/refusal/provider/transport/incomplete-invalid/cancellation terminates without replacement. E0-F may later vary failure/retry.

## 9. Deterministic spend gate

Manifest freezes assumptions for **estimated model-token cost**; provider billing remains external authority. Before inference: prepare exact request; obtain supported Responses input-token count; reserve worst-case uncached input + role max output; refuse before inference if cumulative estimate would exceed USD 5.00. After response replace reservation with estimated actual usage charge, distinguishing cached/reasoning classes where supported. Token-count calls are preflight evidence, not role attempts. `BudgetExceeded` is non-fictional terminal evidence.

## 10. Integrity / State Authority behavior

```text
successful configured Integrity receipt
 -> strict concern array
 -> IntegrityConcernEvidence.Bind
 -> Patch 0008 deterministic Validate / Patch 0018 EvaluateIntegrity
```

No concerns -> Accept. Any concern -> `RequestAnotherTake`; zero retries then terminate before Interpreter/Take/commit. Deterministic Reject short-circuits before semantic review. Assessor technical failure produces no concern evidence/disposition and terminates. Synthetic empty evidence remains test-only, never contributing-run authority.

Reference `StateAuthorityPolicy` contains Patch 0010 policy-eligible domains:

```text
UnresolvedProposition, CharacterBelief, CharacterSuspicion,
CharacterGoal, CharacterCircumstance, CharacterClaim, Pressure
```

Patch 0010 still owns hard rejection/mandatory review. On `ReviewRequired`, explicitly Reject every unresolved review mutation through Patch 0018 and record `E0AReferenceDeterministicReject`, never human/creator review. Mandatory-review mutations never auto-approve; Performance may still be Accepted; rejected proposals/reasons remain provenance.

## 11. Mapping / run loop

Successful Performer receipt -> diagnostics -> `PerformerCandidateContract.ParseJson(exact Context)` -> Patch 0017 BindCandidate -> Turn GateAttempt. Failure/refusal/timeout/incomplete/invalid -> TechnicalFailure; explicit cancel -> Cancelled; partial output diagnostics only.

Successful Interpreter receipt -> diagnostics -> `StateInterpretationContract.ParseJson(exact InterpretationSource)` -> Turn EvaluateAuthority. Any technical/cancel/refusal/timeout/incomplete/schema failure stops before Take.

Each terminal Approved Add gets one derived RecordId/materialization; rejected gets none; Supersede/Deactivate use existing targets. Provider never chooses IDs.

```text
Cycle.ComposeContext
 -> Performer spend gate/receipt -> Patch 0017/Turn.GateAttempt
 -> Integrity packet + spend gate/receipt -> Patch 0008/Turn.EvaluateIntegrity
 -> Interpreter spend gate/receipt -> strict parse
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

Record Blueprint/fixture/hash; executable Git commit + host OS/arch/.NET; variant; prompt/schema hashes; Access/Context; every role profile/disclosure/receipt; Integrity packet membership/concerns; Performer/control; Director/Integrity/Interpreter/Authority; mutation decisions; Take/commit/Opportunity; latency; usage/cost; failures; blind mapping.

Initial fixture + accepted Take/commit/Opportunity evidence must replay supported H1 transitions; `run.final.json` is checkpoint/projection only. Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels; mapping separate.

## 13. Contribution / reasoning sweep

Reference contributes only if manifest preceded execution; configured-path receipts complete; observable model identity stayed frozen; all 12 Turns reached accepted commit + next Opportunity; no technical/cancel/budget/Integrity terminal; evidence finalized; independent post-run hard-gate review finds no Blueprint violation. Post-run failure invalidates contribution but never rewrites it.

Failed/noncontributing runs remain immutable; RunId never reused.

PERFORMER LOW/MEDIUM/HIGH reuse exact envelope except Performer reasoning; Integrity high/Interpreter none fixed. Same role ceilings and USD 5.00 run ceiling remain; extra reasoning cost/truncation/timeout is observable, not compensated.

## 14. Credentials / pre-spend validation

Adapter reads `OPENAI_API_KEY` only as process-injected secret; missing secret fails before provider execution. Never persist/echo/hash/pass below provider edge. E0 only, not product secret storage.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; role profiles/reasoning isolation; exact Context/request/receipt coupling; stale/cross-run receipt rejection; exact Integrity-packet derivation; configured concern provenance required; assessor cannot waive deterministic Reject; one attempt/zero retry; technical outcomes never fiction; cost refusal precedes inference; mandatory-review reject-only policy; deterministic IDs/materializations; 12-turn synchronization; partial streams diagnostic only; no-overwrite/replay-capable/secret-free evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider request.

## 15. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation; retry/backoff; understudy; E0-B mixed model; E0-E playwright implementation; E0-F failure implementation; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol; Performers NONE + Interpreter NONE + Integrity HIGH; isolated Performer LOW/MEDIUM/HIGH characterization; bounded role prompts + Integrity packet; closed configured-path receipts; REST/stateless/no-tools/strict output; 12 accepted Turns; one attempt/zero retry/300s per role; Performer/Interpreter 4096 and Integrity 2048 output ceilings; USD 5.00 estimated run-token ceiling; deterministic authority/review-reject policy and IDs; immutable replay-capable evidence/blind package; Harness-only provider edge, Core unchanged.

Approval **does not authorize provider requests, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before first real provider call.
