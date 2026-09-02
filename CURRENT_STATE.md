# Ensemble Current State

Updated: 2026-09-01

## Authority
- Blueprint 0.1 is FROZEN FOR E0.
- E0-A Preparation Blueprint 0.2 is approved.
- H1 Deterministic Spine Blueprint 0.2 is approved.
- GitHub `Rylascoo/Ensemble-Project` is the authoritative engineering source once code/checkpoints are committed here.
- Google Drive `Ensemble Project` is the design repository for UI/UX architecture, mockups/prototypes, visual identity/artwork, motion/animation, Store/marketing assets, and research/reference material.

## Current phase
E0-A Harness Implementation — H1 Deterministic Spine.

## Current implementation boundary
H1 Patch 0001 only:
1. .NET 9 Core + console Harness skeleton.
2. Strong typed IDs.
3. Strict fixture JSON ingestion/preflight.
4. Fixture envelope transport DTOs.
5. Validation entrypoints.
6. Native `win-arm64` harness target and runtime architecture guard.
7. `global.json` pins SDK 9.0.317 for H1 reproducibility on the validated machine.

Do not advance yet into canonical fixture hashing, full Missing Raft semantic schema, Access Control, persistence, provider integration, WinUI, Windows AI Foundry, NPU/QNN, Store packaging, or post-E0 product implementation.

## GitHub checkpoint
- Canonical implementation branch: `h1-source`.
- Draft PR: #1 `H1: bootstrap deterministic spine foundation`.
- PR remains open while runtime sanity validation is pending.

## Validation authority
- Static review: advisory only.
- Visual Studio / `dotnet` ARM64 build output: compiler authority.
- Actual target-device execution: runtime authority.
- NPU execution requires explicit hardware evidence.
- WACK and Partner Center remain later independent authorities.

## Current validation state
- H1 Patch 0001 has been statically reviewed in GitHub.
- Native machine evidence confirms Windows `win-arm64`, .NET host architecture `arm64`, .NET SDK 9.0.317 installed, and .NET runtime 9.0.19 installed.
- On 2026-09-01, `dotnet build .\src\Ensemble.E0.Harness\Ensemble.E0.Harness.csproj -c Debug` succeeded on the target Windows ARM64 machine using SDK 9.0.317.
- `Ensemble.E0.Core` compiled successfully to `bin\Debug\net9.0\Ensemble.E0.Core.dll`.
- `Ensemble.E0.Harness` compiled successfully to `bin\Debug\net9.0\win-arm64\Ensemble.E0.Harness.dll`.
- Build completed with no reported compiler errors; this establishes compiler validation only, not runtime behavior.
- No Windows ARM64 runtime validation exists yet.
- No Windows AI/NPU/Store validation exists.

## Immediate next action
Run the H1 Harness once on the same ARM64 machine to validate the runtime architecture guard and basic process startup. If clean, record Patch 0001 as compiler+runtime-sanity validated, complete the PR review gate, and then design H1 Patch 0002 for the full typed Missing Raft fixture schema and semantic invariant validation.

## Project rule
Patch first. Freeze only evidence-earned decisions. After every meaningful phase: correctness -> consistency -> authority -> evidence -> simplicity -> vision -> implementation readiness.
