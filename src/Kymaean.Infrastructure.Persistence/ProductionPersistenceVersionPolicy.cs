namespace Kymaean.Infrastructure.Persistence;

internal static class ProductionPersistenceVersionPolicy
{
    public const uint JournalSchemaVersion = 1;
    public const uint ProductionIdentityMetadataVersion = 1;
    public const uint ProductionProjectionSnapshotVersion = 4;
    public const uint ProductionPortableExportVersion = 1;
    public const string ProductionCreatedContractV1 = "kymaean.production.created.v1";
    public const string ProductionCreatedContractV2 = "kymaean.production.created.v2";
    public const string CreatorReplacedWorldCurrentStateContractV1 =
        "kymaean.production.creator-replaced-world-current-state.v1";
    public const string CharacterCreatedContractV1 =
        "kymaean.production.character-created.v1";
    public const string CreatorEstablishedSceneContractV1 =
        "kymaean.production.creator-established-scene.v1";

    public static void RequireJournalSchema(uint foundVersion, string artifact)
    {
        if (foundVersion != JournalSchemaVersion)
        {
            throw new ProductionPersistenceCompatibilityException(
                artifact,
                foundVersion.ToString(System.Globalization.CultureInfo.InvariantCulture),
                JournalSchemaVersion.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    public static void RequireIdentityMetadataVersion(uint foundVersion)
    {
        if (foundVersion != ProductionIdentityMetadataVersion)
        {
            throw new ProductionPersistenceCompatibilityException(
                "Production identity metadata",
                foundVersion.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ProductionIdentityMetadataVersion.ToString(
                    System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    public static void RequirePortableExportVersion(uint foundVersion)
    {
        if (foundVersion != ProductionPortableExportVersion)
        {
            throw new ProductionPersistenceCompatibilityException(
                "Production portable export",
                foundVersion.ToString(System.Globalization.CultureInfo.InvariantCulture),
                ProductionPortableExportVersion.ToString(
                    System.Globalization.CultureInfo.InvariantCulture));
        }
    }

    public static ProductionPersistenceCompatibilityException UnsupportedEventContract(
        string artifact,
        string foundContract,
        string supportedContract) =>
        new(artifact, foundContract, supportedContract);
}

public sealed class ProductionPersistenceCompatibilityException : IOException
{
    internal ProductionPersistenceCompatibilityException(
        string artifact,
        string foundIdentifier,
        string supportedIdentifier)
        : base(
            $"Unsupported {artifact} version '{foundIdentifier}'. " +
            $"This build supports only '{supportedIdentifier}' and does not migrate storage implicitly.")
    {
        Artifact = artifact;
        FoundIdentifier = foundIdentifier;
        SupportedIdentifier = supportedIdentifier;
    }

    public string Artifact { get; }

    public string FoundIdentifier { get; }

    public string SupportedIdentifier { get; }
}
