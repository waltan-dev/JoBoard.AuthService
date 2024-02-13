# Requirements for debugging to work correctly in JetBrains Rider:
# 1) Dockerfile must be in the project directory near .csproj
# 2) Dockerfile must include "base" stage

# https://blog.jetbrains.com/dotnet/2023/06/07/how-docker-fast-mode-works-in-rider/
# When you run or debug Docker applications from JetBrains Rider, it uses a Fast mode by default. 
# Docker Fast mode builds and launches the application directly, without the need to build and publish the container. 
# In this mode, only the "base" stage of the Dockerfile is executed. 
# "build", "publish", and "final" stages are ignored.

# Base stage - only for debug
### first layer 
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS "http://*:5000"
EXPOSE 5000

# Build stage
### second layer - cache nuget packages in Docker layer
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

COPY ["src/JoBoard.AuthService/*.csproj", "/source/src/JoBoard.AuthService/"]
COPY ["src/JoBoard.AuthService.Application/*.csproj", "/source/src/JoBoard.AuthService.Application/"]
COPY ["src/JoBoard.AuthService.Domain/*.csproj", "/source/src/JoBoard.AuthService.Domain/"]
COPY ["src/JoBoard.AuthService.Infrastructure.Auth/*.csproj", "/source/src/JoBoard.AuthService.Infrastructure.Auth/"]
COPY ["src/JoBoard.AuthService.Infrastructure.Data/*.csproj", "/source/src/JoBoard.AuthService.Infrastructure.Data/"]
COPY ["src/JoBoard.AuthService.Infrastructure.Jwt/*.csproj", "/source/src/JoBoard.AuthService.Infrastructure.Jwt/"]
COPY ["src/JoBoard.AuthService.Migrator/*.csproj", "/source/src/JoBoard.AuthService.Migrator/"]

RUN dotnet restore "/source/src/JoBoard.AuthService/JoBoard.AuthService.csproj"
RUN dotnet restore "/source/src/JoBoard.AuthService.Migrator/JoBoard.AuthService.Migrator.csproj"

### third layer - build and publish
COPY . /source/
RUN dotnet publish "/source/src/JoBoard.AuthService/JoBoard.AuthService.csproj" -c Release -o "/app/publish"
RUN dotnet publish "/source/src/JoBoard.AuthService.Migrator/JoBoard.AuthService.Migrator.csproj" -c Release -o "/migrator-app/publish"

# Serve stage
### fourth layer
FROM base AS final

COPY --from=build "/app/publish" /app
COPY --from=build "/migrator-app/publish" /app/migrator/

# for docker compose
COPY "docker-entrypoint.RunMigrator.sh" /usr/local/bin/
COPY "wait-for-it.sh" /usr/local/bin/
RUN chmod +x /usr/local/bin/docker-entrypoint.RunMigrator.sh
RUN chmod +x /usr/local/bin/wait-for-it.sh

# default entrypoint - you can override this in docker-compose
WORKDIR /app
ENTRYPOINT ["dotnet", "JoBoard.AuthService.dll"]