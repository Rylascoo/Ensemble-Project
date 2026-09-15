# E0-D P01 Slot 1 Protected Credential Rebind Readiness

Date: 2026-09-15

Status: **PASS — NEW P01 CREDENTIAL LOCALLY PROTECTED + READY — PROVIDER TRAFFIC ZERO — NAMESPACE UNCONSUMED — WINDOW/CAPACITY GATES STILL PENDING**

## Authority

This evidence implements the non-provider local credential step required by `docs/evidence/E0D_P01_SLOT1_PROVIDER_ASSOCIATION_ROTATION_DIRECTOR_AMENDMENT_2026_09_15.md` for exact P01 Slot 1.

The authorized non-secret association remains:

- project `Ensemble Testing`;
- project ID `gen-lang-client-0793779417`;
- credential label `Gemini API Key`;
- required tier `Free tier`.

No API-key plaintext is repository data. The fresh secret was entered only into a local hidden prompt on SurfSeven and was not supplied to ChatGPT, Git, command arguments, logs or evidence.

## Local protected installation observation

At `2026-09-15T20:06:21.0846400Z`, a non-provider SurfSeven census observed:

- new protected blob `gemini-ensemble-testing-key.dpapi`: present;
- new ready marker `gemini-ensemble-testing-key.ready`: present;
- ready-marker timestamp: `2026-09-15T20:05:07.0051512Z`;
- superseded `gemini-test-key.dpapi`: still present and untouched;
- superseded `gemini-test-key.ready`: still present and untouched;
- ambient `GEMINI_API_KEY`: absent;
- matching `Ensemble.E0.Harness` process count: `0`.
At the same census:

- reserved P01 evidence root: absent;
- deterministic run claim: absent;
- deterministic root claim: absent.

The temporary local installer files used to create the new CurrentUser-protected pair were removed after readiness was observed. The old protected pair was preserved as historical local state but is explicitly ineligible for P01 use under the provider-association rotation.

The installer path was audited before use for local-only behavior. Its success path used Windows CurrentUser data protection and a local protect/unprotect round-trip before writing the ready marker. No standalone Gemini compatibility, key-health, quota or availability request was sent.

## Frozen experiment identity unchanged

This local credential rebind does not alter the P01 RunId, evidence root/claims, model/profile, fixture, native checkout/executable, immutable execution window, service route, no-retry law, scoring contract or successor authorization boundary.

P01 Slot 1 remains unconsumed. Provider traffic remains **ZERO**.

## Remaining execution barrier

The local protected-credential gate is now satisfied. Execution remains blocked until all remaining launch-boundary gates pass simultaneously:

1. current Project authority still authorizes exact P01 Slot 1;
2. UTC is inside `2026-09-16T14:30:00Z..23:30:00Z`;
3. immediately preclaim authenticated Google AI Studio observation confirms `Ensemble Testing` / `gen-lang-client-0793779417` / `Gemini API Key` / Free tier, exact `Gemini 3.5 Flash Lite`, and sufficient current RPM/TPM/RPD capacity;
4. validator/tag/executable/fixture/profile/RunId/root/claim/process identities remain exact and clean;
5. the newly protected credential is the credential selected for the intended child process and the superseded old credential is not selected;
6. no material contrary provider/account/model/lifecycle/pricing/data-use/reasoning-control/quota/route signal exists.

Any failed or indeterminate gate remains a hard stop before claim creation, evidence-root creation, execution credential injection or provider traffic. P01 Slot 2 and all later slots remain separately unauthorized.