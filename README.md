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

PizzaStore/
├── Auth/                  # Authentication and Authorization
├── Models/               # Domain entities
├── Persistence/         # Database context and configurations
├── Services/           # Business logic and interfaces
└── Program.cs         # Application entry point and configuration

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- PostgreSQL
- Visual Studio 2022 or VS Code

### Local Setup

1. Clone the repository:
git clone https://github.com/LuTheZy/PizzaStore.git
cd PizzaStore

2. Update database connection string in `appsettings.json`:
{
  "ConnectionStrings": {
    "PizzeriaDbConnection": "Host=localhost;Database=pizzeria;Username=your_username;Password=your_password"
  }
}

3. Run database migrations:
dotnet ef database update

4. Start the application:
dotnet run

The API will be available at `https://localhost:5001` (or your configured port).

## API Endpoints

### Authentication
- **POST /login** - Obtain JWT token
- **POST /register** - Register a new user
- **POST /forgot-password** - Request password reset

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
Authorization: Bearer your-token-here

## JWT Authentication

The API uses JWT (JSON Web Tokens) for securing endpoints. The `JwtAuthenticationHandler` middleware is responsible for validating the JWT token in the `Authorization` header of incoming requests. If the token is valid, the user's claims are extracted and set in the `HttpContext.User` property.

### Token Validation

The `JwtAuthenticationHandler` extracts the token from the `Authorization` header, validates it using the `IJwtService`, and sets the authenticated user's claims in the `HttpContext.User` property.

### Registering a New User

To register a new user, call the `/register` endpoint with the required user details. Upon successful registration, the new user's information will be returned.

### Password Reset

To request a password reset, call the `/forgot-password` endpoint with the user's email. An email with password reset instructions will be sent to the user.

## Database Migrations

To create a new migration:
dotnet ef migrations add MigrationName

To update database:
dotnet ef database update

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/YourFeature`)
3. Commit your changes (`git commit -m 'Add some feature'`)
4. Push to the branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details
