# CleanArchitecture-Template
This is a solution template for [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) implementation with ASP.NET Core Web Api

![CleanArchitecture](https://user-images.githubusercontent.com/42376112/110762993-a61b1580-8266-11eb-9ac1-438072319971.jpg)

## Give a Star! ⭐
If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Technologies used:

* ASP.NET Core
* Entity Framework Core
* MediatR
* Swagger
* Redis (for distributed caching)
* Jwt Token Auhentication
* Custom Asp.Net Identity
* Api Versioning
* FluentValidation
* PolyCache (for caching)
* Serilog
* Elasticsearch (for writing Logs)
* AutoMapper
* Docker

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
* PipelineBehavior for Validation and Performance tracking.

## For Database Migration:
  ### First:
  Set default project to Persistance
  ### Second:
  Run following code in Package Manager Console
  ```ruby
  > Update-Database -Context CleanArchWriteDbContext
  ```

