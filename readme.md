# TodoApp

A simple console-based Todo application built with .NET 9.

## Features

- Multiple projects, each with its own todo list
- Add, edit, and remove tasks within projects
- Mark tasks as complete or incomplete
- View all tasks across all projects or filter by project
- Sort tasks by creation date or alphabetically
- Save and load all projects and tasks to/from a JSON file
- Menu-driven user interface with keyboard shortcuts

## Main Components

- **Program.cs**: Application entry point.
- **TodoApp.cs**: Core logic, main loop, and user actions.
- **Menu.cs**: Handles menu display and user input.
- **ProjectList.cs**: Manages the collection of projects.
- **Project.cs**: Represents a single project and its tasks.
- **Task.cs**: Represents a single todo task.
- **FileHandle.cs**: Handles saving and loading all projects and tasks.
- **ScreenDraw.cs**: Manages console UI rendering and dialogs.

## Usage

1. Build and run the project with .NET 9.
2. On first run, create a project when prompted.
3. Use the menu to add, edit, remove, or complete tasks.
4. Switch between projects or view all tasks.
5. Save or load your projects and tasks as needed.

---

This project is for educational purposes and demonstrates a modular C# console application structure with support for multiple projects and persistent storage.