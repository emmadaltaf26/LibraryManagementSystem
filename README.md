# Library Management System API

A .NET 9.0 Web API for Library Management using Clean Architecture, CQRS pattern, Entity Framework Core, and Repository Pattern.

## Technology Stack

- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core 9.0
- SQL Server
- MediatR (CQRS Pattern)
- AutoMapper
- FluentValidation
- Swagger/OpenAPI

## Project Structure

```
LibraryManagementSystem/
├── src/
│   ├── Library.API/                 # Presentation Layer
│   │   ├── Controllers/             # API Controllers
│   │   ├── Application/
│   │   │   ├── Commands/            # Write Operations
│   │   │   ├── Queries/             # Read Operations
│   │   │   └── Behaviours/          # Pipeline Behaviors
│   │   └── Infrastructure/
│   │       └── Extensions/          # Service Extensions
│   │
│   ├── Library.Application/         # Application Layer
│   │   ├── Common/
│   │   │   ├── Interfaces/          # Repository Interfaces
│   │   │   └── Mappings/            # AutoMapper Profiles
│   │   └── DTOs/                    # Data Transfer Objects
│   │
│   ├── Library.Domain/              # Domain Layer
│   │   ├── Entities/                # Domain Entities
│   │   └── Common/                  # Base Classes
│   │
│   └── Library.Infrastructure/      # Infrastructure Layer
│       ├── Data/
│       │   └── Configurations/      # Entity Configurations
│       ├── Repositories/            # Repository Implementations
│       └── Migrations/              # EF Core Migrations
│
└── LibraryManagementSystem.sln      # Solution File
```

## Domain Entities

| Entity | Description |
|--------|-------------|
| Book | Represents a book with title, ISBN, author, genre, and availability |
| Author | Represents an author with name and biography |
| Genre | Represents a book category/genre |
| Member | Represents a library member who can borrow books |
| Loan | Represents a book borrowing record |

## Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd LibraryManagementSystem
```

### 2. Configure Connection String

Update the connection string in `src/Library.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "LibraryDbConnection": "Server=localhost;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True"
  }
}
```

### 3. Run Database Migrations

Open Package Manager Console in Visual Studio and run:

```powershell
Update-Database -Project Library.Infrastructure -StartupProject Library.API -Context LibraryDbContext
```

Or using .NET CLI:

```bash
dotnet ef database update --project src/Library.Infrastructure --startup-project src/Library.API --context LibraryDbContext
```

### 4. Run the Application

```bash
cd src/Library.API
dotnet run
```

Or press F5 in Visual Studio.

The API will be available at: `https://localhost:7xxx` or `http://localhost:5xxx`

Swagger UI will open automatically for API testing.

## API Endpoints

### Books

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/books | Get all books |
| GET | /api/books/{id} | Get book by ID |
| POST | /api/books | Create a new book |
| PUT | /api/books | Update a book |
| DELETE | /api/books/{id} | Delete a book |

### Authors

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/authors | Get all authors |
| GET | /api/authors/{id} | Get author by ID |
| POST | /api/authors | Create a new author |
| PUT | /api/authors | Update an author |
| DELETE | /api/authors/{id} | Delete an author |

### Genres

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/genres | Get all genres |
| GET | /api/genres/{id} | Get genre by ID |
| POST | /api/genres | Create a new genre |
| PUT | /api/genres | Update a genre |
| DELETE | /api/genres/{id} | Delete a genre |

### Members

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/members | Get all members |
| GET | /api/members/{id} | Get member by ID |
| POST | /api/members | Create a new member |
| PUT | /api/members | Update a member |
| DELETE | /api/members/{id} | Delete a member |

### Loans

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/loans | Get all loans |
| GET | /api/loans/{id} | Get loan by ID |
| GET | /api/loans/member/{memberId} | Get loans by member |
| GET | /api/loans/overdue | Get overdue loans |
| POST | /api/loans/borrow | Borrow a book |
| POST | /api/loans/{id}/return | Return a book |

## Sample API Requests

### Create an Author
```json
POST /api/authors
{
  "name": "George Orwell",
  "biography": "English novelist and essayist"
}
```

### Create a Genre
```json
POST /api/genres
{
  "name": "Fiction",
  "description": "Literary works of imagination"
}
```

### Create a Book
```json
POST /api/books
{
  "title": "1984",
  "isbn": "978-0451524935",
  "description": "Dystopian social science fiction novel",
  "publishedYear": 1949,
  "totalCopies": 5,
  "authorId": "author-guid-here",
  "genreId": "genre-guid-here"
}
```

### Create a Member
```json
POST /api/members
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@email.com",
  "phoneNumber": "123-456-7890",
  "address": "123 Main St"
}
```

### Borrow a Book
```json
POST /api/loans/borrow
{
  "bookId": "book-guid-here",
  "memberId": "member-guid-here",
  "dueDate": "2025-01-15"
}
```

### Return a Book
```json
POST /api/loans/{loanId}/return
{
  "loanId": "loan-guid-here"
}
```

## Architecture Overview

### Clean Architecture Layers

1. **Domain Layer** - Core business entities and logic (no dependencies)
2. **Application Layer** - DTOs, interfaces, mappings (depends on Domain)
3. **Infrastructure Layer** - Data access, repositories (depends on Domain & Application)
4. **API Layer** - REST endpoints, CQRS handlers (depends on all layers)

### CQRS Pattern

- **Commands** - Write operations (Create, Update, Delete)
- **Queries** - Read operations (Get, List, Search)
- **Handlers** - Process commands and queries via MediatR

### Validation

FluentValidation is used for request validation with automatic pipeline integration via MediatR behaviors.

## Features

- Full CRUD operations for Books, Authors, Genres, and Members
- Loan management with borrow and return functionality
- Overdue loan tracking
- Member loan history
- Input validation using FluentValidation
- Clean separation of concerns
- Repository pattern with Unit of Work
- Swagger documentation for API testing

## Author

Emmad Altaf - emmadaltaf26@gmail.com
