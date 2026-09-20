from __future__ import annotations
import argparse, base64, hashlib, json, struct
from pathlib import Path
EXPECTED_SHA="7f3b3a815d71d1aa47350e69824f14f92a5dd399cf0c68bf16074827ead6b3c0"
EXPECTED_BYTES=1431659
EXPECTED_DIMS=(1672,941)

def png_dims(data: bytes) -> tuple[int,int]:
    if data[:8] != b"\x89PNG\r\n\x1a\n" or data[12:16] != b"IHDR": raise ValueError("not canonical PNG")
    return struct.unpack(">II", data[16:24])

def main() -> None:
    ap=argparse.ArgumentParser(); ap.add_argument("template"); ap.add_argument("stage"); ap.add_argument("output"); a=ap.parse_args()
    template=Path(a.template).read_text(encoding="utf-8"); stage=Path(a.stage).read_bytes()
    sha=hashlib.sha256(stage).hexdigest(); dims=png_dims(stage)
    if sha!=EXPECTED_SHA or len(stage)!=EXPECTED_BYTES or dims!=EXPECTED_DIMS: raise SystemExit(f"stage mismatch sha={sha} bytes={len(stage)} dims={dims}")
    if template.count("__STAGE_DATA_URI__") != 1: raise SystemExit("template Stage placeholder count != 1")
    uri="data:image/png;base64,"+base64.b64encode(stage).decode("ascii")
    runtime=template.replace("__STAGE_DATA_URI__",uri); out=Path(a.output); out.parent.mkdir(parents=True,exist_ok=True); out.write_text(runtime,encoding="utf-8",newline="\n")
    manifest={"stage_sha256":sha,"stage_bytes":len(stage),"stage_dimensions":dims,"runtime_sha256":hashlib.sha256(runtime.encode()).hexdigest(),"template_sha256":hashlib.sha256(template.encode()).hexdigest()}
    out.with_suffix(".manifest.json").write_text(json.dumps(manifest,indent=2)+"\n",encoding="utf-8")
    print(json.dumps(manifest))
if __name__=="__main__": main()
