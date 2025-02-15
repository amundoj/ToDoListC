using System;
using System.Collections.Generic;

namespace ToDoListApp
{
    // Define task priority levels
    enum Priority
    {
        High,
        Medium,
        Low
    }

    // Renamed from "Task" to "TodoTask" to avoid confusion with System.Threading.Tasks.Task
    class TodoTask
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Deadline { get; set; }
        public Priority TaskPriority { get; set; }

        public TodoTask(string description, DateTime deadline, Priority priority)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsCompleted = false;
            Deadline = deadline;
            TaskPriority = priority;
        }

        public override string ToString()
        {
            return $"{Description} (Due: {Deadline:MM-dd-yyyy HH:mm}, Priority: {TaskPriority}) - {(IsCompleted ? "Completed" : "Pending")}";
        }
    }

    class Program
    {
        // Use a readonly list to store tasks
        private static readonly List<TodoTask> tasks = new List<TodoTask>();

        static void Main(string[] args)
        {
            bool exitRequested = false;
            while (!exitRequested)
            {
                ShowMenu();
                string? choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddTask();
                        break;
                    case "2":
                        ViewTasks();
                        break;
                    case "3":
                        MarkTaskAsCompleted();
                        break;
                    case "4":
                        RemoveTask();
                        break;
                    case "5":
                        exitRequested = true;
                        Console.WriteLine("Exiting application. Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
                Console.WriteLine(); // Add a blank line for better readability between actions.
            }
        }

        /// <summary>
        /// Displays the main menu.
        /// </summary>
        private static void ShowMenu()
        {
            Console.WriteLine("=== To-Do List Application ===");
            Console.WriteLine("1. Add a new task");
            Console.WriteLine("2. View all tasks");
            Console.WriteLine("3. Mark a task as completed");
            Console.WriteLine("4. Remove a task");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }

        /// <summary>
        /// Adds a new task after validating user input.
        /// </summary>
        private static void AddTask()
        {
            Console.Write("Enter the task description: ");
            string? description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Task description cannot be empty. Please try again.");
                return;
            }

            // Get and validate deadline date
            Console.Write("Enter the deadline date (MM-dd-yyyy): ");
            string? dateInput = Console.ReadLine();
            if (!DateTime.TryParse(dateInput, out DateTime dueDate))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                return;
            }

            // Get and validate deadline time
            Console.Write("Enter the deadline time (HH:mm): ");
            string? timeInput = Console.ReadLine();
            if (!TimeSpan.TryParse(timeInput, out TimeSpan time))
            {
                Console.WriteLine("Invalid time format. Please try again.");
                return;
            }
            DateTime deadline = dueDate.Date.Add(time);

            // Get and validate priority
            Console.Write("Enter the priority (High, Medium, Low): ");
            string? priorityInput = Console.ReadLine();
            if (!Enum.TryParse(priorityInput, true, out Priority priority))
            {
                Console.WriteLine("Invalid priority. Please try again.");
                return;
            }

            TodoTask newTask = new TodoTask(description, deadline, priority);
            tasks.Add(newTask);
            Console.WriteLine("Task added successfully.");
        }

        /// <summary>
        /// Displays all current tasks.
        /// </summary>
        private static void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.WriteLine("=== Your Tasks ===");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        /// <summary>
        /// Marks a specified task as completed.
        /// </summary>
        private static void MarkTaskAsCompleted()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available to mark as completed.");
                return;
            }

            ViewTasks();
            Console.Write("Enter the task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks[taskNumber - 1].IsCompleted = true;
                Console.WriteLine("Task marked as completed.");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again.");
            }
        }

        /// <summary>
        /// Removes a specified task from the list.
        /// </summary>
        private static void RemoveTask()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available to remove.");
                return;
            }

            ViewTasks();
            Console.Write("Enter the task number to remove: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0 && taskNumber <= tasks.Count)
            {
                tasks.RemoveAt(taskNumber - 1);
                Console.WriteLine("Task removed.");
            }
            else
            {
                Console.WriteLine("Invalid task number. Please try again.");
            }
        }
    }
}
