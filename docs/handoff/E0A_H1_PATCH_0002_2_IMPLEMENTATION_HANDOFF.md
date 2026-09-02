# Ensemble — E0-A H1 Patch 0002.2 Implementation Handoff

Prepared: 2026-09-01
Status: READY FOR FRESH-CHAT IMPLEMENTATION

## Mission
Resume Ensemble at H1 Patch 0002.2 only: implement the approved Missing Raft fixture contract on top of the machine-validated generic E0 Fixture Dialect baseline.

Do not reopen approved product/experiment law merely to elaborate it. Do not enter Patch 0003 hashing or Patch 0004 Access Control until 0002.2 is separately implemented, reviewed, and machine-validated.

## Source-of-truth order
1. `CURRENT_STATE.md` — first read for current checkpoint and validation status.
2. `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md` — approved canonical GitHub implementation specification for the Missing Raft fixture subset.
3. `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` — implementation cleanliness law.
4. Current `main` source/tests/fixtures and machine evidence.
5. Frozen Ensemble Blueprint 0.1 / approved E0-A Prep law when a broader constitutional question genuinely arises.
6. Historical chat only for narrow ambiguity; never reconstruct authority from memory when GitHub contains it.

## Current validated executable baseline
H1 Patch 0002.1 + 0002.1a.

Latest machine-validated executable head:
`365537456f5890f3e2766abeb833f974c8fd7d8e`

Validated on the user's Windows ARM64 machine:
- .NET 9 native ARM64 Core/Harness build: PASS;
- Core tests: 31/31 PASS;
- generic fixture smoke `ensemble.e0.smoke@0.1.0`: PASS;
- process exit code: 0;
- provenance DAG correction: PASS.

Blueprint/checkpoint commits after that head are documentation-only unless new evidence says otherwise. Never promote this compiler/runtime evidence to NPU, WACK, Store, or broader runtime claims.

## Patch 0002.2 approval state
Blueprint 0.4 completed three adversarial review passes and was explicitly approved by the user. PR #5 was promoted to `main` as a blueprint-only fast-forward. No Patch 0002.2 executable source exists yet.

The approved contract intentionally keeps the generic fixture layer story-agnostic and adds one narrow experiment law:

`ValidatedFixture -> MissingRaftContract.Validate(ValidatedFixture)`

Do not create a second `MissingRaftFixture` domain representation.

## Implementation laws
- Preserve E0 Fixture Dialect v1 unless an actual implementation impossibility is demonstrated.
- Use one canonical source: `fixtures/missing-raft/missing-raft-0.1.0.json`.
- One `MissingRaftContract` owns stable IDs, expected sets, chronology, relationship targets, and frozen provenance rules.
- Provenance explains evidence/derivation. It never grants access.
- Character ownership remains structural through nesting.
- Generic validation remains story-agnostic.
- Do not parse English narrative meaning in C#.
- Source prose is reviewed against the approved contract in 0002.2 and hash-bound in Patch 0003.
- E0-D relationship omission later excludes the approved relationship-context ID surface from context; it does not mutate the fixture.
- Do not add a registry/plugin/factory system for one contract.
- Do not add generic tags, ACLs, scores, relationship meters, causal frameworks, or ablation metadata without evidence.

## Clean implementation sequence
Work incrementally and compare every slice against the validated baseline.

1. Confirm current `main` and read `CURRENT_STATE.md` + the approved 0002.2 contract.
2. Create a new implementation branch from current `main`, recommended name: `h1-patch-0002-2-implementation`.
3. Author the one canonical Missing Raft JSON fixture from the approved exact catalog and semantic meanings.
4. Perform a source-content/authority review before calling the JSON canonical.
5. Implement `MissingRaftContract` stable IDs/immutable expected sets and fail-closed `Validate(ValidatedFixture)`.
6. Add the smallest Harness family dispatch; unknown generic fixtures remain generically valid.
7. Extend the existing Core test project with mutation-style negative tests. Do not create malformed-fixture copy sprawl.
8. Run static correctness + consistency + authority + scope + simplicity + hygiene review against the prior validated baseline.
9. Correct any material issue before asking the user for machine validation.
10. Ask the user for one grouped ARM64 validation block at the bottom of the reply: build, full Core tests, canonical Missing Raft runtime, generic smoke regression, and exit codes as needed.
11. Record exact evidence and merge only when all defined gates pass.

## Mandatory source review
Before ARM64 validation, inspect the canonical JSON for:
- no omitted approved fact/state;
- no extra dramatic premise;
- no moral winner;
- no forced confession/accusation/revelation/reconciliation/solution;
- no hidden epistemic leak in prose;
- no relationship wording drift;
- raft failure remains unresolved;
- current did not release a correctly secured mooring;
- Wren observed Marlowe's later return, not the release;
- Voss's accidental-loss explanation remains plausible belief, not truth.

## Patch exit gate
Before merge:
- source-content review PASS;
- native Windows ARM64 Harness/Core build PASS with warnings-as-errors;
- complete Core test suite PASS;
- canonical Missing Raft Harness validation PASS as `ensemble.e0.missing-raft@0.1.0`;
- generic smoke fixture remains PASS through generic path;
- explicit Missing Raft validation rejects generic smoke;
- final baseline comparison finds no duplicate schema, second fixture representation, compatibility layer, prose parser, generic over-abstraction, provenance/access leak, or scope leakage.

## Explicit exclusions
Do not implement during 0002.2:
- ECJ-1 or SHA-256 enforcement;
- deterministic Access Control;
- Context Composer;
- ProductionState/persistence/causal commits;
- Performer/Director/Integrity Validator/Interpreter runtime orchestration;
- providers or AI;
- WinUI;
- Windows AI/NPU;
- packaging/WACK/Store work.

## Approval discipline
Do not ask for approval reflexively.

Request user approval when a decision materially changes:
- frozen/approved product or experiment law;
- authority/security/privacy boundaries;
- architecture or dependency direction;
- public/persistent compatibility contracts;
- a major scope expansion;
- a milestone freeze or convergence decision;
- a choice where two materially different product directions remain genuinely unresolved.

For routine implementation details, naming consistent with canon, local refactors, deletion of superseded code, regression-test additions, and obvious hygiene corrections: make the best cohesive decision, validate it, and report it compactly.

When approval is requested, state a confident recommendation first, then the meaningful tradeoff. Do not ask the user to choose merely because multiple technically possible options exist.

## Chat workflow
Keep chat density efficient.
- Prefer milestone updates over narration of every internal step.
- Group every user action request and every question for the user at the bottom of replies.
- Ask for complete compiler/runtime output only when machine evidence is needed.
- Keep routine questions out of the middle of implementation updates.
- If a material architectural problem is found, stop the affected implementation surface, explain root cause/recommendation, and request approval only if the correction crosses an approval boundary above.

## First fresh-chat action
Read `CURRENT_STATE.md` and `docs/blueprint/H1_PATCH_0002_2_MISSING_RAFT_CONTRACT.md` from GitHub, resolve current `main`, compare it with this handoff, then create the new implementation branch and begin the smallest coherent Patch 0002.2 slice.

Do not ask the user to restate established project state.
