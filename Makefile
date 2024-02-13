# Commands for development
migrate-db:
	dotnet ef database update --connection "Host=localhost;Port=54321;Database=auth-service;Username=auth-service-user;Password=password" -p .\src\JoBoard.AuthService.Migrator\

init-db: wait-db
	docker-compose -f .\docker-compose.Development.yml run --build --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-db: run-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

run-db:
	docker-compose -f .\docker-compose.Development.yml up -d auth-service-db

# Commands for integration tests
test: init-test-db
	dotnet test

migrate-test-db:
	dotnet ef database update --connection "Host=localhost;Port=54322;Database=auth-service-test-db;Username=test-user;Password=password" -p .\src\JoBoard.AuthService.Migrator\

init-test-db: wait-test-db
	docker-compose -f .\docker-compose.Testing.yml run --build --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-test-db: run-test-db
	docker-compose -f .\docker-compose.Testing.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-test-db:5432 -t 10 
	
run-test-db:
	docker-compose -f .\docker-compose.Testing.yml up -d auth-service-test-db