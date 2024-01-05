# Requirements for debugging to work correctly in JetBrains Rider:
# 1) Dockerfile must be in the project directory near .csproj
# 2) Dockerfile must include "base" stage

# https://blog.jetbrains.com/dotnet/2023/06/07/how-docker-fast-mode-works-in-rider/
# When you run or debug Docker applications from JetBrains Rider, it uses a Fast mode by default. 
# Docker Fast mode builds and launches the application directly, without the need to build and publish the container. 
# In this mode, only the base stage of the Dockerfile is executed. 
# build, publish, and final stages are ignored.

# Base stage
### first layer
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS "http://*:5000"
EXPOSE 5000

# Build stage
### second layer - load nuget packages to cache
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /source

# To take advantage of the build cache copy just the .csproj files
# and run the restore command to cache the result of the restore in a Docker layer
COPY ["*.sln", "."]
COPY ["src/JoBoard.AuthService/*.csproj", "src/JoBoard.AuthService/"]
COPY ["src/JoBoard.AuthService.Application/*.csproj", "src/JoBoard.AuthService.Application/"]
COPY ["src/JoBoard.AuthService.Domain/*.csproj", "src/JoBoard.AuthService.Domain/"]
COPY ["src/JoBoard.AuthService.Infrastructure/*.csproj", "src/JoBoard.AuthService.Infrastructure/"]
COPY ["src/JoBoard.AuthService.Infrastructure.Data/*.csproj", "src/JoBoard.AuthService.Infrastructure.Data/"]
COPY ["src/JoBoard.AuthService.Migrator/*.csproj", "src/JoBoard.AuthService.Migrator/"]
COPY ["tests/JoBoard.AuthService.Domain.UnitTests/*.csproj", "tests/JoBoard.AuthService.Domain.UnitTests/"]
COPY ["tests/JoBoard.AuthService.Infrastructure.UnitTests/*.csproj", "tests/JoBoard.AuthService.Infrastructure.UnitTests/"]

RUN dotnet restore

### third layer
COPY . .
RUN dotnet publish "src/JoBoard.AuthService/JoBoard.AuthService.csproj" -c Release -o "/app/publish" /p:UseAppHost=false

# Serve stage
### fourth layer
FROM base AS final
WORKDIR /app
COPY --from=build "/app/publish" .
ENTRYPOINT [ "dotnet", "JoBoard.AuthService.dll" ]