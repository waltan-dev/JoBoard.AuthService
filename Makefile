# Commands for development database
start-dev-db: run-dev-db-container wait-dev-db migrate-dev-db-via-migrator

migrate-dev-db-via-migrator: 
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-dev-db:
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

run-dev-db-container:
	docker-compose -f .\docker-compose.Development.yml up -d auth-service-db
	
# Migrations
add-migration:
	dotnet ef migrations add $(NAME) -s .\src\JoBoard.AuthService.Migrator\ -p .\src\JoBoard.AuthService.Infrastructure.Data\

remove-last-migration:
	dotnet ef migrations remove -s .\src\JoBoard.AuthService.Migrator\ -p .\src\JoBoard.AuthService.Infrastructure.Data\

migrate-dev-db:
	dotnet ef database update --connection "Host=localhost;Port=54321;Database=auth-service;Username=auth-service-user;Password=password" -s .\src\JoBoard.AuthService.Migrator -p .\src\JoBoard.AuthService.Infrastructure.Data