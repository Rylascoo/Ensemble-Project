using System.Diagnostics;

namespace Ensemble.E0.Harness.Run;

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
    private static readonly TimeSpan TokenWindow = TimeSpan.FromMinutes(1);

    private readonly E0AGeminiModelProfile _profile;
    private readonly IE0AGeminiRateClock _clock;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private readonly TimeSpan _requestSpacing;
    private readonly Queue<TokenWindowEntry> _generationTokenWindow = new();
    private long? _lastRequestTimestamp;
    private long _generationTokensInWindow;
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
                await WaitForTokenWindowAsync(exactGenerationInputTokens!.Value, cancellationToken).ConfigureAwait(false);
            }

            var now = _clock.GetTimestamp();
            _lastRequestTimestamp = now;
            if (kind == E0AGeminiRequestKind.Generation && exactGenerationInputTokens!.Value != 0)
            {
                _generationTokenWindow.Enqueue(new TokenWindowEntry(now, exactGenerationInputTokens.Value));
                _generationTokensInWindow = checked(_generationTokensInWindow + exactGenerationInputTokens.Value);
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

    private async Task WaitForTokenWindowAsync(long inputTokens, CancellationToken cancellationToken)
    {
        while (true)
        {
            var now = _clock.GetTimestamp();
            PruneExpiredTokenEntries(now);
            if (checked(_generationTokensInWindow + inputTokens) <= _profile.InputTokensPerMinute)
            {
                return;
            }

            if (_generationTokenWindow.Count == 0)
            {
                throw new E0AHarnessException("E0-A Gemini TPM accounting reached an invalid state.");
            }
            var oldest = _generationTokenWindow.Peek();
            var elapsed = _clock.GetElapsedTime(oldest.Timestamp, now);
            var remaining = TokenWindow - elapsed;
            if (remaining > TimeSpan.Zero)
            {
                await _clock.DelayAsync(remaining, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    private void PruneExpiredTokenEntries(long now)
    {
        while (_generationTokenWindow.Count != 0)
        {
            var oldest = _generationTokenWindow.Peek();
            if (_clock.GetElapsedTime(oldest.Timestamp, now) < TokenWindow)
            {
                return;
            }
            _generationTokenWindow.Dequeue();
            _generationTokensInWindow = checked(_generationTokensInWindow - oldest.InputTokens);
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

    private readonly record struct TokenWindowEntry(long Timestamp, long InputTokens);
}
