namespace TodoApp
{
    public class ScreenDraw
    {
		/**
         * Draws the main screen with the todo list and menu.
         * This method clears the console, draws the main content, and footer with menu options.
         * @param todoList The todo list to display.
         * @param menu The menu to display at the bottom.
         * @param sortMode The current sorting mode for the todo items.
         */
		public void DrawScreen(TodoList todoList, Menu menu, SortMode sortMode)
        {
            Console.Clear();
            DrawMain(todoList, sortMode);
            DrawFooter(menu, sortMode);
        }

		/**
         * Draws the main content of the todo list.
         * This method displays the todo items sorted by the specified mode.
         * @param todoList The todo list containing items to display.
         * @param sortMode The current sorting mode for the todo items.
         */
		private void DrawMain(TodoList todoList, SortMode sortMode)
        {
            Console.WriteLine($"TODO LIST ( {(sortMode == SortMode.Date ? "Date" : "Alphabetic" )} ):");
            int i = 1;
            foreach (var item in todoList.GetSortedItems(sortMode))
            {
                var status = item.IsCompleted ? "[x]" : "[ ]";
                Console.WriteLine($"{i++}. {status} {item.Description}");
            }
            if (todoList.Items.Count == 0)
                Console.WriteLine("No items.");
        }

		/**
         * Draws the footer with menu options.
         * This method displays the available actions at the bottom of the console.
         * @param menu The menu containing available actions.
         * @param sortMode The current sorting mode for the todo items.
         */
		private void DrawFooter(Menu menu, SortMode sortMode)
        {
            string menuText = menu.GetMenuText(sortMode);
            int footerLines = 2; // separator + one-line menu
            int footerTop = Math.Max(0, Console.WindowHeight - footerLines);

            Console.SetCursorPosition(0, footerTop);
            Console.WriteLine(new string('-', Console.WindowWidth));
            Console.SetCursorPosition(0, footerTop + 1);
            Console.Write(menuText.PadRight(Console.WindowWidth));
        }

		/**
         * Displays a dialog box for user input.
         * This method shows a dialog with a title and message, allowing the user to enter text.
         * @param title The title of the dialog.
         * @param message The message to display in the dialog.
         * @return The user input as a string.
         */
		public string ShowDialog(string title, string message)
        {
            // Split message into lines
            var lines = message.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
            int messageLines = lines.Length;

            string borderTitle = $" {title} ";
            int minWidth = Math.Max(borderTitle.Length + 2, lines.Max(l => l.Length) + 4);
            int width = Math.Min(Math.Max(minWidth, 30), Console.WindowWidth - 2);
            int height = 4 + messageLines; // top, blank, message lines, input, bottom

            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 2;

            // Build top border with title centered
            int dashCount = width - borderTitle.Length - 2;
            int dashLeft = dashCount / 2;
            int dashRight = dashCount - dashLeft;
            string topBorder = "+" + new string('-', dashLeft) + borderTitle + new string('-', dashRight) + "+";

            // Draw dialog box
            Console.SetCursorPosition(left, top);
            Console.Write(topBorder);

            Console.SetCursorPosition(left, top + 1);
            Console.Write("|" + new string(' ', width - 2) + "|");

            // Write each message line
            for (int i = 0; i < messageLines; i++)
            {
                Console.SetCursorPosition(left, top + 2 + i);
                string line = lines[i].PadRight(width - 2);
                Console.Write("|" + line + "|");
            }

            // Input line
            Console.SetCursorPosition(left, top + 2 + messageLines);
            Console.Write("|" + new string(' ', width - 2) + "|");

            // Bottom border
            Console.SetCursorPosition(left, top + 3 + messageLines);
            Console.Write("+" + new string('-', width - 2) + "+");

            // Move cursor to input line
            Console.SetCursorPosition(left + 2, top + 2 + messageLines);
            string? input = Console.ReadLine();

            // Clear dialog area after input
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', width));
            }

            return input ?? string.Empty;
        }

		/**
         * Displays a message dialog box.
         * This method shows a dialog with a title and message, and waits for the user to press any key.
         * @param title The title of the dialog.
         * @param message The message to display in the dialog.
         */
		public void ShowMessageDialog(string title, string message)
        {
            // Prepare title on border
            string borderTitle = $" {title} ";
            int minWidth = Math.Max(borderTitle.Length + 2, Math.Max(message.Length + 4, 30));
            int width = Math.Min(minWidth, Console.WindowWidth - 2);
            int height = 5;
            int left = (Console.WindowWidth - width) / 2;
            int top = (Console.WindowHeight - height) / 2;

            // Build top border with title centered
            int dashCount = width - borderTitle.Length - 2;
            int dashLeft = dashCount / 2;
            int dashRight = dashCount - dashLeft;
            string topBorder = "+" + new string('-', dashLeft) + borderTitle + new string('-', dashRight) + "+";

            // Draw dialog box
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

            // Wait for key press
            Console.SetCursorPosition(left + width - 1, top + 4);
            Console.ReadKey(true);

            // Clear dialog area after input
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write(new string(' ', width));
            }
        }

		/**
         * Centers the given text within a specified width.
         * If the text is longer than the width, it will be truncated.
         * @param text The text to center.
         * @param width The width to center the text within.
         * @return The centered text.
         */
		private string CenterText(string text, int width)
        {
            if (text.Length >= width) return text.Substring(0, width);
            int leftPadding = (width - text.Length) / 2;
            int rightPadding = width - text.Length - leftPadding;
            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }
    }
}