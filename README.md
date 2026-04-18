# AccountsHub

AccountsHub is a microservices-based application for managing customer accounts and user authentication. It consists of separate APIs for customer management and user authentication, built with .NET, Entity Framework, and Docker support.

## Architecture

The project is structured as follows:

- **CustomerApi**: RESTful API for customer management using ASP.NET Core Web API with controllers, services, and DTOs.
- **UserAuthApi**: Minimal API for user authentication and authorization using JWT tokens.
- **CustomerEntities**: Domain entities and database context for customer data.
- **UserAuthEntities**: Domain entities and database context for user authentication data.
- **Infra**: Shared infrastructure components, including common services, HTTP resolvers, and notifications.
- **InfraEntities**: Base entities with auditing and soft delete capabilities.
- **UserAuthMigrations**: Database migrations for user auth.
- **Tests**: Unit tests for each component.

The application uses SQL Server for data persistence and supports Docker containerization.

## Prerequisites

- .NET 8.0 or later
- Docker and Docker Compose
- SQL Server (or use the provided Docker setup)

## Setup

1. Clone the repository:
   ```
   git clone <repository-url>
   cd AccountsHub
   ```

2. Restore dependencies:
   ```
   dotnet restore Solutions/AccountsHub.sln
   ```

3. Set up the database:
   - For local SQL Server, update connection strings in `appsettings.json` files.
   - Or use Docker Compose for a containerized setup:
     ```
     docker-compose up -d
     ```

## Building

Build the solution:
```
dotnet build Solutions/AccountsHub.sln
```

Or use the VS Code task:
- Run the "build" task.

## Running

### Locally
Run the APIs:
```
dotnet run --project src/CustomerApi/CustomerApi.csproj
dotnet run --project src/UserAuthApi/UserAuthApi.csproj
```

### With Docker
Build and run using Docker Compose:
```
docker-compose -f docker-compose.yml up --build
```

For debugging:
```
docker-compose -f docker-compose.debug.yml up --build
```

Or use VS Code tasks: "docker-run: debug" or "docker-run: release".

## Testing

Run unit tests:
```
dotnet test
```

Or use VS Code to run tests in the test projects.

API endpoints can be tested using the provided `.http` files, e.g., `UserAuthApi.http`.

## API Documentation

- **CustomerApi**: RESTful endpoints for customer operations. See controllers in `src/CustomerApi/Controllers/`.
- **UserAuthApi**: Minimal API endpoints for auth. See `src/UserAuthApi/Endpoints/`.

Use Swagger or the `.http` files for testing.

## Contributing

Follow these guidelines:

- **CustomerApi**: Use ApiController, async endpoints, REST conventions, ProblemDetails for errors, delegate to services, FluentValidation, thin controllers.
- **UserAuthApi**: Minimal API style, route grouping, JWT auth, ProblemDetails, delegate to services, DataAnnotations.
- **Entities**: Plain POCOs, fluent API for config, no business logic in entities.
- Write unit tests for services and endpoints.
- Use the provided configurations and namespaces.

## License

[Specify license if applicable]

