# = = = Build Stage = = =
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . .

# Build the application
RUN dotnet publish "Pictura.Api.csproj" -c Release -o /app/publish

# = = = Production Stage = = =
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS production
WORKDIR /app
COPY --from=build /app/publish .

# Add a non-root user for security
RUN adduser --disabled-password --gecos "" appuser
USER appuser

# Expose the port the app runs on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "Pictura.Api.dll"]
