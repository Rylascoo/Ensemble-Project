# H1 Patch 0015 — Independent Live Reference Oracle

Status: STATIC REFERENCE ORACLE — NOT YET MACHINE VALIDATED
Date: 2026-09-04

## Purpose

Freeze the independently reconstructed deterministic reference lineage for approved `H1 Patch 0015 — E0 Accepted Performance History + Context Continuity`, Proposal 0.15, before native Windows ARM64 compiler/test validation.

This artifact is static evidence only. It does not establish compilation, test execution, runtime behavior, ARM64 behavior, Harness behavior, NPU behavior, packaging, WACK, or Store certification.

## Exact approved live oracle inputs

The Patch 0015 oracle starts a separate live branch from exact Missing Raft genesis and deliberately reuses the historical Patch 0012 oracle IDs/mutation inputs to isolate the identity effect of changing the source Context from historical fixture-derived v1 to Production-bound v2.

```text
Genesis StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Source Context
VOSS Production-bound v2
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

Candidate VisibleText
No.

Candidate control
addressedCharacterIds = []
nominatedCharacterId = null

TakeId
TAKE-PATCH-0012-ORACLE

CommitId
COMMIT-PATCH-0012-ORACLE

Approved mutation
Pressure Add
text = Pressure increases.
supportingRecordIds = []

State Authority policy
autoApproveDomains = [ Pressure ]
review choices = []

Materialized RecordId
PRESSURE-PATCH-0012-ORACLE
```

## Independent reconstruction method

The reference derivation did not call Patch 0015 production canonicalizers/composers to obtain the expected values.

It independently reconstructed the frozen canonical objects from:

- the canonical Missing Raft fixture;
- frozen Production genesis mapping and property order;
- frozen Production record domain/lifecycle/protection tokens;
- frozen causal-commit payload/property order;
- frozen Opportunity payload/property order;
- frozen Context v2/v3 structured property order;
- frozen Context rendering rules;
- Proposal 0.15 exact live inputs;
- Proposal 0.15 independently established v2-source Candidate/Proposal content hashes.

Canonical JSON was encoded as exact UTF-8 with no insignificant whitespace and SHA-256 was applied to the exact byte sequences required by the established contracts.

## Mandatory inherited self-checks

Before accepting any new Patch 0015 digest, the same reconstruction reproduced the previously frozen references exactly.

### Missing Raft Production genesis

```text
StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
```

### Historical Patch 0012 v1-source causal commit

```text
StateHash
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
```

### Historical Patch 0013 Opportunity transition

```text
SelectedCharacterId
MARLOWE

StateHash
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

### Historical Patch 0014 evolved MARLOWE Production-bound v2 Context

```text
structured bytes
3456

StructuredContextHash
9a3cf788da23eece7497f16f2ff9ab62a588081496f0efde8035300cd0bb6f00

rendered bytes
2389

RenderedContextHash
9925327709d4a99697d4b65e5ad870e6e3ff5c5d573e25c61814e48d77163a2d
```

Only after all four inherited references matched exactly were Patch 0015 values accepted.

## Patch 0015 inherited genesis v2 source Context

The exact genesis Production-bound v2 Context remains the Patch 0014 reference unchanged:

```text
subject
VOSS

Production StateHash
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

structured bytes
2655

StructuredContextHash
27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

ContextPacketId
CTX:27f20b78754132777adcc392199a2490c210fa99ad44150e527ceb4c3f22e565

rendered bytes
1905

RenderedContextHash
ce99a0c5e525276c17b84ad86a21c335028d1a01791f27c223bae4e5edd7cf88
```

## Independently established v2-source semantic intermediates

Proposal 0.15 already froze these independent intermediates for the exact source Context and Candidate/Proposal semantics:

```text
CandidateContentHash
6ce2a98dc6208400126feba1a356910376d40c6155843764ecf673b8b9441ec1

ProposalContentHash
ac0f7a91c855b99fdb6f1b9415ff150352e8e3f79268e666d38531323b7d16c3
```

The Candidate/proposal semantic inputs are otherwise the same as the historical Patch 0012 oracle. Their hashes differ because the semantic identity chain is bound to the Production-backed v2 ContextPacketId.

## New live postcommit StateHash

The postcommit Production projection is byte-for-byte the historical Patch 0012 oracle postcommit projection:

- the current opportunity becomes `null`;
- `PRESSURE-PATCH-0012-ORACLE` is Active Pressure text `Pressure increases.`;
- all other Production fields/records remain unchanged.

The causal payload differs only through the v2-bound semantic identity chain while retaining the exact frozen IDs and authority/materialization semantics.

Exact Patch 0015 live postcommit StateHash:

```text
a7e6e1d5e386b2f6b459f8432b85ee7b14c3b25dbfec6e228240f98e523fe96c
```

## New live Opportunity StateHash

The established Opportunity remains the inherited least-intervention result:

```text
SelectedCharacterId
MARLOWE
```

The result Production projection is byte-for-byte the historical Patch 0013 oracle opportunity-bearing projection. Only the parent causal lineage hash differs.

Exact Patch 0015 live Opportunity StateHash:

```text
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151
```

Expected OpportunityHistory:

```text
[ VOSS, MARLOWE ]
```

Expected `LastOpportunityStateHash`:

```text
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151
```

## First nonempty accepted-history Context v3 oracle

At the new MARLOWE opportunity, accepted Performance history is exactly:

```text
[
  SourceCharacterId = VOSS
  VisibleText = No.
]
```

The durable Production pressure and accepted historical Performance remain separate semantic layers.

### Structured v3

Exact root order remains the approved v2 order with `recentPerformances` final and exact one-item shape:

```json
{"sourceCharacterId":"VOSS","visibleText":"No."}
```

Reference:

```text
schemaVersion
ensemble.e0.context.v3

compositionContract
ensemble.e0.context.production-bound.accepted-history.v1

SourceStateHash
e935dc6c7a3359304d8d6732d07e7fdda401a45094264cc944e6fa7a925ef151

structured bytes
3521

StructuredContextHash
ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f

ContextPacketId
CTX:ebf3263c79fe4787c6827aa4331bc61f0e76d5907c2e200f0e020e7540b54f2f
```

### Render v2

Trusted current-state rendering remains byte-for-byte the inherited Patch 0014 evolved MARLOWE trusted-state text. The new recent-Performance layer is exactly:

```text
[RECENT PERFORMANCES]
Dr. Voss:
[PERFORMANCE]
- No.
```

Reference:

```text
renderingContract
ensemble.e0.context.render.v2

rendered bytes
2443

RenderedContextHash
668c632ebdb4e4a2de838cbc5ae49b17005984880345ed28ec0c6eaa2bfcef16
```

## Regression test

`tests/Ensemble.E0.Core.Tests/Continuity/Patch0015ReferenceOracleTests.cs` pins:

- inherited genesis v2 source Context bytes/hashes;
- Proposal 0.15 v2-source Candidate/Proposal hashes;
- new live postcommit StateHash;
- new live Opportunity StateHash and routing history;
- first nonempty MARLOWE Context v3 structured/rendered byte lengths and hashes;
- exact recent Performance semantic/rendered content.

Native Windows ARM64 validation must pass this test before any Patch 0015 oracle value is promoted from static reference evidence to machine-established authority.
