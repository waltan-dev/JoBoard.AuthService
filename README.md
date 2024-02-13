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
Доменный слой реализован изолированным и с подходом DDD. Большая часть бизнес-логики содержится в доменном слое, 
а именно в агрегатах, сущностях, объектах-значениях и т.д. Используется т.н. подход Богатой модели (Rich model), 
а также паттерн Information expert из GRASP - объекты, которые содержат данные сами занимаются обработкой и валидацией этих данных.

### Application layer:
* В слое присутствует чёткое разделение на команды и запросы. Внутри команд используются шаблоны DDD чтобы повысить качество 
кода, который изменяет состояние системы. А внутри запросов наоборот - шаблоны DDD не используются с целью повышения 
производительности за счёт использования microORM, хранимых процедур, представлений и т.д. для запросов.
* Некоторые бизнес-правила проверяются в слое Application, например:
   - проверка уникальности email при регистрации;
   - проверка уникальности внешней учётной записи при регистрации и при привязке.
* Слой можно использовать совместно с различными фреймворками и способами аутентификации (cookies, JWT и т.д.).

### О тестах:
Теория:
Функциональные тесты проверяют работу приложения по функциональным требованиям и как приложение работает с точки зрения пользователя.
Такие тесты имитируют действия пользователя - регистрация, вход и т.д.
Такие тесты называются функциональными, потому что они проверяют, что приложение выполняет все функции, которые ожидаются от него.

Интеграционные тесты проверяют взаимодействие между разными модулями, а также используются для тестирования инфраструктуры приложения. 
Такие тесты называются интеграционными, потому что они проверяют, как приложение работает с разными компонентами, такими как базы данных, сервисы и т.д.

Практика:
- В проекте JoBoard.AuthService.FunctionalTests содержатся функциональные тесты, которые покрывают логику API controllers и command/query handlers, 
а также проверяют публичный контракт API endpoints.

- В проекте JoBoard.AuthService.IntegrationTests содержатся интеграционные тесты, которые проверяют работоспобность инфраструктуры приложения.

Для интеграционных и функциональных тестов используется отдельная тестовая БД postgres, запускаемая через docker.

### О миграциях:

Проект JoBoard.AuthService.Migrator предназначен для комплексных сценариев миграции

Команды (запускать из корневой директории)
* Сгенерировать новую миграцию:
``` make add-migration ```
* Удалить последнюю миграцию:
``` make remove-migration ```
* Применить миграции к dev db
``` make migrate-dev-db ```