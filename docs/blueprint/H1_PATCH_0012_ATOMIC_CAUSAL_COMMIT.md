# H1 Patch 0012 — E0 Atomic Causal Commit Contract

Status: blueprint proposal 0.1 — RECURSIVE ADVERSARIAL AUDIT IN PROGRESS; approval required; implementation not started
Parent repository checkpoint: `main` at `20b26711ceffa17e0543ca3fc180c9acf69f5e45`
Parent machine-tested executable/test authority: H1 Patch 0011 at `4250011c167cd9850ad891aaea4ee053216cf135`
Branch: `h1-patch-0012-atomic-causal-commit-blueprint`

## 1. Purpose

Define the next E0 deterministic-spine boundary after machine-validated Patch 0011 Take Semantics:

```text
immutable Accepted E0Take
+ authoritative source-state binding
+ current ProductionState projection
+ supplied CommitId
+ deterministic RecordId materialization for Approved Add/Supersede mutations
    -> strict freshness validation
        -> all-or-nothing consequence application
            -> E0CausalCommit
            -> new immutable ProductionState projection
```

Patch 0012 owns only the atomic causal-commit semantic boundary needed by E0.

It establishes the minimum in-memory authoritative Production-state identity and mutation-application semantics necessary to prove that:

- an Accepted Take and every retained Approved consequence become effective together;
- no retained Rejected consequence becomes effective;
- failed/stale/invalid commit attempts alter nothing;
- causal history remains reconstructible rather than being replaced by an opaque mutable snapshot;
- successful causal commit consumes the source Current Opportunity but does not choose the next opportunity.

Patch 0012 does not implement durable storage, recovery, branch DAGs, rehearsal/retcon/canon-promotion UX, provider execution, Scene-loop orchestration, next-Director opportunity selection, Observation, World Resolver, WinUI, Windows AI/NPU, packaging, WACK, or Store behavior.

## 2. Recovered frozen authority

Blueprint 0.1 freezes:

- Production State precedes deterministic Access Control and Context composition;
- Performer, Director, State Interpreter, and models do not own persistence authority;
- the conceptual source of truth is append-only causal event history, not a mutable snapshot;
- Accepted Take plus committed consequences enters causal event history and projects current World/Character/Knowledge/Relationship/Pressure state;
- Accepted Performance and all authoritative Approved consequences form one atomic causal commit;
- either the accepted Take and all Approved mutations commit coherently, or neither does;
- a transcript must never say an event happened while current authority says it did not;
- historical texture remains true even when no durable projected-state mutation is created;
- Accepted Takes are immutable;
- corrections, retcons, branches, and alternate takes are later explicit causal authority, never silent rewrites;
- Production history and diagnostics are distinct;
- a committed consequence must remain traceable to an accepted Performance or another authorized cause;
- Performance may not commit without Approved consequences, and consequences may not commit without the accepted Performance.

Patch 0011 additionally freezes:

- only `E0TakeDisposition.Accepted` is commit-eligible;
- Rejected and Alternate Takes remain non-effective;
- Accepted means commit-eligible, not already historical;
- successful commit may apply only retained Approved consequences, every retained Approved consequence, and no retained Rejected consequence;
- freshness handling may reject an immutable Take but may not silently rewrite its retained consequence package;
- failed commit does not advance Current Opportunity;
- only after successful atomic source commit may later orchestration recompute Director authority and separately establish the next effective opportunity;
- a successful causal commit must associate CommitId with TakeId;
- Rejected/Alternate/failed-before-Take/Accepted-but-failed-commit material has no effective causal CommitId.

## 3. Ordering and authority

Patch 0012 preserves the E0 ordering:

```text
ProductionState source
    -> Access / Context / Performance / Integrity / Interpretation / State Authority
        -> immutable E0Take
            -> Take/source-state binding
                -> strict current-state freshness check
                    -> deterministic atomic causal commit
                        -> new ProductionState projection
                            -> later Director/current-opportunity continuation
```

The commit boundary does not reinterpret prose, rerun a model, choose Take disposition, or invent new State Authority decisions.

The retained Patch 0011 Take is authoritative for the exact consequence package. Patch 0012 owns only freshness, materialization, atomic application, causal-event construction, and next-state identity.

## 4. Causal history is source; ProductionState is projection

Patch 0012 must not make a mutable snapshot the canonical history.

The architectural distinction is:

```text
ValidatedFixture
    -> immutable genesis input

append-only E0CausalCommit sequence
    -> causal Production history / source of truth

ProductionState
    -> immutable reconstructible current projection of genesis + causal history head
```

`ProductionState` is authoritative for current deterministic evaluation while it is current, but it is conceptually reconstructible from genesis plus causal commits. Durable persistence may later persist events and cache projections; Patch 0012 does not choose a database/event-store product.

The original `ValidatedFixture` remains immutable genesis data. Patch 0012 must not mutate it or pretend an evolved Production is still the original fixture.

## 5. Namespaces and contracts

Proposed neutral state namespace:

```text
Ensemble.E0.Core.Production
```

Proposed atomic commit namespace:

```text
Ensemble.E0.Core.CausalCommit
```

Conceptual contract holders:

```text
ProductionStateContracts
- StateContractVersion = "ensemble.e0.production-state.v1"
- StateHashContractVersion = "ensemble.e0.production-state-hash.sha256.v1"

E0CausalCommitContracts
- ContractVersion = "ensemble.e0.causal-commit.v1"
```

Physical `.cs` grouping is not frozen. Implementation should use the smallest coherent files consistent with existing Core patterns.

## 6. Existing CommitId is canonical

`CommitId` already exists in `Ensemble.E0.Core.Domain.StrongIds` and remains canonical.

Patch 0012 must not introduce another Commit identifier type.

Patch 0012 does not freeze a global CommitId allocator or string format beyond existing canonical strong-ID validation.

Core receives a supplied initialized CommitId. It does not derive CommitId from:

- clock/time;
- random/GUID generation;
- TakeId;
- CandidateContentHash;
- ProposalContentHash;
- StateHash;
- provider/model identity.

A supplied CommitId becomes an **effective causal CommitId only if the atomic operation succeeds and returns an `E0CausalCommit`**. A failed attempt may be diagnostic material outside Production history, but no effective commit record is fabricated.

## 7. StateHash is causal current-state identity

Patch 0012 introduces the first authoritative current Production-state identity.

`StateHash` is not an externally assigned ID. It is a deterministic lowercase SHA-256 content identity produced only by the canonical Production-state hashing contract.

A valid StateHash must bind both current projection and causal head so that a state which temporarily returns to the same projected values after intervening causal history does not silently become identical to an earlier causal state.

Conceptual computation:

```text
genesis StateHash
    = SHA256(
        state-hash contract
        + canonical genesis projection)

result StateHash
    = SHA256(
        state-hash contract
        + ParentStateHash
        + canonical causal-commit payload identity
        + canonical result projection identity)
```

The exact byte canonicalization must be frozen before implementation. It must use invariant UTF-8/ordinal rules, length-safe field framing or equivalent unambiguous canonical encoding, stable enum representation, canonical array ordering, and no culture/clock/platform-dependent serialization.

`StateHash` remains distinct from Fixture hash, ContextPacketId, CandidateContentHash, ProposalContentHash, TakeId, CommitId, and RecordId.

## 8. ProductionState conceptual surface

`ProductionState` is immutable and sealed.

It conceptually retains only current authoritative projection material and derived replay/index state needed for deterministic application:

```text
ContractVersion
StateHash
origin FixtureId / FixtureFamilyId / FixtureVersion identity
SceneId
canonical Scene roster
CurrentOpportunityCharacterId? 
all Production records, including inactive superseded/deactivated records
creator/system protection semantics
head CommitId? / committed CommitId and committed TakeId indexes needed to fail closed on duplicate effective commit
```

The exact public versus internal projection/index surface remains an implementation-hygiene decision during architecture audit; public exposure must be minimized.

`ProductionState` does not retain provider responses, model identity, prompt text, credentials, token usage, chain-of-thought, or diagnostics.

## 9. Production record model

Patch 0012 requires a neutral Production record representation because `ValidatedFixture` is genesis-only and `StateAuthoritySnapshot` is an authority descriptor projection without full durable record text/provenance.

The Production record model must preserve, at minimum:

- RecordId;
- exact domain;
- Active / Inactive lifecycle;
- None / SystemImmutable / CreatorLocked protection;
- exact text where the domain has durable text;
- canonical RecordId provenance/support references;
- subject CharacterId when character-scoped;
- target CharacterId when relationship-scoped.

Initial Production records are deterministically projected from `ValidatedFixture` using the already-approved State Authority domain/protection laws:

- HistoricalTruth, Character Constitution, and Character Observation are SystemImmutable;
- creator locks become CreatorLocked unless stronger SystemImmutable protection already applies;
- all genesis records begin Active;
- Scene and roster are exact fixture semantics.

Patch 0012 must not silently collapse claim, belief, memory, knowledge, truth, relationship, pressure, or other frozen domains into a generic fact bucket.

## 10. ProductionState genesis

Conceptual genesis boundary:

```text
ProductionState.Initialize(
    ValidatedFixture fixture,
    ImmutableArray<RecordId> creatorLockedRecordIds)
    -> ProductionState
```

Genesis validation must reuse existing canonical fixture and State Authority rules rather than inventing looser state initialization.

The fixture remains the immutable origin object; the initialized `ProductionState` is a distinct projection with its own StateHash.

Initial Current Opportunity is exactly `ValidatedFixture.InitialOpportunity`.

No causal CommitId exists at genesis.

## 11. Evolved State Authority projection

Patch 0010 currently binds `StateAuthoritySnapshot` from `ValidatedFixture`.

Patch 0012 must make deterministic State Authority projection possible from evolved `ProductionState` without weakening the existing fixture path.

Architectural requirement:

```text
StateAuthoritySnapshot.Bind(ProductionState state)
```

or an equivalently narrow canonical overload/helper owned by State Authority.

The existing fixture-based Bind remains valid for frozen regressions.

The evolved-state projection must preserve exact Scene, roster, RecordId, domain, lifecycle, and protection semantics. It contains no model reasoning and performs no mutation itself.

This is the minimum upstream compatibility extension required for evolved-state freshness. It must not duplicate State Authority decision logic inside the commit subsystem.

## 12. Take/source-state binding

Patch 0011 intentionally did not add StateHash to `E0Take`. Patch 0012 must not mutate the frozen Take surface merely to retrofit that identity.

Instead Patch 0012 proposes a separate immutable binding object, conceptually:

```text
E0TakeStateBinding
- SourceStateHash
- TakeId
- SourceContextPacketId
- SourceCharacterId
```

with sole rich construction:

```text
E0TakeStateBinding.Bind(
    ProductionState sourceState,
    ContextPacket sourceContext,
    E0Take take)
```

Binding must validate at minimum:

- exact TakeId initialized;
- source ContextPacketId equals `take.Performance.ContextPacketId`;
- source Character equals `take.Performance.SubjectCharacterId`;
- source Context subject equals source Context opportunity;
- source Character equals `sourceState.CurrentOpportunityCharacterId`;
- source Scene equals both ProductionState Scene and Take proposal source Scene;
- source roster equals ProductionState roster canonically;
- a fresh StateAuthoritySnapshot projected from `sourceState` is semantically identical to the StateAuthoritySnapshot retained inside the Take authority trace.

If those checks pass, the binding records the exact `sourceState.StateHash` without modifying the Take.

## 13. Explicit Context-source identity limit

Even after Patch 0012 introduces StateHash, the current Patch 0004/0005 Access/Context contracts do not carry a StateHash and cannot yet cryptographically prove that every field inside an existing ContextPacket was composed from the exact supplied evolved ProductionState.

Patch 0012 must not hide that limit.

The proposed Take/source-state binding proves:

- Context identity/subject/scene/roster/opportunity structural association;
- Take Performance association with that ContextPacketId;
- Take State Authority snapshot equivalence to the bound ProductionState;
- strict source StateHash freshness for commit.

It does **not** retroactively prove full Context content derivation from the state.

Effective E0 orchestration must continue to preserve source-state/context provenance. A later Access/Context migration may strengthen this by propagating authoritative StateHash through those boundaries. Patch 0012 must not rewrite frozen Context hashes or add a fake alias merely to claim proof that does not exist.

## 14. Strict freshness policy

Patch 0012 chooses a strict E0 freshness rule rather than re-evaluating and silently changing an immutable Take:

```text
currentState.StateHash == takeStateBinding.SourceStateHash
```

is required for successful commit.

If the hash differs, commit fails closed.

No attempt is made to decide that a later state is "close enough" or that the same Approved/Rejected decision set would probably still apply.

Reason:

- Patch 0011 Take consequences are immutable;
- a changed causal head can change recent history/context even when projected durable values happen to match;
- strict hash equality prevents stale Takes from being repurposed;
- E0 values deterministic authority over throughput.

This rule may be relaxed only by a later explicitly approved re-evaluation/new-Take protocol, never by rewriting the existing Take.

## 15. Commit eligibility

Atomic commit requires all of the following:

- non-null initialized current ProductionState;
- initialized supplied CommitId;
- structurally valid `E0TakeStateBinding` matching the Take;
- `E0Take.Disposition == Accepted`;
- retained Take Authority evaluation remains terminal Complete with no RequiresReview;
- Take binding SourceStateHash equals current ProductionState StateHash;
- current opportunity exists and equals the Take Performance subject;
- current ProductionState re-projects to a State Authority snapshot semantically identical to the Take authority snapshot;
- supplied RecordId materialization set is exact and valid for Approved mutations that create replacement/new records;
- CommitId has not already become effective in current causal projection indexes;
- TakeId has not already become effective in current causal projection indexes.

Rejected or Alternate Takes fail before any state construction.

## 16. RecordId materialization authority

`RecordId` already exists. Patch 0012 does not freeze a global RecordId allocator or derive IDs from clock/random/provider/model data.

Core receives an explicit materialization set from higher-level deterministic orchestration.

Use an index-addressed typed mapping rather than a bare positional list, conceptually:

```text
E0RecordMaterialization
- MutationIndex
- RecordId

E0RecordMaterializationSet.Bind(...)
```

One new RecordId is required for every **Approved** mutation whose transition creates a durable record:

- Add -> one new RecordId;
- Supersede -> one new replacement RecordId;
- Deactivate -> no new RecordId.

Rejected mutations require no RecordId.

The set must be canonical, unique by mutation index and RecordId, contain no extra entries, and contain no RecordId that already exists anywhere in the Production record ledger, including inactive records.

Record IDs are never reused after supersession/deactivation.

## 17. Deterministic mutation application

Patch 0012 applies only the exact retained Take proposal/decision pair.

For each mutation index in canonical order:

```text
StateAuthorityDisposition.Rejected
    -> no effective state mutation

StateAuthorityDisposition.Approved
    -> apply exact corresponding proposal mutation

StateAuthorityDisposition.RequiresReview
    -> impossible for a valid Take; fail closed if encountered
```

No new semantic decision occurs during application.

### Add

Create one new Active Production record with the supplied materialized RecordId and exact proposal text/subject/target/domain semantics.

Record provenance is the canonical supporting RecordId set carried by the proposal.

### Supersede

Require the referenced existing record to be current, Active, and match the exact domain/subject/target semantics already approved by State Authority.

Mark the existing record Inactive and create one new Active replacement record with the supplied materialized RecordId and exact replacement text.

Replacement record provenance must include the superseded RecordId plus the proposal supporting RecordIds in canonical unique order.

### Deactivate

Require the referenced record to be current, Active, and match the exact approved domain/subject/target semantics.

Mark it Inactive. No replacement record is created.

## 18. Historical Performance versus durable projection

Every successful causal commit retains the exact Accepted `CandidatePerformance` through the exact immutable `E0Take` association even when the proposal contains zero mutations or all mutations are Rejected.

Therefore:

```text
Accepted Take + zero effective durable mutations
    -> valid causal commit
    -> Performance is historical truth
    -> durable Production record projection may otherwise remain unchanged
```

The resulting StateHash still changes because the causal head changed even when durable projected records did not.

This preserves Blueprint 0.1 historical texture without forcing every performed detail into permanent state fields.

## 19. Current Opportunity consumption

A successful causal commit consumes the source Current Opportunity because that Performance has become effective history.

The result ProductionState therefore has:

```text
CurrentOpportunityCharacterId = null
```

until later deterministic Director/orchestration authority establishes the next opportunity.

Patch 0012 does not choose that next Character and does not call Director.

Failed/stale/Rejected/Alternate commit attempts leave the current ProductionState unchanged, including Current Opportunity.

The committed source Character remains recoverable from the exact Take/Performance inside the causal commit.

## 20. E0CausalCommit event

Conceptual immutable event:

```text
public sealed class E0CausalCommit
- ContractVersion
- CommitId
- ParentStateHash
- ResultStateHash
- exact Accepted E0Take
- committed source/opportunity CharacterId
- ordered applied mutation effects
```

The event must preserve enough exact typed information to reconstruct the authoritative result projection from its parent state without consulting diagnostics or provider data.

Applied effects must make newly materialized RecordIds explicit.

Rejected Take mutations remain recoverable from the retained exact E0Take Authority evaluation/proposal, but are not represented as applied state effects.

No provider response, prompt, token usage, credentials, or hidden reasoning is retained.

## 21. Atomic result boundary

Conceptual public operation:

```text
E0CausalCommitResult Commit(
    CommitId commitId,
    ProductionState currentState,
    E0TakeStateBinding takeStateBinding,
    E0Take take,
    E0RecordMaterializationSet materializations)
```

Result:

```text
E0CausalCommitResult
- Commit
- ResultState
```

All validation and all result projection construction occur before either successful object is returned.

Because all Core inputs/outputs are immutable and Patch 0012 performs no filesystem/database/network writes, an exception before return exposes no partially mutated ProductionState.

This proves **in-memory semantic atomicity** only. Durable storage transaction/recovery authority remains later work and must eventually persist the causal event as source of truth without permitting transcript/state divergence.

## 22. Canonical StateHash contents

Before implementation, the canonical hashing contract must freeze exact fields and order.

At minimum genesis projection identity includes:

- state/hash contract versions;
- origin FixtureId / family / version and canonical Fixture hash;
- SceneId;
- canonical roster;
- Current Opportunity;
- every Production record in canonical RecordId order with domain/lifecycle/protection/subject/target/text/provenance.

At minimum post-commit causal payload identity includes:

- Commit contract version;
- CommitId;
- ParentStateHash;
- TakeId;
- Candidate content identity/hash as established by the Take pipeline;
- Proposal content identity/hash as established by State Authority input;
- exact ordered State Authority decisions and reason codes;
- committed source Character;
- exact ordered applied mutation effects/materialized RecordIds.

Result projection identity includes the same canonical Production projection fields as genesis plus derived effective CommitId/TakeId replay indexes where those indexes affect validity.

Do not hash object runtime addresses, reflection metadata order, exception strings, culture-dependent formatting, or JSON emitted by an unspecified serializer configuration.

## 23. Replay and reconstruction law

Given identical:

- genesis fixture;
- creator-lock set;
- ordered successful causal commit events;

replay must reconstruct an equivalent ProductionState and exact final StateHash.

A causal commit must be rejected during replay if its ParentStateHash is not the current replay StateHash.

This creates a linear E0 causal chain without freezing post-E0 branch DAG semantics.

Patch 0012 does not delete or rewrite prior commits.

## 24. Duplicate effective identity protection

A successful state projection must permit deterministic rejection of:

- re-committing the same effective CommitId;
- re-committing the same accepted TakeId.

These are derived causal indexes, not substitute source history.

This protection is limited to committed effective history. It does not claim global uniqueness for Rejected/Alternate/non-Take diagnostic attempts.

## 25. Creator/system protection preservation

Mutation application does not bypass State Authority.

Strict source-state hash equality plus exact source-snapshot equivalence means the Take is committed only against the same authoritative record/protection projection under which its retained decisions were produced.

The commit layer must never independently override SystemImmutable or CreatorLocked protection.

If an internally malformed Take/effect attempts an impossible transition despite those checks, application fails closed rather than manufacturing effective state.

## 26. Exception boundary

Patch 0012 should define a publicly catchable, sealed expected contract-failure exception with no public construction path, following the Patch 0011 pattern.

Expected failures include:

- null/missing inputs;
- uninitialized CommitId/RecordId;
- non-Accepted Take;
- binding/Take mismatch;
- stale SourceStateHash;
- source/current opportunity mismatch;
- source/current State Authority snapshot mismatch;
- duplicate effective CommitId/TakeId;
- missing/extra/duplicate/colliding materialized RecordIds;
- malformed decision/mutation alignment;
- invalid Add/Supersede/Deactivate target state;
- canonical state/commit hashing failure caused by malformed trusted semantic input.

Messages and retained inner exceptions must remain structural and sanitized. Candidate text, Context prose, mutation text, provider content, credentials, or arbitrary user content must not leak through exception representation.

Unexpected programming/runtime failures must not be converted into an ordinary commit rejection outcome.

Failure returns no fallback state and no effective commit object.

## 27. Memory, determinism, and ARM64 suitability

Patch 0012 Core logic is model-free and deterministic.

No:

- network;
- filesystem;
- clock;
- randomness;
- provider API;
- GPU;
- NPU;
- background thread;
- polling;
- global mutable state.

ProductionState and causal commit objects use immutable collections. Implementation should structurally share unchanged immutable record objects where safe rather than deep-cloning the entire state graph on every commit.

State hashing and mutation application are linear in the bounded current E0 record/mutation surface. This is appropriate for ARM64 battery and memory behavior because commit work occurs only at an explicit accepted-Take boundary and creates no idle activity. This is architecture reasoning, not measured device power evidence.

## 28. Required implementation test families

Future implementation must prove at minimum:

1. canonical namespaces/contracts and exact version strings;
2. existing CommitId reused; no duplicate Commit ID type/allocator;
3. StateHash is deterministic canonical identity and not externally/randomly allocated;
4. ProductionState genesis from fixture preserves exact Scene/roster/opportunity/record/protection semantics;
5. genesis StateHash deterministic and changes for meaningful genesis/lock differences;
6. StateAuthoritySnapshot projection from genesis ProductionState equals existing fixture-derived semantics;
7. Take/source-state binding validates ContextPacketId/subject/scene/roster/opportunity and source snapshot equivalence;
8. Context full-content/source-state identity limitation is not falsely represented as proven;
9. Rejected/Alternate Takes cannot commit;
10. Accepted Take with zero mutations commits Performance history and changes causal StateHash;
11. Accepted Take with all Rejected consequences commits Performance history and no durable record effect;
12. Accepted Take with Approved Add creates exact new active record;
13. Approved Supersede deactivates old record and creates exact replacement;
14. Approved Deactivate deactivates exact record and creates no replacement;
15. mixed Approved/Rejected applies every Approved and no Rejected;
16. every Approved record-producing mutation requires exactly one materialized RecordId;
17. missing/extra/duplicate/colliding RecordIds fail closed;
18. inactive RecordIds cannot be reused;
19. stale StateHash fails with no Commit/result state;
20. same projected values after intervening causal history do not resurrect an old StateHash;
21. duplicate committed CommitId fails;
22. duplicate committed TakeId fails;
23. successful commit consumes Current Opportunity and does not establish the next one;
24. failed commit leaves Current Opportunity unchanged;
25. successful event associates CommitId with exact TakeId and exact E0Take;
26. ParentStateHash and ResultStateHash chain correctly;
27. deterministic replay from genesis + ordered commits reconstructs exact final StateHash/state;
28. replay rejects wrong-parent commit;
29. prior causal commits are immutable and no rewrite/delete API exists;
30. commit public surface exposes no persistence/database/provider/model/Scene-loop/Director-next-opportunity authority;
31. exception representation remains sanitized;
32. existing Patch 0003–0011 frozen regressions remain green;
33. full Core regression remains green;
34. Missing Raft Harness remains green;
35. generic smoke Harness remains green.

## 29. Explicit non-goals

Patch 0012 does not implement or freeze:

- global CommitId allocator/format;
- global RecordId allocator/format;
- durable database/event-store technology;
- filesystem persistence/recovery transaction;
- branch DAG identity;
- alternate-history promotion;
- retcon/rehearsal/canon-promotion UX;
- final Archive queries/search;
- full Access Control migration to evolved ProductionState;
- full Context Composer migration to evolved ProductionState;
- cryptographic proof of existing ContextPacket full-content derivation from StateHash;
- provider/prompt/assessor authentication;
- next Director opportunity selection/application;
- Scene-loop orchestration;
- Observation engine;
- World Resolver;
- final consequence-review UX;
- final Another Take / Take a Seat UX;
- Windows AI Foundry;
- NPU execution;
- WinUI;
- MSIX;
- WACK;
- Store certification.

## 30. Patch boundary summary

Completed deterministic spine after Patch 0012 is intended to become:

```text
Validated genesis / current Production projection
    -> Access Control
        -> Context Composer
            -> Performer Candidate
                -> Integrity
                    -> State Interpretation
                        -> deterministic State Authority
                            -> immutable E0 Take
                                -> strict source-state binding/freshness
                                    -> ATOMIC CAUSAL COMMIT
                                        -> append-only causal event
                                        -> new immutable ProductionState projection
                                            -> later Director continuation
```

Principal law:

> Patch 0012 may make history effective only by committing one immutable Accepted Take and every consequence retained as Approved under that Take, with no retained Rejected consequence, against the exact unchanged causal Production state to which the Take was bound. The successful operation returns one immutable causal event plus one reconstructible result projection; failure returns neither. Causal events are the conceptual source of truth, ProductionState is a deterministic current projection, and a history-sensitive StateHash prevents stale Takes from being repurposed. The original Take remains immutable, the source Current Opportunity is consumed only on success, and next-opportunity authority remains later.

## 31. Approval / implementation gate

This blueprint is architecture only.

Before implementation:

1. recursively adversarial-audit Proposal 0.1 against frozen Blueprint 0.1, approved Patches 0003–0011, current source/tests, engineering hygiene, causal-history source-of-truth law, State Authority ownership, Take immutability, source-state identity limitations, materialized RecordId semantics, replay, atomicity, invalid-state construction, memory/ARM64 suitability, E0 experiment isolation, and future persistence/branch separation;
2. restart the audit after every material correction;
3. require one complete final pass with zero material corrections and zero worthwhile architectural improvements;
4. obtain explicit user approval;
5. create an implementation handoff for a fresh project chat;
6. do not write Patch 0012 executable code in the architecture phase.
