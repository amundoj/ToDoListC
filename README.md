# To-Do List Application

A simple command-line To-Do List application written in C#. This application allows you to:

- Add tasks
- View tasks
- Edit tasks
- Delete tasks

## Database Setup

This application uses a MySQL database to store the tasks.

### Requirements

- MySQL database running locally or on a remote server.
- .NET SDK (version 6 or above).

### Database Configuration

The connection string for the database is configured using an environment variable. Make sure to set up the environment variable `DB_PASSWORD` on your system with the correct MySQL password.

#### Setting the Environment Variable

**Windows**:
1. Open PowerShell or Command Prompt.
2. Run the following:
   ```bash
   $env:DB_PASSWORD="your_pword"

**Linux/macOS:**

1. Open a terminal and add the following to your shell configuration file (~/.bashrc or ~/.zshrc):
    export DB_PASSWORD="your_password"

2. Reload the shell:
    source ~/.bashrc   # Or ~/.zshrc

### Migrations

After setting up your database, run the following commands to create the required tables:

dotnet ef migrations add InitialCreate
dotnet ef database update

### Running the Application

Clone this repository.
Set up the environment variable for the database password.
Run the application:
dotnet run