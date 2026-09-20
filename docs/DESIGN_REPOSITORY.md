# Ensemble Design Repository

Status: cross-lane locator only — not design, product, phase, or validation authority.

## Canonical design surfaces

- Active application-design authority: `docs/design/app/AUTHORITY.md`; immutable transfer provenance: `docs/design/app/TRANSFER_RECEIPT.json`.
- Website design/implementation repository: `Rylascoo/Ensemble-Website`.
- Canonical visual workspace / master-asset Drive root: `Ensemble Project` (`1VKomZE6PSaEM9c7q22p4r_N6UY0HM2D8`).
- Active Kymaean visual workspace: `Ensemble Project / 03 Visual Identity & Artwork / Kymaean` (`1MJrfMi1EZ_Wk3-IqC_BMMz1GTYpsrMnz`).
- Quarantined legacy Drive-root folder: `Kymaean Project` (`1b825q4ou6kWFRlAz75sjQ5LL5HgrpF81`). It is not the active Kymaean workspace and must never be selected by name match alone.

Stable Drive root locator:

`https://drive.google.com/drive/folders/1VKomZE6PSaEM9c7q22p4r_N6UY0HM2D8`

Stable active Kymaean locator:

`https://drive.google.com/drive/folders/1MJrfMi1EZ_Wk3-IqC_BMMz1GTYpsrMnz`

## Drive root structure

- `01 UI-UX Architecture`
- `02 Mockups & Prototypes`
- `03 Visual Identity & Artwork`
- `04 Motion & Animation`
- `05 Store & Marketing Assets`
- `06 Research & References`

The active `Kymaean` workspace beneath `03 Visual Identity & Artwork` further separates active product-UI reintegration, preserved brand/atmosphere work, historical inspiration, intake, and brand-thesis/continuity material. Folder names are descriptive; stable IDs resolve collisions.

## Boundary with GitHub

Project `docs/design/app/` owns current app UX/UI contracts, app design-system/state/accessibility/motion/disclosure contracts and necessary app-specific evidence. The exact Website sources sealed by `docs/design/app/TRANSFER_RECEIPT.json` remain historical provenance, not a second live copy.

`Rylascoo/Ensemble-Website` owns website design/implementation and website-specific design evidence. It may consume pinned shared/app contracts but does not retain current app-design authority.

Google Drive `Ensemble Project` owns visual exploration, renders, imagery, source/master visual assets, motion/animation material, Store/marketing assets, and design research/reference material.

`Rylascoo/Ensemble-Project` owns Product/application architecture, implementation, tests/tooling/validation and central product/policy/ODR/Director decisions. App Design and Engineering authority remain separate even when co-located.

When an app-design result becomes an implementation requirement, Engineering consumes the pinned Project app contract and records implementation/deviation evidence without editing Design meaning. Website requirements stay in Website. Shared-master references retain Drive ID/revision/hash provenance. Follow `docs/REPOSITORY_RESIDENCY.md`.
