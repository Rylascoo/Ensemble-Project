# E0-A Gemini `countTokens` Input Projection — Native Windows ARM64 Validation

Status: **NATIVE MACHINE VALIDATION PASS — EXACT CHECKOUT `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2` — ANNOTATED VALIDATION TAG VERIFIED — PROMOTED NATIVE CHECKPOINT — PROVIDER AUTHORIZATION NONE**

Recorded: **2026-09-09**

## Authority boundary

This evidence records Director-host native Windows ARM64 validation of the frozen Q-E0A-04 correction. It does not authorize Gemini/OpenAI credentials, provider traffic, `countTokens`, generation, inference, spend, fallback, retry, or any E0 reference run.

The machine-tested checkout is exactly:

`3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`

Branch at validation time:

`e0a-gemini-counttokens-input-projection-correction`

Frozen contract:

`docs/blueprint/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_CORRECTION_AMENDMENT.md`

Implementation audit:

`docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_IMPLEMENTATION_AUDIT_2026_09_09.md`

Attempt-01 failure/diagnosis evidence:

`docs/evidence/E0A_GEMINI_COUNTTOKENS_INPUT_PROJECTION_NATIVE_ARM64_VALIDATION_ATTEMPT_01_2026_09_09.md`

## Attempt 02 composition

Attempt 02 used a new linked detached worktree:

`C:\Users\Wiryl\Sol Dev\Ensemble-Project-Worktrees\e0a-counttokens-input-projection-native-arm64-attempt02`

The failed Attempt-01 worktree remained preserved at exact checkout `de38d5d52279c22a1786e11200239c445e04377b`. The historical validated root remained preserved at `689655eed677b789ab3ee395f1c65b4f2cb72cc8`.

Attempt 02 occurred in two contiguous validation segments because the first packet's assertion helper used a Windows PowerShell 5.1-incompatible two-argument `String.Contains` overload after the first credentialless 3.5 route had already returned the expected refusal. That helper defect was validation-apparatus-only; it did not modify source, tests, fixtures, checkout identity, provider state, or evidence state.

The continuation first proved the crashed helper left no `ensemble-e0a-a02-*` evidence root, re-established candidate/worktree/historical-root identities, and then repeated every credentialless boundary with a PowerShell-5.1-safe ordinal `IndexOf` assertion before completing all closeout checks.

No test/build/smoke result from Attempt 01 was reused.

## Native host identity

Director-host transcript established:

- `PROCESSOR_ARCHITECTURE=ARM64`;
- Windows host;
- .NET SDK `9.0.317`;
- runtime RID `win-arm64`;
- .NET host architecture `arm64`;
- Microsoft.NETCore.App `9.0.19` available.

Result: **native Windows ARM64 host identity PASS**.

## Complete native test/build results

At exact checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`:

- `Ensemble.E0.Core.Tests`: **622/622 PASS**;
- `Ensemble.E0.Harness.Tests`: **131/131 PASS**;
- fresh Debug Harness build for `net9.0/win-arm64`: **PASS**;
- Missing Raft fixture smoke: **PASS** (`ensemble.e0.missing-raft@0.1.0`);
- generic E0 smoke: **PASS** (`ensemble.e0.smoke@0.1.0`).

The 131/131 Harness result confirms the Attempt-01 stale comparison-oracle correction. No production-source correction was made after the original Q-E0A-04 implementation.

## Credentialless provider-edge validation

Process `GEMINI_API_KEY` and `OPENAI_API_KEY` were absent.

The continuation established all required boundaries using the freshly built candidate:

| Route | Expected boundary | Result |
|---|---|---|
| `GEMINI-3.5-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL` | exact `GEMINI_API_KEY is required at the E0-A provider edge.` refusal, native exit 1 | **PASS** |
| `GEMINI-3.1-FLASH-LITE-MINIMAL` / `CREATIVE-MINIMAL` | same exact missing-key refusal, native exit 1 | **PASS** |
| `GEMINI-2.5-FLASH-LITE-NONE` / `CREATIVE-NONE` | same exact missing-key refusal, native exit 1 | **PASS** |
| retired `GEMINI-2.5-FLASH-NONE` / `CREATIVE-NONE` | exact `E0-A Gemini provider profile is not approved for live selection.` refusal, native exit 1 | **PASS** |

For every probe:

- expected message: PASS;
- expected native exit: PASS;
- evidence root: **ABSENT**.

Provider network: **NOT PERFORMED**.

Spend: **$0**.

## Integrity closeout

Continuation closeout established:

- `POST_VALIDATION_HEAD=3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`;
- remote correction branch resolved to the exact candidate during native closeout;
- Attempt-02 worktree remained detached and clean;
- no material untracked content appeared under `src`, `tests`, or `fixtures`;
- Attempt-01 preserved head remained `de38d5d52279c22a1786e11200239c445e04377b`;
- historical root remained `689655eed677b789ab3ee395f1c65b4f2cb72cc8` with unchanged status;
- provider keys absent at closeout;
- all probe evidence roots absent.

Result: **integrity/preservation closeout PASS**.

## Annotated validation tag

The required durable annotated validation tag was created and pushed after the native transcript completed:

- tag: `validation/e0a-gemini-counttokens-input-projection-native-arm64`;
- tag object: `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`;
- object type: annotated `tag`;
- dereferenced target: exact commit `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`.

GitHub was independently queried after the push. The remote tag ref resolves to tag object `b9341c1c0ab48be5370ed22e7d2511f9e3be00d9`, and that tag object identifies exact commit `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2` as its target.

Result: **annotated tag identity/dereference PASS**.

## Hosted documentation closeout

Follow-on documentation-only checkpoint `0863c5a21c29165160b48c3ace31c01dc0a5b369` reconciled the native-success authority graph and removed the obsolete active validation handoff. Hosted Validation gate run `34405315708` completed successfully across repository law enforcement, document authority census, oracle assertion coverage, required x64 Core regression, and ARM64 cross-compile.

That documentation-only checkpoint does **not** inherit native runtime authority. Native runtime authority remains the exact tagged checkout `3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`.

## Validation conclusion

Q-E0A-04 is **MACHINE-VALIDATED on native Windows ARM64 and durably tagged** at exact checkout:

`3a010df5d26fc58d6f3820f2dc2cfbb0d015a9d2`

The complete result is:

- Core **622/622 PASS**;
- Harness **131/131 PASS**;
- fresh native Harness build PASS;
- Missing Raft smoke PASS;
- generic smoke PASS;
- all current credentialless Gemini route gates PASS;
- retired route pre-credential rejection PASS;
- provider keys absent;
- provider network NOT PERFORMED;
- spend $0;
- candidate/worktree/preservation checks PASS;
- annotated validation tag created, pushed, and independently verified.

The PowerShell helper interruption does not weaken this result because the continuation independently proved the first credentialless probe left no evidence root, repeated every credentialless route gate with the corrected validator, and completed all remaining closeout assertions against the unchanged exact candidate.

## Promotion boundary

This exact checkout is the current promoted native machine-tested checkpoint in `docs/VALIDATION_LEDGER.md` and `CURRENT_STATE.md`.

Later documentation-only closeout commits do not inherit native runtime authority. Q-E0A-04 is DONE. Q-E0A-03 remains blocked by its own provider-route and authorization prerequisites. Provider authorization remains **NONE**.
