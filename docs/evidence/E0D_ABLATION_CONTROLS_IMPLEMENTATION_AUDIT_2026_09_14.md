# E0-D Ablation Controls Implementation Audit

Date: 2026-09-14

Status: **PASS — DIRECTOR-AUTHORIZED EXPERIMENT-ONLY IMPLEMENTATION COMPLETE — LIVE ACTIVATION / PROVIDER TRAFFIC BLOCKED**

## Authority and identity

Frozen method: `docs/blueprint/E0D_ABLATION_CONTROLS_METHOD_AND_ISOLATION_CONTRACT_01.md`.

Preregistration: `docs/evidence/E0D_Q_E0D_01_PREREGISTRATION_2026_09_14.json`.

Preparation recursive audit: docs/evidence/E0D_Q_E0D_01_PREPARATION_RECURSIVE_AUDIT_2026_09_14.md.

Director authorization: `docs/evidence/E0D_IMPLEMENTATION_NATIVE_VALIDATION_DIRECTOR_AUTHORIZATION_2026_09_14.md`.

Current-main implementation base after source-neutral Administrator reconciliation: `0c2ef2c0e8e141a03237ed4eaf00ce8b56c75a7a`.

Initial source freeze: `56a441096538d526c678981f7ba0cc865e0e4616`.

Rebased implementation source: `2afda26825d7ecad11677f858d523300419aa23a`; `src/**` and `tests/**` are identical to the initial source freeze.

Prevalidation authority checkpoint: `5ce587b6c9ec83b91f3b5d63a2545cce9b1fe891`; it adds only repository authority/evidence documentation and does not change source/tests.

## Implemented isolation

The E0-D layer exposes only the four frozen variants: Full reference, relationship omission, omniscient context, and deterministic round-robin.

- **Full reference** delegates to the existing deterministic Context/Opportunity/Turn/commit path; the reference composer itself has zero implementation diff.
- **Relationship omission** removes only the frozen 13-record `RelationshipContextRecordIds` surface and otherwise preserves the reference projection/history/authority path.
- **Omniscient context** is the category-preserving union of the three safe Character Access projections. It does not expose denied/provenance/credential/technical data and uses the frozen variant-specific hard-gate profile.
- **Round-robin** preserves genesis `VOSS` and then advances deterministically by canonical roster order. It carries its own explicit strategy Evaluation/Trace while nomination/address metadata remains noncontrolling evidence.

## Exact replay / authority preservation

Context ablations use experiment-only composition/replay helpers. Generic canonical/turn/causal seams recognize only the two frozen E0-D composition contracts and require exact deterministic recomposition; unsupported contracts still fail closed.

Causal binding and accepted-performance-history continuity retain state/scene/roster/history/authority validation and exact Context identity. Full and round-robin continue through the original reference turn/commit path.

The harness adds a dedicated `e0d-run` host with the Run-08 reference envelope hard-bound in code (`CREATIVE-MINIMAL`, Gemini 3.5 Flash-Lite Minimal). No arbitrary E0-D provider/model selector exists. E0-D manifests are variant-bound; `e0d-evaluate` verifies the sealed manifest variant before applying the corresponding hard-gate checklist.

## Deterministic proof

Native ARM64 deterministic coverage includes:

- exact Full reference Context identity and wrapper behavior;
- exact 13-record relationship omission plus a full 12-turn scripted-provider run;
- exact category-preserving omniscient safe-projection union plus a full 12-turn/evaluation-seal run;
- exact 12-turn round-robin subject schedule and strategy trace;
- variant-bound manifest/checklist provenance and omniscient Integrity constraints;
- exact experimental turn replay, causal commit, and accepted-history continuity.

The final prevalidation native suites reached Core **626/626** and Harness **154/154** with scripted providers and zero network traffic.

## Recursive audit findings corrected before freeze

The implementation audit caught and corrected: missing variant-to-manifest evaluation binding; an experimental composer initially placed inside the reference composer; absent round-robin Evaluation/Trace; generic sealing that initially recognized only the E0-A checklist; reference-context replay assumptions in turn, causal binding, and accepted-history continuity; and missing end-to-end Harness coverage for Full and relationship-omission variants.

Each correction was made before the validation tag. No future provider/transcript result informed the implementation.

## Provider boundary

Credentialless validation reaches the existing Gemini credential edge and fails before evidence-root creation or network activity. No E0-D live RunId, namespace, execution window, credential use, provider probe, `countTokens`, generation, inference, or spend is authorized by this implementation.

## Conclusion

The frozen E0-D architecture-ablation implementation satisfies the preregistered single-variable isolation contract and is suitable for exact native validation/integration. This audit authorizes neither live E0-D execution nor scoring. Those remain separate Director gates after integration.
