# H1 Patch 0002.1a — Provenance DAG Correction

Status: targeted correction
Parent baseline: H1 Patch 0002.1 — E0 Fixture Dialect v1

## Reason
The validated 0002.1 implementation rejects unknown provenance references and direct self-reference, but a longer cycle such as `A -> B -> A` can still pass. Provenance represents causal/evidentiary derivation, so cyclic provenance is invalid.

## Correction
- Require the complete fixture provenance graph to be acyclic.
- Enforce the invariant at `ValidatedFixture` construction so future internal construction paths inherit it.
- Use iterative topological validation rather than recursive DFS to avoid stack-depth risk from deeply chained fixture data.
- Add one regression test proving a two-record provenance cycle is rejected.

## Scope exclusions
No fixture schema change, Missing Raft content, authority-category change, hashing, Access Control, Context Composer, persistence, provider/AI, UI, NPU, or Store work.

## Exit gate
On the target Windows ARM64 machine:
1. Harness build succeeds.
2. Core tests succeed with the new cycle regression.
3. Generic smoke fixture still validates successfully.
