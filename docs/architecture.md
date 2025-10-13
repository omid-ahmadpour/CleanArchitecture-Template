# Clean Architecture (ASP.NET Core + CQRS + EF Core)

This diagram illustrates the high-level architecture for the repository CleanArchitecture-Template. It follows Clean Architecture with CQRS and EF Core, organized into Presentation, Application, Domain, and Infrastructure layers, plus Testing and cross-cutting concerns.

```mermaid
flowchart TB
  %% LAYERS
  subgraph Presentation["Presentation Layer (ASP.NET Core Web API)"]
    Controllers["API Controllers"]
    Middleware["Middleware / Filters"]
    Versioning["API Versioning"]
    Swagger["Swagger / OpenAPI"]
  end

  subgraph Application["Application Layer (CQRS, Use Cases)"]
    CQRS["Commands / Queries (DTOs)"]
    Handlers["Handlers (Use Cases)"]
    Behaviors["Pipeline Behaviors (Validation / Logging / Caching)"]
    Validators["Validators"]
    AppPorts["Application Interfaces (Ports)"]
  end

  subgraph Domain["Domain Layer (Enterprise Logic)"]
    Entities["Entities / Aggregates"]
    ValueObjects["Value Objects"]
    DomainEvents["Domain Events"]
    RepoPorts["Repository Interfaces (Ports)"]
    DomainServices["Domain Services"]
  end

  subgraph Infrastructure["Infrastructure Layer (Implementations)"]
    Repositories["Repository Implementations (Adapters)"]
    DbContext["EF Core DbContext + Configurations"]
    External["External Integrations (e.g., Email, Cache, Message Bus)"]
    Auth["Authentication / Authorization"]
  end

  subgraph Testing["Testing"]
    UnitTests["Unit Tests"]
    IntegrationTests["Integration Tests"]
  end

  Database[("Relational Database")]

  %% FLOWS
  Client["Client (HTTP / Swagger UI)"] --> Controllers
  Controllers --> CQRS
  Middleware -. cross-cutting .-> Controllers
  Versioning -. applies .-> Controllers
  Swagger -. docs/ui .-> Client

  CQRS --> Handlers
  Handlers --> Domain
  Validators --> Behaviors
  Behaviors -. cross-cutting .-> Handlers

  %% PORTS AND ADAPTERS
  Handlers -->|via ports| RepoPorts
  RepoPorts --> Repositories
  Repositories --> DbContext
  DbContext --> Database

  %% DOMAIN EVENTS
  DomainEvents -. publish/handle .-> Handlers

  %% TESTING SURFACES
  UnitTests --> Domain
  UnitTests --> Application
  IntegrationTests --> Presentation
  IntegrationTests --> Infrastructure

  %% INTERNAL RELATIONSHIPS
  DomainServices --- Entities
  ValueObjects --- Entities
  AppPorts --- Handlers
```

## Key Notes
- Presentation exposes REST endpoints, handles concerns like API versioning, middleware, and OpenAPI/Swagger.
- Application implements use cases via CQRS (Commands/Queries + Handlers), with pipeline behaviors for validation, logging, and caching.
- Domain is independent and contains core business logic: Entities, Value Objects, Domain Events, and repository interfaces (ports).
- Infrastructure provides implementations (adapters) for the domain/application ports, including EF Core repositories, DbContext, and external integrations.
- Data flow: Controller -> Command/Query -> Handler -> Domain -> Repository -> DbContext -> Database, with responses traveling back up the stack.
- Testing includes fast unit tests for domain/application and integration tests against the API and infrastructure boundaries.
