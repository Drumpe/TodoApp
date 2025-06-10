namespace TodoApp
{
    public class TodoList
    {
        public List<TodoItem> Items { get; set; }

        public TodoList()
        {
            Items = new List<TodoItem>();
        }

        public void AddItem(TodoItem item) => Items.Add(item);

        public void RemoveItem(int index)
        {
            if (index >= 0 && index < Items.Count)
                Items.RemoveAt(index);
        }

        public void MarkCompleted(int index, bool completed)
        {
            if (index >= 0 && index < Items.Count)
                Items[index].MarkCompleted(completed);
        }

        public void ToggleCompletion(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                var item = Items[index];
                item.MarkCompleted(!item.IsCompleted);
            }
        }

        public IEnumerable<TodoItem> GetSortedItems(SortMode mode)
        {
            return mode switch
            {
                SortMode.Alphabetic => Items.OrderBy(x => x.Description, StringComparer.OrdinalIgnoreCase),
                _ => Items.OrderBy(x => x.CreatedDate)
            };
        }

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