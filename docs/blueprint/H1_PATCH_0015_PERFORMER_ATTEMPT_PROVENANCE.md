# H1 Patch 0015 — E0 Performer Attempt Provenance

Status: blueprint proposal 0.1 — EXPLORATORY; recursive adversarial audit not complete; approval not requested
Parent promoted main checkpoint: `7475a9397cff9063673908c666a729f0f3cd4525`
Branch: `h1-patch-0015-blueprint`

## 1. Purpose

Define the next E0-A boundary after promoted Patch 0014 Production Context Continuity.

The current deterministic spine can now prove:

```text
ProductionState
    -> exact current Opportunity
        -> Production-backed Access
            -> exact source-bound ContextPacket v2
                -> CandidatePerformance contract
                    -> Integrity
                        -> State Interpretation
                            -> State Authority
                                -> Take
                                    -> atomic causal commit
                                        -> effective next Opportunity
```

What is still missing for a real E0 run is the non-fictional orchestration record for **one actual Performer attempt**: which bounded Character context was prepared for disclosure, which Performer assignment was selected, what request framing/contract was intended, what attempt identity the run assigned, and how later raw/provider outcomes can be attributed without turning provider mechanics into Production truth.

Patch 0015 therefore proposes a deterministic, provider-neutral **Performer Attempt Provenance boundary**. It does not call a provider/model and does not yet own retry/spend/cancellation execution.

Candidate execution remains later. The point of this patch is to close the provenance and disclosure-association gap that earlier patches intentionally deferred before any network/provider call is allowed.

## 2. Correction to the rejected “Context Consumption” idea

A separate `Character Context Consumption` or `PerformerInput` projection is not proposed.

Frozen Blueprint 0.1 already states:

```text
Production State
  -> deterministic Access Control
      -> permitted information set for this Character
          -> Context Composer
              -> bounded relevance packet
                  -> Performer
```

and separately:

```text
PERFORMER
Receives one bounded Character context and produces a candidate Performance plus any required typed control output.
```

Patch 0006 already implemented the semantic CandidatePerformance contract directly from a ContextPacket and explicitly deferred provider request framing/provenance.

Therefore inserting another generic consumption/input layer would duplicate an already-frozen boundary, weaken the exact ContextPacket identity established by Patches 0005/0014, and create abstraction without a new authority responsibility.

This Proposal 0.1 rejects that path.

## 3. Authority basis

Frozen Blueprint 0.1 requires:

- Character != Performer;
- deterministic Access Control before Context Composer;
- a provider receives a temporary bounded packet for one assigned Performance, not the whole Production by default;
- every external disclosure should be attributable by provider, model, Character, Performance, and ContextPacket identity;
- minimal necessary disclosure is the default;
- provider failures/refusals/timeouts/cancellations remain technical outcomes and never become fiction;
- deterministic systems own spending, retries, cancellation, eligibility, and access policy;
- E0 records complete provenance and immutable comparison batches;
- E0-A uses the same capable frontier model for all three Characters while preserving separate Character contexts.

Prior patch authority also explicitly deferred:

- provider/model/settings;
- provider-request framing/identity;
- exact external disclosure;
- raw/partial/error output;
- attempt identity;
- Run-level attribution;
- retry/spend/cancellation authority.

Patch 0009 stated provider execution was premature until the project had Interpreter disclosure composition, provider-attempt provenance, concern-review authentication, Take timing, retry/cost policy, ProductionState/StateHash, and persistence/commit semantics. The deterministic spine now supplies the semantic/state/take/commit side of that prerequisite list, but provider-attempt provenance and deterministic orchestration remain open.

## 4. Patch boundary

Proposal 0.1 introduces only the deterministic association required to prepare one future provider attempt.

Conceptual flow:

```text
ProductionStateCheckpoint
    -> Patch 0014 Production Context Continuity
        -> source-bound ContextPacket v2
            + configured Performer Assignment descriptor
            + E0 run identity
                -> PerformerAttemptPlan
                    -> deterministic disclosure/request material
                    -> exact semantic provenance association
                    -> later provider adapter/execution
```

No provider/model is called in Patch 0015.

No CandidatePerformance is fabricated by Patch 0015.

No retry is authorized by Patch 0015.

No cost is spent by Patch 0015.

No Production state changes.

## 5. Proposed public concepts

Names are provisional in Proposal 0.1.

### `E0RunId`

Opaque run occurrence identity supplied by orchestration/test harness.

It is not derived from Production content and is not a StateHash/CommitId/TakeId replacement.

### `PerformerAssignmentDescriptor`

Non-secret configured Performer identity for one attempt plan:

```text
ProviderId
ModelId
PerformanceIntent
ConfigurationId
```

Proposal 0.1 intentionally does not freeze provider-specific temperature/token/reasoning fields into Core. Exact provider settings remain adapter/run provenance and may be represented by a canonical non-secret configuration descriptor or hash only after the setting-boundary audit.

No credential, API key, account token, endpoint secret, or protected-local-store locator may enter this descriptor.

### `PerformerAttemptId`

Identity of one attempted generation occurrence within a Run.

It is not a hash of CandidatePerformance content because multiple attempts may legitimately return identical semantic output.

It is not a ContextPacketId because multiple attempts may legitimately use the same ContextPacket.

Proposal 0.1 leaves allocation strategy open pending uniqueness/replay audit.

### `PerformerAttemptPlan`

Proposed immutable association object:

```text
ContractVersion
RunId
AttemptId
SourceStateHash
SceneId
SubjectCharacterId
ContextPacketId
StructuredContextHash
RenderingContract
RenderedContextHash
PerformerAssignment
RequestContract
RequestMaterialHash
```

The plan contains associations/identities, not Character-visible prose, raw request payload, credentials, raw response, CandidatePerformance, Take, CommitId, or state authority.

## 6. Production-bound Context requirement

Ordinary E0-A reference attempt planning must require a Patch 0014 production-bound Context v2 packet.

Required association:

```text
ContextPacket.SchemaVersion == ensemble.e0.context.v2
ContextPacket.CompositionContract == ensemble.e0.context.production-bound.v1
ContextPacket.SourceStateHash initialized
```

Legacy v1 Context remains valid for historical deterministic regression/compatibility but is not proposed as sufficient authority for a new external Performer attempt.

Rationale:

- external disclosure must be attributable to exact current Production authority;
- Patch 0014 exists specifically to prove this association;
- allowing a fresh provider attempt from unbound v1 would reopen the exact-source ambiguity Patch 0014 closed.

## 7. SourceStateHash is provenance, not prompt content

Patch 0014 classifies SourceStateHash as non-diegetic system association metadata.

Therefore:

- it belongs in PerformerAttemptPlan provenance;
- it may bind attempt identity/association;
- it must not be inserted into Character-facing provider prompt text by default;
- a provider does not need to know the Production StateHash to portray the Character.

Any future debug/provider mode that transmits it would require explicit separate disclosure authority and would not silently reuse the reference E0 request contract.

## 8. Request material versus provider transport

Proposal 0.1 distinguishes three layers.

### Layer A — Character semantic context

Existing `ContextPacket` v2 and `RenderedContext` v1.

### Layer B — deterministic Performer request material

Provider-neutral material constructed only from safe ContextPacket fields plus frozen Performer output instructions/control vocabulary.

This is the candidate Patch 0015 responsibility.

### Layer C — provider-specific transport

HTTP/API request envelope, provider role/message format, SDK object graph, headers, endpoint, authentication, provider-specific settings, streaming protocol, and raw response.

This remains later provider-adapter/execution scope.

A `RequestMaterialHash` therefore proves exact Layer-B bytes under a named `RequestContract`; it must not claim to be the hash of exact Layer-C network bytes.

## 9. Proposed deterministic request material

Proposal 0.1 suggests one versioned E0 reference request-material contract.

Working token:

```text
ensemble.e0.performer.request-material.v1
```

Exact canonical bytes remain to be frozen after adversarial audit.

The semantic content may contain only:

1. trusted Performer system contract;
2. safe Character roster map from ContextPacket roster metadata;
3. `Rendered.TrustedStateText`;
4. `Rendered.RecentPerformanceText`;
5. `Rendered.OpportunityText`;
6. strict `ensemble.e0.performer.candidate-json.v1` output instructions.

It must not include:

- SourceStateHash;
- record provenance/lifecycle/protection metadata;
- denied Access audit rows;
- creator-only truth excluded from Context;
- other Characters' private Context;
- credentials;
- provider secrets;
- diagnostic logs;
- prior rejected/alternate/failed attempts unless a later approved context rule explicitly includes them;
- hidden chain-of-thought requests;
- State Authority policy internals;
- Director diagnostics beyond already-rendered opportunity semantics.

## 10. Roster control map

Patch 0006 already established that future AI provider request construction needs stable Character IDs for typed address/nomination control while creative rendering can remain human-readable.

Proposal 0.1 therefore includes a machine-control roster map derived only from the safe ContextPacket roster:

```text
CharacterId -> DisplayName
```

Rules:

- exact Context roster only;
- canonical ordinal CharacterId order;
- no extra Production roster/cast entries;
- no hidden metadata;
- no relationship/private-state expansion;
- no alias that maps one ID to multiple Characters;
- request material tells the Performer that typed control IDs must use these exact IDs.

This map is control syntax support, not world truth or social authority.

## 11. Request contract must preserve authority layering

Future request material must distinguish trusted instructions from untrusted creative text.

The request contract must not concatenate everything into an undifferentiated prompt where fictional dialogue can impersonate system instructions.

At minimum, canonical request material must preserve separable sections for:

```text
SYSTEM CONTRACT
SAFE CONTROL VOCABULARY
TRUSTED CHARACTER STATE
RECENT PERFORMANCE CONTENT
CURRENT OPPORTUNITY
OUTPUT CONTRACT
```

Imported/user content remains untrusted even when included inside trusted structured state as quoted creative material.

Exact transport role mapping is provider-adapter scope.

## 12. Attempt planning is not execution authorization

Creating a PerformerAttemptPlan does not itself authorize:

- external network disclosure;
- spend;
- retry;
- model substitution;
- understudy use;
- streaming;
- provider failover;
- cancellation policy;
- another take;
- commit.

Proposal 0.1 deliberately leaves the question open whether Patch 0015 should also define a deterministic `AttemptAuthorization` decision or whether that belongs in a separate Patch 0016 orchestration/cost-policy boundary.

Default audit posture: split it unless a real implementation need proves the plan is unusable without an authorization token.

## 13. Provider Assignment remains separate from Character

PerformerAssignmentDescriptor is recasting metadata, never Character identity.

Changing provider/model/configuration:

- does not change CharacterId;
- does not rewrite Character state;
- does not change ContextPacket semantic content by itself;
- creates a different Performer attempt association;
- must remain attributable in E0 provenance.

E0-A reference runs require the same ProviderId/ModelId/configuration intent across all three Characters within one comparison batch, while their ContextPacket identities differ by Character perspective.

E0-B later varies Performer assignments deliberately.

## 14. No provider-specific API in Core

Patch 0015 must not add OpenAI-, Anthropic-, Google-, Microsoft-, or other provider SDK dependencies to `Ensemble.E0.Core`.

No HTTP client belongs in Core.

No Windows AI Foundry / NPU API belongs in this patch.

Any future provider adapter should depend on the frozen provider-neutral attempt/request contract, not the reverse.

This keeps deterministic Core testable, portable, ARM64-friendly, and free of network/credential side effects.

## 15. No secret persistence

Performer attempt provenance may identify:

- provider;
- model;
- non-secret configuration identity;
- ContextPacket identity;
- request-material contract/hash;
- result classification;
- metrics later.

It must never store:

- API keys;
- OAuth tokens;
- cookies;
- provider session secrets;
- credential-store handles that would make exported Production/run artifacts usable as authentication material.

Credential association is local runtime configuration outside portable Production provenance.

## 16. Attempt outcome taxonomy — reserved but constrained

Patch 0015 does not need to implement provider execution, but it must leave a non-fictional outcome vocabulary possible.

Future attempt outcomes must distinguish at least:

```text
CompletedResponse
ProviderRefusal
TransportFailure
Timeout
Cancelled
MalformedOutput
```

No technical outcome may automatically become CandidatePerformance.VisibleText.

A Character refusal is valid fictional output only when the provider successfully returns a valid candidate whose visible Performance expresses refusal.

## 17. Raw and partial output provenance

Frozen E0 requires attempted raw output/control, partial streams, failures/refusals/cancellations, provider/model/settings, and metrics to be retained as experimental provenance/diagnostics where applicable.

Proposal 0.1 does not store those outputs yet because there is no provider execution.

It does require the AttemptId/RunId/Context/request association that later raw outcome records must reference.

This prevents a future provider adapter from emitting detached logs that cannot be causally associated with the source Character context.

## 18. Run identity and immutable comparison batches

A Performer attempt must belong to exactly one E0 Run.

Proposal 0.1 does not define the full E0-A through E0-G batch manifest, but RunId is introduced because later provenance cannot reliably attach Take/attempt/provider facts without an occurrence scope.

A future Run manifest may retain:

- fixture version/hash;
- experimental condition;
- provider/model assignments;
- fixed settings;
- RunId sequence;
- attempt provenance;
- Take/commit outcomes;
- metrics and blind-review artifacts.

Patch 0015 must not silently freeze final experiment-storage format before the actual run loop exists.

## 19. Identity distinctions

Proposal 0.1 preserves:

```text
StateHash
    authoritative Production-state identity

ContextPacketId
    exact structured bounded Character context identity

RenderedContextHash
    exact provider-neutral rendered Character disclosure identity

RequestMaterialHash
    exact provider-neutral Performer request-material bytes under RequestContract

PerformerAttemptId
    occurrence identity for one attempted generation

CandidateContentHash
    semantic CandidatePerformance content identity

TakeId
    Take occurrence/package identity

CommitId
    atomic causal-commit identity

RunId
    E0 run occurrence identity
```

No identity silently substitutes for another.

## 20. Replay and determinism

Patch 0015 planning/request construction must be deterministic.

Given the same:

```text
source ContextPacket v2
+ same PerformerAssignmentDescriptor
+ same RunId/AttemptId values
+ same RequestContract
```

it must produce identical canonical Layer-B request material and RequestMaterialHash.

AttemptId/RunId may be caller-supplied occurrence identities and therefore are not required to be derived deterministically from content.

A later replay/audit can recompute request material from retained semantic source and compare the exact bytes/hash.

This is reconstruction proof, not provider authentication and not proof that an external service received those bytes.

## 21. Authentication boundary

Patch 0015 must not claim that an immutable attempt plan authenticates provider execution.

It proves only deterministic local association.

Later provider execution/provenance must establish, as appropriate:

- which configured adapter executed;
- provider/model response attribution;
- exact provider-specific request framing actually sent;
- raw response/stream/error association;
- authenticated/local runtime credential selection without persisting secrets;
- metrics/cost/retry/cancellation facts.

Hash association is not authentication.

## 22. Dependency direction

Proposed dependency direction:

```text
Domain / Production / Access / Context
    -> Performer Attempt contract
        -> future provider adapters / Harness orchestration
```

Existing Candidate/Integrity/Interpreter/Authority/Take/CausalCommit/Opportunity must not depend on provider transport.

Performer Attempt provenance may reference Context identities and Performer candidate transport contract constants for output instructions, but must not create a reverse dependency from Context into orchestration/provider code.

Whether the attempt contract belongs in Core or a new E0 orchestration assembly is an explicit Proposal 0.1 audit question. Current preference: keep purely deterministic semantic/request canonicalization in Core only if it remains provider-neutral and side-effect free; keep run storage, credentials, network, timers, metrics, and adapter execution outside Core.

## 23. Candidate parser relationship

Future successful provider response path:

```text
Provider-specific completed response bytes
    -> extract exact candidate-json payload without rewriting
        -> PerformerCandidateContract.ParseJson(source ContextPacket, bytes)
            -> CandidatePerformance
```

The provider layer must not silently repair malformed Candidate JSON into valid fiction.

A retry/repair decision, if later allowed, is a separately attributed attempt under deterministic retry/spend policy.

## 24. State Interpreter provider path remains separate

Patch 0015 concerns Character Performer attempts only.

State Interpreter semantic-model execution remains a separate later request/disclosure/provenance path because:

- it consumes a different bounded semantic source;
- it proposes consequences rather than Character Performance;
- its output schema differs;
- it has different authority/privacy risks.

Do not generalize Proposal 0.1 into an abstract “AI request framework” solely to share code before both real callers exist.

## 25. Concern-review provenance remains separate

Integrity model-assisted concern evidence, when used, must later be attributable to its configured review path/provider attempt.

Patch 0015 does not authenticate or execute Integrity semantic review.

The first patch should close the Character Performer attempt seam without building a speculative universal assessor/provider provenance hierarchy.

## 26. No persistence claim

Patch 0015 may define immutable record contracts and canonical bytes/hashes.

It does not by itself provide durable persistence/recovery.

A future run store must persist E0 provenance without confusing it with Production causal history.

Provider-attempt diagnostics may be deletable independently only if doing so does not destroy provenance required for a frozen E0 comparison record. Final product diagnostic-retention policy remains open.

## 27. ARM64 and battery implications

Patch 0015 is deliberately model-free and side-effect-free.

No:

- network call;
- background polling;
- filesystem requirement;
- timer loop;
- GPU/NPU work;
- Windows AI API;
- provider SDK;
- emulation path.

The runtime cost is bounded serialization/hash work at explicit Performance boundaries only.

This preserves low idle battery impact and keeps future heavy inference isolated behind an explicit provider boundary where capability gating and NPU/provider policy can later be implemented correctly.

This is architecture reasoning, not measured power/performance evidence.

## 28. Explicit non-scope

Patch 0015 Proposal 0.1 does not implement:

- actual provider/model invocation;
- credentials/secrets integration;
- HTTP/streaming;
- deterministic spend/retry/cancellation policy;
- understudy selection/failover;
- State Interpreter provider invocation;
- Integrity semantic-assessor invocation;
- full E0 Run manifest/store;
- durable persistence/recovery;
- CharacterClaim disclosure;
- recent-Performance population beyond existing Context semantics;
- Observation;
- World Resolver;
- full Scene loop;
- WinUI;
- Windows AI Foundry;
- NPU execution;
- MSIX;
- WACK;
- Store certification.

## 29. Proposal 0.1 implementation hypothesis

If this architecture survives audit, the smallest implementation surface is expected to be:

```text
new deterministic Performer-attempt/request models
new canonical request-material serializer/hash
focused Core tests
no modification to provider/network/Harness execution yet
```

Patch 0014 source should remain unchanged except only if a narrowly proven accessor/visibility change is required. Prefer no change.

Existing Candidate, Integrity, Interpreter, State Authority, Take, CausalCommit, Opportunity, Fixture, and Harness semantics should remain unchanged.

## 30. Required tests if later approved

At minimum:

- production-bound Context v2 accepted;
- legacy/unbound Context v1 rejected for new external attempt planning;
- exact source StateHash/Scene/subject/ContextPacket associations copied correctly;
- SourceStateHash absent from canonical creative request bytes;
- denied/private/provenance metadata cannot enter request material;
- roster map exact/canonical and safe;
- deterministic request bytes/hash across repeat and culture changes;
- same Context + different provider/model descriptor changes attempt association without changing Context identity;
- same Context + different AttemptId remains distinct occurrence;
- request-material hash does not claim provider-transport identity;
- credentials cannot be represented by public contracts;
- no network/provider SDK dependency enters Core;
- malformed/default IDs fail closed;
- no Candidate/Take/Commit/state mutation created;
- no hidden repair of candidate output;
- public surface remains minimal.

## 31. Recursive adversarial audit questions

Proposal 0.1 is not ready for approval. Audit must answer:

1. Is Performer Attempt Provenance actually the next required E0 seam, or should deterministic spend/retry/cancellation authority precede it?
2. Does `PerformerAttemptPlan` belong in Core, or should Core expose only deterministic request canonicalization while occurrence/provenance lives in a Harness orchestration assembly?
3. Is `RequestMaterialHash` necessary now, or can exact canonical bytes plus existing RenderedContextHash suffice until a provider adapter exists?
4. What exact request material is necessary to satisfy Candidate JSON control without introducing prompt-architecture policy prematurely?
5. Should provider/model configuration be semantic structured fields, one canonical descriptor/hash, or entirely outside Core until the first real provider adapter?
6. How is AttemptId allocated so duplicate IDs fail within a Run without introducing global mutable state into Core?
7. What exact relationship exists between one Attempt and one future CandidatePerformance when malformed/partial/refusal outcomes produce none?
8. Does a future retry create a new AttemptId under the same ContextPacket or require fresh Context recomposition if state remains unchanged?
9. What provider-independent information is safe to persist in E0 provenance while keeping credentials and provider-sensitive payloads local?
10. Does E0-A require the State Interpreter provider path in the same milestone, or can Performer execution be validated first with deterministic/synthetic interpretation while preserving experiment integrity?
11. What is the minimal complete run record required before behavioral E0 transcripts can be treated as valid experimental evidence?
12. Does exact provider request attribution require freezing Layer C now, making Proposal 0.1 too early to define Layer-B identity?

## 32. Current recommendation

Proposal 0.1 direction is stronger than the rejected Context Consumption idea because it closes an explicitly deferred, source-supported gap rather than inventing a second boundary between Context and Performer.

However, the patch is **not frozen**.

The next action is recursive adversarial audit of Proposal 0.1 against Blueprint 0.1, Patches 0005/0006/0008/0009/0011/0014, current source dependency direction, E0-A control-isolation requirements, privacy, provenance, and future provider execution. Any material correction restarts the audit.

No implementation is permitted until the resulting proposal reaches a full clean pass and receives explicit user approval.
