# ODR-26 — Cost Governance and Provider Admission Working Contract

Status: **WORKING CONTINUITY CONTRACT — STRUCTURE ADOPTED; NOT FROZEN; NOT ODR-26 RESOLUTION; NOT PROJECT LAW**

Updated: 2026-09-06

## 1. Purpose and authority boundary

This file preserves Director-reviewed working structure for ODR-26 so fresh chats do not reconstruct it from conversation history.

Blueprint 0.1 remains frozen and authoritative. In particular, deterministic user cost limits, call limits, cancellation, retry/failure enforcement, user spend authority, and eventual attributable cost/token visibility remain requirements. This working contract **cannot falsify those requirements**.

This file also does **not** alter, reopen, select, or provide evidence for the frozen E0-A provider/model configuration or any later E0-B provider pin.

## 2. What ODR-26 may falsify

ODR-26 may test:

- whether a casual user should manually allocate computational resources at all;
- whether exact token/depth quantities belong in ordinary creative UI;
- whether background-character economy should be user-managed or adaptive;
- where and how mandatory cost governance is represented without turning Stage into a billing dashboard;
- whether automatic adaptive allocation is non-inferior to manual control under matched spend.

ODR-26 does not own reasoning-intent legibility, provider-capability disclosure, casting, or understudy presentation; those remain ODR-09. Honest degradation when an admitted capability is unsupported or unavailable remains ODR-25.

## 3. Manual-allocation falsification structure

Null hypothesis:

> A casual user should not be required to manage tokens or computational depth if Ensemble can allocate resources automatically within the same user-authorized deterministic spend boundary without reducing session quality.

Required comparison structure:

1. **Adaptive** — Ensemble allocates resources automatically inside the fixed spend boundary.
2. **Abstracted manual** — the user controls a non-numeric cost/resource intention without provider-mechanics knowledge.
3. **Numeric manual** — the user directly controls computational quantities.

Comparisons claiming quality benefit must hold total spend constant or otherwise make spend differences explicit enough that the architectural variable is identifiable.

Decision hierarchy:

> **Automation first -> abstract resource intent only if automation is insufficient -> raw computational control only if it adds further value.**

### Eight falsifiers

Manual allocation is rejected or demoted if any of the following is observed:

1. no meaningful session-quality gain at matched spend;
2. the apparent advantage disappears once spend is matched;
3. users cannot allocate resources intelligently to moments that later prove consequential;
4. visible budgeting anxiety causes harmful under-allocation;
5. configuration time displaces creation without compensating quality benefit;
6. repeated adjustment/reversal shows the feedback loop is too opaque to use confidently;
7. benefits appear only for advanced users, falsifying casual/default placement even if an expert control remains useful;
8. adaptive allocation is non-inferior while requiring materially less user attention.

## 4. Numeric-threshold provenance and executability

No population threshold is frozen.

Previously discussed values (`>=80%`, `>=75%`, `>=90%`, `<10%`, and `0.4 SD`) were **unsourced heuristic strictness choices**, not Blueprint requirements, Ensemble evidence, validated usability standards, or approved project thresholds. They must never be cited later as sourced or frozen.

Solo-Director evaluation may reject obviously illegible or burdensome candidates now through scenario tests, semantic audits, ignorability checks, matched-spend transcript comparison, and observed adjustment/configuration burden.

Population comprehension percentages, standardized effect sizes, and population-level non-inferiority claims are deferred until an explicitly authorized casual-user UX-validation phase and participant pool exist. No such phase, sample size, or measurement protocol is currently authorized; this file does not invent one.

## 5. Provider admission precedes ODR-09 and ODR-25

Provider eligibility is evaluated before capability presentation or graceful degradation. A route that fails admission is not offered and therefore does not reach ODR-09 or ODR-25.

Admission is always by **criteria applied to a dated exact route snapshot**, never by a frozen named-provider allowlist.

The admission unit is the exact combination of provider/endpoint/tier/region/routing path/relevant account-data settings and, when applicable, downstream processors.

## 6. Gate P — Policy Admissibility

Working criterion:

> An exact provider route is eligible for Director consideration only if its governing terms permit Production-derived inputs and outputs to be processed for the requested service without mandatory use to train, fine-tune, or improve general-purpose/provider models, or for advertising, resale, or unrelated secondary commercial purposes.

Permitted operational processing may include automated or human classification/review reasonably necessary to provide the service, detect or prevent abuse/fraud/security incidents, enforce acceptable-use rules, diagnose reliability failures, or comply with law, provided that material is not thereby repurposed for general model training or unrelated product improvement.

### Gate P authority

Assistant/research work may collect sources, identify discrepancies, and prepare evidence. It is **not** authority to represent a third party's data handling to users.

Provider-route admission requires:

1. a qualified legal-review dependency/recommendation; and
2. an explicit Director admission decision.

The project has not yet named or authorized the legal-review process or phase. If terms are conflicting, ambiguous, incomplete, stale, or legally unreviewed, the route must not be represented as Gate-P-admitted.

### Gate P staleness

Future admission records should carry at minimum:

- `PolicyVerifiedOn`;
- exact policy/source documents;
- endpoint/tier/region;
- relevant data settings;
- routing path/downstream processors where applicable;
- legal-review identity/record;
- Director decision.

A **30-day maximum policy-snapshot age** was discussed only as an intentionally conservative operating proposal. It is **not sourced, approved, or frozen**.

Re-verification should be triggered when material terms/privacy/data-use policy changes; tier/endpoint/region/model-host/routing path changes; training/retention controls or defaults change; downstream processors change; corporate ownership/control materially changes; a provider announces a material data-policy change; credible evidence conflicts with the admitted snapshot; or a release would otherwise continue making a user-facing admission representation from stale evidence.

The durable law under consideration is fail-closed staleness, not a named provider list or permanently frozen cadence.

## 7. Gate C — Scene Capacity

Gate C is multi-axis. Passing request-rate limits while failing token throughput is a failure.

An exact route must be evaluated against all applicable binding constraints, including:

- RPM and burst/concurrency limits;
- RPD or equivalent request-volume limits;
- TPM;
- separate input/output token rates when enforced;
- daily token volume when enforced;
- per-request context/output ceilings;
- account/project/organization quota sharing.

Working pass rule:

> A route is Scene-capable only when the most restrictive applicable quota can sustain the defined ordinary-Scene workload, including input packets and outputs, without quota exhaustion or rate-limit pacing becoming part of the creative experience.

No product Gate-C threshold is frozen because ordinary Scene length, pacing, input-context size, output distribution, and final role composition have not been measured.

### Current E0-A arithmetic — screening evidence only

The approved E0-A Phase-B envelope currently uses three probabilistic invocations per accepted Turn and caps a run at 12 accepted Turns, with `4096` maximum output tokens per role invocation.

Therefore the present envelope permits:

- all three probabilistic roles on one route: `36` calls and up to `147,456` output tokens before input tokens;
- Performer-only use: `12` calls and up to `49,152` output tokens before input tokens.

These are **arithmetic ceilings from the E0-A experimental envelope**, not expected consumption, ordinary-Scene requirements, or a product Scene-ending definition. The 12-Turn E0-A stop is technical truncation, not Scene ending.

Published provider quota figures are volatile and may disagree across provider pages, account dashboards, tiers, models, regions, or secondary sources. A provider-wide headline limit is therefore insufficient evidence for Gate C; the exact admitted route requires a dated snapshot.

## 8. Named-provider non-law

Do not freeze an allowlist of providers or free tiers. Provider ownership, terms, routing, quotas, models, and data controls are volatile facts.

Free status neither qualifies nor disqualifies a route. A route may independently fail Gate P, Gate C, both, or neither.

## 9. Current recommendation awaiting Director freeze decision

The current recommendation is to freeze **structure only** when ODR-26 is ready for a formal decision:

- mandatory deterministic cost-governance carve-out;
- matched-spend three-arm falsification design;
- automation -> abstraction -> numeric-control hierarchy;
- eight falsifiers;
- solo-now / population-later distinction;
- ODR-09 / ODR-25 ownership boundaries;
- Gate P / Gate C provider-admission structure;
- criteria-based, dated route admission rather than named-provider lists.

Do **not** freeze population percentages, effect sizes, policy-review cadence, ordinary-Scene call/token thresholds, or a provider allowlist until the relevant test/review is executable and authorized.

## 10. Fresh-chat continuation

A fresh chat should read Blueprint 0.1 and current repository state first, then use this file only as the durable working record for ODR-26.

Do not treat this file as ODR-26 resolution, provider approval, legal advice, E0-A/E0-B provider evidence, or authorization to change product/engineering implementation.
