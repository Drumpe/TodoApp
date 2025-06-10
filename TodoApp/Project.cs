namespace TodoApp
{
    public class Project
    {
        public string Name { get; set; }
        public List<Task> Tasks { get; set; } = new();

        public Project(string name)
        {
            Name = name;
        }

		public void AddTask(Task item) => Tasks.Add(item);

		public void RemoveTask(int index)
		{
			if (index >= 0 && index < Tasks.Count)
				Tasks.RemoveAt(index);
		}

		public void ToggleCompletion(int index)
		{
			if (index >= 0 && index < Tasks.Count)
				Tasks[index].MarkCompleted(!Tasks[index].IsCompleted);
		}

		// Gets the sorted tasks based on the specified sort mode
		public IEnumerable<Task> GetSortedTasks(SortMode mode)
		{
			return mode switch
			{
				SortMode.Alphabetic => Tasks.OrderBy(x => x.Description, StringComparer.OrdinalIgnoreCase),
				_ => Tasks.OrderBy(x => x.CreatedDate)
			};
		}

		// Gets the actual index of an item based on its display index in the sorted list
		public int? GetActualIndexFromSortedDisplayIndex(int displayIndex, SortMode mode)
		{
			var sorted = GetSortedTasks(mode).ToList();
			if (displayIndex < 1 || displayIndex > sorted.Count)
				return null;
			var item = sorted[displayIndex - 1];
			return Tasks.IndexOf(item);
		}

	}
}