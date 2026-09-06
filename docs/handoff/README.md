# Implementation handoffs

The H1 implementation handoff files were removed from the active tree during the end-of-H1 convergence audit on 2026-09-05.

They were transition artifacts, not enduring architecture authority. Their approved blueprints, implementation evidence, machine evidence, and Git history remain authoritative for the checkpoints they describe.

Historical evidence may still name an old `docs/handoff/...` path because that path existed at the recorded checkpoint. Resolve such a reference through repository history rather than treating it as current implementation guidance.

A temporary active transition handoff exists for the in-progress E0-A Phase-B implementation:

`docs/handoff/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE_IMPLEMENTATION_HANDOFF.md`

It preserves continuation state only. The applicable approved blueprint/evidence outrank it on architecture, and it should be removed from the active tree after the implementation is validated/promoted and continuity is folded back into `CURRENT_STATE.md`.

Fresh engineering work starts by reading `CURRENT_STATE.md` and resolving current `main`, then follows any explicitly active transition handoff plus the applicable approved blueprint/evidence.
