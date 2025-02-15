using System;

namespace ToDoListApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new();
            bool exitRequested = false;

            while (!exitRequested)
            {
                ShowMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1": taskManager.AddTask(); break;
                    case "2": taskManager.ViewTasks(); break;
                    case "3": taskManager.EditTask(); break;
                    case "4": exitRequested = true; Console.WriteLine("👋 Exiting application."); break;
                    default: Console.WriteLine("⚠ Invalid choice."); break;
                }
                Console.WriteLine();
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n=== 📝 To-Do List Application ===");
            Console.WriteLine("1️ Add a task");
            Console.WriteLine("2️ View tasks");
            Console.WriteLine("3️ Edit a task");
            Console.WriteLine("4️ Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
