# Q-PROD-05 concurrent branch reconciliation

Date: 2026-09-22  
Status: **CANONICAL CANDIDATE SELECTED / ALTERNATE SUPERSEDED / ALTERNATE BRANCH TEMPORARILY PRESERVED**

Exact shared base:
`a6bac97f3fe543d36aa28067d8c566f7d0882d7c`

Two independent Windows-only Q-PROD-05 candidates appeared from the same exact base:

- canonical candidate: `qprod05/create-production-presentation-2026-09-22@ca950c70297ff9f6bd85785ed98397c9c1835272`;
- alternate candidate: `qprod05/create-production-windows-2026-09-22@ce46c78f21bd14aeb04694348bc7dd7ba079f0ba`.

Neither changed Product/Persistence source. Both passed compiler/Application/Persistence/authority-census jobs; each initial hosted run failed only the root `CURRENT_STATE.md` 3 KiB law after adding active-package prose.

## Semantic comparison

The canonical candidate is selected because it more exactly matches the adopted Q-DESIGN-22 contract:

1. **Submission transition:** it separates begin/complete submission and yields once to the UI, making the pending application transition representable and disabling repeated Create/Cancel submission while pending.
2. **Fingerprint collision law:** it extends only colliding Production-code prefixes in 4-hex increments, exactly matching the contract rather than extending every code in the same-name group.
3. **Narrow layout:** it removes the inherited 360px row minimum and wraps both name/code, reducing 720×520 overflow risk.
4. **Exact selection:** the presentation row is the selected item and carries the authoritative `ProductionId`; post-create selection/focus is bound to the exact returned row, never a name lookup.
5. **Empty-state wording:** it uses the adopted `No Productions yet.` copy.

The alternate candidate contains no unique Product/Design semantic authority that needs promotion. Its exact SHA is preserved here for provenance.

## Disposition

Continue only the canonical branch. Keep the alternate remote branch until the canonical candidate is integrated and exact-main validation passes; then it may be deleted as a superseded unique implementation after this durable comparison is present on main.

No merge, Product decision, FIRSTUSE/Home adoption, or provider authority is created by this reconciliation.
