using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using kontado_csharp.Models;

namespace kontado_csharp.Data;

public abstract class NestedYamlStore<T>(
    string parentRoot,      // "clients"
    string nestedFolder,    // "projects"
    string fileName)        // "project.yaml"
    where T : class, IEntity
{
    // LoadAll needs the parent slug because it's part of the path on disk.
    // clients/<parentSlug>/projects/<*>/project.yaml
    public List<T> LoadAll(string parentSlug)
    {
        if (!Directory.Exists(parentRoot)) return [];

        var deserializer = MakeDeserializer();
        var items = new List<T>();


        var nestedRoot = Path.Combine(parentRoot, parentSlug, nestedFolder);
        if (!Directory.Exists(nestedRoot)) return [];
        foreach (var dir in Directory.EnumerateDirectories(nestedRoot))
        {
            var file = Path.Combine(dir, fileName);
            if (!File.Exists(file)) continue;
            var yaml = File.ReadAllText(file);
            var item = deserializer.Deserialize<T>(yaml);
            items.Add(item);
        }
        return items.OrderBy(x => x.Id).ToList();
    }

    public void Save(string parentSlug, string slug, T item, bool overwrite = false)
    {
        var nestedRoot = Path.Combine(parentRoot, parentSlug, nestedFolder);
        Directory.CreateDirectory(nestedRoot);
        var dir = Path.Combine(nestedRoot, slug);

        if (Directory.Exists(dir) && !overwrite)
            throw new InvalidOperationException($"'{slug}' already exists.");

        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, fileName);
        var tmp  = file + ".tmp";

        var serializer = MakeSerializer();
        using (var sw = File.CreateText(tmp))
            serializer.Serialize(sw, item);

        File.Move(tmp, file, overwrite: true);
    }
    public int NextId(string parentSlug) =>
        LoadAll(parentSlug).Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;

    public void AddFolder(string parentSlug, string slug, string folderName)
    {
        var dir = Path.Combine(parentRoot, parentSlug, nestedFolder, slug, folderName);
        Directory.CreateDirectory(dir);
    }

    protected virtual ISerializer MakeSerializer() =>
        new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

    protected virtual IDeserializer MakeDeserializer() =>
        new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
}