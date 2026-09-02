# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- `docs/ENGINEERING_HYGIENE_CONSTITUTION.md` is project law for implementation quality and cleanup unless a stronger frozen specification explicitly overrides it.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source.
- Google Drive `Ensemble Project` is the design repository.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Validated baseline
H1 Patch 0002.1 — E0 Fixture Dialect v1 — is merged to `main` through PR #3.

Machine evidence for that baseline:
- native Windows ARM64 compiler gate: PASS;
- Core tests: 30/30 PASS;
- generic fixture runtime smoke: PASS, exit code `0`;
- detailed evidence: `docs/evidence/H1_PATCH_0002_1_ARM64_VALIDATION.md`.

## Active correction boundary
Branch: `h1-patch-0002-1a-provenance-dag`

Patch 0002.1a corrects one inherited invariant only:
- multi-record provenance cycles were not rejected by Patch 0002.1;
- `ValidatedFixture` construction now requires the complete provenance graph to be acyclic;
- cycle detection is iterative/topological, avoiding recursive stack-depth risk;
- one regression test proves a two-record provenance cycle is rejected.

No fixture schema, authority category, Missing Raft content, hashing, Access Control, Context Composer, persistence, provider/AI, UI, NPU, or Store surface changes in this correction.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- `dotnet test` on the target machine is test-execution authority for the tests actually exercised.
- Actual target-device execution: runtime authority for the behavior actually exercised.

## Current validation state
Patch 0002.1a has static review only until the target Windows ARM64 machine completes its build/test/smoke gate.

## Immediate next action
Validate Patch 0002.1a on the target machine. If clean, merge it before designing Patch 0002.2 so Missing Raft inherits the corrected provenance invariant.

## Project rule
Every patch is reviewed against the previous validated baseline: correctness -> consistency -> authority -> scope -> tests -> simplicity -> hygiene -> ARM64 suitability -> vision -> evidence. Git preserves history; the active source tree preserves only the best current architecture.
