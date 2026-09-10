# E0-A Q-E0A-03 Gemini 3.5 Flash-Lite Run 01 — Local Preflight Attempt 01

Status: **LOCAL PREFLIGHT FAIL — WRAPPER-ONLY UTF-8/NATIVE-OUTPUT STATUS CHECK DEFECT — PROVIDER INVOCATION NOT STARTED — AUTHORIZATION UNCONSUMED**

Date: **2026-09-09**

RunId: `E0A-Q03-G35L-20260909-01`

Authorization record: `docs/evidence/E0A_Q_E0A_03_G35L_RUN01_DIRECTOR_AUTHORIZATION_2026_09_09.md`

## Submitted Director-machine result

The guarded Windows PowerShell packet terminated before credential entry and before the provider-invocation boundary. The terminal markers were:

```text
=== Q-E0A-03 G35L RUN 01: GUARDED LOCAL PREFLIGHT ===
THIS_PACKET_DID_NOT_CONSUME_AUTHORIZATION=YES
PROVIDER_TRAFFIC_NOT_STARTED=YES
LOCAL_PREFLIGHT_STOP=Authorization is no longer in its exact unconsumed state.
Authorization is no longer in its exact unconsumed state.
```

No `LOCAL_PREFLIGHT=PASS`, credential prompt, `PROVIDER_INVOCATION_STARTED=YES`, `AUTHORIZATION=CONSUMED`, provider response, evidence-root terminal record, or spend record was reached.

## Consumption classification

**UNCONSUMED.**

The Director authorization explicitly states that a purely local preflight failure before the first provider-bound invocation does not consume the authorization. The submitted packet itself reached its pre-provider catch path and emitted both `THIS_PACKET_DID_NOT_CONSUME_AUTHORIZATION=YES` and `PROVIDER_TRAFFIC_NOT_STARTED=YES`.

Therefore this attempt does not authorize a different RunId/model/profile/fixture and does not consume the existing exact authorization. The same exact run may be presented again only after this local wrapper defect is corrected and all preflight gates pass.

## Recursive diagnosis

The failed assertion occurred inside `Assert-AuthorizationStillLive` after the packet had already successfully resolved and matched the exact authorization-record Git blob SHA:

`a36f190a552efa4b3e63ee2d845074d4053a44cb`

That blob contains the exact status:

```text
Status: **AUTHORIZED — UNCONSUMED — LOCAL PREFLIGHT PENDING**
```

A fresh repository read after the Director transcript independently confirmed that the same blob remains on `main` with the same authorized/unconsumed status and that `main` itself had not moved.

The only failing operation was a second, redundant semantic check that searched `git show` native-command text for the full Unicode status line including em dashes. Under Windows PowerShell 5.1, native-process output decoding can transform non-ASCII UTF-8 punctuation according to the active console/native encoding. The exact Git blob identity had already proved the file bytes were the audited authorization record, so making the subsequent authorization decision depend on a Unicode-rendered native-output string was an apparatus defect.

Classification: **PowerShell/native-output encoding fragility in the wrapper; no repository-authority contradiction and no Harness/source defect.**

## Correction law

The corrected packet must:

1. retain the exact authorization-record blob-SHA assertion;
2. avoid authorization-state dependence on non-ASCII punctuation emitted through native `git show` output;
3. use only ASCII semantic checks where a live-state text check is still useful;
4. preserve all existing pre-provider evidence-root/claim/worktree/fixture/native-host/freshness guards;
5. preserve the exact `PROVIDER_INVOCATION_STARTED=YES` consumption boundary;
6. make no source, test, fixture, executable, validation-tag, provider-profile, model, RunId, evidence-root, retry, fallback, or quota change.

## Authority consequence

- Q-E0A-03 exact RunId authorization remains **AUTHORIZED / UNCONSUMED**;
- provider traffic for this attempt: **NOT STARTED**;
- spend: **$0 inferred only from no provider invocation; no provider usage record exists because the boundary was not crossed**;
- promoted native executable/tag authority remains unchanged;
- next action remains a corrected guarded local preflight for the same exact authorized run.
