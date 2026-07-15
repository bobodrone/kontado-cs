using kontado_csharp.Models;

namespace kontado_csharp.Data;

public class ClientRepository : YamlStore<Client>
{
    public ClientRepository() : base("clients", "client.yaml") { }

    // Kept so ListCommand / ShowCommand that call repo.GetAll() still work.
    public List<Client> GetAll() => LoadAll();

    public Client? GetClient(string name) =>
        LoadAll().Find(c => c.Name == name);

    public void Add(Client client, string slug, bool overwrite = false) =>
        Save(slug, client, overwrite);
}