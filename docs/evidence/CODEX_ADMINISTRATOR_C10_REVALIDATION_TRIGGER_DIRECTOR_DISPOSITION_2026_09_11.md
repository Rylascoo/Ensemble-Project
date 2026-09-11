# Codex Administrator C10 Revalidation Trigger — Director Disposition — 2026-09-11

Status: **DIRECTOR DISPOSITION — C10 REMAINS FALSIFIED; REVALIDATION TRIGGER DEFINED; C11+ BLOCKED**

## Authority and preserved result

This record captures the Director's post-falsification disposition for Q-ADMIN-02. It does not rewrite the C10 contract and does not amend the original falsification evidence.

Original immutable result: `docs/evidence/CODEX_ADMINISTRATOR_C10_HOOK_PILOT_FALSIFICATION_2026_09_11.md`. The commissioned `codex-cli 0.153.4` realization remains falsified because deliberate nonzero `PreToolUse` hook-process failure was fail-open and the underlying command executed.

The governing contract remains `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`: C10 requires hook failure to fail closed. C11 and later Administrator gates remain blocked until a compliant C10 realization passes.

## Director disposition

1. Preserve the original C10 falsification exactly as historical gate evidence.
2. Do **not** consume a redundant live C10 attempt on ambient Codex 0.154.0.
3. Keep Hooks disabled in the normal Administrator runtime and keep C11/Automations blocked.
4. Establish an upstream-capability trigger before any future C10 realization/revalidation may execute.
5. A future Codex version becomes eligible only after source/release inspection proves that `PreToolUse` process-failure paths can block tool dispatch.
6. Once eligible, commission the exact executable/version/hash as a new runtime realization and run a strengthened multi-failure-class C10 suite from a fresh disposable fixture.

## Why 0.154.0 is source-disqualified

OpenAI Codex tags `rust-v0.153.4` and `rust-v0.154.0` resolve the relevant `codex-rs/hooks/src/events/pre_tool_use.rs` content to the same Git blob: `c1baebd4f01fd1906f461fe84cb2bf0d73058bfc`.

That implementation marks generic nonzero/no-status hook failures as failed without setting `should_block=true`; its serialization-failure outcome explicitly returns `should_block: false`. A valid protocol deny remains able to block, but a hook-process failure does not fail closed.
OpenAI Codex issue #41979, `[Hooks] Add opt-in fail-closed handling for PreToolUse hook failures`, remained open during this disposition review and describes the current timeout/spawn/crash/non-protocol/malformed-output behavior as failed-hook `should_block=false` tool dispatch. The requested feature is the missing fail-closed capability, not evidence that it already exists.

The ambient SurfSeven `codex-cli 0.154.0` executable was censused only, not commissioned or tested for C10. Observed SHA-256: `DC6D744D747A50F8CAF7F08817E0CCC9B09781F6269DEC3609E2F9FBE036233D`. No Hook enablement, trust mutation, C10 fixture mutation, or runtime substitution occurred during this source review.

## Upstream-capability eligibility trigger

Before another C10 live attempt, inspect the exact candidate Codex tag/source and release notes. The candidate is **not eligible** merely because its version number is newer.

Eligibility requires concrete implementation evidence that `PreToolUse` can prevent underlying tool dispatch when the protective hook itself fails, including at minimum:

- non-protocol nonzero exit;
- process spawn/launch failure;
- timeout;
- malformed protocol output;
- event/input serialization failure or the current equivalent failure class.

The exact mechanism may be native fail-closed default behavior or an explicit supported fail-closed mode, but it must be inspectable before execution and must preserve the Runtime Specification's security property rather than weaken it.

## Strengthened C10 revalidation suite

After the eligibility trigger is satisfied and the Director/Project opens the new realization, preregister and execute at least: normal allow, valid explicit deny, deliberate non-protocol nonzero failure, spawn failure, timeout, malformed output, and serialization failure where safely reproducible through supported interfaces. Every failure-class case must prove the underlying command was not dispatched. Preserve exact version/hash/config/trust state and transcripts.

A passing future realization does not erase the 0.153.4 falsification; both remain durable regression evidence. A failing future realization leaves C10 blocked.

## Non-authority / continuity boundary

This disposition creates no Engineering, provider, spend, validation, Design, ODR, or experiment authority. It does not authorize a live C10 rerun today. External notification/watch tooling may assist discovery of an upstream change but is not Project authority and is not C11 Administrator Automations.

Until the eligibility trigger and a subsequent explicit realization/revalidation opening are both satisfied: **Hooks remain disabled; C10 remains falsified/blocked; C11+ remain blocked.**
