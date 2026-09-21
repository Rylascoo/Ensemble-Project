# Q-PROD-01 lossless Persistence consumer validation

Date: 2026-09-21. Status: RETURNED / INTEGRATION PENDING; independently reviewed implementation candidate. No merge authority.

## Exact authority and source

- Fresh live base: `fb5c8d630b5aec1d6ab75bb57b7ffc65da79c00f`, matching dispatch and fresh continuation read. GitHub Validation gate run 35563253160 completed successfully. State distance 1, 3026 bytes at base.
- Producer integration: PR #240 at `9516a27ca019d2c6af304f46da89bde215449df4`; tested producer `168c5d306afc88202e9d5d30aacaeca8e4119dc6`, reviewed correction `0a48949d6c1e02be420bb9204b3b85b08fd76150`. Preserved evidence: `docs/evidence/Q_PROD_01_WORLD_CURRENT_TRUTH_SUCCESSOR_NATIVE_ARM64_VALIDATION_2026_09_20.md`.
- Implementation/test candidate: `e07267185cbae7caa62f70bc9f9a39852c0eedd0`.
- Branch: `codex/qprod01-lossless-persistence-consumer-2026-09-21`; exclusive primary writer in the existing isolated task worktree `C:\Users\Wiryl\.codex\worktrees\caa0\Ensemble-Project`, originally clean/detached at producer integration. Director explicitly authorized continuing this worktree. No second source writer.
- New Director authority: `docs/Q_PROD_01_LOSSLESS_UTF16_PERSISTENCE_CONTRACT_2026_09_21.md`.
- Issue #225 remains OPEN historical interface transport; its producer integration prerequisite is met, but the issue itself is not the lease. No closure/rewrite is required here.

## Architecture/source scope

Five Persistence source files: `FileProductionCatalog.cs`, `FileProductionEventStore.cs`, `ProductionPersistenceVersionPolicy.cs`, `ProductionProjectionSnapshotCache.cs`, new `Utf16CodeUnits.cs`.

Five Persistence test files: new `WorldCurrentPersistenceTests.cs` and existing catalog/event-store/snapshot/export tests (future creation versions advanced to v3, new-write golden payload advanced to v2, snapshot fixtures advanced to v2).

No Application source/test, UI, journal coordination, portable-export runtime, provider, build or fixture format source changed outside this named surface. Existing identity metadata and portable identity encoding manually use big-endian UInt16; snapshot already did so for ProductionName. These were verified before mutation.

Catalog known-ID resolution remains non-creating. It implements the writer, appends once through `AppendValidated`, then loads committed authoritative replay. Existing compatibility/corruption/environment distinctions and explicit recovery remain. Tests compare event count and preceding record hashes after each successful call. Failed validated append preserves files; same-root gate contention throws IOException and another Production remains independent.

Creation v2 and replacement v1 use Base64 over manual big-endian code units. Old creation v1 decode remains; v3 creation and v2 replacement fail as Incompatible. Malformed Base64, odd byte counts, missing/duplicate/unexpected properties and invalid reconstructed Application states fail closed. A new snapshot version 2 contains name plus all truths; v1 cache is expendable. Export version and journal schema remain 1. Export runtime code is unchanged.

## UTF-16 stop and correction

Original isolated native .NET 9.0.19 probe accepted D800/DC00 through actual exact-base Application constructors. Direct JSON output produced FFFD; decode and Application replay retained FFFD. Manually escaped D800/DC00 caused GetString InvalidOperationException. The sibling ProductionCreatedEvent constructor/API probe reproduced the same mismatch. This was a representation probe, not a claim that a new codec already existed.

Director subsequently retained arbitrary UTF-16 and authorized explicit lossless representation plus the sibling v2 evolution. Corrected real-journal tests assert string length and each UInt16 value for D800, DC00, D83D DE00, decomposed Unicode, embedded NUL, surrounding whitespace and ordinary Unicode. One test additionally includes every code-unit value 0000..FFFF in a single accepted string, checking both creation and replacement through reopen, snapshot and portable inspection. D800 payload golden value is Base64 `2AA=`. These are exact code-unit assertions, not rendered-text comparisons.

Existing v1 FFFD remains FFFD: previously lost surrogate identity cannot be reconstructed. V1 ordinary-name histories remain readable/exportable and accept later replacement events. No Application-domain modification occurred.

## Native validation and diagnostics

Host runtime: Windows ARM64; repository SDK 9.0.317, .NET 9.0.19. Explicit WinUI project build avoids the solution's x64 mapping.

| Check | Result |
|---|---|
| Focused consumer tests, MTP `-- --filter FullyQualifiedName~WorldCurrentPersistenceTests` | 44/44 PASS |
| Full Persistence at exact candidate | 129/129 PASS |
| Full Application, identical final source/test blobs | 57/57 PASS |
| Core regression, identical final source/test blobs | 628/628 PASS |
| Explicit WinUI Release `-p:Platform=ARM64 -r win-arm64`, identical final runtime/build blobs | PASS, 0 warnings / 0 errors |

Commands: `dotnet test <project> -c Release -r win-arm64` for `tests/Kymaean.Infrastructure.Persistence.Tests`, `tests/Kymaean.Application.Tests`, `tests/Ensemble.E0.Core.Tests`; `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`.

Initial outcomes are preserved rather than hidden:

1. First-turn restricted Git networking and scratch writes were denied. Exact read succeeded with approved access; probe used TEMP. Two probe build/restore attempts were blocked reading user NuGet.Config; package-free approved restore/run succeeded. No Product tests had executed in those setup failures.
2. First implementation test invocation used top-level `--filter`; MSTest's platform integration ignored it and executed all 124 tests. Result: 122 passed, 2 failed. Both failures were synthetic v1 snapshot fixtures still expected by the updated assertion to be v2 (`PlausibleSnapshotCannotMaskReplayInvalidJournal`, `PlausibleSnapshotCannotMaskIncompatibleJournal`). The primary updated fixture frame version and empty-world count; no runtime repair was needed for these failures. Initial log preserved locally in TEMP `qprod01-consumer-validation-20260921/initial-124-two-fixture-failures.log`.
3. Correct MTP pass-through filter ran 43/43; full regression 128/128; the final all-code-unit test then raised focused/full totals to 44/129. Application/Core/build remained source-identical. No unresolved nondeterminism observed.
4. Test help generated one empty TestingPlatform first-use sentinel under the checkout. Its exact path/content were verified and the generated empty file/directories removed; no user evidence was removed.

## Guards, review and handoff

Pre-continuity repository law PASS; document census 511 total / 390 current / 30 historical / 91 archive / 0 unexplained, PASS. Exact base-to-candidate independent review found no actionable findings. Effective reviewer policy was workspace-write / auto_review, so technical write containment was unproven; the reviewer performed no writes, Git mutations, escalation, providers or test/build execution. Primary evidence includes the final all-code-unit test. Staged continuity census PASS: 513 total / 392 current / 30 historical / 91 archive / 0 unexplained; repository law and diff hygiene PASS. Candidate protected-diff PASS with no protected runtime changes. Final clean commissioning closeout and hosted checks are separate exact-head evidence; none authorize merge.

All 14 live heads were resolved. Main is the base above; 12 non-main refs are ancestral integrated/historical continuity or E0 evidence; preserved producer `dc1780e46ef999eb01e214671486b83186ca04c4` is diverged historical evidence, PR #226 closed unmerged. Local worktree registry was inspected and all inherited worktrees preserved; this package does not certify their individual owner status or release them. No active consumer branch existed in the live census, and no concurrent local source change was observed.

Concurrency remains one journal operation per Production root, with append and replay separately gated. This package does not claim a global transaction, same-root transparent queue/retry, or general power-loss guarantee. The governing provisional policy remains `docs/evidence/Q_PROD_01_FINAL_PROVISIONAL_STORAGE_CONCURRENCY_POLICY_NATIVE_ARM64_2026_09_18.md`.

`DESIGN_ARCHITECTURE_READY = NOT READY`: this candidate is not integrated, and consumer integration/reassessment remains required. Exact successor is Director disposition of the returned draft; after authorized integration/exact-main checks, explicit readiness reassessment. Recommended model/effort: gpt-5.6-sol / high, per current tooling guidance. No UI/provider/deferred-E0/final-architecture/Alpha/release/WACK/Store authority is earned.
