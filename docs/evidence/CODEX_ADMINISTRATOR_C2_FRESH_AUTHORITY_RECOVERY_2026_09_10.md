# Codex Administrator C2 Runtime Capability Correction

Date: 2026-09-10

Status: **ACTIVE ADMINISTRATIVE COMMISSIONING EVIDENCE — C2 PASSED; C3 EARNED NEXT AFTER DURABLE CLOSEOUT**

## Authority and scope

This record is subordinate to `CURRENT_STATE.md`, `docs/PROJECT_AUTHORITY.md`, and the Director-approved `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.

C1 is durably closed on `main@fcbf07be5073e34df6dc80ad471d7a12728dc650`. C2 was the only earned Administrator gate when this commissioning work began. This record closes C2 only; it changes no Engineering phase, validation rung, provider authority, Design authority, or product law.

Predecessor chain: `docs/evidence/CODEX_ADMINISTRATOR_C1_REPOSITORY_RECONCILIATION_2026_09_10.md` -> `docs/evidence/CODEX_ADMINISTRATOR_C0_COMMISSIONING_CLOSEOUT_2026_09_10.md` -> `docs/evidence/CODEX_ADMINISTRATOR_RUNTIME_SPEC_POST_APPROVAL_TRANSITION_2026_09_09.md`.

## Fresh C2 failure

A genuinely fresh Administrator thread `01a08a07-3f59-7351-a041-a84058440be4` received only the C2 acceptance task, not prior chat facts. It failed closed: local PowerShell/cmd process creation was rejected by policy, root `AGENTS.md` and the Runtime Specification could not be read, and no callable GitHub repository/CI tool was exposed under the then-current app-deny configuration.

The thread correctly returned `C2 FAIL`; it did not invent main/ref, queue, validation, provider, worktree, or CI truth and did not claim C3.

Earlier launcher attempts that stopped before model execution are not C2 attempts. They exposed two host-interface defects: successful native Codex stderr could terminate the PowerShell launcher under `$ErrorActionPreference="Stop"`, and Remote Desktop leaves stdin open unless the one-shot prompt is provided through a finite redirected input file. The native-stderr defect was corrected before the fresh thread above; finite stdin is a commissioning harness requirement, not project authority.

## Recursive runtime audit findings

The C2 failure triggered the Runtime Specification section-31 audit rule. The approved design itself remains valid; its executable realization was incomplete.

1. `allow_login_shell=false` was required by the approved base security contract but absent from both dedicated base config and launcher. Current Codex defaults this setting to true, so C0 did not fully materialize its stated posture.
2. Native Windows sandbox mode was absent. `codex doctor` therefore reported the sandbox backend as disabled. With `windows.sandbox="elevated"`, the pinned binary identifies the intended backend but reports provisioning incomplete.
3. The C0 evidence/manifest value `marketplace_plugins=0` is not safe as current executable truth. `codex plugin list` under the dedicated home currently reports nine installed/enabled host/account plugins. Their installation is not treated as Administrator admission.
4. The remote plugin catalog is unnecessary for the Administrator and materially increases irrelevant tool/context exposure. Current Codex documents `features.remote_plugin` as the remote catalog switch.
5. C2 demonstrated a concrete need for the GitHub authority/CI connector already contemplated by Runtime Specification section 19. The installed `github.fetch` tool advertises `readOnlyHint=true`, `destructiveHint=false`, `openWorldHint=false` and can retrieve approved GitHub GET resources, including repository files, branches, workflow runs and subresources.

## Least-privilege correction materialized locally

Secret-free backup: `C:\Users\Wiryl\.codex-ensemble\backups\C2-runtime-capability-correction-20260910T065506Z`.

Dedicated runtime after correction:

- Codex `0.153.4` executable SHA-256 `77F792476FE0DEF726503F02A7C55F485E562DD7AD8801FE61DC8F4BF9991D20` unchanged;
- `config.toml` SHA-256 `30684DE493FACB0AD7FDF5EABAA1B4E0691091F9AFEF1A65360B19369BD4A466`;
- `bin/ensemble-admin.ps1` SHA-256 `8D04219DF081D65292F2694BE892B853943C25DD3658246B33302F12085D8CED`;
- `state/runtime-manifest.json` final post-provisioning SHA-256 `1C484B267A8AEA8542571D572DBDC8EBCD4003AC941A719D823A88127EA5CC7D`, census `2026-09-10T17:55:17.624385Z`; pre-provisioning hash was `6480EB348E98DDABEF88F0D088920E86757B25DE2D8FD2CD7BCDAAC46EBBCDA3`.

Enforced changes:

- normal Administrator remains `sandbox=read-only`, `approval=on-request`, reviewer=user;
- `allow_login_shell=false`;
- `windows.sandbox="elevated"` selected;
- command web search remains disabled and command-network policy is not broadened;
- `remote_plugin=false`;
- global app default remains disabled;
- only connector `connector_76869538009648d5b282a4bb21c3d157` (GitHub) is enabled for Administrator use;
- that connector has `default_tools_enabled=false` and only `fetch` enabled with approval mode `auto`;
- open-world and destructive GitHub app tools remain denied;
- browser/CDP/computer-use, Hooks, memories, goals and multi-agent execution remain disabled.

Both Project and Website launcher preflights pass with the corrected runtime.

## Capability falsification

A scratch-only read-only capability probe, thread `01a08b25-0938-72a0-a12f-c6c116ee00f4`, proved that GitHub `fetch` remains callable even with `remote_plugin=false` and recovered `main@fcbf07be5073e34df6dc80ad471d7a12728dc650` from GitHub.

That probe also proved changing approval policy to `never` does **not** solve local shell execution while native Windows sandbox provisioning is absent. Therefore approval policy is not the root cause and the approved normal `on-request` model is preserved. The scratch probe is not counted as C2 acceptance.

The Director then completed the supported elevated sandbox setup for the dedicated Codex home. Post-provisioning `doctor` reports backend `elevated`, filesystem sandbox `restricted`, network sandbox `restricted`, approval `OnRequest`, and provisioning `complete`. ChatGPT authentication remains configured with stored API key `false`.

## Accepted fresh C2 rerun

After provisioning, a brand-new Administrator thread `01a08c39-01e7-7161-9deb-ce9665479fac` received only the C2 acceptance task and bootstrap procedure, not recovered project facts. It resolved authoritative `main@fcbf07be5073e34df6dc80ad471d7a12728dc650`, read root `AGENTS.md` through EOF (175 lines) and the Runtime Specification through EOF (842 lines), then independently recovered state currency, queue state, validation/provider authority, the complete local Project worktree topology, and exact-main CI.

It identified Validation gate run `34445188901` / run number 587 as completed `success` at exact SHA `fcbf07be5073e34df6dc80ad471d7a12728dc650`, while preserving the distinction between CI/compiler evidence and native Windows ARM64 validation authority at `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`. It returned `C2 PASS`.

The fresh run treated the dirty C2 commissioning worktree as non-authoritative, preserved all detached evidence/validation worktrees, reported provider authority `NONE`, and did not ask the Director for repository/tool-resolvable facts.

## Closeout regression

On the unchanged executable tree, local .NET 9.0.317 closeout regression passed all required builds with zero warnings/errors, Core **622/622**, and E0-E deterministic tests **10/10**. These checks do not promote native validation authority.

## C2 judgment

C2 **PASSES**. The accepted rerun proves fresh Administrator authority recovery under the commissioned read-only runtime after native Windows sandbox provisioning. No source mutation, provider traffic, validation promotion, Hooks, Automations, Claude, browser/CDP, Design/ODR adjudication, or later-gate enablement occurred.

**Only C3 — Deterministic Skills — is earned next after this closeout is durably promoted. C4 and later gates remain blocked.**
