FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and restore as distinct layers
COPY *.sln .
COPY src/Altairis.Domain/*.csproj ./src/Altairis.Domain/
COPY src/Altairis.Infrastructure/*.csproj ./src/Altairis.Infrastructure/
COPY src/Altairis.API/*.csproj ./src/Altairis.API/
RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/src/Altairis.API
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Altairis.API.dll"]
