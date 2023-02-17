# Build environment stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
# Copy everything sln and project files to /source
WORKDIR /source
COPY *.sln .
COPY partsbin/ partsbin/
# Full release build and publish to /app
RUN dotnet publish -c release -o /app

# Runtime image stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Copy the app from the builder stage
WORKDIR /app
COPY --from=builder /app .
# Expose port 80 (remap it using -p) and start the app
EXPOSE 80
ENTRYPOINT dotnet partsbin.dll




