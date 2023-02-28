# Build environment stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS builder
# Copy everything sln and project files to /source
WORKDIR /source
COPY . ./
# Full release build and publish to /app
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime image stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
# Copy the app from the builder stage
WORKDIR /app
COPY --from=builder /source/out .
# Expose port 80 (remap it using -p or via docker-compose.yml)
EXPOSE 80
# Start the app
ENTRYPOINT dotnet partsbin.dll




