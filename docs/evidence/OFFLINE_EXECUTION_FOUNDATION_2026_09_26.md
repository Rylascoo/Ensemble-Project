# Offline execution foundation

Status: **AUTHORIZED IMPLEMENTATION CANDIDATE / VALIDATION IN PROGRESS / NOT MERGED**

## Authority and scope

The Director separately authorized this bounded implementation on 2026-09-26 after PR #279 integrated at `11ef377888ede2f31ea6505377962543b4016aa6` (exact-main Validation #1442 PASS). Base: that exact merge. Branch: `codex/offline-execution-foundation-2026-09-26`. This is Q-PROD-10, not provider admission or completion of the creator-facing interaction.

The adopted `docs/PRODUCT_EXECUTOR_ADMISSION_DECISION_PACKET_2026_09_25.md` remains governing. Product semantics, Application synchronous ports, journal/replay/revision behavior and all falsifiers are unchanged. PRs #262, #265 and #269 remain preserved/unmerged.

## Implemented boundary

- `Kymaean.Infrastructure.Execution` is a small inward-facing Application adapter project. It contains the two Product port adapters and an injected runtime interface, with **no runtime implementation**. Separate attributed requests carry the exact actor context; the interpreter receives the exact candidate. Missing/truncated/refused/mismatched output fails; explicit empty Performance is preserved. No trimming, repair, default circumstance, retries or fallback.
- `ProductOperationCoordinator` owns each Application through a shared weak-key registry. Startup transfers the mutable instance into this owner; Windows/Presentation receives only the owner. Every Product read/navigation/mutation goes through its guarded access. Cached immutable presentation reads do not call Product.
- Acquisition establishes Pending before notifications or scheduling. Exactly one worker may invoke. Other Product operations fail immediately, and every in-app navigation/edit entrypoint is guarded. The native navigation/content root disables input while pending; queued handlers also decline. The worker needs no dispatcher callback to finish.
- Target capture includes session/open/view generation, exact Production/Scene/Character IDs, expected Product history revision and display witnesses. Presentation movement invalidates old targets. Immutable terminal evidence is retained before any UI callback; only the identical request may publish or release. A stale result retains original attribution, freezes its Production until ordinary Open and cannot repaint a replacement view. A late callback cannot release a newer lease.
- Product success is immutable across rendering/notification faults. Publication failure is a separate flag. Typed Invalid remains outcome unknown; Incompatible remains distinct; observed executor exceptions are technical failures before commit, with I/O/access still conservatively requiring reopen. Unobserved invocation exceptions remain unknown. Dispatch rejection is known noncommit only when it wins the once-only start fence.
- Reopen blocks are Production-scoped and survive navigation, other Productions, Recover and failed Open. Only successful ordinary Open of the same Production clears that block. Earlier unknown request evidence remains in session; a new activation has a new session request ID and is not a retry protocol.
- Ordinary composition can only acquire an owner without executors. Injection and activation seams are internal to the Presentation test assembly. Neither shipping Windows nor Presentation references the execution adapters or tests. The ordinary Scene detail shows an unavailable explanation and disabled `Request & record Performance` entry. There is no enabled synthetic/live interaction or provider settings surface.

## Validation plan and limits

Focused native ARM64 Presentation tests cover absent/either-missing capability, same-name identity, duplicate/reentrant activation, manual scheduling and a worker barrier, all Product operations during Pending, stale/replacement views, old callbacks, success then render failure, failure classification, real journal append then Invalid, uncertainty clearing, exact empty/text mapping and invalid runtime outputs. Synthetic runtime code lives only in tests.

Standing gates: Application/Persistence (including unchanged Q-PROD-09 history/replay/ABA tests), Core/Harness, Preview tests; ordinary and Preview WinUI ARM64 builds; repository-law, document census and oracle guards; standing native ARM64 baseline at a clean exact executable commit; independent exact-ref behavioral no-write review. Final exact references/results will be recorded here before readiness is claimed.

No provider SDK, transport, endpoint, credential/configuration read, model selection, network discovery, health/count-token request or spend is introduced. The new execution sources use only Application contracts and in-memory mapping. Tests use isolated temporary journals and synthetic transports. GitHub publication/CI and existing build tooling are repository operations, not provider execution.

This package does not claim full interaction Design acceptance, pending WinUI human usability, Narrator/High Contrast, creator interpretation, provider quality, production exactly-once delivery or OS-crash durability. Native Presentation tests prove coordinator behavior; native startup/build evidence has its own narrower rung.

## Remaining prerequisites and non-authority

A real runtime/provider and supported model, exact request/response mapping, credentials, network/spend bounds and first-live execution must receive separate admission before ordinary Performance can be enabled. The real invocation UI and its native Design validation also remain separate. No Current/Active Scene, lifecycle, broader context/consequence, persistent casting, history browsing, Take/review, durable request protocol, E0 dependency, deferred-E0, ODR closure, architecture reopening or release/Store authority is created. All adopted falsifiers still return to Product/Architecture disposition.

Next: complete candidate evidence and review; draft PR only. Director merge authorization remains required.
