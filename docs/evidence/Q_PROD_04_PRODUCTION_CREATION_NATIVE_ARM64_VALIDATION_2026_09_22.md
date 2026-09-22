# Q-PROD-04 Production creation — native ARM64 validation

Date: 2026-09-22. Status: exact-source implementation candidate; Director A/A/A Product contract implemented; PR #246 unmerged.

## Identity

- Exact base: `f8a5a6008b2bad93a8a13b1927da302c34113e33`; exact-main Validation #1158 PASS.
- Exact tested executable source: `8f7d5f05909dd88f6dd24e20b60b80ae5b36f5cc`.
- Director decision: `docs/Q_PROD_04_PRODUCTION_CREATION_DIRECTOR_DECISION_2026_09_22.md`.
- Draft PR: #246, branch `qprod04/production-creation-2026-09-22`.

## Implemented Product contract

Director-approved **A / A / A**:

1. catalog/creation boundary issues a new opaque stable `ProductionId`;
2. creation returns authoritative identity + creation-only replay and updates ProductApplication's known Production list without implicitly opening/navigating/changing ProductSpace;
3. duplicate creator-facing Production names are allowed; identity disambiguates them.

Application owns `IProductionCreator` and `ProductionCreation`. `ProductApplication.CreateProduction` preserves active scope, existing open Production/replay, and prior ProductSpace state while adding a validated created summary to the known list.

## Persistence publication

`FileProductionCatalog.CreateProduction`:

- prevalidates the existing complete catalog;
- generates new opaque identity and an independent bounded technical locator;
- constructs identity metadata and the creation-only journal in a pending directory outside the discoverable `production-catalog` namespace;
- validates exact creation name + empty World current state before publication;
- publishes the fully formed directory into the catalog with one directory move;
- cleans unpublished pending construction on failure;
- does no post-publication I/O before returning the already-built authoritative replay.

`ProductionIdentityMetadata.Write` emits the existing version-1 identity framing with explicit big-endian UTF-16 code units and checksum. Creation-event persistence remains the existing lossless `kymaean.production.created.v2` event codec.

No rename/delete/import/restore, UI/FIRSTUSE/Home, Character/Scene ontology, provider/Performer, Stage or recovery/retry semantics were added.

## Exact native ARM64 validation

Host: SurfSeven, Windows 11 ARM64, native ARM64 PowerShell 7.6.6 and .NET SDK 9.0.317.

- Environment verifier: **PASS**, 10 projects, Windows SDK 10.0.26100.0.
- `tools/test-application.ps1 -Full`:
  - Application **66/66 PASS**;
  - Persistence **135/135 PASS**;
  - native `win-arm64`.
- `dotnet build src/Kymaean.Windows/Kymaean.Windows.csproj -c Release -p:Platform=ARM64 -r win-arm64`: **PASS**, 0 warnings / 0 errors.
- `git diff --check`: PASS.
- Native worktree remained clean.
- First environment-verifier attempt from the Remote Desktop process lacked the standard `ProgramFiles(x86)` environment value; rerun with the ordinary `C:\Program Files (x86)` value passed. This was a remote-process environment quirk, not a repository/source failure.

New native tests establish:

- duplicate Production names succeed with distinct identities;
- created identities remain openable after catalog reconstruction;
- creation-only replay starts with empty World state;
- exact accepted UTF-16 ProductionName code units, including lone-surrogate input, survive create/list/open;
- invalid/incompatible pre-existing catalogs reject creation without publishing another catalog entry;
- invalid creation names mutate nothing;
- ProductApplication does not implicitly open/navigate;
- creation preserves an already-open Production and its prior ProductSpace;
- typed creation failure preserves prior Application state;
- malformed creator success (duplicate identity, name mismatch, non-empty creation replay) fails closed.

## Hosted exact-source validation

- push Validation #1160: **PASS**;
- PR Validation #1161: **PASS**.
- Repository law, document census, oracle coverage, Core/Application/Persistence regressions and ARM64 cross-compilation all passed.

## Validation boundary

This is Application/Persistence/native target-device evidence only. No creation UI has been implemented or accepted. FIRSTUSE and Home A/B remain unresolved Design gates. No Alpha/final-architecture/provider/WACK/Store/release authority is created.

Integration requires final evidence/continuity head hosted PASS and explicit Director merge authorization.
