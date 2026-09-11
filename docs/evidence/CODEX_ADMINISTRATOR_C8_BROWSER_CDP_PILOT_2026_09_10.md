# Codex Administrator C8 Browser/CDP Pilot — 2026-09-10

Status: **PASS — C8 mechanical commissioning evidence only**

## Authority and boundary

- Queue item: `Q-ADMIN-02`.
- Governing contract: `docs/blueprint/CODEX_ADMINISTRATOR_RUNTIME_SPECIFICATION.md`.
- Exact Project authority at pilot and closeout race check: `origin/main@19f6fbb03b890aaa34910a1dc27bd7fad30b6655`.
- Authorized machine: `SurfSeven`, device UUID `9b0f1184-49e4-44fb-9177-f32b29f2ec69`.
- C8 requirement: dedicated Kymaean browser profile, origin/session isolation, bounded permissions, browser-only evidence labeling, safe teardown, and no unrelated authenticated session visible.
- C8 created no Engineering, provider, product, Design, ODR, or validation authority.

## Preflight

The ordinary Edge browser was already running, so C8 did not attach to it. No Chrome, Brave, or Firefox process was active. No existing Kymaean C8 profile existed.

The commissioned Administrator runtime remained `codex-cli 0.153.4`, read-only/on-request by configuration and Doctor. Browser Use, external Browser Use, full CDP, computer use, Hooks, remote plugin, and multi-agent remained disabled by default.

A noninteractive `codex exec` preflight again reported `approval: never` and exposed no Browser Use tool. Its optional Cloudflare MCP startup path returned OAuth `AuthRequired`. No browser navigation occurred. This reproduced the C7 observation and classified that surface as unsuitable for C8 rather than broadening permissions.

A dedicated app-server under `C:\Users\Wiryl\.codex-ensemble` accepted an ephemeral read-only/on-request thread and strict per-origin Browser Use policy, but the Browser Use tool was still not model-visible. No navigation occurred. Direct Edge/CDP was therefore used as the bounded C8 browser capability.

## Dedicated Edge/CDP evidence

Microsoft Edge was launched with a fresh dedicated user-data root under `C:\Users\Wiryl\.codex-ensemble\browser`, headless Guest mode, sync/background networking/component update/default apps/extensions disabled, and a CDP listener bound only to `127.0.0.1`.

The first non-Guest dedicated profile was rejected after read-only metadata audit found one automatically associated OS account entry. No account identity was read. Current Microsoft Edge policy documentation explains that implicit sign-in is enabled by default unless browser sign-in/implicit sign-in policy is disabled. C8 did not alter machine/user-wide Edge policy because doing so would affect the ordinary browser.

The corrected dedicated root `kymaean-c8-guest` created an active `Guest Profile`. Its active-profile audit returned:

- `ACCOUNT_INFO_COUNT=0`;
- `LOGIN_COUNT=0` / Login Data absent;
- after shutdown, `COOKIE_COUNT=0`.

Any inactive auto-created `Default` profile and both failed attempt roots were deleted after measurement. The clean `Guest Profile` remained as the dedicated Kymaean development profile.

CDP discovery reported Edge `152.0.4191.66`, protocol `1.3`, with the listener bound to `127.0.0.1` only. The active Guest context initially exposed exactly one target: `about:blank`.

The same single target was navigated through CDP to the zero-network specimen:

`file:///C:/Users/Wiryl/.codex-ensemble/scratch/c8-browser-pilot/index.html`

A read-only CDP `Runtime.evaluate` returned exactly:

- `href`: the dedicated specimen `file://` URL;
- `origin`: `file://`;
- `title`: `Kymaean C8 Browser Pilot`;
- `h1`: `Kymaean C8 Browser Pilot`;
- `data-pilot`: `C8-LOCALHOST-ONLY`.

No external website, personal browser profile, browser history, credential value, upload, download, product-provider API, Run-05 evidence, or repository source file was accessed by the successful browser specimen.

## Safe teardown and evidence class

`Browser.close` over the browser CDP WebSocket returned success. Port `9224` stopped listening, the dedicated Edge root exited, and the pre-existing ordinary Edge session remained running. The closed Guest Profile then audited to zero account entries, zero cookies, and no Login Data database.

Browser/CDP evidence is **browser evidence only**. It is not WinUI, native Windows ARM64, accessibility, WACK, Store, provider, experiment, or validation-rung evidence.

No browser capability was made ambient: the Administrator base configuration remains browser-off by default. C8 proves that the bounded capability can be invoked explicitly with a dedicated Guest profile and loopback-only CDP, then torn down cleanly.

## Failure taxonomy / recursive audit

- Noninteractive `codex exec` Browser Use absence: `TOOL_FAILURE`; no navigation.
- Dedicated app-server Browser Use absence: `TOOL_FAILURE`; no navigation.
- First direct profile auto-associated OS account: isolation falsification; rejected before adoption and corrected by changing the execution hypothesis to Guest context.
- Generic loopback HTTP fixture returned empty replies; it was removed from the proof. The accepted specimen is zero-network `file://`.
- No blind retry loop occurred; each subsequent attempt changed the identified failing surface or isolation hypothesis.

**C8 mechanical recommendation: PASS. C9 is not started by this record.**
