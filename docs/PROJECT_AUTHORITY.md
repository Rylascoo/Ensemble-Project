# Ensemble Project Authority

## Source-of-truth order
1. Frozen Ensemble Blueprint and approved phase specifications.
2. GitHub `CURRENT_STATE.md` for the active engineering checkpoint.
3. GitHub source, tests, fixture versions/hashes, commits, and PR evidence.
4. Decision Log and Evidence Register as they are added.
5. Google Drive design repository for visual/design evidence and assets.
6. Historical chat/source material only when a narrow continuity ambiguity requires it.

## Engineering authority
GitHub is the canonical engineering workspace. Changes should be patch-first and reviewed against the smallest affected surface.

## Design authority
The connected Google Drive `Ensemble Project` folder is the canonical visual/design workspace for:
- UI/UX architecture
- mockups and prototypes
- visual identity and artwork
- motion and animation studies
- Store and marketing assets
- design research and references

Design artifacts are evidence and design direction unless explicitly promoted into an approved implementation specification.

## Validation hierarchy
Static reasoning < compiler output < target-device runtime < hardware/NPU evidence < package validation < Store certification.

Never promote a lower validation level into a higher one.

## Continuity rule
Nothing important changes silently. Meaningful decisions, evidence, implementation milestones, and reopened assumptions must be recorded in durable project state.
