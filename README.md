# To-Do List Application

A simple command-line To-Do List application written in C#. This application allows you to:

- Add tasks with descriptions, deadlines, and priorities
- View pending or completed tasks
- Edit existing tasks
- Mark tasks as completed
- Delete tasks

## Database Setup

This application uses a MySQL database to store tasks persistently, managed via Entity Framework Core with the Pomelo MySQL provider.

### Requirements

- MySQL database running locally or on a remote server
- .NET SDK (version 6 or above)
- MySQL user with appropriate permissions to create and modify the `todolist` database

### Database Configuration

The database connection string is currently hard-coded in `AppDbContext.cs`. Update the connection string in the `OnConfiguring` method with your MySQL credentials:

optionsBuilder.UseMySql("server=localhost;database=todolist;user=root;password=YOUR_PASSWORD",
new MySqlServerVersion(new Version(8, 0, 21)));


#### Optional: Use Environment Variables

For better security, you can configure the connection string using an environment variable (`DB_PASSWORD`):

**Windows (PowerShell):**

$env:DB_PASSWORD="your_password"

**Linux/macOS (Terminal):**
Add to `~/.bashrc` or `~/.zshrc`:

export DB_PASSWORD="your_password"

Reload the shell:

source ~/.bashrc  # or ~/.zshrc

Then update `AppDbContext.cs` to use it:

var password = Environment.GetEnvironmentVariable("DB_PASSWORD");
optionsBuilder.UseMySql($"server=localhost;database=todolist;user=root;password={password}",
new MySqlServerVersion(new Version(8, 0, 21)));

### Database Initialization

The application uses `EnsureCreated()` to automatically create the `todolist` database and table on first run. Ensure your MySQL user has permissions to create databases. Alternatively, create the database manually:

CREATE DATABASE todolist;

For production use, consider switching to EF Core migrations:

dotnet ef migrations add InitialCreate
dotnet ef database update

### Running the Application

1. Clone this repository.
2. Update the MySQL connection string in `AppDbContext.cs` (or set the `DB_PASSWORD` environment variable if implemented).
3. Build and run the application:

dotnet build todolistc.csproj
dotnet run --project todolistc.csproj

## Notes

- If the folder contains multiple `.csproj` files, specify the correct one with `--project`.
- Ensure MySQL is running and accessible before starting the app.