# H1 Patch 0014 — Static Implementation Audit

Status: ADVISORY STATIC IMPLEMENTATION EVIDENCE — ZERO-MATERIAL-CHANGE PASS ACHIEVED; NATIVE VALIDATION REQUIRED
Date: 2026-09-03

Patch: `H1 Patch 0014 — E0 Production Context Continuity`
Approved architecture: Proposal `0.10`
Implementation branch: `h1-patch-0014-production-context-continuity-implementation`

## Exact audited source/test head

`5dea43677f1738abc8d135078d04d8f495b990e3`

Implementation base / approved handoff checkpoint:

`2136b8b9a1781cd9d28a14a67f33296cd863c6ef`

Promoted parent `main` checkpoint remains:

`e06668a2307433bf99b0501dc38a701db392c633`

## Evidence level

This is static/adversarial implementation evidence only.

It does **not** establish:

- C# compilation;
- Core-test discovery or execution;
- native Windows ARM64 behavior;
- Harness build or execution;
- NPU/Windows AI execution;
- packaging/WACK/Store behavior.

The available execution container does not contain the .NET SDK, so no non-authoritative local compile was possible. Compiler/test authority must come from the user's native Windows ARM64 machine.

## Implemented source boundary

Patch 0014 changes only the approved deterministic seam.

Modified Core source:

```text
src/Ensemble.E0.Core/Access/CharacterAccessModels.cs
src/Ensemble.E0.Core/Access/CharacterBoundedAccessControl.cs
src/Ensemble.E0.Core/Context/ContextModels.cs
src/Ensemble.E0.Core/Context/ContextPacketCanonicalizer.cs
src/Ensemble.E0.Core/Context/DeterministicContextComposer.cs
src/Ensemble.E0.Core/CausalCommit/CausalCommitModels.cs
```

Added Core source:

```text
src/Ensemble.E0.Core/Continuity/E0ProductionContextContinuity.cs
```

No Production transition/canonicalizer source, Opportunity source, Performer, Integrity, Interpreter, or StateAuthority source was modified.

## Focused test/evidence boundary

Added Patch 0014 tests:

```text
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014TestSupport.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionAccessContinuityTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionAccessStructuralTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextContinuityTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/ProductionContextBindingTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ContractAuditTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014DeterminismTests.cs
tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ReferenceOracleTests.cs
```

Narrowly evolved inherited temporary non-scope assertions only:

```text
tests/Ensemble.E0.Core.Tests/CausalCommit/Patch0012ContractAuditTests.cs
tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ContractAuditTests.cs
```

Those two historical tests previously asserted that Production-backed Access did not yet exist. Patch 0014 is the separately approved architecture that supersedes exactly that temporary condition. Their enduring Patch0012/Patch0013 public-surface, dependency, hardware-exclusion, and authority assertions remain.

Added independent oracle evidence:

```text
docs/evidence/H1_PATCH_0014_REFERENCE_ORACLE.md
```

Historical evidence files were not modified.

## Implemented authority laws

The implementation now enforces:

1. Production-backed Character Access with exact `SourceStateHash` association.
2. Every retained Production record receives one deterministic AccessDecision.
3. Only Active currently authorized records enter Character-facing projection.
4. Inactive records are denied with `InactiveRecordExcluded` before domain/ownership policy.
5. Active CharacterClaim is denied for every Character with `CharacterClaimDisclosureDeferred`.
6. Production truth domains remain denied.
7. Other Characters' private state and outbound Relationships remain denied.
8. Subject-owned current inherited state and Relationships remain permitted.
9. Production Character/roster/record/domain/subtype/text/relationship structure is defensively validated without performing a full provenance-DAG rescan.
10. Historical fixture Access remains available and has null `SourceStateHash`.
11. Historical Context v1 remains the sole public composer path and rejects Production-backed projections.
12. Production-bound Context v2 is internal composition only.
13. Context v2 adds only schema/composition versioning plus structured `sourceStateHash`.
14. Existing `ensemble.e0.context.render.v1` rendering remains unchanged.
15. `recentPerformances` remains exactly empty and `RecentPerformanceText` remains exactly empty.
16. No Claims or recent-Performance public Context surface was added.
17. `SourceStateHash` never enters Character-facing rendering.
18. Context canonicalization fails closed on unsupported/hybrid v1/v2 shape.
19. Continuity preserves both Access evaluation and Context evaluation from one checkpoint.
20. E0TakeStateBinding does not trust StateHash metadata alone: it freshly recomposes the exact expected Production-derived Context before binding.
21. Evolved state requires exact Production-bound v2 Context.
22. Historical v1 binds only for exact genesis after fresh Production Access + frozen-v1 recomposition.
23. Exact source proof compares canonical structured bytes, canonical rendered bytes, ContextPacketId, structured/rendered stored hashes, SourceStateHash, and existing Scene/subject/opportunity/roster association.
24. Production/Opportunity mutation authority is unchanged.

## Independent reference oracle

The Patch 0014 reference derivation first reproduced frozen Patch 0005 VOSS v1 authority:

```text
v1 structured bytes = 2569
v1 StructuredContextHash =
bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b

rendered bytes = 1905
RenderedContextHash =
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

It then uses the independently established inherited Production chain:

```text
Genesis StateHash:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Patch0012 Pressure-add postcommit StateHash:
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30

Patch0013 MARLOWE opportunity StateHash:
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

New independently derived v2 identities pinned by tests:

### Genesis / VOSS

```text
Structured bytes = 2655
StructuredContextHash =
27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
ContextPacketId =
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565
RenderedContextHash = frozen v1 ce99a0c5...
Rendered bytes = 1905
```

### Canonical Patch0012 -> Patch0013 evolved / MARLOWE

The evolved oracle includes both active public pressures, including exact committed `PRESSURE-PATCH-0012-ORACLE = "Pressure increases."`.

```text
Structured bytes = 3456
StructuredContextHash =
9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
ContextPacketId =
CTX:9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00
Rendered bytes = 2389
RenderedContextHash =
9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

These remain static oracle candidates until native tests pass.

## New test count expectation

The inherited native Patch 0013 suite contains 496 tests.

Patch 0014 adds 42 non-DataRow test methods and removes no tests.

Therefore the expected native discovery count is:

```text
538
```

This is an expectation only until `dotnet test` prints the actual count on the user's machine.

## Material corrections found during implementation audit

The recursive implementation audit found and corrected the following before closure:

1. a new Access-equivalence test used HashSet enumeration in a signature; corrected to explicit ordinal ordering;
2. Production Access initially checked initialized strong IDs but did not defensively revalidate canonical ID form; hardened CharacterId/RecordId and source Scene/StateHash identity validation;
3. malformed-state coverage was expanded using the already-proven reflection-only invariant-test pattern, without introducing production bypass APIs;
4. exact-binding tests were expanded from structured-content tampering to rendered-byte tampering, stored-hash tampering, missing SourceStateHash, and unsupported schema/composition combinations;
5. a hidden Production-authority test now freezes the intended distinction where two states can render identically yet receive different v2 structured identities through SourceStateHash;
6. culture-invariance was added under `ar-SA`;
7. evolved subject-owned Character state now explicitly tests Belief Supersede: inactive old record denied, new active record permitted;
8. the independent evolved Context oracle was corrected before freezing when audit detected an invalid intermediate derivation that paired the Patch0013 `dc7e...` state hash with content omitting its causal Pressure-add record. The final frozen oracle uses the exact canonical Pressure-add chain and includes the new pressure.

Every material correction or worthwhile test improvement restarted the audit from correctness.

## Final recursive audit

Order:

```text
correctness
-> consistency
-> authority
-> disclosure/privacy
-> dependency direction
-> canonical/version compatibility
-> tests
-> simplicity
-> hygiene
-> ARM64/battery suitability
-> project vision
-> evidence
```

Final static result at `5dea43677f1738abc8d135078d04d8f495b990e3`:

- zero material correctness corrections;
- zero material consistency corrections;
- zero authority corrections;
- zero disclosure/privacy corrections;
- zero dependency-direction corrections;
- zero canonical/version corrections;
- zero worthwhile test improvements;
- zero worthwhile simplifications;
- zero material hygiene corrections;
- zero material ARM64/battery corrections;
- zero project-vision inconsistencies;
- zero evidence-boundary corrections.

## ARM64 / battery conclusion

Patch 0014 adds only on-demand deterministic CPU work.

No background/idle task, network, provider, GPU, NPU, clock, randomness, or platform service was added.

Approximate approved scaling remains:

```text
R = retained Production records
A = active permitted records
B = permitted canonical/rendered bytes

Production Access                ~ O(R)
Context composition              ~ O(A log A + B)
Continuity Compose               ~ O(R + A log A + B)
Take-binding fresh source proof  ~ O(R + A log A + B) + existing StateAuthority validation
```

No retail battery/performance claim is made without native device profiling.

## Native gate

The next authority level is the user's native Windows ARM64 machine.

Required initial commands:

```powershell
git pull --ff-only
git rev-parse HEAD

dotnet test .\tests\Ensemble.E0.Core.Tests\Ensemble.E0.Core.Tests.csproj -c Debug
```

If the full Core suite passes at the exact branch head, proceed to native Harness build and both existing fixture executions at that same head.

Do not promote or update `CURRENT_STATE.md` before that gate succeeds.
