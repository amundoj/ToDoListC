using System;
using System.Collections.Generic;

namespace ToDoListApp
{
    enum Priority
    {
        High,
        Medium,
        Low
    }
    // Task class to represent each to-do item
    class Task
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public Priority TaskPriority { get; set;}

        public Task(string description, DateTime dueDate, Priority priority)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsCompleted = false;
            DueDate = dueDate;
            TaskPriority = priority;
        }

        public override string ToString()
        {
            return $"{Description} (Due: {DueDate.ToShortDateString()}, Priority {TaskPriority}) - {(IsCompleted ? "Completed" : "Pending")}";
        }
    }

    class Program
    {
        static List<Task> tasks = new List<Task>();

        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();
                string? choice = Console.ReadLine();

                if (choice == null)
                {
                    Console.WriteLine("Invalid choice, please try again.");
                    continue;
                }

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
                        return; // Exit the application
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("To-Do List Application");
            Console.WriteLine("1. Add a new task");
            Console.WriteLine("2. View all tasks");
            Console.WriteLine("3. Mark a task as completed");
            Console.WriteLine("4. Remove a task");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
        }

        static void AddTask()
        {
            Console.Write("Enter the task description: ");
            string? description = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Task description cannot be empty. Please try again.");
                return;
            }

            Console.WriteLine("Enter the du date (mm-dd-yyyy)");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                Console.WriteLine("Invalid date format. Please try again.");
                return;
            }

            Console.WriteLine("Enter the priority (High, Medium, Low): ");
            if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
            {
                Console.WriteLine("Invalid priority. Please try again");
                return;
            }

            Task newTask = new Task(description, dueDate, priority);
            tasks.Add(newTask);
            Console.WriteLine("Task added successfully.");
        }

        static void ViewTasks()
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
                return;
            }

            Console.WriteLine("Your Tasks:");
            for (int i = 0; i < tasks.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {tasks[i]}");
            }
        }

        static void MarkTaskAsCompleted()
        {
            ViewTasks();
            if (tasks.Count == 0) return;

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

        static void RemoveTask()
        {
            ViewTasks();
            if (tasks.Count == 0) return;

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
