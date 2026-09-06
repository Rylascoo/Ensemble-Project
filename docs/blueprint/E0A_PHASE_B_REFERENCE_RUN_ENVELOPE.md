# E0-A Phase B — Reference Run Envelope

Status: **PROPOSAL 0.2 — EXPLORATORY; IMPLEMENTATION NOT AUTHORIZED**
Date: 2026-09-05
Parent `main`: `2b628c2d97eb39f4d96ce3e81eafefe93ba28b09`
Branch: `e0a-reference-run-envelope-blueprint`

## 1. Question

Can one immutable experimental envelope drive the closed H1 Cycle/Turn spine through a real Same-Model Isolated Cast while keeping provider execution, spend, cancellation, credentials and run evidence outside Core and making every disclosure/result attributable?

```text
frozen fixture -> H1 Cycle/Context -> provider Performer
 -> Patch 0017/0018 -> Integrity -> provider Interpreter
 -> deterministic State Authority -> Accepted Take/commit
 -> explicit Opportunity -> repeat
```

Blueprint 0.1 remains authority: E0-A precedes E0-B; all three Characters use the same frontier model; provider/model and supported generation/reasoning settings remain fixed within a comparison batch; E0-C/D/E return to that reference unless separately labeled; every run preserves complete provenance; technical failure never becomes fiction; deterministic access/cost/cancel/retry rules never belong to an LLM.

## 2. Primary reference

Director-directed E0-A reference:

```text
Provider                 OpenAI official API
Model                    gpt-5.6-sol
Deliberation intent      LeastDeliberativeSupported
Provider-native setting  reasoning.effort = none
```

Use the same profile for Voss, Marlowe, Wren and the State Interpreter. Director, Access, Integrity hard checks, State Authority, Take, commit and Opportunity remain deterministic Core authority.

The primary reference has no model-assisted Integrity assessor. Later hard-gate/blind evaluation may invalidate a completed run but cannot rewrite it.

## 3. Reasoning characterization

`none` remains the normative reference. After it works, separately labeled characterization variants may change only reasoning:

```text
NONE    gpt-5.6-sol / none   <- reference
LOW     gpt-5.6-sol / low
MEDIUM  gpt-5.6-sol / medium
HIGH    gpt-5.6-sol / high
```

Fixture, prompts, Context, model, deterministic authority, service tier, timeout, output ceiling, spend ceiling and run protocol stay matched. `xhigh`/`max` are deferred ceiling tests. E0-C/D/E continue to use `none` unless another explicit variant changes reasoning.

Cross-provider law for later E0-B:

```text
DeliberationIntent = LeastDeliberativeSupported
ProviderNativeSetting = exact native control sent
```

Providers' `none`, disabled, `minimal`, or low modes are recorded exactly and never claimed computationally equivalent.

## 4. E0-only topology

If approved:

```text
Ensemble.E0.Core
  unchanged deterministic authority

Ensemble.E0.Experiment
  E0-only provider-neutral run/policy/evidence contracts
  -> Core only; no HTTP, provider SDK, filesystem or secrets

Ensemble.E0.Harness
  native ARM64 host
  -> Experiment + Core
  CLI, local evidence, credentials, OpenAI HTTP/streaming adapter
```

This does not freeze the post-E0 Application architecture. Experiment is disposable E0 infrastructure.

## 5. OpenAI transport

Use the official Responses REST API through `HttpClient`, not a provider SDK dependency. Reverify the exact API/model surface immediately before implementation and again before the contributing batch.

Reference request settings:

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

No provider conversation/`previous_response_id`; no web/file/computer/code/MCP/function tools; omit `temperature` and `top_p` rather than inventing a sampling policy. Use strict Structured Outputs for Performer Candidate and Interpreter proposal. Request no reasoning summary/encrypted reasoning/chain-of-thought.

If OpenAI exposes an exact immutable snapshot identifier suitable for the batch, freeze it. Otherwise record the requested model alias and every returned model/version identifier available; do **not** claim snapshot immutability that the provider does not expose. Any observable returned identity change inside the batch fails closed.

## 6. Bounded disclosures and prompts

Implementation creates exactly two versioned prompt contracts:

```text
PerformerPromptContract
StateInterpreterPromptContract
```

Freeze their exact UTF-8 bytes and SHA-256 hashes before a contributing run; no prompt tuning inside the batch.

Performer disclosure: exact current Character-bounded rendered Context + Candidate output contract only.

Interpreter disclosure: that same bounded source Context + exact Candidate Performance/control + mutation-proposal contract. It receives no full Production repository, denied/private material, State Authority snapshot/locks, other Characters' private state, creator-only provenance or credentials.

Before network execution create a prepared-attempt record with:

```text
RunId / attempt identity / role / turn ordinal
CharacterId when Performer
provider / requested model / reasoning setting
ContextPacketId / StructuredContextHash / RenderedContextHash
prompt and response-schema version/hash
exact request-body SHA-256
max output / service tier
```

Write the exact request body locally before disclosure. Never record auth headers/secrets. Couple returned response ID, observable model identity, service tier, status, usage and terminal metadata to that prepared attempt.

## 7. Identity

The manifest supplies one initialized Core `RunId`. Derive fictional/causal IDs without clock/randomness:

```text
TakeId   = {RunId}:TAKE:{turn:D3}
CommitId = {RunId}:COMMIT:{turn:D3}
RecordId = {RunId}:RECORD:{turn:D3}:{mutationIndex:D3}
```

The manifest constrains RunId length so all derived values remain within Core CanonicalId limits.

Provider attempt identity is experimental metadata, not a new Core strong ID:

```text
{RunId}:ATTEMPT:{role}:{turn:D3}:{attempt:D2}
```

## 8. Fixed reference run envelope

```text
accepted-Turn cap        12
attempts/role invocation 1
automatic retries        0
attempt timeout          300 seconds
max_output_tokens        4096
estimated model-token spend ceiling  USD 5.00
```

Twelve accepted Turns bound cost while allowing multiple three-Character causal cycles. After Turn 12, still establish the next Patch 0016 Opportunity so final state is synchronized, then stop as `AcceptedTurnCapReached`. This is an experimental stop, not a dramatic Scene-ending decision (ODR-13 remains open).

Timeout/refusal/provider error/incomplete or invalid output/transport failure/cancellation terminates the reference run without replacement. E0-F may later introduce retry/failure variants.

## 9. Deterministic spend gate

The manifest freezes the rate assumptions used for **estimated model-token cost**. Provider billing/invoice remains external financial authority.

Before each inference request:

1. prepare exact request;
2. obtain exact input token count from the provider's supported Responses input-token-count endpoint;
3. calculate worst-case next inference token charge using uncached input rate + `4096` output tokens;
4. add to already incurred estimated inference cost;
5. if the sum would exceed USD 5.00, do not send the inference request.

After a response, replace the reservation with an estimated actual charge from returned usage, distinguishing cached/reasoning classes when pricing supports it. Preflight/token-count activity is separately recorded and is not a Performer/Interpreter attempt or fictional event. No claim is made that the USD 5.00 estimate equals final account billing to the cent.

`BudgetExceeded` is terminal non-fictional evidence.

## 10. Integrity and authority reference policy

Live reference Integrity calls Patch 0018 `EvaluateIntegrity` with an initialized empty semantic-concern set. Thus current deterministic source/structural checks remain authoritative and no hidden second model judges/rephrases the Candidate. If Integrity requests another Take, the zero-retry run terminates before Take/commit.

Reference `StateAuthorityPolicy` contains every Patch 0010 policy-eligible domain:

```text
UnresolvedProposition
CharacterBelief
CharacterSuspicion
CharacterGoal
CharacterCircumstance
CharacterClaim
Pressure
```

Patch 0010 still owns hard rejection and transition-specific mandatory review. If evaluation is `ReviewRequired`, the run driver supplies explicit **Reject** choices for every unresolved review-required mutation and resolves through the existing Patch 0018 transition.

These are recorded as `E0AReferenceDeterministicReject`, never as creator/human review. Mandatory-review mutations are never auto-approved. Patch 0011 may still accept the Performance; rejected proposals/reasons remain provenance.

## 11. Provider result mapping

Performer:

```text
completed structured output
 -> save raw/stream diagnostics
 -> PerformerCandidateContract.ParseJson(exact Context)
 -> Patch 0017 BindCandidate
 -> Turn.GateAttempt
```

Refusal/timeout/transport/provider/incomplete/invalid output -> Patch 0017 `TechnicalFailure`; explicit cancel -> `Cancelled`. Partial output remains diagnostics only.

Interpreter:

```text
ReadyForInterpretation
 -> bounded provider request
 -> save raw/stream diagnostics
 -> StateInterpretationContract.ParseJson(exact InterpretationSource)
 -> Turn.EvaluateAuthority
```

Any Interpreter technical/cancel/refusal/timeout/incomplete/schema failure terminates before Take. No partial proposal reaches State Authority.

## 12. Materialization and run loop

For each terminal Approved `Add`, derive one RecordId and create one `E0RecordMaterialization`. Rejected mutations get none; Supersede/Deactivate use existing targets. Provider output never chooses IDs.

Canonical loop for accepted-turn ordinal 1..12:

```text
opportunity-bearing Cycle
 -> ComposeContext
 -> prepare/record Performer request -> spend gate -> one streamed attempt
 -> Patch 0017 -> Turn.GateAttempt
 -> Turn.EvaluateIntegrity(empty concerns)
 -> prepare/record Interpreter request -> spend gate -> one streamed attempt
 -> strict Interpreter parse
 -> Turn.EvaluateAuthority(reference policy)
 -> reject unresolved mandatory-review mutations if required
 -> Turn.BindAcceptedTake(derived TakeId)
 -> deterministic materializations
 -> Turn.CommitAccepted(derived CommitId)
 -> VALID POSTCOMMIT STATE
 -> Patch 0016 EstablishOpportunity
 -> adopt next synchronized opportunity-bearing state
```

Technical/cancel/budget/Integrity/Interpreter terminal outcomes stop immediately. No provider call occurs between accepted commit and deterministic Opportunity establishment.

## 13. Immutable experimental evidence

Run evidence is local experimental provenance, **not Production persistence**.

Minimum run directory:

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

Manifest is immutable before first inference. Attempt artifacts are write-once; event log append-only; finalization records SHA-256 for every artifact. Credentials never enter evidence. Partial/rejected/technical output remains diagnostic and never accepted history. Hidden reasoning is neither requested nor stored.

Record Blueprint-required provenance: Blueprint/fixture/hash; variant/reference identity; access/context; provider/model/native settings; Performer output/control; Director/Integrity/Interpreter/Authority; accepted/rejected mutations/reasons; Take/commit; latency; token usage/cost estimate; technical outcomes; blind mapping.

Blind transcript hides variant/provider/reasoning/RunId/hashes/implementation metadata and replaces Character names with stable neutral speaker labels. Mapping remains separate/private to evaluation coordination.

## 14. Contribution and reasoning-sweep rules

A reference run contributes experiential evidence only if:

- manifest preceded provider execution;
- observable provider/model identity stayed within the frozen condition;
- all 12 Turns reached Accepted Take + atomic commit + next Opportunity;
- no technical/cancel/budget/Integrity terminal occurred;
- evidence finalization succeeded;
- hard-integrity review finds no Blueprint gate violation.

Failed/noncontributing runs remain immutable evidence and are never silently rerun under the same RunId.

LOW/MEDIUM/HIGH characterization reuses the exact envelope except provider-native reasoning setting. Keep 4096 output and USD 5.00 estimated token ceiling matched. Extra reasoning cost/truncation/timeout is therefore observable experimental behavior, not silently compensated. These variants do not replace NONE as the E0 reference.

## 15. Credentials and tests

The first adapter may consume `OPENAI_API_KEY` only as process-injected secret material. Missing credential fails before provider execution. Never persist, echo, hash or pass the secret below the Harness provider edge. This is E0 infrastructure, not the post-E0 secret-store design.

Before any real spend, deterministic fake-adapter tests must prove:

- Core unchanged and dependency direction clean;
- same model/profile applied to all three Performers + Interpreter;
- NONE reference and separate LOW/MEDIUM/HIGH mappings;
- exact Context/request coupling and stale-result rejection;
- one attempt/zero retry; timeout/cancel/refusal/invalid output never fiction;
- cost gate refuses before inference;
- mandatory-review mutations reject, never auto-approve;
- deterministic Take/Commit/Record IDs;
- 12 accepted Turns preserve Cycle/Turn synchronization;
- partial streams remain diagnostic;
- evidence is append-only/write-once and secret-free;
- blind package omits experimental identity.

Native Windows ARM64 compile/tests/Harness smoke must pass before implementation is considered ready for a real provider call.

## 16. Exclusions / approval boundary

No Core redesign; product Application/persistence; provider conversation state; automatic retry/backoff; understudy; mixed-model E0-B; E0-E playwright implementation; E0-F failure implementation; semantic Integrity model; Context optimization; Scene-ending resolution; ODR-12/13/30/32; WinUI; Windows AI/NPU; MSIX/WACK/Store.

Approval freezes: GPT-5.6 Sol/NONE reference; LOW/MEDIUM/HIGH characterization; same profile for Performers + Interpreter; REST/stateless/no-tools/strict-output transport; 12 accepted Turns; one attempt/zero retry/300s timeout; 4096 output ceiling; USD 5.00 estimated model-token ceiling; deterministic authority/review-reject policy; deterministic run-local IDs; immutable evidence/blind package; E0-only Experiment + Harness edge with Core unchanged.

Approval **does not authorize a provider request, credential use, or spend**. Implementation remains fake-first and returns to native ARM64 validation before the first real call.
