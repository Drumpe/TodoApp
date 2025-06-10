namespace TodoApp
{
    public class ProjectList
    {
        public List<Project> Projects { get; set; } = new();

        public Project? GetProject(string name) =>
            Projects.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Project GetOrAddProject(string name)
        {
            var project = GetProject(name);
            if (project == null)
            {
                project = new Project(name);
                Projects.Add(project);
            }
            return project;
        }
    }
}