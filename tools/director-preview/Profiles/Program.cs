using System.Text.Json;
using Kymaean.DirectorPreview;
using Kymaean.Windows.Preview;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;

if (args.Length == 3 && args[0] == "inspect-build-boundary")
{
    bool ContainsPreview(string path)
    {
        using var file = File.OpenRead(path);
        using var pe = new PEReader(file);
        var metadata = pe.GetMetadataReader();
        return metadata.TypeDefinitions.Any(handle => metadata.GetString(metadata.GetTypeDefinition(handle).Name) == "PreviewEnvironment");
    }
    if (ContainsPreview(args[1]) || !ContainsPreview(args[2])) throw new InvalidOperationException("Preview compilation boundary failed.");
    Console.WriteLine("ORDINARY_PREVIEW_TYPE_ABSENT; PREVIEW_TYPE_PRESENT=PASS");
    return;
}

if (args.Length < 2) throw new ArgumentException("command localState [kind name source | profileId]");
using var lease = PreviewEnvironment.Acquire(args[1]);
object result = args[0] switch
{
    "create" when args.Length == 5 => Profiles.Create(args[1], args[2], args[3], args[4]),
    "select" when args.Length == 3 => Profiles.Select(args[1], args[2]),
    "verify" => Profiles.Verify(args[1]),
    _ => throw new ArgumentException("Unsupported Preview profile operation.")
};
Console.WriteLine(JsonSerializer.Serialize(result));
