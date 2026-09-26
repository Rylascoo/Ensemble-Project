# Creator-facing first Performance

Status: **DIRECTOR-ADOPTED PRODUCT / DESIGN CONTRACT — IMPLEMENTATION NOT AUTHORIZED**

Date: 2026-09-25 (Director local date).
Exact adoption baseline: `bbf364b09aa08a441b20b781f09fd4dae0533e41`.

## Adoption and authority

The Director adopted the reviewed creator-facing first Performance proposal, with exactly two presentation amendments: the primary action is **Request & record Performance**, and the commitment explanation is the exact text below. These amendments clarify the existing atomic commitment; they change no Q-PROD-09 Product semantics and introduce no second confirmation or acceptance stage. All other proposed interaction, failure, focus, scope and execution boundaries are preserved.

Product authority: `docs/FIRST_PRODUCT_NATIVE_PERFORMANCE_Q1_Q2_Q5_DIRECTOR_DISPOSITION_2026_09_24.md`; integration: `docs/evidence/Q_PROD_09_FIRST_PRODUCT_NATIVE_PERFORMANCE_INTEGRATION_CLOSEOUT_2026_09_24.md`. Existing presentation law: `docs/design/app/contracts/SCENE_INITIAL_ROSTER_PRESENTATION.md`, `docs/design/app/contracts/CURRENT_PRODUCTION_WORKSPACE_COMPOSITION.md` and `docs/design/app/contracts/ACCESSIBILITY.md`.

This contract exposes one explicitly requested Performance and its committed consequence while preserving Q-PROD-09's limits. It is adopted interaction law, not implemented behavior, empirical Design acceptance or provider admission.

## Creator interaction

Entry: **Current Production -> Scenes -> Inspect initial roster -> Performance… beside a Character**.

**Performance…** opens a request surface; it does not invoke anything. The roster remains read-only, and no Character is selected automatically.

The surface identifies the Production, Scene code, and exact Character name with its existing conditional Character code. Internally, the target remains the explicit `SceneId + CharacterId` pair. Names, codes, row positions and keyboard focus never substitute for identity.

This is a contextual command/result surface. It leaves ProductSpace unchanged and does not establish a Current Scene, live Stage, transcript or history browser. Adoption amends only the narrow Performance exclusion in the Scene contract and corresponding gated wording in the current Design index.

Before invocation, display:

> **Performance**
>
> If successful, this Performance and one new circumstance for this Character will be recorded together. There is no separate approval step.
>
> **Request & record Performance**

Back leaves without invoking anything. There are no prompt editors, actor-context controls, Performer assignments or suggested next actors.

## Unchanged Product truth

The adopted Q-PROD-09 disposition and `src/Kymaean.Application/ProductApplication.cs` at the exact baseline establish:

- The Scene exists, the Character belongs to Production Cast, and that Character belongs to the Scene's initial roster.
- Actor context contains only SceneId, CharacterId, CharacterName and that Character's committed Circumstances. Those Circumstances are Character-bound across Scenes, not restricted to the selected Scene.
- Performer and consequence interpreter are separate invocation-scoped inputs.
- Successful acceptance records visible Performance text and exactly one additive Character Circumstance together.
- There is no separate creator accept/reject step in this operation.

An empty initial roster remains valid. It simply offers no Character-specific Performance action.

## State and presentation

| State or transition | Creator-visible presentation | Required behavior |
|---|---|---|
| Executor unavailable | **Performance is unavailable.** | Request disabled. No invented setup command, provider diagnosis or fictional explanation. |
| Ready | Identity and commitment explanation above; **Request & record Performance** | Require a confirmed target and an admitted executor. No invocation from an unresolved Scene/Character witness. |
| Request submitted | **Requesting Performance…** | Capture target and identity witnesses; invoke once. Block duplicate activation, all other Product operations and all in-app navigation until outcome classification. |
| Confirmed success | **Performance recorded.** Then **Performance** and **Added circumstance for <Character>** | Show exact accepted text and exact consequence from the successful returned result. Present both as one recorded outcome. |
| Typed `Incompatible` | **Performance unavailable in this version.** | Restore Back. No automatic retry, migration or upgrade action. |
| Typed `Invalid` | **Performance not confirmed. Kymaean could not confirm whether this Performance was recorded.** | Preserve the requested Scene/Character witness and enter uncertainty handling. |
| Invocation I/O/access failure | **Confirmation unavailable. Kymaean could not confirm whether this Performance was recorded.** | Same conservative uncertainty handling, while retaining the distinct technical classification. |
| Unexpected technical fault | Technical failure, with outcome unconfirmed unless the commit boundary is known | Preserve diagnostics and fail closed. Do not relabel it as Product Invalid, corruption, Character refusal or silence. |

Pending is application activity. It does not mean the Character is thinking, hesitating, speaking or acting. No streaming, percentage progress, cancellation, retry or timeout guarantee is implied.

All in-app navigation includes Back, Home, Productions, Settings, ProductSpace changes and contextual commands. Window termination or an OS interruption cannot be represented as cancellation or rollback.

A rendering failure **after a successful Application result** must retain the known recorded outcome; it must not become a failed request with a retry affordance.

## Performance and consequence presentation

Preserve exact returned content without rewriting, trimming, summarizing or adding quotation marks as content. Both fields remain fully reachable through wrapping and vertical scrolling.

An accepted Performance may contain an empty string. In that case, show **No visible Performance text.** as explanatory interface text outside the content field. The committed circumstance still appears. Empty text is not evidence of missing output, provider failure or an unrecorded request.

The consequence is labeled as an added circumstance for the identified Character. It does not become World-current truth, another Character's state, or a claim of Knowledge or Belief.

There is no edit, reject, undo, “another Take,” automatic continuation or next-actor action.

Before leaving a successful result, display:

> Leaving closes this result view. The Performance and circumstance remain recorded.

Back closes this request's transient result view. This package supplies no subsequent history inspection or “latest Performance” query. That limitation is part of the adoption, not an implied completed creator workflow.

## Uncertainty and reopening

The current result type has no request identity or explicit commit-status field. `Invalid` can occur after the committer has returned success; an I/O failure can also occur after durable writing has begun. Therefore, **“nothing was saved” is not a permissible generic failure message**.

- Retain only the requested Scene/Character identity witness. Do not display guessed Performance text, a guessed consequence or a success row.
- Block further Performance requests for that Production throughout the session, including leaving and returning.
- Explain: **Open the Production again to reload its recorded state. Reopening cannot confirm the outcome of this request.**
- Clear the stale-state block only after successful ordinary Open. Failed Open does not clear it. Open is not promised to repair persistence.
- Never infer attribution from matching text, collection length or roster membership.
- A later request is a **new request**, potentially recording another Performance. It is not a deduplicated retry.

After successful Open clears the block, keep the earlier request's unconfirmed outcome visible and warn before another request that it may record another Performance. Clearing the block does not clear that distinction.

Request witnesses and uncertainty tracking are session-only. No cross-restart request tracking or exactly-once invocation guarantee is created.

## Accessibility and focus

Entry focuses quiet Back and announces the Performance heading, Scene identity and Character identity. Opening the surface never invokes the request.

While pending, focus moves to an accessible status target rather than remaining on a disabled button. Completion focuses the distinct outcome heading and announces the transition once; it does not automatically narrate long Performance text.

Back restores the exact originating Character action within the identified Scene. If that target no longer exists, focus the roster heading rather than another row. Existing Scene-list and Current Production return behavior remains intact.

Preserve keyboard operation, focus/selection separation, Light/Dark parity, MAT F1, wrapping, vertical access at 720x520, large text and reduced-motion compatibility. Status cannot depend on color or motion. Narrator intelligibility and High Contrast remain future native evidence requirements.

## Smallest later implementation boundary

| Surface | Likely change if separately authorized |
|---|---|
| `src/Kymaean.Windows/Presentation/MainPageViewModel.cs` and `src/Kymaean.Windows/Presentation/MainPageViewModel.Scenes.cs` | A bounded Performance state/command partial, immutable request/result projections, session uncertainty and invocation guards. |
| `src/Kymaean.Windows/MainPage.xaml` and `src/Kymaean.Windows/MainPage.Scenes.cs` | Entry action, request/result content, focus and announcements. |
| `src/Kymaean.Windows/App.xaml.cs` | Supply a separately admitted executor through a bounded invocation adapter. |
| Presentation and Application boundary tests | Delayed completion, navigation blocking, duplicate/reentrant activation, identity binding, every outcome, empty text and uncertainty persistence. |

Reuse `ProductApplication.Perform`, its existing ports and its existing persistence event. No new history API, storage schema, E0 dependency or general orchestration framework is justified.

The operation is synchronous and the Application object is mutable and unsynchronized. Responsive pending presentation therefore requires verified serialized ownership and result publication. Moving the call into `Task.Run` alone would not establish those guarantees.

## Execution prerequisite and falsifiers

At the exact adoption baseline there is **no production implementation of either `IProductPerformer` or `IProductConsequenceInterpreter`**. Contract adoption cannot enable ordinary-app invocation. Deterministic doubles belong in tests or a separately authorized, clearly identified isolated Preview exercise, not as pretend production behavior.

Return for the smallest further Product decision if:

- usable interaction requires a Current Scene, broader actor context, final Take review or history browsing;
- users reasonably interpret **Request & record Performance** as preview-only despite its commitment explanation;
- Scene/Character codes cannot support reliable accessible targeting;
- pending work blocks the UI or loses serialized ownership/result attribution;
- uncertainty permits accidental duplicate invocation or false success/noncommit claims.

## Review and explicit non-authority

Proposal review against the exact baseline found a pending-navigation gap; the reviewed revision closes it, and targeted re-review returned no remaining finding. This was static independent behavioral no-write review under workspace-write permissions, not technically contained read-only execution. No new tests, builds or native accessibility validation were performed for the proposal. The Director subsequently adopted that revision with only the action/explanation copy amendment recorded above.

This contract creates no implementation, queue activation, merge, provider/model selection, credentials, traffic or spend authority. It adds no persistent casting, automatic opportunity selection, Scene switching/endings/lifecycle, broader epistemic or consequence semantics, recent Performance context, zero-consequence acceptance, final Take policy, history browser, ODR-13/19/30 resolution, deferred-E0 execution, architecture reopening or release advancement.

Q-DESIGN-25 remains complete. Implementation and executor admission require separate Director disposition. Native/Design acceptance and validation rungs remain bound to their previously recorded exact executables.
