# Q-E0A-03 Gemini 3.5 Flash-Lite Run 04 — Terminal Evidence Analysis

Date: 2026-09-10

Status: **TERMINAL — NONCONTRIBUTING — PERFORMER CONTROL-IDENTITY MISALIGNMENT LOCALIZED**

## Exact run

- RunId: `E0A-Q03-G35L-20260910-04`
- executable: `e052ef4cdaf8ac7291971f8c42d121e1ee6eecf0`
- native tag: `validation/e0a-state-interpreter-semantic-output-alignment-native-arm64`
- profile: `CREATIVE-MINIMAL / GEMINI-3.5-FLASH-LITE-MINIMAL`
- fixture: `ensemble.e0.missing-raft@0.1.0`
- canonical fixture hash: `5556a02325e6a7f774e6997942b395d670741d494ea86f1a50b83633e26b6703`
- terminal: `InvalidOutput`
- accepted turns: `0`
- final Opportunity: `VOSS`

Run 04 was executed exactly once under the standing project-relevant Gemini authority after its durable preexecution activation package was integrated and post-merge hosted Validation passed. It was not retried or replayed.

Activation predecessor: `docs/evidence/E0A_Q_E0A_03_G35L_RUN04_PREEXECUTION_ACTIVATION_2026_09_10.md`.

## Provider consumption

The run made exactly **2 Gemini API HTTP requests**: one Performer `models.countTokens(generateContentRequest)` preflight and one Performer generation request.

The token preflight reported 649 input tokens. The generation receipt succeeded from `gemini-3.5-flash-lite` and reported 649 input, 130 output, 0 reasoning, and 0 cached-input tokens. Paid-tier shadow estimate: USD `0.00051970`; `hasUnknownProviderUsage=false`.

## Runtime result

The generation response was syntactically valid and satisfied the provider JSON Schema. Its visible performance was coherent story text, but typed control returned `addressedCharacterIds=["Marlowe","Wren"]` and `nominatedCharacterId=null`.

The fixture roster canonical IDs are `MARLOWE`, `VOSS`, and `WREN`. `PerformerCandidateContract` compares parsed control IDs against the exact roster with ordinal identity. Display-style `Marlowe` and `Wren` are therefore outside that exact ID set, and the deterministic parser correctly rejected the candidate before Integrity with:

`Candidate control addressedCharacterIds contains a Character outside the roster.`

This is not a Gemini transport, model-identity, schema-encoding, quota, timeout, credential, or usage-accounting failure. It is schema-valid but application-semantically-invalid Performer control identity.

## Evidence integrity

Every artifact listed by `run.final.json` independently matched its recorded SHA-256. The evidence root contains 10 files and one provider-attempt directory.

- `run.final.json` SHA-256: `1d7f63c4949de2b6a5981ebea98926e3d9d3b253ec804ab5784ec7af066f5dbb`
- runtime root: `b06a90903a00c7d90a7f210c52845b4782a65e16211edbe8cc0ab137f392aec3`
- runtime seal identity: `6ea8ad4d24c71bd2037b748664e4f5c8915f9b98d1813fc271f59a1a01e66211`
- independently verified artifact-hash mismatches: `0`

## Project relevance and contribution

Run 04 confirms that the native-validated Run 03 Interpreter correction did not regress Gemini transport or accounting, but it cannot contribute to the E0-A reference: the frozen contribution law requires all 12 turns to reach accepted Take/commit and Run 04 accepted zero.

The two requests were decision-changing. They exposed an earlier semantic interface hole and eliminate any justification for another provider call under the same request contract. Run 04 is immutable/consumed and must never be replayed.

## Correction boundary

The Performer response contract requires canonical Character IDs for typed address/nomination control, but the bounded provider context exposed only the subject/opportunity canonical ID. Other roster members appeared in rendered story text by display name, so the model was not supplied the exact identifiers it was required to emit.

The narrow correction is to expose the roster's already-authoritative canonical Character IDs in the bounded structured context and explicitly require exact case-sensitive reuse for typed Character-ID fields. The same bounded ID set should be available to Interpreter because its relationship and Character-domain fields are governed by the same exact roster identity law.

Application parsers remain authoritative. No case normalization, fuzzy matching, display-name coercion, retry, repair, fallback, Fixture change, or authority relaxation is permitted.

Provider traffic returns to **zero** until this identity-interface correction is recursively audited, machine-validated on native Windows ARM64, tagged, integrated, and freshly activated under the standing authorization law.