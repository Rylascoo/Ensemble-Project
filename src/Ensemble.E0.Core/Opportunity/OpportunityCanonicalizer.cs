using System.Security.Cryptography;
using System.Text;
using Ensemble.E0.Core.Domain;
using Ensemble.E0.Core.Production;
using Ensemble.E0.Core.Serialization;

namespace Ensemble.E0.Core.Opportunity;

internal static class OpportunityCanonicalizer
{
    internal static StateHash ComputeResultHash(
        StateHash parentStateHash,
        string strategyContract,
        CharacterId selectedCharacterId,
        ProductionStateProjection resultProjection)
    {
        var builder = new StringBuilder();
        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "hashContract",
            ProductionStateContracts.StateHashContractVersion);
        AppendStringProperty(builder, ref first, "kind", "opportunityTransition");
        AppendStringProperty(builder, ref first, "parentStateHash", parentStateHash.Value);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "opportunityPayload");
        AppendPayload(builder, strategyContract, selectedCharacterId);

        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, "resultProjection");
        ProductionStateCanonicalizer.AppendProjection(builder, resultProjection);
        builder.Append('}');

        var bytes = CanonicalJson.EncodeUtf8(builder.ToString());
        var digest = SHA256.HashData(bytes);
        return StateHash.Create(Convert.ToHexString(digest).ToLowerInvariant());
    }

    internal static byte[] SerializePayload(
        string strategyContract,
        CharacterId selectedCharacterId)
    {
        var builder = new StringBuilder();
        AppendPayload(builder, strategyContract, selectedCharacterId);
        return CanonicalJson.EncodeUtf8(builder.ToString());
    }

    private static void AppendPayload(
        StringBuilder builder,
        string strategyContract,
        CharacterId selectedCharacterId)
    {
        builder.Append('{');
        var first = true;
        AppendStringProperty(
            builder,
            ref first,
            "schemaVersion",
            E0OpportunityTransitionContracts.ContractVersion);
        AppendStringProperty(builder, ref first, "strategyContract", strategyContract);
        AppendStringProperty(
            builder,
            ref first,
            "selectedCharacterId",
            selectedCharacterId.Value);
        builder.Append('}');
    }

    private static void AppendStringProperty(
        StringBuilder builder,
        ref bool first,
        string name,
        string value)
    {
        CanonicalJson.AppendSeparator(builder, ref first);
        CanonicalJson.AppendPropertyName(builder, name);
        CanonicalJson.AppendString(builder, value);
    }
}
