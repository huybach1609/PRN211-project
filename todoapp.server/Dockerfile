# Use the official .NET 8 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5291

# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["todoapp.server.csproj", "."]
RUN dotnet restore "todoapp.server.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "todoapp.server.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "todoapp.server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------------------------- Runtime stage -----------------------------------------------------------
# Create the final runtime image
FROM base AS final
WORKDIR /app

# Behind a reverse proxy, this enables X-Forwarded-* handling automatically
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true

# .NET 8 container default port is 8080; pick your port and make it explicit
ENV ASPNETCORE_URLS=http://+:5291

COPY --from=publish /app/publish .

# Expose only the port you actually bind to
EXPOSE 5291

ENTRYPOINT ["dotnet", "todoapp.server.dll"]
