# E0-D P01 Slot 1 Preclaim Checkout-Guard Refusal

Date: 2026-09-15

Status: **ACTIVE PRECLAIM EXECUTION EVIDENCE ? CHECKOUT-GUARD REFUSAL NON-CONSUMING ? ONE CORRECTED PRECLAIM LAUNCH DIRECTOR-AUTHORIZED ? NAMESPACE UNCONSUMED ? PROVIDER TRAFFIC ZERO**

## Authority and launch boundary

Project `main@40409d0d438afe4631c07e41d38fa1179d358664` carried the integrated P01 timing amendment; push-triggered Validation #866 passed. The active P01 pair window was `2026-09-15T21:00:00Z..2026-09-16T06:00:00Z`.

Immediately before launch, authenticated Google AI Studio showed project `Ensemble Testing`, Free tier, exact `Gemini 3.5 Flash Lite`, and live row `0 / 15` RPM, `0 / 250K` TPM, `0 / 500` RPD. A machine census at `2026-09-15T23:08:33.7581342Z` confirmed exact Project main, validator/tag `0dacdbf6bd5453c192568cd4718207e145dfcf40`, executable SHA-256 `b2f9c45b376ef08b8c4ec0c7b56c4f595b1d7377ebdb858b63eb555e3cf28625`, clean validator, absent evidence root/claims, zero Harness processes, absent ambient `GEMINI_API_KEY`, and successful in-memory decryption of the new protected credential.

## Observed refusal

After Director continuation, the guarded launcher started the exact validated Harness with the frozen Slot 1 identity. The child exited `1` with:

`E0-A Git worktree does not match the live-run authority.`

The launcher had not set `ProcessStartInfo.WorkingDirectory`, so the child inherited a non-repository launcher working directory. `E0ARepositoryCheckoutGuard.Validate` therefore failed its initial `git rev-parse --show-toplevel` check.

## Consumption and provider boundary

The exact E0-D host source validates the Git checkout before fixture loading, provider HTTP-client creation, protected-provider binding, and `E0AFileEvidenceStore` construction. Namespace claims are first created inside `E0AFileEvidenceStore` construction.

Post-refusal census at `2026-09-15T23:16:47.4064909Z` confirmed:

- reserved evidence root absent;
- deterministic run claim absent;
- deterministic root claim absent;
- Harness process count `0`;
- ambient `GEMINI_API_KEY` absent.

Therefore this refusal produced no namespace/evidence-root creation and no Gemini provider request. The protected credential was inherited by the child process but the checkout guard rejected execution before provider construction.

## Local deterministic repair

The local guarded launcher was corrected only by adding the exact validator checkout as `ProcessStartInfo.WorkingDirectory`. Its CurrentUser `SecureString` decrypt path remains the verified new `gemini-ensemble-testing-key.dpapi` path; it does not reference the superseded credential. Parser/safety audit passes. The corrected launcher has **not** been executed.

## Director-disposition boundary

Current authority states both that actual namespace claim/evidence-root creation consumes Slot 1 and that exactly one live execution/no retry is authorized with terminal runtime results immutable. This preclaim checkout-guard refusal falls between those statements: no experiment namespace/provider operation occurred, but an exact Harness child did exit with a runtime guard error.

The Director has now resolved that ambiguity: this refusal was non-consuming because it occurred before deterministic claim/root/evidence creation and before provider construction. Exactly one corrected preclaim launch is authorized, but it must still repeat the immediately-preclaim authenticated project/key/model/tier/RPM/TPM/RPD observation and every frozen interlock before crossing the namespace boundary. P01 Slot 2 and later slots remain separately unauthorized.

## Retained predecessor authority

This event does not supersede the non-timing controls in these current predecessors:

- `docs/evidence/E0D_P01_SLOT1_LIVE_EXECUTION_DIRECTOR_AUTHORIZATION_2026_09_15.md`;
- `docs/evidence/E0D_P01_SLOT1_PROVIDER_ASSOCIATION_ROTATION_DIRECTOR_AMENDMENT_2026_09_15.md`;
- `docs/evidence/E0D_P01_SLOT1_PROTECTED_CREDENTIAL_REBIND_READINESS_2026_09_15.md`;
- `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_PREEXECUTION_ACTIVATION_2026_09_15.md`;
- `docs/evidence/E0D_Q_E0D_01_P01_SLOT1_ACTIVATION_INTEGRATION_CLOSEOUT_2026_09_15.md`.

## Director disposition — one corrected preclaim launch authorized

In the active Administrator conversation, after the checkout-guard refusal was reported as pre-namespace / pre-provider and the Director was asked to authorize one corrected P01 Slot 1 preclaim launch, the Director replied `continue`.

This disposition treats the checkout-guard refusal as **non-consuming** because no deterministic run claim, root claim, evidence root or provider operation occurred. It authorizes exactly one corrected launch of the same frozen `P01-FULL` identity after fresh authenticated capacity and all frozen interlocks pass again. It does not authorize a second post-claim attempt, replay, replacement, retune, model/profile/fixture substitution, paid/Priority route, P01 Slot 2 or any later slot.
