# Setup Instructions

This document provides detailed step-by-step instructions to set up and run the Library Management System API.

## Prerequisites

Before running this application, ensure you have the following installed:

| Requirement | Version | Download Link |
|-------------|---------|---------------|
| .NET SDK | 9.0 or later | https://dotnet.microsoft.com/download/dotnet/9.0 |
| SQL Server | 2019 or later | https://www.microsoft.com/en-us/sql-server/sql-server-downloads |
| Visual Studio | 2022 (v17.8+) | https://visualstudio.microsoft.com/downloads/ |
| Git | Latest | https://git-scm.com/downloads |

**Note:** SQL Server LocalDB (included with Visual Studio) or SQL Server Express is sufficient for development.

## Step 1: Clone the Repository

```bash
git clone <repository-url>
cd LibraryManagementSystem
```

## Step 2: Open the Solution

1. Navigate to the project folder
2. Double-click `LibraryManagementSystem.sln` to open in Visual Studio
3. Wait for NuGet packages to restore automatically

## Step 3: Configure Connection String

Open `src/Library.API/appsettings.json` and update the connection string to match your SQL Server instance:

```json
{
  "ConnectionStrings": {
    "LibraryDbConnection": "Server=localhost;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

### Connection String Options

| SQL Server Type | Connection String |
|-----------------|-------------------|
| LocalDB | `Server=(localdb)\\mssqllocaldb;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True` |
| SQL Server Express | `Server=.\\SQLEXPRESS;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True` |
| SQL Server (Default) | `Server=localhost;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True` |
| SQL Server (With Credentials) | `Server=localhost;Initial Catalog=LibraryManagementDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True` |

## Step 4: Run Database Migrations

### Option A: Using Package Manager Console (Visual Studio)

1. Open **Tools → NuGet Package Manager → Package Manager Console**
2. Set **Default Project** to `src/Library.Infrastructure`
3. Run the following commands:

```powershell
# Create migration (only needed if migrations folder is empty)
Add-Migration InitialCreate -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext

# Apply migration to create database and tables
Update-Database -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext
```

### Option B: Using .NET CLI (Command Line)

Open terminal in the solution root folder and run:

```bash
# Create migration (only needed if migrations folder is empty)
dotnet ef migrations add InitialCreate --project src/Library.Infrastructure --startup-project src/Library.API --context LibraryDbContext

# Apply migration to create database and tables
dotnet ef database update --project src/Library.Infrastructure --startup-project src/Library.API --context LibraryDbContext
```

**Note:** If migrations already exist in the `Migrations` folder, skip the `Add-Migration` command and only run `Update-Database`.

## Step 5: Run the Application

### Option A: Using Visual Studio

1. Set `Library.API` as the startup project (Right-click → Set as Startup Project)
2. Press `F5` or click the green "Start" button
3. The browser will open automatically with Swagger UI

### Option B: Using .NET CLI

```bash
cd src/Library.API
dotnet run
```

## Step 6: Access the API

Once the application is running:

| URL | Description |
|-----|-------------|
| https://localhost:7121 | API Base URL (HTTPS) |
| http://localhost:5175 | API Base URL (HTTP) |
| https://localhost:7121/swagger | Swagger UI Documentation |

**Swagger UI** provides an interactive interface to test all API endpoints.

## Step 7: Test the API

Follow this sequence to test the complete workflow:

1. **Create an Author** (POST /api/authors)
```json
{
  "name": "George Orwell",
  "biography": "English novelist and essayist",
  "dateOfBirth": "1903-06-25"
}
```

2. **Create a Genre** (POST /api/genres)
```json
{
  "name": "Fiction",
  "description": "Literary works of imagination"
}
```

3. **Create a Book** using Author and Genre IDs from above responses (POST /api/books)
```json
{
  "title": "1984",
  "isbn": "978-0451524935",
  "description": "Dystopian social science fiction novel",
  "publishedYear": 1949,
  "totalCopies": 5,
  "authorId": "<author-id-from-step-1>",
  "genreId": "<genre-id-from-step-2>"
}
```

4. **Create a Member** (POST /api/members)
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@email.com",
  "phoneNumber": "123-456-7890",
  "address": "123 Main St"
}
```

5. **Borrow a Book** (POST /api/loans/borrow)
```json
{
  "bookId": "<book-id-from-step-3>",
  "memberId": "<member-id-from-step-4>",
  "loanDays": 14,
  "notes": "First time borrower"
}
```

6. **Return the Book** (POST /api/loans/{loan-id}/return)
```json
{
  "notes": "Returned in good condition"
}
```

7. **View All Loans** (GET /api/loans)

8. **View Overdue Loans** (GET /api/loans/overdue)

## Troubleshooting

| Issue | Solution |
|-------|----------|
| Database connection failed | Verify SQL Server is running and connection string is correct |
| Migration errors | Delete `Migrations` folder and re-run `Add-Migration` then `Update-Database` |
| Port already in use | Change ports in `Properties/launchSettings.json` or stop the conflicting application |
| NuGet restore failed | Run `dotnet restore` in the solution folder |
| EF Core tools not found | Run `dotnet tool install --global dotnet-ef` |

## Database Schema

The application creates the following tables:

| Table | Description |
|-------|-------------|
| Authors | Stores author information |
| Genres | Stores book genres/categories |
| Books | Stores book details with foreign keys to Authors and Genres |
| Members | Stores library member information |
| Loans | Stores book borrowing records with foreign keys to Books and Members |

## Additional Commands

### Generate SQL Script (without applying)

```powershell
Script-Migration -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext
```

### Remove Last Migration

```powershell
Remove-Migration -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext
```

### Drop Database

```powershell
Drop-Database -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext
```
