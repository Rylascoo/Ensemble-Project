namespace Ensemble.E0.Harness.Run;

internal interface IE0AGeminiRateRouter
{
    Task BeforeRequestAsync(
        E0ARoleProfile profile,
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken);
}

internal sealed class E0ASingleGeminiRateRouter : IE0AGeminiRateRouter
{
    private readonly IE0AGeminiRateDiscipline _discipline;

    internal E0ASingleGeminiRateRouter(IE0AGeminiRateDiscipline discipline) =>
        _discipline = discipline ?? throw new ArgumentNullException(nameof(discipline));

    public Task BeforeRequestAsync(
        E0ARoleProfile profile,
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);
        profile.Validate();
        return _discipline.BeforeRequestAsync(kind, exactGenerationInputTokens, cancellationToken);
    }
}

internal sealed class E0ANoopGeminiRateRouter : IE0AGeminiRateRouter
{
    internal static E0ANoopGeminiRateRouter Instance { get; } = new();

    private E0ANoopGeminiRateRouter() { }

    public Task BeforeRequestAsync(
        E0ARoleProfile profile,
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(profile);
        profile.Validate();
        if (!Enum.IsDefined(kind))
        {
            throw new E0AHarnessException("E0-A Gemini request kind is invalid.");
        }
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}

internal sealed class E0BMixedGeminiRateRouter : IE0AGeminiRateRouter, IDisposable
{
    private readonly E0BMixedCastConfiguration _configuration;
    private readonly Dictionary<string, E0ASmoothGeminiRateDiscipline> _routeDisciplines;
    private readonly E0ASmoothGeminiRateDiscipline _aggregateDiscipline;
    private readonly Dictionary<string, int> _requestsByProfile = new(StringComparer.Ordinal);
    private bool _disposed;

    internal E0BMixedGeminiRateRouter(
        E0BMixedCastConfiguration configuration,
        IE0AGeminiRateClock? clock = null)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        var routes = configuration.Routes;
        if (routes.Count == 0)
        {
            throw new E0AHarnessException("E0-B rate router requires at least one approved route.");
        }

        _routeDisciplines = routes.ToDictionary(
            x => x.ProfileId,
            x => new E0ASmoothGeminiRateDiscipline(x, clock),
            StringComparer.Ordinal);
        foreach (var route in routes)
        {
            _requestsByProfile.Add(route.ProfileId, 0);
        }

        var aggregate = routes[0] with
        {
            ProfileId = "E0B-SHARED-PROJECT-AGGREGATE",
            Model = "e0b-shared-project-aggregate",
            RequestsPerMinute = routes.Min(x => x.RequestsPerMinute),
            InputTokensPerMinute = routes.Min(x => x.InputTokensPerMinute),
            RequestsPerDay = routes.Min(x => x.RequestsPerDay)
        };
        _aggregateDiscipline = new E0ASmoothGeminiRateDiscipline(aggregate, clock);
    }

    public async Task BeforeRequestAsync(
        E0ARoleProfile profile,
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(E0BMixedGeminiRateRouter));
        }
        var model = _configuration.ModelFor(profile);
        if (!_routeDisciplines.TryGetValue(model.ProfileId, out var route))
        {
            throw new E0AHarnessException("E0-B rate router received an unapproved route.");
        }
        if (_requestsByProfile[model.ProfileId] >= model.RequestsPerDay)
        {
            throw new E0AHarnessException("E0-B route exhausted its admitted RPD envelope.");
        }

        await route.BeforeRequestAsync(kind, exactGenerationInputTokens, cancellationToken).ConfigureAwait(false);
        await _aggregateDiscipline.BeforeRequestAsync(kind, exactGenerationInputTokens, cancellationToken).ConfigureAwait(false);
        _requestsByProfile[model.ProfileId] = checked(_requestsByProfile[model.ProfileId] + 1);
    }

    internal int RequestsFor(string profileId) =>
        _requestsByProfile.TryGetValue(profileId, out var count) ? count : 0;

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        foreach (var discipline in _routeDisciplines.Values)
        {
            discipline.Dispose();
        }
        _aggregateDiscipline.Dispose();
        _disposed = true;
    }
}
