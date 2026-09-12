# E0-B — Mixed-Model Cast Method Proposal 01

Date: 2026-09-12

Status: **PROPOSAL 0.1 — NON-PROVIDER PREPARATION COMPLETE — DIRECTOR CAST/METHOD DECISION REQUIRED — NO IMPLEMENTATION OR NETWORK AUTHORITY**

## 1. Objective and frozen authority

Blueprint 0.1 freezes E0-B as the deliberate mixed-model exception after E0-A. E0-B must assign different supported provider/models to the same three unchanged Characters and Fixture, keep each assignment fixed within the comparison batch, and compare the result against E0-A rather than treating variety as automatic improvement.

Selected E0-A reference authority is `docs/evidence/E0A_Q_E0A_03_RUN08_SELECTED_REFERENCE_DESCRIPTOR_2026_09_11.json`, sourced from Run 08 `E0A-Q03-G35L-20260911-08`.

The proposal changes only Performer casting. It does not change Character identity/history, Fixture, deterministic authority, Access/Context law, Integrity semantics, State Interpreter semantics, Take/commit semantics, opportunity selection, hard-gate criteria, retry law, or experiment order.

## 2. Reference condition inherited from Run 08

Run 08 reference profile is Google Gemini API `gemini-3.5-flash-lite`, provider profile `GEMINI-3.5-FLASH-LITE-MINIMAL`, service tier `standard`, creative reasoning level `minimal`, Integrity reasoning `high`, 12 accepted Turns, one attempt per probabilistic invocation, zero automatic retries, 300-second attempt timeout, 4096 visible-output-token ceiling, and USD 5.00 shadow-spend ceiling.

Fixture remains `ensemble.e0.missing-raft@0.1.0` with canonical Fixture hash `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`.

The exact Character definitions remain `MARLOWE`, `VOSS`, and `WREN`; the initial Opportunity remains `VOSS`.

## 3. Proposed single-variable mixed cast

Recommended E0-B condition identifier: `E0B-MIXED-CAST-01`.
| Surface | Proposed fixed assignment |
|---|---|
| `VOSS` Performer | `gemini-3.1-flash-lite` / `GEMINI-3.1-FLASH-LITE-MINIMAL` / native `minimal` |
| `MARLOWE` Performer | Run 08 reference `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL` / native `minimal` |
| `WREN` Performer | Run 08 reference `gemini-3.5-flash-lite` / `GEMINI-3.5-FLASH-LITE-MINIMAL` / native `minimal` |
| Integrity, every Turn | Run 08 reference 3.5 Flash-Lite / `high` |
| Interpreter, every Turn | Run 08 reference 3.5 Flash-Lite / `minimal` |

`VOSS` is the recommended alternate-model Character because the frozen Fixture gives `VOSS` the initial Opportunity. The mixed route is therefore exercised immediately rather than depending on later Director routing.

Only one Performer is recast. This is the smallest condition that is genuinely mixed-model and minimizes simultaneous provider/model variation while preserving the three-Character cast.

The proposal deliberately uses the same provider family for both Performer routes. It therefore tests model diversity without adding a second provider-policy, credential, endpoint, or transport family as an unnecessary confound.

## 4. Why 3.1 is the recommended alternate route

The current Harness already contains live-selectable Gemini 3.5 and 3.1 Flash-Lite profiles. Both use the same semantic creative intent `LeastDeliberativeSupported`, map to provider-native `minimal`, support the 12-Turn experimental ceiling, and have prior live transport/structured-output evidence.

The frozen 2.5 Flash-Lite route is not recommended for this E0-B condition. Its executable profile is limited to 20 RPD and a 3-Turn control envelope, uses a different budget-style thinking control, and Run 09 terminated on its first Performer `countTokens` request with HTTP 404 / `NOT_FOUND`. Including it would change quota capacity, reasoning-control semantics, and compatibility state in addition to Performer identity.

No claim is made that 3.1 and 3.5 are behaviorally equivalent. Their difference is the deliberate E0-B variable.

## 5. Required matched variables
The following remain exact Run 08 reference law unless a later Director decision explicitly changes them:

- Missing Raft Fixture bytes/identity/hash and initial state;
- Character definitions, constitutions, knowledge/access boundaries, and initial Opportunity;
- Context composition/rendering, Performer prompt/schema, Integrity packet/prompt/schema, Interpreter prompt/schema;
- deterministic Director, Integrity binding, State Authority, Take, atomic commit, and next-Opportunity semantics;
- accepted-Turn cap 12, one attempt per probabilistic invocation, zero automatic retries/fallback, 300-second role deadline;
- 4096 visible candidate/output ceiling and USD 5.00 total shadow-spend ceiling;
- cancellation/failure/refusal rules, immutable evidence, blind-transcript rules, runtime sealing, and hard-gate checklist.

E0-B does not create Character identity from model identity. The cast map is experimental infrastructure provenance only.

## 6. Required Harness amendment before implementation can be authorized

The current E0-A envelope cannot represent E0-B truthfully because it enforces one provider/model/tier across all three roles, exposes one Performer profile for every Character, carries one run-level `providerProfileId`, constructs one rate discipline from one model profile, prices the run under one profile, and treats one returned model identity as invariant for the entire run.

A minimal E0-B implementation must therefore add, outside Core:

1. an immutable CharacterId -> Performer provider-profile map validated against the exact three-Character Fixture roster;
2. per-attempt Performer profile resolution from the current Context subject Character;
3. separate observed-model identity invariants per approved route rather than one whole-run returned-model value;
4. route-specific quota/accounting state plus one conservative shared-project aggregate request discipline across all selected Gemini routes. Cross-route concurrency or pacing gain must not be assumed without authoritative quota evidence; route-specific TPM/RPD limits and current capacity remain separately enforced;
5. per-attempt pricing/reservation/reconciliation against the profile actually used while preserving one deterministic USD 5.00 run ceiling;
6. manifest provenance that records the complete fixed Performer cast and the unchanged Integrity/Interpreter reference profiles;
7. tests proving model/profile evidence cannot cross Character, Turn, role, or route boundaries.

Core must remain byte-for-byte unchanged unless this preparation is falsified by an unavoidable Core dependency; such a finding would stop the package for separate architecture review rather than silently expanding scope.

## 7. Evidence and comparison
E0-B must preserve its own immutable run identity and remain separately labeled from E0-A. The comparison package must retain exact cast assignments, attempt/model receipts, usage/cost, failures, runtime seal, hard-gate evaluation, accepted transcript, and blind mapping.

Before any E0-B provider-bound transcript can exist, the experiential scoring instrument must be frozen. Reuse the exact ten-dimension blind E0 rubric already frozen for the E0-E comparison contract: Character distinction, social causality, Character agency, relationship expression, coherence, earned surprise, assistant-convergence avoidance, anticipation, memorability, and mechanical invisibility. This reuse supplies a common E0 comparison language; it does not import E0-E's playwright-specific interpretation.

Run 08 and a contributing E0-B transcript must be presented under neutral LEFT/RIGHT identities with the existing neutral speaker-label normalization. The scorer records LEFT, RIGHT, TIE, or INDETERMINATE per dimension plus brief evidence; no weighted master score is computed before unblinding. Scores are sealed before the mapping is revealed. Hard-gate-invalid runs receive no experiential credit, while failure remains condition evidence.

The E0-B behavioral result is compared against the selected Run 08 E0-A reference. Provider/model diversity is not scored as inherently better; the question is whether mixed Performer casting enriches portrayal without replacing Character identity or destabilizing access, causal intelligibility, integrity, or deterministic authority.

No clean-run cherry-picking is permitted. Failed/noncontributing E0-B attempts remain visible condition evidence under the same immutability principle used for E0-A.

## 8. Activation boundary

This proposal authorizes no source change, credential access, provider request, inference, or spend.

If the Director approves the cast/method, Engineering may implement only the minimal Harness/evidence amendments above on an isolated branch, run fake/credentialless tests, recursively audit them, and obtain fresh native Windows ARM64 validation/tagging before any live E0-B request.

Live execution then still requires a separate current activation record covering the exact validated checkout, run identity/evidence root, protected credential readiness, current model lifecycle/pricing/data-use facts, authenticated project/tier/quota/capacity for every selected route, and applicable Director/provider authority.

## 9. Director decision requested

Approve, modify, or reject the following coupled method:

> **E0B-MIXED-CAST-01:** preserve Run 08 in every feasible variable; recast only `VOSS` Performer from Gemini 3.5 Flash-Lite Minimal to Gemini 3.1 Flash-Lite Minimal; keep `MARLOWE` and `WREN` Performers plus Integrity and Interpreter on the Run 08 3.5 reference route; implement exact per-Character/per-route provenance, model-identity, quota and cost accounting; retain conservative shared-project request pacing; and freeze the blind Run-08-vs-E0-B scoring instrument before any live transcript.

No implementation decision should be inferred from silence or from this proposal's presence in the repository.
