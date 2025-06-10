using System.Text.Json;

namespace TodoApp
{
    public class FileHandle
    {
        public string FileName { get; set; }

        public FileHandle(string fileName)
        {
            FileName = fileName;
        }

        public bool FileExists => File.Exists(FileName);

        public void SaveAsJSON(TodoList list)
        {
            var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json);
        }

        public TodoList? ReadJSON()
        {
            if (!FileExists)
                return null;
            var json = File.ReadAllText(FileName);
            return JsonSerializer.Deserialize<TodoList>(json);
        }
    }
}