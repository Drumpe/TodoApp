namespace TodoApp
{
	public enum SortMode
	{
		Date,
		Alphabetic
	}

	public class TodoApp
    {
        private Menu Menu { get; }
        private TodoList List { get; }
        private ScreenDraw Draw { get; }
        private FileHandle File { get; }
        private SortMode CurrentSortMode { get; set; } = SortMode.Date;

        public TodoApp()
        {
            Menu = new Menu();
            List = new TodoList();
            Draw = new ScreenDraw();
            File = new FileHandle("todos.json");

            // Load on startup if file exists
            if (File.FileExists)
            {
                var loaded = File.ReadJSON();
                if (loaded != null)
                    List.Items = loaded.Items;
            }
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                Draw.DrawScreen(List, Menu, CurrentSortMode);
                var action = Menu.GetAction(CurrentSortMode);
                running = Handle(action);
            }
        }

        private bool Handle(char action)
        {
            switch (char.ToUpperInvariant(action))
            {
                case 'A':
                    var desc = Draw.ShowDialog("Add Task", "Enter description:");
                    if (!string.IsNullOrWhiteSpace(desc))
                        List.AddItem(new TodoItem(desc));
                    break;
                case 'E':
                    var editIdxStr = Draw.ShowDialog("Edit Task", "Enter item number to edit:");
                    if (int.TryParse(editIdxStr, out int editIdx))
                    {
                        var realIndex = List.GetActualIndexFromSortedDisplayIndex(editIdx, CurrentSortMode);
                        if (realIndex.HasValue)
                        {
                            var item = List.Items[realIndex.Value];
                            var newDesc = Draw.ShowDialog("Edit Task", $"Current: {item.Description}\nNew description:");
                            if (!string.IsNullOrWhiteSpace(newDesc))
                                item.Description = newDesc;
                        }
                    }
                    break;
                case 'R':
                    var idxStr = Draw.ShowDialog("Remove Task", "Enter item number to remove:");
                    if (int.TryParse(idxStr, out int idx))
                    {
                        var realIndex = List.GetActualIndexFromSortedDisplayIndex(idx, CurrentSortMode);
                        if (realIndex.HasValue)
                            List.RemoveItem(realIndex.Value);
                    }
                    break;
                case 'C':
                    var cidxStr = Draw.ShowDialog("Toggle Task", "Enter item number to toggle:");
                    if (int.TryParse(cidxStr, out int cidx))
                    {
                        var realIndex = List.GetActualIndexFromSortedDisplayIndex(cidx, CurrentSortMode);
                        if (realIndex.HasValue)
                            List.ToggleCompletion(realIndex.Value);
                    }
                    break;
                case 'S':
                    File.SaveAsJSON(List);
                    Draw.ShowMessageDialog("Save", "File saved successfully.");
                    break;
                case 'L':
                    var loaded = File.ReadJSON();
                    if (loaded != null)
                    {
                        List.Items = loaded.Items;
                        Draw.ShowMessageDialog("Load", "File loaded successfully.");
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
            }
            return true;
        }
    }
}