from __future__ import annotations
import argparse, base64, hashlib, json
from pathlib import Path
from PIL import Image
EXPECTED_SHA='7f3b3a815d71d1aa47350e69824f14f92a5dd399cf0c68bf16074827ead6b3c0'
EXPECTED_BYTES=1431659
EXPECTED_DIMS=(1672,941)

def sha256(p:Path)->str:
    return hashlib.sha256(p.read_bytes()).hexdigest()

def main()->None:
    ap=argparse.ArgumentParser()
    ap.add_argument('--source',required=True)
    ap.add_argument('--outdir',required=True)
    ap.add_argument('--repo',default=str(Path(__file__).resolve().parents[1]))
    a=ap.parse_args(); repo=Path(a.repo); src=Path(a.source); out=Path(a.outdir); out.mkdir(parents=True,exist_ok=True)
    if sha256(src)!=EXPECTED_SHA or src.stat().st_size!=EXPECTED_BYTES: raise SystemExit('source identity mismatch')
    master=Image.open(src).convert('RGBA')
    if master.size!=EXPECTED_DIMS: raise SystemExit('source dimensions mismatch')
    method=json.loads((repo/'docs/evidence/APPUI_01_STAGE_PRESENTATION_SUCCESSOR_METHOD_01.json').read_text(encoding='utf-8'))
    template=(repo/'prototypes/appui-01/stage-presentation-successor-01.template.html').read_text(encoding='utf-8')
    rows=[]
    for c in method['candidates']:
        l,t,r,b=map(int,c['source_rect']); cand=master.crop((l,t,r,b)).convert('RGBA')
        source_crop=master.crop((l,t,r,b)).convert('RGBA')
        if cand.size!=tuple(c['output_dimensions']): raise SystemExit(f"{c['id']} dimensions mismatch")
        for box in c.get('alpha_notches',[]):
            nl,nt,nr,nb=map(int,box)
            px=cand.load()
            for y in range(nt,nb):
                for x in range(nl,nr):
                    rr,gg,bb,_=px[x,y]; px[x,y]=(rr,gg,bb,0)
        if cand.convert('RGB').tobytes()!=source_crop.convert('RGB').tobytes(): raise SystemExit(f"{c['id']} RGB mutation")
        alpha=cand.getchannel('A'); transparent=sum(1 for v in alpha.getdata() if v==0)
        expected_transparent=sum((x2-x1)*(y2-y1) for x1,y1,x2,y2 in c.get('alpha_notches',[]))
        if transparent!=expected_transparent: raise SystemExit(f"{c['id']} alpha mismatch")
        png=out/f"{c['id']}.png"; cand.save(png,format='PNG',optimize=False)
        data='data:image/png;base64,'+base64.b64encode(png.read_bytes()).decode('ascii')
        proof=f"{cand.width}×{cand.height} · crop {l},{t}→{r},{b} · RGB exact · transparent pixels {transparent}"
        html=template.replace('__CANDIDATE_LABEL__',c['id']).replace('__CANDIDATE_DATA_URI__',data)
        html=html.replace('__CAND_W__',str(cand.width)).replace('__CAND_H__',str(cand.height)).replace('__CANDIDATE_PROOF__',proof).replace('__CANDIDATE_ID__',c['id'])
        if '__CANDIDATE_' in html or '__CAND_' in html: raise SystemExit(f"{c['id']} unresolved placeholder")
        runtime=out/f"runtime-{c['id']}.html"; runtime.write_text(html,encoding='utf-8',newline='\n')
        rows.append({'id':c['id'],'rect':[l,t,r,b],'dimensions':[cand.width,cand.height],'alpha_notches':c.get('alpha_notches',[]),'transparent_pixels':transparent,'png_sha256':sha256(png),'png_bytes':png.stat().st_size,'runtime_sha256':sha256(runtime)})
    manifest={'source_sha256':EXPECTED_SHA,'source_bytes':EXPECTED_BYTES,'source_dimensions':list(EXPECTED_DIMS),'template_sha256':hashlib.sha256(template.encode('utf-8')).hexdigest(),'candidates':rows}
    (out/'manifest.json').write_text(json.dumps(manifest,indent=2)+'\n',encoding='utf-8',newline='\n')
    print(json.dumps(manifest))

if __name__=='__main__':
    main()
