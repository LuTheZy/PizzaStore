# Pizza Store API

A modern, lightweight Pizza Store API built with .NET 8 using minimal API patterns and clean architecture principles.

## Tech Stack

- **.NET 8.0**
- **PostgreSQL** - Database
- **Entity Framework Core** - ORM
- **Custom JWT Authentication** - Security
- **Swagger/OpenAPI** - API Documentation
- **Minimal API Pattern** - API Architecture

## Features

- **Custom JWT Authentication** - Secure endpoint access
- **Entity Framework Core with PostgreSQL** - Robust data persistence
- **Repository Pattern** - Clean separation of data access logic
- **SOLID Principles** - Maintainable and testable code
- **Swagger Documentation** - Interactive API documentation
- **Pizza CRUD Operations** - Full management of pizza entities

## Project Structure

```
PizzaStore/
├── Auth/                  # Authentication and Authorization
├── Models/               # Domain entities
├── Persistence/         # Database context and configurations
├── Services/           # Business logic and interfaces
└── Program.cs         # Application entry point and configuration
```

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 or VS Code

### Local Setup

1. Clone the repository:
```bash
git clone https://github.com/LuTheZy/PizzaStore.git
cd PizzaStore
```

2. Update database connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "PizzeriaDbConnection": "Host=localhost;Database=pizzeria;Username=your_username;Password=your_password"
  }
}
```

3. Run database migrations:
```bash
dotnet ef database update
```

4. Start the application:
```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or your configured port).

## API Endpoints

### Authentication
- **POST /login** - Obtain JWT token

### Pizza Operations
All endpoints require JWT authentication
- **GET /pizzas** - List all pizzas
- **GET /pizzas/{id}** - Get specific pizza
- **POST /pizzas** - Create new pizza
- **PUT /pizzas** - Update existing pizza
- **DELETE /pizzas/{id}** - Delete pizza

## Authentication

1. Call the login endpoint with credentials
2. Use the returned JWT token in subsequent requests:
```
Authorization: Bearer your-token-here
```

## Database Migrations

To create a new migration:
```bash
dotnet ef migrations add MigrationName
```

To update database:
```bash
dotnet ef database update
```

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details
