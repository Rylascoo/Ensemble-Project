# H1 Patch 0013 — Reference Oracle

Status: STATIC REFERENCE ORACLE — NOT YET MACHINE VALIDATED
Date: 2026-09-03

## Purpose

Freeze an independently derived deterministic reference result for the approved Patch 0013 `opportunityTransition` StateHash envelope before native compiler/runtime validation.

This artifact is not ARM64 execution evidence. The user's native Windows ARM64 run remains compiler/test/runtime authority.

## Source chain

The reference path deliberately begins from the already-established Patch 0012 Missing Raft oracle chain.

Genesis StateHash:

```text
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
```

Patch 0012 reference source commit:

```text
CommitId = COMMIT-PATCH-0012-ORACLE
TakeId = TAKE-PATCH-0012-ORACLE
Approved mutation = add Pressure "Pressure increases."
Materialized RecordId = PRESSURE-PATCH-0012-ORACLE
```

Exact Patch 0012 postcommit StateHash:

```text
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
```

That postcommit state has:

```text
CurrentOpportunityCharacterId = null
```

The source OpportunityHistory is the exact genesis history:

```text
SceneId = SCENE-MISSING-RAFT
LastOpportunityStateHash = 30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104
CharacterIds = [ VOSS ]
```

## Director result

The accepted source Candidate has no nomination and no addressed Characters.

Frozen Patch 0007 least-intervention semantics therefore use `RecencyFallback` over canonical roster:

```text
[ MARLOWE, VOSS, WREN ]
```

With source history:

```text
[ VOSS ]
```

`MARLOWE` and `WREN` are both never opportunitied. The frozen ordinal CharacterId tie-break selects:

```text
MARLOWE
```

## Exact opportunity payload

Canonical UTF-8 JSON bytes decode to exactly:

```json
{"schemaVersion":"ensemble.e0.opportunity-transition.v1","strategyContract":"ensemble.e0.director.least-intervention.v1","selectedCharacterId":"MARLOWE"}
```

No whitespace or additional properties occur.

## Result projection

The Patch 0013 result Production projection is byte-for-byte the Patch 0012 reference postcommit projection except:

```text
CurrentOpportunityCharacterId:
null -> MARLOWE
```

The Patch 0012 oracle Pressure record remains effective and unchanged. No other Production record, origin identity, Scene, Character, roster, lifecycle, protection, text, or provenance field changes.

## Exact Patch 0013 hash envelope

Conceptual property order:

```json
{
  "hashContract":"ensemble.e0.production-state-hash.sha256.v1",
  "kind":"opportunityTransition",
  "parentStateHash":"057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30",
  "opportunityPayload":{...exact payload above...},
  "resultProjection":{...existing canonical Production projection with Current Opportunity MARLOWE...}
}
```

SHA-256 over those exact canonical UTF-8 bytes yields:

```text
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

Therefore the expected reference result is:

```text
ParentStateHash:
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30

SelectedCharacterId:
MARLOWE

ResultStateHash:
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310

Result OpportunityHistory:
[ VOSS, MARLOWE ]

Result LastOpportunityStateHash:
dc7e169fc52a0051525baf13cb579b87126ba55c77a3b972c4d5a6a6b3246310
```

## Independent derivation check

The static derivation did not call Patch 0013 production code.

It independently reconstructed canonical Production/state envelopes from:

- the canonical Missing Raft fixture;
- the frozen fixture hash;
- the existing Patch 0012 Production projection mapping/token rules;
- the existing Patch 0012 reference causal payload;
- the approved Patch 0013 payload/property order.

As a self-check, the same independent reconstruction first reproduced exactly:

```text
Patch 0012 genesis:
30041ae0dd287b9ef192aaf90c0cee4aedf85e4e8a9c3b31ad14094fbfda0104

Patch 0012 postcommit:
057034560f8d97f648ed7ba66776ba7d1ced7ed694c2bd6561dd0ef1fac24c30
```

Only after those established digests matched was the new opportunity-transition digest calculated.

## Implementation regression

`tests/Ensemble.E0.Core.Tests/Opportunity/Patch0013ReferenceOracleTests.cs` pins this exact result and simultaneously reasserts the two established Patch 0012 hashes.

Native validation must confirm the test passes on the user's Windows ARM64 machine before this reference digest becomes machine-established Patch 0013 evidence.
