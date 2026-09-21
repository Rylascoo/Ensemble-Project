# Native Codex application workflow

Status: Director-authorized operating-model successor, 2026-09-20. Scope: Project Engineering preparation and bounded application work. Evidence: `docs/evidence/NATIVE_CODEX_APPLICATION_PREPARATION_2026_09_20.md`.

## Authority and operating model

The Director's native Codex preparation decision retires the plan for an API-driven autonomous Codex Administrator. Director + ChatGPT decide/reconcile work manually; Codex executes complete bounded packages through the ChatGPT/Codex product; Git preserves authority and GitHub remains PR/integration authority. No OpenAI API key or Agents API infrastructure is required for this development workflow. This does not remove or authorize the application's independently governed provider adapters.

This decision supersedes persistent Engineering #2/#3 management, custom automatic routing, native-subagent default-off and suspended custom-Reviewer prerequisites **for this native Project workflow**. Engineering Sol remains accountable; one primary writer owns each isolated worktree. Native supporting subagents are bounded evidence producers, not managers or independent authorities. Historical leases, failed experiments, validation, provider restrictions and preserved candidates remain evidence. No old lease silently resumes.

Preserved first-slice provenance: `docs/Q_PROD_01_FRESH_CHAT_HANDOFF_2026_09_17.md`. Preserved prior ownership/sequence: `docs/Q_PROD_01_THREE_ENGINEER_WORKSTREAM_CHARTER_2026_09_17.md`. These links retain evidence reachability, not current permission to recreate the old roles or resume their leases.

Ryladmin remains governance/recovery/evidence infrastructure. Its live installation, maintenance, C10/C11, custom Reviewer and automation gates are not promoted or retried here. Older establishment/runtime/MA documents describe that separate historical program, not prerequisites to ordinary native Codex application work. No Ryladmin, Website or Drive mutation is authorized by this package. Product constitution, Design taste, disclosure, repository residency and validation hierarchy are unchanged. Project remains the single execution queue.

Use a fresh worktree for implementation. Resolve live refs and current authority, verify exclusive ownership, then inspect -> internally plan -> delegate useful read-heavy work -> implement -> focused validation -> diagnose/correct -> required broader validation -> independent review when consequential -> final diff -> commit -> push authorized branch -> draft/update PR -> handoff. Continue routine authorized steps without repeated Director prompts. Stop for material authority ambiguity, concurrent-write collision, actual external/manual dependency, safety/validation boundary, or package completion. Merge requires the Director's authorization.

## Native configuration and supporting agents

`.codex/config.toml` sets workspace-write, on-request approval, disabled web search, command network off, and two supporting agents maximum (excluding the primary). Project layers require a trusted project and cannot override managed requirements or live session controls. This task's managed automatic approval review remains its actual policy; adding a project file does not change a running task.

Built-in `worker` and `explorer` already exist; do not duplicate them. Delegate bounded exploration, architecture mapping, log analysis and independent research when useful. Default: one writer plus read-only supporting work, no overlapping writers. `.codex/agents/reviewer.toml` defines only an exact-ref/diff reviewer: read-only sandbox, no approval escalation, no child delegation, no source repairs or authority writes. Parent/live permission overrides can supersede custom-agent defaults. Before relying on enforced read-only isolation, verify the effective child policy; if it is broader, use a separate read-only native review task and do not claim containment from this file alone. Review findings must bind an immutable commit and return to the primary writer for reconciliation.

Installed verification: Codex CLI `0.155.0-alpha.9.2`. Current session native delegation works without an API-key runtime. Supported settings and standalone-agent fields were checked against official documentation and the installed parser; see evidence for verification limits. Keep model selection in task/config/UI, not architecture. Recommended next producer adoption: **gpt-5.6-sol, high**, available in this host's current model menu. Reverify when starting; supporting agents inherit the selected model/effort unless explicitly configured otherwise.

Sources, retrieved 2026-09-20: [configuration](https://learn.chatgpt.com/docs/config-file/config-reference), [subagents](https://learn.chatgpt.com/docs/agent-configuration/subagents), [local environments](https://learn.chatgpt.com/docs/environments/local-environment). The installed model menu supplies the exact `gpt-5.6-sol` identifier; generic documentation labels are not an account-availability guarantee.

## Application environment and Desktop actions

In Desktop Settings, create/select the local environment **Ensemble Application** for this repository root. Select **Worktree** when starting implementation. Set the Windows setup command to:

```powershell
pwsh -NoProfile -File tools/verify-application-environment.ps1
```

Desktop owns generation of its local environment/action file under `.codex`; do not invent its schema. Save the UI-generated file and inspect it before checking it into Git. This package supplies commands, not a claim that the UI environment/actions are already configured. Existing saved-project paths must resolve to the real Project repository; a project label alone is insufficient.

| Action | Windows command |
|---|---|
| Verify Application Environment | `pwsh -NoProfile -File tools/verify-application-environment.ps1` |
| Focused Tests | `pwsh -NoProfile -File tools/test-application.ps1` |
| Full Application Tests | `pwsh -NoProfile -File tools/test-application.ps1 -Full` |
| Build Application | `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64` |

Focused Tests defaults to Application; pass `-Filter <exact MSTest filter>` for a narrower task. Full Application Tests includes Application and Persistence sequentially and fails at the first failure. Add Core/Harness regressions when the change reaches those boundaries. No packaging action is admitted at this phase. Actions run in Desktop's terminal and do not themselves enforce the agent sandbox or create provider authority.

The readiness script is cheap/offline: checks Windows/native ARM64, repository-selected SDK, Windows SDK tools, solution/project references, and reports pinned packages and restore assets. It does not install, restore, build or test. Actual repository stack: `global.json` .NET 9.0.100/latestFeature (CI 9.0.317); WinUI `net9.0-windows10.0.26100.0`; WindowsAppSDK 2.5.1, Windows.SDK.BuildTools 10.0.28000.2705, BuildTools.WinApp 0.6.1; MSTest.Sdk 4.1.0. No extra .NET workload is presently required by the project files. `Ensemble.sln` Any CPU maps Windows to x64, so the build action must name the ARM64 project explicitly.

On a fresh machine, manually install ARM64 PowerShell 7, ARM64 .NET 9 SDK compatible with global.json (9.0.317 is the CI version), Windows SDK 10.0.26100.0 with ARM64 tools, and Git. Use Python 3.12+ or the Codex bundled Python for repository guards. No machine-wide toolchain upgrade is part of setup. Current host has the build prerequisites; Python is not on PATH, so this task used the bundled interpreter.

First test/build restores NuGet packages explicitly and may require task-scoped network approval. Versions are pinned in projects, but there is no package lockfile, so do not claim fully locked dependency resolution. Missing assets require restore, not a machine install. Cache presence/asset presence does not prove build success. Native UI launch/runtime registration and actual-device acceptance remain separate; no new Windows App Runtime deployment, WACK or Store readiness is claimed.

## Exact next Application package

**QPROD01-NATIVE-PRODUCER-01: adopt and revalidate the corrected World-current-truth producer. Disposition: RESUME.** This is the next bounded Product package, not executed by this setup PR. Start from freshly resolved Project main after setup integration; record the actual base SHA and claim one exclusive primary-writer worktree. Examined baseline is `daf5f541bb9caa73e249aa8a0ba219b34730a206`; producer is `dc1780e46ef999eb01e214671486b83186ca04c4` with merge-base `c8da0c1f42a40d7e3f492e0e76b343d648c69bd6`. Do not reuse the old producer worktree or move its ref automatically. Reconcile its exact seven-file Application/test delta onto the new baseline and preserve original validation provenance. Refresh stale PR #226 metadata only as part of that later authorized PR operation. A successor candidate may carry the same reviewed delta; that is not rejection/reconstruction of the semantic contract.

Acceptance: immutable canonical `WorldCurrentTruth` state, creator-specific replacement event, old histories replay to empty state, deterministic ordered replay, null/duplicate rejection, typed failure preserves prior usable projection/navigation, no automatic Character knowledge or UI/provider change. Resolve any conflict with frozen Product law before implementation; original Blueprint text was not newly located by this setup audit, so the recovered refined authority audit must not be described as a fresh read of that original.

Run native ARM64 Application tests, Persistence/Core regressions, explicit ARM64 WinUI compile, repository law/census/oracle/diff/race checks, independent immutable-ref review and exact-head hosted checks. Preserve exact source evidence/tag for any new validation claim. Return a draft/reviewable candidate; do not merge without authorization. This future package's invocation establishes its exclusive lease; setup does not reopen the old Engineer #2/#3 leases.

After producer integration, the first new implementation is the **World-current-truth Persistence consumer** described by corrected Issue #225: typed event encode/decode, known-Production writer, exactly one append and authoritative replay, cache preservation and raw portable-export replay. Current catalog does not implement the writer; event store only encodes creation events; snapshot cache contains only ProductionName. Separate bounded lease from fresh integrated main; no UI/provider work. Preserve typed `Incompatible`/`Invalid`/environmental failure distinctions, no auto-recovery, exact accepted string identity and existing concurrency policy. If the producer's permitted UTF-16 domain makes consumer encoding ambiguous, return that exact contract question before choosing a new domain rule.

Home A/B and FIRSTUSE remain unresolved/pending and do not block this nonvisual producer slice. `DESIGN_ARCHITECTURE_READY = NOT READY` until consumer integration/reassessment. Provider traffic, deferred E0, final architecture/Alpha/release/WACK/Store and later UI work retain their independent gates. `docs/design/app/AUTHORITY.md` remains the Design entry point; its inherited closeout-pending/Administrator-next-action prose is superseded by the exact Engineering reconciliation here, not by edits to frozen receipt or Design contracts.
