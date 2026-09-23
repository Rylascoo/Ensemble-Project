# Scene presentation sufficiency decision

Status: **DESIGN STATIC REVIEW COMPLETE / DIRECTOR REVIEW PENDING / NO IMPLEMENTATION AUTHORITY**

Date: 2026-09-23. Package: **Q-DESIGN-24**.
Contract: `docs/design/app/contracts/SCENE_INITIAL_ROSTER_PRESENTATION.md`.

## Director disposition and ownership

The Director's 2026-09-23 task disposition selected a bounded Scene presentation-contract package and authorized presentation-only treatment derived from integrated Q-PROD-08 truth. It explicitly prohibited implementation, new Product/Persistence schema, persistent Scene naming, currentness/lifecycle/roster mutation and the other exclusions preserved in the contract. It required a stop and smallest exact Product question if presentation-only distinction proved insufficient.

Primary Design author owns the contract and this static decision. The same task performs a separate bounded Governance/Recovery responsibility for package registration and two explicitly authorized documentation discrepancies; that does not confer implementation authority. Director adoption, merge and any later implementation remain separate. No Product implementation successor is created.

## Exact recovery and source evidence

- Live main and clean opening checkout: `a225fc56acc1ef4514538562491d28d7a6bb4fb4`.
- Exact-main Validation gate #1294, run `35898340730`: completed/success; current-state distance 1.
- All 14 live branch refs freshly resolved: main; 12 ancestral completed-work refs; preserved divergent historical PR #226 head `dc1780e46ef999eb01e214671486b83186ca04c4`. No successor implementation is inferred from them. Deferred local residue remains Q-ADMIN-07; no disposition performed.
- Q-PROD-08 native authority: `31e16cbe274f9b90192bba67cd443fa3320ed8e2`; Q-ADMIN-08 .NET 10 native authority: `4b2d286a4baf1bb01b222aceda26fbaae8772c15`. Later platform-baseline changes do not promote this Design reasoning into runtime proof.
- `SceneIdentity.cs`: opaque identity, immutable initial roster, distinct identities permit identical rosters; no name/currentness.
- `ProductionReplay.cs`: journal-derived Scene collection; initial roster references existing Cast and canonicalizes by Cast order.
- `ProductApplication.cs` / `FileProductionCatalog.cs`: explicit establishment, exact created Scene + replay, existing Invalid/Incompatible results; ProductSpace, World and Cast preserved. Exact replay derives under the append gate after the Q-PROD-08 repair.
- `tests/Kymaean.Application.Tests/SceneReplayTests.cs`, `ProductApplicationSceneTests.cs` and `tests/Kymaean.Infrastructure.Persistence.Tests/ScenePersistenceTests.cs`: existing coverage for empty/large rosters, identity invariants, replay, navigation preservation, recovery/export and snapshot reconstruction. Read, not rerun in this documentation package.
- Q-DESIGN-23 and `src/Kymaean.Windows/Presentation/PresentationIdentityCode.cs`: fingerprint and Character-name disambiguation precedent only; no existing Scene UI implementation is claimed.

## Alternatives and decision

| Treatment | Disposition | Reason |
|---|---|---|
| Roster names alone | Rejected | Empty and identical rosters cannot distinguish Scene identities. |
| Scene 1 / Scene 2 as primary identity | Rejected | Encourages dramatic-order interpretation and binds focus/recognition to position. |
| Raw opaque Scene ID | Rejected | Leaks technical representation and does not use the established creator-facing identity convention. |
| Persistent creator-authored title | Outside grant | Adds Product schema before evidence proves necessity. |
| Derived Scene code + separately inspectable initial roster | Selected for Director review | Distinguishes exact records without adding Product truth; supports existing identity-based focus and duplicate-name treatment. |

The code is always visible for Scenes. Prefix uniqueness is evaluated across the full Production Scene collection. Character disambiguation uses the full Cast even in subset rosters. These choices prevent view-dependent ambiguity. Code length may grow after a collision; the contract does not promise immutable short codes.

## Static acceptance / falsification matrix

All PASS entries below mean **contract-level reasoning PASS**, not executed UI, browser, accessibility or native evidence.

| Case | Contract witness | Static result |
|---|---|---|
| No Scenes | Ordinary empty state; New Scene available | PASS |
| Empty Cast / roster | Explicit legal empty establishment; no invented minimum | PASS |
| Two empty or identical-roster Scenes | Different identity-derived codes; actions keyed by identity | PASS |
| Multiple duplicate Character names | Full-Cast conditional codes on checkbox, detail and submission witness | PASS |
| Reverse draft selection order / more than five members | Review and return use canonical Cast order; no cap/rank | PASS |
| Prefix collision | Extend codes across full collection; recompute all labels together | PASS |
| Full-hash collision | Block ambiguous actions, return presentation defect; no invented identity law | PASS as fail-closed boundary, not successful usability |
| Focus/return after code extension | Track opaque identity internally; heading fallback if absent | PASS |
| Typed Invalid/Incompatible | Preserve classifications; Invalid retains witness and blocks further establishment until ordinary Open | PASS |
| Append succeeds but Application rejects replay against stale caller state | Typed Invalid does not prove non-commit or corruption; same conservative reopening boundary | PASS |
| Environmental non-confirmation | Last-confirmed/Submitted separation; no guessed Scene code | PASS |
| Reopen after uncertain identical-roster submission | No roster-based attribution; ordinary replay only, no automatic retry | PASS |
| Navigation during uncertainty | Session block retained until successful ordinary Open | PASS |
| Creator demands memorable persistent labels | Not claimed satisfied by codes; smallest naming question returns to Director | Explicit falsifier |

## Recursive audit and limits

First pass removed three tempting authority leaks: position-based Scene naming, roster-as-identity matching after uncertain submission, and per-roster duplicate-name grouping that could hide a Character code. A further pass reconciled Q-PROD-08's append repair rather than copying stale post-append-read mechanics from the older Character presentation contract.

Independent static review of candidate `4317bfa54213da9f99e3c32b21d3c7987e814f45` against the exact baseline found one P2 issue: typed Invalid can follow successful persistence append when Application rejects replay against stale caller state. The correction preserves the typed result, avoids claiming corruption/non-commit, retains the submission witness and blocks another establishment until successful ordinary Open. The static matrix and future acceptance criteria now explicitly cover this case. Reviewer policy was workspace-write/automatic approval review; review remained behaviorally no-write, with no tests or escalation and no proven technical write containment. The author restarted the failure/identity/scope audit after correction and found no additional required Product primitive.

Design accepts **static sufficiency for Director review only**. No persistent name or current-Scene decision is requested on this evidence. This does not establish empirical usability, memorability, screen-reader delivery, native focus/layout, collision implementation, High Contrast, runtime or Director adoption. Those remain NOT TESTED / NOT ADOPTED as applicable. No render, provider call, test run or executable change was performed for this Design decision.

Deferred E0 exposure is inherited rather than removed: no direct identity/initial-roster dependency on the frozen E0-D/E methods; E0-F failure semantics and E0-G generalization can still require changes; final convergence can replace provisional Scene semantics. There is no Scene-event migration adapter. This package introduces no new event or persistence obligation to shield from that later evidence.

## Continuity repairs and next gate

The hypothesis ledger's stale .NET 10 approval sentence is replaced with the integrated Q-ADMIN-08 fact and its evidence link. The Design bootstrap now recognizes Q-PROD-08 identity/initial-roster Product truth while keeping Scene presentation unimplemented and this contract pending Director review. Neither repair changes authority.

Q-DESIGN-24 is registered as awaiting Director review. The next decision is whether to adopt this bounded contract or invoke its falsifier with the smallest missing Product requirement. No Windows implementation, merge or Product successor is authorized by this return.
