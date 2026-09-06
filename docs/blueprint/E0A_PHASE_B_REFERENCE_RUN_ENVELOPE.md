# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.4 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one immutable E0-A envelope drive the closed H1 Cycle/Turn spine through a real Same-Model Isolated Cast while provider execution, spend, cancellation, credentials and run evidence remain outside Core and every disclosure/result remains attributable?

```text
fixture -> H1 Cycle/Context -> provider Performer
 -> Patch 0017/0018 -> Integrity -> provider Interpreter
 -> deterministic State Authority -> Accepted Take/commit
 -> Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; all three Characters use the same frontier model; provider/model and supported generation/reasoning settings stay fixed within a comparison batch; E0-C/D/E return to that reference unless separately labeled; every run preserves provenance; technical failure never becomes fiction; deterministic access/cost/cancel/retry rules never belong to an LLM.

## 2. Primary reference

```text
Provider                 OpenAI official API
Model                    gpt-5.6-sol
Deliberation intent      LeastDeliberativeSupported
Provider-native setting  reasoning.effort = none
```

Use the same profile for Voss, Marlowe, Wren and State Interpreter. Director, Access, Integrity hard checks, State Authority, Take, commit and Opportunity remain deterministic Core authority.

No model-assisted Integrity assessor is used in the primary reference. Later hard-gate/blind evaluation may invalidate a run but cannot rewrite it.

## 3. Reasoning characterization

`none` is the normative reference. Separately labeled characterization may later change only reasoning:

```text
NONE    gpt-5.6-sol / none   <- reference
LOW     gpt-5.6-sol / low
MEDIUM  gpt-5.6-sol / medium
HIGH    gpt-5.6-sol / high
```

Fixture, prompts, Context, model, deterministic authority, service tier, timeout, output ceiling, spend ceiling and run protocol stay matched. `xhigh`/`max` are deferred. E0-C/D/E continue to use `none` unless explicitly varied.

Later E0-B uses:

```text
DeliberationIntent = LeastDeliberativeSupported
ProviderNativeSetting = exact native control sent
```

Provider `none`, disabled, `minimal`, or low modes are recorded exactly and never claimed computationally equivalent.

## 4. Harness boundary

Do not create a product Application layer or another disposable runtime assembly for E0.

If approved, Core stays unchanged. Harness gains logical components:

```text
Run       provider-neutral run/policy + fakeable adapter contract
Evidence  local experimental evidence
OpenAI    HTTP/streaming adapter
Host      minimal CLI + credential/cancellation edge
```

Harness references Core only. Provider/network/filesystem/secrets never enter Core. Add a Harness test project for fake-adapter/evidence/run-driver tests. No post-E0 compatibility promise.

## 5. OpenAI transport

Use official Responses REST API via `HttpClient`, avoiding provider SDK architecture. Reverify API/model surface immediately before implementation and contributing batch.

```text
model              gpt-5.6-sol
reasoning.effort   none
stream             true
store              false
service_tier       default
tools              []
truncation          disabled
max_output_tokens  4096
```

No provider conversation/`previous_response_id`; no tools; omit `temperature`/`top_p`; use strict Structured Outputs for Candidate and Interpreter proposal. Request no reasoning summary/encrypted reasoning/chain-of-thought.

Freeze an immutable model snapshot if OpenAI exposes one suitable for the batch. Otherwise record requested alias and every returned model/version identifier available; never claim snapshot immutability not exposed. Observable identity change inside the batch fails closed.

## 6. Bounded disclosure

Create exactly two versioned prompt contracts:

```text
PerformerPromptContract
StateInterpreterPromptContract
```

Freeze exact UTF-8 bytes + SHA-256 before a contributing run; no in-batch prompt tuning.

Performer receives only exact current Character-bounded rendered Context + Candidate output contract. Interpreter receives that bounded source Context + exact Candidate Performance/control + mutation-proposal contract. No full Production, denied/private material, State Authority snapshot/locks, other Character private state, creator-only provenance or credentials.

Before network execution record:

```text
RunId / attempt / role / turn / CharacterId if Performer
provider / requested model / reasoning
ContextPacketId / structured + rendered hashes
prompt + response-schema hashes
request-body SHA-256 / max output / service tier
```

Write exact request body locally before disclosure. Never record auth headers/secrets. Couple response ID, observable model identity, service tier, status, usage and terminal metadata to that attempt.

## 7. Identity / no overwrite

Manifest supplies one initialized Core `RunId`. A run cannot start if that RunId/directory already exists.

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
Attempt  = {RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

Constrain RunId so derived Core IDs remain within CanonicalId limits. Attempt identity is experimental metadata, not a new Core strong ID.

## 8. Fixed reference envelope

```text
accepted-Turn cap                     12
attempts per probabilistic invocation  1
automatic retries                       0
attempt timeout                         300 seconds
max_output_tokens                       4096
estimated model-token spend ceiling     USD 5.00
```

Twelve accepted Turns bound cost while allowing multiple three-Character causal cycles. After Turn 12 establish the next Patch 0016 Opportunity, then stop `AcceptedTurnCapReached`. This is an experimental stop, not a Scene-ending decision; ODR-13 remains open.

Timeout/refusal/provider error/incomplete-invalid output/transport failure/cancellation terminates without replacement. E0-F may later vary retry/failure behavior.

## 9. Deterministic spend gate

Manifest freezes rate assumptions for **estimated model-token cost**; provider billing remains external financial authority.

Before inference:

1. prepare exact request;
2. obtain supported Responses input-token count;
3. compute worst-case next inference charge using uncached input rate + 4096 output tokens;
4. add to incurred estimated inference cost;
5. if above USD 5.00, do not send inference.

After response replace reservation with estimated actual charge from returned usage, distinguishing cached/reasoning classes where supported. Preflight/token-count activity is recorded separately and is not a fictional/provider-role attempt. USD 5.00 is not claimed to equal final billing exactly.

`BudgetExceeded` is terminal non-fictional evidence.

## 10. Integrity / State Authority policy

Reference Integrity calls Patch 0018 `EvaluateIntegrity` with an initialized empty semantic-concern set. Deterministic source/structural checks remain authoritative; no hidden model judges or rewrites Candidate. If Integrity requests another Take, zero-retry policy terminates before Take/commit.

Reference `StateAuthorityPolicy` contains all Patch 0010 policy-eligible domains:

```text
UnresolvedProposition
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterCircumstance
CharacterClaim
Pressure
```

Patch 0010 still owns hard rejection and transition-specific mandatory review. If `ReviewRequired`, supply explicit **Reject** choices for every unresolved review-required mutation and resolve through Patch 0018. Record as `E0AReferenceDeterministicReject`, never creator/human review. Mandatory-review mutations never auto-approve; Patch 0011 may still accept Performance; rejected proposals/reasons remain provenance.

## 11. Provider result mapping

Performer:

```text
completed structured output
 -> raw/stream diagnostics
 -> PerformerCandidateContract.ParseJson(exact Context)
 -> Patch 0017 BindCandidate -> Turn.GateAttempt
```

Refusal/timeout/transport/provider/incomplete/invalid output -> `TechnicalFailure`; explicit cancel -> `Cancelled`. Partial output stays diagnostic.

Interpreter:

```text
ReadyForInterpretation
 -> bounded provider request
 -> raw/stream diagnostics
 -> StateInterpretationContract.ParseJson(exact InterpretationSource)
 -> Turn.EvaluateAuthority
```

Interpreter technical/cancel/refusal/timeout/incomplete/schema failure terminates before Take; no partial proposal reaches State Authority.

## 12. Materialization / loop

Each terminal Approved `Add` gets one derived RecordId and `E0RecordMaterialization`; rejected mutations get none; Supersede/Deactivate use existing targets. Provider output never chooses IDs.

For accepted turn 1..12:

```text
Cycle -> ComposeContext
 -> record Performer request -> spend gate -> one streamed attempt
 -> Patch 0017 -> Turn.GateAttempt -> Turn.EvaluateIntegrity
 -> record Interpreter request -> spend gate -> one streamed attempt
 -> strict Interpreter parse -> Turn.EvaluateAuthority
 -> reject unresolved mandatory-review mutations if required
 -> Turn.BindAcceptedTake(derived TakeId)
 -> materializations -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT STATE -> Patch 0016 EstablishOpportunity
 -> adopt next synchronized opportunity-bearing state
```

Technical/cancel/budget/Integrity/Interpreter terminal outcomes stop immediately. No provider call occurs between accepted commit and Opportunity establishment.

## 13. Immutable evidence

Run evidence is local experimental provenance, **not Production persistence**.

```text
manifest.json
attempts/<id>/request.json
attempts/<id>/stream.ndjson
attempts/<id>/terminal.json
events.ndjson
run.final.json
transcript.json
blind/transcript.json
blind/mapping.json
```

Manifest precedes inference and is immutable; attempt artifacts write-once; events append-only; finalization SHA-256 hashes all artifacts. Credentials never enter evidence. Partial/rejected/technical output never enters accepted history. Hidden reasoning is neither requested nor stored.

Record Blueprint/fixture/hash; repository executable commit + host OS/architecture/.NET runtime; variant/reference identity; prompt/schema hashes; access/context; provider/model/native settings; Performer output/control; Director/Integrity/Interpreter/Authority; accepted/rejected mutations/reasons; Take/commit/Opportunity; latency; token usage/cost estimate; technical outcomes; blind mapping.

Initial fixture plus accepted Take/commit/Opportunity evidence must replay the supported H1 transition chain; `run.final.json` is projection/checkpoint, not competing causal authority.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and uses stable neutral speaker labels. Mapping remains separate.

## 14. Contribution / reasoning sweep

Reference run contributes experiential evidence only if manifest preceded execution; observable provider identity stayed frozen; all 12 Turns reached Accepted Take + commit + next Opportunity; no technical/cancel/budget/Integrity terminal occurred; evidence finalized; hard-integrity review finds no Blueprint gate failure.

Failed/noncontributing runs remain immutable and never silently reuse RunId.

LOW/MEDIUM/HIGH reuse the exact envelope except native reasoning. Keep 4096 output and USD 5.00 estimated-token ceiling matched. Extra reasoning cost/truncation/timeout is observable behavior, not silently compensated. These variants never replace NONE as reference.

## 15. Credentials / pre-spend validation

Adapter may consume `OPENAI_API_KEY` only as process-injected secret. Missing secret fails before provider execution. Never persist/echo/hash/pass it below Harness provider edge. This is E0, not product secret-store architecture.

Before real spend, fake-adapter tests prove: Core unchanged/dependencies clean; same model/profile for three Performers + Interpreter; reasoning mappings; exact Context/request/stale-result behavior; one attempt/zero retry; technical outcomes never fiction; cost refusal before inference; mandatory-review reject-only behavior; deterministic IDs/materializations; 12-turn synchronization; partial-stream diagnostics only; no-overwrite/replay-capable/secret-free evidence; blind identity removal.

Native Windows ARM64 compile/tests/Harness smoke must pass before first real provider call.

## 16. Exclusions / approval

No Core redesign; product Application/persistence; provider conversation state; retry/backoff; understudy; E0-B mixed model; E0-E playwright implementation; E0-F failure implementation; semantic Integrity model; Context optimization; Scene ending; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol/NONE reference; LOW/MEDIUM/HIGH characterization; same profile for Performers + Interpreter; REST/stateless/no-tools/strict-output transport; 12 accepted Turns; one attempt/zero retry/300s timeout; 4096 output; USD 5.00 estimated model-token ceiling; deterministic authority/review-reject policy; deterministic IDs; immutable replay-capable evidence/blind package; Harness-only provider edge with Core unchanged.

Approval **does not authorize provider request, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before the first real call.
