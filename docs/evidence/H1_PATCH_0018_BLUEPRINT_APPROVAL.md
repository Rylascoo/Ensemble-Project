# H1 Patch 0018 Blueprint Approval

Date: 2026-09-05

Approved architecture: `docs/blueprint/H1_PATCH_0018_DETERMINISTIC_TURN_ORCHESTRATION.md`
Approved proposal: **0.6**
Parent `main`: `0548078a060267e136968c2bf97b384356b50554`
Blueprint approval commit: `75e7ebb43a8a44c04374b8cc8bff3e9ae7b92597`

Director approval was granted in the Ensemble project conversation by the explicit continuation after the approval recommendation.

Approval covers only the exact recursively audited Proposal 0.6 contract:

- deterministic Turn state machine over existing Core semantic authorities;
- fresh Context recomposition and Patch 0017 attempt replay;
- typed technical/cancelled/request-another/review/take-bindable/accepted-ready states;
- same-proposal/same-policy State Authority review continuation;
- TakeId introduced only after terminal State Authority;
- reference E0 Accepted Take binding only;
- accepted causal commit ending at `E0PostCommitCycleState`;
- explicit later Patch 0016 Opportunity establishment remains separate.

Implementation must not add provider/model execution, raw provider transport, retry/spend policy, persistence, Scene lifecycle, repeated run-loop behavior, post-E0 Context optimization, UI/platform/package work, or any lower-authority semantic redesign.

Any implementation need to change Patch 0017, Integrity, State Interpreter, State Authority, Take, Cycle, Opportunity, continuity, canonicalization, framework, or SDK semantics reopens architecture.

Validation authority remains separate from architecture approval. Native Windows ARM64 compiler/runtime claims require Director-machine evidence.
