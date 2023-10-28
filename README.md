# CleanArchitecture-Template Plus
Welcome to the **CleanArchitecture-Template** repository, a powerful solution template that exemplifies the principles of [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) and incorporates the robustness of CQRS implementation using ASP.NET Core.

![CleanArchitecture](https://user-images.githubusercontent.com/42376112/110762993-a61b1580-8266-11eb-9ac1-438072319971.jpg)

## Show Your Appreciation! ⭐
If you find value in this project, whether you're using it for learning or kickstarting your solution, a star is a wonderful way to express your support. Thank you in advance!

# Prerequisites
- Visual Studio 2022
- .NET 7.0 Runtime

# Effortless Project Creation
Here's the simplest way to get started with your project:

1. Open your command-line interface.
2. Execute the following command:
   ```shell
   dotnet new install ASPNETCleanTemplate.nuspec::3.4.0
   ```
3. Create an empty folder to house your solution and navigate into it.
4. Run the subsequent command, replacing `MyNewCleanTemplate` with your desired project name:
   ```shell
   dotnet new aspnetcleantemplate -n MyNewCleanTemplate
   ```

# Smooth Database Migration:
To handle database migration with finesse:

1. Set the default project to **Persistence**.
2. Open the Package Manager Console and run:
   ```shell
   Update-Database -Context AppDbContext
   ```

# Monitor Health
For health check administration, utilize the following URL:
[https://Url:Port/healthchecks-ui](https://Url:Port/healthchecks-ui)

## Technologies at Play:

* ASP.NET Core
* Entity Framework Core
* MediatR
* Swagger
* Redis (for distributed caching)
* Jwt Token Authentication
* Custom Asp.Net Identity
* Api Versioning
* FluentValidation
* PolyCache (for caching)
* Serilog
* Elasticsearch (for writing Logs)
* Mapper
* Docker
* xUnit

## Championing Best Practices and Design Principles:

* Clean Architecture
* Clean Code
* CQRS
* Authentication and Authorization
* Distributed caching
* SOLID Principles
* Segregated ReadOnly and Write DbContext
* Segregated ReadOnly and Write Repository
* REST API Naming Conventions
* Multi-environment Utilization in ASP.NET Core (Development, Production, Staging, etc.)
* Modular Design
* Custom Exceptions
* Tailored Exception Handling
* Unit Tests
* Integration Tests
* PipelineBehavior for Validation and Performance Tracking.

## Further Reading
1. [The Significance of Clean Architecture Template with .NET](https://medium.com/@omid-ahmadpour/clean-architecture-template-with-net-and-its-importance-e5b3b97a6e48)
2. [Understanding and Implementing Scalability in CQRS](https://virgool.io/@ahmadpooromid/%D9%85%D9%81%D9%87%D9%88%D9%85-%D9%88-%D9%BE%DB%8C%D8%A7%D8%AF%D9%87-%D8%B3%D8%A7%D8%B2%DB%8C-scalability-%D8%AF%D8%B1-cqrs-peixkgrbdgff)
3. [Why We Need Clean Architecture?](https://www.youtube.com/watch?v=GO61-MiWirk&t=17s)

Elevate your development journey with the CleanArchitecture-Template Plus!
