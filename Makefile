wait-db:
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-db:5432 -t 10 

init-db: wait-db
	docker-compose -f .\docker-compose.Development.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"


# Commands for integration tests
wait-test-db:
	docker-compose -f .\docker-compose.Testing.yml run --rm --entrypoint="/bin/bash" auth-service wait-for-it.sh auth-service-test-db:5432 -t 10 

init-test-db: wait-test-db
	docker-compose -f .\docker-compose.Testing.yml run --rm --entrypoint="/bin/bash" auth-service "docker-entrypoint.RunMigrator.sh"