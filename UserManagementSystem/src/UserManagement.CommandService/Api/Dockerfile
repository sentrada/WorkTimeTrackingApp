# .NET SDK használata a buildhez
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserManagement.CommandService.Api.csproj", "./"]
RUN dotnet restore "UserManagement.CommandService.Api.csproj"
COPY . .
RUN dotnet build "UserManagement.CommandService.Api.csproj" -c Release -o /app/build
RUN dotnet publish "UserManagement.CommandService.Api.csproj" -c Release -o /app/publish

# .NET ASP.NET Core Runtime használata a futtatáshoz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.CommandService.Api.dll"]
