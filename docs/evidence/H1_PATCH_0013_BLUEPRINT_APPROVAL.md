# H1 Patch 0013 — Blueprint Approval Record

Date: 2026-09-03
Status: APPROVED FOR IMPLEMENTATION HANDOFF

## Approved contract

Canonical blueprint:
`docs/blueprint/H1_PATCH_0013_EFFECTIVE_OPPORTUNITY_AUTHORITY.md`

Approved proposal:
`0.6`

Exact recursively audited proposal head:
`a060ce71c9a1dfa5ae9d9faec6b03b7a07f7d781`

Blueprint branch:
`h1-patch-0013-effective-opportunity-authority-blueprint`

Parent repository checkpoint used for architecture work:
`main` at `8c89f998fe6f42e04a75b9090fbcc10f0574f5a2`

Parent machine-tested executable/test authority:
H1 Patch 0012 at `39bc078c130ab1165c6a81c1673dd5cd25da3724`

## Approval authority

The user explicitly approved H1 Patch 0013 Proposal 0.6 on 2026-09-03 after recursive adversarial review reached a complete pass with zero material corrections and zero worthwhile architectural improvements.

Approval freezes the Patch 0013 Effective Opportunity Authority architecture only. It does not claim implementation, compilation, tests, runtime behavior, ARM64 execution, NPU execution, WACK, packaging, or Store validation.

## Frozen Patch 0013 laws

Approval freezes at minimum:

- Patch 0013 owns only the postcommit E0 effective-opportunity authority reserved by Patch 0007;
- Patch 0012 remains closed: an Accepted Take commit consumes the source Current Opportunity and yields a no-opportunity Production state;
- the next Character may not enter Access/Context/Performer until a successful Patch 0013 transition establishes effective Current Opportunity;
- live Patch 0013 authority must prove the exact source causal chain, bind the closed OpportunityHistory projection to the source commit parent state, re-Bind the accepted source Context/Candidate after commit, and recompute the existing least-intervention Director after commit;
- speculative/precommit Director evaluation is never promoted into effective routing authority;
- Current Opportunity establishment and its corresponding opportunity-history advancement are one coherent authority result: either Event + State + History are returned together or no effective opportunity transition exists;
- `ProductionState` remains the authoritative immutable current projection;
- existing `StateHash` remains the single history-sensitive causal state identity; Patch 0013 introduces no second history hash chain;
- `E0OpportunityHistory` is closed-construction, initializes only from an exact genesis Production state, and may advance only through successful opportunity transition/replay;
- `LastOpportunityStateHash` identifies the exact Production state in which the final history Character became effective Current Opportunity;
- authoritative anti-splice binding requires `sourceHistory.LastOpportunityStateHash == sourceCommit.ParentStateHash` and the final history Character to equal the accepted source Character;
- OpportunityHistory remains outside `ProductionStateProjection` so the validated Patch 0012 projection bytes and genesis/causal-commit hash preimages remain byte-for-byte unchanged;
- Patch 0013 adds only the disjoint `kind = opportunityTransition` StateHash envelope under the existing StateHash contract;
- existing Patch 0012 genesis and causal-commit hashes/oracles must remain unchanged;
- the existing `LeastInterventionDirector` reference strategy is the only live Patch 0013 strategy;
- `ensemble.e0.director.least-intervention.v1` becomes replay-stable causal semantics and may not be silently reinterpreted later;
- source Candidate addressed/nominated control is already causally bound by the accepted Take/Candidate content identity and the source causal commit parent chain; the opportunity event does not duplicate it;
- the opportunity event is minimal and does not duplicate source CommitId/TakeId, Candidate control, full OpportunityHistory, Director diagnostics, Context prose, Performance prose, or State mutation prose;
- the narrow Production mutation helper may establish only a roster Character from null Current Opportunity and must not accept an arbitrary replacement Production projection;
- effective opportunity transition changes exactly one Production projection field: `CurrentOpportunityCharacterId: null -> selected Character`;
- effective CommitId/TakeId caches remain unchanged through the opportunity transition;
- one-step deterministic opportunity replay is required and must fail closed on parent/source/history/event mismatch;
- replay may reconstruct the deterministic Director evaluation from source commit Candidate control plus authoritative source OpportunityHistory and does not require retained private Context prose;
- Patch 0013 does not claim evolved Production->Access/Context, common Context StateHash identity, CharacterClaim disclosure, recent accepted Performance injection, complete Scene loop, full multi-event session replay, persistence/recovery, branches/canon/retcon/rehearsal, alternate promotion, new Director strategies, Observation, World Resolver, provider execution, WinUI, Windows AI/NPU, packaging, WACK, or Store behavior;
- Patch 0013 introduces no filesystem/network/clock/random/provider/GPU/NPU/background-polling/global-mutable-state work;
- ARM64 suitability is architectural only: deterministic immutable Core work, bounded structural copies/hashing, no idle execution, and no hardware-specific dependency. No measured power or hardware claim is established by architecture approval.

## Validation boundary

This record establishes architecture approval only.

Implementation must remain patch-first, preserve all machine-validated Patch 0012 behavior and hash oracles, complete the blueprint's required test matrix, undergo recursive static review, and then return to the user's native Windows ARM64 machine for compiler/test authority.

No lower validation level may be promoted into runtime, hardware, package, WACK, or Store evidence.
