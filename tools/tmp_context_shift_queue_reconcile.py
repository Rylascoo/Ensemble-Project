from pathlib import Path

queue_path = Path('docs/PROJECT_EXECUTION_QUEUE.md')
queue = queue_path.read_text(encoding='utf-8')
lines = queue.splitlines()
indices = [i for i, line in enumerate(lines) if line.startswith('| Q-DESIGN-14 |')]
assert len(indices) == 1, indices
row14 = "| Q-DESIGN-14 | Design Sol / Director | DONE | Materialized and browser-preflighted the non-production F1A deployment-envelope exemplar set, then resolved the use law through direct Design Sol + Director adjudication. | Closed on `Rylascoo/Ensemble-Website/main` at `126a03ac462a1709bab731551ff0c93c3c96933a`; `docs/evidence/MOT_01_F1A_DEPLOYMENT_ENVELOPE_ADJUDICATION_01.json` approves A1/A2/A3 and prohibits N1/N2 without reopening F1A cadence. Director additionally requires a visibly different treatment when context itself shifts. | F1A remains provisional bounded within-context succession; successor is Q-DESIGN-15. |"
row15 = "| Q-DESIGN-15 | Design Sol / Director | ACTIVE | Define and test a visibly distinct non-sequential treatment for transitions between already-ready contexts. Freeze fixture-only timing/amplitude values; materialize CS0 Cut, CS1 Synchronized Field Settle, CS2 Balanced Crossfade Exchange, and CS3 Simultaneous Opposed-Field Exchange across APP/WEB × WIDE/320; run full/reduced-motion browser preflight; freeze Design Sol view; present to Director. | Contract on Website main: `docs/evidence/MOT_01_CONTEXT_SHIFT_DIFFERENTIATION_CONTRACT_01.json`. No F1A retune; no stagger inside context-shift candidates; one motion grammar owns one transition event; CUT/none is a valid outcome. No production tokens, app/site/navigation implementation, Stage/Character/Performance/Take/Opportunity motion, final identity or Phase-C convergence. | Direct adjudication selects at most one provisional context-shift treatment or CUT/none; any implementation/production-token work requires a separate successor gate. |"
i = indices[0]
lines[i:i+1] = [row14, row15]
queue_path.write_text('\n'.join(lines) + '\n', encoding='utf-8')

state_path = Path('CURRENT_STATE.md')
state = state_path.read_text(encoding='utf-8')
old = "Q-DESIGN-13 DONE; Q-DESIGN-14 ACTIVE for non-production F1A deployment-envelope exemplars + direct Director/Design Sol adjudication. F1A 70/460 remains provisional; E0 blind scoring and engineering order unchanged."
new = "Q-DESIGN-14 DONE; Q-DESIGN-15 ACTIVE for non-production context-shift motion differentiation under Website authority. F1A 70/460 remains provisional within-context succession; context shifts require a distinct non-sequential treatment. E0 blind scoring and engineering order unchanged."
assert state.count(old) == 1, state.count(old)
state_path.write_text(state.replace(old, new), encoding='utf-8')
