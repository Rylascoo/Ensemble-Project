namespace Ensemble.E0.Core.Domain;

public readonly record struct FixtureFamilyId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("FixtureFamilyId is uninitialized.");
    private FixtureFamilyId(string value) => _value = value;

    public static FixtureFamilyId From(string value)
    {
        var canonical = CanonicalId.Validate(value, nameof(value));
        if (canonical.Contains('@', StringComparison.Ordinal))
        {
            throw new ArgumentException("Fixture family IDs may not contain '@'.", nameof(value));
        }

        return new FixtureFamilyId(canonical);
    }

    public override string ToString() => Value;
}

public readonly record struct FixtureId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("FixtureId is uninitialized.");
    private FixtureId(string value) => _value = value;
    public static FixtureId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct SceneId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("SceneId is uninitialized.");
    private SceneId(string value) => _value = value;
    public static SceneId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct CharacterId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("CharacterId is uninitialized.");
    private CharacterId(string value) => _value = value;
    public static CharacterId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct RecordId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("RecordId is uninitialized.");
    private RecordId(string value) => _value = value;
    public static RecordId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct RunId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("RunId is uninitialized.");
    private RunId(string value) => _value = value;
    public static RunId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct TakeId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("TakeId is uninitialized.");
    private TakeId(string value) => _value = value;
    public static TakeId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct CommitId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("CommitId is uninitialized.");
    private CommitId(string value) => _value = value;
    public static CommitId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}

public readonly record struct ContextPacketId
{
    private readonly string? _value;
    public string Value => _value ?? throw new InvalidOperationException("ContextPacketId is uninitialized.");
    private ContextPacketId(string value) => _value = value;
    public static ContextPacketId From(string value) => new(CanonicalId.Validate(value, nameof(value)));
    public override string ToString() => Value;
}
