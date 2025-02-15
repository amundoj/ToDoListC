using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoListApp
{
    /// <summary>
    /// Manages the list of tasks, handling operations like adding, removing, and updating tasks.
    /// </summary>
    public class TaskManager
    {
        private readonly List<TodoTask> tasks = new();

        /// <summary>
        /// Adds a new task to the list.
        /// </summary>
        public void AddTask()
        {
            Console.Write("Enter the task description: ");
            string? description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("⚠ Task description cannot be empty. Please try again.");
                return;
            }

            Console.Write("Enter the deadline date (MM-dd-yyyy): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                Console.WriteLine("⚠ Invalid date format. Please try again.");
                return;
            }

            Console.Write("Enter the deadline time (HH:mm): ");
            if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan time))
            {
                Console.WriteLine("⚠ Invalid time format. Please try again.");
                return;
            }

            Console.Write("Enter the priority (High, Medium, Low): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
            {
                Console.WriteLine("⚠ Invalid priority. Please try again.");
                return;
            }

            TodoTask newTask = new(description, dueDate.Date.Add(time), priority);
            tasks.Add(newTask);
            Console.WriteLine("✅ Task added successfully!");
        }

        /// <summary>
        /// Displays all tasks in the list.
        /// </summary>
        public void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("📭 No tasks available.");
                return;
            }

            Console.WriteLine("\n=== Your Tasks ===");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        /// <summary>
        /// Marks a task as completed.
        /// </summary>
        public void MarkTaskAsCompleted()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("📭 No tasks available to mark as completed.");
                return;
            }

            ViewTasks();
            Console.Write("Enter the task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks[taskNumber - 1].IsCompleted = true;
                Console.WriteLine("✅ Task marked as completed!");
            }
            else
            {
                Console.WriteLine("⚠ Invalid task number. Please try again.");
            }
        }

        /// <summary>
        /// Removes a specified task from the list.
        /// </summary>
        public void RemoveTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("📭 No tasks available to remove.");
                return;
            }

            ViewTasks();
            Console.Write("Enter the task number to remove: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks.RemoveAt(taskNumber - 1);
                Console.WriteLine("🗑 Task removed!");
            }
            else
            {
                Console.WriteLine("⚠ Invalid task number. Please try again.");
            }
        }
    }
}
