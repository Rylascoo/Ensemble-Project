#!/usr/bin/env python3
import hashlib,json,re,sys
from pathlib import Path
R=Path(__file__).resolve().parents[1]
P={
 'AUD':R/'docs/evidence/PHASE_C_POST_DUR_01_REENTRY_AUDIT_08.json',
 'METHOD':R/'docs/evidence/APPICON_01_PRIMARY_APP_ICON_DESIGN_MASTER_METHOD_01.json',
 'PREF':R/'docs/evidence/APPICON_01_MECHANICAL_ACCESSIBILITY_PREFLIGHT_01.json',
 'EVAL':R/'docs/evidence/APPICON_01_DESIGN_SOL_ENVELOPE_EVALUATION_01.json',
 'PACK':R/'docs/evidence/packets/PKT_APPICON_01_PRIMARY_APP_ICON_DESIGN_MASTER_01.json',
 'CARRIER':R/'prototypes/appicon-01/carrier.html',
 'PROBE':R/'tools/appicon01_browser_probe.mjs',
 'C0':R/'assets/brand/kymaean-threshold-k-working-candidate.svg',
 'REG':R/'docs/evidence/DESIGN_PACKET_REGISTRY_01.json',
 'CUR':R/'CURRENT_STATE.md','LED':R/'docs/evidence/DESIGN_LEDGER.md',
 'HAND':R/'docs/HANDOFF_APPICON_01_PRIMARY_APP_ICON_DESIGN_MASTER_2026_09_14.md',
 'ATTR':R/'.gitattributes'}
H={'AUD':'305e4c7fff0ddd58cd0c43fb0da5a9c62f6535e231a5818d28e0246e0ab29447','METHOD':'37bd7fff587779167a2062868430829f2fc660da7753214ea15a1f43b0c7712a','PREF':'bec35eccabc14429ec4e15eb5e912c9b2a993a2d765e4a7d3148e56f650ec1ea','EVAL':'960d5266aec673010b28a306582d669a3ea7047f414d7bc65567167e634a565d','PACK':'9e0782c3e13642c3239e421e5079847da122022f0244400b2f31ddea1f15add0','CARRIER':'0a5a4fcbdd4927830f32aba54e8bd9d1a5512d4d74c592d717bc81258fb933f1','PROBE':'22af9f2365a7df6141cf9997bdbef446d5202ec83691eade9c7a34ad8735d46a','C0':'72cdd4c35928e1fb0bc279b680b9b7a698707dc1564ee800a84840fe61356305'}
SEM='95304d003e63ab2272501da3451c1bca3deb84d206468d7e35264575d2bfe30d'
def b(p): return p.read_bytes().replace(b'\r\n',b'\n')
def sha(p): return hashlib.sha256(b(p)).hexdigest()
def load(p): return json.loads(p.read_text(encoding='utf-8'))
def fail(x): raise AssertionError(x)
def main():
 for k in ['AUD','METHOD','PREF','EVAL','PACK','CARRIER','PROBE','C0']:
  got=sha(P[k])
  if got!=H[k]: fail(f'{k} hash mismatch {got}')
 src=P['C0'].read_text(encoding='utf-8')
 paths=re.findall(r'<path d="([^"]+)"',src)
 norm='|'.join(' '.join(x.split()) for x in paths).encode('utf-8')
 if len(paths)!=3 or hashlib.sha256(norm).hexdigest()!=SEM: fail('C0 path-semantic mismatch')
 aud,method,pref,ev,pack,reg=[load(P[k]) for k in ['AUD','METHOD','PREF','EVAL','PACK','REG']]
 if aud.get('status')!='COMPLETE_APPICON_01_ACTIVATED' or aud.get('selected_program',{}).get('id')!='APPICON-01': fail('activation audit state')
 if method.get('status')!='FROZEN_PRE_CONSTRUCTION_DETERMINISTIC_NON_RENDER': fail('method status')
 if method.get('activation_audit_sha256')!=H['AUD']: fail('method audit linkage')
 if [(x.get('id'),x.get('inset_percent')) for x in method.get('candidates',[])]!=[('E06',6),('E12',12),('E18',18)]: fail('candidate freeze')
 msrc=method.get('source',{})
 if msrc.get('canonical_lf_git_blob_sha256')!=H['C0'] or msrc.get('path_semantic_sha256')!=SEM: fail('method source provenance')
 if pref.get('status')!='PASS_COMPLETE_MATRIX' or len(pref.get('matrix',[]))!=4: fail('preflight matrix state')
 expected=[('normal',1200,False),('normal',320,False),('forced',1200,True),('forced',320,True)]
 actual=[(x.get('mode'),x.get('viewport_width'),x.get('forced_colors')) for x in pref['matrix']]
 if actual!=expected: fail(f'preflight matrix identity {actual}')
 for x in pref['matrix']:
  if x.get('overflow_count')!=0 or x.get('record_count')!=36: fail('preflight mechanics')
  if any(x.get(k)!=0 for k in ['images','background_image_elements','external_resources','animated_or_transitioning_elements']): fail('preflight resource/motion isolation')
 sf=pref.get('source_fidelity',{})
 if sf.get('canonical_lf_git_blob_sha256')!=H['C0'] or sf.get('path_semantic_sha256')!=SEM or sf.get('path_count')!=3: fail('preflight source provenance')
 if not all(sf.get(k) is True for k in ['all_instances_square','all_instances_center_source_viewbox_at_128_128','all_instances_three_paths_identical','all_paths_inside_canvas','currentColor_only']): fail('source fidelity')
 if pref.get('carrier_sha256')!=H['CARRIER'] or pref.get('browser_probe_sha256')!=H['PROBE']: fail('preflight tool linkage')
 if len(pref.get('invalid_attempts',[]))!=2: fail('invalid-attempt accounting')
 if ev.get('status')!='CLOSED_SINGLE_SURVIVOR' or ev.get('survivors')!=['E06'] or ev.get('director_adjudication_required') is not False: fail('evaluation survivor state')
 cr=ev.get('candidate_results',{})
 expected_classes={'E06':'ENVELOPE_SURVIVES','E12':'TARGETED_REFINEMENT_REQUIRED','E18':'REJECTED'}
 if {k:cr.get(k,{}).get('class') for k in expected_classes}!=expected_classes: fail('candidate classes')
 if ev.get('preflight_sha256')!=H['PREF'] or ev.get('carrier_sha256')!=H['CARRIER']: fail('evaluation linkage')
 nr=ev.get('no_retune',{})
 if nr!={'criteria_frozen_before_construction':True,'candidate_geometry_changed_after_preference':False,'carrier_changed_after_valid_matrix':False}: fail('no-retune record')
 if pack.get('packet_id')!='PKT-APPICON-01-01' or pack.get('status')!='FROZEN_LOCAL_RESULT': fail('packet state')
 if pack.get('local_result',{}).get('director_taste_gate_triggered') is not False: fail('packet director gate')
 evid=pack.get('evidence',{})
 for k,want in [('mechanical_preflight_sha256',H['PREF']),('design_sol_evaluation_sha256',H['EVAL']),('carrier_sha256',H['CARRIER']),('browser_probe_sha256',H['PROBE'])]:
  if evid.get(k)!=want: fail(f'packet evidence linkage {k}')
 rows=[x for x in reg.get('packets',[]) if x.get('packet_id')=='PKT-APPICON-01-01']
 if len(rows)!=1 or rows[0].get('status')!='FROZEN_LOCAL_RESULT' or rows[0].get('production_authority') is not False: fail('registry packet row')
 sp=reg.get('synthesis_policy',{})
 if sp.get('active_appicon_program')!='APPICON-01' or sp.get('active_appicon_status')!='E06_SINGLE_SURVIVOR_CLOSED': fail('registry appicon state')
 for k,want in [('active_appicon_method_sha256',H['METHOD']),('active_appicon_activation_audit_sha256',H['AUD']),('active_appicon_preflight_sha256',H['PREF']),('active_appicon_evaluation_sha256',H['EVAL']),('active_appicon_packet_sha256',H['PACK'])]:
  if sp.get(k)!=want: fail(f'registry linkage {k}')
 cur=P['CUR'].read_text(encoding='utf-8'); led=P['LED'].read_text(encoding='utf-8'); hand=P['HAND'].read_text(encoding='utf-8')
 for token in ['APPICON-01 - Primary App Icon Design-Master Convergence','CLOSED / E06 SINGLE SURVIVOR']:
  if token not in cur: fail(f'CURRENT_STATE missing {token}')
 if len(P['CUR'].read_bytes())>3072: fail('CURRENT_STATE exceeds 3 KiB')
 for token in ['## L-165 - APPICON-01 closes with E06 as the sole design-master envelope survivor',H['PREF'],H['EVAL'],H['PACK'],H['CARRIER'],H['PROBE']]:
  if token not in led: fail(f'ledger missing {token}')
 for token in ['APPICON-01 CLOSED E06',H['PREF'],H['EVAL'],H['PACK'],'post-APPICON']:
  if token not in hand: fail(f'handoff missing {token}')
 attrs=P['ATTR'].read_text(encoding='utf-8')
 for rel in ['docs/evidence/APPICON_01_MECHANICAL_ACCESSIBILITY_PREFLIGHT_01.json','docs/evidence/APPICON_01_DESIGN_SOL_ENVELOPE_EVALUATION_01.json','docs/evidence/packets/PKT_APPICON_01_PRIMARY_APP_ICON_DESIGN_MASTER_01.json','prototypes/appicon-01/carrier.html','tools/appicon01_browser_probe.mjs']:
  if f'{rel} text eol=lf' not in attrs: fail(f'LF policy missing {rel}')
 for token in ['No final shared identity or production symbol/vector authority.','No Store/package/MSIX/shipping/native/runtime implementation.']:
  if token not in pack.get('explicit_non_authority',[]): fail(f'packet authority boundary missing {token}')
 for p in P.values():
  if not p.exists(): fail(f'missing {p.relative_to(R)}')
  if p.suffix.lower() in {'.json','.md','.py','.mjs','.html','.svg'} or p.name=='.gitattributes':
   txt=p.read_text(encoding='utf-8')
   bad=[ord(c) for c in txt if ord(c)<32 and c not in '\n\r\t']
   if bad: fail(f'control chars in {p.relative_to(R)}')
 print('APPICON_01_CHECKPOINT=PASS')
 print('RESULT=E06_SINGLE_SURVIVOR E12=TARGETED_REFINEMENT_REQUIRED E18=REJECTED')
 print('C0=IMMUTABLE DESIGN_MASTER_ONLY=TRUE SHIPPING=NOT_AUTHORIZED')
 return 0
if __name__=='__main__':
 try: sys.exit(main())
 except Exception as e:
  print(f'APPICON_01_FAIL: {e}',file=sys.stderr)
  sys.exit(1)
