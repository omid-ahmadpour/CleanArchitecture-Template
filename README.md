# CleanArchitecture-Template
This is a solution template for [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) and CQRS implementation with ASP.NET Core

![CleanArchitecture](https://user-images.githubusercontent.com/42376112/110762993-a61b1580-8266-11eb-9ac1-438072319971.jpg)

## Give a Star! ⭐
If you like or are using this project to learn or start your solution, please give it a star. Thanks!

# Prerequisites
Visual Studio 2022

.NET 7.0 Runtime

# The easiest way to create your project
### 1. Open CMD
### 2. Run
```ruby
  dotnet new --install ASPNETCleanTemplate.nuspec::3.4.0
  ```
### 3. Create an empty folder for your solution and cd into it.
### 4. Run the following code and enter your project name instead of MyNewCleanTemplate

```ruby
  dotnet new aspnetcleantemplate -n MyNewCleanTemplate
  ```

# For Database Migration:
  ### First:
  Set default project to Persistence
  ### Second:
  Run following code in Package Manager Console
  ```ruby
  Update-Database -Context AppDbContext
  ```
  
 # HealthCheck
 ### use the following url to open health check admin ui
 
 https://Url:Port/healthchecks-ui

## Technologies used:

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

## Software Development Best Practices and Design Principles used:

* Clean Architecture
* Clean Code
* CQRS
* Authentication and Authorization
* Distributed caching
* Solid Principles
* Separate ReadOnly and Write DbContext
* Separate ReadOnly and Write Repository
* REST API Naming Conventions
* Use multiple environments in ASP.NET Core (Development,Production,Staging,etc)
* Modular Design
* Custom Exceptions
* Custom Exception Handling
* Unit tests
* Integration tests
* PipelineBehavior for Validation and Performance tracking.

## Read More
1. https://virgool.io/@ahmadpooromid/%D9%85%D9%81%D9%87%D9%88%D9%85-%D9%88-%D9%BE%DB%8C%D8%A7%D8%AF%D9%87-%D8%B3%D8%A7%D8%B2%DB%8C-scalability-%D8%AF%D8%B1-cqrs-peixkgrbdgff
2. https://medium.com/@omid-ahmadpour/clean-architecture-template-with-net-and-its-importance-e5b3b97a6e48
