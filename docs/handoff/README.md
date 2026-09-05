# Historical implementation handoffs

The H1 implementation handoff files were removed from the active tree during the end-of-H1 convergence audit on 2026-09-05.

They were transition artifacts, not enduring architecture authority. Their approved blueprints, implementation evidence, machine evidence, and Git history remain authoritative for the checkpoints they describe.

Historical evidence may still name an old `docs/handoff/...` path because that path existed at the recorded checkpoint. Resolve such a reference through repository history rather than treating it as current implementation guidance.

Fresh engineering work starts from `CURRENT_STATE.md`, the applicable approved blueprint/evidence, and current `main`.
