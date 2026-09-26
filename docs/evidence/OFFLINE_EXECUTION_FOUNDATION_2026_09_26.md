# Offline execution foundation

Status: **NATIVE ARM64 PASS / REVIEW CLEAN / CANDIDATE NOT MERGED**

## Authority and scope

The Director separately authorized this bounded implementation on 2026-09-26 after PR #279 integrated at `11ef377888ede2f31ea6505377962543b4016aa6` (exact-main Validation #1442 PASS). Base: that exact merge. Branch: `codex/offline-execution-foundation-2026-09-26`. This is Q-PROD-10, not provider admission or completion of the creator-facing interaction.

The adopted `docs/PRODUCT_EXECUTOR_ADMISSION_DECISION_PACKET_2026_09_25.md` remains governing. Product semantics, Application synchronous ports, journal/replay/revision behavior and all falsifiers are unchanged. PRs #262, #265 and #269 remain preserved/unmerged.

## Implemented boundary

- `Kymaean.Infrastructure.Execution` is a small inward-facing Application adapter project. It contains the two Product port adapters and an injected runtime interface, with **no runtime implementation**. Separate attributed requests carry the exact actor context; the interpreter receives the exact candidate. Missing/truncated/refused/mismatched output fails; explicit empty Performance is preserved. No trimming, repair, default circumstance, retries or fallback.
- `ProductOperationCoordinator` owns each Application through a shared weak-key registry. Startup transfers the mutable instance into this owner; Windows/Presentation receives only the owner. Every Product read/navigation/mutation goes through its guarded access. Cached immutable presentation reads do not call Product.
- Acquisition establishes Pending before notifications or scheduling. Exactly one worker may invoke. Other Product operations fail immediately, and every in-app navigation/edit entrypoint is guarded. The native navigation/content root disables input while pending; queued handlers also decline. The worker needs no dispatcher callback to finish.
- Target capture includes session/open/view generation, exact Production/Scene/Character IDs, expected Product history revision and display witnesses. Presentation movement invalidates old targets. Immutable terminal evidence is retained before any UI callback; only the identical request may publish or release. A stale unstarted request is rejected without executor invocation; an already-started stale result retains original attribution, freezes its Production until ordinary Open and cannot repaint a replacement view. A late callback cannot release a newer lease.
- Product success is immutable across rendering/notification faults. Publication failure has a separate cause/diagnostic flag. WinUI supplies an explicit TryEnqueue publication boundary; rejection fences any late callback, retains the terminal outcome and freezes its Production without worker-thread UI notifications. Typed Invalid remains outcome unknown; Incompatible remains distinct; observed executor exceptions are technical failures before commit, with I/O/access still conservatively requiring reopen. Unobserved invocation exceptions remain unknown. Commit knowledge, failure cause and bounded exception type are recorded independently; raw exception/provider text is not published. Dispatch rejection is known noncommit only when it wins the once-only start fence.
- Reopen blocks are Production-scoped and survive navigation, other Productions, Recover and failed Open. Only successful ordinary Open of the same Production clears that block. Earlier unknown request evidence remains in session; a new activation has a new session request ID and is not a retry protocol.
- Ordinary composition can only acquire an owner without executors. Injection and activation seams are internal to the Presentation test assembly. Neither shipping Windows nor Presentation references the execution adapters or tests. The ordinary Scene detail shows an unavailable explanation and disabled `Request & record Performance` entry. There is no enabled synthetic/live interaction or provider settings surface.

## Validation plan and limits

Focused native ARM64 Presentation tests cover absent/either-missing capability, same-name identity, duplicate/reentrant activation, manual scheduling and a worker barrier, all Product operations during Pending, stale/replacement views, old callbacks, success then render failure, failure classification, real journal append then Invalid, uncertainty clearing, exact empty/text mapping and invalid runtime outputs. Synthetic runtime code lives only in tests.

Standing gates: Application/Persistence (including unchanged Q-PROD-09 history/replay/ABA tests), Core/Harness, Preview tests; ordinary and Preview WinUI ARM64 builds; repository-law, document census and oracle guards; standing native ARM64 baseline at a clean exact executable commit; independent exact-ref behavioral no-write review. Final exact references/results are recorded below.

No provider SDK, transport, endpoint, credential/configuration read, model selection, network discovery, health/count-token request or spend is introduced. The new execution sources use only Application contracts and in-memory mapping. Tests use isolated temporary journals and synthetic transports. GitHub publication/CI and existing build tooling are repository operations, not provider execution.

This package does not claim full interaction Design acceptance, pending WinUI human usability, Narrator/High Contrast, creator interpretation, provider quality, production exactly-once delivery or OS-crash durability. Native Presentation tests prove coordinator behavior; native startup/build evidence has its own narrower rung.

## Remaining prerequisites and non-authority

A real runtime/provider and supported model, exact request/response mapping, credentials, network/spend bounds and first-live execution must receive separate admission before ordinary Performance can be enabled. The real invocation UI and its native Design validation also remain separate. No Current/Active Scene, lifecycle, broader context/consequence, persistent casting, history browsing, Take/review, durable request protocol, E0 dependency, deferred-E0, ODR closure, architecture reopening or release/Store authority is created. All adopted falsifiers still return to Product/Architecture disposition.

Next: exact-final-head hosted gates and Director disposition of the draft PR. Director merge authorization remains required.

## Review correction record

Independent behavioral no-write review of `834d1fcdec9d3707c1efddecd057b33cc1ae3540` identified four P2s: implicit completion dispatch, stale-before-start invocation, missing independent technical-cause diagnostics, and invented runtime-installation copy. All four were corrected and covered by deterministic tests; the shipping copy is exactly **Performance is unavailable.** A separate exact-ref composition review found no production runtime/test fallback or dependency/isolation defect. Effective sandbox was workspace-write; technical read-only containment is not claimed. Corrected independent exact-ref review at `78e7537105d4de2020d90a9bb440856321234dba` returned no remaining actionable findings. The final native baseline at that same clean commit passed.

## Exact-source final validation

Executable candidate: `78e7537105d4de2020d90a9bb440856321234dba`. Annotated tag: `validation/offline-execution-foundation-native-arm64-20260926`. Subsequent closeout changes are documentation only and do not change this executable evidence boundary.

Host: SURFSEVEN, Windows 11 ARM64; process ARM64; .NET SDK 10.0.400. Standing `tools/native-arm64-baseline.ps1` completed successfully against the clean exact source above.

| Check | Result |
|---|---|
| Presentation, including 17 new offline foundation tests | 34/34 PASS |
| Application | 92/92 PASS |
| Persistence | 153/153 PASS |
| Core | 628/628 PASS |
| Harness | 155/155 PASS |
| Preview tool tests | 8/8 PASS |
| Ordinary and Director Preview WinUI Release ARM64 | PASS, 0 warnings / 0 errors each |
| Profiles helper and ordinary/Preview build isolation | PASS |
| Preview refresh, existing profile inventory/replay/selection preservation | PASS |
| Verified native launches and graceful closes | 3/3 PASS; nativeMachine 43620, processMachine 0 |
| Environment readiness / 12-project dependency classification | PASS |
| Repository law / CURRENT_STATE cap and currency / diff whitespace | PASS |
| Document census | 565 total / 443 current / 30 historical / 92 archive / 0 unexplained; PASS |
| Oracle guard | 927 documented hashes / 17 asserted / 910 document-only; PASS |
| Independent corrected exact-ref review | CLEAN; behavioral no-write |

Preserved local native evidence root: `C:/Users/Wiryl/Sol Dev/admin-scratch/offline-foundation-20260926/native-arm64-78e7537105d4-365f846b02d8494b824f43054b7073a6`.

- `baseline.json` SHA-256: `FD27926E7EC8C8E0AE4CCDA32F84F00E63C910977F866E92CDD272A080BAFEB3`.
- `presentation-tests.txt` SHA-256: `1E34838C3000D342BA8443D1F8EF9F35F4CEB347FC8A50924FAFFCE9A279A7A7`.
- Staged build receipt: `stages/78e7537105d4-341b8d056cd546cdbee21825b01d08f2/build.json`, SHA-256 `B1BB75FFEB16F04922F94650A863202D28C3374089EBD00157C13AC60D6D1166`. It binds all eight build/test command logs and staged executable inventory.
- Three exact-source launch receipts and executable paths are retained in `baseline.json`. Median harness launch measurement 27,095 ms and median working set 150,810,624 bytes include verification overhead; these are regression measurements, not responsiveness/Design acceptance KPIs.

Final scope: one new four-file Execution project; shared Application owner and Performance Presentation partial; guarded existing ViewModel/code-behind and startup ownership transfer; two Scene-detail unavailable UI elements; one new test file and test-project reference; solution/project-law registration; bounded continuity and validation records. Application, Persistence and E0 production source are unchanged. Existing hosted Presentation tests compile/test the new adapter project, so no CI workflow change is needed.

Canonical main remains `11ef377888ede2f31ea6505377962543b4016aa6`. Implementation worktree is `C:/Users/Wiryl/Sol Dev/Ensemble-Project-Worktrees/offline-execution-foundation-20260926`; the earlier documentation worktree and canonical main were observed clean. Historical worktrees/branches remain preservation-sensitive Q-ADMIN-07 residue; no cleanup or disposal was performed. Live #262/#265/#269 remain OPEN with no merge time. No provider/credential/live-AI action occurred.
