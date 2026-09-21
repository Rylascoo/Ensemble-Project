# Q-PROD-01 explicit Design architecture readiness reassessment

Date: 2026-09-21. Status: Engineering determination; documentation/state/evidence only.

## Baseline and recovered definition

Fresh GitHub main and PR #242 merge are both `4283505a0cd44dae9fa812893ec79d9a052ee6f9`; #242 merged at 2026-09-21T06:04:51Z. Exact-main push Validation gate #1119, run [35566903678](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35566903678), completed/success. Complete exact-SHA query contained one run; deterministic CI normalization PASS. Starting state: 2995 bytes, distance 1. Its integration-pending prose was stale despite passing currency.

Baseline AGENTS, state, authority, hygiene, orchestration, reasoning protocol, native workflow, queue, validation ledger and relevant Product/Design records were recovered. Task worktree `C:/Users/Wiryl/.codex/worktrees/96b4/Ensemble-Project` began clean/detached at main; sole writer owns four Engineering documents on `codex/qprod01-design-readiness-reassessment-2026-09-21`. Common Git directory: `C:/Users/Wiryl/Sol Dev/Ensemble-Project/.git`; task Git directory ends `worktrees/Ensemble-Project3`. No implementation or Design lease is opened.

Historical definition, `docs/PROJECT_THREE_ENGINEER_PARALLEL_OPERATING_MODEL_DIRECTOR_AMENDMENT_2026_09_17.md`, Design-unblocking milestone:

> That milestone means the Application/Product contracts required for Design Sol to continue app design are stable enough that Design is no longer guessing about runtime Product state.
>
> It does not require complete P1, complete Windows runtime wiring, provider integration, final architecture freeze, or release authority.

`docs/Q_PROD_01_THREE_ENGINEER_WORKSTREAM_CHARTER_2026_09_17.md` requires exact safe contracts, still-provisional areas and integrated/exact-main-validated adoption. Native workflow supersedes its persistent-role mechanics, not the milestone. Historical Application architecture evidence identified typed failure outcomes and persisted internal/creator state beyond ProductionName as deficits. Typed results, catalog, Windows composition and now the creator World-current producer/consumer resolve those concrete deficits. The native workflow required consumer integration followed by explicit reassessment; integration alone did not promote readiness.

## Integrated facts Design may rely on

Paths below are relative to baseline `src/`. Historical evidence and exact tags for Application architecture, typed results, catalog/recovery, Windows composition, snapshot, export and provisional storage/concurrency remain in `docs/VALIDATION_LEDGER.md`.

| Earned contract | Exact-main source and bounded meaning |
|---|---|
| Opaque stable Production identity; summary/list/open | `Kymaean.Application/ProductionIdentity.cs`, `ProductApplication.cs`: exact identity distinct from display name; complete-or-fail list; known-identity open. No catalog creation lifecycle inferred. |
| ApplicationScope vs ProductSpace | `Kymaean.Application/ProductApplicationProjection.cs`, `ProductSpace.cs`: app navigation separate from Studio/Stage/Archive orientation; enums do not choose final visible labels/tabs. |
| Typed bootstrap/open/explicit recover | `Kymaean.Application/ProductAccessResult.cs`, `ProductApplication.cs`: Incompatible vs Invalid; typed failure preserves prior usable projection/navigation. No auto-recovery or prediction of recoverability. |
| Real catalog and Windows composition | `Kymaean.Infrastructure.Persistence/FileProductionCatalog.cs`; `Kymaean.Windows/App.xaml.cs`, `WindowsStartupResult.cs`: app-private LocalState composes real Application/Persistence; environmental startup failure remains distinct. |
| Authoritative journal/replay; old-history compatibility | `FileProductionJournal.cs`, `FileProductionEventStore.cs` in Persistence; `Kymaean.Application/ProductionReplay.cs`: creation-only history yields empty World state; deterministic ordered replacement yields latest whole state. |
| Immutable canonical current truth; creator cause | `Kymaean.Application/WorldCurrentState.cs`, `ProductionEvents.cs`: ordinal canonical set, null/duplicate rejection, creator-specific whole-state replacement; no generic claim/belief/possibility bucket or automatic Character knowledge mutation. |
| Known-ID writer; one causal append; authoritative replay | Catalog lines 106-120 and 216-284: unknown identity creates nothing; each successful replacement appends exactly one event then returns replay. |
| Exact accepted UTF-16 and version policy | Persistence `FileProductionEventStore.cs`, `Utf16CodeUnits.cs`, `ProductionPersistenceVersionPolicy.cs`: creation v2/replacement v1 preserve every accepted code unit, including isolated surrogates; creation v1 decode retained; journal schema v1 unchanged. Already-lost legacy text cannot be recovered. |
| Full snapshot v2 and raw export | Persistence `ProductionProjectionSnapshotCache.cs`, `ProductionPortableExport.cs`: name plus all truths; authoritative replay precedes cache reconciliation; old/bad cache rebuilds; expected cache I/O best-effort. Raw committed payload order/identity survives export inspection/replay; no credentials/local layout/import authority. |
| Typed failures and provisional concurrency | Compatibility -> Incompatible; corruption/malformed/replay-invalid -> Invalid; environmental I/O stays exceptional. One exclusive journal operation per Production root, fail-fast same-root contention. Append and replay are separately gated: post-append failure can leave a committed event. No automatic retry/rollback/idempotence, global transaction or cross-Production atomicity. |

Current producer/consumer evidence: `docs/evidence/Q_PROD_01_WORLD_CURRENT_TRUTH_SUCCESSOR_NATIVE_ARM64_VALIDATION_2026_09_20.md`; `docs/evidence/Q_PROD_01_LOSSLESS_PERSISTENCE_CONSUMER_NATIVE_ARM64_VALIDATION_2026_09_21.md`; Director contract `docs/Q_PROD_01_LOSSLESS_UTF16_PERSISTENCE_CONTRACT_2026_09_21.md`.

Annotated tag `validation/q-prod-01-lossless-persistence-consumer-native-arm64` peels to tested/reviewed `e07267185cbae7caa62f70bc9f9a39852c0eedd0`. Diff to starting main contains only five documentation files: runtime/test/build blobs are identical. Preserved native evidence: focused 44/44, Persistence 129/129, Application 57/57, Core 628/628; explicit WinUI ARM64 Release 0 warnings/errors. No Product suite or native UI was rerun here. Snapshots earn no read acceleration, exports no global freeze, interruption tests no universal power-loss guarantee.

## Design dependency audit and decision

`DESIGN_ARCHITECTURE_READY = READY`.

Design can reconcile known-Production navigation/access/recovery and creator current-truth inspection/replacement presentation against actual durable state and failure semantics. Engineering supplies these facts; Design selects lawful presentation. Causal replacement does not implement a general Archive/history-query UI or a current-situation/Scene primitive. READY is the historical architecture handoff, not blanket permission to use unearned runtime meanings.

Read-only Design authority: `docs/design/app/AUTHORITY.md`; contracts `NATIVE_IMPLEMENTATION_HANDOFF.md`, `APP_AND_FAILURE_STATES.md`, `EXPERIENCE_ONTOLOGY.md`, `PRESENTATION_PERSPECTIVES_AND_DISCLOSURE.md` under `docs/design/app/contracts/`; `docs/design/app/contracts/alpha-stage/APPUI_ALPHA_STATIC_CONFORMANCE_MATRIX_01.json`. The matrix labels richer performance/Character/history/perspective meanings conditional capacities, not build-now requirements. Infrastructure stays outside fiction; current truth is not Character knowledge. Historical NOT READY observations remain true at their observed refs and are unchanged.

A = required for this milestone; B = not required here / separately gated. All A conditions in the earned-contract table are satisfied; no remaining A blocker was found.

| Still unearned/open | Class | Retained authority/gate |
|---|---|---|
| Home A/B | B | `docs/design/app/unresolved/HOME_REENTRY_AB.md`: co-equal unresolved alternatives, no selection implied. |
| FIRSTUSE adoption | B | `docs/design/app/evidence/pending/FIRSTUSE_STUDY.md`: pending adoption; no create/import commands. |
| Richer Studio/Stage/Archive ontology | B | Design conformance matrix capacities remain conditional; complete ontology is not the milestone. |
| Product create/rename/delete; import/restore | B | Consumer contract excludes lifecycle expansion; lower-level creation event is not catalog lifecycle. |
| Provider/Performer behavior | B | Explicit milestone exclusion; build-ahead amendment and ODR-26 retain provider/spend gates. |
| Character/Circumstance/Scene/Pressure/Take/Rehearsal; accepted-performance evolution | B | No new persisted semantics earned; conditional Design capacity cannot manufacture them. ODR-12/13/30/32 and roadmap remain governing. |
| Deferred E0; final post-E0 architecture | B | Director build-ahead amendment permits provisional construction; Q-POSTE0-01/02 remain gated. |
| Alpha/Beta/release; packaging/WACK/Store | B | Queue retains separate runtime/certification gates. |
| Final UI labels/new visual choices | B | Design-owned; enums and READY do not freeze vocabulary. |
| Stage motion/transcript-dependent design | B | Q-DESIGN-02 and conformance matrix retain experiment/runtime-specific gates. |
| New native accessibility/launch; transparent concurrency/power-loss guarantees | B | Independent validation/policy obligations; not hidden architecture-handoff prerequisites. |

## Disposition and successor

Q-PROD-01 is COMPLETE for its intended design-unblocking architecture milestone; producer/consumer integrated and closed. Later Product construction is tracked separately, not indefinitely folded into Q-PROD-01. This does not declare P1 or Alpha complete.

Issue [#225](https://github.com/Rylascoo/Ensemble-Project/issues/225) is CLOSED / completed, fulfilled by #242. The charter permits CLOSED after disposition/integration reconciliation, now satisfied. Original body and corrections remain; factual [closeout](https://github.com/Rylascoo/Ensemble-Project/issues/225#issuecomment-5756190657) links PR/main/evidence. PR #226 remains closed unmerged/preserved, superseded by integrated #240. Transport closure is not itself readiness authority.

No immediate Engineering prerequisite blocks Design's fresh bounded reconciliation. Next independent Product package: **Q-PROD-02, read-only Windows presentation consumer of known-Production World-current truth**. Basis: build-ahead amendment's Application -> native shell sequence, roadmap P2/P4, and `docs/design/app/evidence/native/APPUI_01_Q_DESIGN_20_POST_COMPOSITION_RECONCILIATION_01.json`, which requests a bounded Windows consumer after integration. Fresh scope must bind existing projection/query/navigation only, preserve contextual Current Production, Home/Productions/Settings, quiet Back and infrastructure separation. No new history-query API, writer control, lifecycle, ontology, provider or visual selection. Stop for an actual new Product/Design decision. PREPARATION-READY, no implementation lease or execution here; Home/FIRSTUSE need not be selected to define this package.

## Preservation and package checks

Fresh GitHub census: 15 heads including main. Thirteen non-main heads are ancestral integration/E0/continuity evidence; historical producer is divergent/preserved. Originating contexts retain ownership pending Q-ADMIN-07 owner/evidence/archive disposition. Registered worktrees were inspected without mutation; other paths are preserved historical/validation/owner-context residue. Dubious-ownership refusals leave cleanliness UNKNOWN. No safe.directory/ownership change or bypass; no inherited path/ref released. Authority measurement's unexpected-worktree classification is preserved rather than relabeled clean. It does not block this isolated four-file package.

Closing gates: diff hygiene, state size/currency, queue consistency, document census, oracle, protected/scope guard, immutable behavioral no-write review, fresh-main race and hosted checks. Observed candidate/review/integration identities and results are recorded in the PR. This record adds no machine-validation rung or Design authority.
