using System.Collections.Immutable;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Fixture;

namespace Ensemble.E0.Harness.Run;

internal sealed class E0BMixedCastConfiguration
{
    internal const string ApprovedConditionId = "E0B-MIXED-CAST-01";

    private readonly ImmutableDictionary<string, E0ARoleProfile> _performerByCharacter;
    private readonly ImmutableDictionary<string, E0AGeminiModelProfile> _routesByModel;

    private E0BMixedCastConfiguration(
        ImmutableDictionary<string, E0ARoleProfile> performerByCharacter,
        ImmutableDictionary<string, E0AGeminiModelProfile> routesByModel)
    {
        _performerByCharacter = performerByCharacter;
        _routesByModel = routesByModel;
    }

    internal string ConditionId => ApprovedConditionId;

    internal IReadOnlyList<KeyValuePair<string, E0ARoleProfile>> PerformerCast =>
        _performerByCharacter.OrderBy(x => x.Key, StringComparer.Ordinal).ToArray();

    internal IReadOnlyList<E0AGeminiModelProfile> Routes =>
        _routesByModel.Values.OrderBy(x => x.ProfileId, StringComparer.Ordinal).ToArray();

    internal E0ARoleProfile PerformerFor(CharacterId characterId)
    {
        string value;
        try
        {
            value = characterId.Value;
        }
        catch (InvalidOperationException)
        {
            throw new E0AHarnessException("E0-B Performer cast received an uninitialized CharacterId.");
        }

        if (!_performerByCharacter.TryGetValue(value, out var profile))
        {
            throw new E0AHarnessException("E0-B Performer Character is outside the approved fixed cast.");
        }
        return profile;
    }

    internal E0AGeminiModelProfile ModelFor(E0ARoleProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        if (!_routesByModel.TryGetValue(profile.Model, out var model))
        {
            throw new E0AHarnessException("E0-B role profile is outside the approved mixed-model routes.");
        }
        return model;
    }

    internal E0APricingAssumptions PricingFor(E0ARoleProfile profile) =>
        E0AGeminiPricingPolicy.ConservativeShadowPricingFor(ModelFor(profile));

    internal static E0BMixedCastConfiguration Approved01(E0ARunEnvelope referenceEnvelope)
    {
        ArgumentNullException.ThrowIfNull(referenceEnvelope);
        referenceEnvelope.Validate();
        if (!string.Equals(
                referenceEnvelope.ProviderProfileId,
                E0AGeminiModelCatalog.FlashLite35MinimalId,
                StringComparison.Ordinal))
        {
            throw new E0AHarnessException("E0-B mixed cast requires the selected Run 08 3.5 reference envelope.");
        }

        var alternate = E0AGeminiModelCatalog.FlashLite31Minimal;
        var voss = new E0ARoleProfile(
            E0ARole.Performer,
            E0AGeminiProviderPolicy.Provider,
            alternate.Model,
            E0AReasoningLevel.Minimal,
            true,
            E0ARunEnvelope.RoleMaxOutputTokens,
            E0AGeminiProviderPolicy.ServiceTier);

        var cast = ImmutableDictionary.CreateRange(StringComparer.Ordinal, new[]
        {
            new KeyValuePair<string, E0ARoleProfile>(MissingRaftContract.MarloweCharacterId, referenceEnvelope.Performer),
            new KeyValuePair<string, E0ARoleProfile>(MissingRaftContract.VossCharacterId, voss),
            new KeyValuePair<string, E0ARoleProfile>(MissingRaftContract.WrenCharacterId, referenceEnvelope.Performer)
        });
        var routes = ImmutableDictionary.CreateRange(StringComparer.Ordinal, new[]
        {
            new KeyValuePair<string, E0AGeminiModelProfile>(referenceEnvelope.ModelProfile.Model, referenceEnvelope.ModelProfile),
            new KeyValuePair<string, E0AGeminiModelProfile>(alternate.Model, alternate)
        });
        var configuration = new E0BMixedCastConfiguration(cast, routes);
        configuration.Validate(referenceEnvelope);
        return configuration;
    }

    internal void Validate(E0ARunEnvelope referenceEnvelope)
    {
        ArgumentNullException.ThrowIfNull(referenceEnvelope);
        referenceEnvelope.Validate();
        if (_performerByCharacter.Count != 3 || _routesByModel.Count != 2 ||
            !string.Equals(referenceEnvelope.ProviderProfileId, E0AGeminiModelCatalog.FlashLite35MinimalId, StringComparison.Ordinal) ||
            !string.Equals(referenceEnvelope.Variant, "CREATIVE-MINIMAL", StringComparison.Ordinal) ||
            referenceEnvelope.RunAcceptedTurnCap != E0ARunEnvelope.AcceptedTurnCap)
        {
            throw new E0AHarnessException("E0-B mixed-cast reference envelope is inconsistent.");
        }

        var expectedCharacters = new[]
        {
            MissingRaftContract.MarloweCharacterId,
            MissingRaftContract.VossCharacterId,
            MissingRaftContract.WrenCharacterId
        };
        if (!expectedCharacters.All(_performerByCharacter.ContainsKey))
        {
            throw new E0AHarnessException("E0-B mixed-cast roster is inconsistent.");
        }

        foreach (var entry in _performerByCharacter)
        {
            entry.Value.Validate();
            _ = ModelFor(entry.Value);
        }
        foreach (var route in _routesByModel.Values)
        {
            route.Validate();
            if (!route.LiveSelectable || route.AcceptedTurnCap != E0ARunEnvelope.AcceptedTurnCap ||
                route.ThinkingControl != E0AGeminiThinkingControlKind.Level)
            {
                throw new E0AHarnessException("E0-B mixed-cast route is outside the approved 12-Turn minimal envelope.");
            }
        }

        var voss = _performerByCharacter[MissingRaftContract.VossCharacterId];
        if (!string.Equals(voss.Model, E0AGeminiModelCatalog.FlashLite31Minimal.Model, StringComparison.Ordinal) ||
            !string.Equals(_performerByCharacter[MissingRaftContract.MarloweCharacterId].Model, referenceEnvelope.Performer.Model, StringComparison.Ordinal) ||
            !string.Equals(_performerByCharacter[MissingRaftContract.WrenCharacterId].Model, referenceEnvelope.Performer.Model, StringComparison.Ordinal) ||
            voss.Reasoning != E0AReasoningLevel.Minimal ||
            referenceEnvelope.Integrity.Reasoning != E0AReasoningLevel.High ||
            referenceEnvelope.Interpreter.Reasoning != E0AReasoningLevel.Minimal)
        {
            throw new E0AHarnessException("E0-B mixed-cast assignments do not match the approved Director decision.");
        }
    }
}
