# Accessibility

Status: **RECEIVED / VALIDATED / SOURCE RELINQUISHMENT PENDING**

This is a received, frozen application-design contract, subordinate to the exact Website source until the Ryladmin cross-repository receipt audit authorizes relinquishment. It is not independently mutable Design authority. Source-era checkpoint, approval and next-action text records provenance; it does not reopen execution. Current Product/Engineering law controls implementation and validation. Browser/static evidence is not native-runtime evidence.


Source: [A:docs/KYMAEAN_APPUI_PHASE_1_STAGE_INTERFACE_ENVELOPE_AND_LAYOUT_MAP_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_PHASE_1_STAGE_INTERFACE_ENVELOPE_AND_LAYOUT_MAP_01.md#L255-L274); frozen accessibility section.

## 9. Adaptive layout contract

### Wide desktop

Preferred relationship:

```text
[quiet durable scope] [dominant primary task / Stage] [adjacent inspector when invoked]
```

Supporting transcript/history may occupy a subordinate lower or contextual region only when needed.

### Medium desktop

The primary task remains dominant. Navigation and inspectors may collapse, overlay, or become temporarily substituted surfaces while preserving the same semantic relationships.

### Narrow desktop

One primary surface is shown at a time. Inspectors and deep detail become sequential/overlay surfaces with predictable Back/close behavior. No essential meaning may depend on simultaneous columns.

Source: [A:docs/KYMAEAN_APPUI_PHASE_1_STAGE_INTERFACE_ENVELOPE_AND_LAYOUT_MAP_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_PHASE_1_STAGE_INTERFACE_ENVELOPE_AND_LAYOUT_MAP_01.md#L293-L307); frozen accessibility section.

## 11. Accessibility and state grammar

Every Phase-1 composition must preserve:

- keyboard access to durable scope, primary work surface, inspector and commands;
- meaningful focus return when transient surfaces close;
- selection distinct from keyboard focus;
- Character identity independent of portrait recognition or color alone;
- current/historical and provisional/effective distinctions without color alone;
- Presentation Perspective exposed semantically when active;
- causal/history content in a meaningful reading order;
- reduced-motion compatibility;
- high-contrast survivability;
- scalable text without requiring precise spatial interpretation.

Source: [A:docs/KYMAEAN_APPUI_COMPONENT_SYSTEM_FOUNDATION_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_COMPONENT_SYSTEM_FOUNDATION_01.md#L82-L97); frozen accessibility section.

## 9. Responsive and accessibility contract

The component foundation must survive the same semantic system across wide, medium and narrow layouts. Narrow layouts may stack actions, move subordinate row actions, sequence transient/detail surfaces and increase vertical depth; they may not require horizontal escape to preserve meaning.

Required design behavior:

- visible keyboard focus remains a solid outer ring independent from selection;
- icon-only controls retain accessible names;
- conventional icon carriers remain at least 32px in the deterministic reference;
- editable text controls retain usable control height while text may scroll internally when its value exceeds the field;
- forced colors remain comprehensible without authored hue;
- 200% text and text-spacing stress may increase vertical geometry but must not create page-level horizontal overflow;
- control meaning must not depend on hover, portrait recognition, motion, glow or theme.

The frozen APPUI-COMP-01 carrier passes 1600, 1024, 390 and 320 widths plus forced-colors 320, 200% text 320 and text-spacing stress 320 with zero layout overflow.

Source: [A:docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md#L100-L104); frozen accessibility section.

## 7. Theme parity law

Light and Dark communicate identical Product meaning, control availability, information hierarchy, disclosure, lifecycle position, and return behavior.

Darkness never means historical, private, unavailable, failed, provisional, disabled, technical, more dramatic, or more authoritative. Lightness never means current, true, healthy, effective, complete, or creator-global.

Source: [A:docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md#L105-L124); frozen accessibility section.

## 8. Semantic distinctions that remain non-color-dependent

The visual system must preserve, when relevant:

- keyboard focus != selection;
- selection != Director attention/opportunity;
- current != historical;
- provisional/non-effective != effective/accepted;
- creator action != Character agency;
- Character identity != Performer assignment;
- Character-bounded disclosure != creator-global truth;
- unknown != false;
- private/withheld != absent;
- infrastructure state != fiction;
- Character silence/refusal != provider/system failure;
- Back/Close != Undo;
- persistent identity != portrait or theme.

Labels, structure, borders, disclosure, ordering and semantic origin carry these meanings before color does.

Source: [A:docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/KYMAEAN_APPUI_PHASE_3_STATIC_APP_VISUAL_SYSTEM_01.md#L149-L162); frozen accessibility section.

## 10. Responsive and accessibility invariants

Representative design evidence has already exercised wide desktop, medium desktop, `390px`, `320px`, forced-colors, `200%` text and text-spacing stress. Successor work must preserve those same design invariants.

- No essential meaning may require simultaneous columns.
- Narrow layouts may stack or sequence content while preserving semantic origin and focus restoration.
- Normal text must retain at least the applicable `4.5:1` design contrast floor; large text and meaningful non-text boundaries retain at least the applicable `3:1` floor.
- Forced-colors comprehension must survive without authored hue.
- Focus and selection must remain independently recoverable.
- Required meaning must not depend on portrait recognition, hover, animation, continuous motion, glow or color alone.
- Large-text and text-spacing repairs may change responsive geometry, but may not retune selected color roles or semantic meaning.

These are design-evidence constraints, not claims of native Windows accessibility validation.

Source: [A:docs/evidence/APPUI_ALPHA_STAGE_ENSEMBLE_RESPONSIVE_COMPOSITION_LAW_01.json](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/evidence/APPUI_ALPHA_STAGE_ENSEMBLE_RESPONSIVE_COMPOSITION_LAW_01.json#L1-L135); JSON objects /ensemble_thesis, /tested_density, /placement_rules, /density_rules, /responsive_rules, /compression_order, /prohibited, /open_variables, /non_authority.

## Kymaean Alpha Stage Ensemble Layout & Responsive Composition Law 01

```json
{
  "ensemble_thesis": "One shared social Stage field contains multiple independent Character anchors. Composition is relational, not slot/grid based; negative space is meaningful; spatial position never silently creates Product meaning.",
  "tested_density": {
    "range": "2–5 present Characters",
    "status": "design-stress evidence only",
    "not_authority": [
      "no Product minimum",
      "no Product maximum",
      "no Scene membership law",
      "no runtime addition/removal behavior",
      "no automatic solution for counts beyond 5"
    ],
    "beyond_range_rule": "If future Product truth exceeds current density evidence and the shared-field acceptance tests fail, open a new bounded Design study rather than silently introduce grid, carousel, pagination, hidden roster, or horizontal-scroll behavior."
  },
  "placement_rules": [
    "center position does not imply protagonist/speaker/selection/importance",
    "left-right position does not encode turn order",
    "near-far position does not encode relationship type/strength",
    "scale/height differences cannot establish rank/authority",
    "focus/selection/performance/opportunity/relationship salience remain independent from placement",
    "no visible grid/cell scaffolding around anchors"
  ],
  "density_rules": {
    "sparse_2": [
      "preserve generous negative space",
      "do not inflate anchors into portrait monuments/cards",
      "do not add metadata merely because space exists",
      "do not force bilateral symmetry"
    ],
    "open_3": [
      "preserve an open social field",
      "avoid three equal columns",
      "keep all state meanings independent"
    ],
    "moderate_4": [
      "reduce spacing without losing anchor anatomy",
      "never fall back to 2x2 dashboard",
      "selection remains local"
    ],
    "dense_5": [
      "compress decoration before identity/state",
      "listeners remain visibly present",
      "selection does not consume neighboring anchors",
      "no horizontal scroll required by current tested evidence"
    ]
  },
  "responsive_rules": {
    "wide": "Stage remains largest contiguous live surface; full ensemble stays visible where tested density permits; invoked inspector/causal context may be adjacent and subordinate.",
    "medium": "Stage remains primary; anchors recompose within same semantic field; inspector/context overlays, collapses, or substitutes before Character identity/state degrades.",
    "narrow": "Stage/current performance remains primary; Character anchors recompose for legibility; contextual/deep surfaces become sequential; Back/Close restores exact Stage origin/focus when valid.",
    "continuity": [
      "identity follows Character, not coordinates",
      "selection follows semantic target, not pixel position",
      "focus restoration follows semantic origin",
      "theme/viewport never changes feature availability",
      "responsive adaptation cannot mutate Production truth"
    ]
  },
  "compression_order": [
    "reduce decorative spacing/atmosphere first",
    "reduce simultaneous contextual adjacency second",
    "move contextual/deep surfaces to overlay/substitute/sequential presentation third",
    "never remove names/state distinctions or merge focus/selection before secondary UI yields",
    "never invent carousel/paging/hidden roster/horizontal-scroll as a shortcut"
  ],
  "prohibited": [
    "equal-card grid default",
    "2x2 or column dashboard fallback",
    "automatic carousel/pagination/hidden roster",
    "semantic left-to-right turn order",
    "center-equals-protagonist law",
    "scale-equals-importance law",
    "selection-triggered reflow",
    "theme-specific Stage arrangement",
    "wide-only state meaning",
    "narrow-only feature removal"
  ],
  "open_variables": [
    "exact Character coordinates/spacing",
    "exact Stage camera/perspective",
    "exact arc/curve/alignment geometry",
    "exact cone scaling by viewport",
    "exact narrow Character arrangement",
    "exact width breakpoints",
    "counts beyond tested 2–5",
    "entry/exit/reflow motion",
    "Scene membership/cast-cardinality behavior",
    "performance/opportunity/relationship algorithms",
    "native layout/GPU/hit-testing/accessibility metrics"
  ],
  "non_authority": [
    "No new Product/runtime semantics",
    "No cast cardinality",
    "No Scene membership behavior",
    "No scrolling/paging/carousel behavior",
    "No final geometry/breakpoints",
    "No motion/reflow authority",
    "No Engineering mutation"
  ]
}
```

Source: [A:docs/evidence/APPUI_ALPHA_STAGE_STATE_WITNESS_REDUNDANCY_GRAMMAR_01.json](https://github.com/Rylascoo/Ensemble-Website/blob/33b8e13c4e71c8ceee1fdc3a646f291c9422a047/docs/evidence/APPUI_ALPHA_STAGE_STATE_WITNESS_REDUNDANCY_GRAMMAR_01.json#L1-L102); JSON objects /redundancy_principle, /witness_layers, /collision_rules, /stress_cases, /prohibited, /open_variables, /non_authority.

## Kymaean Alpha Stage State Witness & Redundancy Grammar 01

```json
{
  "redundancy_principle": "Every essential state requires a structural or textual witness that survives loss of color and motion. Decorative variation may reinforce, never replace, semantic witness.",
  "witness_layers": {
    "I": "Identity — human-readable Character name + stable programmatic identity; no color/portrait/position dependence.",
    "P": "Presence/performance — listening, speaking/performing, performed silence, nonverbal action, refusal remain explicit fictional meanings.",
    "O": "Opportunity — explicit independent non-color witness only when Product truth exposes current opportunity.",
    "C": "Creator interaction — keyboard focus and creator selection remain independent; focus uses solid outer ring reference, selection uses persistent structural edge witness at floor/name hinge.",
    "R": "Relationship salience — quiet composition-level grouping, never type/score/obligation/permanent graph.",
    "T": "Temporal/effect — current solid+label, historical dashed+label, provisional/non-effective dotted+label for supporting information.",
    "A": "Application/infrastructure — explicit application-side status outside Character fictional carriers."
  },
  "collision_rules": [
    "identity must remain readable before all other emphasis",
    "performance state remains explicit independently of opportunity/selection",
    "opportunity stays independent from focus/selection/performance/relationship salience",
    "focus and selection may coexist and remain separately perceivable",
    "relationship salience must not obscure individual Character state",
    "temporal/effect status applies to information surfaces, not automatically to Character performance",
    "infrastructure must not mutate fictional Character presentation"
  ],
  "stress_cases": [
    "selected+listening Character while another speaks",
    "selected Character while another has current opportunity",
    "focus inside inspector while Stage selection persists",
    "speaking Character in salient relationship while neither member is selected",
    "performed silence with different selected and opportunity Characters",
    "historical causal context open while Stage performance remains current",
    "technical non-response outside Stage with fictional states unchanged",
    "focus+selection on same Character without creating opportunity/performance"
  ],
  "prohibited": [
    "color-only identity/state",
    "motion-only state",
    "portrait-only identity",
    "full-cone selected card",
    "one generic glow for focus+selection+opportunity+speaking",
    "dimmed listener as absence",
    "silence as blank output",
    "refusal as system error",
    "relationship score/graph inferred from proximity",
    "technical state painted into Character performance",
    "new pip/icon vocabulary without separately justified semantic need"
  ],
  "open_variables": [
    "final Character colors/textures",
    "exact cone/floor modulation for performance/opportunity",
    "final Stage state labels/copy",
    "redundant Character identity symbol",
    "relationship-salience geometry",
    "native focus/selection metrics beyond static reference intent",
    "motion/timing/cadence/transitions",
    "state/opportunity/relationship algorithms",
    "final performance-modality iconography",
    "screen-reader/native accessibility implementation",
    "forced-colors/high-contrast native validation"
  ],
  "non_authority": [
    "No new Product/runtime semantics",
    "No opportunity algorithm",
    "No relationship algorithm",
    "No motion/timing authority",
    "No final icon/symbol vocabulary",
    "No native accessibility validation",
    "No Engineering mutation"
  ]
}
```

Full source identities, scoped dependencies, dispositions and destination hashes: `docs/design/app/TRANSFER_RECEIPT.json`.
