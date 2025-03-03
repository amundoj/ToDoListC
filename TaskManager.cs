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
            _context.Database.EnsureCreated(); // Oppretter databasen hvis den ikke finnes
        }

        // Hjelpemetode for å formatere frist på en brukervennlig måte
        private static string FormatDeadline(DateTime deadline)
        {
            var timeLeft = deadline - DateTime.Now;
            if (timeLeft.TotalMinutes < 0) return "Forfalt!";
            if (timeLeft.TotalDays > 1) return $"{(int)timeLeft.TotalDays} dager igjen";
            if (timeLeft.TotalHours > 1) return $"{(int)timeLeft.TotalHours} timer igjen";
            return $"Om {(int)timeLeft.TotalMinutes} minutter";
        }

        // Legg til oppgave
        public void AddTask()
        {
            try
            {
                Console.Write("Skriv inn oppgavebeskrivelse: ");
                string? description = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(description))
                {
                    Console.WriteLine("⚠ Beskrivelsen kan ikke være tom.");
                    return;
                }

                Console.Write("Skriv inn frist (dd.MM.yyyy HH:mm, f.eks. 05.03.2025 14:00): ");
                if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime deadline))
                {
                    Console.WriteLine("⚠ Ugyldig datoformat. Bruk dd.MM.yyyy HH:mm.");
                    return;
                }

                Console.Write("Skriv inn prioritet (High, Medium, Low): ");
                if (!Enum.TryParse(Console.ReadLine()?.Trim(), true, out Priority priority) || !Enum.IsDefined(typeof(Priority), priority))
                {
                    Console.WriteLine("⚠ Ugyldig prioritet. Bruk: High, Medium, Low.");
                    return;
                }

                var newTask = new TodoTask
                {
                    Description = description,
                    Deadline = deadline,
                    TaskPriority = priority,
                    IsCompleted = false
                };

                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                Console.WriteLine("✅ Oppgave lagt til!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Feil ved adding av oppgave: {ex.Message}");
            }
        }

        // Vis oppgaver
        public void ViewTasks(bool showCompleted = false)
        {
            try
            {
                var tasks = _context.Tasks
                    .Where(t => showCompleted || !t.IsCompleted) // Filtrer basert på fullført-status
                    .OrderBy(t => t.Deadline)
                    .ToList();

                if (!tasks.Any())
                {
                    Console.WriteLine(showCompleted ? "📭 Ingen fullførte oppgaver." : "📭 Ingen ventende oppgaver.");
                    return;
                }

                Console.WriteLine("\n=== Dine oppgaver ===");
                foreach (var task in tasks)
                {
                    ConsoleColor color = task.TaskPriority switch
                    {
                        Priority.High => ConsoleColor.Red,
                        Priority.Medium => ConsoleColor.Yellow,
                        Priority.Low => ConsoleColor.Green,
                        _ => ConsoleColor.White
                    };
                    Console.ForegroundColor = color;
                    Console.WriteLine($"{task.Id}. {task.Description} (Frist: {FormatDeadline(task.Deadline)}, Prioritet: {task.TaskPriority}) - {(task.IsCompleted ? "✅ Fullført" : "⏳ Ventende")}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Feil ved henting av oppgaver: {ex.Message}");
            }
        }

        // Rediger oppgave
        public void EditTask()
        {
            var tasks = _context.Tasks.ToList();
            if (!tasks.Any())
            {
                Console.WriteLine("⚠ Ingen oppgaver å redigere.");
                return;
            }

            ViewTasks();
            Console.Write("Skriv inn oppgavenummer å redigere: ");
            if (!int.TryParse(Console.ReadLine(), out int taskId) || taskId <= 0)
            {
                Console.WriteLine("⚠ Ugyldig oppgavenummer.");
                return;
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task == null)
            {
                Console.WriteLine("⚠ Oppgaven ble ikke funnet.");
                return;
            }

            Console.Write($"Ny beskrivelse (nåværende: {task.Description}, trykk Enter for å beholde): ");
            string? newDescription = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(newDescription)) task.Description = newDescription;

            Console.Write($"Ny frist (nåværende: {task.Deadline:dd.MM.yyyy HH:mm}, trykk Enter for å beholde): ");
            string? newDeadlineInput = Console.ReadLine();
            if (DateTime.TryParseExact(newDeadlineInput, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime newDeadline))
            {
                task.Deadline = newDeadline;
            }

            Console.Write($"Ny prioritet (nåværende: {task.TaskPriority}, trykk Enter for å beholde): ");
            string? newPriorityInput = Console.ReadLine()?.Trim();
            if (Enum.TryParse(newPriorityInput, true, out Priority newPriority) && Enum.IsDefined(typeof(Priority), newPriority))
            {
                task.TaskPriority = newPriority;
            }

            _context.SaveChanges();
            Console.WriteLine("✅ Oppgave oppdatert!");
        }

        // Marker som fullført
        public void MarkTaskAsCompleted()
        {
            var tasks = _context.Tasks.Where(t => !t.IsCompleted).ToList();
            if (!tasks.Any())
            {
                Console.WriteLine("⚠ Ingen ventende oppgaver å markere som fullført.");
                return;
            }

            ViewTasks();
            Console.Write("Skriv inn oppgavenummer å markere som fullført: ");
            if (!int.TryParse(Console.ReadLine(), out int taskId) || taskId <= 0)
            {
                Console.WriteLine("⚠ Ugyldig oppgavenummer.");
                return;
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null && !task.IsCompleted)
            {
                task.IsCompleted = true;
                _context.SaveChanges();
                Console.WriteLine("✅ Oppgave markert som fullført!");
            }
            else
            {
                Console.WriteLine("⚠ Oppgaven ble ikke funnet eller er allerede fullført.");
            }
        }

        // Slett oppgave
        public void DeleteTask()
        {
            var tasks = _context.Tasks.ToList();
            if (!tasks.Any())
            {
                Console.WriteLine("⚠ Ingen oppgaver å slette.");
                return;
            }

            ViewTasks();
            Console.Write("Skriv inn oppgavenummer å slette: ");
            if (!int.TryParse(Console.ReadLine(), out int taskId) || taskId <= 0)
            {
                Console.WriteLine("⚠ Ugyldig oppgavenummer.");
                return;
            }

            var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                Console.Write($"Er du sikker på at du vil slette '{task.Description}'? (ja/nei): ");
                if (Console.ReadLine()?.Trim().ToLower() == "ja")
                {
                    _context.Tasks.Remove(task);
                    _context.SaveChanges();
                    Console.WriteLine("🗑 Oppgave slettet!");
                }
                else
                {
                    Console.WriteLine("ℹ Sletting avbrutt.");
                }
            }
            else
            {
                Console.WriteLine("⚠ Oppgaven ble ikke funnet.");
            }
        }
    }
}