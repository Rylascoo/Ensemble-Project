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

Built-in `worker` and `explorer` already exist; do not duplicate them. Delegate bounded exploration, architecture mapping, log analysis and independent research when useful. Default: one writer plus behaviorally no-write supporting work, no overlapping writers. `.codex/agents/reviewer.toml` defines only an exact-ref/diff reviewer and requests a read-only sandbox, no approval escalation, no child delegation, no source repairs and no authority writes. Those settings are requested child defaults, not proof of technical containment: the Desktop smoke test widened the effective reviewer policy to workspace-write with managed escalation. Consequential review must therefore use either a separately launched native review task/session whose effective policy is verified read-only, or an explicit behavioral no-write review whose report states that technical write containment was not proven. In either case the reviewer must not edit, mutate Git, repair the reviewed ref, change authority, invoke providers or escalate. Review findings must bind an immutable commit and return to the primary writer for reconciliation.

Installed verification: Codex CLI `0.155.0-alpha.9.2`. Current session native delegation works without an API-key runtime. Supported settings and standalone-agent fields were checked against official documentation and the installed parser; see evidence for verification limits. Keep model selection in task/config/UI, not architecture. Recommended next producer adoption: **gpt-5.6-sol, high**, available in this host's current model menu. Reverify when starting; supporting agents inherit the selected model/effort unless explicitly configured otherwise.

Sources, retrieved 2026-09-20: [configuration](https://learn.chatgpt.com/docs/config-file/config-reference), [subagents](https://learn.chatgpt.com/docs/agent-configuration/subagents), [local environments](https://learn.chatgpt.com/docs/environments/local-environment). The installed model menu supplies the exact `gpt-5.6-sol` identifier; generic documentation labels are not an account-availability guarantee.

## Application environment and Desktop actions

In Desktop Settings, create/select the local environment **Ensemble Application** for this repository root. Select **Worktree** when starting implementation. Set the Windows setup command to:

```powershell
pwsh -NoProfile -File tools/verify-application-environment.ps1
```

Desktop owns generation of its local environment/action file under `.codex`; do not invent its schema. The completed smoke test generated `.codex/environments/environment.toml`; inspection found only portable relative commands and no secret, machine identifier or absolute path. Official local-environment documentation permits checking this project-root `.codex` configuration into Git for sharing, so the exact generated file is the canonical portable `Ensemble Application` configuration. Existing saved-project paths must still resolve to the real Project repository; a project label alone is insufficient.

| Action | Windows command |
|---|---|
| Verify Application Environment | `pwsh -NoProfile -File tools/verify-application-environment.ps1` |
| Focused Tests | `pwsh -NoProfile -File tools/test-application.ps1` |
| Full Application Tests | `pwsh -NoProfile -File tools/test-application.ps1 -Full` |
| Build Application | `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64` |

Focused Tests defaults to Application; pass `-Filter <exact MSTest filter>` for a narrower task. Full Application Tests includes Application and Persistence sequentially and fails at the first failure. Add Core/Harness regressions when the change reaches those boundaries. No packaging action is admitted at this phase. Actions run in Desktop's terminal and do not themselves enforce the agent sandbox or create provider authority.

The readiness script is cheap/offline: checks Windows/native ARM64, repository-selected SDK, Windows SDK tools, solution/project references, and reports pinned packages and restore assets. It does not install, restore, build or test. Actual repository stack: `global.json` .NET 9.0.100/latestFeature (CI 9.0.317); WinUI `net9.0-windows10.0.26100.0`; WindowsAppSDK 2.5.1, Windows.SDK.BuildTools 10.0.28000.2705, BuildTools.WinApp 0.6.1; MSTest.Sdk 4.1.0. No extra .NET workload is presently required by the project files. `Ensemble.sln` Any CPU maps Windows to x64, so the build action must name the ARM64 project explicitly.

On a fresh machine, manually install ARM64 PowerShell 7, ARM64 .NET 9 SDK compatible with global.json (9.0.317 is the CI version), Windows SDK 10.0.26100.0 with ARM64 tools, and Git. Use Python 3.12+ or the Codex bundled Python for repository guards. No machine-wide toolchain upgrade is part of setup. The smoke-test host resolved native PowerShell 7.6.6 at `C:\Program Files\PowerShell\7.6.6-arm64\pwsh.exe`, not a WindowsApps/MSIX path; it selected .NET SDK 9.0.317 / `win-arm64`. Current host has the build prerequisites; Python is not on PATH, so this task used the bundled interpreter.

First test/build restores NuGet packages explicitly and may require task-scoped network approval. Versions are pinned in projects, but there is no package lockfile, so do not claim fully locked dependency resolution. Missing assets require restore, not a machine install. Cache presence/asset presence does not prove build success. Native UI launch/runtime registration and actual-device acceptance remain separate; no new Windows App Runtime deployment, WACK or Store readiness is claimed.

## Current package resolution

This workflow does not carry a durable “next Application package.” That was a bootstrap-era continuity shortcut and became stale after the Q-PROD-01 sequence completed.

At every invocation, resolve the active implementation package from exact live `CURRENT_STATE.md` plus `docs/PROJECT_EXECUTION_QUEUE.md` after completing the root `AGENTS.md` bootstrap. The queue transports current sequencing; it does not override stronger Product/Design/Director authority.

When the current package is implementation-authorized:

1. start from freshly resolved `main` and record the exact base SHA;
2. use one fresh exclusive primary-writer Worktree;
3. read the package's exact current contract/evidence named by `CURRENT_STATE.md` / queue;
4. implement only that bounded scope;
5. run focused then required broader validation;
6. obtain immutable-ref independent review for consequential changes;
7. preserve native ARM64 evidence at the exact executable source;
8. push one reviewable branch/PR and stop at the merge or other genuine external authority gate.

Historical package-specific instructions below their own evidence records remain provenance only. Never resume an old producer, lease, issue, branch or worktree because this operating-model document once named it next.

Home A/B, FIRSTUSE, provider traffic, deferred E0, final architecture/Alpha/release/WACK/Store and other separately gated work remain controlled by their current authority surfaces, not by this workflow.
