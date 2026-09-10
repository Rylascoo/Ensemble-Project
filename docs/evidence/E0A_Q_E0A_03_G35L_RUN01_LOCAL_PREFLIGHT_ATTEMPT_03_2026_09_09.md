# E0-A Q-E0A-03 Gemini 3.5 Flash-Lite Run 01 — Local Preflight Attempt 03

Status: **WRAPPER-ONLY FALSE GATE RESOLVED — PROVIDER INVOCATION NOT STARTED — AUTHORIZATION UNCONSUMED**

Date: **2026-09-09**

RunId: `E0A-Q03-G35L-20260909-01`

## Director-machine result

The guarded Windows ARM64 packet stopped before credential entry and before the provider boundary with:

```text
THIS_PACKET_DID_NOT_CONSUME_AUTHORIZATION=YES
PROVIDER_TRAFFIC_NOT_STARTED=YES
LOCAL_PREFLIGHT_STOP=Canonical Missing Raft fixture SHA-256 mismatch.
```

No `PROVIDER_INVOCATION_STARTED=YES` marker appeared. No credential prompt was reached. No provider request was sent.

## Classification

Attempt 03 is **UNCONSUMED** under the existing Director authorization law. The exact one-run authorization remains live subject to all frozen freshness and activation gates.

The stop was caused by an invalid packet-level assertion, not a Harness fixture validation failure.

## Root cause

The wrapper computed:

```text
Get-FileHash <working-tree fixture JSON> -Algorithm SHA256
```

and compared that raw working-tree-file digest directly with:

```text
5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703
```

Repository source proves that `5556...` is the frozen **canonical ECJ-1 Fixture identity**, not a requirement that platform-specific working-tree JSON bytes hash directly to that value.

At exact authorized executable `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`:

- `FixtureHash.Compute(ValidatedFixture)` serializes the validated Fixture through `Ecj1FixtureCanonicalizer.Serialize(fixture)` and SHA-256 hashes those canonical bytes;
- `MissingRaftContract.Validate` compares `FixtureHash.Compute(fixture)` to exact expected hash `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- the Harness fixture-smoke path reads the JSON, loads and generically validates it, invokes `MissingRaftContract.Validate` for the Missing-Raft family, and prints `Fixture validated: ensemble.e0.missing-raft@0.1.0` only after those checks pass;
- the exact committed fixture at the authorized executable is Git blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd`.

The wrapper therefore confused canonical semantic Fixture identity with raw working-tree byte identity. That check was redundant and semantically wrong.

## Corrected packet law

The invalid raw `Get-FileHash` gate is removed. The corrected local packet must instead require all of the following before credential activation:

1. execution worktree exact at authorized executable `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`;
2. execution worktree detached and clean;
3. exact fixture path exists;
4. `git rev-parse HEAD:fixtures/missing-raft/missing-raft-0.1.0.json` equals exact authorized Git blob `6c2ed0e1081ee4e1165dbfb162b3028cbd1136fd`;
5. fresh native Harness build passes;
6. Missing-Raft Harness smoke exits zero and reports `Fixture validated: ensemble.e0.missing-raft@0.1.0`, thereby exercising `MissingRaftContract.Validate` and the canonical `FixtureHash.Compute` law;
7. all pre-existing evidence/claim/authorization/freshness/provider-boundary guards remain intact.

This correction does not normalize, rewrite, or alter Fixture bytes. It changes only the external wrapper assertion so it matches the executable's actual frozen Fixture identity semantics.

## Authorization consequence

- exact RunId authorization: **AUTHORIZED / UNCONSUMED**;
- provider invocation: **NOT STARTED**;
- provider traffic: **NONE**;
- Gemini credential: **NOT REQUESTED**;
- executable/source/test/fixture/tag/profile/model/RunId/evidence-root unchanged;
- no retry/rerun/fallback/probe/alternate-model authority is created;
- guarded local preflight may resume only with the corrected Git-blob + Harness canonical-validation gate and the current audited authorization-record blob.
