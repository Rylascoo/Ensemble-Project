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
  for(let i=0;i<400;i++){if(fs.existsSync(active)){const [p]=fs.readFileSync(active,'utf8').trim().split(/\r?\n/);if(p)return p;}await sleep(100)}
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
  const nav=await send('Page.navigate',{url:'http://127.0.0.1:8765/prototypes/cps-01/carrier.html'});
  if(nav?.errorText) throw new Error('navigate: '+nav.errorText);
  for(let i=0;i<100;i++){const q=await send('Runtime.evaluate',{expression:'document.readyState',returnByValue:true});if(q?.result?.value==='complete')break;await sleep(50)}
  if(mode==='text200') await send('Runtime.evaluate',{expression:"document.documentElement.dataset.textScale='200'",returnByValue:true});
  await sleep(120);
  const expr=`(()=>{
    const d=window.__CPS01_DIAGNOSTICS__();
    const all=[...document.querySelectorAll('*')];
    const overflowElements=all.filter(e=>!e.closest('.crop-window')).map((e,i)=>{const r=e.getBoundingClientRect();return {i,tag:e.tagName,id:e.id||'',cls:typeof e.className==='string'?e.className:'',text:(e.textContent||'').trim().replace(/\\s+/g,' ').slice(0,100),left:r.left,right:r.right,scrollWidth:e.scrollWidth,clientWidth:e.clientWidth}}).filter(x=>x.right>innerWidth+.5||x.left<-.5||x.scrollWidth>x.clientWidth+1).slice(0,60);
    const crop=[...document.querySelectorAll('.crop-window')].map(w=>{const wr=w.getBoundingClientRect(),id=w.querySelector('.id'),ir=id.getBoundingClientRect(),segs=[...id.querySelectorAll('.sig')].map(s=>{const r=s.getBoundingClientRect();const iw=Math.max(0,Math.min(r.right,wr.right)-Math.max(r.left,wr.left)),ih=Math.max(0,Math.min(r.bottom,wr.bottom)-Math.max(r.top,wr.top));return iw*ih});return {candidate:w.closest('.candidate').dataset.candidate,character:w.dataset.cropCharacter,identityVisibleArea:Math.max(0,Math.min(ir.right,wr.right)-Math.max(ir.left,wr.left))*Math.max(0,Math.min(ir.bottom,wr.bottom)-Math.max(ir.top,wr.top)),visibleSegments:segs.filter(a=>a>.5).length}});
    const translations=[...document.querySelectorAll('.candidate[data-role="APP_WEB_TRANSLATION"]')].map(c=>{const app=c.querySelector('.app .id'),web=c.querySelector('.web .id');return {candidate:c.dataset.candidate,appSignature:app?.dataset.signature||null,webSignature:web?.dataset.signature||null,appIdentity:app?.dataset.identity||null,webIdentity:web?.dataset.identity||null}});
    const facts={}; for(const cid of ['CPS-A','CPS-B','CPS-C']){facts[cid]={states:[...document.querySelectorAll('.candidate[data-candidate="'+cid+'"] .state')].map(x=>x.textContent.trim()),history:[...document.querySelectorAll('.candidate[data-candidate="'+cid+'"] .history')].map(x=>x.textContent.trim())}}
    return {...d,mode:${JSON.stringify(mode)},viewport:{width:innerWidth,height:innerHeight,scrollWidth:document.documentElement.scrollWidth,scrollHeight:document.documentElement.scrollHeight},overflowElements,crop,translations,facts,noImageDepictions:[...document.querySelectorAll('[data-role="NO_IMAGE"] .optional-depiction')].length,focusedDepictions:[...document.querySelectorAll('[data-role="FOCUSED"] .optional-depiction')].length,translationDepictions:[...document.querySelectorAll('[data-role="APP_WEB_TRANSLATION"] .web .optional-depiction')].length};
  })()`;
  const ev=await send('Runtime.evaluate',{expression:expr,returnByValue:true});
  const diag=ev?.result?.value; if(!diag) throw new Error('CPS-01 diagnostic missing');
  fs.writeFileSync(diagPath,JSON.stringify(diag)+'\n');
  if(screenshotPath){
    const layout=await send('Page.getLayoutMetrics'); const cs=layout.cssContentSize||layout.contentSize;
    const shot=await send('Page.captureScreenshot',{format:'png',captureBeyondViewport:true,clip:{x:0,y:0,width:Math.ceil(cs.width),height:Math.ceil(cs.height),scale:1}});
    fs.writeFileSync(screenshotPath,Buffer.from(shot.data,'base64'));
  }
  await send('Browser.close').catch(()=>{});
  ws.close();
}finally{
  if(cp.exitCode===null) cp.kill('SIGTERM');
  await Promise.race([new Promise(r=>cp.once('exit',r)),sleep(1500)]);
}
