namespace TodoApp
{
	public class Menu
	{
		private Dictionary<char, string> Options { get; }

		public Menu()
		{
			Options = new Dictionary<char, string>
			{
				{ 'A', "Add" },
				{ 'E', "Edit" },
				{ 'R', "Remove" },
				{ 'C', "Complete" },
				{ 'T', "" }, // Placeholder for dynamic label
                { 'S', "Save" },
				{ 'L', "Load" },
				{ 'Q', "Quit" }
			};
		}

		public char GetAction(SortMode sortMode)
		{
			return HandleInput(sortMode);
		}

		public string GetMenuText(SortMode sortMode)
		{
			var parts = new List<string>();
			foreach (var opt in Options)
			{
				string label = opt.Key == 'T'
					? $"Sort: {(sortMode == SortMode.Date ? "Alphabetic" : "Date")}"
					: opt.Value;
				parts.Add($"[{opt.Key}] {label}");
			}
			return string.Join(" | ", parts);
		}

		public char HandleInput(SortMode sortMode)
		{
			Console.CursorVisible = false;
			try
			{
				while (true)
				{
					var key = Console.ReadKey(true).KeyChar;
					var upperKey = char.ToUpperInvariant(key);
					if (Options.ContainsKey(upperKey))
						return upperKey;
				}
			}
			finally
			{
				Console.CursorVisible = true;
			}
		}
	}
}