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
                try
                {
                    ShowMenu();
                    string? choice = Console.ReadLine()?.Trim();

                    switch (choice)
                    {
                        case "1": taskManager.AddTask(); break;
                        case "2": taskManager.ViewTasks(); break; // Viser kun ufullførte som standard
                        case "3": taskManager.ViewTasks(true); break; // Viser fullførte
                        case "4": taskManager.EditTask(); break;
                        case "5": taskManager.MarkTaskAsCompleted(); break;
                        case "6": taskManager.DeleteTask(); break;
                        case "7": exitRequested = true; Console.WriteLine("👋 Avslutter applikasjonen."); break;
                        default: Console.WriteLine("⚠ Ugyldig valg. Velg et tall mellom 1 og 7."); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ En feil oppstod: {ex.Message}");
                }
                Console.WriteLine();
            }
        }

        private static void ShowMenu()
        {
            Console.WriteLine("\n=== 📝 To-Do List Applikasjon ===");
            Console.WriteLine("1️⃣ Legg til en oppgave");
            Console.WriteLine("2️⃣ Vis ventende oppgaver");
            Console.WriteLine("3️⃣ Vis fullførte oppgaver");
            Console.WriteLine("4️⃣ Rediger en oppgave");
            Console.WriteLine("5️⃣ Marker oppgave som fullført");
            Console.WriteLine("6️⃣ Slett en oppgave");
            Console.WriteLine("7️⃣ Avslutt");
            Console.Write("Velg et alternativ: ");
        }
    }
}