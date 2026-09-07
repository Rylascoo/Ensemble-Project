# Oracle assertion-coverage guard

Run `python tools/oracle-index.py --check` from the repository to reject loss of scanned assertion coverage for documented 64-hex oracle values. The historical script name is retained for continuity; E-R1 no longer commits a generated path inventory.

The scanner reads UTF-8 documents under `docs/` and C# files under `tests/`. Git-ignored files and build output are excluded; new unignored files are included. Hashes are bounded 64-hex sequences, normalized to lowercase.

ASSERTED means a documented hash occurs in either of the first two operands of an MSTest `Assert.AreEqual` or `CollectionAssert.AreEqual` call, directly or through a same-file `const string`. Comments and assertion-message arguments do not count. This is a lexical guard, not C# semantic analysis, reachability analysis, or proof that a test executes. Computed values, cross-file constants, generic assertion calls, and other assertion styles need an explicit scanner extension before they can be credited.

`--check` scans the current checkout and its first parent. `--baseline <ref>` additionally scans that prior revision. If a hash was both documented and ASSERTED in either comparison revision, the current checkout must still contain scanned assertion coverage for that hash. Moving a document between active and archive surfaces does not matter because path identity is not the safety property. Removing or renaming assertion coverage without an equivalent scanned assertion fails closed.

The guard intentionally does not require a documented hash to remain documented forever; documentation lifecycle and archive authority belong to the document census and Repository Surface laws. It also does not treat document paths as validation authority.

CI runs the guard against the push/PR baseline. The guard supplies repository-integrity evidence only. It does not execute tests and cannot establish behavioral, compiler, native runtime, provider-network, hardware/NPU, packaging, WACK, or Store authority.
