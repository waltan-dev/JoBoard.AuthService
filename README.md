Use Cases:
1) Register new user by email and password
2) Register new user by external network account
3) Confirm email
4) Login by email and password / Get access token
5) Login by external network account / Get access token
6) Reset password request
7) Reset password command
8) Change email
9) Attach external network account

Большая часть бизнес-логики содержится в слое Domain. Но некоторые бизнес-правила проверяются в слое Application 
чтобы сохранить слой Domain изолированным и удобным при unit-тестировании. Примеры бизнес-правил в слое Application:
- проверка уникальности email при регистрации;
- проверка уникальности внешней учётной записи при регистрации и при привязке.
Если передавать интерфейсы репозиториев в качестве параметров в доменные сущности, то усложнится их unit-тестирование.