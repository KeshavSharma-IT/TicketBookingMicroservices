# Stage 1: Base Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build & Restore
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files for layer caching
COPY ["EventServices.Domain/EventServices.Domain.csproj", "EventServices.Domain/"]
COPY ["EventServices.Application/EventServices.Application.csproj", "EventServices.Application/"]
COPY ["EventServices.Infrastructure/EventServices.Infrastructure.csproj", "EventServices.Infrastructure/"]
COPY ["EventServices/EventServices.API.csproj", "EventServices/"]

# Restore dependencies
RUN dotnet restore "EventServices/EventServices.API.csproj"

# Copy all source code
COPY . .

# Build
WORKDIR "/src/EventServices"
RUN dotnet build "EventServices.API.csproj" -c Release --no-restore

# Stage 3: Publish
FROM build AS publish
RUN dotnet publish "EventServices.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-build

# Stage 4: Final Lean Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventServices.API.dll"]
