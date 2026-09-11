# Codex Administrator C9A Read-Only Claude Orchestration — Director Amendment

Status: **DIRECTOR APPROVED — C9A MAY BE COMMISSIONED; C10 REMAINS FALSIFIED; C11+ REMAIN BLOCKED**

Date: 2026-09-11
Owning authority: Director
Repository baseline recovered for this amendment: `Rylascoo/Ensemble-Project` `main@5a78da04e7a3b7f8f80c0a30e3e07136ae9e3b1c`.

## Decision

The Director approves a narrow read-only Codex↔Claude orchestration amendment to the Administrator Runtime Specification.

The amendment does **not** reinterpret or erase the C10 falsification. The commissioned Codex 0.153.4 Hook realization remains fail-open for the tested nonzero `PreToolUse` process-failure class; Hooks remain disabled. Ambient/newer Codex versions remain governed by the existing upstream-capability revalidation trigger.

The amendment also does **not** pass, bypass, or partially satisfy C11. General Administrator Automations remain disabled. Any automation capable of repository/worktree/Git mutation, authority mutation, provider traffic, deployment/publication, branch disposal, or another external project side effect remains behind C10 and later explicit admission.
## Rationale

C9 already proved a safe independent Claude Code review plane: dedicated `CLAUDE_CONFIG_DIR`, first-party `claude.ai` Pro authentication, no API-key route, fixed packet, no built-in tools, no inherited MCP, no session persistence, and advisory-only output.

C10 then falsified a different security property: Codex `PreToolUse` hook-process failures do not fail closed in the commissioned realization. That defect is material for protective enforcement around mutation, but it does not create a capability need for a Claude reviewer that is structurally unable to mutate project state.

Blocking all useful Codex→Claude orchestration on that unrelated upstream Hook defect would therefore couple two separable risk classes. The approved correction is to add **C9A — Read-Only Claude Orchestration** after C9 while leaving C10 and C11 unchanged as the gates for protective Hooks and general Automation admission.

## C9A permitted surface

A commissioned C9A dispatcher may only:

1. accept a bounded review request whose authority and exact repository/ref inputs have already been recovered;
2. construct a deterministic fixed review packet in Administrator scratch outside project worktrees;
3. record packet provenance, exact refs, size, and SHA-256 before dispatch;
4. invoke the dedicated Claude Code realization using the C9 authentication/billing boundary;
5. capture one structured advisory result plus actual runtime-selected provider/model telemetry when available;
6. hash/seal the result and return it to the owning Sol/Administrator for reconciliation.
## C9A mandatory isolation

Every C9A invocation must preserve all of the following:

- project repositories/worktrees are read-only to the review plane;
- no Git mutation, push, PR mutation, tag/branch lifecycle, or authority-file write;
- Claude built-in tools disabled;
- inherited MCP disabled/empty;
- Claude subagents disabled/not invoked;
- session persistence disabled;
- no browser/CDP surface;
- no product-provider credentials or provider execution;
- `ANTHROPIC_API_KEY` and alternate cloud-provider credential routes absent;
- no automatic implementation or action on findings;
- no background, scheduled, polling, or self-triggering behavior;
- one invocation maximum per packet unless a later explicit corrected-attempt decision identifies a changed hypothesis or malformed mechanical input.

A missing/expired Claude subscription login stops for supported interactive reauthentication. It must never fall back to API-key billing automatically.

The dispatcher may write only its bounded Administrator scratch packet/result/telemetry. Those writes are execution evidence, not project authority.

## C9A commissioning pass condition

One harmless exact-ref pilot must prove deterministic packet construction, pre-dispatch hashing, isolated Claude invocation, structured result capture, no repository/Git mutation, no unauthorized tool/MCP/subagent/browser use, no API-key/cloud-provider billing route, bounded single-attempt behavior, result hashing, and manager reconciliation without automatic implementation.

A C9A pass commissions only this on-demand advisory review path. It does not enable Hooks, C11 Automations, mutation, scheduled/background execution, or any product/provider/validation authority.
## Gate relationship

The commissioning relationship becomes:

```text
C0 -> C1 -> ... -> C9
                  |\
                  | -> C9A read-only Claude orchestration
                  |
                  -> C10 protective Hook pilot -> C11 general Automation admission -> C12 -> C13
```

C9A depends on C9 but **not** on C10. C9A does not satisfy C10. C11 continues to require a valid C10 pass. A future C10 pass does not automatically admit mutation-capable automation; any such capability still requires the Runtime Specification's later admission rules and explicit Director authority.

## Director operating delegation

The Director also approves the operating division stated with this amendment: the Administrator should handle repository inspection, deterministic mechanics, evidence/hash/CI work, packet generation, Claude dispatch, reconciliation, cleanup, and continuity whenever already authorized. The Director remains responsible for consequential product/policy decisions, explicit provider/spend or external-side-effect authorization, interactive account authentication when required, and approval prompts that cannot lawfully be delegated.

No blanket/full-access permission, sandbox bypass, Hook-trust bypass, API-key bridge, third-party orchestration framework, or independent second backlog is authorized.

## Repository effect

This is an Administrator commissioning-law amendment only. It changes no Engineering phase/checkpoint, native validation rung, E0 experiment state, provider authorization, Design authority, product/ODR resolution, or application source/test/fixture behavior.

`CURRENT_STATE.md` therefore remains untouched. The central execution queue must record C9A as the next available Administrator commissioning action while preserving C10 as falsified and C11+ as blocked.