import { spawn } from 'node:child_process';
import fs from 'node:fs';
import path from 'node:path';
import { pathToFileURL } from 'node:url';
const [mode,widthArg,diagPath,profile,screenshotPath] = process.argv.slice(2);
const carrierUrl=pathToFileURL(path.resolve('prototypes/appicon-01/carrier.html')).href;
const width=Number(widthArg),height=9000,browser=process.env.BROWSER;
if(!browser) throw new Error('BROWSER not set');
fs.rmSync(profile,{recursive:true,force:true});fs.mkdirSync(profile,{recursive:true});
const args=['--headless=new','--no-sandbox','--disable-gpu','--disable-dev-shm-usage','--remote-debugging-port=0',`--user-data-dir=${profile}`,'about:blank'];
if(mode==='forced') args.splice(args.length-1,0,'--force-high-contrast');
const cp=spawn(browser,args,{stdio:['ignore','ignore','inherit']});
const sleep=ms=>new Promise(r=>setTimeout(r,ms));
async function port(){const active=profile+'/DevToolsActivePort';for(let i=0;i<400;i++){if(fs.existsSync(active)){try{const [p]=fs.readFileSync(active,'utf8').trim().split(/\r?\n/);if(p)return p}catch(e){if(e?.code!=='EBUSY'&&e?.code!=='EACCES')throw e}}await sleep(100)}throw new Error('DevToolsActivePort unavailable; exit='+cp.exitCode)}
try{
 const p=await port();let target=null;
 for(let i=0;i<100;i++){try{const r=await fetch(`http://127.0.0.1:${p}/json/new?about%3Ablank`,{method:'PUT'});if(r.ok){target=await r.json();break}}catch{}await sleep(100)}
 if(!target?.webSocketDebuggerUrl) throw new Error('page target unavailable');
 const ws=new WebSocket(target.webSocketDebuggerUrl);await new Promise((res,rej)=>{ws.onopen=res;ws.onerror=rej});
 let id=0;const pending=new Map();
 ws.onmessage=e=>{const m=JSON.parse(e.data);if(m.id&&pending.has(m.id)){const q=pending.get(m.id);pending.delete(m.id);m.error?q.rej(new Error(JSON.stringify(m.error))):q.res(m.result)}};
 const send=(method,params={})=>new Promise((res,rej)=>{const n=++id;pending.set(n,{res,rej});ws.send(JSON.stringify({id:n,method,params}))});
 await send('Page.enable');await send('Runtime.enable');await send('Emulation.setDeviceMetricsOverride',{width,height,deviceScaleFactor:1,mobile:false,screenWidth:width,screenHeight:height});
 const nav=await send('Page.navigate',{url:carrierUrl});if(nav?.errorText)throw new Error('navigate: '+nav.errorText);
 for(let i=0;i<100;i++){const q=await send('Runtime.evaluate',{expression:'document.readyState',returnByValue:true});if(q?.result?.value==='complete')break;await sleep(50)}
 await sleep(150);
 const expr=`(()=>{const d=window.__APPICON01_DIAGNOSTICS__();const all=[...document.querySelectorAll('*')];
 const overflowElements=all.map((e,i)=>{const r=e.getBoundingClientRect();return {i,tag:e.tagName,role:e.closest('.role')?.dataset.role||'',cls:typeof e.className==='string'?e.className:'',text:(e.textContent||'').trim().replace(/\\s+/g,' ').slice(0,100),left:r.left,right:r.right,scrollWidth:e.scrollWidth,clientWidth:e.clientWidth}}).filter(x=>x.right>innerWidth+.5||x.left<-.5||x.scrollWidth>x.clientWidth+1).slice(0,80);
 const micro={};for(const cid of ['E06','E12','E18'])micro[cid]=d.records.filter(r=>r.candidate===cid&&[16,20,24].includes(r.size)).map(r=>({size:r.size,visual:r.visual,visiblePaths:r.visiblePaths,allPathsInside:r.allPathsInside}));
 const hostColors=[...document.querySelectorAll('[data-role="MONOCHROME_LIGHT_HOST"] .fixture,[data-role="MONOCHROME_DARK_HOST"] .fixture,[data-role="FORCED_COLORS"] .fixture')].map(x=>{const s=getComputedStyle(x),svg=x.querySelector('svg.icon'),ss=getComputedStyle(svg);return {role:x.closest('.role')?.dataset.role||'',candidate:x.dataset.candidateFixture,color:s.color,background:s.backgroundColor,svgColor:ss.color}});
 return {...d,mode:${JSON.stringify(mode)},viewport:{width:innerWidth,height:innerHeight,scrollWidth:document.documentElement.scrollWidth,scrollHeight:document.documentElement.scrollHeight},overflowElements,micro,hostColors};})()`;
 const ev=await send('Runtime.evaluate',{expression:expr,returnByValue:true});const diag=ev?.result?.value;if(!diag)throw new Error('APPICON-01 diagnostic missing');
 fs.writeFileSync(diagPath,JSON.stringify(diag)+'\n');
 if(screenshotPath){const layout=await send('Page.getLayoutMetrics');const cs=layout.cssContentSize||layout.contentSize;const shot=await send('Page.captureScreenshot',{format:'png',captureBeyondViewport:true,clip:{x:0,y:0,width:Math.ceil(cs.width),height:Math.ceil(cs.height),scale:1}});fs.writeFileSync(screenshotPath,Buffer.from(shot.data,'base64'))}
 await send('Browser.close').catch(()=>{});ws.close();
}finally{if(cp.exitCode===null)cp.kill('SIGTERM');await Promise.race([new Promise(r=>cp.once('exit',r)),sleep(1500)])}
