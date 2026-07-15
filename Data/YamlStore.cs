using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using kontado_csharp.Models;

namespace kontado_csharp.Data;


// Generic YAML store. One subclass per entity type (ClientRepository, ProjectRepository, ...).
// T must be a reference type (class) AND implement IEntity so we can compute NextId generically.
public abstract class YamlStore<T>(string rootFolder, string fileName) where T : class, IEntity
{
    private readonly string _rootFolder = rootFolder;
    private readonly string _fileName = fileName;

    // Read every <rootFolder>/<slug>/<fileName> into a List<T>.
    public List<T> LoadAll()
    {
        if (!Directory.Exists(_rootFolder)) return [];

        var deserializer = MakeDeserializer();
        var items = new List<T>();
        foreach (var dir in Directory.EnumerateDirectories(_rootFolder))
        {
            var file = Path.Combine(dir, _fileName);
            if (!File.Exists(file)) continue;
            var yaml = File.ReadAllText(file);
            var item = deserializer.Deserialize<T>(yaml);
            items.Add(item);
        }
        return items.OrderBy(x => x.Id).ToList();
    }    
    
    // Write one entity to <rootFolder>/<slug>/<fileName>.
    // Atomic: serialize to a .tmp file, then File.Move (rename) over the target.
    public void Save(string slug, T item, bool overwrite = false)
    {
        Directory.CreateDirectory(_rootFolder);
        var dir  = Path.Combine(_rootFolder, slug);
        if (Directory.Exists(dir) && !overwrite)
            throw new InvalidOperationException($"'{slug}' already exists.");

        Directory.CreateDirectory(dir);
        var file = Path.Combine(dir, _fileName);
        var tmp  = file + ".tmp";

        var serializer = MakeSerializer();
        using (var sw = File.CreateText(tmp))
            serializer.Serialize(sw, item);

        File.Move(tmp, file, overwrite: true);
    }

    public int NextId() => LoadAll().Select(x => x.Id).DefaultIfEmpty(0).Max() + 1;

    protected virtual ISerializer MakeSerializer() =>
        new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

    protected virtual IDeserializer MakeDeserializer() =>
        new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
}