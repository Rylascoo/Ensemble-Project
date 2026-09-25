#!/usr/bin/env python3
import hashlib, json, sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
EV=ROOT/'docs/evidence'
AUDIT=EV/'PHASE_C_POST_CPS_01_REENTRY_AUDIT_07.json'
METHOD=EV/'DUR_01_INTEGRATED_IDENTITY_DURABILITY_METHOD_01.json'
PREF=EV/'DUR_01_MECHANICAL_ACCESSIBILITY_PREFLIGHT_01.json'
EVAL=EV/'DUR_01_DESIGN_SOL_INTEGRATED_DURABILITY_EVALUATION_01.json'
PACK=EV/'packets/PKT_DUR_01_INTEGRATED_IDENTITY_DURABILITY_01.json'
DECISION=EV/'CPS_01_DIRECTOR_ARCHITECTURE_DECISION_01.json'
REGISTRY=EV/'DESIGN_PACKET_REGISTRY_01.json'
CARRIER=ROOT/'prototypes/dur-01/carrier.html'
PROBE=ROOT/'tools/dur01_browser_probe.mjs'
CURRENT=ROOT/'CURRENT_STATE.md'; LEDGER=EV/'DESIGN_LEDGER.md'
HANDOFF=ROOT/'docs/HANDOFF_DUR_01_INTEGRATED_IDENTITY_DURABILITY_2026_09_14.md'
EXPECTED={AUDIT:'7623092491bdc705351cc74d856ab15b61be04a71ae0d72ef3f5e27c956080f3',METHOD:'7b82ddc895843f27e7ca8b0740b5108fa0b44f2a29507e129cf6aa14e9d8c308',PREF:'2c845d354e13ee872a74cf4a82b1533f6168f94711baa4d88cdbad7b2a2de3e8',EVAL:'84ffd1639b169cc32040e308e9e1cb459437fe2ff6bd506663d213aea1d87a4a',PACK:'a792c7508de8c3741b89cfba9face7d1ea78ff1608ff307c43d00db9bbaf330c',DECISION:'02f14c5360b07ca6b5d04f80e16fb9877976eee0c4a0ec10f1d9a924f82413a8',CARRIER:'88384760873d44198cf94f1ac242eeaa8d30027b61f9760073ad40a3bffde971',PROBE:'06b440e91ea57a8231518595f67fe0bbbe59160c32bf8192866e48300a9e918d'}
def sha(p): return hashlib.sha256(p.read_bytes()).hexdigest()
def load(p): return json.loads(p.read_text(encoding='utf-8'))
def fail(s): raise AssertionError(s)
def main():
 for p,h in EXPECTED.items():
  if not p.exists(): fail(f'missing {p.relative_to(ROOT)}')
  actual=sha(p)
  if actual!=h: fail(f'hash mismatch {p.relative_to(ROOT)}: {actual}')
 audit,method,pref,ev,packet,decision,reg=map(load,[AUDIT,METHOD,PREF,EVAL,PACK,DECISION,REGISTRY])
 if audit.get('selected_program',{}).get('id')!='DUR-01': fail('activation audit no longer selects DUR-01')
 if method.get('status')!='FROZEN_PRE_CONSTRUCTION_DETERMINISTIC_NON_RENDER': fail('method status')
 if pref.get('status')!='PASS_COMPLETE_MATRIX_POST_REPAIR': fail('preflight status')
 if ev.get('result_class')!='PASS_INTEGRATED_DURABILITY': fail('evaluation result')
 if packet.get('packet_id')!='PKT-DUR-01-01' or packet.get('status')!='FROZEN_LOCAL_RESULT': fail('packet state')
 if decision.get('selected_architecture',{}).get('id')!='CPS-B': fail('CPS-B decision')
 if pref.get('authority',{}).get('carrier_sha256')!=EXPECTED[CARRIER]: fail('carrier linkage')
 if pref.get('authority',{}).get('browser_probe_sha256')!=EXPECTED[PROBE]: fail('probe linkage')
 if ev.get('evaluation_basis',{}).get('preflight_sha256')!=EXPECTED[PREF]: fail('evaluation/preflight linkage')
 if ev.get('evaluation_basis',{}).get('scoring_after_clean_preflight_only') is not True: fail('scoring order')
 if ev.get('no_post_result_retuning',{}).get('carrier_or_packet_mechanisms_tuned_after_result') is not False: fail('post-result retuning')
 ca=pref.get('cross_mode_assertions',{})
 for k in ['zero_unintended_overflow_all_modes','zero_images_all_modes','zero_background_images_all_modes','zero_external_resources_all_modes','exact_c0_hash_preserved','exact_o3_hash_preserved','o3_visible_wide_and_omitted_narrow','semantic_name_fallback_visible_narrow','text_200_percent_verified_1440_320','forced_colors_verified_1440_320','reduced_motion_zero_animation_dependencies','selection_witness_distinct_from_focus','app_web_c0_and_cps_b_identity_match','no_hidden_clr_or_stage_asset_dependency']:
  if ca.get(k) is not True: fail(f'preflight assertion {k}')
 if ca.get('role_count')!=12 or ca.get('exact_c0_path_count')!=3 or ca.get('exact_o3_path_count')!=19: fail('structural census')
 dims=ev.get('dimensions',{})
 if len(dims)!=12 or any(v!='PASS' for v in dims.values()): fail('evaluation dimensions')
 sp=reg.get('synthesis_policy',{})
 if sp.get('active_character_presence_status')!='DIRECTOR_SELECTED_CPS_B': fail('CPS status')
 if sp.get('active_durability_status')!='PASS_INTEGRATED_DURABILITY_CLOSED': fail('registry DUR close state')
 if sp.get('active_durability_preflight_sha256')!=EXPECTED[PREF] or sp.get('active_durability_evaluation_sha256')!=EXPECTED[EVAL] or sp.get('active_durability_packet_sha256')!=EXPECTED[PACK]: fail('registry DUR hashes')
 rows=[x for x in reg.get('packets',[]) if x.get('packet_id')=='PKT-DUR-01-01']
 if len(rows)!=1 or rows[0].get('status')!='FROZEN_LOCAL_RESULT': fail('registry packet row')
 cur=CURRENT.read_text(encoding='utf-8'); led=LEDGER.read_text(encoding='utf-8'); hand=HANDOFF.read_text(encoding='utf-8')
 for token in ['DUR-01 — Integrated Identity Durability','CLOSED / PASS_INTEGRATED_DURABILITY']:
  if token not in cur: fail(f'CURRENT_STATE missing {token}')
 if len(CURRENT.read_bytes())>3072: fail('CURRENT_STATE exceeds 3 KiB cap')
 for token in ['## L-162 - DUR-01 integrated identity durability closes PASS',EXPECTED[PREF],EXPECTED[EVAL],EXPECTED[PACK],EXPECTED[CARRIER],EXPECTED[PROBE]]:
  if token not in led: fail(f'ledger missing {token}')
 for token in ['DUR-01 CLOSED PASS','PASS_INTEGRATED_DURABILITY',EXPECTED[PREF],EXPECTED[EVAL],EXPECTED[PACK],'post-DUR reentry audit']:
  if token not in hand: fail(f'handoff missing {token}')
 for p in [AUDIT,METHOD,PREF,EVAL,PACK,DECISION,REGISTRY,CURRENT,LEDGER,HANDOFF,CARRIER,PROBE]:
  txt=p.read_text(encoding='utf-8')
  bad=[ord(c) for c in txt if ord(c)<32 and c not in '\n\r\t']
  if bad: fail(f'control characters in {p.relative_to(ROOT)}')
 print('DUR_01_CHECKPOINT=PASS')
 print('RESULT=PASS_INTEGRATED_DURABILITY MATRIX=9_MODES DIMENSIONS=12_PASS')
 print('CPS_B=CANONICAL CLR=DEFERRED STAGE=UNCHANGED PHASE_D=NOT_AUTHORIZED')
 return 0
if __name__=='__main__':
 try: sys.exit(main())
 except Exception as e:
  print(f'DUR_01_FAIL: {e}',file=sys.stderr); sys.exit(1)
