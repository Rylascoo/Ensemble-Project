# Ensemble Current State

Updated: 2026-09-07

## Authority

`Rylascoo/Ensemble-Project` is engineering authority. Frozen Blueprint 0.1 plus approved phase/patch specifications govern architecture. `CURRENT_STATE.md` alone carries phase/checkpoint/validation/next-action authority. Product/policy authority: `docs/PROJECT_AUTHORITY.md`.

## Current checkpoint

Runtime: **E0-A Experimental Harness — Phase B.** E-R1 repository/tooling/workflow restructuring is **COMPLETE / PROMOTED / ARCHIVED** and must not be reopened.

Promoted `main` continuity checkpoint remains `7490de24bfd2a9829f6afc1ae4b3831c98c50837`.

Active engineering work branch:

```text
e0a-gemini-rate-discipline-model-comparison
```

Approved architecture is now the composition of:

- `docs/blueprint/E0A_PHASE_B_REFERENCE_RUN_ENVELOPE.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_NORMATIVE_REFERENCE_AMENDMENT.md`;
- `docs/blueprint/E0A_PHASE_B_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_AMENDMENT.md` at architecture commit `76fc0c64da4724a7a352a656a062d5c3ed431ad6`.

Audited executable/test implementation checkpoint: `277dcb5e3bdd99ef02df4bd91d3df309d4c707ef`. Implementation evidence: `docs/evidence/E0A_GEMINI_RATE_DISCIPLINE_MODEL_COMPARISON_IMPLEMENTATION_AUDIT.md`.

The implementation adds explicit model selection, conservative Gemini RPM/TPM discipline, and latency/resource evidence without changing Core or fixtures. Compare from `main` to `277dcb5e...` is 16 ahead / 0 behind with no `src/Ensemble.E0.Core`, Core-test, or fixture delta.

## Validation state

Promoted native machine-tested authority remains:

```text
cc395a25162a0a682796bffb44060c799df0db32
validation/e0a-pre-restructure-closure-native-arm64
```

At that historical checkpoint: Core 622/622, Harness 103/103, fresh native Windows ARM64 build/smokes, and credentialless Gemini refusal PASS; no provider network/inference/spend.

New comparison implementation checkpoint `277dcb5e...` passed cloud Validation run `34177033966`:

- ARM64 cross-compile of Core, Harness, Core tests, Harness tests: PASS;
- x64 Core regression: PASS;
- repository laws: PASS;
- oracle assertion coverage: PASS;
- document authority census: PASS.

This is **compiler/cloud-static evidence only**. The cloud workflow does not execute Harness tests and is not native Windows ARM64 validation. Native Windows ARM64 validation of the comparison implementation is **PENDING**. Do not project `cc395a...` native authority onto the new Harness source.

## Provider boundary

Google Gemini API remains the sole E0-A provider method. Real provider network execution remains **NOT AUTHORIZED**.

The Director approved this comparison matrix for implementation and later separately authorized experiments:

```text
GEMINI-2.5-FLASH-LITE-NONE
  arm: CREATIVE-NONE
  model: gemini-2.5-flash-lite
  verified Free-tier snapshot: 10 RPM / 250,000 input TPM
  true thinking-off for Performer/Interpreter

GEMINI-3.5-FLASH-LITE-MINIMAL
  arm: CREATIVE-MINIMAL
  model: gemini-3.5-flash-lite
  verified Free-tier snapshot: 15 RPM / 250,000 input TPM
  minimal thinking for Performer/Interpreter; not represented as zero thinking

GEMINI-2.5-FLASH-NONE
  arm: CREATIVE-NONE
  model: gemini-2.5-flash
  verified Free-tier snapshot: 5 RPM / 250,000 input TPM
  true thinking-off for Performer/Interpreter
```

RPD was not established by the supplied AI Studio account evidence and remains `unverified-pre-live`. The Free-tier route remains synthetic-fixture-only.

The previously exposed API key was replaced. No replacement credential value belongs in repository evidence or chat. Replacement-key status does not itself authorize use.

The live Harness conservatively paces every Gemini API-bound operation, including `countTokens`, because current provider evidence does not establish that `countTokens` is exempt from request-rate accounting. Generation also enforces an exact rolling 60-second input-TPM window from the exact preflight token count. One attempt / zero retries remains law.

Because each accepted turn still contains three sequential roles and each role currently has a paced `countTokens` plus generation operation, provider quota and actual role latency are first-class experiment measurements. The comparison selects the quality/speed frontier, not automatically the cheapest, newest, or highest-RPM model.

## E-R1 closure

Contract: `docs/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE.md`.
Evidence: `docs/evidence/E_R1_REPOSITORY_TOOLING_WORKFLOW_RESTRUCTURE_EVIDENCE.md`.

PR #46 merge `21a10aff823734418f36284744a1fd26aef3bcf6`; PR #47 continuity merge `139621dc63e1a67aca5f1a5eedb7c1347ede9fd5`; final E-R1 branch head `bec3db07cef41296855be8f702d2f3637c472dc1` is preserved by `archive/repo-restructure-e-r1`. Final promoted-main Validation gate `34170749305` PASS. E-R1 contained no runtime source/test/fixture delta.

Plan-dependent `main` protection remains deferred.

## Boundary / next

The model-comparison amendment does not authorize real Gemini credentials, `countTokens`, provider-network execution, inference, or spend. It does not authorize E0-B+, app/UI/persistence, Windows AI/NPU, packaging/WACK/Store, or design-lane work.

Live transition handoff: `docs/handoff/E0A_POST_ER1_FIRST_REAL_GEMINI_RUN_DIRECTOR_GATE_HANDOFF_2026_09_07.md`.

**Next consequential gate: Director Windows ARM64 native validation of the active comparison branch.** The native gate must run Core tests, Harness tests, fresh native Harness build/fixture smokes, and credentialless refusal for all three explicit arm/profile pairings on one exact clean checkout. It must perform no Gemini network request.

Only after that native gate is recorded may the project return to a separate Director decision on the first real provider run. Planned comparison order, subject to separate authorization for each run:

```text
1. GEMINI-2.5-FLASH-LITE-NONE
2. GEMINI-3.5-FLASH-LITE-MINIMAL
3. GEMINI-2.5-FLASH-NONE
```
