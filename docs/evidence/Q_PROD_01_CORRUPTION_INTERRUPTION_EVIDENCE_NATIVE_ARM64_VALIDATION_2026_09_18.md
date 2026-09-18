# Q-PROD-01 Broader Corruption / Interruption Evidence — Native ARM64 Validation

Date: 2026-09-18

Status: **INTEGRATED EVIDENCE CHECKPOINT / EXISTING RUNTIME LAW CONFIRMED**

## Identity

- Lease: `ENG1-QPROD01-CORRUPT-05` / Issue #217.
- Exact validated base: `7d050a4a8330514a8b126718dd3fe8bb9332b506`; push-triggered Validation #1043 PASS.
- Final exact native evidence source: `a77170e99daa614c89c7949baab1ad959ffe64bc`.
- Validation tag: `validation/q-prod-01-corruption-interruption-evidence-native-arm64`.
- Source scope: exactly one Persistence test file; no production source changed.
- Integrated by PR #218 to `main@70b832ff61195a4818282b838716c6e92689fff7`.
- Branch Validation #1044 PASS.
- PR exact-head Validation #1045 PASS.
- Push-triggered exact-main Validation #1046 PASS.

## Exact source scope

Exactly one path changed:

- `tests/Kymaean.Infrastructure.Persistence.Tests/FileProductionJournalTests.cs`

No Application, Persistence runtime, Windows, provider, deferred-E0, Design, workflow, tool or shared-authority production source changed in the exact evidence commit.

## Newly exercised interruption / corruption states

The checkpoint adds non-duplicative fault-state evidence around the already-earned journal/recovery model:

1. A truncated committed journal entry fails closed on ordinary reopen and on explicit `Recover()`; the committed head is not silently advanced or repaired.
2. A truncated committed head fails closed on ordinary reopen and on explicit `Recover()`; recovery does not silently reconstruct authority from a structurally corrupt committed head.
3. A checksummed but impossible head sequence that points beyond available committed history fails closed on reopen and recovery without rewriting the head.
4. A crash suffix containing a valid uncommitted entry followed by a structurally corrupt entry is all-or-nothing: explicit recovery fails and does not partially advance the committed head to the valid prefix.
5. Arbitrary stale pending-entry and pending-head artifacts are ignored by ordinary reads and never become committed causal history. Existing recovery coverage separately proves stale pending cleanup without invention.

These tests supplement—not replace—the existing evidence for:
- committed payload/hash corruption;
- missing committed sequences/final entries;
- unsupported entry/head schema versions;
- missing-head explicit recovery;
- semantic event incompatibility and replay-invalid history;
- snapshot corruption/staleness/interruption;
- portable-export corruption/version/framing;
- process reopen/reconstruction and semantic recovery.

## Result

No production defect was exposed.

The existing Persistence implementation already satisfied every newly injected fault state. Therefore the correct outcome for this evidence rung was to leave runtime source unchanged rather than add fault-injection hooks or alter recovery semantics.

This checkpoint confirms the current fail-closed/all-or-nothing behavior at the tested boundaries. It does **not** convert those tests into general power-loss certification, filesystem durability proof across all hardware, or a final multi-process concurrency guarantee.

## Exact-source native Windows ARM64 validation

At exact source `a77170e...` on SurfSeven:

- Kymaean.Infrastructure.Persistence tests: **85/85 PASS**.
- Kymaean.Application regression: **38/38 PASS**.
- Kymaean.Windows Release `win-arm64`: **PASS, 0 warnings / 0 errors**.
- repository law: PASS.
- document census: **345 inventory / 224 current / 30 historical / 91 archive / 0 unexplained**.
- oracle guard: **288 documented / 17 asserted / 271 document-only**.
- diff hygiene, clean worktree, owned-scope and unchanged-main race: PASS.

One local WinUI build attempt encountered a transient file lock from a concurrent/stale XAML compiler process. After that process cleared and the worktree intermediates were rebuilt cleanly, the same exact source passed Release `win-arm64` with 0 warnings / 0 errors. This environmental retry did not cause a source change.

## Hosted validation

- Branch Validation #1044: PASS, all eight jobs.
- PR #218 exact-head Validation #1045: PASS, all eight jobs.
- Integrated `main@70b832ff61195a4818282b838716c6e92689fff7` push-triggered Validation #1046: PASS, all eight jobs.

## Earned authority

This checkpoint earns evidence that the currently implemented Persistence journal/recovery model remains fail-closed/all-or-nothing for the newly covered torn-entry, torn-head, impossible-head, mixed crash-suffix and pending-artifact states.

It does not change Product semantics or storage format.

## Non-authority

This checkpoint does not establish:

- final storage-location policy;
- final in-process or multi-process concurrency policy;
- full filesystem/power-loss certification;
- guarantees for every hardware/filesystem write-cache behavior;
- Product import/restore/create/rename/delete lifecycle;
- richer Studio/Stage/Archive persisted ontology;
- provider behavior or credential semantics;
- WACK/Store/release authority;
- deferred-E0 conclusions.

Per the active Q-PROD-01 charter, Engineer #1's next Persistence rung is **final provisional storage/concurrency policy**.

No provider traffic occurred and no deferred-E0 namespace was consumed.
