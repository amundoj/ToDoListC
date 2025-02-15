using System;

namespace ToDoListApp
{
    /// <summary>
    /// Represents a to-do task with a description, deadline, priority, and completion status.
    /// </summary>
    public class TodoTask
    {
        public int Id { get; set; } // Primary Key

        public required string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Deadline { get; set; }
        public Priority TaskPriority { get; set; }

        // Parameterless constructor required by EF Core
        public TodoTask() { }

        public TodoTask(string description, DateTime deadline, Priority priority)
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            IsCompleted = false;
            Deadline = deadline;
            TaskPriority = priority;
        }

        public override string ToString()
        {
            return $"{Description} (Due: {Deadline:MM-dd-yyyy HH:mm}, Priority: {TaskPriority}) - {(IsCompleted ? "✅ Completed" : "⏳ Pending")}";
        }
    }

    /// <summary>
    /// Defines priority levels for tasks.
    /// </summary>
    public enum Priority
    {
        High,
        Medium,
        Low
    }
}
