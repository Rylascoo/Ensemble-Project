# Create Production presentation contract

Status: **ADOPTED / Q-DESIGN-22 ENGINEERING HANDOFF READY / NATIVE ACCEPTANCE PENDING**

Date: 2026-09-22  
Exact Product baseline: `Rylascoo/Ensemble-Project@07fe8c3728d3c4c0a326e39a3089282e8cad246d`

## Purpose

Present the integrated Q-PROD-04 Create Production contract without adopting FIRSTUSE, selecting Home A/B, or inventing rename/delete/import/restore.

The Product contract remains A/A/A:

- catalog generates the opaque stable `ProductionId`;
- successful creation updates the known Production list but does not implicitly open, navigate or change ProductSpace;
- duplicate creator-facing `ProductionName` values are allowed.

## Information architecture

- Creation lives in the durable **Productions** route.
- Primary action: **New Production**.
- Creation is contextual library work, not Home, Settings, Current Production, Stage, or a new durable route.
- Home A/B and FIRSTUSE remain independent. If FIRSTUSE is later adopted, it may invoke this same Product operation rather than define another creation semantics.

## Creation interaction

1. **New Production** reveals a bounded creation form in the Productions context.
2. Field label: **Production name**.
3. The UI blocks null/empty/whitespace-only values before Product submission.
4. Accepted text is exact. The UI must not silently trim, case-fold, Unicode-normalize or rewrite punctuation.
5. Final action: **Create Production**.
6. During the synchronous Product call, repeated submission is disabled. No percentage, fictional time, provider state, retry, rollback or recovery semantics are created.
7. On authoritative success:
   - return to the Productions list;
   - select and keyboard-focus the exact newly created Production by returned `ProductionId`;
   - do **not** open it automatically;
   - ordinary **Open** remains explicit.
8. Creator-facing confirmation may read **Production created.**

## Duplicate-name disambiguation

Duplicate exact Production names remain Product-valid.

For a visible group with the same exact `ProductionName`:

1. serialize `ProductionId.Value` as exact big-endian UTF-16 code units;
2. SHA-256 that byte sequence;
3. render `Production code XXXXXXXX` using the first 8 uppercase hex characters;
4. if two codes collide inside the same visible same-name group, extend the colliding codes by 4 hex characters at a time until unique;
5. a full-hash collision fails closed as a presentation defect rather than inventing another identity rule.

The code appears only when disambiguation is needed. It is presentation metadata, not Product state, a filesystem locator, an editable field, a sort key, or part of `ProductionName`.

Accessibility name:

- unique-name row: the visible Production name;
- duplicate-name row: **<Production name>, Production code <code>**.

The exact created row is selected/focused by `ProductionId`, never by matching name text.

## Empty library

An empty library remains valid.

- **No Productions yet.**
- **New Production** remains available.

No first-use tutorial, onboarding sequence, marketing copy, import alternative or Home redesign is implied.

## Failure presentation

- Existing typed Product `Invalid` / `Incompatible` behavior remains authoritative.
- Creation UI may prevent locally knowable invalid name input before submission.
- Environmental I/O/access failure remains application-side infrastructure state.
- No automatic retry, recovery, rollback or claim of partial Product creation is added.

## Visual/accessibility law

Preserve Home / Productions / Settings, contextual Current Production, Light F2 / Dark D3 parity, MAT F1, quiet row/separator hierarchy, native focus, and focus/selection separation.

At 720×520, name entry, Create/Cancel, list return and duplicate-code witness must remain reachable without page-level horizontal overflow.

## Explicit non-authority

This contract does not authorize FIRSTUSE adoption, Home A/B selection, rename/delete/import/restore, automatic open after creation, creation-time Character/Scene/World setup, provider/Performer/Stage behavior, Product identity editing, final typography/Stage packaging, WACK, Store or release authority.

## Native return required

Q-PROD-05 must return exact native evidence for empty library + New Production; exact name entry; whitespace blocking; successful creation and no implicit open; exact created-row selection/focus by returned ID; duplicate-name rows with distinct conditional Production codes; unique-name rows with no unnecessary code; accessibility names without internal leakage; Light/Dark + 720×520; and preservation of Open/Recover/Current Production behavior.

Product/Persistence changes are a stop condition for Q-PROD-05.
