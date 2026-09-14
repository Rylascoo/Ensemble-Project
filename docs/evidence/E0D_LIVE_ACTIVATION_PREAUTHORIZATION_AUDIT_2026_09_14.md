# E0-D Live Activation Preauthorization Audit

Date: 2026-09-14

Status: **READ-ONLY AUDIT PASS — ACTIVATION REMAINS BLOCKED — NEW KEY-TYPE + SNAPSHOT-FRESHNESS GATES IDENTIFIED — PROVIDER TRAFFIC ZERO**

## Scope and authority

This audit prepares Director input only. It does not allocate a RunId, evidence root, namespace claim, UTC window, credential, provider request, inference, spend, scoring artifact, or live activation.

Governing authority remains `CURRENT_STATE.md`, `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`, and `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`.

Exact integrated implementation/native authority remains checkout `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`, tag `validation/e0d-ablation-controls-native-arm64`, executable SHA-256 `498c25f528ab1ba26a592d247737186ac839c61b56154303d6e57a94319d0769`.

## Frozen experiment boundary

E0-D remains exactly three matched pairs / six slots in frozen order: P01 Full then relationship omission; P02 omniscient then Full; P03 Full then round-robin. Every RunId remains unallocated. Before the first namespace claim, all six unique RunIds/roots and one immutable UTC window per pair must be frozen.

Every slot still requires a fresh execution-sensitive authenticated capacity observation after its predecessor reaches terminal state. A stale account/quota snapshot plus arithmetic is not sufficient. Any claimed slot is consumed by any terminal; no retry, replacement, seventh slot, retune, model/profile/fixture substitution, or outcome-driven timing change exists.

The preregistration itself already freezes the blind instrument semantics: ten dimensions, pair-specific mechanism questions, arm-identity blinding, neutral speaker normalization, mapping only after pair eligibility, commitment before scoring, mapping withheld through score seal, and no master-score/significance claim.
## Public-provider facts rechecked 2026-09-14

Official Google Gemini documentation currently confirms `gemini-3.5-flash-lite` is GA/stable with no announced shutdown date; `minimal` and `high` thinking levels remain supported; Standard remains the default synchronous inference tier. Standard Free-tier token use remains free and Free-tier submitted content remains eligible for product improvement, preserving the synthetic-only boundary. Published paid shadow rates remain USD 0.30/M input, USD 0.03/M cache, and USD 2.50/M output including thinking, matching the validated executable.

Official sources rechecked: `https://ai.google.dev/gemini-api/docs/deprecations`, `/pricing`, `/thinking`, `/rate-limits`, `/api-key`, and `/optimization`.

A new load-bearing credential-policy fact is now current: Google is transitioning Gemini API Standard keys to authorization (Auth) keys. New AI Studio keys default to Auth keys; unrestricted Standard keys are rejected; Google states Standard-key Gemini API requests will be rejected in September 2026 and requires migration to Auth keys. The current protected project testing key's type is not durably proven by repository evidence. A historical temporary `AQ.` Auth key succeeded externally on 2026-09-08 but was exposed/quarantined and cannot establish the protected key's current type.

Therefore **current protected key type/readiness is PENDING**. Any activation must prove, without exposing key material, that the intended protected key is a current usable Auth key associated with the intended Kymaean project.

## Executable freshness boundary

The exact validated executable hard-codes `E0AGeminiPricingPolicy.SnapshotValidThrough = 2026-09-14` and fails closed when the UTC date becomes later than 2026-09-14. `docs/HYPOTHESIS_LEDGER.md` already requires an audited source snapshot refresh plus renewed native validation/tagging for provider execution after that date.

Public facts matching the snapshot do not extend this executable guard by prose. Therefore any E0-D provider execution on UTC 2026-09-15 or later is **BLOCKED pending narrow source snapshot refresh + renewed native Windows ARM64 validation/tagging**. No execution window has been selected in this audit.
## Gate matrix

PASS: frozen method/preregistration; exact integrated implementation/native tag; public model lifecycle; frozen thinking controls; Standard-tier semantics; 2026-09-14 public pricing/data-use match; six-slot order/no-retry law; blind-measurement law; no known E0-D evidence root/claim/external process on the Director host.

PENDING / BLOCKED: explicit E0-D live-activation/preexecution Director authority; six RunIds/roots; three pair windows; protected current key type; protected credential readiness; authenticated current project/key association and Free tier; fresh exact 3.5 Flash-Lite quota/capacity; live-run spend/use authority; and every later slot's post-predecessor fresh execution-sensitive capacity observation.

TIME-BOUND: provider traffic using the currently validated executable must occur no later than UTC 2026-09-14. After that boundary, source snapshot refresh and renewed native validation/tagging become prerequisites before provider execution can proceed.

## Director decision input

No objective evidence supports launching or allocating E0-D yet. The next safe gate is a separate Director authorization for preexecution activation preparation. That authority may permit authenticated read-only AI Studio/account inspection, protected credential-readiness verification without key disclosure, and—only if the executable freshness boundary remains satisfiable or is first renewed—freezing the six RunIds/roots and three pair windows. Provider traffic should remain zero until the completed activation package is durably integrated and all exact prelaunch gates pass.
