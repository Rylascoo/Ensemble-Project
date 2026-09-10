# Codex Administrator C0 Commissioning Closeout

Date: 2026-09-10

Status: **ACTIVE ADMINISTRATIVE COMMISSIONING EVIDENCE -- C0 PASSED; NO ENGINEERING PHASE / PROVIDER / VALIDATION AUTHORITY**

## Boundary and anchors

This record closes C0 -- Environment / tool / authentication census -- under `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`. It changes only Administrator runtime/continuity state. It does not change Ensemble source, provider authorization, experiment state, Design/product/ODR authority, or validation authority. C1 is the only next earned Administrator gate.

- Transition evidence: `docs/evidence/CODEX_ADMINISTRATOR_RUNTIME_SPEC_POST_APPROVAL_TRANSITION_2026_09_09.md`.
- C0 baseline tag: `commissioning/codex-administrator-c0-baseline`.
- Reviewed transition content: `6378081101788a8ea45f1631ee7ed8a2588a235f`.
- Accepted merge: `63afcfc243cca76bc0486f40020749a73c51af4b`.
- Dedicated home: `C:\Users\Wiryl\.codex-ensemble`.

## Executable and configuration provenance

Pinned executable: `C:\Users\Wiryl\AppData\Local\OpenAI\Codex\bin\02c7a9ff819938f0\codex.exe`; `codex-cli 0.153.4`; 250288944 bytes; SHA-256 `77F792476FE0DEF726503F02A7C55F485E562DD7AD8801FE61DC8F4BF9991D20`; Authenticode `Valid`; signer thumbprint `6EBE77CE17C9B78603A4932CA0E6068DD58DABBB`.

An earlier C0 census observed a different hash/size at the same path while it still reported `0.153.4`. The Desktop package did not show a completed package update explaining that replacement. The current signed binary is stable across repeated hashes; the earlier drift remains unexplained. The launcher fails closed on further version/hash drift. Although CLI `0.154.0` was advertised, no required C0 capability was missing, so the current binary was deliberately pinned for reproducibility.

| Non-secret runtime file | SHA-256 |
|---|---|
| `config.toml` | `80723724B9FEAED65086DE9F0616E11522156665D08BE648D8BAEB40418769A4` |
| `administrator.config.toml` | `4DA1696D4C0371593565E656E7C0423B4BC7031A9762A84DE709DE6CB582C8F2` |
| `AGENTS.md` | `E4D233EAC873CCF2D15A4EF81317673FA7EBB9FD1893861E4161052B7BFA82E5` || `agents/scout.toml` | `7E072EA63A29F7F09226660A7E110F428060DBB88ACC58030E85953D823892A4` |
| `agents/worker.toml` | `6533B67957999F71384D25A34993D0A422E63720363D329615742203FF7B0DC3` |
| `agents/reviewer.toml` | `6AB29BC4F5BA55EC7B5B996DDFC49A05B9D436809CF89EB68CD58B7BD13B9C40` |
| `bin/ensemble-admin.ps1` | `3DB9603DC30BECCC135EEEA301F78D39893FD9388557C8A3FCA0EF166BC82DF9` |
| `state/runtime-manifest.json` | `CB1E1F0FA5F4675DB694A4C803AE41EE9DD74748E7CC3F9EA5A43EB44DEC912F` |

The manifest contains executable/config provenance only, never queue/phase/provider/validation authority. Secret-free rollback snapshots are `backups/C0-launcher-fix-20260910T045924Z` and `backups/C0-model-manifest-correction-20260910T051530Z`; recursive census found zero backed-up `auth.json` files.

## Authentication, capability, and model posture

Dedicated ChatGPT login is confirmed: stored auth mode `chatgpt`, stored ChatGPT tokens `true`, stored API key `false`, provider credentials present `false`. Credential contents were not inspected, copied, or hashed into project evidence.

Base posture is on-request/user approval, read-only Administrator sandbox, core-only shell inheritance with default secret-name exclusions retained, one-thread ceiling, and disabled agent spawning. Browser use, external/full-CDP browser use, computer use, goals, Hooks, memories, and multi-agent execution remain disabled. `[apps._default] enabled = false`; MCP servers = 0; marketplace plugins = 0. The personal-home `cua_repl` observation did not reproduce in the clean dedicated home and is classified as personal/plugin-derived state.

Authenticated `codex debug models` returned `gpt-6-astra`, `gpt-reserve`, `gpt-5.6-sol`, `gpt-5.6-terra`, `gpt-5.6-luna`, `gpt-5.5`, and `codex-auto-review`. Initial Administrator `gpt-5.4` was absent and was corrected to `gpt-5.6-sol` / Medium. Dormant Scout/Worker/Reviewer profiles already used `gpt-5.6-sol` and remain disabled until earned gates.

Current mapping: M0 deterministic/no model; M1 not yet qualified; M2 `gpt-5.6-sol` (Administrator Medium, later Worker/Reviewer High); M3 `gpt-6-astra` scarce escalation only.

## Launcher and schema evidence

`ensemble-admin.ps1 -Check` passes for both canonical Project and Website repositories. It verifies exact root/origin, Codex path/version/hash, dedicated `CODEX_HOME`, and security overrides; it fails closed if either repository introduces `.codex/config.toml`, which would outrank the selected profile under current precedence.

A credential-free strict probe copied base config into a disposable `CODEX_HOME` and applied Administrator fields through Codex `-c` overrides under root `--strict-config`: `config.load=ok | config loaded`. Its only failure was expected missing credentials; it created no `auth.json` and was removed. Naive TOML concatenation was rejected as an oracle because open-table scope does not model profile layering.
## Warnings, scope, and host facts

`doctor --json` reports config/auth/MCP/runtime provenance healthy. Overall `warning` is explained by: (1) `rg.exe` not verified -- not admitted at C0 because Git/Python cover current deterministic needs; (2) endpoint protection detected with exclusions unverified -- no Defender exclusion added without evidence of interference. No warning caused a security relaxation.

Confirmed Remote Desktop scope plan is the canonical Project/Website repositories, their named worktree/evidence roots, and `C:\Users\Wiryl\.codex-ensemble`; the `Sol Dev` umbrella is not a permanent grant. Personal `.codex` access was C0 comparison/migration only. This is a scope plan, not a claim that connector-level enforcement was changed.

Observed `0.153.4`/PowerShell interface facts: PowerShell `$home` collides case-insensitively with read-only `$HOME`; some successful Codex status text arrives on stderr and may become `NativeCommandError`; `--strict-config` is a root option and is unsupported after auxiliary `features`/`mcp`/`sandbox` subcommands; `doctor` does not accept `--profile`; and `--version`/`sandbox -p` did not reject a fake profile, so those interfaces are not profile-resolution oracles.

## Repository preservation and C0 judgment

The historical Project root remains detached at `689655eed677b789ab3ee395f1c65b4f2cb72cc8`; five pre-existing detached validation/evidence worktrees remain untouched. Fresh fetched-origin census still shows 58 non-main remote refs plus `main`. C0 neither classifies nor disposes of them; C1 owns deterministic reconciliation.

C0 **PASSES**: provenance is pinned; dedicated ChatGPT auth is isolated with no API key/provider credentials; launcher/security/config behavior is deterministic; MCP/plugin and warnings are explained; scope plan is confirmed; later-gate capabilities remain off; and no app source, provider authority, validation rung, Design/product authority, provider traffic, automatic merge, or automatic unique-branch deletion changed.

**Only C1 -- Repository reconciliation -- is earned next.** C1 must prove deterministic repository-state classifications against real repositories and synthetic fixtures, preserve detached validation worktrees, verify remote identity, and perform zero automatic overwrite/checkout behavior.