# School Management System API

A RESTful Web API for managing students, instructors, courses, and enrollments at a school. Built with ASP.NET Core and Entity Framework Core.

## Tech Stack

- **Framework:** ASP.NET Core Web API (.NET 10)
- **ORM:** Entity Framework Core with SQL Server
- **Testing:** xUnit + Moq
- **API Docs:** Swagger

## Architecture

The project follows a layered architecture to keep concerns separated:

```
Controller → Service → Repository → Database
```

- **Controllers** — handle HTTP requests and responses
- **Services** — contain business logic and map between models and DTOs
- **Repositories** — handle all database operations via EF Core
- **DTOs** — define what data is accepted in requests and returned in responses

## API Endpoints

### Students `/api/students`
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/students` | Get all students |
| GET | `/api/students/{id}` | Get a student by ID |
| POST | `/api/students` | Create a new student |
| PUT | `/api/students/{id}` | Update a student |
| DELETE | `/api/students/{id}` | Delete a student |

---

### Instructors `/api/instructors`
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/instructors` | Get all instructors |
| GET | `/api/instructors/{id}` | Get an instructor by ID |
| POST | `/api/instructors` | Create a new instructor |
| PUT | `/api/instructors/{id}` | Update an instructor |
| DELETE | `/api/instructors/{id}` | Delete an instructor |

> Deleting an instructor that still has courses assigned returns `409 Conflict`.

---

### Courses `/api/courses`
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/courses` | Get all courses |
| GET | `/api/courses/{id}` | Get a course by ID |
| POST | `/api/courses` | Create a new course |
| PUT | `/api/courses/{id}` | Update a course |
| DELETE | `/api/courses/{id}` | Delete a course |

> Creating or updating a course with a non-existent `instructorId` returns `409 Conflict`.  
> Deleting a course cascade deletes all of its enrollments.

---

### Enrollments `/api/enrollments`
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/enrollments` | Get all enrollments |
| GET | `/api/enrollments/{studentId}/{courseId}` | Get an enrollment by composite key |
| POST | `/api/enrollments` | Enroll a student in a course |
| PUT | `/api/enrollments/{studentId}/{courseId}` | Update a grade |
| DELETE | `/api/enrollments/{studentId}/{courseId}` | Remove an enrollment |

> Enrollment uses a composite key — both `studentId` and `courseId` are required to identify a record.  
> `grade` is optional and can be assigned later via PUT.

---

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (or SQL Server Express)

### Setup

1. Clone the repository
2. Update the connection string in `SchoolApp.Api/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DevConnection": "Server=YOUR_SERVER;Database=SchoolDb;Trusted_Connection=True;"
  }
}
```
3. Apply the database migrations:
```bash
dotnet ef database update --project SchoolApp.Api
```
4. Run the API:
```bash
dotnet run --project SchoolApp.Api
```
5. Open Swagger UI at `https://localhost:{port}/swagger`

## Running Tests

```bash
dotnet test
```

To generate a code coverage report:
```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"[Path to XML file]" -targetdir:"coverage-report" -reporttypes:Html
start coverage-report/index.html
```
