# Library Management System

This project is a simple Library Management System built using C#, .NET, and NHibernate. It includes functionalities for managing books, their details and states via REST API.

## Features

- Add, update, delete, and retrieve books
- Change book state
- Manage book details, e.g. author, title, ISBN etc

## Technologies Used

- C#
- .NET 8
- MediatR
- NHibernate
- SQLite (for persistent storage and in-memory database during testing)
- FluentValidation (for input validation)
- AutoMapper
- xUnit (for unit testing)
- Moq (for mocking dependencies in tests)
- FluentAssertions (for better assertions in tests)
- AutoMocker and AutoFixture for simless tests configuration
- State pattern for book state resolution

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio or any other C# IDE

### Installation

1. Clone the repository:
`git clone https://github.com/pLewandowskiEncke/Library.git`

2. Open the solution in Visual Studio: `Library.sln`
3. Restore the dependencies: Visual Studio should automatically restore the required NuGet packages when the solution is opened. If not, they can manually restore the packages by right-clicking on the solution in Solution Explorer and selecting "Restore NuGet Packages" or by running the following command in the terminal:

`dotnet restore`
### Configuration
To make sure the SQLite database is created, in `NHibernateHelper` file uncomment the following line:

`.ExposeConfiguration(cfg => new SchemaExport(cfg).Create(false, true))`

Comment it back after the database is created to avoid recreating it and losing data.

## Project Structure

- `Library.API`: Contains the API controllers.
- `Library.Application`: Contains the application layer, including DTOs, queries, and command handlers.
- `Library.Domain`: Contains the domain entities and interfaces.
- `Library.Infrastructure`: Contains the data access layer, including NHibernate configurations and mappings.
- `Library.Shared`: Contains shared resources, including custom exception types.
