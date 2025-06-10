namespace TodoApp
{
    public class TodoList
    {
        public List<TodoItem> Items { get; set; }

        public TodoList()
        {
            Items = new List<TodoItem>();
        }

		// Adds a new item to the todo list
		public void AddItem(TodoItem item) => Items.Add(item);

		// Removes an item from the todo list by index
		public void RemoveItem(int index)
        {
            if (index >= 0 && index < Items.Count)
                Items.RemoveAt(index);
        }

		// Toggles the completion status of an item
		public void ToggleCompletion(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                var item = Items[index];
                item.MarkCompleted(!item.IsCompleted);
            }
        }

		// Gets the sorted items based on the specified sort mode
		public IEnumerable<TodoItem> GetSortedItems(SortMode mode)
        {
            return mode switch
            {
                SortMode.Alphabetic => Items.OrderBy(x => x.Description, StringComparer.OrdinalIgnoreCase),
                _ => Items.OrderBy(x => x.CreatedDate)
            };
        }

		// Gets the actual index of an item based on its display index in the sorted list
		public int? GetActualIndexFromSortedDisplayIndex(int displayIndex, SortMode mode)
        {
            var sorted = GetSortedItems(mode).ToList();
            if (displayIndex < 1 || displayIndex > sorted.Count)
                return null;
            var item = sorted[displayIndex - 1];
            return Items.IndexOf(item);
        }
    }
}