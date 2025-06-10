namespace TodoApp
{
    public enum SortMode // Enum to define the sorting modes for the todo list
    {
        Date,
        Alphabetic
    }

    public class TodoApp
    {
        private Menu Menu { get; }
        private ProjectList ProjectList { get; }
        private ScreenDraw Draw { get; }
        private FileHandle File { get; }
        private SortMode CurrentSortMode { get; set; } = SortMode.Date;
        private Project? CurrentProject { get; set; }

        /**
         * Initializes a new instance of the TodoApp class.
         * This constructor sets up the menu, project list, screen drawing, and file handling.
         * It also attempts to load existing projects from a JSON file on startup.
         */
        public TodoApp()
        {
            Menu = new Menu();
            ProjectList = new ProjectList();
            Draw = new ScreenDraw();
            File = new FileHandle("projects.json");

            // Load on startup if file exists
            if (File.FileExists)
            {
                var loaded = File.ReadJSON();
                if (loaded != null)
                    ProjectList.Projects = loaded.Projects;
            }

            // Select or create a project at startup
            CurrentProject = SelectOrCreateProject();
        }

        /**
         * Starts the application and runs the main loop.
         * This method will continue to run until the user chooses to exit.
         */
        public void Run()
        {
            bool running = true;
            while (running)
            {
                Draw.DrawScreen(ProjectList, CurrentProject, Menu, CurrentSortMode);
                var action = Menu.GetAction(CurrentSortMode);
                running = Handle(action);
            }
        }

        /**
         * Handles the action based on the user's input.
         * This method will perform the corresponding action for each key press.
         * Returns false if the user chooses to quit the application.
         */
        private bool Handle(char action)
        {
            switch (char.ToUpperInvariant(action))
            {
                case 'A':
                    if (CurrentProject == null) break;
                    var desc = Draw.ShowDialog("Add Task", "Enter description:");
                    if (!string.IsNullOrWhiteSpace(desc))
                        CurrentProject.AddTask(new Task(desc));
                    break;
                case 'E':
                    if (CurrentProject == null) break;
                    var editIdxStr = Draw.ShowDialog("Edit Task", "Enter task number to edit:");
                    if (int.TryParse(editIdxStr, out int editIdx))
                    {
                        var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(editIdx, CurrentSortMode);
                        if (realIndex.HasValue)
                        {
                            var item = CurrentProject.Tasks[realIndex.Value];
                            var newDesc = Draw.ShowDialog("Edit Task", $"Current: {item.Description}\nNew description:");
                            if (!string.IsNullOrWhiteSpace(newDesc))
                                item.Description = newDesc;
                        }
                    }
                    break;
                case 'R':
                    if (CurrentProject == null) break;
                    var idxStr = Draw.ShowDialog("Remove Task", "Enter task number to remove:");
                    if (int.TryParse(idxStr, out int idx))
                    {
                        var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(idx, CurrentSortMode);
                        if (realIndex.HasValue)
                            CurrentProject.RemoveTask(realIndex.Value);
                    }
                    break;
                case 'C':
                    if (CurrentProject == null) break;
                    var cidxStr = Draw.ShowDialog("Toggle Complete", "Enter task number to toggle:");
                    if (int.TryParse(cidxStr, out int cidx))
                    {
                        var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(cidx, CurrentSortMode);
                        if (realIndex.HasValue)
                            CurrentProject.ToggleCompletion(realIndex.Value);
                    }
                    break;
                case 'S':
                    File.SaveAsJSON(ProjectList);
                    Draw.ShowMessageDialog("Save", "Projects saved successfully.");
                    break;
                case 'L':
                    var loaded = File.ReadJSON();
                    if (loaded != null)
                    {
                        ProjectList.Projects = loaded.Projects;
                        Draw.ShowMessageDialog("Load", "Projects loaded successfully.");
                        CurrentProject = SelectOrCreateProject();
                    }
                    else
                    {
                        Draw.ShowMessageDialog("Load", "No file to load.");
                    }
                    break;
                case 'T':
                    CurrentSortMode = CurrentSortMode == SortMode.Date ? SortMode.Alphabetic : SortMode.Date;
                    Draw.ShowMessageDialog("Sort Mode", $"Now sorting by {(CurrentSortMode == SortMode.Date ? "Date" : "Alphabetic")}.");
                    break;
                case 'Q':
                    return false;
                case 'P':
                    CurrentProject = SelectOrCreateProject();
                    break;
            }
            return true;
        }

        /**
         * Prompts the user to select or create a project.
         * If no projects exist, the user is prompted to create a new project.
         * If projects exist, the user can select an existing project or create a new one.
         * Returns the selected or newly created project.
         */
        private Project SelectOrCreateProject()
        {
            while (true)
            {
                if (ProjectList.Projects.Count == 0)
                {
                    var name = Draw.ShowDialog("New Project", "No projects found. Enter a project name:");
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        var project = new Project(name);
                        ProjectList.Projects.Add(project);
                        return project;
                    }
                }
                else
                {
                    var projectNames = ProjectList.Projects.Select((p, i) => $"{i + 1}. {p.Name}").ToList();
                    var selection = Draw.ShowDialog("Select Project", string.Join("\n", projectNames) + "\nN. New Project\n\nEnter number or N:");
                    if (selection.Trim().ToUpper() == "N")
                    {
                        var name = Draw.ShowDialog("New Project", "Enter project name:");
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            var project = new Project(name);
                            ProjectList.Projects.Add(project);
                            return project;
                        }
                    }
                    if (int.TryParse(selection, out int idx) && idx > 0 && idx <= ProjectList.Projects.Count)
                        return ProjectList.Projects[idx - 1];
                }
            }
        }
    }
}