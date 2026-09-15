# E0-D P01 Slot 1 Provider Association Rotation — Director Amendment

Date: 2026-09-15

Status: **ACTIVE BOUNDED DIRECTOR AMENDMENT — EXACT P01 SLOT 1 ONLY — PROVIDER ASSOCIATION ROTATED BEFORE CONSUMPTION — LOCAL PROTECTED CREDENTIAL REBIND PENDING — PROVIDER TRAFFIC ZERO**

## Director decision and provenance

In the active Ensemble Project Administrator conversation, the Director reported that a new Gemini API project and fresh API key had been created to resume project work, agreed to the proposed narrow provider-association amendment, and supplied an authenticated Google AI Studio API Keys screenshot.

The screenshot establishes the following non-secret association:

- project display name: `Ensemble Testing`;
- project ID: `gen-lang-client-0793779417`;
- API-key label: `Gemini API Key`;
- billing tier: `Free tier`;
- project/key creation date shown by the UI: `Sep 15, 2026`.

The exact Director-supplied screenshot has SHA-256 `3b288d5a2aaba27919078556ac708f1ca2ac68b4acff7a871f65a06602cc533b`. The screenshot itself is not committed because account UI context is not required for executable reproduction. No full API key was exposed to chat or recorded in this repository; even the UI-visible key suffix is intentionally omitted from durable evidence.

## Superseded association for P01

For future execution of exact P01 Slot 1, this amendment supersedes only the authenticated project/key association carried forward from `docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md` and repeated by the existing P01 activation package.

The old P01 association:

- project `Gemini Project - Kymaean`;
- project ID `gen-lang-client-0490221700`;
- credential label `Gemini API Key - testing`;

must no longer be treated as satisfying the P01 preclaim provider/account/key gate.

The amended exact P01 association is:

- project `Ensemble Testing`;
- project ID `gen-lang-client-0793779417`;
- credential label `Gemini API Key`;
- required billing tier `Free tier`.

This amendment is intentionally scoped to P01 Slot 1. It does not silently establish a new global standing association for later E0-D slots or unrelated future provider work.

## Frozen experiment identity preserved

This is a pre-consumption provider-association rotation, not a replacement run, retry, replay, fallback, retune, or experimental-condition substitution. P01 Slot 1 remains unconsumed.

The following remain unchanged and binding:

- pair `E0D-P01-RELATIONSHIP-OMISSION`;
- slot `P01-FULL`;
- variant `E0D-FULL-REFERENCE-01`;
- RunId `E0D-Q01-P01-FULL-20260914-01`;
- reserved evidence root and deterministic claim identities;
- immutable window `2026-09-16T14:30:00Z..23:30:00Z`;
- model `gemini-3.5-flash-lite`;
- profile `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- service route `standard`, Free tier;
- canonical Missing-Raft fixture and semantic/provenance SHA-256 `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- native checkout `0dacdbf6bd5453c192568cd4718207e145dfcf40`;
- executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`;
- one-attempt / zero-automatic-retry / no-fallback / no-retune law;
- synthetic-only, evidence-immutability, hard-gate, scoring and successor law.

The existing activation and activation-closeout records remain valid predecessors except where they identify the superseded old project/key association. This amendment controls that association field for P01.

## Protected credential boundary

The Director did not provide the new secret key to ChatGPT. Therefore no repository or chat action in this amendment installs or verifies the secret credential.

Before P01 can execute, the fresh key must be bound locally on SurfSeven through the established CurrentUser-protected credential mechanism without printing, logging, hashing, committing, exporting or placing plaintext in command arguments. The previously protected old credential must not be injected into P01 merely because its artifact remains present.

A local protected-credential rebinding/readiness step may occur before the P01 window because it is non-provider preparation. It must not send a Gemini request, create the evidence root, create either deterministic claim, or set an ambient `GEMINI_API_KEY`.

## Remaining launch barrier

Exactly one P01 Slot 1 execution remains authorized only if all original launch barriers still pass with this amended association:

1. this amendment is integrated to Project `main` through required hosted validation;
2. the fresh secret credential is locally protected and ready without disclosure;
3. current UTC is inside `2026-09-16T14:30:00Z..23:30:00Z`;
4. immediately before the irreversible claim boundary, authenticated Google AI Studio freshly confirms `Ensemble Testing` / `gen-lang-client-0793779417`, `Gemini API Key`, Free tier, exact `Gemini 3.5 Flash Lite`, and sufficient current RPM/TPM/RPD capacity;
5. every frozen executable/tag/fixture/profile/RunId/root/claim/process interlock remains exact and clean;
6. no contrary provider/account/model/lifecycle/pricing/data-use/reasoning-control/quota/route signal exists.

A stale UI observation plus arithmetic does not satisfy item 4. Do not use a standalone API compatibility, key-health, availability or quota probe. Failure or indeterminacy of any gate is a hard stop before namespace claim, evidence-root creation, credential injection or provider traffic.

## Authority boundary

This amendment creates no P01 Slot 2, P02, P03, retry, replacement, paid/Priority route, alternate model/profile/fixture, window move, E0-E execution, Administrator MA-series, Reviewer, Hook, Automation or autonomous-chaining authority.

Provider traffic remains **ZERO** while this amendment is prepared and integrated.
