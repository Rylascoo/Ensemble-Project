import { spawn } from 'node:child_process';
import fs from 'node:fs';
const [mode,widthArg,diagPath,profile,screenshotPath] = process.argv.slice(2);
const width=Number(widthArg), height=12000, browser=process.env.BROWSER;
if(!browser) throw new Error('BROWSER not set');
fs.rmSync(profile,{recursive:true,force:true}); fs.mkdirSync(profile,{recursive:true});
const args=['--headless=new','--no-sandbox','--disable-gpu','--disable-dev-shm-usage','--remote-debugging-port=0',`--user-data-dir=${profile}`,'about:blank'];
if(mode==='forced') args.splice(args.length-1,0,'--force-high-contrast');
if(mode==='reduced') args.splice(args.length-1,0,'--force-prefers-reduced-motion');
const cp=spawn(browser,args,{stdio:['ignore','ignore','inherit']});
const sleep=ms=>new Promise(r=>setTimeout(r,ms));
async function port(){
 const active=profile+'/DevToolsActivePort';
 for(let i=0;i<400;i++){if(fs.existsSync(active)){try{const [p]=fs.readFileSync(active,'utf8').trim().split(/\r?\n/);if(p)return p;}catch(e){if(e?.code!=='EBUSY'&&e?.code!=='EACCES')throw e;}}await sleep(100)}
 throw new Error('DevToolsActivePort unavailable; launcher exit='+cp.exitCode);
}
try{
 const p=await port(); let target=null;
 for(let i=0;i<100;i++){try{const r=await fetch(`http://127.0.0.1:${p}/json/new?about%3Ablank`,{method:'PUT'});if(r.ok){target=await r.json();break}}catch{}await sleep(100)}
 if(!target?.webSocketDebuggerUrl) throw new Error('page target unavailable');
 const ws=new WebSocket(target.webSocketDebuggerUrl); await new Promise((res,rej)=>{ws.onopen=res;ws.onerror=rej});
 let id=0; const pending=new Map();
 ws.onmessage=e=>{const m=JSON.parse(e.data);if(m.id&&pending.has(m.id)){const q=pending.get(m.id);pending.delete(m.id);m.error?q.rej(new Error(JSON.stringify(m.error))):q.res(m.result)}};
 const send=(method,params={})=>new Promise((res,rej)=>{const n=++id;pending.set(n,{res,rej});ws.send(JSON.stringify({id:n,method,params}))});
 await send('Page.enable'); await send('Runtime.enable');
 await send('Emulation.setDeviceMetricsOverride',{width,height,deviceScaleFactor:1,mobile:false,screenWidth:width,screenHeight:height});
 const nav=await send('Page.navigate',{url:'http://127.0.0.1:8765/prototypes/dur-01/carrier.html'}); if(nav?.errorText) throw new Error('navigate: '+nav.errorText);
 for(let i=0;i<100;i++){const q=await send('Runtime.evaluate',{expression:'document.readyState',returnByValue:true});if(q?.result?.value==='complete')break;await sleep(50)}
 if(mode==='text200') await send('Runtime.evaluate',{expression:"document.documentElement.dataset.textScale='200'",returnByValue:true});
 if(mode==='noimage') await send('Runtime.evaluate',{expression:"document.documentElement.dataset.noImage='1'",returnByValue:true});
 await sleep(150);
 const expr=`(()=>{
  const d=window.__DUR01_DIAGNOSTICS__(); const all=[...document.querySelectorAll('*')];
  const overflowElements=all.map((e,i)=>{const r=e.getBoundingClientRect();return {i,tag:e.tagName,role:e.closest('.role')?.dataset.role||'',cls:typeof e.className==='string'?e.className:'',text:(e.textContent||'').trim().replace(/\\s+/g,' ').slice(0,100),left:r.left,right:r.right,scrollWidth:e.scrollWidth,clientWidth:e.clientWidth}}).filter(x=>x.right>innerWidth+.5||x.left<-.5||x.scrollWidth>x.clientWidth+1).slice(0,80);
  const microMeshes=[...document.querySelectorAll('[data-role="CHARACTER_MICRO_CPS_B"] .mesh.micro')].map(m=>{const mr=m.getBoundingClientRect();const seg=[...m.querySelectorAll('.sig')].map(s=>{const r=s.getBoundingClientRect();return {w:r.width,h:r.height,inside:r.left>=mr.left-.5&&r.right<=mr.right+.5&&r.top>=mr.top-.5&&r.bottom<=mr.bottom+.5}});return {character:m.dataset.character,signature:m.dataset.signature,w:mr.width,h:mr.height,visibleSegments:seg.filter(x=>x.w>0&&x.h>0&&x.inside).length}});
  const microMarks=[...document.querySelectorAll('[data-role="SYMBOL_MICRO_16_20_24"] [data-source="C0_THRESHOLD_K"]')].map(m=>{const r=m.getBoundingClientRect();return {w:r.width,h:r.height,paths:m.querySelectorAll('path').length,hash:m.dataset.sourceSha}});
  const app=document.querySelector('[data-role="APP_WEB_TRANSLATION"] [data-surface="APP"]'),web=document.querySelector('[data-role="APP_WEB_TRANSLATION"] [data-surface="WEB"]');
  const appMesh=app?.querySelector('[data-cps="CPS-B"]'),webMesh=web?.querySelector('[data-cps="CPS-B"]'); const appMark=app?.querySelector('[data-source="C0_THRESHOLD_K"]'),webMark=web?.querySelector('[data-source="C0_THRESHOLD_K"]');
  const selected=document.querySelector('[data-state-treatment="STA-F2"]'),focus=document.querySelector('[data-focus-fixture="keyboard"]'); const sw=selected?getComputedStyle(selected,'::after'):null,fs=focus?getComputedStyle(focus):null;
  return {...d,mode:${JSON.stringify(mode)},viewport:{width:innerWidth,height:innerHeight,scrollWidth:document.documentElement.scrollWidth,scrollHeight:document.documentElement.scrollHeight},overflowElements,microMeshes,microMarks,appWeb:{appSignature:appMesh?.dataset.signature||null,webSignature:webMesh?.dataset.signature||null,appDecision:appMesh?.dataset.cpsDecisionSha||null,webDecision:webMesh?.dataset.cpsDecisionSha||null,appC0Hash:appMark?.dataset.sourceSha||null,webC0Hash:webMark?.dataset.sourceSha||null},stateFocus:{selectionWitnessWidth:sw?.width||null,selectionWitnessDisplay:sw?.display||null,focusOutlineStyle:fs?.outlineStyle||null,focusOutlineWidth:fs?.outlineWidth||null},wordmarkFallbacksVisible:[...document.querySelectorAll('.wordmark-fallback')].filter(x=>getComputedStyle(x).display!=='none').length};
 })()`;
 const ev=await send('Runtime.evaluate',{expression:expr,returnByValue:true}); const diag=ev?.result?.value; if(!diag) throw new Error('DUR-01 diagnostic missing');
 fs.writeFileSync(diagPath,JSON.stringify(diag)+'\n');
 if(screenshotPath){const layout=await send('Page.getLayoutMetrics');const cs=layout.cssContentSize||layout.contentSize;const shot=await send('Page.captureScreenshot',{format:'png',captureBeyondViewport:true,clip:{x:0,y:0,width:Math.ceil(cs.width),height:Math.ceil(cs.height),scale:1}});fs.writeFileSync(screenshotPath,Buffer.from(shot.data,'base64'))}
 await send('Browser.close').catch(()=>{}); ws.close();
}finally{
 if(cp.exitCode===null) cp.kill('SIGTERM');
 await Promise.race([new Promise(r=>cp.once('exit',r)),sleep(1500)]);
}
