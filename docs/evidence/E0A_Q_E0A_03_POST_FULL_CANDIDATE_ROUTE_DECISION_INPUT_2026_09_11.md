# Q-E0A-03 Post-Full-Candidate Route Decision Input

Date: 2026-09-11

Status: **INPUT ONLY — DIRECTOR DECISION REQUIRED — PROVIDER TRAFFIC NOT AUTHORIZED**

This is an Engineering/Evidence input to the Director-owned provider/admissibility decision. It does not select a route, authorize spend or provider traffic, amend the frozen experiment, or advance Q-E0A-03.

## Decision boundary

The frozen closure contract has reached its explicit `neither full candidate contributes` branch. Q-E0A-03 remains open/blocked until evidence is diagnosed and a separately audited correction/route decision is durable.

- 3.5 Flash-Lite Run 06: first generation HTTP 503 / `UNAVAILABLE`, `0/12` accepted turns.
- 3.1 Flash-Lite Run 07: seven successful exact-model generations and two committed turns, then Turn 3 Integrity generation HTTP 503 / `UNAVAILABLE`, `2/12` accepted turns.
- Both runs are immutable/noncontributing and may never be replayed.
- Neither terminal is a credential, quota-429, structured-output, parser, canonical-ID, deterministic-authority, or local semantic failure.
- Provider traffic is zero after Run 07.

Run 07 terminal evidence: `docs/evidence/E0A_Q_E0A_03_G31L_RUN07_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`.
Run 06 terminal evidence: `docs/evidence/E0A_Q_E0A_03_G35L_RUN06_TERMINAL_EVIDENCE_ANALYSIS_2026_09_11.md`.
Frozen closure law: `docs/blueprint/E0A_Q_E0A_03_REFERENCE_EVIDENCE_ACTIVATION_AND_CLOSURE_CONTRACT.md`.

## Diagnosis

Google's current Gemini API error documentation classifies HTTP 503 / `UNAVAILABLE` as service temporarily overloaded or down and recommends retry with exponential backoff. That provider recommendation is evidence that 503 is normally treated as transient availability failure; it does not establish why either Ensemble request failed or guarantee a later request would succeed.

Official current sources checked 2026-09-11:

- `https://ai.google.dev/gemini-api/docs/api-errors`
- `https://ai.google.dev/gemini-api/docs/troubleshooting`
- `https://ai.google.dev/gemini-api/docs/models`
- `https://ai.google.dev/gemini-api/docs/deprecations`
- `https://ai.google.dev/gemini-api/docs/optimization`
- `https://ai.google.dev/gemini-api/docs/generate-content/priority-inference`
- `https://ai.google.dev/gemini-api/docs/pricing`

The E0-A reference envelope deliberately differs from ordinary provider retry guidance: one attempt per probabilistic role invocation, zero automatic retries, and any provider/timeout/refusal/incomplete failure terminates the run without replacement. The blueprint explicitly defers retry/failure variation to E0-F. Failed-run frequency must remain visible and clean-run selection may not hide instability.

Run 07 materially weakens a model/protocol incompatibility explanation because Performer, Integrity, and Interpreter all produced valid 3.1 structured outputs before the terminal. Run 06 and Run 07 place separate 503 terminals on different admitted full-candidate models and different role/streaming positions. The common factor is the Standard Gemini Developer API route, but two observations are insufficient to infer persistent route failure or provider root cause.

## Current executable constraint

The promoted executable is not route-neutral at the service-tier/billing layer. It fixes `serviceTier = standard`, validates all role profiles against that tier, omits the service-tier request field as provider-default Standard, and records the actual route expectation as the verified synthetic Free-tier AI Studio route.

Therefore changing to paid Standard, Priority inference, Flex, another endpoint/provider, or another model outside the admitted profiles requires an audited amendment and renewed native validation as applicable; it is not an account-only switch.

## Route options

| Option | Route | Engineering assessment |
|---|---|---|
| A | Exactly one fresh 3.5 Flash-Lite Standard/Free full-candidate replacement | **Recommended.** Lowest drift: preserves the admitted model/profile, Standard route, one-attempt/zero-retry law, 12-turn contribution law, synthetic Fixture, and current executable if all freshness/native/account gates still hold. It must be preregistered as one replacement only, never retry-until-clean. |
| B | Paid Standard on an admitted model | Not recommended solely to answer the 503 problem. Billing/data-use/quota and current manifest assumptions change, while current public evidence does not establish that Standard paid traffic eliminates availability-class 503s. Requires audited amendment/validation and Director paid-route authorization. |
| C | Gemini Priority inference | Strongest current Google reliability-oriented route: documented as highest-reliability/non-sheddable and supported by 3.5/3.1/2.5 Flash-Lite, but available only on higher paid tiers. It changes service-tier request/receipt semantics, billing and route identity; Priority overflow may downgrade to Standard. Requires explicit route-drift law, source/tests, renewed native validation, account-tier eligibility and Director spend/admission approval. |
| D | Add retry/backoff inside E0-A | **Not recommended.** It follows ordinary provider guidance for 503 but violates the frozen one-attempt/zero-retry reference envelope, can hide instability, and imports an E0-F failure/retry variable into E0-A. |
| E | Start the 2.5 three-turn control | **Not a reference-unblocking route.** Frozen law says it cannot substitute for a full candidate. It remains separately dispositioned after the full-reference route is resolved. |
| F | Another provider or non-admitted model | Possible only as a major provider/envelope redesign. Requires criteria-based provider admission, comparability rationale, new adapter/profile work as applicable, and renewed validation. Higher semantic and schedule disruption. |
| G | Stop Standard reference acquisition and redesign E0-A before another call | Valid Director choice if avoiding further provider consumption is preferred. Preserves evidence integrity but has the largest immediate schedule/architecture impact. |

## Engineering recommendation

Recommend **Option A: authorize exactly one fresh 3.5 Flash-Lite Standard/Free replacement full candidate**, under a new RunId/evidence root and the unchanged one-attempt/zero-retry envelope.

This is not a claim that Run 06 should be erased or retried. Run 06 remains an immutable failed observation. The proposal is a separately decided replacement experiment after both planned full candidates failed to contribute for availability-class reasons.

Reasons to prefer A over the other provider-call options:

1. It introduces no retry/backoff variable and does not lower the 12-turn contribution requirement.
2. It has the smallest code/contract drift. If execution occurs while the current pricing/data-use freshness guard remains valid through 2026-09-14 and no contrary provider/account/native fact appears, the exact `bb869fb...` executable can remain eligible; otherwise the applicable snapshot/change must be audited and revalidated before traffic.
3. Historical 3.5 evidence contains repeated successful generation/structured-output/accounting behavior, while Run 06's hardened diagnostic localized its final observation to availability rather than local incompatibility.
4. Current Google lifecycle documentation names `gemini-3.5-flash-lite` as the recommended replacement for 3.1 Flash-Lite and currently publishes no 3.5 Flash-Lite shutdown date; 3.1 Flash-Lite has an earliest shutdown date of 2027-05-07.
5. It avoids treating billing elevation as a reliability fix without evidence and avoids introducing Priority service-tier semantics before they are experimentally needed.

### Required hard stop if A is selected

The replacement must be **one and only one** fresh 3.5 Standard/Free full-candidate run. Before traffic it requires a separately durable exact activation with fresh RunId/root, current model/lifecycle/pricing/data-use/account/project/key/quota checks, exact executable/tag/Fixture/profile, and no-retry/no-fallback boundaries.

If that replacement terminates technically before 12 accepted turns, no further Standard/Free replacement run is earned under this decision. Provider traffic returns to zero and the Director must choose a materially different route/redesign disposition. This prevents retry-until-clean selection bias while allowing one explicit test of the transient-availability diagnosis.

If it reaches 12 accepted turns, contribution still requires the existing independent hard-gate evaluation and valid evaluation seal. Any reference-selection record must report Runs 03-07 and the replacement, including all noncontributing/failed runs rather than presenting only the successful transcript.

The 2.5 control remains outside this approval. Its execution or omission still requires the explicit disposition required by the frozen closure contract.

## Director decision requested

Choose one disposition after this input is integrated:

- **Approve A** — permit Engineering to preregister and activate one fresh 3.5 Flash-Lite Standard/Free replacement full candidate under the hard stop above. Approval of A does not itself launch traffic; exact activation and current gates still precede the call.
- **Choose B, C, F, or G** — Engineering will first draft the corresponding audited amendment/provider-admission package; no provider traffic occurs from this input.
- **Reject further provider work for now** — Q-E0A-03 remains blocked and all existing evidence remains immutable.

Engineering does **not** recommend D. E is not a substitute for the missing reference and is not included in any replacement authorization.

Until the Director decision is made durable, Q-E0A-03 remains **OPEN/BLOCKED** and provider traffic remains **NOT AUTHORIZED**.
