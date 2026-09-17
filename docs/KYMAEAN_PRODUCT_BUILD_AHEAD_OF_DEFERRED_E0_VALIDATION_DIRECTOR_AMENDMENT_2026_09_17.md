# Kymaean Product Build Ahead of Deferred E0 Validation — Director Amendment — 2026-09-17

Status: **DIRECTOR-APPROVED PRODUCT-SEQUENCING AMENDMENT — BUILD MAY PROCEED — DEFERRED E0 VALIDATION REMAINS MANDATORY**

## Director decision

The Director has changed the project sequencing objective: build the Kymaean application continuously now and perform the remaining specific E0 validation experiments later rather than allowing those experiments to block visible product construction.

This amendment is product/policy authority. It changes sequencing, not experimental facts. It does not declare E0-D/E/F/G passed, does not erase consumed evidence, and does not convert provisional implementation into final architecture by assertion.

## New operating rule

The project may now implement the real product in parallel with deferred E0 validation.

The remaining E0-D/E/F/G work becomes a **deferred validation obligation** rather than a prerequisite for beginning product construction. Its frozen identities, evidence, claims, scoring law and no-retry rules remain intact.

Product implementation must preserve seams around conclusions that remaining E0 evidence could still change. Prefer replaceable ports, adapters and policy objects over irreversible coupling to unresolved experimental choices.

## Product lanes opened now

1. **Application + persistence foundation** — semantic Application commands/queries, Production lifecycle, causal persistence/replay/recovery ports and deterministic tests may be implemented now.
2. **Native Windows application shell** — a native ARM64 Windows shell may be built now around the frozen Studio / Stage / Archive product spaces, with strict separation from provider/persistence internals.
3. **Provider runtime** — production-shaped provider abstractions, credential boundaries, streaming/cancel/retry/spend provenance and test adapters may be implemented now.
4. **UI/design implementation convergence** — Engineering may consume approved Design contracts; Design Sol may continue app-facing UI exploration and implementation specifications without waiting for E0 convergence.

All four lanes may progress concurrently when their implementation dependencies permit it. One-writer-per-worktree and serialized authority integration still apply.

## What remains deferred

The remaining E0 plan is preserved for later execution and reconciliation. This Director decision places the currently authorized P03 Slot 1 on a **deferred-test hold**: do not auto-launch it when the existing fixed window opens. If that window lapses while the slot remains unconsumed, preserve its root/claim namespace exactly and require a later explicit Director resume decision plus any necessary timing amendment before execution.

Deferred E0 results must be reconciled before:

- declaring the product-runtime architecture final/frozen;
- declaring the integrated runtime-complete Alpha exit gate passed;
- Beta/release architecture convergence;
- an immutable Store release candidate or Store submission.

If later E0 evidence contradicts provisional product code, the product code changes. The experiment is not rewritten to protect implementation already built.

## Provider separation

The frozen E0 provider association `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` remains reserved for the frozen experiment plan and may not be repurposed as ordinary product-development traffic while those experiments remain outstanding.

Ordinary product-development provider traffic should use a separately identified development Google project/key so development quota/activity cannot contaminate frozen E0 capacity observations. Creating that development association does not change Core/Application architecture and does not alter E0 evidence.

Until a separate development provider association is durably identified, provider-runtime implementation and credentialless/fake-adapter testing may proceed, but new non-experimental Gemini network traffic remains held.

## Validation classification

Code built under this amendment is **provisional product implementation** until the deferred E0 obligations are reconciled. It can receive normal source/compiler/native-runtime validation for what it actually proves, but those validations do not substitute for the deferred E0 experimental questions.

Conversely, deferred E0 experiments do not need to block implementation work whose correctness does not depend on their result.

## Immediate product objective

Start with the smallest vertical product path that makes Kymaean visible while preserving architecture seams:

`Application semantic contracts -> durable Production persistence seam -> native ARM64 shell -> Studio/Stage/Archive navigation -> deterministic local fixture/demo path -> development provider adapter -> integrated creator flow`.

The first implementation package should establish the Application boundary and a native shell scaffold without embedding provider/model identities in Core/Application or treating placeholder UI as final Design authority.
