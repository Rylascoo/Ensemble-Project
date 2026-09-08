# E0-A Post-E-R1 Fresh-Chat Handoff — Gemini Comparison Native / Live Gates

Status: ACTIVE FRESH-CHAT TRANSITION
Recorded: 2026-09-07
Authority: `CURRENT_STATE.md` alone carries active checkpoint/validation/next-action authority. This handoff is temporary.

## Fresh-chat bootstrap

1. Read `CURRENT_STATE.md` first.
2. Resolve `main`, active branch/head, relation to `main`, and latest Validation gate from GitHub.
3. Then read only the current boundary surfaces:
   - `docs/PROJECT_AUTHORITY.md`
   - `docs/ENGINEERING_HYGIENE_CONSTITUTION.md`
   - `docs/VALIDATION_LEDGER.md`
   - `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`
   - `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`
   - `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md`
   - `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`
   - `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`
   - `docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`
4. Treat live repository state, not this snapshot, as authority if anything moved.

## Closed history

E-R1 is CLOSED / PROMOTED / ARCHIVED and must not be reopened. Promoted `main` continuity checkpoint: `7490de24bfd2a9829f6afc1ae4b3831c98c50837`. Historical promoted native authority remains `cc395a25162a0a682796bffb44060c799df0db32` under `validation/e0a-pre-restructure-closure-native-arm64`; it does not validate comparison Harness source.

## Current comparison boundary

Branch: `e0a-gemini-rate-discipline-model-comparison`.

Architecture commit: `76fc0c64da4724a7a352a656a062d5c3ed431ad6`.

Cloud-validated source/test checkpoint: `8ed1563ec7a4c3ae919a649db0dc3c29e17a03e0`.

Validation run `34179384123`: SUCCESS. ARM64 cross-compile of Core/Harness/Core-tests/Harness-tests, x64 Core regression, repository law, oracle coverage, and document census passed. This did **not** execute Harness tests and is not native Windows ARM64 evidence.

No Core source, Core-test, or fixture delta exists from `main` through the source/test checkpoint.

## Approved comparison profiles

```text
CREATIVE-NONE    GEMINI-2.5-FLASH-LITE-NONE     gemini-2.5-flash-lite   10 RPM / 250K TPM
CREATIVE-MINIMAL GEMINI-3.5-FLASH-LITE-MINIMAL  gemini-3.5-flash-lite   15 RPM / 250K TPM
CREATIVE-NONE    GEMINI-2.5-FLASH-NONE           gemini-2.5-flash         5 RPM / 250K TPM
```

`CREATIVE-MINIMAL` is not literal thinking-off. RPD remains unverified. Free-tier provider execution remains synthetic-fixture-only.

The Harness paces every Gemini API-bound operation, including `countTokens`, and enforces exact rolling generation-input TPM. One attempt / zero retries remains law.

## Gemini 3.5 compatibility correction

Fresh Google GenerateContent documentation established that Gemini 3 may return opaque `thoughtSignature` metadata on ordinary response parts, including with minimal thinking. The transport was also found to retain an obsolete single-model route guard.

The source/test checkpoint above now:

- admits exactly the three catalogued comparison models;
- accepts opaque signatures only on the explicit 3.5 profile;
- strips signatures before streaming diagnostic persistence;
- never places signatures in semantic output or receipts;
- continues to reject `thought=true` material;
- retains strict signature rejection on 2.5 profiles;
- keeps role requests stateless and does not add signature replay/history semantics.

See `docs/evidence/E0A_GEMINI35_THOUGHT_SIGNATURE_COMPATIBILITY_AUDIT.md`.

## Current consequential gate — native validation

**Do not present or execute a real-provider gate yet.** Next is Director Windows ARM64 native validation on one exact clean documentation-inclusive branch checkout.

Required:

1. establish ARM64 host identity using `DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`;
2. run Core tests;
3. run Harness tests;
4. perform a fresh native Harness build after clearing stale outputs;
5. run Missing Raft and generic fixture smokes;
6. with `GEMINI_API_KEY` absent, prove all three approved arm/profile live paths exit `1`, emit `GEMINI_API_KEY is required at the E0-A provider edge.`, and create no evidence root;
7. re-check exact HEAD and working-tree cleanliness.

CLI form:

```text
e0a-run <arm> <provider-profile> <fixture.json> <run-id> <evidence-root> <executable-commit>
```

Use distinct nonexistent evidence roots/run IDs. Capture expected-failure stderr and `$LASTEXITCODE` exactly as the Director-host behavior document requires. This gate requires no key and must make no provider `countTokens`, network, inference, or spend.

## Later live gate

Only after native evidence is recorded and recursively audited may a separate Director authorization be requested for a real synthetic-fixture run. Planned order: 2.5 Flash-Lite None -> 3.5 Flash-Lite Minimal -> 2.5 Flash None. Reverify project/key association, tier/status, model availability, quotas including RPD, pricing, data-use terms, snapshot freshness, evidence destination, and checkout identity immediately before each authorized run.

Do not expose credentials or perform provider-network work without explicit authorization in that chat.

## Continuity discipline

Do not reopen E-R1, reintroduce OpenAI executable support, or enter E0-B+, UI/persistence, Windows AI/NPU, packaging/WACK/Store, or design-lane work. Recursively audit correctness, consistency, authority, scope, tests, simplicity, hygiene, ARM64 suitability, and evidence before consequential replies. Remove this temporary handoff only after the transition has fully converged and durable state has been folded into `CURRENT_STATE.md`.
