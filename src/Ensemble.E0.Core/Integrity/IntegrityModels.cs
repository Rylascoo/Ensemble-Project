using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.Context;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Performer;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Integrity;

public static class E0IntegrityContracts
{
    public const string CandidateContentIdentityContract =
        "ensemble.e0.integrity.candidate-content.v1";
    public const string ConcernEvidenceContract =
        "ensemble.e0.integrity.concerns.v1";
    public const string ValidationContract =
        "ensemble.e0.integrity.validation.v1";
}

public enum IntegrityDeterministicRejectCode
{
    SubjectContextMismatch,
    ContextPacketIdentityMismatch
}

public enum IntegrityConcernKind
{
    PotentialInaccessibleInformationUse,
    PotentialProtectedInformationExposure,
    PotentialLockedAuthorityViolation,
    PotentialTechnicalArtifactLeak,
    IndeterminateSemanticIntegrity
}

public enum IntegrityDisposition
{
    Accept,
    Reject,
    RequestAnotherTake
}

public sealed class IntegrityCandidateInput
{
    internal IntegrityCandidateInput(
        string candidateContentIdentityContract,
        string candidateContentHash,
        ContextPacketId sourceContextPacketId,
        ImmutableArray<IntegrityDeterministicRejectCode> deterministicRejectCodes)
    {
        CandidateContentIdentityContract = candidateContentIdentityContract;
        CandidateContentHash = candidateContentHash;
        SourceContextPacketId = sourceContextPacketId;
        DeterministicRejectCodes = deterministicRejectCodes;
    }

    public string CandidateContentIdentityContract { get; }
    public string CandidateContentHash { get; }
    public ContextPacketId SourceContextPacketId { get; }
    public ImmutableArray<IntegrityDeterministicRejectCode> DeterministicRejectCodes { get; }

    public static IntegrityCandidateInput Bind(
        ContextPacket sourceContext,
        CandidatePerformance candidate)
    {
        if (sourceContext is null)
        {
            throw new IntegrityValidationException("Integrity source ContextPacket is required.");
        }

        if (candidate is null)
        {
            throw new IntegrityValidationException("Integrity CandidatePerformance is required.");
        }

        ValidateSourceContext(sourceContext);
        ValidateCandidateForSafeBinding(candidate);

        string candidateContentHash;
        try
        {
            candidateContentHash = ComputeCandidateContentHash(candidate);
        }
        catch (Exception exception) when (
            exception is CanonicalJsonException or
            EncoderFallbackException or
            InvalidOperationException)
        {
            throw new IntegrityValidationException(
                "Integrity CandidatePerformance cannot be canonicalized safely.",
                exception);
        }

        var rejectCodes = ImmutableArray.CreateBuilder<IntegrityDeterministicRejectCode>(2);

        if (candidate.SubjectCharacterId != sourceContext.SubjectCharacterId)
        {
            rejectCodes.Add(IntegrityDeterministicRejectCode.SubjectContextMismatch);
        }

        if (candidate.ContextPacketId != sourceContext.ContextPacketId)
        {
            rejectCodes.Add(IntegrityDeterministicRejectCode.ContextPacketIdentityMismatch);
        }

        return new IntegrityCandidateInput(
            E0IntegrityContracts.CandidateContentIdentityContract,
            candidateContentHash,
            sourceContext.ContextPacketId,
            rejectCodes.ToImmutable());
    }

    private static void ValidateSourceContext(ContextPacket sourceContext)
    {
        try
        {
            _ = sourceContext.SubjectCharacterId.Value;
            _ = sourceContext.OpportunityCharacterId.Value;
            _ = sourceContext.ContextPacketId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new IntegrityValidationException(
                "Integrity source ContextPacket identity is not initialized.",
                exception);
        }

        if (sourceContext.SubjectCharacterId != sourceContext.OpportunityCharacterId)
        {
            throw new IntegrityValidationException(
                "Integrity source ContextPacket subject must equal its opportunity Character.");
        }
    }

    private static void ValidateCandidateForSafeBinding(CandidatePerformance candidate)
    {
        if (!string.Equals(
                candidate.ContractVersion,
                PerformerCandidateContract.CandidateContractVersion,
                StringComparison.Ordinal))
        {
            throw new IntegrityValidationException(
                "Integrity CandidatePerformance contract version is unsupported.");
        }

        try
        {
            _ = candidate.SubjectCharacterId.Value;
            _ = candidate.ContextPacketId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new IntegrityValidationException(
                "Integrity CandidatePerformance identity is not initialized.",
                exception);
        }

        if (candidate.VisibleText is null || candidate.Control is null)
        {
            throw new IntegrityValidationException(
                "Integrity CandidatePerformance is not structurally initialized.");
        }

        if (candidate.Control.AddressedCharacterIds.IsDefault)
        {
            throw new IntegrityValidationException(
                "Integrity CandidatePerformance control is not initialized.");
        }

        foreach (var characterId in candidate.Control.AddressedCharacterIds)
        {
            try
            {
                _ = characterId.Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new IntegrityValidationException(
                    "Integrity CandidatePerformance control contains an uninitialized Character ID.",
                    exception);
            }
        }

        if (candidate.Control.NominatedCharacterId is { } nomination)
        {
            try
            {
                _ = nomination.Value;
            }
            catch (InvalidOperationException exception)
            {
                throw new IntegrityValidationException(
                    "Integrity CandidatePerformance nomination is not initialized.",
                    exception);
            }
        }
    }

    private static string ComputeCandidateContentHash(CandidatePerformance candidate)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        var firstRootProperty = true;

        AppendStringProperty(
            builder,
            ref firstRootProperty,
            "identityContract",
            E0IntegrityContracts.CandidateContentIdentityContract);
        AppendStringProperty(
            builder,
            ref firstRootProperty,
            "candidateContractVersion",
            candidate.ContractVersion);
        AppendStringProperty(
            builder,
            ref firstRootProperty,
            "subjectCharacterId",
            candidate.SubjectCharacterId.Value);
        AppendStringProperty(
            builder,
            ref firstRootProperty,
            "contextPacketId",
            candidate.ContextPacketId.Value);
        AppendStringProperty(
            builder,
            ref firstRootProperty,
            "visibleText",
            candidate.VisibleText);

        CanonicalJson.AppendSeparator(builder, ref firstRootProperty);
        CanonicalJson.AppendPropertyName(builder, "control");
        builder.Append('{');
        var firstControlProperty = true;

        CanonicalJson.AppendSeparator(builder, ref firstControlProperty);
        CanonicalJson.AppendPropertyName(builder, "addressedCharacterIds");
        builder.Append('[');
        for (var index = 0; index < candidate.Control.AddressedCharacterIds.Length; index++)
        {
            if (index != 0)
            {
                builder.Append(',');
            }

            CanonicalJson.AppendString(
                builder,
                candidate.Control.AddressedCharacterIds[index].Value);
        }

        builder.Append(']');

        CanonicalJson.AppendSeparator(builder, ref firstControlProperty);
        CanonicalJson.AppendPropertyName(builder, "nominatedCharacterId");
        if (candidate.Control.NominatedCharacterId is { } nomination)
        {
            CanonicalJson.AppendString(builder, nomination.Value);
        }
        else
        {
            builder.Append("null");
        }

        builder.Append('}');
        builder.Append('}');

        var canonicalBytes = CanonicalJson.EncodeUtf8(builder.ToString());
        return Convert.ToHexString(SHA256.HashData(canonicalBytes)).ToLowerInvariant();
    }

    private static void AppendStringProperty(
        StringBuilder builder,
        ref bool first,
        string propertyName,
        string value)
    {
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, propertyName);
        CanonicalJson.AppendString(builder, value);
    }
}

public sealed class IntegrityConcernEvidence
{
    internal IntegrityConcernEvidence(
        string evidenceContract,
        string candidateContentIdentityContract,
        string candidateContentHash,
        ImmutableArray<IntegrityConcernKind> concerns)
    {
        EvidenceContract = evidenceContract;
        CandidateContentIdentityContract = candidateContentIdentityContract;
        CandidateContentHash = candidateContentHash;
        Concerns = concerns;
    }

    public string EvidenceContract { get; }
    public string CandidateContentIdentityContract { get; }
    public string CandidateContentHash { get; }
    public ImmutableArray<IntegrityConcernKind> Concerns { get; }

    public static IntegrityConcernEvidence Bind(
        IntegrityCandidateInput input,
        ImmutableArray<IntegrityConcernKind> concerns)
    {
        IntegrityValidationInvariants.ValidateInput(input);

        if (input.DeterministicRejectCodes.Length != 0)
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence cannot bind to a deterministic Reject input.");
        }

        var canonicalConcerns = IntegrityValidationInvariants.CanonicalizeConcerns(concerns);

        return new IntegrityConcernEvidence(
            E0IntegrityContracts.ConcernEvidenceContract,
            input.CandidateContentIdentityContract,
            input.CandidateContentHash,
            canonicalConcerns);
    }
}

public sealed class IntegrityValidationTrace
{
    internal IntegrityValidationTrace(
        string validationContract,
        IntegrityCandidateInput input,
        IntegrityConcernEvidence? concernEvidence)
    {
        ValidationContract = validationContract;
        Input = input;
        ConcernEvidence = concernEvidence;
    }

    public string ValidationContract { get; }
    public IntegrityCandidateInput Input { get; }
    public IntegrityConcernEvidence? ConcernEvidence { get; }
}

public sealed class IntegrityValidationEvaluation
{
    internal IntegrityValidationEvaluation(
        IntegrityDisposition disposition,
        IntegrityValidationTrace trace)
    {
        Disposition = disposition;
        Trace = trace;
    }

    public IntegrityDisposition Disposition { get; }
    public IntegrityValidationTrace Trace { get; }
}

public sealed class IntegrityValidationException : Exception
{
    public IntegrityValidationException(string message)
        : base(message)
    {
    }

    public IntegrityValidationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}

internal static class IntegrityValidationInvariants
{
    private static readonly IntegrityConcernKind[] ConcernOrder =
    {
        IntegrityConcernKind.PotentialInaccessibleInformationUse,
        IntegrityConcernKind.PotentialProtectedInformationExposure,
        IntegrityConcernKind.PotentialLockedAuthorityViolation,
        IntegrityConcernKind.PotentialTechnicalArtifactLeak,
        IntegrityConcernKind.IndeterminateSemanticIntegrity
    };

    public static void ValidateInput(IntegrityCandidateInput? input)
    {
        if (input is null)
        {
            throw new IntegrityValidationException("IntegrityCandidateInput is required.");
        }

        if (!string.Equals(
                input.CandidateContentIdentityContract,
                E0IntegrityContracts.CandidateContentIdentityContract,
                StringComparison.Ordinal))
        {
            throw new IntegrityValidationException(
                "IntegrityCandidateInput content identity contract is unsupported.");
        }

        ValidateLowerHexHash(input.CandidateContentHash, "IntegrityCandidateInput content hash");

        try
        {
            _ = input.SourceContextPacketId.Value;
        }
        catch (InvalidOperationException exception)
        {
            throw new IntegrityValidationException(
                "IntegrityCandidateInput source ContextPacket identity is not initialized.",
                exception);
        }

        ValidateRejectCodes(input.DeterministicRejectCodes);
    }

    public static void ValidateEvidence(
        IntegrityCandidateInput input,
        IntegrityConcernEvidence? evidence)
    {
        if (evidence is null)
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence is required for a non-Reject input.");
        }

        if (!string.Equals(
                evidence.EvidenceContract,
                E0IntegrityContracts.ConcernEvidenceContract,
                StringComparison.Ordinal))
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence contract is unsupported.");
        }

        if (!string.Equals(
                evidence.CandidateContentIdentityContract,
                E0IntegrityContracts.CandidateContentIdentityContract,
                StringComparison.Ordinal))
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence content identity contract is unsupported.");
        }

        ValidateLowerHexHash(
            evidence.CandidateContentHash,
            "Integrity concern evidence Candidate content hash");

        if (!string.Equals(
                evidence.CandidateContentIdentityContract,
                input.CandidateContentIdentityContract,
                StringComparison.Ordinal) ||
            !string.Equals(
                evidence.CandidateContentHash,
                input.CandidateContentHash,
                StringComparison.Ordinal))
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence does not match the evaluated Candidate content identity.");
        }

        ValidateCanonicalConcerns(evidence.Concerns);
    }

    public static ImmutableArray<IntegrityConcernKind> CanonicalizeConcerns(
        ImmutableArray<IntegrityConcernKind> concerns)
    {
        if (concerns.IsDefault)
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence concerns must be initialized.");
        }

        if (concerns.Length > ConcernOrder.Length)
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence exceeds the supported concern count.");
        }

        var seen = new HashSet<IntegrityConcernKind>();
        foreach (var concern in concerns)
        {
            if (!IsDefinedConcern(concern))
            {
                throw new IntegrityValidationException(
                    "Integrity concern evidence contains an unsupported concern kind.");
            }

            if (!seen.Add(concern))
            {
                throw new IntegrityValidationException(
                    "Integrity concern evidence contains a duplicate concern kind.");
            }
        }

        return ConcernOrder
            .Where(seen.Contains)
            .ToImmutableArray();
    }

    private static void ValidateRejectCodes(
        ImmutableArray<IntegrityDeterministicRejectCode> rejectCodes)
    {
        if (rejectCodes.IsDefault)
        {
            throw new IntegrityValidationException(
                "IntegrityCandidateInput Reject codes must be initialized.");
        }

        if (rejectCodes.Length > 2)
        {
            throw new IntegrityValidationException(
                "IntegrityCandidateInput has too many deterministic Reject codes.");
        }

        var previousRank = -1;
        var seen = new HashSet<IntegrityDeterministicRejectCode>();
        foreach (var code in rejectCodes)
        {
            var rank = RejectCodeRank(code);
            if (!seen.Add(code))
            {
                throw new IntegrityValidationException(
                    "IntegrityCandidateInput contains duplicate deterministic Reject codes.");
            }

            if (rank <= previousRank)
            {
                throw new IntegrityValidationException(
                    "IntegrityCandidateInput deterministic Reject codes are not in canonical order.");
            }

            previousRank = rank;
        }
    }

    private static void ValidateCanonicalConcerns(
        ImmutableArray<IntegrityConcernKind> concerns)
    {
        var canonical = CanonicalizeConcerns(concerns);
        if (!canonical.SequenceEqual(concerns))
        {
            throw new IntegrityValidationException(
                "Integrity concern evidence concerns are not in canonical order.");
        }
    }

    private static int RejectCodeRank(IntegrityDeterministicRejectCode code) =>
        code switch
        {
            IntegrityDeterministicRejectCode.SubjectContextMismatch => 0,
            IntegrityDeterministicRejectCode.ContextPacketIdentityMismatch => 1,
            _ => throw new IntegrityValidationException(
                "IntegrityCandidateInput contains an unsupported deterministic Reject code.")
        };

    private static bool IsDefinedConcern(IntegrityConcernKind concern) =>
        concern is
            IntegrityConcernKind.PotentialInaccessibleInformationUse or
            IntegrityConcernKind.PotentialProtectedInformationExposure or
            IntegrityConcernKind.PotentialLockedAuthorityViolation or
            IntegrityConcernKind.PotentialTechnicalArtifactLeak or
            IntegrityConcernKind.IndeterminateSemanticIntegrity;

    private static void ValidateLowerHexHash(string? hash, string fieldName)
    {
        if (hash is null || hash.Length != 64)
        {
            throw new IntegrityValidationException($"{fieldName} must be 64 lowercase hexadecimal characters.");
        }

        foreach (var character in hash)
        {
            if (!((character >= '0' && character <= '9') ||
                  (character >= 'a' && character <= 'f')))
            {
                throw new IntegrityValidationException($"{fieldName} must be 64 lowercase hexadecimal characters.");
            }
        }
    }
}
