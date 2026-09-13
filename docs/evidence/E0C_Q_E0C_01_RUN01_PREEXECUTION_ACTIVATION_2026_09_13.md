# E0-C Q-E0C-01 - Slot 1 Preexecution Activation

Date: 2026-09-13

Status: **SLOT 1 ACTIVATION PREPARED - EXACT ONE LAUNCH AUTHORIZED ONLY AFTER INTEGRATION + EXACT-MAIN VALIDATION AND ONLY INSIDE FROZEN UTC WINDOW**

## Frozen identity

- condition: `E0C-IDENTICAL-REFERENCE-01`;
- RunId: `E0C-Q01-IDENT-20260912-01`;
- evidence root: `C:\Users\Wiryl\Sol Dev\Ensemble-Project-Evidence\E0C-Q01-IDENT-20260912-01`;
- executable: `bb869fb1c505603612bc718f739b3f1b358e5539`;
- variant/profile: `CREATIVE-MINIMAL` / `GEMINI-3.5-FLASH-LITE-MINIMAL`;
- Fixture SHA-256: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`;
- blind instrument SHA-256: `7857699de82904f52892f3e6d0f9794c80e50ee488c9e30ef9fdb13ba4edfd64`;
- one launch, one attempt per role invocation, zero retries, accepted-turn cap 12, timeout 300s, shadow ceiling USD 5.

## Frozen batch timing

Before any E0-C namespace claim or provider traffic, the Director-ratified batch order remains Slot 1 then Slot 2 and the immutable UTC batch window is frozen as:

- start: `2026-09-13T20:30:00Z`;
- end: `2026-09-13T23:30:00Z`.

The window may not be extended, shifted, reopened, or replaced based on transcript, artistic result, provider response, or terminal outcome. If an objective gate blocks a planned slot through window end, preserve it as unexecuted/noncontributing and stop for disposition.

## Activation gates

This activation retains the current predecessor chain through `docs/evidence/E0C_Q_E0C_01_RUN01_PREACTIVATION_2026_09_12.md`, `docs/evidence/E0C_Q_E0C_01_RUN01_FRESH_AUTHENTICATED_RATE_LIMIT_INTAKE_2026_09_13.md`, and `docs/evidence/E0C_Q_E0C_01_PUBLIC_FACT_AUDIT_2026_09_12.md`. Those records remain supporting evidence and do not independently create execution authority.

Fresh checks before this record found both canonical evidence roots absent and all four deterministic Slot 1/Slot 2 run/root claims absent. No matching Harness process was active. The preserved validator is clean at exact `bb869fb1c505603612bc718f739b3f1b358e5539`; its validation tag peels to the same commit. A fresh project-level Release `win-arm64` Harness build passed with `0` warnings and `0` errors and left the checkout clean.

Protected credential artifacts remain present at the established CurrentUser protected location outside Git, and ambient `GEMINI_API_KEY` is absent. Plaintext must not be printed, logged, persisted, committed, or passed in command arguments; it may exist only transiently for the intended child process at actual launch.

The standing association amendment `docs/evidence/E0C_Q_E0C_01_STANDING_PROJECT_KEY_ASSOCIATION_DIRECTOR_AMENDMENT_2026_09_13.md` closes the project/testing-key association gate from the previously authenticated baseline: `Gemini Project - Kymaean`, project ID `gen-lang-client-0490221700`, and `Gemini API Key - testing`, absent any contrary signal. No contrary signal is present.

Fresh authenticated rate-limit evidence remains current: Gemini 3.5 Flash Lite on Free tier at `9/15 RPM`, `22.07K/250K TPM`, `42/500 RPD`, leaving conservative minimum headroom of `6 RPM`, `227.93K TPM`, and `458 RPD`. No quota contradiction is present.

Standing project-relevance and test-key Director authorizations remain applicable. This activation creates no probe authority.

## Exact execution law

After this activation is merged and push-triggered exact-main Validation succeeds, Slot 1 may be launched exactly once inside the frozen UTC window, only after a final immediate check confirms current time is inside the window, the Slot 1 root/claims are still absent, exact validator/tag/Fixture remain intact, the protected artifacts remain present, no ambient key exists, and no material contrary provider/account/capacity signal has appeared.

The exact Harness invocation is `Ensemble.E0.Harness.exe e0a-run CREATIVE-MINIMAL GEMINI-3.5-FLASH-LITE-MINIMAL <fixture.json> E0C-Q01-IDENT-20260912-01 <evidence-root> bb869fb1c505603612bc718f739b3f1b358e5539` with the canonical Fixture path and evidence root. The credential is injected only through the existing protected child-process mechanism.

Any namespace claim consumes Slot 1 regardless of terminal result. No retry, replay, fallback, replacement RunId, alternate key, paid/Priority switch, model/profile substitution, source change, or second Slot 1 launch exists. Terminal evidence must be preserved, sealed, hard-gate audited, and ledgered before Slot 2 receives its own fresh activation. Slot 2 remains reserved but not automatically executable.

Provider traffic remains **ZERO** until all integration and final prelaunch conditions above pass.
