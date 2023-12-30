### Use Cases:
1) Register new user by email and password
2) Register new user by external network account
3) Confirm email
4) Login by email and password / Get access token
5) Login by external network account / Get access token
6) Reset password request
7) Reset password command
8) Change email
9) Change password
10) Attach external network account

### Domain layer
Доменный слой реализован с подходом DDD. 
Большая часть бизнес-логики содержится в доменном слое, а именно в агрегатах, сущностях, объектах-значениях и т.д. 
Используется т.н. подход Богатой модели (Rich model), а также паттерн Information expert из GRASP.
Объекты, которые содержат данные сами занимаются обработкой и валидацией этих данных.

### Application layer:
1) Некоторые бизнес-правила проверяются в слое Application чтобы сохранить слой Domain изолированным и удобным при unit-тестировании.
Примеры бизнес-правил в слое Application:
- проверка уникальности email при регистрации;
- проверка уникальности внешней учётной записи при регистрации и при привязке.
Если же передавать интерфейсы репозиториев в качестве параметров в доменные сущности, то усложнится их unit-тестирование.
2) Слой можно использовать совместно с различными фреймворками и способами аутентификации (cookies, JWT и т.д.).
3) В слое пока что нет валидации полей входящих команд и запросов, но вся необходимая валидация есть в слое Domain.