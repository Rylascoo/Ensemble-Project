# E0-D P02 Pair Window — Director Amendment

Date: 2026-09-16

Status: **ACTIVE BOUNDED DIRECTOR TIMING AMENDMENT — P02 PAIR WINDOW MOVED PRE-CONSUMPTION — SLOT 1 ONLY LIVE-AUTHORIZED — PROVIDER TRAFFIC ZERO**

## Director decision

In the active Ensemble Project Administrator conversation, before any P02 namespace consumption or provider traffic, the Director stated: **`let's open the P02 window now and continue`**.

At that decision boundary, trusted current time was `2026-09-16T14:32:19-05:00` / `2026-09-16T19:32:19Z` and the deterministic window begins at the next full UTC minute after the instruction. This record converts the instruction into one exact timing amendment. The prior P02 pair window was `2026-09-17T14:30:00Z..2026-09-17T23:30:00Z`, a nine-hour interval. To preserve duration while moving the window to the current Director-authorized boundary, the amended P02 pair window is:

- start: `2026-09-16T19:33:00Z`;
- end: `2026-09-17T04:33:00Z`;
- duration: exactly nine hours.

P03 remains `2026-09-18T14:30:00Z..2026-09-18T23:30:00Z`.

## Supersession scope

This amendment supersedes **only the P02 pair-window field** carried by the frozen allocation and repeated by the P02 Slot 1 live authorization, activation package, queue and current-state surfaces.

Those predecessor records remain immutable historical evidence. Their old P02 timing statements are not executable after this amendment integrates to Project `main`; all other controls remain binding.

## Frozen experiment identity preserved

The amendment does **not** change the P02 pair, slot identities, variants, RunIds, evidence roots, deterministic claim names, model, profile, service route, fixture, native checkout, executable hash, one-attempt/zero-retry law, hard-gate law, scoring law or evidence-immutability law.

Exact Slot 1 remains `P02-ABLATION` / `E0D-OMNISCIENT-CONTEXT-01` / `E0D-Q01-P02-OMNI-20260914-03`.

The authorized provider association remains `Ensemble Testing` / `gen-lang-client-0793779417` / credential label `Gemini API Key` / Free tier. The `gemini-ensemble-testing-key` CurrentUser-protected credential remains the only eligible protected credential; the superseded old protected credential remains ineligible.

## Integration and launch barrier

This Director decision does not itself permit a namespace claim or provider request from an unintegrated branch. Exactly one Slot 1 execution becomes available only after:

1. this amendment is integrated to current Project `main` through the required exact-head hosted gates and push-triggered exact-main Validation passes;
2. current UTC is inside `2026-09-16T19:33:00Z..2026-09-17T04:33:00Z`;
3. immediately before the irreversible claim boundary, authenticated Google AI Studio freshly confirms `Ensemble Testing` / `gen-lang-client-0793779417`, `Gemini API Key`, Free tier, exact `Gemini 3.5 Flash Lite`, and sufficient current RPM/TPM/RPD capacity;
4. the exact validator/tag/executable/fixture/profile/RunId/root/claim/process interlocks remain clean and the eligible protected credential is selected;
5. no material contrary provider/account/model/lifecycle/pricing/data-use/reasoning-control/quota/route signal exists.

A stale capacity observation plus arithmetic does not satisfy item 3. No standalone Gemini compatibility, key-health, availability or quota probe is authorized.

## Single-use and successor boundary

The first deterministic claim/evidence-root creation still consumes P02 Slot 1 regardless of terminal outcome. No retry, replay, replacement, fallback, paid/Priority route, alternate model/profile/fixture, retune or automatic window extension is authorized.

This timing amendment changes the P02 **pair** window only because the frozen allocation defines timing at pair level. It does **not** authorize `P02-FULL`. P02 Slot 2 remains separately unauthorized and may be considered only after Slot 1 reaches terminal state, terminal evidence is sealed/reconciled, a fresh post-predecessor authenticated capacity observation passes, and the Director separately authorizes exact Slot 2 live execution.

If those Slot 2 prerequisites cannot be completed before `2026-09-17T04:33:00Z`, Slot 2 remains unexecuted and requires further Director disposition. The amended pair window must not be extended automatically to rescue a later slot.

P03, E0-E, Administrator MA-series, Reviewer, Hooks, Automations and autonomous chaining remain outside this amendment.

Provider traffic and P02 namespace consumption remain **ZERO** while this amendment is prepared and integrated.
