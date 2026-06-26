# StackOverflow Lite

StackOverflow Lite is a simplified Question & Answer platform built as a robust RESTful API. It adheres to Clean Architecture principles, leveraging CQRS (Command Query Responsibility Segregation) via MediatR, and utilizes modern tools like Docker, PostgreSQL, and Redis to deliver a scalable, enterprise-grade backend.

This project allows registered users to post questions, answer them, vote on content, and build a reputation score based on community interactions.

## Architecture & Tech Stack

- **ASP.NET Core 10 Web API**
- **Clean Architecture** (Domain, Application, Infrastructure, Persistence, API)
- **CQRS** (MediatR)
- **Entity Framework Core** with PostgreSQL
- **Redis** for caching and view counting
- **ASP.NET Identity & JWT** for Authentication/Authorization
- **FluentValidation** for MediatR pipeline validation
- **Docker Compose** for local infrastructure orchestration

---

## Project Structure

This project rigidly follows Clean Architecture. Dependencies only point inwards towards the Domain layer.

```text
StackOverflowLite/
│
├── StackOverflowLite.Domain/          # Core layer (No dependencies)
│   ├── Entities/                      # Database models (User, Question, Answer, Vote, Tag)
│   └── Interfaces/                    # Abstract domain interfaces
│
├── StackOverflowLite.Application/     # Application logic (Depends on Domain)
│   ├── DependencyInjection.cs         # IoC registrations (Validators, MediatR Pipeline)
│   ├── Features/                      # CQRS MediatR Handlers
│   │   ├── Auth/                      # Login, Register, Profile Logic
│   │   ├── Questions/                 # Question CRUD, Dtos, Validation
│   │   ├── Answers/                   # Answer CRUD, Validation
│   │   └── Votes/                     # Upvote/Downvote logic & validation
│   └── Services/                      # Application interfaces (ICacheService, IJwtService)
│
├── StackOverflowLite.Infrastructure/  # External systems (Depends on Application & Domain)
│   ├── Services/                      # Concrete implementations (RedisCacheService, JwtService)
│   └── DependencyInjection.cs         # IoC registrations
│
├── StackOverflowLite.Persistence/     # Database access (Depends on Application & Domain)
│   ├── Data/                          # AppDbContext and Migrations
│   └── DependencyInjection.cs         # IoC registrations (PostgreSQL)
│
├── StackOverflowLite.API/             # Presentation layer (Depends on Application, Infra, Persistence)
│   ├── Controllers/                   # Thin API Controllers
│   ├── Middleware/                    # Global Exception Handler
│   ├── appsettings.json               # Config strings
│   └── Program.cs                     # API startup
│
└── docker-compose.yml                 # Local dev orchestration (PostgreSQL, Redis, API)
```

---

## How to Run Locally

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running.
- [.NET 10 SDK](https://dotnet.microsoft.com/download) installed.

### Step 1: Start the Infrastructure
The database (PostgreSQL) and caching layer (Redis) are containerized. Open your terminal in the root folder (where `docker-compose.yml` is located) and run:

```bash
docker compose up -d db redis
```

*Note: The first time you run this, it will download the PostgreSQL and Redis Alpine images.*

### Step 2: Apply Migrations (Optional)
The application is configured to automatically apply Entity Framework migrations on startup. However, if you need to apply migrations manually or make schema changes, run the following command from the root folder:

```bash
dotnet ef database update --project StackOverflowLite.Persistence --startup-project StackOverflowLite.API
```

### Step 3: Run the API
To run the web API locally, execute the following command from the root folder:

```bash
dotnet run --project StackOverflowLite.API
```

### Step 4: Explore the API
Once the application says "Now listening on: http://localhost:5105", open your web browser and navigate to:
**[http://localhost:5105/swagger](http://localhost:5105/swagger)**

---

## Environment Variables

When running locally via `dotnet run`, the application reads from `appsettings.json`. However, when running the full stack entirely within Docker (using `docker compose up api`), the following environment variables are injected into the API container:

- `ASPNETCORE_ENVIRONMENT=Development`
- `ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=stackoverflowlite;Username=postgres;Password=admin`
- `ConnectionStrings__Redis=redis:6379`
- `Jwt__Key=your-super-secret-key-minimum-32-chars-long!!`

---

## API Guide

All endpoints are thoroughly documented in the Swagger UI. However, here is a brief guide on how to interact with the system.

### 1. Authentication
Most endpoints require authentication. You must first register and log in to receive a JWT token.
- **POST `/api/auth/register`**: Create a new account.
- **POST `/api/auth/login`**: Authenticate and receive a JWT.
- *In Swagger: Click the "Authorize" button at the top and paste your token in the format: `Bearer <your_token>`*

### 2. Questions
- **POST `/api/questions`**: Create a question (requires Auth).
- **GET `/api/questions`**: Fetch all questions (cached in Redis for 10 minutes). Supports filtering via query parameter: `?tag=csharp`.
- **GET `/api/questions/{id}`**: View a single question. This immediately increments the question's `ViewCount` in Redis.
- **PUT / DELETE `/api/questions/{id}`**: Edit or delete your own questions (requires Auth).

### 3. Answers & Accepted Answers
- **POST `/api/questions/{questionId}/answers`**: Post an answer to a question (requires Auth).
- **GET `/api/questions/{questionId}/answers`**: List all answers for a specific question.
- **POST `/api/questions/{questionId}/answers/{answerId}/accept`**: Mark an answer as the "Accepted Answer". Only the author of the question can do this.

### 4. Voting & Reputation
Users can Upvote (+5 for Questions, +10 for Answers) or Downvote (-1 for Questions, -2 for Answers) content.
- **POST `/api/questions/{questionId}/vote`**: Vote on a question.
- **POST `/api/questions/{questionId}/answers/{answerId}/vote`**: Vote on an answer.
- *Note: Users cannot vote on their own content, and changing a vote will correctly recalculate the author's reputation score.*
- **GET `/api/auth/profile`**: Check your current Reputation score.
