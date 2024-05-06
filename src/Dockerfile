# Build environment stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /source
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime image stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /source/out .
EXPOSE 8080
ENTRYPOINT dotnet partsbin.dll




