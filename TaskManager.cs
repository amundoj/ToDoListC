using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApp
{
    public class TaskManager
    {
        private readonly AppDbContext _context;

        public TaskManager()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated(); // Creates the DB if it doesn't exist
        }

        // Add task
        public void AddTask()
        {
            try
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
                    Console.WriteLine("⚠ Invalid date format. Please use MM-dd-yyyy HH:mm.");
                    return;
                }

                Console.Write("Enter priority (High, Medium, Low): ");
                if (!Enum.TryParse(Console.ReadLine(), true, out Priority priority))
                {
                    Console.WriteLine("⚠ Invalid priority. Valid values are: High, Medium, Low.");
                    return;
                }

                // Create new task with all the required properties set
                var newTask = new TodoTask
                {
                    Description = description,
                    Deadline = deadline,
                    TaskPriority = priority,
                    IsCompleted = false // Default value
                };

                _context.Tasks.Add(newTask);
                _context.SaveChanges(); // Save to the database

                Console.WriteLine("✅ Task added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ An error occurred while adding the task: {ex.Message}");
            }
        }

        // View all tasks
        // View all tasks
public void ViewTasks()
{
    try
    {
        var tasks = _context.Tasks.OrderBy(t => t.Deadline).ToList();

        if (tasks.Count == 0)
        {
            Console.WriteLine("📭 No tasks available.");
            return;
        }

        Console.WriteLine("\n=== Your Tasks ===");
        foreach (var task in tasks)
        {
            Console.WriteLine($"{task.Id}. {task}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ An error occurred while retrieving tasks: {ex.Message}");
    }
}


        // Edit task
        // Edit task
public void EditTask()
{
    // Check if there are any tasks to edit
    var tasks = _context.Tasks.ToList();
    if (tasks.Count == 0)
    {
        Console.WriteLine("⚠ There are no tasks to edit.");
        return;
    }

    ViewTasks();
    Console.Write("Enter task number to edit: ");
    if (int.TryParse(Console.ReadLine(), out int taskId) && taskId > 0)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            Console.Write("Enter new description (or press Enter to keep current): ");
            string? newDescription = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                task.Description = newDescription;
            }

            Console.Write("Enter new deadline (or press Enter to keep current): ");
            string? newDeadlineInput = Console.ReadLine();
            if (DateTime.TryParse(newDeadlineInput, out DateTime newDeadline))
            {
                task.Deadline = newDeadline;
            }

            Console.Write("Enter new priority (High, Medium, Low, or press Enter to keep current): ");
            string? newPriorityInput = Console.ReadLine();
            if (Enum.TryParse(newPriorityInput, true, out Priority newPriority))
            {
                task.TaskPriority = newPriority;
            }

            _context.SaveChanges();
            Console.WriteLine("✅ Task updated!");
        }
        else
        {
            Console.WriteLine("⚠ Invalid task selection.");
        }
    }
}


        // Mark task as completed
        public void MarkTaskAsCompleted()
        {
            ViewTasks();
            Console.Write("Enter task number to mark as completed: ");
            if (int.TryParse(Console.ReadLine(), out int taskNumber) && taskNumber > 0)
            {
                var task = _context.Tasks.FirstOrDefault(t => t.Id == taskNumber);
                if (task != null)
                {
                    task.IsCompleted = true;
                    _context.SaveChanges();
                    Console.WriteLine("✅ Task marked as completed!");
                }
                else
                {
                    Console.WriteLine("⚠ Invalid task number.");
                }
            }
        }

        // Delete task
        public void DeleteTask()
        {
    // Check if there are any tasks to delete
    var tasks = _context.Tasks.ToList();
    if (tasks.Count == 0)
    {
        Console.WriteLine("⚠ No tasks available to delete.");
        return;
    }

    ViewTasks();
    Console.Write("Enter task number to delete: ");
    if (int.TryParse(Console.ReadLine(), out int taskId) && taskId > 0)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        if (task != null)
        {
            _context.Tasks.Remove(task); // Remove task from the context
            _context.SaveChanges(); // Save changes to the database
            Console.WriteLine("🗑 Task deleted successfully!");
        }
        else
        {
            Console.WriteLine("⚠ Invalid task selection.");
        }
    }
}
}
}
