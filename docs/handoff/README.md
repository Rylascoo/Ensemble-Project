# Implementation handoffs

Implementation handoffs are temporary transition artifacts, not enduring architecture, product, phase, provider, or validation authority.

The H1 implementation handoffs were removed from the active tree during end-of-H1 convergence on 2026-09-05. Completed E0 transition handoffs are likewise removed after their applicable work converges and continuity is folded back into `CURRENT_STATE.md` and the durable architecture/evidence surfaces.

Historical evidence may still name an old `docs/handoff/...` path because that path existed at the recorded checkpoint. Resolve such references through repository history rather than treating them as current implementation guidance.

A handoff is live only when `CURRENT_STATE.md` identifies its exact `docs/handoff/...` path. At current `main`, `CURRENT_STATE.md` names no live handoff, so this directory intentionally contains only this lifecycle README.

Fresh chats therefore bootstrap from `CURRENT_STATE.md` and `AGENTS.md`, then follow only the exact current contracts/evidence named by the authority chain. Do not recreate a handoff merely for convenience when durable current state already supplies the transition.
