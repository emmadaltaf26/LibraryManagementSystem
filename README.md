# Library Management System API

A .NET 9.0 Web API for Library Management using Clean Architecture, CQRS pattern, Entity Framework Core, and Repository Pattern.

## Technology Stack

- **Framework**: .NET 9.0
- **Database**: SQL Server with Entity Framework Core 9.0
- **Architecture**: Clean Architecture with CQRS
- **Patterns**: Repository Pattern, Unit of Work, MediatR
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Logging**: Serilog
- **API Documentation**: Swagger/OpenAPI

## Project Structure

```
LibraryManagementSystem/
├── src/
│   ├── Library.API/                 # Web API Layer
│   │   ├── Controllers/             # API Controllers
│   │   ├── Program.cs               # Application Entry Point
│   │   └── appsettings.json         # Configuration
│   │
│   ├── Library.Application/         # Application Layer
│   │   ├── Common/                  # Shared Components
│   │   │   ├── Behaviors/           # MediatR Pipeline Behaviors
│   │   │   ├── Interfaces/          # Abstractions
│   │   │   └── Mappings/            # AutoMapper Profiles
│   │   ├── DTOs/                    # Data Transfer Objects
│   │   └── Features/                # CQRS Commands & Queries
│   │       ├── Books/
│   │       ├── Members/
│   │       ├── Authors/
│   │       ├── Genres/
│   │       └── Loans/
│   │
│   ├── Library.Domain/              # Domain Layer
│   │   ├── Common/                  # Base Entities
│   │   ├── Entities/                # Domain Entities
│   │   └── Enums/                   # Enumerations
│   │
│   └── Library.Infrastructure/      # Infrastructure Layer
│       ├── Data/                    # DbContext & Configurations
│       └── Repositories/            # Repository Implementations
│
└── LibraryManagementSystem.sln      # Solution File
```

## Domain Entities

| Entity | Description |
|--------|-------------|
| **Book** | Represents a book with title, ISBN, author, genre, and availability |
| **Author** | Represents an author with name and biography |
| **Genre** | Represents a book category/genre |
| **Member** | Represents a library member who can borrow books |
| **Loan** | Represents a book borrowing record |

## API Endpoints

### Books
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/books` | Get all books |
| GET | `/api/books/{id}` | Get book by ID |
| POST | `/api/books` | Create a new book |
| PUT | `/api/books/{id}` | Update a book |
| DELETE | `/api/books/{id}` | Delete a book |

### Members
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/members` | Get all members |
| GET | `/api/members/{id}` | Get member by ID |
| POST | `/api/members` | Create a new member |
| PUT | `/api/members/{id}` | Update a member |
| DELETE | `/api/members/{id}` | Delete a member |

### Authors
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/authors` | Get all authors |
| GET | `/api/authors/{id}` | Get author by ID |
| POST | `/api/authors` | Create a new author |
| PUT | `/api/authors/{id}` | Update an author |
| DELETE | `/api/authors/{id}` | Delete an author |

### Genres
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/genres` | Get all genres |
| GET | `/api/genres/{id}` | Get genre by ID |
| POST | `/api/genres` | Create a new genre |
| PUT | `/api/genres/{id}` | Update a genre |
| DELETE | `/api/genres/{id}` | Delete a genre |

### Loans
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/loans` | Get all loans |
| GET | `/api/loans/{id}` | Get loan by ID |
| GET | `/api/loans/member/{memberId}` | Get loans by member |
| GET | `/api/loans/overdue` | Get overdue loans |
| POST | `/api/loans/borrow` | Borrow a book |
| POST | `/api/loans/{id}/return` | Return a book |

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022 or VS Code

### Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/emmadaltaf26/LibraryManagementSystem.git
   cd LibraryManagementSystem
   ```

2. **Update connection string** in `src/Library.API/appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Initial Catalog=LibraryManagementDb;Integrated Security=True;TrustServerCertificate=True"
   }
   ```

3. **Run the application**
   ```bash
   cd src/Library.API
   dotnet run
   ```

4. **Access Swagger UI**
   - Navigate to `https://localhost:5001` or `http://localhost:5000`

### Database Migration

The application automatically applies migrations and seeds sample data on startup.

To manually create migrations:
```bash
cd src/Library.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Library.API
dotnet ef database update --startup-project ../Library.API
```

## Architecture Overview

### Clean Architecture Layers

1. **Domain Layer** - Core business entities and logic (no dependencies)
2. **Application Layer** - Business rules, CQRS handlers, DTOs (depends on Domain)
3. **Infrastructure Layer** - Data access, external services (depends on Domain & Application)
4. **API Layer** - REST endpoints, configuration (depends on all layers)

### CQRS Pattern

- **Commands** - Write operations (Create, Update, Delete)
- **Queries** - Read operations (Get, List, Search)
- **Handlers** - Process commands and queries via MediatR

### Validation

FluentValidation is used for request validation with automatic pipeline integration via MediatR behaviors.

## Author

**Emmad Altaf**
- Email: emmadaltaf26@gmail.com
- GitHub: [@emmadaltaf26](https://github.com/emmadaltaf26)

## License

This project is licensed under the MIT License.
