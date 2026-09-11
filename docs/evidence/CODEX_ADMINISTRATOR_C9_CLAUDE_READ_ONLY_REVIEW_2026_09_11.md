# Codex Administrator C9 Claude Read-Only Review — 2026-09-11

Status: **PASS — C9 mechanical commissioning evidence only**

## Authority and boundary

- Queue item: `Q-ADMIN-02`.
- Governing contract: `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.
- C9 requirement: commission Claude under a restricted independent-review posture, prove subscription/billing/auth boundaries, and complete one exact-SHA bounded review.
- Exact reviewed Project source: merge `e9511cacb2a58e59aa18595248bdf9ea16d7c435`.
- Project closeout baseline after concurrent Engineering advance: `fdfd5c69f6d439fd9cdc738be52366a3309b9597`.
- Claude is advisory review only; C9 creates no Engineering, provider, product, Design, ODR, experiment, validation, or later-gate authority.

## Installation provenance

Claude Code was absent from `PATH` at C9 preflight. The existing personal `C:\Users\Wiryl\.claude` surface was not reused or inspected for credentials.

WinGet resolved official package `Anthropic.ClaudeCode` version `2.1.267`, publisher Anthropic PBC, native `win32-arm64`, release date 2026-09-09. WinGet verified the downloaded installer hash before installation.

Installed executable alias: `C:\Users\Wiryl\AppData\Local\Microsoft\WinGet\Links\claude.exe`.

Measured executable SHA-256: `0DC306259E3036AF4255F66B77451D3F7297BCD376BFAE20F7B42E2FA1473607`, matching the WinGet package installer SHA-256. `claude --version` returned `2.1.267 (Claude Code)` and `claude doctor` reported `win32-arm64`, WinGet management, and no installation issues.

## Subscription, authentication, and billing boundary

Dedicated Claude configuration root: `C:\Users\Wiryl\.claude-ensemble` via `CLAUDE_CONFIG_DIR`. It did not exist before C9 and is separate from the personal Claude configuration.

Before login, `claude auth status --json` reported `loggedIn=false`, `authMethod=none`, `apiProvider=firstParty`. `doctor` reported no usable credentials for remote settings/policy lookup and no active claude.ai subscription authentication.

The Director completed the normal `claude auth login` browser flow. Console/API login was not selected. After login, the dedicated profile reported `loggedIn=true`, `authMethod=claude.ai`, `apiProvider=firstParty`, `subscriptionType=pro`.

Immediately before review, the process explicitly removed Anthropic API/auth-token and supported cloud-provider credential-selection environment variables. No `ANTHROPIC_API_KEY` was present.

Current Anthropic public guidance was reverified on 2026-09-11: Pro includes Claude Code in the same subscription allocation; Claude Console/API billing is separate; an `ANTHROPIC_API_KEY` environment variable overrides subscription authentication and causes API pay-as-you-go charging; switching to API credits requires explicit user choice. C9 created no API key, Console credential, Bedrock/Vertex/Foundry route, or automatic credit transition.

The review JSON contains `costBasis="list"` model-usage telemetry and a `total_cost_usd` list-equivalent field. Those fields are recorded as telemetry, not evidence of an API charge. The active authentication route and current Anthropic billing guidance establish that this invocation used the Pro subscription allocation.

## Restricted review posture

The single review ran from `C:\Users\Wiryl\.claude-ensemble\scratch\c9-review`, outside every Project worktree. No repository checkout was exposed as working context.
The invocation used:

- `--tools=""` — all built-in tools disabled;
- `--strict-mcp-config` — no inherited MCP surface;
- `--setting-sources user` with the dedicated configuration root;
- `--permission-mode plan`;
- `--permission-prompts none`;
- `--no-session-persistence`;
- `--output-format json`;
- `--effort high`;
- no explicit model name and no fallback model.

The source packet was generated deterministically from exact Project merge `e9511cacb2a58e59aa18595248bdf9ea16d7c435`: its C8/C9 commissioning-contract excerpt plus the exact first-parent diff for the five C8 closeout paths. Packet SHA-256: `20633420193345E6FAEA251F398D52A144B6C4FE1E4CCA90B881AE26A941DEEA`; size `21,359` bytes.

Claude completed one turn. Output JSON SHA-256: `8059F527A08C7DDAB80523CB8B36D3A7DB9B24A85F2CDA14DA08645FA529E590`; size `8,508` bytes. The result reports `provider=firstParty`, zero web-search requests, zero web-fetch requests, zero spawned subagents, zero permission denials, `num_turns=1`, terminal reason `completed`, and no API error.

No Claude tool could read/write files, execute shell commands, access Git, call MCP, browse the web, mutate authority, or inspect newer Engineering state. The review saw only the fixed packet.

## Independent review result

Claude returned **`PASS_WITH_FINDINGS`**, not a clean PASS. Its authority analysis agreed that the C8 closeout did not create authority beyond C8 and that C9 was the next gate only.
Its material findings were:

1. **HIGH** — the original C8 evidence did not directly demonstrate cross-origin storage isolation;
2. **MEDIUM/HIGH** — launch flags alone did not directly demonstrate that permission requests were denied/bounded;
3. **MEDIUM** — the rejected non-Guest profile's navigation-versus-account-audit sequencing was ambiguous;
4. **LOW/MEDIUM** — the original cookie proof was post-shutdown rather than live-session;
5. **LOW** — Guest-profile persistence/re-derivation semantics were not explicit;
6. **LOW** — the original accepted CDP port was not repeated in the discovery paragraph;
7. **LOW** — browser-version/policy assertions could not be independently verified from the packet.

The Administrator manager adopted findings 1–6 as worthwhile corrections and treated finding 7 as an explicit review limit rather than a contradiction.

## Manager reconciliation and C8 repair

C8 was reopened narrowly for browser-only falsification. A fresh Guest context on loopback-only CDP `9225` proved cross-origin storage separation between `http://127.0.0.1:8766` and `http://localhost:8766`: A stored marker `A`; B initially observed `null`, stored `B`; returning to A restored `A`. Live cookie count remained `0` throughout.

A second fresh Guest context on loopback-only CDP `9226` used Chromium's deterministic `--deny-permission-prompts` switch. An actual geolocation request returned denied/code `1`; an actual notification request returned `denied`; both Permissions API states were `denied`. Live cookies were `0`, active Guest account entries were `0`, Login Data was absent, and post-close cookies/accounts/logins were `0`/`0`/absent. Ordinary Edge remained running; correction roots and local fixture were removed after measurement.

The C8 record now explicitly states that the first non-Guest attempt had navigated local test content before the account metadata audit discovered one OS-associated account entry. That attempt is rejected evidence; no identity value was read. Future bounded browser work must fresh-audit the dedicated Guest context rather than infer cleanliness from persistence.
## C9 result and successor

C9 demonstrates that Claude can serve as an independent advisory review plane through a dedicated Claude configuration, Pro-subscription authentication, explicit no-API-key billing boundary, and a tool-less/no-MCP/no-session-persistence exact-SHA review.

The review's adverse findings were useful and changed the evidence state: C8 was strengthened rather than the reviewer being retried or overruled. No second Claude review was sent. This preserves independence and satisfies the one-review commissioning objective without turning review into a consensus loop.

**C9 mechanical recommendation: PASS.** Claude remains advisory; owning Sol/Administrator reconciliation is still required before any recommendation changes repository authority.

C10 is the sole next earned Administrator gate after durable C9 promotion. Its Runtime Specification contract is one deterministic protective Hook pilot: prove the expected event fires, a forbidden disposable action is blocked before execution, an allowed action still works, hook change invalidates prior trust where applicable, hook failure fails closed, and no authority rewrite occurs. Automations remain blocked until C11.
