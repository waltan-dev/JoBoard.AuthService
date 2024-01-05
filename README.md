### Anonymous Use Cases:
1) Register new user by email and password
2) Register new user by external account
3) Confirm email
4) Login by email and password
5) Login by external account
6) Reset password (request + confirmation)

### Authenticated Use Cases:
1) Change email (request + confirmation)
2) Change password
3) Change role
4) Attach external account
5) Deactivate account

### Domain layer
1) Доменный слой реализован изолированным и с подходом DDD. Большая часть бизнес-логики содержится в доменном слое, 
а именно в агрегатах, сущностях, объектах-значениях и т.д. Используется т.н. подход Богатой модели (Rich model), 
а также паттерн Information expert из GRASP - объекты, которые содержат данные сами занимаются обработкой и валидацией этих данных.

### Application layer:
1) Некоторые бизнес-правила проверяются в слое Application чтобы сохранить слой Domain изолированным и удобным для unit-тестирования.
Примеры бизнес-правил в слое Application:
- проверка уникальности email при регистрации;
- проверка уникальности внешней учётной записи при регистрации и при привязке.
p.s.: если передавать интерфейсы репозиториев в качестве параметров в доменные сущности, то усложнится их unit-тестирование.
2) Слой можно использовать совместно с различными фреймворками и способами аутентификации (cookies, JWT и т.д.).
3) В слое пока что нет валидации полей входящих команд и запросов, но вся необходимая валидация есть в слое Domain.