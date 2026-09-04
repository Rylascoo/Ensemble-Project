# H1 Patch 0015 Documentation Errata

Status: AUTHORITATIVE CLARIFICATION — DOCUMENTATION ONLY

Patch: H1 Patch 0015 — Recent Performance Context Continuity

This artifact records two documentation inconsistencies discovered during PR #29 review. It does not change approved architecture, implementation semantics, canonical values, or machine-validation authority.

Historical evidence/handoff checkpoints are preserved rather than rewritten after the fact.

## 1. Implementation handoff live-oracle IDs

Historical pre-implementation handoff:

`docs/handoff/E0A_H1_PATCH_0015_IMPLEMENTATION_HANDOFF.md`

contains an older live-oracle example using:

```text
TAKE-PATCH-0015-LIVE-ORACLE
COMMIT-PATCH-0015-LIVE-ORACLE
PRESSURE-PATCH-0015-LIVE-ORACLE
```

That example is superseded by the exact approved Proposal 0.15 oracle branch and the final reference oracle.

Authoritative Patch 0015 live-oracle inputs are:

```text
TakeId
TAKE-PATCH-0012-ORACLE

CommitId
COMMIT-PATCH-0012-ORACLE

Materialized RecordId
PRESSURE-PATCH-0012-ORACLE
```

These IDs are deliberately reused on a separate genesis-derived live reference branch to isolate the identity effect of changing the source Context from historical fixture-derived v1 to Production-bound v2. There is no collision with the historical chain because the oracle branches are separate.

Authority:

1. approved blueprint Proposal 0.15, Section 34;
2. `docs/evidence/H1_PATCH_0015_REFERENCE_ORACLE.md`;
3. `tests/Ensemble.E0.Core.Tests/Continuity/Patch0015TestSupport.cs` `FirstOracleTurn()`;
4. native ARM64 `571/571` Core-test pass at `b890b7eca66c391fae3ec30af0442dcc0e9f6aec`.

No implementation correction is required.

## 2. Blueprint-audit dependency arrow wording

Historical architecture-audit evidence:

`docs/evidence/H1_PATCH_0015_BLUEPRINT_AUDIT.md`

contains an ambiguous/contradictory shorthand where `Context -> Performer` and `CausalCommit -> Opportunity` appear in both permitted-flow and prevented-dependency lists.

The intended and approved dependency law is unambiguous when stated as compile/source reference ownership:

- Domain does not depend on Context, Performer, CausalCommit, Opportunity, or Continuity.
- Context may depend on Domain/Access/Production types as already approved, but Context does **not** depend on Performer, CausalCommit, Opportunity, or Continuity.
- Performer may consume Context and Domain types.
- CausalCommit may consume lower Context/Domain/Take/StateAuthority types, but CausalCommit does **not** depend on Opportunity or Continuity.
- Opportunity may consume CausalCommit types; the reverse dependency is forbidden.
- Continuity may compose Production/Access/Context/CausalCommit/Opportunity authorities as approved; lower authorities do not depend upward on Continuity.
- Production does not depend on Access, Context, CausalCommit, Opportunity, or Continuity.

This is the same dependency law implemented and tested by Patch 0015; only the historical arrow shorthand was ambiguous.

## Validation impact

None.

This errata changes documentation only and does not modify:

- `src/`;
- tests;
- fixtures;
- Harness;
- canonical oracle values;
- approved public APIs;
- native validation authority.

The exact machine authority remains:

- Core tests: `b890b7eca66c391fae3ec30af0442dcc0e9f6aec` — `571/571` PASS on native Windows ARM64;
- Harness/fixture execution: `5cb055e6dddea721aee98fee7f633191543e6490` for unchanged executable source.
