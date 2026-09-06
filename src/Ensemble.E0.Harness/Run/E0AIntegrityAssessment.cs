using System.Collections.Immutable;
using System.Text.Json;
using Ensemble.E0.Core.Access;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Integrity;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Production;

namespace Ensemble.E0.Harness.Run;

internal sealed record E0AIntegrityConstraint(
    RecordId RecordId,
    string Text,
    AccessReason? DenialReason,
    ProductionRecordProtection Protection);

internal sealed class E0AIntegrityAssessmentPacket
{
    internal E0AIntegrityAssessmentPacket(
        ContextPacket context,
        CandidatePerformance candidate,
        ImmutableArray<E0AIntegrityConstraint> constraints)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Candidate = candidate ?? throw new ArgumentNullException(nameof(candidate));
        Constraints = constraints;
        if (constraints.IsDefault)
        {
            throw new E0AHarnessException("E0-A Integrity constraints are invalid.");
        }

        PacketHash = PreparedRoleAttempt.LowerSha256(JsonSerializer.SerializeToUtf8Bytes(ToTransport()));
    }

    internal ContextPacket Context { get; }
    internal CandidatePerformance Candidate { get; }
    internal ImmutableArray<E0AIntegrityConstraint> Constraints { get; }
    internal string PacketHash { get; }

    internal object ToTransport() => new
    {
        source = new
        {
            contextPacketId = Context.ContextPacketId.Value,
            characterId = Context.SubjectCharacterId.Value,
            structuredContextHash = Context.StructuredContextHash,
            renderedContextHash = Context.RenderedContextHash
        },
        candidate = new
        {
            visibleText = Candidate.VisibleText,
            addressedCharacterIds = Candidate.Control.AddressedCharacterIds.Select(x => x.Value).ToArray(),
            nominatedCharacterId = Candidate.Control.NominatedCharacterId.HasValue
                ? Candidate.Control.NominatedCharacterId.Value.Value
                : null
        },
        characterVisibleContext = E0APromptContracts.ContextData(Context),
        authoritativeConstraints = Constraints.Select(x => new
        {
            recordId = x.RecordId.Value,
            text = x.Text,
            denialReason = x.DenialReason?.ToString(),
            protection = x.Protection.ToString()
        }).ToArray(),
        concernDefinitions = E0AIntegrityConcernParser.AllowedNames
    };
}

internal static class E0AIntegrityAssessmentPacketBuilder
{
    internal static E0AIntegrityAssessmentPacket Build(
        ProductionState state,
        CharacterAccessEvaluation access,
        ContextPacket context,
        CandidatePerformance candidate)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(access);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(candidate);

        var decisions = access.Decisions.ToDictionary(x => x.RecordId.Value, StringComparer.Ordinal);
        var constraints = new Dictionary<string, E0AIntegrityConstraint>(StringComparer.Ordinal);

        foreach (var record in state.Records)
        {
            if (record.Lifecycle != ProductionRecordLifecycle.Active)
            {
                continue;
            }

            decisions.TryGetValue(record.RecordId.Value, out var decision);
            var denied = decision?.Disposition == AccessDisposition.Deny;
            var protectedRecord = record.Protection is ProductionRecordProtection.SystemImmutable or ProductionRecordProtection.CreatorLocked;
            if (!denied && !protectedRecord)
            {
                continue;
            }

            constraints[record.RecordId.Value] = new E0AIntegrityConstraint(
                record.RecordId,
                record.Text,
                denied ? decision!.Reason : null,
                record.Protection);
        }

        var ordered = constraints.Values
            .OrderBy(x => x.RecordId.Value, StringComparer.Ordinal)
            .ToImmutableArray();
        return new E0AIntegrityAssessmentPacket(context, candidate, ordered);
    }
}

internal static class E0AIntegrityConcernParser
{
    internal static readonly string[] AllowedNames =
    {
        nameof(IntegrityConcernKind.PotentialInaccessibleInformationUse),
        nameof(IntegrityConcernKind.PotentialProtectedInformationExposure),
        nameof(IntegrityConcernKind.PotentialLockedAuthorityViolation),
        nameof(IntegrityConcernKind.PotentialTechnicalArtifactLeak),
        nameof(IntegrityConcernKind.IndeterminateSemanticIntegrity)
    };

    internal static ImmutableArray<IntegrityConcernKind> Parse(ReadOnlySpan<byte> utf8)
    {
        try
        {
            using var document = JsonDocument.Parse(utf8.ToArray());
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object || root.EnumerateObject().Count() != 1 ||
                !root.TryGetProperty("concerns", out var concerns) || concerns.ValueKind != JsonValueKind.Array)
            {
                throw new E0AHarnessException("E0-A Integrity response shape is invalid.");
            }

            var result = ImmutableArray.CreateBuilder<IntegrityConcernKind>();
            foreach (var item in concerns.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.String ||
                    !Enum.TryParse<IntegrityConcernKind>(item.GetString(), false, out var parsed) ||
                    !Enum.IsDefined(parsed))
                {
                    throw new E0AHarnessException("E0-A Integrity response contains an unsupported concern.");
                }
                result.Add(parsed);
            }
            return result.ToImmutable();
        }
        catch (JsonException)
        {
            throw new E0AHarnessException("E0-A Integrity response JSON is invalid.");
        }
    }
}
