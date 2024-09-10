# .NET SDK használata a buildhez
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["UserManagement.QueryService.Api.csproj", "./"]
RUN dotnet restore "UserManagement.QueryService.Api.csproj"
COPY . .
RUN dotnet build "UserManagement.QueryService.Api.csproj" -c Release -o /app/build
RUN dotnet publish "UserManagement.QueryService.Api.csproj" -c Release -o /app/publish

# .NET ASP.NET Core Runtime használata a futtatáshoz
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserManagement.QueryService.Api.dll"]
