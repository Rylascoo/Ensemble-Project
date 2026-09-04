# H1 Patch 0014 — Production Context v2 Reference Oracle

Status: STATIC INDEPENDENT REFERENCE ORACLE — NOT MACHINE VALIDATION
Date: 2026-09-03

## Purpose

Freeze independent deterministic reference identities for the approved H1 Patch 0014 Production-bound Context v2 contract before native Windows ARM64 validation.

This artifact is static evidence only. It does not establish compilation, test execution, Harness execution, runtime behavior, NPU behavior, packaging, WACK, or Store certification.

## Independence boundary

The new Context v2 digests were derived outside `DeterministicContextComposer` and `ContextPacketCanonicalizer` production code.

The independent reconstruction used:

- the canonical Missing Raft fixture semantics;
- frozen Patch 0004 Character-bounded Access category/ownership rules;
- frozen Patch 0005 Context property order and render-v1 format;
- approved Patch 0014 v2 changes only: schema/composition tokens plus `sourceStateHash` immediately after `compositionContract`;
- the independently established Production state hashes already frozen by the Patch 0013 reference-oracle chain.

The independent implementation used ordinary UTF-8 JSON string escaping matching the frozen canonical rules, ordinal record ordering, SHA-256, and no Patch 0014 production canonicalizer calls.

## Self-check 1 — frozen Context v1

Before computing any new v2 digest, the independent reconstruction reproduced the exact frozen VOSS Patch 0005 oracle:

```text
Structured bytes: 2569
StructuredContextHash:
bbb82aa9400ea76290f9e3a62dcea3cb922f5d5b5a5677216a8922a6472e274b

Rendered envelope bytes: 1905
RenderedContextHash:
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

These exactly match the existing Patch 0005 regression authority.

## Self-check 2 — inherited Production state chain

Patch 0014 reuses the previously independent Patch 0013 reference-oracle state chain, which itself first reproduced the Patch 0012 genesis and causal-commit hashes before deriving the Patch 0013 opportunity-transition hash.

Exact inherited state identities:

```text
Genesis StateHash:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Patch 0012 Pressure-add postcommit StateHash:
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30

Patch 0013 MARLOWE opportunity result StateHash:
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

The inherited independent derivation is recorded in:

`docs/evidence/H1_PATCH_0013_REFERENCE_ORACLE.md`

## Oracle A — Missing Raft genesis / VOSS

Source:

```text
Production StateHash:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Current Opportunity / Context subject:
VOSS
```

Exact v2 structured prefix semantics:

```text
schemaVersion = ensemble.e0.context.v2
compositionContract = ensemble.e0.context.production-bound.v1
sourceStateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
```

All permitted Character content is the exact frozen VOSS Patch 0004/0005 set. `recentPerformances` remains exactly empty.

Independent result:

```text
Structured bytes:
2655

StructuredContextHash:
27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

ContextPacketId:
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

Rendered envelope bytes:
1905

RenderedContextHash:
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

The rendered bytes/hash remain exactly equal to historical v1 because Patch 0014 changes no Character-visible rendering semantics.

## Oracle B — canonical Patch0012 -> Patch0013 evolved / MARLOWE

This uses the exact prior reference chain, not the unrelated empty-mutation helper scenario.

Patch 0012 accepted source:

```text
TakeId = TAKE-PATCH-0012-ORACLE
CommitId = COMMIT-PATCH-0012-ORACLE
Approved mutation = add Pressure "Pressure increases."
Materialized RecordId = PRESSURE-PATCH-0012-ORACLE
```

Patch 0012 postcommit:

```text
StateHash =
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
CurrentOpportunityCharacterId = null
```

Patch 0013 then establishes:

```text
SelectedCharacterId = MARLOWE
Result StateHash =
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

The Production-bound MARLOWE Access projection includes the inherited permitted Missing Raft state plus both active public pressures, ordinally:

```text
PRESSURE-ISOLATION
PRESSURE-PATCH-0012-ORACLE
```

`PRESSURE-PATCH-0012-ORACLE` text is exactly:

```text
Pressure increases.
```

Independent v2 result:

```text
Structured bytes:
3456

StructuredContextHash:
9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00

ContextPacketId:
CTX:9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00

Rendered envelope bytes:
2389

RenderedContextHash:
9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

No recent Performance or CharacterClaim content is present.

## Implementation regression

`tests/Ensemble.E0.Core.Tests/Continuity/Patch0014ReferenceOracleTests.cs` pins both new v2 identities while simultaneously reasserting the inherited genesis, Patch 0012 postcommit, and Patch 0013 opportunity StateHashes on the canonical source chain.

Native validation must confirm those tests pass on the user's Windows ARM64 development machine before these new v2 digests become machine-established Patch 0014 authority.
