# E0-C Q-E0C-01 - Director Timing-Law and Dual-Preregistration Reconciliation

Date: 2026-09-12

Status: **CORRECTED OWNING AUTHORITY - EARLIER IDENT FREEZE CANONICAL - RACED REF REALIZATION HISTORICAL - PROVIDER TRAFFIC ZERO**

## Trigger and Director authority

Project Issue #107 identified a temporal-selection defect before any E0-C namespace claim: preregistering two slots fixed sample count but did not prevent outcome-conditioned delay of Slot 2. The Director explicitly ratified the correction in Issue #107 comment `5650353003` at `2026-09-13T02:44:41Z`.

The ratified law preserves exactly two fresh Run-08-identical repeats from executable `bb869fb1c505603612bc718f739b3f1b358e5539`, freezes order Slot 1 then Slot 2, and requires one explicit UTC batch window with fixed start/end timestamps covering both planned launches before the first namespace claim. After the first claim, order/window are immutable. A slot blocked by an objective gate through window end is preserved unexecuted/noncontributing; no extension, probe, retry, replacement/third slot, model/route/source change is permitted.

The Director decision explicitly authorizes Engineering reconciliation only. It authorizes no namespace claim, credential use, provider request/probe, inference, or spend.

## Race chronology

- Earlier preregistration commit `e440d104f8f63f0486865a49542bd0fc6684165f` was created at `2026-09-13T02:22:27Z` through PR #109 and fixed `E0C-Q01-IDENT-20260912-01` / `...-02` plus blind instrument SHA-256 `7857699de82904f52892f3e6d0f9794c80e50ee488c9e30ef9fdb13ba4edfd64`.
- Director corrected-method ratification occurred at `2026-09-13T02:44:41Z`.
- Later PR #110 head `06bec51adaa3359e662f7413e660fd24d6e01c17` merged at `2026-09-13T02:45:15Z`, 34 seconds after ratification, using alternate `REF` RunIds/instrument and omitting the newly ratified UTC-window law.
- PR #111 later closed the `REF` activation lifecycle and advanced `main` to `4f084f6820dbdcf8d3a55ab76fc3c5e59a4fb6d2`; hosted validation was green, but validation cannot override the earlier Director method decision.

## Deterministic preregistration disposition

The earlier `IDENT` freeze is canonical because it was fixed first and no objective repository-law defect was found that independently invalidates it. Its exact executable, Fixture, profile, sample count, unique namespaces, no-retry/replacement law, contribution rule, and blind instrument are compatible with the corrected method. The only identified defect was missing temporal law, which is amendable without changing the frozen RunIds or instrument.

The later `REF` realization is therefore superseded for current execution authority, not erased. Its former current surfaces are moved under `docs/evidence/archive/` with `E0C_RACED_REF_...` names; PR #110/#111, merge commits, hosted validations, and archive tags remain immutable historical provenance.

The legacy tag `archive/e0c-two-slot-preregistration-superseded-2026-09-12` still peels to `e440d104...`; its name reflects the earlier mistaken race disposition and does not override this later owning reconciliation.

## Canonical post-repair boundary

Canonical fresh slots are `E0C-Q01-IDENT-20260912-01` then `E0C-Q01-IDENT-20260912-02`. The unchanged canonical blind instrument is `docs/evidence/E0C_Q_E0C_01_BLIND_REPEATABILITY_INSTRUMENT_2026_09_12.json`.

Slot 1 remains preactivation-only. Before any namespace claim, a later exact activation must reverify fresh namespace state, exact native executable/Fixture, authenticated intended project/key/Free tier/current 3.5 RPM/TPM/RPD capacity, protected credential readiness, applicable provider authority, and freeze the explicit UTC batch start/end covering Slot 1 then Slot 2. That activation must be integrated and exact-main validated before provider traffic.

Provider traffic is **ZERO** throughout this repair.
