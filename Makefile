start-api: start-dev-db
	docker-compose -f .\docker-compose.Development.yml up -d

clear-docker:
	docker image rm --force auth-service

# Commands for development database
start-dev-db: run-container-dev-db wait-container-dev-db run-container-migrator

run-container-migrator: 
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-container-dev-db:
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

run-container-dev-db:
	docker-compose -f .\docker-compose.Development.yml up -d auth-service-db

stop-container-dev-db:
	docker-compose -f .\docker-compose.Development.yml rm -s -v auth-service-db
	
# Migrations
add-migration:
	dotnet ef migrations add $(NAME) -s .\src\JoBoard.AuthService.Migrator\ -p .\src\JoBoard.AuthService.Infrastructure.Data\

remove-last-migration:
	dotnet ef migrations remove -s .\src\JoBoard.AuthService.Migrator\ -p .\src\JoBoard.AuthService.Infrastructure.Data\

migrate-dev-db:
	dotnet ef database update --connection "Host=localhost;Port=54321;Database=auth-service;Username=auth-service-user;Password=password" -s .\src\JoBoard.AuthService.Migrator -p .\src\JoBoard.AuthService.Infrastructure.Data