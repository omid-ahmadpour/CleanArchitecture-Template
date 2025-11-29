#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Web/Api/CleanTemplate.Api.csproj", "src/Web/Api/"]
COPY ["src/Web/ApiFramework/CleanTemplate.ApiFramework.csproj", "src/Web/ApiFramework/"]
COPY ["src/Infrastructure/Persistance/CleanTemplate.Persistence.csproj", "src/Infrastructure/Persistance/"]
COPY ["src/Core/Domain/CleanTemplate.Domain.csproj", "src/Core/Domain/"]
COPY ["src/Core/Application/CleanTemplate.Application.csproj", "src/Core/Application/"]
COPY ["src/Common/CleanTemplate.Common.csproj", "src/Common/"]
RUN dotnet restore "src/Web/Api/CleanTemplate.Api.csproj"
COPY . .
WORKDIR "/src/src/Web/Api"
RUN dotnet build "CleanTemplate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanTemplate.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanTemplate.Api.dll"]