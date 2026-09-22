# Creator World-current truth presentation contract

Status: **ADOPTED / Q-DESIGN-21 CONTRACT FROZEN / ENGINEERING HANDOFF READY / NATIVE ACCEPTANCE PENDING**

Date: 2026-09-21  
Exact Product baseline: `Rylascoo/Ensemble-Project@41c8912eee34ca00932feabaca99e34bd027e08f`  
Exact-main Validation: #1146 PASS

## Purpose

Define the creator-facing presentation of the already-earned `WorldCurrentState` contract for a known, open Production. This is a presentation contract over existing Product truth. It does not add Product entities, lifecycle, history queries, Character knowledge, provider behavior or persistence semantics.

The governing relationship is:

`authoritative replay -> creator inspection -> local proposed replacement -> explicit whole-state review -> Product replacement -> authoritative replay confirmation`.

It is not:

`editable rows -> incremental database CRUD`.

## Product facts Design must preserve

1. `WorldCurrentState` is an immutable canonical set of `WorldCurrentTruth` text values. Exact duplicate text is invalid; blank/whitespace-only text is invalid.
2. Product canonicalization uses ordinal text ordering. Order has no rank, chronology, priority or causal meaning.
3. Accepted text identity is exact. Presentation must not silently trim, case-fold, Unicode-normalize, rewrite punctuation or otherwise alter accepted text.
4. Creation-only histories legitimately produce an empty current World state.
5. Replacement is creator-specific and replaces the whole set. It is not add/delete/patch semantics.
6. A successful replacement is confirmed only by the authoritative replay returned from the writer/Application boundary.
7. Typed `Incompatible` and `Invalid` replacement failures do not further append and preserve the prior usable Application projection.
8. Environmental I/O/access failure is not a Product failure kind. Append and authoritative replay are separate operations, so an environmental exception after submission can leave whether the replacement committed unconfirmed.
9. World-current truth is creator/Production truth. It does not transfer, reveal or mutate Character knowledge, belief, memory, perspective or disclosure state.

## Information architecture

- The surface is contextual depth inside the already-open Current Production.
- It is not a fourth durable NavigationView item, not a permanent World/Knowledge silo, and not a new `ProductSpace`.
- Entry may be exposed from Current Production through a contextual creator action labeled **World truths**.
- Surface heading: **Current world truths**.
- Supporting meaning: **What is currently true in this Production's world.**
- Boundary witness: **Creator view — this does not describe what any Character knows or believes.**
- Back returns to the Current Production origin and restores the invoking control's focus. Back is navigation, never Undo.

The surface may reuse accepted contextual/deep-inspection structure, but it may not inherit fact/claim/belief/provenance categories that current Product truth does not expose.

## Interaction states

### 1. Authoritative inspection

Render only `CurrentProductionReplay.WorldCurrentState.Truths`.

- Non-empty: quiet textual rows/separators; no ranking or timeline treatment.
- Empty: **There are no current world truths.**
- Display order follows Product canonical order for deterministic review only.
- Do not show event history, timestamps, provenance, confidence, source, Character visibility or inferred categories.

### 2. Compose replacement

**Edit replacement** creates local, non-effective presentation state.

- Draft heading: **Proposed replacement**.
- Add/edit/remove controls operate only on the local proposal.
- Current authoritative truths remain distinguishable from the proposal.
- Blank/whitespace-only rows and exact duplicates must be blocked before Product submission.
- User text must not be silently normalized.
- Draft state is not current truth merely because it is visible.

An empty proposal is valid and means replacing the whole current set with the empty set.

### 3. Review replacement

**Review replacement** opens an explicit pre-effect review.

Required witness:

**This will replace the entire current world truth set.**

Show current and proposed sets as distinct textual groups. The proposal shown for confirmation must use the same canonical ordering the Product will use. No item may be presented as an independent save/delete operation.

Final action label: **Replace current world truths**.

### 4. Submission pending

While the synchronous Product operation has not resolved:

- identify the condition as an application transition;
- disable repeated submission;
- do not mutate the visible authoritative set optimistically;
- do not imply fictional time, Character action or provider state.

No progress percentage, retry, cancel or rollback semantics are created.

### 5. Confirmed success

Only `ProductAccessResult.Success` plus the returned authoritative replay establishes success.

- Replace the inspection set with the replayed `WorldCurrentState`, not the local draft object.
- Clear the proposal/review state.
- Confirmation copy: **Current world truths updated.**
- Confirmation may become quiet after focus/announcement; color is not the authority carrier.

### 6. Typed non-effective failure

`Incompatible` and `Invalid` are known non-effective outcomes for this operation under the integrated contract.

- Keep the previously confirmed authoritative set.
- Keep any useful proposal visibly separate and label it **Not applied**.
- Do not auto-retry, recover, repair, rollback or discard the proposal.

Creator-language baselines:

- Incompatible: **This Production's current world truths can't be changed in this version.**
- Invalid: **Kymaean can't change this Production's current world truths because its contents are invalid.**

The two meanings must remain distinguishable without backend/storage vocabulary.

### 7. Environmental non-confirmation

An environmental exception after submission is **not** the generic failed/cancelled state from earlier APPUI evidence, because Product evidence does not prove the replacement was non-effective.

Required heading: **Confirmation unavailable**.

Required meaning:

**Kymaean couldn't confirm whether the replacement became current.**

Presentation law:

- relabel the prior display **Last confirmed world truths**;
- show the submitted proposal separately as **Submitted replacement**;
- neither group may be labeled current;
- do not say saved, failed, rolled back, unchanged or applied;
- mutation controls remain unavailable until a later authoritative inspection is established;
- this contract defines no Retry, Recover, rollback or idempotence promise.

Navigation remains available. The mechanism by which Engineering later re-establishes authoritative inspection must use existing earned Product access and may not be invented as a new Product recovery contract.

## Visual and accessibility law

- Preserve Light F2 / Dark D3 parity, MAT F1 continuous-field topology, STA F2 focus/selection separation and creator-language copy.
- Current/proposed/submitted/last-confirmed distinctions must be structural and textual before color.
- Healthy persistence remains quiet; environmental non-confirmation stays application-side and never enters fiction.
- Prefer quiet rows/separators over repeated cards.
- No essential meaning may require side-by-side columns; narrow layouts may sequence the groups.
- Each truth row's accessible name is the visible truth text, not an internal object representation or ID.
- Empty, error and non-confirmed states require programmatic headings/status text and keyboard-reachable return.
- No new bespoke icon is authorized.

## Explicit non-authority

This contract does not authorize:

- World-current history/timeline/query UI;
- per-truth provenance, confidence, truth taxonomy, source or timestamps;
- Scene/current-situation semantics;
- Character knowledge, belief, memory, discovery, deception or bounded perspective;
- create/rename/delete/import/restore Production lifecycle;
- retry/recovery/rollback/idempotence algorithms;
- provider/model/Performer controls;
- Stage placement or motion;
- Home A/B or FIRSTUSE selection;
- final typography/Stage packaging, High Contrast proof, WACK, Store or release authority.

## Native return required

A bounded Engineering implementation must return exact native evidence for:

1. non-empty authoritative inspection;
2. valid empty state;
3. exact-text/no-silent-normalization behavior;
4. duplicate/blank draft blocking;
5. whole-state review and empty replacement;
6. confirmed replay success;
7. distinguishable Incompatible and Invalid states;
8. environmental **Confirmation unavailable** with last-confirmed/submitted separation and no retry control;
9. no Character/Stage/provider leakage;
10. Light/Dark parity, keyboard/focus return, accessibility names and responsive minimum-window behavior.

Native evidence returns to Q-DESIGN-21 for acceptance. Product/Persistence source changes are a stop condition, not part of this handoff.
