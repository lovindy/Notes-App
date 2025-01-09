# NotesApp - ASP.NET Core Clean Architecture

A notes application built with ASP.NET Core following clean architecture principles and industry best practices.

## Architecture Overview

![Architecture Diagram](https://raw.githubusercontent.com/lovindy/Notes-App/32fdd4fd26740e679139fc45f077b579fe6f65b7/Design%20Pattern.png)

The application follows the principles of Clean Architecture, with a clear separation of concerns between layers.

## Project Structure

```
NotesApp/
├── Controllers/         # API Controllers
├── DTOs/               # Data Transfer Objects
├── Models/             # Domain Models
├── Repositories/       # Data Access Layer
├── Services/           # Business Logic
└── Interfaces/         # Contracts for Services & Repositories
```

## Layer Responsibilities

### Controllers Layer
- Entry point for HTTP requests
- Input validation
- Route handling
- Converting between DTOs and domain models
- Error handling and HTTP responses

### DTOs (Data Transfer Objects)
- Define data contracts for API requests/responses
- Separate API models from domain models
- Handle data validation
- Located in both Controller and Service layers

### Services Layer
- Contains business logic
- Orchestrates between controllers and repositories
- Handles domain operations
- Implements business rules and validations
- Uses DTOs for input/output
- Communicates with repositories using domain models

### Repositories Layer
- Data access layer
- CRUD operations
- Works with domain models
- Database interactions
- Uses Dapper for efficient data access

### Models Layer
- Domain entities
- Business objects
- Database mappings
- Core business rules

## Data Flow

1. **Client Request Flow**
   ```
   Client Request → Controller (DTO) → Service (DTO ↔ Model) → Repository (Model) → Database
   ```

2. **Response Flow**
   ```
   Database → Repository (Model) → Service (Model → DTO) → Controller → Client Response
   ```

## Authentication Flow

The application uses JWT (JSON Web Token) authentication:

1. User submits credentials
2. Server validates and generates JWT
3. JWT is stored in HTTP-only cookie
4. Subsequent requests are authenticated via JWT
5. Protected endpoints use `[Authorize]` attribute

## Key Features

- Clean Architecture implementation
- JWT Authentication
- Cookie-based token storage
- Dapper for data access
- DTOs for API contracts
- Repository pattern
- Dependency Injection
- Interface-based design

## Technologies Used

- ASP.NET Core 7.0
- Dapper
- SQL Server
- JWT for Authentication
- System.IdentityModel.Tokens.Jwt

## Setup and Configuration

1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run database migrations
4. Start the application

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your_Connection_String_Here"
  },
  "Jwt": {
    "Key": "Your_Secret_Key_Here",
    "Issuer": "Your_Issuer",
    "Audience": "Your_Audience"
  }
}
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - User login
- `POST /api/auth/logout` - User logout
- `GET /api/auth/check` - Check authentication status

### Notes
- `GET /api/notes` - Get all notes
- `POST /api/notes` - Create new note
- `PUT /api/notes/{id}` - Update note
- `DELETE /api/notes/{id}` - Delete note

## Security Considerations

- JWT stored in HTTP-only cookies
- Password hashing using secure algorithms
- HTTPS required for production
- CSRF protection
- XSS prevention through proper encoding
- Input validation using DTOs

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
