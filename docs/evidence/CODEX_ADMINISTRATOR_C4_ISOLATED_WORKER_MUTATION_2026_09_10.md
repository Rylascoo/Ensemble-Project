# Codex Administrator C4 — Isolated Worker Mutation Closeout

Date: 2026-09-10
Status: C4 BEHAVIORAL GATE PASS — durable repository promotion required before C5 starts

## Scope and authority

C4 commissions exactly one harmless R0 mutation in a new isolated Project worktree under the Director-approved `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.

C4 does not grant product, Engineering, Design, provider, validation, experiment, merge, deletion, Hooks, Automations, Claude, browser/CDP, or C5+ authority. The owning Project `CURRENT_STATE.md` remains authoritative for Engineering/provider/validation truth.

Authoritative C4 baseline at execution: `main@6fffffba7275a8612b0d4d43cd2498c0e5513ad4`.

C4 worktree/branch: `q-admin-02-c4-isolated-worker-2026-09-10`.

## Native permission-inheritance falsification

Fresh diagnostic thread: `01a08d84-c08d-7f81-b2b0-fcd7c68e8318`.

The read-only Administrator control session enabled native multi-agent only for this bounded inheritance test. Parent Administrator, custom Scout, custom native Worker, and custom Reviewer each attempted a uniquely named harmless write. All four were denied with the read-only sandbox result `patch rejected: writing is blocked by read-only sandbox; rejected by user approval settings`.

Observed effective behavior:

| Role | Native-child write result |
|---|---|
| Administrator parent | denied |
| Scout | denied |
| Worker profile requesting workspace-write | denied |
| Reviewer | denied |

This proves the pinned Codex `0.153.4` native-child inheritance cannot safely produce a workspace-write Worker while preserving a read-only Administrator. C4 therefore uses the Runtime Specification's prescribed fallback: a separate top-level Worker execution context. The Administrator was not broadened.

## Top-level Worker runtime realization

Pre-C4 rollback snapshot: `C:\Users\Wiryl\.codex-ensemble\backups\C4-top-level-worker-20260910T225814Z`.

Pre-C4 observed runtime hashes included `config.toml` `94EFA449D531B45AC4A719DF86CB8A070D9AD8672A7AF20A91E5917260D450CF`, Administrator launcher `5B46A520EEB822A8E9CC9D0E435FD3BC61D594AAD5B0BB49DB691F2C8173A1FC`, and runtime manifest `79209C28E42BE503C9EF18360348337D83143865E2E87B20C1A2B198EB64BD7A`.

C4 added a separate Worker profile/launcher and a named `ensemble-worker-workspace` permission profile. The Administrator default remains read-only/on-request. Worker launch requires an exact isolated linked worktree, exact expected HEAD, matching Git common directory/origin, a non-detached branch, and a clean dispatch baseline; it rejects the canonical historical Project root.

C4 runtime candidate hashes after materialization:

- `config.toml`: `A3ECBDD23914D84C4A9B87FDB180366495CC76FD5214E532434312B4864F71C1`
- `worker.config.toml`: `340907F398FA5A20E3853D34679A6C1F679084189FD4175EF0D3B89EC3F8BDFC`
- `bin/ensemble-worker.ps1`: `B1F1EE5D5C55F06BD73B3D2DF9D7E3E36170BFD012CB5E11B7C26F36E139093A`
- `state/runtime-manifest.json`: `ABD909ABF3DC1E775550CE3BEF3AEFB5744E36367AA9292F97A3F0373CA48408`

Strict configuration/doctor remained healthy: elevated Windows sandbox provisioning complete, filesystem restricted, network restricted. Administrator and Worker launcher preflights both passed. Canonical-root Worker launch was rejected before Codex execution.

The Worker context uses workspace-write intent, approval `never`, command network off, its configured app/connector tool surface disabled, subagents disabled, browser/CDP/Hooks/memories/goals disabled, ChatGPT authentication, and the pinned private Python runtime. It carries no provider credential or provider authority.

## Direct sandbox diagnostic limitation

`codex sandbox -P :workspace -C <worktree>` and the custom named permission profile both denied an inside-worktree command write as well as sibling/shared-Git writes. The direct Windows sandbox runner therefore did not reproduce the normal `codex exec` workspace-root realization on this pinned build and is not used as the positive C4 Worker mechanism.

## Harmless R0 Worker package

Top-level Worker thread: `01a08d97-4561-7532-bf73-095248656d63`.

The dispatch permitted only one untracked file in the assigned C4 worktree: `.c4-worker-r0-canary.txt`, exact UTF-8 bytes `C4_WORKER_R0_OK` plus one LF, no BOM. It prohibited tracked-file edits, Git, reads/writes outside the worktree, network, provider/connector use, and C4 authority interpretation.

The Worker created exactly that file and used the pinned `ENSEMBLE_PYTHON` in isolated mode (`-I -B`) to test it. Worker result:

- verification `True`;
- size `16` bytes;
- SHA-256 `63542cdb3229264420a53c3507679b90d46480673536371aa026dcf44348bfa5`.

Worker event evidence contained one `file_change`, at the exact canary path, followed by the Python verification command. No Worker Git command was emitted. The turn completed normally.

Independent Administrator verification before cleanup found:

- bytes hex `43345F574F524B45525F52305F4F4B0A`;
- size `16`;
- identical SHA-256 `63542cdb3229264420a53c3507679b90d46480673536371aa026dcf44348bfa5`;
- tracked diff count `0`;
- `git status` count `1`, solely `?? .c4-worker-r0-canary.txt`;
- `CURRENT_STATE.md` baseline blob `3b1b1efdf4d3e8cb674ba4225c0b56a584061330`;
- `docs/PROJECT_EXECUTION_QUEUE.md` baseline blob `b8fb6550c7424a01cb155167b633aed88c77670b`;
- Worker stdout SHA-256 `09D1B5829E8F9042CAF3DD1FA6E1E5F4C352E96F92E9719EE1B52431C2C8EFB7`;
- Worker stderr SHA-256 `0BA53AA398E42EC61650562683BB4DD9C329E0ADD3D42E8083902BCF04F79989`.

The exact-content/hash assertion is attributable deterministic test evidence for the only Worker mutation. No product source changed, so a product build was not part of the R0 package itself.

## Isolation and disposal

The Worker changed no tracked file or protected authority surface and emitted no Git operation. Shared Git lifecycle remained Administrator-owned: the Administrator created the isolated branch/worktree at the exact baseline and retained all ref/worktree lifecycle control.

After sealing the R0 evidence, the Administrator removed only `.c4-worker-r0-canary.txt`. The C4 worktree returned to status count `0` at unchanged `HEAD@6fffffba7275a8612b0d4d43cd2498c0e5513ad4`. This proves clean mutation disposal before manager closeout edits.

Scout and Reviewer write isolation was already proven by the native inheritance diagnostic; both remained read-only. Review class for the harmless R0 package is mechanical executor self-check plus deterministic validation, so no independent judgment was required to validate the canary payload.

## Managed-host connector observation

Worker stderr contained an OAuth-required Cloudflare transport warning even though Worker apps/remote plugin were disabled. Follow-up under the dedicated `CODEX_HOME` reported `No MCP servers configured yet`, and `codex plugin list --json` returned empty installed and available sets. The warning therefore originates from a higher managed host integration rather than a commissioned Worker MCP/plugin surface.

The OAuth attempt failed, yielded no callable commissioned tool, and produced no repository/provider mutation. It is retained as diagnostic evidence and a runtime re-verification trigger; it is not treated as Worker authority or silently omitted.

## Authority preservation

At the behavioral C4 execution baseline, promoted native Windows ARM64 authority was `7868e5cb12a27260e288d95c248d6f846cf37701`. During closeout reconciliation, Engineering independently advanced that authority to `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`. C4 did not alter either Engineering checkpoint/provider/Design authority or that promotion; CI/compiler/test success must not be relabeled as validation-rung promotion.

The standing Gemini diagnostic authority remains Engineering-owned. No Gemini/provider request, countTokens call, generation, inference, credential access, or spend occurred during C4.

## Manager closeout checks

After the R0 canary was removed and only the five C4 closeout files were staged, the candidate passed:

- `git diff --cached --check`;
- `tools/repository-law-check.py` with every repository-law marker PASS;
- document census schema `ensemble.repository-document-census.v8`: inventory `195`, current `79`, historical `30`, archive `86`, unexplained current `0`;
- oracle guard: documented hashes `91`, asserted `17`, document-only `74`.

The first document-census attempt correctly failed because advancing Q-ADMIN-02 had accidentally dropped the explicit C2 predecessor link, orphaning C0/C1/C2/post-approval-transition evidence from current reachability. The queue row was corrected to preserve C4 -> C3 -> C2 predecessor continuity, and the complete static gate set was restarted and passed. No authority was inferred from the failed attempt.
## C4 judgment

C4 **PASSES** the approved behavioral gate:

- Administrator control remained read-only;
- native Scout and Reviewer were experimentally read-only;
- native Worker inheritance was experimentally read-only, proving the need for the prescribed fallback;
- separate top-level Worker wrote only the single bounded file inside its exact isolated worktree;
- shared Git lifecycle remained Administrator-owned;
- protected authority surfaces and tracked files were untouched by Worker;
- deterministic test evidence was attributable to the exact Worker mutation;
- Administrator cleanup restored the worktree to the exact clean baseline;
- no passing result invented authority or inflated validation.

C5 — Branch / tag / push / PR lifecycle — becomes the next earned Administrator gate only after this C4 closeout is durably adopted on Project `main`. C5 has not started.

## Promotion-baseline reconciliation

After behavioral C4 execution, Engineering advanced live `main` by two commits to `7d275d93f7aa7c9d838e1a2a0c3be6df2c6d3c6d`, including promoted native authority `cef3fc15e31192a48aa3bddd99450b65a58bd8f1`. The uncommitted C4 closeout was therefore reconstructed onto that exact live tip before commit. C4 preserved the newer Engineering/provider/validation truth and changed only Administrator continuity/index/evidence surfaces.

Post-reconstruction static gates at `main@7d275d93f7aa7c9d838e1a2a0c3be6df2c6d3c6d` passed with document census inventory `199`, current `83`, historical `30`, archive `86`, unexplained current `0`; oracle guard documented hashes `93`, asserted `17`, document-only `76`; repository law and diff hygiene PASS.
