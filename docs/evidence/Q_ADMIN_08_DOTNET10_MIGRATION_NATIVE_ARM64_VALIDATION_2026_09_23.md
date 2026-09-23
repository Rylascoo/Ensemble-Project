# Q-ADMIN-08 .NET 10 migration — native ARM64 validation

Status: **DONE / INTEGRATED THROUGH PR #259 / EXACT-MAIN VALIDATION #1291 PASS**

## Authority and scope

On 2026-09-23 the Director chose **RETARGET NOW** and authorized a dedicated platform/tooling package from fresh live `main@5ae2bf2307df4faa8c7342303419ba3fc5e620e1` (push Validation #1286 PASS). The earlier disposable probe at `a2ab3a4287b0b3c3e42eef961877c3a973efd9aa` predates integrated Q-PROD-08 and is decision evidence only: `docs/evidence/Q_ADMIN_08_DOTNET10_MIGRATION_DECISION_INPUT_2026_09_22.md`.

One primary writer owns branch `codex/qadmin08-dotnet10-migration-2026-09-23` in `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\codex-qadmin08-dotnet10-20260923`. Authorized changes: SDK 10.0.400 feature band, all eleven project TFMs to net10, Microsoft Testing Platform selection/invocation and necessary CI/operator guidance. Package versions remain unchanged; a required package-version change is a stop boundary. Product, persistence contracts, Character/Scene behavior, Design/UX, provider behavior, deferred E0 and release policy are excluded. Q-PROD-09 is not authorized. The accepted Q-PROD-08 validation-tag repair remains continuity only.

## Fresh .NET 9 comparison baseline

Measured on SurfSeven at exact base above using native ARM64 SDK 9.0.317 / runtime 9.0.19: environment/RID PASS; eleven forced-evaluation restores and eleven Release builds PASS, all zero warnings/errors; Core 628/628, Harness 155/155, Application 84/84, Persistence 148/148, preparation 10/10 PASS, no skips. Both credential-free fixture smokes PASS. This includes integrated Q-PROD-08, unlike the earlier probe.

Native development registration of a copied exact layout under a new disposable identity launched a responsive ARM64 `Kymaean` window with the empty Home state; clean close and removal PASS. No existing package identity/data was reused. The validation apparatus corrected an inappropriate staged-package flag and an incorrect Home label assertion before the successful smoke; neither required a project change.

## Exact candidate and measurements

Executable: `4b2d286a4baf1bb01b222aceda26fbaae8772c15`. Annotated tag: `validation/q-admin-08-dotnet10-native-arm64`; tag object `d347b67a749a5d92a6ed8c1108255e808688fa2a` peels to that executable. Native evidence belongs to this exact SHA; later documentation commits do not acquire separate runtime authority.

| Check | Fresh .NET 9 baseline | Exact .NET 10 candidate |
|---|---|---|
| Native environment | SDK 9.0.317 / runtime 9.0.19 | SDK 10.0.400 / runtime 10.0.11 |
| OS/process/RID | Windows ARM64 / ARM64 / win-arm64 | Same; environment verifier PASS |
| Restore and eleven Release builds | PASS, zero warnings/errors | PASS, zero warnings/errors |
| Application, including Q-PROD-08 | 84/84 | 84/84 |
| Persistence, including Q-PROD-08 | 148/148 | 148/148 |
| Core | 628/628 | 628/628 |
| Harness | 155/155 | 155/155 |
| E0-E preparation deterministic tests | 10/10 | 10/10 |
| Test discovery | 1,025 cases, zero skips | Identical case names/counts; zero skips |
| Credential-free fixture smokes | Both PASS | Both PASS; stdout byte-identical |
| ARM64 WinUI Release | PASS, zero warnings/errors | PASS, zero warnings/errors |
| Native startup / close | Responsive empty Home; clean close | Same automation names; clean close |

All eleven project files were restored separately with `dotnet restore <project> -r win-arm64 --force-evaluate -p:Configuration=Release`, then built with `dotnet build <project> -c Release -r win-arm64 --no-restore`. Windows additionally uses `-p:Platform=ARM64`; the preparation executable uses `--no-self-contained`. All five suites ran with `dotnet test --project <project> -c Release -r win-arm64 --no-build`; the .NET 9 baseline used its original positional-project invocation. SDK/runner/package versions were not installed or upgraded on the host.

`tools/test-application.ps1 -Full` passed 84+148. `-Full -Filter 'FullyQualifiedName~Scene'` passed 12+7. An intentionally absent exact filter returned exit **8** after Application selected zero tests, before Persistence started. This is expected fail-fast test-runner evidence, not a failing Ensemble assertion. MSTest 4.1.0 filter syntax is retained; MTP replaces the SDK's VSTest entry path. CLI summary formatting and orchestration timing changed; no performance claim is made from these runs.

## Dependency and output comparison

No `PackageReference` or `MSTest.Sdk` version changed, and no package upgrade was needed. Every retained NuGet package has the same resolved version and SHA-512 content hash. The Windows graph remains 17 packages. Each test graph changes from 17 to 14 because SDK 10's measured `packagesToPrune` removes framework-provided transitive `System.Collections.Immutable/8.0.0`, `System.Reflection.Metadata/8.0.0`, and `System.Diagnostics.DiagnosticSource/5.0.0`; no replacement package was added. This is SDK/TFM restore pruning, not an edited dependency policy. MSTest.Sdk/MSTest remain 4.1.0 and Microsoft Testing Platform remains 2.1.0. Restore is measured, not claimed lockfile-enforced.

The runtime changes from .NET 9.0.19 to 10.0.11, and framework-targeted assemblies/apphost change bytes as expected. WindowsAppRuntime remains 2.5.1.0 ARM64. `resources.pri`, `App.xbf`, `MainWindow.xbf`, `MainPage.xbf`, both fixture stdout streams and observed Home automation names are unchanged. Candidate `Kymaean.Windows.exe` SHA-256: `8cce3c57088cc1a3f598d0850c60259d1767cddaa26d1b7ea8629a94b0358ee8`; DLL: `b03fa473795893557f66a9828e9d497337edab4114a17bf4fd4201dad1118266`. No application C#, XAML, persistence codec, fixture or test assertion changed.

## Persistence and native runtime comparison

An external scratch probe referenced each baseline/candidate's freshly built Application/Persistence assemblies. Each runtime seeded one Production, World truths containing isolated UTF-16 surrogate and canonically distinct Unicode sequences, two Characters with identical exact names, and an initial Scene roster supplied in reverse Cast order. It emitted UTF-16 code units, identities/order, event count, portable-export hash and authoritative journal/head/identity hashes. Five events were preserved.

For **both .NET 9 -> 10 and .NET 10 -> 9**, separate copies passed (1) reopen/export inspection, (2) snapshot deletion/rebuild, and (3) committed-head deletion/explicit recovery. All six transcripts matched their originating seed exactly; reexports were byte-identical and authoritative bytes/hashes were preserved. This supplies a measured cross-runtime compatibility check alongside the complete Persistence suite; it does not add an import API, new persistence law or power-loss certification.

Native launch copied the exact generated layout and unchanged tracked assets, verified binary/XAML/resource hashes, and changed only `Identity.Name` in the disposable manifest. Separate `Kymaean.QAdmin08.baseline.20260923` and `Kymaean.QAdmin08.candidate.20260923` development identities isolated LocalState. Both processes were native `0xAA64` (process machine `0x0000`), responsive, and loaded the expected .NET runtime. Both closed normally; temporary registrations were removed. Existing `Kymaean.Development` and historical layouts/data were preserved. No runtime deployment, shipping-manifest edit, signed-package validation, WACK or Store claim is made.

## Repository and independent review

At exact executable: repository-law PASS; document authority census **546 / 424 current / 30 historical / 92 archive / 0 unexplained**; oracle assertion coverage **696 documented / 17 asserted / 679 document-only**, with no lost assertions; whitespace and protected-diff checks PASS. At evidence head `46b7d7a22ca0de4b4af0561ff576b12fa4edd69e`, all guards passed again: census **547 / 425 / 30 / 92 / 0 unexplained**, oracle **802 / 17 / 785**, state distance **0** and **2,848 UTF-8 bytes**. The added hashes are evidence-artifact identities; all 17 asserted oracles remain covered. The native suites exercise the existing deterministic/reference oracles unchanged. Final document-closeout guards must pass again before push.

Independent reviewer task `dotnet10_review` returned **NO ACTIONABLE FINDINGS** for exact `5ae2bf2307df4faa8c7342303419ba3fc5e620e1 -> 4b2d286a4baf1bb01b222aceda26fbaae8772c15`, including runner/filter/fail-fast, all native logs, six compatibility transcripts and guard evidence. Effective policy was workspace-write/auto_review; technical write containment was **not proven**. Review was behaviorally no-write, with no escalation, validation execution, repairs or authority mutation. Hosted gates and later documentation were outside that executable review.

The same independent reviewer separately reviewed documentation-only `4b2d286 -> 46b7d7a`, returned no actionable findings, and verified all **108** inventoried artifact sizes/hashes plus tag peeling and embedded comparison data. Later closeout documentation remains subject to its own bounded review; it does not alter the executable.

Raw commands/results, dependency manifests and external probe: `C:\Users\Wiryl\Sol Dev\admin-scratch\qadmin08-dotnet10-migration-20260923`. The retained artifact inventory, byte sizes, hashes and comparison data are in `docs/evidence/Q_ADMIN_08_DOTNET10_MIGRATION_MEASUREMENTS_2026_09_23.json`. The .NET 9 first-use sentinel generated by a help invocation was preserved in scratch, excluded from source, and telemetry-disabled validation left the worktree clean.

## Continuity and hosted gate boundary

At the pre-merge checkpoint, live `main` was `5ae2bf2307df4faa8c7342303419ba3fc5e620e1`. At that checkpoint, the twelve pre-existing live refs were unchanged: ten merged provenance refs, `main`, and the unique closed/unmerged PR #226 branch. No historical ref/worktree was reset, adopted, pruned or deleted. This task adds only its isolated writer branch/worktree and exact validation tag; Q-ADMIN-07's ambiguous historical local residue remains deferred. Any future merged-ref removal must follow archive-tag-before-delete law.

Read-only local continuity inventory found **49** registered worktrees, all resolving to the same Git common directory; the writer and primary `main` checkout were clean. Two preserved historical worktrees contain staged residue: `C:\Users\Wiryl\Sol Work\Ensemble-Project\worktrees\e0c-p01-blind-score-2026-09-13` at `5c0284cdf5f9a0ec7dfbef529f3196df1434e0ae` (six staged paths), and `C:\Users\Wiryl\Sol Work\Ensemble-Project\worktrees\e0c-p01-unblind-closeout-2026-09-13` at `e1c20e6f25fa876c8c9ee8fabb911bf8365025bc` (five staged paths). Both remain owner/evidence-disposition work under deferred Q-ADMIN-07, not migration inputs. Full paths/HEADs/status/Git ownership boundaries: scratch `worktree-continuity.json`, SHA-256 `dc26b0659f91d5a1daf5b7f470289cf2cc0a9433abf3ea9026236591b35b8526`.

At its initial evidence checkpoint, draft [PR #259](https://github.com/Rylascoo/Ensemble-Project/pull/259) targeted that unchanged exact main. At evidence head `46b7d7a22ca0de4b4af0561ff576b12fa4edd69e`, every exact-head workflow run completed successfully:

| Event / gate | Run | Result |
|---|---|---|
| Push Validation | [#1287](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35827425150) | PASS |
| PR Validation | [#1288](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35827470289) | PASS |
| PR E0-E preparation | [#204](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35827470327) | PASS |

The hosted compiler/x64 gates remain distinct from native ARM64 evidence. Any later documentation-only closeout head requires fresh exact-head hosted checks before merge; no success is inferred by ancestry. Branch and annotated native validation tag are published. The initial draft/merge gate was subsequently satisfied as recorded below.

## Authorized integration

On 2026-09-23 the Director explicitly authorized merging PR #259. Fresh checks resolved final head `670309d2fcd6db7654db043c02b08a4705866183`, unchanged base `5ae2bf2307df4faa8c7342303419ba3fc5e620e1`, clean writer and successful exact-head push [Validation #1289](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35828067514), PR [Validation #1290](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35828071206), and PR [E0-E preparation #205](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35828071213). Independent documentation review at the final head returned no actionable findings. Final guards passed: census 547 / 425 current / 30 historical / 92 archive / 0 unexplained; oracle coverage 803 documented / 17 asserted / 786 document-only; state distance 0 and 2,910 UTF-8 bytes.

PR #259 merged with expected-head protection at `7650fa40488fd19741c5bd0b65830d50fd62fa7b`. Its tree is identical to the final PR head. Exact-main [Validation #1291](https://github.com/Rylascoo/Ensemble-Project/actions/runs/35828613021) completed successfully. The primary local main checkout fast-forwarded cleanly to that merge. .NET 10 is now the integrated baseline; native evidence remains bound to executable `4b2d286a4baf1bb01b222aceda26fbaae8772c15` and its annotated tag.

The same isolated primary writer prepares documentation-only branch `codex/qadmin08-post-merge-closeout-2026-09-23` from the merge. The original migration branch remains preserved provenance; no historical branch/worktree is deleted or adopted. This closeout requires its own review, hosted checks and merge authorization.

## Exit boundary

Next is fresh Product/Design successor reconciliation for Director disposition. No Product implementation package or Q-PROD-09 is authorized. Q-ADMIN-07 and all unrelated historical branches/worktrees remain preserved.
