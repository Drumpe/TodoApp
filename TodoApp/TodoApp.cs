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
					AddTask();
					break;
				case 'E':
					EditTask();
					break;
				case 'R':
					RemoveTask();
					break;
				case 'C':
					ToggleCompletion();
					break;
				case 'S':
					File.SaveAsJSON(ProjectList);
					Draw.ShowMessageDialog("Save", "Projects saved successfully.");
					break;
				case 'L':
					LoadFromFile();
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
		 * Loads projects from a JSON file and updates the current project list.
		 * If the file is successfully loaded, it will show a success message.
		 * If no file exists, it will show an error message.
		 */
		private void LoadFromFile()
		{
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
		}

		/**
		 * Prompts the user to remove a task.
		 * If no projects exist, it will prompt the user to select a task from all tasks across all projects.
		 * If a project is selected, it will prompt the user to remove a task within that project.
		 */
		private void RemoveTask()
		{
			var idxStr = Draw.ShowInputDialog("Remove Task", "Enter task number to remove:");
			if (!int.TryParse(idxStr, out int idx)) return; // Invalid input
			if (CurrentProject == null)
			{
				var allTasks = ProjectList.Projects.SelectMany(p => p.GetSortedTasks(CurrentSortMode)).ToList();
				if (idx > 0 && idx <= allTasks.Count)
				{
					var task = allTasks[idx - 1];
					var project = ProjectList.Projects.First(p => p.Tasks.Contains(task));
					int taskIndex = project.Tasks.IndexOf(task);
					if (taskIndex >= 0)
						project.RemoveTask(taskIndex);
				}
			}
			else
			{
				var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(idx, CurrentSortMode);
				if (realIndex.HasValue)
					CurrentProject.RemoveTask(realIndex.Value);
			}
		}

		/**
		 * Toggles the completion status of a task.
		 * If no projects exist, it will prompt the user to select a task from all tasks across all projects.
		 * If a project is selected, it will prompt the user to toggle completion for a task within that project.
		 */
		private void ToggleCompletion()
		{
			var cidxStr = Draw.ShowInputDialog("Toggle Complete", "Enter task number to toggle:");
			if (!int.TryParse(cidxStr, out int cidx)) return; // Invalid input

			if (CurrentProject == null)
			{
				var allTasks = ProjectList.Projects.SelectMany(p => p.GetSortedTasks(CurrentSortMode)).ToList();
				if (cidx > 0 && cidx <= allTasks.Count)
				{
					var task = allTasks[cidx - 1];
					var project = ProjectList.Projects.First(p => p.Tasks.Contains(task));
					int taskIndex = project.Tasks.IndexOf(task);
					if (taskIndex >= 0)
						project.ToggleCompletion(taskIndex);
				}
			}
			else
			{
				var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(cidx, CurrentSortMode);
				if (realIndex.HasValue)
					CurrentProject.ToggleCompletion(realIndex.Value);
			}
		}

		/**
		 * Prompts the user to edit an existing task.
		 * If no projects exist, it will prompt the user to select a task from all tasks across all projects.
		 * If a project is selected, it will prompt the user to edit a task within that project.
		 * The user can change the task description and due date.
		 */
		private void EditTask()
		{
			var editIdxStr = Draw.ShowInputDialog("Edit Task", "Enter task number to edit:");

			if (!int.TryParse(editIdxStr, out int editIdx))
				return; // Invalid input, exit edit

			if (CurrentProject == null)
			{
				var allTasks = ProjectList.Projects
					.SelectMany(p => p.GetSortedTasks(CurrentSortMode))
					.ToList();

				if (editIdx > 0 && editIdx <= allTasks.Count)
				{
					var task = allTasks[editIdx - 1];
					InputTask(task);
				}
			}
			else
			{
				var realIndex = CurrentProject.GetActualIndexFromSortedDisplayIndex(editIdx, CurrentSortMode);
				if (realIndex.HasValue)
				{
					var task = CurrentProject.Tasks[realIndex.Value];
					InputTask(task);
				}
			}

			/**
			 * Local method to handle task input for editing.
			 * Prompts the user to input new details for a task.
			 * This method will show input dialogs for editing the task description and due date.
			 * @param task The task to edit.
			 */
			void InputTask(Task task)
			{
				var newDesc = Draw.ShowInputDialog(
					"Edit Task",
					$"Current: {task.Description}\nNew description:"
				);
				if (!string.IsNullOrWhiteSpace(newDesc))
					task.Description = newDesc;

				var newDueStr = Draw.ShowInputDialog(
					"Edit Task",
					$"Current due: {(task.DueDate.HasValue ? task.DueDate.Value.ToString("yyyy-MM-dd") : "none")}\nNew due date (yyyy-MM-dd) or leave blank:"
				);
				if (!string.IsNullOrWhiteSpace(newDueStr) && DateTime.TryParse(newDueStr, out var newDue))
					task.DueDate = newDue;
			}
		}

		/**
		 * Prompts the user to add a new task to a project.
		 * If no projects exist, the user is prompted to create a new project.
		 * If projects exist, the user can select an existing project or create a new one.
		 * The user is then prompted to enter the task description and optional due date.
		 */
		private void AddTask()
		{
			var project = SelectOrCreateProject();
			if (project == null) return;
			var desc = Draw.ShowInputDialog("Add Task", "Enter description:");
			if (!string.IsNullOrWhiteSpace(desc))
			{
				var dueDateStr = Draw.ShowInputDialog("Add Task", "Enter due date (yyyy-MM-dd) or leave blank:");
				DateTime? dueDate = null;
				if (!string.IsNullOrWhiteSpace(dueDateStr) && DateTime.TryParse(dueDateStr, out var parsedDue))
					dueDate = parsedDue;
				project.AddTask(new Task(desc, dueDate));
			}
		}

		/**
         * Prompts the user to select or create a project.
         * If no projects exist, the user is prompted to create a new project.
         * If projects exist, the user can select an existing project or create a new one.
         * Returns the selected or newly created project.
         */
		private Project? SelectOrCreateProject()
		{
			Console.Clear();
			while (true)
			{
				if (ProjectList.Projects.Count == 0)
				{
					var name = Draw.ShowInputDialog("New Project", "No projects found. Enter a project name:");
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
					var selection = Draw.ShowInputDialog(
						"Select Project",
						"A. All tasks\n" +
						string.Join("\n", projectNames) +
						"\nN. New Project\n\nEnter number, A, or N:"
					);
					if (selection.Trim().ToUpper() == "A")
						return null; // Special value for "All tasks"
					if (selection.Trim().ToUpper() == "N")
					{
						var name = Draw.ShowInputDialog("New Project", "Enter project name:");
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