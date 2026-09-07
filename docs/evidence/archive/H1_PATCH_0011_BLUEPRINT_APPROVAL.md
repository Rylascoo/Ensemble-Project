# H1 Patch 0011 — Blueprint Approval Record

Date: 2026-09-03
Status: APPROVED FOR IMPLEMENTATION

## Approved contract

Canonical blueprint:
`docs/blueprint/H1_PATCH_0011_TAKE_SEMANTICS.md`

Approved proposal:
`0.15`

Exact recursively audited proposal head:
`8e939c8efe100473f3409185e9de5fde65bcc8b1`

Blueprint branch:
`h1-patch-0011-take-semantics-blueprint`

Approval PR:
`#23 — H1: approve E0 Take semantics contract`

PR #23 squash-merged the exact audited blueprint plus approval/handoff documentation without executable changes.

Canonical blueprint promotion commit on `main`:
`5ed7be9aec6c83616ab743da3081781fb97b41a2`

The approved blueprint content is the exact Proposal 0.15 document at the audited head above. Its audit-stage header is intentionally preserved as historical evidence; live approval status is established by this record and `CURRENT_STATE.md`.

## Approval authority

The user explicitly approved H1 Patch 0011 Proposal 0.15 on 2026-09-03 after the recursive adversarial audit reached one complete pass with zero material corrections and zero worthwhile architectural improvements.

Approval freezes the Patch 0011 E0 Take Semantics architecture only. It does not claim implementation, compiler, test, runtime, ARM64, NPU, WACK, or Store validation.

## Frozen Patch 0011 laws

Approval freezes at minimum:

- E0 ordering as `CandidatePerformance -> Integrity evaluation -> State Interpretation -> deterministic State Authority -> E0 Take -> later atomic causal commit`;
- no separate provisional Take object before State Interpreter / State Authority in E0;
- canonical namespace `Ensemble.E0.Core.Take`;
- canonical contract holder `E0TakeContracts` with `ContractVersion = "ensemble.e0.take.v1"`;
- reuse of the existing canonical `TakeId` strong type with no new Take identifier type or Core allocator;
- `E0Take` as an immutable sealed aggregate retaining exact `CandidatePerformance`, exact `StateInterpretationProposal`, fresh canonical `StateAuthorityEvaluation`, explicit `TakeId`, and explicit `E0TakeDisposition`;
- `E0Take.Bind(...)` as the sole rich construction/reconciliation boundary with a private `E0Take` constructor;
- deterministic re-binding of the supplied Context/Candidate/Integrity/Proposal association and fresh replay of Patch 0010 from the supplied evaluation Trace;
- supplied State Authority Status/Decisions are not independently trusted when binding a Take; the fresh deterministic replay is canonical;
- a Take can bind only after a fresh terminal `Complete` State Authority evaluation with no `RequiresReview` decision;
- `E0TakeDisposition.Unspecified = 0` and invalid/default/undefined dispositions fail closed;
- Take disposition (`Accepted`, `Rejected`, `Alternate`) remains independent from State Authority consequence disposition (`Approved`, `Rejected`, `RequiresReview`);
- Accepted Take may coexist with zero, all-approved, mixed approved/rejected, or all-rejected terminal consequence sets;
- later successful atomic commit under an Accepted Take must make the exact Performance plus every retained Approved consequence effective together and no retained Rejected consequence effective;
- Accepted means commit-eligible, not already historical or effective;
- Rejected and Alternate Takes remain immutable non-effective provenance and never advance effective opportunity/history/state;
- Integrity Reject / RequestAnotherTake and technical/provider failures do not produce an `E0Take`;
- the reference E0 orchestration policy accepts every Take-bindable package unless a separately labeled explicit intervention/test/control applies;
- Rejected/Alternate reference deviations require attributable provenance and are never inferred from State Authority decisions or model output;
- TakeId remains distinct from CandidateContentHash, ProposalContentHash, CommitId, RecordId, ContextPacketId, RunId, and future StateHash;
- no new authority-decision content hash is introduced merely to duplicate already-immutable deterministic evaluation semantics;
- later freshness handling may validate or reject the immutable Take but may not silently replace its retained Approved/Rejected consequence package under the same Take identity;
- Patch 0011 does not claim a ProductionState/StateHash/common-state proof between ContextPacket and StateAuthoritySnapshot because that identity does not yet exist;
- effective E0 orchestration must preserve fixture/current-authority provenance sufficient to explain why source Context and State Authority snapshot came from the same run-state source;
- `E0TakeException` is a public sealed typed failure domain with no public constructors; expected upstream structural failures are normalized without leaking Candidate/Context/mutation prose;
- unexpected programming/runtime failures are not converted into Take dispositions;
- Patch 0011 remains deterministic, model-free, network-free, filesystem-free, clock-free, randomness-free, and idle-work-free by design;
- Patch 0011 introduces no ProductionState, StateHash, RecordId allocation, mutation application, CommitId, atomic commit, persistence, branch/rehearsal/retcon UX, provider execution, Scene loop, Windows AI/NPU, WinUI, MSIX, WACK, or Store machinery.

## Validation boundary

This record establishes architecture approval only.

The next fresh engineering chat must implement the smallest canonical Patch 0011 source/test surface, run static/regression review, and then return to the user's native Windows ARM64 machine for compiler/test authority.

No lower validation level may be promoted into machine/runtime/hardware/package/Store evidence.
