# Commands for development database
init-dev-db: wait-dev-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-dev-db: run-dev-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

run-dev-db:
	docker-compose -f .\docker-compose.Development.yml up -d auth-service-db
	
# Migrations
migrate-dev-db:
	dotnet ef database update --connection "Host=localhost;Port=54321;Database=auth-service;Username=auth-service-user;Password=password" -p .\src\JoBoard.AuthService.Migrator\