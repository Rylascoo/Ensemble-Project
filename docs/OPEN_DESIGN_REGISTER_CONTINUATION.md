# Open Design Register — Post-Freeze Continuation

Status: OPEN QUESTIONS ONLY
Updated: 2026-09-05

Blueprint 0.1 remains frozen. ODR-01 through ODR-31 retain their exact frozen wording and status. This file records questions raised after the freeze without editing the frozen blueprint.

## Director-priority open items

### ODR-12 — Opportunity selection after E0

What opportunity-selection mechanism works best after E0—structured rules, local semantic judgment, or a hybrid—and how is it calibrated without fake numeric precision?

Current E0 reference authority remains the deterministic least-intervention Director. This open item does not alter that reference or the required E0-D round-robin ablation.

### ODR-13 — Scene endings

How should Scene endings emerge from pressure resolution while deterministic budgets and limits remain enforceable?

Current E0 does not freeze a product Scene-ending mechanism. Experimental run limits may remain deterministic without being promoted into dramatic-ending authority.

### ODR-30 — Active Scene size

Is the tentative V1 active Scene limit of five correct after real UX observation?

Current E0 remains exactly three co-present Characters. Five is a product/UX hypothesis, not a Core ontology or engine limit.

### ODR-32 — Context continuity at scale

What is the Context Composer's actual continuity mechanism as committed state grows: recency window, state-fact reconstruction, or both, and where does summarization (if any) sit relative to State Authority?

Current E0 reference behavior is already explicit:

- current permitted state is freshly reconstructed each turn from authoritative `ProductionState` through deterministic Access Control;
- accepted current-Scene Character-legible Performance history is a distinct append-ordered continuity projection;
- E0 v3 includes every accepted current-Scene Performance item;
- E0 performs no recency windowing, retrieval, relevance ranking, truncation, summarization, paraphrase, token budgeting, or model compression.

Post-E0 scaling remains open. Any future window/retrieval/summarization contract must preserve Access-Control-before-relevance, causal history as source of truth, and the distinction between derived context representation and authoritative Production state. A summary must never acquire truth/Knowledge/Memory/Belief/Claim authority merely because the Context Composer produced or used it.

## Resolution timing

ODR-12, ODR-13, ODR-30, and ODR-32 do not block the current H1/E0-A deterministic reference path. They become decision gates only when later work would otherwise encode their unresolved product behavior into durable architecture.
