using kontado_csharp.Models;

namespace kontado_csharp.Data;

public class ProjectRepository : NestedYamlStore<Project>
{
    public ProjectRepository() : base(parentRoot: "clients", nestedFolder: "projects", fileName: "project.yaml") { }

    public List<Project> GetAll(string clientSlug) => LoadAll(clientSlug);
    public Project? GetProject(string clientSlug, string name) => LoadAll(clientSlug).Find(p => p.Name == name);
    public void Add(string clientSlug, Project project, string slug, bool overwrite = false)
    {
        Save(clientSlug, slug, project, overwrite);
        AddFolder(clientSlug, slug, "line_items");
    }
}