using System.Diagnostics;
using Ensemble.E0.Harness.Run;

namespace Ensemble.E0.Harness.Gemini;

internal enum E0AGeminiRequestKind
{
    CountTokens = 1,
    Generation = 2
}

internal interface IE0AGeminiRateClock
{
    long GetTimestamp();
    TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp);
    Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken);
}

internal sealed class E0ASystemGeminiRateClock : IE0AGeminiRateClock
{
    internal static E0ASystemGeminiRateClock Instance { get; } = new();

    private E0ASystemGeminiRateClock() { }

    public long GetTimestamp() => Stopwatch.GetTimestamp();

    public TimeSpan GetElapsedTime(long startingTimestamp, long endingTimestamp) =>
        Stopwatch.GetElapsedTime(startingTimestamp, endingTimestamp);

    public Task DelayAsync(TimeSpan delay, CancellationToken cancellationToken) =>
        Task.Delay(delay, cancellationToken);
}

internal interface IE0AGeminiRateDiscipline
{
    Task BeforeRequestAsync(
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken);
}

internal sealed class E0ANoopGeminiRateDiscipline : IE0AGeminiRateDiscipline
{
    internal static E0ANoopGeminiRateDiscipline Instance { get; } = new();

    private E0ANoopGeminiRateDiscipline() { }

    public Task BeforeRequestAsync(
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(kind))
        {
            throw new E0AHarnessException("E0-A Gemini request kind is invalid.");
        }
        cancellationToken.ThrowIfCancellationRequested();
        return Task.CompletedTask;
    }
}

internal sealed class E0ASmoothGeminiRateDiscipline : IE0AGeminiRateDiscipline, IDisposable
{
    private readonly E0AGeminiModelProfile _profile;
    private readonly IE0AGeminiRateClock _clock;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly TimeSpan _requestSpacing;
    private long? _lastRequestTimestamp;
    private long? _lastGenerationTimestamp;
    private TimeSpan _lastGenerationTokenSpacing;
    private bool _disposed;

    internal E0ASmoothGeminiRateDiscipline(
        E0AGeminiModelProfile profile,
        IE0AGeminiRateClock? clock = null)
    {
        _profile = profile ?? throw new ArgumentNullException(nameof(profile));
        _profile.Validate();
        _clock = clock ?? E0ASystemGeminiRateClock.Instance;
        _requestSpacing = TimeSpan.FromSeconds(60d / _profile.RequestsPerMinute);
    }

    public async Task BeforeRequestAsync(
        E0AGeminiRequestKind kind,
        long? exactGenerationInputTokens,
        CancellationToken cancellationToken)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(E0ASmoothGeminiRateDiscipline));
        }
        if (!Enum.IsDefined(kind))
        {
            throw new E0AHarnessException("E0-A Gemini request kind is invalid.");
        }
        if (kind == E0AGeminiRequestKind.Generation)
        {
            if (!exactGenerationInputTokens.HasValue || exactGenerationInputTokens.Value < 0)
            {
                throw new E0AHarnessException("E0-A Gemini generation pacing requires exact preflight input tokens.");
            }
            if (exactGenerationInputTokens.Value > _profile.InputTokensPerMinute)
            {
                throw new E0AHarnessException("E0-A Gemini generation input exceeds the selected profile TPM limit.");
            }
        }
        else if (exactGenerationInputTokens.HasValue)
        {
            throw new E0AHarnessException("E0-A Gemini countTokens pacing cannot carry generation token usage.");
        }

        await _gate.WaitAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            await WaitForRequestSpacingAsync(cancellationToken).ConfigureAwait(false);
            if (kind == E0AGeminiRequestKind.Generation)
            {
                await WaitForTokenSpacingAsync(cancellationToken).ConfigureAwait(false);
            }

            var now = _clock.GetTimestamp();
            _lastRequestTimestamp = now;
            if (kind == E0AGeminiRequestKind.Generation)
            {
                _lastGenerationTimestamp = now;
                _lastGenerationTokenSpacing = TimeSpan.FromSeconds(
                    60d * exactGenerationInputTokens!.Value / _profile.InputTokensPerMinute);
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    private async Task WaitForRequestSpacingAsync(CancellationToken cancellationToken)
    {
        if (!_lastRequestTimestamp.HasValue)
        {
            return;
        }
        var now = _clock.GetTimestamp();
        var elapsed = _clock.GetElapsedTime(_lastRequestTimestamp.Value, now);
        var remaining = _requestSpacing - elapsed;
        if (remaining > TimeSpan.Zero)
        {
            await _clock.DelayAsync(remaining, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task WaitForTokenSpacingAsync(CancellationToken cancellationToken)
    {
        if (!_lastGenerationTimestamp.HasValue || _lastGenerationTokenSpacing <= TimeSpan.Zero)
        {
            return;
        }
        var now = _clock.GetTimestamp();
        var elapsed = _clock.GetElapsedTime(_lastGenerationTimestamp.Value, now);
        var remaining = _lastGenerationTokenSpacing - elapsed;
        if (remaining > TimeSpan.Zero)
        {
            await _clock.DelayAsync(remaining, cancellationToken).ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }
        _gate.Dispose();
        _disposed = true;
    }
}
