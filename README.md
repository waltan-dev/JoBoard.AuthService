[![Tests](https://github.com/waltan-dev/JoBoard.AuthService/actions/workflows/dotnet.yml/badge.svg)](https://github.com/waltan-dev/JoBoard.AuthService/actions/workflows/dotnet.yml)
[![Docker Image CI](https://github.com/waltan-dev/JoBoard.AuthService/actions/workflows/docker-image.yml/badge.svg)](https://github.com/waltan-dev/JoBoard.AuthService/actions/workflows/docker-image.yml)

Demo проект, микросервис аутентификации. Еще в разработке

## Application Use Cases:
Authentication Use Cases:
- Register new user by email and password
- Register new user by external account
- Confirm email (request + confirmation)
- Reset password (request + confirmation)

Manage Account Use Cases:
- Change email (request + confirmation)
- Change password
- Change role
- Attach external account
- Detach external account
- Deactivate account (request + confirmation)

API Tokens Use Cases:
- Login / Get access & refresh tokens by password
- Login / Get access & refresh tokens by external account
- Refresh token
- Logout / Revoke refresh token

## Solution structure:
### Domain layer
Доменный слой реализован изолированным и с подходом DDD. Вся бизнес-логики содержится в доменном слое, 
а именно в агрегатах, сущностях, объектах-значениях и т.д. Используется т.н. подход Богатой модели (Rich model), 
а также паттерн Information expert из GRASP - объекты, которые содержат данные сами занимаются обработкой и валидацией этих данных.

### Application layer:
* В слое присутствует чёткое разделение на команды и запросы. Внутри команд используются шаблоны DDD чтобы повысить качество кода, 
который изменяет состояние системы. А внутри запросов наоборот - шаблоны DDD не используются с целью повышения производительности 
за счёт использования microORM, хранимых процедур, представлений и т.д. для запросов.
* Слой не привязан к конкретному способу аутентификации - можно подключать любые (cookies, JWT и т.д.) и использовать слой совместно с различными фреймворками.


## Tests:
### JoBoard.AuthService.FunctionalTests
Функциональные тесты проверяют работу приложения по функциональным требованиям и как приложение работает с точки зрения конечного пользователя/клиента.
Такие тесты имитируют действия/запросы пользователя/клиента - регистрация, вход и т.д.
Такие тесты называются функциональными, потому что они проверяют, что приложение корректно выполняет все функции, которые ожидаются от него.
В проекте содержатся тесты, которые покрывают логику и публичный контракт API endpoints.

### JoBoard.AuthService.IntegrationTests
Интеграционные тесты проверяют взаимодействие между различными компонентами, а также используются для тестирования инфраструктуры приложения. 
Такие тесты называются интеграционными, потому что они проверяют, как приложение работает в интеграции с разными компонентами, такими как базы данных, сервисы и т.д.
В проекте содержатся тесты, которые проверяют работоспобность инфраструктуры приложения.

* Для интеграционных и функциональных тестов используется отдельная тестовая БД postgres, запускаемая через docker.

### Migrations:
Команды (запускать из корневой директории)
* Сгенерировать новую миграцию:
``` make add-migration ```
* Удалить последнюю миграцию:
``` make remove-migration ```
* Применить миграции к dev db
``` make migrate-dev-db ```
