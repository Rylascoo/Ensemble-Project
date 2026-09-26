using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using Kymaean.Application;

namespace Kymaean.Windows.Presentation;

public enum PerformanceOutcome { KnownNoncommit, KnownSuccess, OutcomeUnknown, ProductIncompatible, ExecutorFailure }
public enum PerformanceAdmission { Accepted, Unavailable, Busy, StaleTarget, ReopenRequired }

public sealed record PerformanceTarget
{
    internal PerformanceTarget(Guid session, long generation, ProductionId productionId,
        ProductionHistoryRevision revision, SceneId sceneId, CharacterId characterId,
        string productionName, string sceneCode, string characterName, string? characterCode)
    {
        Session = session; Generation = generation; ProductionId = productionId; Revision = revision;
        SceneId = sceneId; CharacterId = characterId; ProductionName = productionName;
        SceneCode = sceneCode; CharacterName = characterName; CharacterCode = characterCode;
    }
    public Guid Session { get; }
    public long Generation { get; }
    public ProductionId ProductionId { get; }
    public ProductionHistoryRevision Revision { get; }
    public SceneId SceneId { get; }
    public CharacterId CharacterId { get; }
    public string ProductionName { get; }
    public string SceneCode { get; }
    public string CharacterName { get; }
    public string? CharacterCode { get; }
}

public sealed record PerformanceRequest(Guid Id, PerformanceTarget Target);
public sealed record PerformanceTerminal(
    PerformanceRequest Request, PerformanceOutcome Outcome, PerformanceExecution? Execution,
    ProductApplicationProjection? Projection, bool RequiresReopen);
public sealed record PerformancePublication(PerformanceTerminal Terminal, bool Stale, bool RenderingFailed);
public sealed record PerformanceActivation(
    PerformanceAdmission Admission, PerformanceRequest? Request, Task<PerformanceTerminal>? Completion);

/// <summary>
/// Session owner of one mutable Application. Only this class invokes Product after startup.
/// Performance reserves ownership before dispatch; the worker never needs the UI dispatcher.
/// Application semantics and persistence remain synchronous and unchanged.
/// </summary>
public sealed class ProductOperationCoordinator
{
    private static readonly ConditionalWeakTable<ProductApplication, ProductOperationCoordinator> Owners = new();
    private readonly ProductApplication _application;
    private readonly object _gate = new();
    private readonly Guid _session = Guid.NewGuid();
    private readonly IProductPerformer? _performer;
    private readonly IProductConsequenceInterpreter? _interpreter;
    private readonly Action<Action> _schedule;
    private readonly HashSet<ProductionId> _reopenRequired = [];
    private ImmutableArray<PerformanceTerminal> _terminals = [];
    private ProductApplicationProjection _snapshot;
    private PerformanceRequest? _pending;
    private PerformanceTerminal? _pendingTerminal;
    private PerformancePublication? _lastPublication;
    private long _generation;
    private bool _busy;
    private bool _publishing;

    private ProductOperationCoordinator(ProductApplication application,
        IProductPerformer? performer = null, IProductConsequenceInterpreter? interpreter = null,
        Action<Action>? schedule = null)
    {
        _application = application; _snapshot = application.Query();
        _performer = performer; _interpreter = interpreter;
        _schedule = schedule ?? (work => { _ = Task.Run(work); });
    }

    // The only public composition is unavailable. There is no admission flag, discovery,
    // fallback executor or provider selection. A real composition needs separate admission.
    public static ProductOperationCoordinator Own(ProductApplication application)
    {
        lock (Owners) return Owners.GetValue(application, app => new ProductOperationCoordinator(app));
    }

    internal static ProductOperationCoordinator ForTests(ProductApplication application,
        IProductPerformer? performer, IProductConsequenceInterpreter? interpreter, Action<Action>? schedule = null)
    {
        lock (Owners)
        {
            if (Owners.TryGetValue(application, out _)) throw new InvalidOperationException("Application already owned.");
            var owner = new ProductOperationCoordinator(application, performer, interpreter, schedule);
            Owners.Add(application, owner);
            return owner;
        }
    }

    internal event Action? StateChanged;
    internal ProductApplicationProjection AttachPresentation()
    {
        lock (_gate) { _generation++; return _snapshot; }
    }
    private void NotifyStateChanged()
    {
        if (StateChanged is not { } handlers) return;
        foreach (Action handler in handlers.GetInvocationList())
        {
            try { handler(); }
            catch (Exception) { /* Observers cannot abandon the reserved operation. */ }
        }
    }

    public bool IsBusy => Volatile.Read(ref _busy);
    public bool HasExecutors => _performer is not null && _interpreter is not null;
    public ImmutableArray<PerformanceTerminal> Terminals { get { lock (_gate) return _terminals; } }
    public PerformancePublication? LastPublication { get { lock (_gate) return _lastPublication; } }
    public bool RequiresReopen(ProductionId id) { lock (_gate) return _reopenRequired.Contains(id); }

    private T Access<T>(Func<ProductApplication, T> action, bool mutation = false)
    {
        if (!Monitor.TryEnter(_gate)) throw new InvalidOperationException("Application operation in progress.");
        try
        {
            if (_busy) throw new InvalidOperationException("Application operation in progress.");
            _busy = true;
            try
            {
                if (mutation && _snapshot.CurrentProduction is { } production && _reopenRequired.Contains(production.Id))
                    throw new InvalidOperationException("Open this Production before making further changes.");
                var value = action(_application);
                _snapshot = _application.Query();
                _generation++;
                return value;
            }
            finally { _busy = false; }
        }
        finally { Monitor.Exit(_gate); }
    }

    public ProductApplicationProjection Query() => Access(app => app.Query());
    public ProductApplicationProjection Navigate(ApplicationScope scope) => Access(app => app.Navigate(scope));
    public ProductApplicationProjection NavigateProduction(ProductSpace space) => Access(app => app.NavigateProduction(space));
    public ProductAccessResult<ProductionCreation> CreateProduction(string name) => Access(app => app.CreateProduction(name));
    public ProductAccessResult<CharacterCreation> CreateCharacter(string name) => Access(app => app.CreateCharacter(name), true);
    public ProductAccessResult<SceneCreation> EstablishScene(IEnumerable<CharacterId> ids) => Access(app => app.EstablishScene(ids), true);
    public ProductAccessResult<ProductApplicationProjection> ReplaceWorldCurrentState(WorldCurrentState state) =>
        Access(app => app.ReplaceWorldCurrentState(state), true);
    public ProductAccessResult<ProductApplicationProjection> RecoverProduction(ProductionId id) => Access(app => app.RecoverProduction(id));
    public ProductAccessResult<ProductApplicationProjection> OpenProduction(ProductionId id) => Access(app =>
    {
        var result = app.OpenProduction(id);
        if (result.IsSuccess) _reopenRequired.Remove(id);
        // Earlier unknown request evidence is deliberately retained for the whole session.
        return result;
    });

    // A view/session replacement can invalidate publication without accessing Product or
    // releasing its lease. Ordinary in-app navigation is blocked while busy.
    internal void InvalidatePresentation() { lock (_gate) _generation++; }

    public PerformanceTarget? CaptureTarget(SceneId sceneId, CharacterId characterId)
    {
        lock (_gate)
        {
            if (_busy || _snapshot.CurrentProduction is not { } production ||
                _snapshot.CurrentProductionReplay is not { } replay) return null;
            var cast = CharacterPresentationRow.Build(replay.ProductionCast.Characters);
            var scene = ScenePresentationRow.Build(replay, cast).SingleOrDefault(row => row.Id == sceneId);
            var actor = scene?.Roster.SingleOrDefault(row => row.Id == characterId);
            if (scene?.Code is null || actor is null) return null;
            return new(_session, _generation, production.Id, replay.HistoryRevision,
                sceneId, characterId, production.ProductionName, scene.Code, actor.CharacterName, actor.CharacterCode);
        }
    }

    internal PerformanceActivation Begin(PerformanceTarget target)
    {
        PerformanceRequest request;
        var completion = new TaskCompletionSource<PerformanceTerminal>(TaskCreationOptions.RunContinuationsAsynchronously);
        lock (_gate)
        {
            if (_busy) return new(PerformanceAdmission.Busy, null, null);
            if (!HasExecutors) return new(PerformanceAdmission.Unavailable, null, null);
            if (target.Session != _session || target.Generation != _generation ||
                target.ProductionId != _snapshot.CurrentProduction?.Id ||
                target.Revision != _snapshot.CurrentProductionReplay?.HistoryRevision)
                return new(PerformanceAdmission.StaleTarget, null, null);
            if (_reopenRequired.Contains(target.ProductionId)) return new(PerformanceAdmission.ReopenRequired, null, null);
            request = new(Guid.NewGuid(), target);
            _pending = request; _pendingTerminal = null; _busy = true;
        }

        NotifyStateChanged();
        var started = 0;
        void Execute()
        {
            if (Interlocked.Exchange(ref started, 1) != 0) return;
            CaptureTerminal(request, Invoke(request), completion);
        }
        try { _schedule(Execute); }
        catch (Exception)
        {
            // Dispatch may have accepted work before throwing. Only win-before-start
            // proves non-invocation; never release an invocation already in flight.
            if (Interlocked.CompareExchange(ref started, 1, 0) == 0)
                CaptureTerminal(request, new(request, PerformanceOutcome.KnownNoncommit, null, null, false), completion);
        }
        return new(PerformanceAdmission.Accepted, request, completion.Task);
    }

    private PerformanceTerminal Invoke(PerformanceRequest request)
    {
        var executorFailed = false;
        try
        {
            var performer = new ObservedPerformer(context =>
            {
                try { return _performer!.Perform(context) ?? throw new InvalidOperationException("Missing candidate."); }
                catch { executorFailed = true; throw; }
            });
            var interpreter = new ObservedInterpreter((context, candidate) =>
            {
                try { return _interpreter!.Interpret(context, candidate) ?? throw new InvalidOperationException("Missing consequence."); }
                catch { executorFailed = true; throw; }
            });
            var result = _application.Perform(new(request.Target.SceneId, request.Target.CharacterId), performer, interpreter);
            if (result.IsSuccess)
                return new(request, PerformanceOutcome.KnownSuccess, result.Value, _application.Query(), false);
            return result.FailureKind == ProductAccessFailureKind.Incompatible
                ? new(request, PerformanceOutcome.ProductIncompatible, null, null, true)
                : new(request, PerformanceOutcome.OutcomeUnknown, null, null, true);
        }
        catch (Exception error)
        {
            // An observed port failure cannot reach commit. I/O/access still receives
            // the adopted conservative uncertainty presentation and reopen block.
            var conservativeIo = error is IOException or UnauthorizedAccessException;
            return new(request, executorFailed ? PerformanceOutcome.ExecutorFailure : PerformanceOutcome.OutcomeUnknown,
                null, null, conservativeIo || !executorFailed);
        }
    }

    private void CaptureTerminal(PerformanceRequest request, PerformanceTerminal terminal,
        TaskCompletionSource<PerformanceTerminal> completion)
    {
        lock (_gate)
        {
            if (!ReferenceEquals(_pending, request) || _pendingTerminal is not null) return;
            _pendingTerminal = terminal;
            _terminals = _terminals.Add(terminal); // Before dispatch, formatting, binding or focus.
            if (terminal.RequiresReopen) _reopenRequired.Add(request.Target.ProductionId);
        }
        completion.SetResult(terminal);
    }

    internal bool Publish(PerformanceRequest request, Action<PerformanceTerminal> render)
    {
        lock (_gate)
        {
            if (_publishing || !ReferenceEquals(request, _pending) || _pendingTerminal is not { } terminal) return false;
            _publishing = true;
            var stale = request.Target.Generation != _generation;
            _lastPublication = new(terminal, stale, false);
            if (stale) _reopenRequired.Add(request.Target.ProductionId);
            else if (terminal.Projection is { } projection) _snapshot = projection;
            // Keep busy through callbacks: notification reentry cannot start another request.
            try { if (!stale) render(terminal); }
            catch (Exception) { _lastPublication = new(terminal, stale, true); }
            finally { _pending = null; _pendingTerminal = null; _generation++; _busy = false; _publishing = false; }
            NotifyStateChanged();
            return !stale;
        }
    }

    private sealed class ObservedPerformer(Func<CharacterPerformanceContext, PerformanceCandidate> call) : IProductPerformer
    { public PerformanceCandidate Perform(CharacterPerformanceContext context) => call(context); }
    private sealed class ObservedInterpreter(Func<CharacterPerformanceContext, PerformanceCandidate, CharacterCircumstanceProposal> call) : IProductConsequenceInterpreter
    { public CharacterCircumstanceProposal Interpret(CharacterPerformanceContext context, PerformanceCandidate performance) => call(context, performance); }
}
