# Solution Documentation

**Candidate Name:** Deval Jhingran  
**Completion Date:** 30 March 2026

---

## Problems Identified

1. Architecture & Design Issues:

Tight coupling between controller and service (new TodoService() inside controller).

No use of Dependency Injection.

No separation of concerns (Controller handling business logic + mapping).

No clear layering (Controller -> Service -> Data Access mixed).

2. Code Quality & Maintainability:

Repeated object mapping logic across layers.

No interfaces -> difficult to test or extend.

Poor naming conventions for endpoints (createTodo, getTodo etc.).

DTOs mixed inside controller instead of proper structure.

3. Security Vulnerabilities:

SQL Injection risk due to string interpolation in queries.

No input validation.

Exception messages directly returned to clients (information leakage).

4. Performance & Reliability:

Database connection created manually in every method without abstraction.

No logging or observability.

No centralized error handling.

5. API Design Issues:

Incorrect HTTP methods (POST used for GET/DELETE/UPDATE).

Non-RESTful routes.

No proper status codes (200 used everywhere).

6. Testing Gaps.

Tests were: Hitting real database ,they were not isolated, not testing actual behavior.

No negative or edge case coverage

---

## Architectural Decisions

1. Layered Architecture

Implemented clear separation:

Controller-> Service-> Repository-> Database
Benefits:
Loose coupling,
Better testability, 
Clear responsibility boundaries.


2. Dependency Injection
Introduced interfaces:
ITodoService,
ITodoRepository, 
Registered via DI container
Reason behind implementing: Makes our application code loosely coupled, Improves testability, Enables mocking, Follows SOLID principles.

3. Repository Pattern
Extracted all database logic into TodoRepository
Benefits:
Isolates data access,
Easier to swap DB in future,
Cleaner service layer.

4. DTO Pattern

Introduced DTO's:
CreateTodoRequest,
UpdateTodoRequest,
TodoResponse
Why:
Avoid exposing domain models directly,
Better API contract control,
Supports versioning and flexibility.

5. RESTful API Design

Refactored endpoints:

Operation	Method	Endpoint
Create	    POST	/api/todos
Get All	    GET	    /api/todos
Get By Id	GET	    /api/todos/{id}
Update	    PATCH	/api/todos/{id}
Delete	    DELETE	/api/todos/{id}


6. SQL Injection Fix
Replaced string interpolation with parameterized queries

7. Global Exception Handling
Implemented middleware for centralized error handling
Benefits:
Consistent error responses,
Cleaner controllers,
Improved security (no stack trace exposure).

8. Logging
Added structured logging using ILogger
Coverage:
Service layer operations,
Repository DB operations,
Error scenarios.

9. Unit Testing
Refactored tests using:
xUnit,
Moq,
FluentAssertions.
Focus:
Service layer testing,
Mocked repository.
Covered:
Success cases,
Failure cases,
Edge cases.
---

## Trade-offs

1. Skipped ORM (Entity Framework):
Used raw SQLite for simplicity.
Trade-off: More manual mapping.

2. Partial Validation:
Basic validation handled implicitly.
Could add FluentValidation for robustness.

3. Limited Controller Testing: 
Focused on service layer testing.
Controllers kept thin.

4. Mapping Inside Service:
Used manual mapping instead of AutoMapper.
Trade-off: Less abstraction but simpler for scope.

5. Database Connection Handling:
Opened per request (standard practice).
Did not implement connection pooling manually.


---

## How to Run

### Prerequisites
.NET 6/7 SDK
Visual Studio / VS Code

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project TodoApi
```

### Test
```bash
dotnet test
```

---

## API Documentation

### Endpoints

#### Create TODO
```
Method: POST
URL: /api/todos
Request Body: {
  "title": "Learn Devops",
  "description": "Watch how pipeline is built and maintained"
}
Response: {
  "id": 7,
  "title": "Learn Devops",
  "description": "Watch how pipeline is built and maintained",
  "isCompleted": false,
  "createdAt": "2026-03-29T23:22:13.396187Z"
}
```

#### Get TODO(s)
```

Get All
Method: GET
URL: /api/todos
Response: [
  {
    "id": 4,
    "title": "Learn System Design",
    "description": "watch HLD and LLD videos, look into Githubrepos for help",
    "isCompleted": false,
    "createdAt": "2026-03-30T01:14:56.2003505+05:30"
  },
  {
    "id": 5,
    "title": "learn .Net Backend architecture ",
    "description": "Learn how microservice architecture is implemented",
    "isCompleted": false,
    "createdAt": "2026-03-30T01:44:14.0473022+05:30"
  },
  {
    "id": 6,
    "title": "Learn Angular",
    "description": "Watch angular tutorial videos",
    "isCompleted": false,
    "createdAt": "2026-03-30T03:29:44.4425784+05:30"
  },
  {
    "id": 7,
    "title": "Learn Devops",
    "description": "Watch how pipeline is built and maintained",
    "isCompleted": false,
    "createdAt": "2026-03-30T04:52:13.3744389+05:30"
  }
]

Get By Id
Method: GET
URL: /api/todos/{id}
id: 7
Response: {
  "id": 7,
  "title": "Learn Devops",
  "description": "Watch how pipeline is built and maintained",
  "isCompleted": false,
  "createdAt": "2026-03-30T04:52:13.3744389+05:30"
}

```

#### Update TODO
```
Method: PATCH
URL: /api/todos/{id}
id : 7
Request Body: {
  "description": "Understand CI/CD and automate the job so it gets automatically completed once started"
}
Response: {
  "id": 7,
  "title": "Learn Devops",
  "description": "Understand CI/CD and automate the job so it gets automatically completed once started",
  "isCompleted": false,
  "createdAt": "2026-03-30T04:52:13.3744389+05:30"
}
```

#### Delete TODO
```
Method: DELETE
URL: /api/todos/{id}
id : 7
Response: 204 No Content
```

---

## Future Improvements

1. Validation Layer : 

Introduce FluentValidation,
Enforce strict input validation.

2. ORM Integration: 

Replace raw SQL with Entity Framework Core,
Reduce boilerplate and improve maintainability.

3. Filtering & Pagination:

Add query parameters:
Get By Name/Description,
isCompleted,
date range,
pagination.

4. Authentication & Authorization:

Add JWT-based authentication,
Role-based access.

5. Caching:

Add caching for GET endpoints,
Improve performance.

6. Observability:

Integrate external logging systems (Serilog, ELK),
Add metrics and monitoring.

7. Integration Tests:

Add API-level tests using TestServer.

8. CI/CD Pipeline:

Automate build, test, and deployment.
