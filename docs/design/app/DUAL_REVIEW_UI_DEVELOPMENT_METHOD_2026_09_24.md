# Application UI dual-review development method

Status: **DIRECTOR-ADOPTED DESIGN METHOD / NO PRODUCT SEMANTIC AUTHORITY**

Date: 2026-09-24

## Purpose

Use Director Preview as the live native design surface while removing the Director as a routine UI diagnostic or regression bottleneck.

The Blueprint, current Product truth and active App Design contracts define the design problem. Design owns diagnosis. Independent AI review provides adversarial second-pass evidence. Automation owns deterministic behavior. Human Director input is reserved for irreducible taste/Product decisions and claims that inherently require a human observer.

## Roles

### Design Sol

Design Sol owns App Design interpretation and acceptance within current authority.

For each bounded workflow, Design Sol:

- recovers current Product/Design authority and exact native candidate;
- audits hierarchy, information scent, affordances, copy, density, rhythm, interaction flow, responsive behavior and consistency;
- reconciles Blueprint intent, Product semantics, prior evidence and falsifiers;
- classifies independent-review findings;
- selects the smallest bounded correction;
- never invents Product semantics to repair a presentation problem.

### Claude Code — independent UI/UX reviewer

Claude Code is an independent read-only reviewer, not Design authority and not an implementation writer for the reviewed pass.

Give it the exact immutable source, relevant native screenshots/state, Blueprint/Design contracts, Product boundary and current falsifier. Its job is to search for defects that the primary Design loop may have normalized.

Claude may identify:

- discoverability and information-scent failures;
- hierarchy, density, spacing and visual-rhythm problems;
- misleading or weak affordances;
- unnecessary complexity or implementation-detail leakage;
- inconsistent interaction patterns;
- copy/label ambiguity;
- accessibility and keyboard/focus risks;
- mismatch between implemented UI and supplied Design/Product authority.

Claude must distinguish concrete defects from subjective alternatives, cite the supplied evidence it relies on, avoid inventing Product semantics, and recommend the smallest correction. Design Sol independently reconciles the result; Claude findings never self-promote to authority.

### Engineering / automation

Automation owns deterministic claims: persistence/replay, identity, state guards, keyboard/focus mechanics, responsive/layout checks where automatable, theme parity, profile preservation, native builds and regressions.

A failed automation claim is evidence of a defect or NOT TESTED. It is never delegated to the Director.

### Director

The Director is not the UI diagnostician or regression harness.

Director input is required only when:

- Product/taste/brand choice is irreducible;
- competing Design directions remain valid after Design review;
- a claim explicitly concerns human comprehension, discoverability or preference and cannot be established by expert/automated evidence;
- a milestone human falsifier is useful after the design has already survived the primary review loop.

When human evidence is required, ask the Director to use the interface naturally or choose among bounded alternatives. Record what happened. Do not ask the Director to explain the cause, prescribe the fix or perform Design diagnosis.

## Default development loop

1. Recover exact Blueprint/Product/Design authority.
2. Define one complete creator workflow and its normal, empty and stress/falsifier states.
3. Materialize it in native Director Preview.
4. Design Sol performs the primary recursive UI/UX audit.
5. Claude Code independently reviews the same immutable native/source state.
6. Design Sol reconciles both reviews against authority and selects the smallest correction.
7. Implementation applies only that bounded correction.
8. Automated/native regression checks run.
9. Refresh Director Preview and repeat until both reviews are clean enough for the current bounded scope.
10. Freeze the earned state. Use the Director only for a remaining irreducible decision or milestone human claim.

Human tasks are therefore **falsifiers/checkpoints, not the ordinary iteration engine**.

## Fixed Claude review brief

Use this brief unless a narrower claim needs additional constraints:

> Review this exact native KYMÆAN UI state against the supplied Blueprint, current Product truth and App Design contracts. Assume the implementation may be wrong. Identify discoverability failures, hierarchy problems, weak or misleading affordances, unnecessary complexity, accessibility risks, semantic leakage, inconsistent interaction patterns, copy ambiguity and divergence from the supplied authority. Separate concrete defects from subjective alternatives. Do not invent Product semantics or broaden scope. For each material finding, cite the source/screenshot/state that supports it, state the user consequence, and recommend the smallest bounded correction. Also identify anything that is strong and should be preserved. Return unresolved questions only when the supplied authority genuinely cannot decide them.

The review record must include exact source SHA, native build/receipt or screenshot identities, supplied authority refs, reviewer mode/containment, and whether any mutation occurred.

## Q-DESIGN-24 application

The preserved first Task A presentation/discoverability failure remains valid evidence.

The corrected Scene candidate and its Director Preview admission remain INCONCLUSIVE. The human Task A rerun is no longer the immediate iteration gate. First run the Design-Sol + Claude independent review loop against the admitted corrected Scene experience, apply bounded corrections, and rerun deterministic/native checks.

A later human observation is still required if the project wants to claim actual uncoached human comprehension/discoverability, Scene-code usefulness or Narrator intelligibility. That human checkpoint does not require the Director to diagnose why the result occurred.

Preview admission remains distinct from Design acceptance and PR #262 merge authority.
