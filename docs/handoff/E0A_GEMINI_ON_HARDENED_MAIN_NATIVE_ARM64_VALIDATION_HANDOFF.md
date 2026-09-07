# E0-A Gemini on Hardened Main — Native Windows ARM64 Validation Handoff

Status: **READY FOR DIRECTOR-MACHINE VALIDATION — PROVIDER NETWORK GATE CLOSED**

Date: **2026-09-07**

## Validation target

Repository:

`Rylascoo/Ensemble-Project`

Integration branch:

`e0a-gemini-on-hardened-main`

Exact executable/test checkout to validate:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

Hardened composition base:

`aa9a3d0dfbba9e882b67a7d0d9d1d510374103fd`

Static closure:

`docs/evidence/E0A_GEMINI_ON_HARDENED_MAIN_INTEGRATION_STATIC_CLOSURE.md`

The target passed GitHub Actions ARM64-target cross-compile/compiler validation with 0 warnings and 0 errors in all four project builds. That is not native runtime authority.

## Native scope

Validate only fake-only / credentialless behavior on the Director's native Windows ARM64 host:

1. exact checkout and working-tree authority;
2. trusted Windows ARM64 host probes;
3. Core tests;
4. Harness tests;
5. fresh native ARM64 Harness build after deleting target outputs;
6. Missing Raft fixture smoke;
7. generic fixture smoke;
8. credentialless Gemini `e0a-run CREATIVE-NONE` expected-refusal gate;
9. post-validation exact checkout and tracked/staged cleanliness.

Do **not** supply or use a Gemini credential. Do not make `countTokens`, inference, or any provider-network request. Do not spend provider funds.

## Host apparatus authority

Generate/execute the validation sequence under:

`docs/evidence/DIRECTOR_WINDOWS_ARM64_VALIDATION_HOST_BEHAVIORS.md`

In particular:

- trust `PROCESSOR_ARCHITECTURE=ARM64` plus parsed `dotnet --info` (`RID: win-arm64`, Host `Architecture: arm64`), not the known blank PowerShell `RuntimeInformation` probes;
- capture `$LASTEXITCODE` immediately after native commands used as oracles;
- expected-failure native stderr must be isolated from `$ErrorActionPreference='Stop'` using temporary stdout/stderr files and a temporary `Continue` setting;
- delete prior Harness target `bin/Debug/net9.0/win-arm64` and corresponding target-specific `obj` output before the authoritative build;
- if the current-checkout Harness build fails, hard-stop all executable smokes so stale output cannot be mistaken for current authority;
- keep complete interactive `if ... else ...` expressions in one submitted statement when assigning captured output.

## Credentialless expected-refusal oracle

Before the live-host probe, both environment variables must be absent:

```text
OPENAI_API_KEY
GEMINI_API_KEY
```

Invoke the freshly built Harness DLL using explicit `e0a-run CREATIVE-NONE`, exact checkout identity `5f286e8cfa896d38d85d4611f69a224fae5b55fd`, the approved fixture, a fresh RunId, and a nonexistent temporary evidence root.

Required result:

```text
native exit       1
stderr contains   GEMINI_API_KEY is required at the E0-A provider edge.
evidence root     absent
OPENAI_API_KEY    absent
GEMINI_API_KEY    absent
```

This proves only that the exact native compiled host reaches the Gemini provider edge and fails closed before evidence-root creation when its credential is absent.

## Required classification

If all steps pass, classify the exact checkout as:

```text
Checkout / working-tree authority        PASS
Native Windows ARM64 host probes         PASS
Core tests                               PASS
Harness tests                            PASS
Fresh native ARM64 Harness build         PASS
Missing Raft smoke                       PASS
Generic fixture smoke                    PASS
Credentialless Gemini provider edge      PASS expected refusal
Evidence-root absence                    PASS
Gemini credential use                    NOT PERFORMED
Gemini token-count network request        NOT PERFORMED
Gemini inference                          NOT PERFORMED
Gemini provider-network execution         NOT PERFORMED
Gemini spend                              NOT PERFORMED
Overall credentialless native validation PASS
```

Do not infer live API compatibility, model availability, account quota/tier, pricing/data-use freshness, cache behavior, provider usage accounting, or inference correctness from this validation.

## Repository Surface gate after PASS

A successful native validation must be filed as evidence and followed by an annotated validation tag at exactly:

`5f286e8cfa896d38d85d4611f69a224fae5b55fd`

The tag message must record validation level, fake-only/credentialless scope, and the evidence document path. Only after that durable tag exists may `CURRENT_STATE.md` promote the combined checkpoint as machine-validated runtime authority.
