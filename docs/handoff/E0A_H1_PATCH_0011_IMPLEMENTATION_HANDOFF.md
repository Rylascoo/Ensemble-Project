# Ensemble — E0-A H1 Patch 0011 Implementation Handoff

Prepared: 2026-09-03
Status: READY FOR FRESH-CHAT IMPLEMENTATION

## Mission

Resume Ensemble at H1 Patch 0011 only: implement the explicitly approved E0 Take Semantics Proposal 0.15 on top of the machine-validated Patch 0010 deterministic State Authority baseline.

Do not reopen the approved Take architecture merely to elaborate it. Do not enter Patch 0012 atomic causal commit, ProductionState, persistence, provider execution, Scene loop, UI, Windows AI/NPU, packaging, WACK, or Store implementation during Patch 0011.

## Source-of-truth order

1. `CURRENT_STATE.md` — first read for the authoritative checkpoint and validation level.
2. `docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md` — exact approved Proposal 0.15 implementation specification.
3. `docs/evidence/H1_PATCH_0011_BLUEPRINT_APPROVAL.md` — explicit approval record and frozen contract summary.
4. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation cleanliness law.
5. Current `main` source/tests plus Patch 0010 machine evidence.
6. Relevant Patch 0006–0010 contracts/source only when needed to implement or verify a specific upstream boundary.
7. Frozen Blueprint 0.1 / approved H1 deterministic-spine authority only when a broader constitutional question genuinely arises.
8. Google Drive only for supporting product/design context when materially relevant; GitHub remains engineering authority.
9. Historical chat only for narrow ambiguity; never reconstruct approved state from memory when GitHub contains it.

## Approved architecture state

Architecture: FROZEN FOR PATCH 0011 IMPLEMENTATION

Blueprint: APPROVED — Proposal 0.15

Exact audited proposal head:
`8e939c8efe100473f3409185e9de5fde65bcc8b1`

Recursive architecture audit: COMPLETE — final full pass found zero material corrections and zero worthwhile architectural improvements.

Implementation: NOT STARTED

## Current machine-validated executable baseline

H1 Patch 0010 deterministic State Authority.

Exact machine-tested executable/test head:
`b07c8e161d221bc9bf2e57802de0aae7b542ce5a`

Validated on the user's native Windows ARM64 development machine:

- Core/Harness native ARM64 build: PASS;
- Harness target: `net9.0\win-arm64`;
- full Core tests: `392/392` PASS;
- Missing Raft Harness: PASS;
- generic smoke Harness: PASS.

Detailed authority:
`docs/evidence/H1_PATCH_0010_ARM64_VALIDATION.md`

Do not promote this evidence to Patch 0011 until the user's machine actually compiles/tests Patch 0011.

## Patch 0011 canonical boundary

The approved ordering is:

```text
CandidatePerformance
    -> Integrity evaluation
        -> State Interpretation
            -> deterministic State Authority
                -> E0 Take
                    -> future atomic causal commit
```

There is no separate provisional Take object before State Interpreter / State Authority in E0.

Patch 0011 defines only immutable accepted/rejected/alternate Take semantics and association verification. It applies no authoritative state and writes no Production history.

## Required canonical public semantics

Implement under:

```text
Ensemble.E0.Core.Take
```

Canonical contract holder:

```csharp
public static class E0TakeContracts
{
    public const string ContractVersion = "ensemble.e0.take.v1";
}
```

Required semantic types:

```text
E0TakeDisposition
- Unspecified = 0
- Accepted = 1
- Rejected = 2
- Alternate = 3

public sealed class E0Take
public sealed class E0TakeException
```

`E0Take` must have a private constructor and `E0Take.Bind(...)` must be its sole construction/reconciliation path.

`E0TakeException` is publicly catchable but has no public constructors. Internal construction is assembly-visible; do not claim C# namespace-level constructor enforcement that the language does not provide.

Physical `.cs` grouping is implementation hygiene, not frozen architecture. Use the smallest coherent layout consistent with existing Core subsystem patterns.

## Required `E0Take` retained package

The immutable Take retains exactly:

- `ContractVersion`;
- supplied initialized `TakeId`;
- explicit valid `E0TakeDisposition`;
- exact immutable `CandidatePerformance` reference;
- exact immutable `StateInterpretationProposal` reference;
- fresh canonical `StateAuthorityEvaluation` produced by Patch 0010 replay.

It must not retain:

- `ContextPacket`;
- `IntegrityValidationEvaluation`;
- `StateInterpretationSource`;
- RunId;
- provider/model identity;
- confidence/rationale/chain-of-thought;
- CommitId;
- ProductionState;
- StateHash;
- committed/history/effective flags.

Do not introduce defensive deep-clone layers: the retained upstream semantic graphs already use get-only properties and immutable collections. Reuse those immutable objects rather than adding duplicate representations or allocations.

## Sole rich binding boundary

Implement the approved conceptual signature:

```text
E0Take.Bind(
    TakeId takeId,
    ContextPacket sourceContext,
    CandidatePerformance performance,
    IntegrityValidationEvaluation integrityEvaluation,
    StateInterpretationProposal interpretationProposal,
    StateAuthorityEvaluation authorityEvaluation,
    E0TakeDisposition disposition)
    -> E0Take
```

The exact C# formatting/parameter naming may follow repository conventions, but do not alter the semantic inputs or split the authority across weaker factories.

## Canonical replay law

`E0Take.Bind` must reuse existing upstream deterministic contracts rather than reimplement their semantics.

Required flow:

1. validate initialized TakeId;
2. reject `Unspecified` or undefined `E0TakeDisposition`;
3. require a structurally readable/current supplied Patch 0010 evaluation Trace/Input/Policy/ReviewSet;
4. re-bind the exact source association with:

```text
StateInterpretationSource.Bind(
    sourceContext,
    performance,
    integrityEvaluation)
```

5. re-bind a fresh Patch 0010 input using the exact Snapshot carried by the supplied evaluation Trace and the exact supplied InterpretationProposal;
6. re-evaluate with `DeterministicStateAuthority.Evaluate` using the exact Policy and ReviewSet carried by the supplied Trace;
7. require fresh status `Complete` and no fresh `RequiresReview` decision;
8. retain the exact Performance, exact InterpretationProposal, and fresh replayed StateAuthorityEvaluation.

Do not independently trust/copy the supplied evaluation's caller-visible Status/Decisions. The fresh deterministic replay is canonical for the Take.

Do not add a second State Authority implementation or bespoke full evaluation-equality algorithm.

## Authority separation law

Do not collapse these three authorities:

```text
Integrity Validator
    -> whether Candidate may progress

State Authority
    -> which proposed consequences are Approved / Rejected / RequiresReview

Take disposition
    -> whether the fully evaluated package is Accepted / Rejected / Alternate
```

State Authority `Rejected` consequences do not reject the Performance and do not infer Take disposition.

The following must be valid after terminal Complete authority evaluation:

- Accepted + zero decisions;
- Accepted + all Approved;
- Accepted + mixed Approved/Rejected;
- Accepted + all Rejected;
- Rejected + any terminal Approved/Rejected set;
- Alternate + any terminal Approved/Rejected set.

`RequiresReview` never binds a Take.

## Accepted / Rejected / Alternate laws

`Accepted` means selected for later atomic commit eligibility only. It does not mean historical/effective/committed.

`Rejected` and `Alternate` preserve exact evaluated immutable packages as non-effective E0 provenance. They apply no Approved consequence, history, Director progression, effective Candidate control, or Current Opportunity change.

All dispositions are immutable. No mutation API may convert one disposition into another.

## Take identity law

Reuse the existing canonical `TakeId` strong type.

Do not add:

- another Take ID type;
- CandidateId;
- TakeId allocator;
- RunId+ordinal format;
- random/time-derived ID generation;
- content-derived TakeId;
- Take content hash merely to duplicate State Authority semantics.

Core validates only that supplied TakeId is initialized.

Per-Run uniqueness belongs to later orchestration/provenance; Core Patch 0011 is stateless and cannot enforce uniqueness across separate Bind calls.

## Source-state identity limit

Do not invent a ProductionState/StateHash/common-state identity between `ContextPacket` and `StateAuthoritySnapshot`.

Current Patch 0010 executable validation uses fixture-derived initial snapshots. Patch 0011 can structurally re-bind Context/Candidate/Integrity/Proposal and replay State Authority, but it cannot cryptographically prove the Context and snapshot came from the exact same evolved Production state because no such state identity exists yet.

Do not hide this boundary with an alias/hash/fixture-derived fake StateHash.

Later orchestration must preserve fixture/current-authority provenance, and Patch 0012/later state authority owns real freshness/current-state identity.

## Future freshness immutability guard

Do not implement stale-state application in Patch 0011, but preserve this invariant in the Take representation:

- later freshness checking may accept/reject an immutable Take;
- it may not silently substitute a different Approved/Rejected consequence package under the same Take identity;
- a successful commit under this Take can eventually apply only the retained Approved set, every retained Approved consequence, and no retained Rejected consequence;
- if current state would require a different consequence set, this Take cannot simply be rewritten.

Do not create Patch 0012 machinery merely to test this future rule.

## Reference E0 disposition policy

Core `E0Take.Bind` validates an explicit disposition; it does not choose reference policy.

Later E0 reference orchestration law is:

```text
Take-bindable package
+ no separately labeled explicit Take-disposition intervention
    -> Accepted
```

Rejected/Alternate reference deviations must be explicitly labeled and attributable. They are never model-authored or inferred from State Authority decisions.

Do not add a second Core policy class/service for this mapping in Patch 0011.

## Exception boundary

Expected public contract failures crossing `E0Take.Bind` become sanitized `E0TakeException`.

Normalize expected:

- `StateInterpretationException` from canonical source re-binding;
- `StateAuthorityException` from canonical input binding/evaluation;
- Take-owned `InvalidOperationException` encountered only while checking an uninitialized supplied strong ID/property.

For upstream StateInterpretation/StateAuthority domain exceptions, retain the exact sanitized upstream domain exception as `InnerException`; do not concatenate upstream text into the new public Take message.

For Take-owned uninitialized-ID runtime exceptions, normalize without retaining the runtime exception as inner evidence.

Do not copy exception `Data`.

No Candidate text, Context prose, mutation text, provider content, credentials, arbitrary user text, or unknown payload snippets may leak through `Message`, retained inner chain/data, or `ToString()` for exceptions emitted by `E0Take.Bind`.

Do not catch arbitrary unexpected runtime/programming failures and relabel them as Take rejection or ordinary contract failure.

## Required implementation tests

Implement the complete approved test matrix in Section 30 of `H1_PATCH_0011_TAKE_SEMANTICS.md`. Do not silently narrow it.

Important families include:

- namespace/contract exactness;
- existing TakeId reuse;
- default/undefined disposition failure;
- private Take construction / sole Bind path;
- no public E0TakeException constructors;
- exact retained Performance/Proposal references;
- fresh State Authority replay canonicality;
- malformed/mismatched upstream association failure;
- Integrity Reject / RequestAnotherTake cannot bind;
- fresh ReviewRequired / RequiresReview cannot bind;
- all terminal consequence-set combinations across Accepted/Rejected/Alternate;
- no State/history/Director/effective-control side effects;
- no CommitId/StateHash/ProductionState/persistence/public application surface;
- no retained Context/Integrity/Run/provider/model/rationale/chain-of-thought surface;
- distinct TakeIds may carry semantically identical content;
- deterministic repeated Bind semantics;
- exception normalization/sanitization;
- fixed Missing Raft regression identities unchanged;
- full Core regression;
- Missing Raft Harness regression;
- generic smoke Harness regression.

Use existing reflection-based contract-audit patterns when malformed internal construction must be tested. Do not open weaker production constructors merely to make negative tests easy.

## Patch-first implementation shape

Start from the smallest affected surface.

Expected new subsystem surface should normally be limited to:

```text
src/Ensemble.E0.Core/Take/...
tests/Ensemble.E0.Core.Tests/Take/...
```

Do not edit upstream Access, Context, Performer, Director, Integrity, State Interpreter, State Authority, Fixture, Harness, or project configuration merely for convenience unless a concrete compiler/contract defect proves a smallest necessary correction.

If a prior approved implementation must change, identify the exact incompatibility and treat any semantic change as an architecture approval boundary rather than silently patching upstream law.

## Clean implementation sequence

1. Read `CURRENT_STATE.md`, the approved Patch 0011 blueprint, approval record, hygiene constitution, and this handoff.
2. Resolve current `main` and confirm the implementation branch begins exactly from the promoted approved checkpoint.
3. Confirm no newer Patch 0011 implementation branch/PR already exists before creating one.
4. Create/use `h1-patch-0011-take-semantics-implementation` from current `main`.
5. Implement the minimal immutable Take types and binding/replay boundary.
6. Add the complete approved Patch 0011 tests using existing test conventions.
7. Run recursive static audits: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence.
8. Restart the audit after every material correction until one full pass finds no further material corrections or worthwhile improvements.
9. Do not claim compilation from static reasoning.
10. Ask the user for grouped native Windows ARM64 validation only when the source/test patch is statically ready.
11. For compiler/runtime feedback, patch the smallest affected surface and rerun relevant tests; do not regenerate archives or redesign the contract.
12. After successful machine validation, record exact evidence and only then promote Patch 0011 implementation status.

## Machine validation gate

When statically ready, request one grouped target-machine block at the bottom of the implementation reply. At minimum:

```powershell
git status --short
git rev-parse HEAD

dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug

dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\missing-raft\missing-raft-0.1.0.json

dotnet run --project .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug --no-build -- .\fixtures\smoke\smoke-0.1.0.json
```

Preserve complete build/test output when compiler evidence is needed. If build fails, Harness `--no-build` results after that failure are not Patch 0011 evidence because they may execute older binaries.

Do not claim NPU, Windows AI, WACK, or Store validation from these commands.

## Exit gate

Patch 0011 is not complete until:

- implementation matches exact approved Proposal 0.15 semantics;
- no duplicate Take identity/authority representation exists;
- full Core regression is green on the user's native Windows ARM64 machine;
- native ARM64 Core/Harness build is green with warnings-as-errors;
- Missing Raft Harness regression remains green after the successful build;
- generic smoke Harness regression remains green after the successful build;
- final hygiene/authority/scope audit finds no material defect;
- exact machine-tested commit is recorded;
- validation evidence clearly distinguishes compiler/test/runtime authority from unverified NPU/package/Store claims.

## Explicit exclusions

Do not implement during Patch 0011:

- ProductionState;
- StateHash;
- source-Context/snapshot authoritative common-state identity;
- new RecordId allocation;
- mutation application;
- CommitId;
- atomic causal commit;
- stale-state runtime/freshness application;
- persistence/recovery;
- branch DAG;
- rehearsal/retcon/alternate promotion;
- final Another Take UX;
- final Take a Seat UX;
- final ODR-19 consequence-acceptance UX;
- final ODR-20 branching/canon-promotion UX;
- provider/model execution;
- State Interpreter request composition;
- Scene loop;
- World Resolver;
- Observation engine;
- Windows AI Foundry;
- NPU execution;
- WinUI;
- MSIX;
- WACK;
- Store certification.

## Approval discipline

The Patch 0011 architecture is approved. Do not ask the user to reconfirm routine implementation choices that are already determined by the contract.

Request approval only if implementation evidence reveals a material need to change:

- frozen/approved Patch 0011 semantics;
- authority/security/privacy boundaries;
- public/persistent compatibility contracts;
- dependency direction or architecture;
- major scope;
- a genuine unresolved product choice.

For naming/file grouping consistent with the approved namespace, local refactors, negative tests, obvious defensive checks, and deletion of superseded implementation code inside the affected boundary: make the cohesive patch-first decision, validate it, and report it.

## Chat workflow

Keep implementation chat dense and evidence-driven.

- Root cause -> exact correction -> verification -> next action for build failures.
- Prefer milestone updates over narration.
- Do not repeatedly summarize locked project features.
- Do not ask the user to paste information already available through GitHub/Drive.
- Treat the user's native ARM64 build/test output as authoritative for that machine.
- Update `CURRENT_STATE.md` only when a meaningful checkpoint changes.
- Do not create full ZIP/manifests after ordinary compiler corrections.

## First fresh-chat action

Read `CURRENT_STATE.md`, resolve current `main`, then read:

1. `docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md`;
2. `docs/evidence/H1_PATCH_0011_BLUEPRINT_APPROVAL.md`;
3. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`;
4. this handoff;
5. only the exact upstream Patch 0006–0010 source/contracts needed for implementation.

Confirm the approved implementation branch does not already exist, create/use `h1-patch-0011-take-semantics-implementation` from the promoted `main`, and begin the smallest coherent Patch 0011 source/test slice.

Do not ask the user to restate established project state.
