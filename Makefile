# Commands for development
init-db: wait-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-db: run-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

run-db:
	docker-compose -f .\docker-compose.Development.yml up -d auth-service-db

# Commands for integration tests
test: init-test-db
	dotnet test

init-test-db: wait-test-db
	docker-compose -f .\docker-compose.Testing.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"

wait-test-db: run-test-db
	docker-compose -f .\docker-compose.Testing.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-test-db:5432 -t 10 
	
run-test-db:
	docker-compose -f .\docker-compose.Testing.yml up -d auth-service-test-db