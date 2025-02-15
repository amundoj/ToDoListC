using Microsoft.EntityFrameworkCore;
using System;

namespace ToDoListApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoTask> Tasks { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
            options.UseMySql($"server=localhost;database=ToDoListDB;user=root;password={password}", 
                new MySqlServerVersion(new Version(8, 0, 25)));
        }
    }
}
