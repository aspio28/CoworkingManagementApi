# Coworking Management Api

## Getting Started

Follow these steps to set up the development environment and run the system locally.

### 1. Prerequisites

* **.NET 8.0 SDK** (or higher)
* **PostgreSQL** (Running locally on port 5432)
* **EF Core Global Tool**:
  ```bash
    dotnet tool install --global dotnet-ef

### 2. Clone the repository

```bash
    git clone https://github.com/aspio28/CoworkingManagementApi.git
    cd CoworkingManagementApi
```

### 3. Database Configuration

* Create postgres database
* Locate the settings file in `src/CoworkingManagement.Api/appsettings.json`
* Update ConnectionStrings section with your local PostgreSQL credentials. It should look like this:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Port=5432;Database=CoworkingDb;Username=YOUR_USER;Password=YOUR_PASSWORD"
   }

### 4. Run the app

```bash
    # Restore dependencies
    dotnet restore

    # Build and Run
    dotnet run --project src/CoworkingManagement.Api
```

## Arquitecture

The project follows a Clean Architecture approach to separate different responsibilities into specialized layers, each designed for a specific function.

### 1. Layers

* **Infrastructure**: This layer is responsible for managing everything external to the application, such as connections to PostgreSQL, Entity Framework Core configuration, and database migrations.

* **Domain**: This layer is responsible for handling the business logic. It defines the entities, enums, and domain-specific exceptions. This layer does not depend on any other layer of the architecture.

* **Application**: This layer is responsible for connecting the API layer with the domain layer. It follows the CQRS pattern using MediatR and is in charge of receiving commands and queries, validating inputs, creating domain entities, and instructing the infrastructure layer to persist them.

* **API**: This layer is the entry point to the application. It is responsible for handling HTTP requests following RESTful standards, as well as defining the application's global middleware.

### 2. Key Patterns

* **CQRS**: To separate read and write operations in the application layer.

* **Dependency Injection**: Classes should not create the objects they need; instead, they should receive them from the outside.

### 3. Cross-Cutting Concerns

#### Pipelines Behavior de MediatR

* **ValidationBehavior**: Automatically executes FluentValidation validators to validate the objects (commands and queries) that reach the handler.
* **CachingBehavior**: Responsible for storing query data in the cache using the keys defined for each query.

### 4. Security & Authentication
* **JWT Bearer Token:** The API is secured using JSON Web Tokens.
* **Role-Based Access Control (RBAC):** Access to specific endpoints (like managing coworking rooms) is restricted based on user roles (`Admin`, `Member`).

## API Documentation & Endpoints

The API follows RESTful conventions and uses **Swagger (OpenAPI)** for interactive documentation. Once the application is running, you can access the full UI at:

`https://localhost:5074/swagger`

### Base URL
`http://localhost:5074/api`

###  Resource: Rooms (Coworking Spaces)
Endpoints related to the management of physical spaces.

| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `GET` | `/Rooms` | Get a paginated list of all rooms | Member/ Admin |
| `GET` | `/Rooms/{id}` | Get detailed information of a specific room | Member/ Admin |
| `POST` | `/Rooms` | Create a new coworking room | Admin |
| `PUT` | `/Rooms/{id}` | Update room details (capacity, price, etc.) | Admin |
| `DELETE` | `/Rooms/{id}` | Logical delete of a room | Admin |

### Resource: Reservations
Endpoints for managing user reservations.

| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `GET` | `/Reservations` | Get a paginated list off all active reservations for the logged-in user. | Member/ Admin |
| `GET` | `/Reservations/{id}` | Get detailed information of a specifica reservation | Admin |
| `POST` | `/Reservations` | Create a new reservation for a room | Member/ Admin |
| `PATCH` | `/Reservations/{id}/cancel` | Cancel an existing reservation | Member/ Admin |

### Resource: Login/ Register
Endpoints for authentication
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `POST` | `/Auth/login` | Obtain access token. | Public |
| `POST` | `/Auth/register` | Register a new user in the app | Public |
---

### Resource: Users
Endpoints for managing users
| Method | Endpoint | Description | Auth |
| :--- | :--- | :--- | :--- |
| `GET` | `/User` | Get a paginated list of all registered users. | Admin |
| `PATCH` | `/User/{id}` | Change a user's role. | Admin |


### Request & Response Example

#### Example: Create a Reservation
**POST** `http://localhost:5074/api/Reservations`

**Request Body:**
```json
{
  "roomId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "startDate": "2026-02-01T09:00:00Z",
  "endDate": "2026-02-01T17:00:00Z"
}