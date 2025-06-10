namespace TodoApp
{
    public class ScreenDraw
    {
        public void DrawScreen(ProjectList projectList, Project? currentProject, Menu menu, SortMode sortMode)
        {
            Console.Clear();
            DrawMain(currentProject, sortMode);
            DrawFooter(menu, sortMode);
        }

        private void DrawMain(Project? project, SortMode sortMode)
        {
            if (project == null)
            {
                Console.WriteLine("No project selected.");
                return;
            }
            Console.WriteLine($"PROJECT: {project.Name}");
            Console.WriteLine($"TASKS ({(sortMode == SortMode.Date ? "Date" : "Alphabetic")}):");
            int i = 1;
            foreach (var item in project.GetSortedTasks(sortMode))
            {
                var status = item.IsCompleted ? "[x]" : "[ ]";
                var due = item.DueDate.HasValue ? $" (Due: {item.DueDate.Value:yyyy-MM-dd})" : "";
                Console.WriteLine($"{i++,2}. {status} {item.Description}{due}");
            }
            if (project.Tasks.Count == 0)
                Console.WriteLine("No tasks.");
        }

        private void DrawFooter(Menu menu, SortMode sortMode)
        {
            string menuText = menu.GetMenuText(sortMode);
            int footerLines = 2;
            int footerTop = Math.Max(0, Console.WindowHeight - footerLines);

            Console.SetCursorPosition(0, footerTop);
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.SetCursorPosition(0, footerTop + 1);
            Console.Write(menuText.PadRight(Console.WindowWidth));
        }

        public string ShowDialog(string title, string message)
        {
            var lines = message.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            int messageLines = lines.Length;

            string borderTitle = $" {title} ";
            int minWidth = Math.Max(borderTitle.Length + 2, lines.Max(l => l.Length) + 4);
            int width = Math.Min(Math.Max(minWidth, 30), Console.WindowWidth - 2);
            int height = 4 + messageLines;

            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 2;

            int dashCount = width - borderTitle.Length - 2;
            int dashLeft = dashCount / 2;
            int dashRight = dashCount - dashLeft;
            string topBorder = "+" + new string('-', dashLeft) + borderTitle + new string('-', dashRight) + "+";

            Console.SetCursorPosition(left, top);
            Console.Write(topBorder);

            Console.SetCursorPosition(left, top + 1);
            Console.Write("|" + new string(' ', width - 2) + "|");

            for (int i = 0; i < messageLines; i++)
            {
                Console.SetCursorPosition(left, top + 2 + i);
                string line = lines[i].PadRight(width - 2);
                Console.Write("|" + line + "|");
            }

            Console.SetCursorPosition(left, top + 2 + messageLines);
            Console.Write("|" + new string(' ', width - 2) + "|");

            Console.SetCursorPosition(left, top + 3 + messageLines);
            Console.Write("+" + new string('-', width - 2) + "+");

            Console.SetCursorPosition(left + 2, top + 2 + messageLines);
            string? input = Console.ReadLine();

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', width));
            }

            return input ?? string.Empty;
        }

        public void ShowMessageDialog(string title, string message)
        {
            string borderTitle = $" {title} ";
            int minWidth = Math.Max(borderTitle.Length + 2, Math.Max(message.Length + 4, 30));
            int width = Math.Min(minWidth, Console.WindowWidth - 2);
            int height = 5;
            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 2;

            int dashCount = width - borderTitle.Length - 2;
            int dashLeft = dashCount / 2;
            int dashRight = dashCount - dashLeft;
            string topBorder = "+" + new string('-', dashLeft) + borderTitle + new string('-', dashRight) + "+";

            Console.SetCursorPosition(left, top);
            Console.Write(topBorder);
            Console.SetCursorPosition(left, top + 1);
            Console.Write("|" + new string(' ', width - 2) + "|");
            Console.SetCursorPosition(left, top + 2);
            Console.Write("|" + message.PadRight(width - 2) + "|");
            Console.SetCursorPosition(left, top + 3);
            Console.Write("|" + CenterText("Press any key to continue...", width - 2) + "|");
            Console.SetCursorPosition(left, top + 4);
            Console.Write("+" + new string('-', width - 2) + "+");

            Console.SetCursorPosition(left + width - 1, top + 4);
            Console.ReadKey(true);

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', width));
            }
        }

        private string CenterText(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int leftPadding = (width - text.Length) / 2;
            int rightPadding = width - text.Length - leftPadding;
            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }
    }
}