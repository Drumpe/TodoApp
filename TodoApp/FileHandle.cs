using System.Text.Json;

namespace TodoApp
{

    // FileHandle class for managing file operations related to ProjectList
    public class FileHandle
    {
        public string FileName { get; set; }

        public FileHandle(string fileName)
        {
            FileName = fileName;
        }

        public bool FileExists => File.Exists(FileName);

        public void SaveAsJSON(ProjectList projectList)
        {
            var json = JsonSerializer.Serialize(projectList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }

        public ProjectList? ReadJSON()
        {
            if (!FileExists)
                return null;
            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<ProjectList>(json);
        }
    }
}