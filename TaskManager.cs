using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ToDoListApp
{
    /// <summary>
    /// Manages the list of tasks, including saving, loading, sorting, filtering, and editing.
    /// </summary>
    public class TaskManager
    {
        private const string FilePath = "tasks.json";
        private const string LogFile = "app.log";
        private readonly List<TodoTask> tasks = new();

        public TaskManager()
        {
            LoadTasks(); // Load tasks when the program starts
        }

        /// <summary>
        /// Adds a new task.
        /// </summary>
        public void AddTask()
        {
            Console.Write("Enter task description: ");
            string? description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("⚠ Task description cannot be empty.");
                return;
            }

            Console.Write("Enter deadline (MM-dd-yyyy HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
            {
                Console.WriteLine("⚠ Invalid date format.");
                return;
            }

            Console.Write("Enter priority (High, Medium, Low): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
            {
                Console.WriteLine("⚠ Invalid priority.");
                return;
            }

            TodoTask newTask = new(description, deadline, priority);
            tasks.Add(newTask);
            Console.WriteLine("✅ Task added successfully!");

            SaveTasks();
            LogAction($"Task added: {newTask.Description}");
        }

        /// <summary>
        /// Views all tasks, sorted by due date.
        /// </summary>
        public void ViewTasks(bool sortByPriority = false)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("📭 No tasks available.");
                return;
            }

            var sortedTasks = sortByPriority 
                ? tasks.OrderBy(t => t.TaskPriority).ToList() 
                : tasks.OrderBy(t => t.Deadline).ToList();

            Console.WriteLine("\n=== Your Tasks ===");
            for (int i = 0; i < sortedTasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sortedTasks[i]}");
            }
        }

        /// <summary>
        /// Edits an existing task.
        /// </summary>
        public void EditTask()
        {
            ViewTasks();
            Console.Write("Enter task number to edit: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= tasks.Count)
            {
                Console.Write("Enter new description (or press Enter to keep current): ");
                string? newDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDescription))
                {
                    tasks[index - 1].Description = newDescription;
                }

                Console.Write("Enter new deadline (or press Enter to keep current): ");
                string? newDeadlineInput = Console.ReadLine();
                if (DateTime.TryParse(newDeadlineInput, out DateTime newDeadline))
                {
                    tasks[index - 1].Deadline = newDeadline;
                }

                Console.Write("Enter new priority (High, Medium, Low, or press Enter to keep current): ");
                string? newPriorityInput = Console.ReadLine();
                if (Enum.TryParse(newPriorityInput, true, out Priority newPriority))
                {
                    tasks[index - 1].TaskPriority = newPriority;
                }

                SaveTasks();
                Console.WriteLine("✅ Task updated!");
                LogAction($"Task edited: {tasks[index - 1].Description}");
            }
            else
            {
                Console.WriteLine("⚠ Invalid selection.");
            }
        }

        /// <summary>
        /// Saves tasks to a JSON file.
        /// </summary>
        private void SaveTasks()
        {
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(tasks, Formatting.Indented));
        }

        /// <summary>
        /// Loads tasks from a JSON file.
        /// </summary>
        private void LoadTasks()
        {
            if (File.Exists(FilePath))
            {
                tasks.AddRange(JsonConvert.DeserializeObject<List<TodoTask>>(File.ReadAllText(FilePath)) ?? new List<TodoTask>());
            }
        }

        /// <summary>
        /// Logs actions to a file for debugging and tracking.
        /// </summary>
        private void LogAction(string action)
        {
            File.AppendAllText(LogFile, $"{DateTime.Now}: {action}{Environment.NewLine}");
        }
    }
}
