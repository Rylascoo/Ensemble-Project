# Q-PROD-08 Scene identity + initial roster foundation — native ARM64 validation

Date: 2026-09-22
Status: **INTEGRATED / NATIVE VALIDATED / INDEPENDENT EXACT-CANDIDATE REVIEW CLEAN**
Baseline main: `11f04eb45e0ddeb688016d8d23c1657b421d604d`
Exact executable source: `31e16cbe274f9b90192bba67cd443fa3320ed8e2`
Product/Architecture authority: `docs/evidence/Q_PROD_08_SUCCESSOR_SELECTION_SCENE_IDENTITY_INITIAL_ROSTER_FOUNDATION_2026_09_22.md`

## Earned scope

The exact executable implements only the authorized Scene identity / initial-roster substrate:

- Application-owned opaque `SceneId`;
- `SceneRoster` containing only existing same-Production `CharacterId` values;
- duplicate roster identity rejection and Production-Cast-order canonicalization;
- `EstablishedScene` plus replay-derived `ProductionScenes`;
- one creator-authority event family in the existing Production journal;
- explicit Scene establishment against the currently open Production;
- exact returned Scene plus authoritative post-append replay;
- versioned persistence, reopen, recovery, portable export and rebuildable projection snapshot support.

Successful Scene establishment preserves Production name, World current state, Production Cast and ProductSpace/navigation. It does not create a current/active Scene, roster mutation, Scene ending/cardinality law, Character Core, Performer/provider assignment, Opportunity/Performance, Scene UI, fictional Character action, provider traffic or deferred-E0 execution.

## Exact event / compatibility contract

The durable event family is exactly:

`kymaean.production.creator-established-scene.v1`

Payload carries exact UTF-16BE code units for Scene identity and each roster Character identity.

Fail-closed behavior:

- supported v1 decodes and replays;
- unsupported same-family future version such as `kymaean.production.creator-established-scene.v2` throws `ProductionPersistenceCompatibilityException` and maps through the catalog as `Incompatible`;
- an unrelated unknown contract remains `InvalidDataException` / Product `Invalid`; inherited `FileProductionEventStoreTests` covers `kymaean.production.unknown.v1`;
- duplicate roster identities, duplicate Scene identities and roster identities outside Production Cast fail closed.

There is **no migration/compatibility adapter for a reshaped or withdrawn Scene event family**. That capability is **UNEARNED**. A future semantic replacement must introduce an explicit version/migration/compatibility policy; current durable history must never be silently ignored or reinterpreted.

If later deferred-E0 convergence changes the Scene primitive, the directly exposed Product/Persistence surfaces are `SceneId`, `SceneRoster`, `EstablishedScene`, `ProductionScenes`, `ProductionReplayProjection`, `CreatorEstablishedSceneEvent`, the Production event codec/version policy, catalog Scene-establishment port and snapshot representation.

## Snapshot / cache authority

Projection snapshot version advances from v3 to **v4** only because the cache now contains replay-derived Scene state in addition to already-earned Product projection fields.

The snapshot remains optional, derived and non-authoritative.

Native regression `DeletedSnapshotRebuildsIdenticalCastAndSceneFromJournal` proves:

1. a Production is created with two Characters;
2. Scene input is submitted in reverse Cast order;
3. authoritative replay stores the canonical Cast-order roster;
4. the v4 snapshot is deleted;
5. ordinary reopen reconstructs identical Production Cast + Production Scenes from the authoritative journal;
6. a fresh v4 snapshot is recreated.

Malformed, stale, old-version or semantically inconsistent snapshots remain cache misses and cannot override journal truth.

## Independent-review corrections

### 1. Exact post-append replay

Review of predecessor `f58bbbd5b9e5109293318dddf6a745e78c48ec77` found an avoidable post-commit interleaving window: catalog mutation appended under one journal gate and then reacquired the gate to reload replay. An unsupported overlapping writer could therefore append between those operations, making the first caller observe a richer replay and potentially report failure after its own event was already committed.

Repair `31e16cbe274f9b90192bba67cd443fa3320ed8e2` adds `AppendValidatedHistory`: the exact candidate replay/anchor is derived from the candidate history while `AppendValidated` owns the existing journal gate and is returned only after append publication completes. Catalog mutation no longer performs a second post-append replay load.

This does **not** broaden the existing concurrency contract. Same-Production overlapping access remains unsupported and fail-fast under the established Q-PROD-01 per-journal lock policy; no wait, retry, queue, rollback or multi-writer transaction is introduced.

### 2. Cast / Scene projection invariant

The same review found that the public `ProductionReplayProjection` constructor previously accepted Production Cast and Production Scenes independently.

The repaired constructor now requires every Scene roster Character to exist in Production Cast and requires the stored roster order to equal Production-Cast canonical order. Invalid custom catalog/snapshot projections therefore cannot cross the public Application projection boundary.

Focused native validation after both repairs passed Application **84/84** and Persistence **148/148**.

### Final exact-candidate review

A fresh separate Codex review session on exact `31e16cbe274f9b90192bba67cd443fa3320ed8e2` used:

- model: GPT-5.6 Sol;
- reasoning effort: High;
- effective sandbox: **read-only**;
- base: exact `11f04eb45e0ddeb688016d8d23c1657b421d604d`;
- static source/diff review only;
- no tests/builds, repairs, provider calls or repository mutation.

Result: **CLEAN**.

A prior built-in review attempt tried to launch test binaries inside a read-only sandbox and was denied creation of MSTest `TestResults`; those apparatus failures were not Product/test evidence and were discarded. The final review above explicitly prohibited test execution.

## Native Windows ARM64 evidence

Host: SurfSeven.

Environment verifier on the executable lineage:

- Windows ARM64 OS/process;
- .NET SDK 9.0.317 under repository 9.0.100/latestFeature policy;
- Windows target `net9.0-windows10.0.26100.0`;
- Windows SDK 10.0.26100.0;
- nine-project prerequisites PASS.

Remote process launch omitted the standard `ProgramFiles(x86)` environment variable; verifier was rerun with only `C:\Program Files (x86)` supplied process-locally. No repository or persistent machine configuration changed.

At exact executable `31e16cbe274f9b90192bba67cd443fa3320ed8e2`:

- Application tests: **84/84 PASS**, native `win-arm64`;
- Persistence tests: **148/148 PASS**, native `win-arm64`;
- ARM64 WinUI Release build: **PASS, 0 warnings / 0 errors**;
- `git diff --check`: clean;
- worktree source status: clean.

Repository-law execution then stopped only because `CURRENT_STATE.md` had reached four commits of staleness while the executable/review repairs accumulated; this documentation checkpoint exists to restore the required N=3 state-currency invariant. That bookkeeping failure does not change the native executable results.

## Deferred-E0 / reversibility status

The package-local deferred-E0 exposure table in the successor-selection record remains controlling:

- Q-E0D-01: no direct identity/roster dependency under the frozen method;
- Q-E0E-RUN: no direct identity/roster dependency under the current control purpose;
- Q-E0F-01: potential failure-semantics exposure remains provisional;
- Q-E0G-01: potential generalization exposure remains provisional;
- Q-E0-CONV / Q-POSTE0: final Scene primitive/persistence architecture remains explicitly revisable.

No deferred-E0 result is predicted or consumed by this implementation.

## Gate provenance

- **Product / Architecture authority:** Q-PROD-08 successor-selection evidence above, authorized under standing Director delegation.
- **Primary Implementation writer surface:** `qprod08-current-wt` on the bounded Q-PROD-08 branch; shared Git author identity is not used as role proof.
- **Independent Review:** separate exact-ref Codex read-only review of executable `31e16cb...`, final result CLEAN after two earlier findings were repaired.
- **Design acceptance:** **NOT APPLICABLE** — this package creates no Scene UI/presentation contract.
- **Director merge authority:** **EXERCISED** — PR #257 merge was explicitly authorized by the Director and completed; exact-main Validation #1283 PASS.

## Integration closeout

Final documentation head `43176756bac1b3726f00d1525496e41a9f0e529e` passed branch Validation #1281, PR Validation #1282 and E0-E preparation #202. Under explicit Director merge authorization, PR #257 merged to `main` at `9e4a1e30d6d40b46c0fca5e24f6735d3453b2d45`; push-triggered exact-main Validation #1283 passed.

Native runtime/test authority remains bound to exact executable source `31e16cbe274f9b90192bba67cd443fa3320ed8e2`.

## Evidence boundary

Native machine-test authority belongs only to executable source `31e16cbe274f9b90192bba67cd443fa3320ed8e2`.

This evidence/state checkpoint and later hosted/merge commits may describe that result but do not inherit or inflate native runtime authority.

No .NET 10 retarget, package upgrade, provider traffic, deferred-E0 execution, package/WACK/Store or release authority is claimed.
