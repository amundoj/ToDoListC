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
                    case "1":
                        taskManager.AddTask();
                        break;
                    case "2":
                        taskManager.ViewTasks();
                        break;
                    case "3":
                        taskManager.MarkTaskAsCompleted();
                        break;
                    case "4":
                        taskManager.RemoveTask();
                        break;
                    case "5":
                        exitRequested = true;
                        Console.WriteLine("👋 Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("⚠ Invalid choice, please try again.");
                        break;
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Displays the main menu.
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("\n=== 📝 To-Do List Application ===");
            Console.WriteLine("1️ Add a new task");
            Console.WriteLine("2️ View all tasks");
            Console.WriteLine("3️ Mark a task as completed");
            Console.WriteLine("4️ Remove a task");
            Console.WriteLine("5️ Exit");
            Console.Write("Enter your choice: ");
        }
    }
}
