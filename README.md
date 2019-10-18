# Salary

Решение состоит из двух проектов.

API - веб-API, основной проект.

MVC - веб-приложение с БД MSSQL, сделанное для:
- реализации простейших автогенерируемых графического интерфейса и валидации вводимых данных (по сравнению с альтернативами - WinForms/WPF для Console или фронт-части на базе специализированного фреймворка для API);
- практики MVC;
- практики MSSQL.

Расшифровка составляющих задания:
[+] - выполнено или исправлено в проекте API;
[mvc] - выполнено в проекте MVC;
[*] - выполнено частично;
[-] - не выполнено.

# Задание

Для отдела HR одной компании нужно написать приложение расчета заработной платы.
В компании работают сотрудники, характеризующиеся именем, датой поступления на работу, группой и базовой ставкой заработной платы.
Есть 3 группы сотрудников - Employee, Manager и Salesman. У каждого сотрудника может быть начальник. У каждого сотрудника кроме Employee могут быть подчинённые.
Зарплата сотрудника рассчитывается следующим образом:
⦁	Employee - это базовая ставка плюс 3% за каждый год работы, но не больше 30% суммарной надбавки. 
⦁	Manager - это базовая ставка плюс 5% за каждый год работы, но не больше 40% суммарной надбавки за стаж работы. Плюс 0,5% зарплаты всех подчинённых первого уровня.
⦁	Salesman - это базовая ставка плюс 1% за каждый год работы в компании, но не больше 35% суммарной надбавки за стаж работы. Плюс 0,3% зарплаты всех подчинённых всех уровней.
⦁	У сотрудников (кроме Employee) может быть любое количество подчинённых любой группы.
Требуется: составить структуру классов, описывающих данную модель, а также реализовать алгоритм расчета зарплаты каждого сотрудника на произвольный момент времени, а также подсчёт суммарной зарплаты всех сотрудников фирмы в целом.
Замечание: при реализации тестового задания необходимо предположить, что вы разрабатываете не просто прототип, а систему enterprise уровня, соответственно важнее продемонстрировать архитектурно более красивое решение, даже в ущерб быстродействию. Не обязательно реализовывать всю архитектуру в полном объёме, не реализованные или упрощённые моменты нужно прокомментировать.
Решение: нужно сделать на C# с использованием sqlite, применяя любые библиотеки.
Код передать в виде репозитория git на github или аналоге.

Дополнительные плюсы:
[-]	Написан краткий обзор решения тестовой задачи, описана архитектура, ее плюсы и минусы (что можно улучшить, поменять или еще какие-то соображения для использования решения в реальных целях).
[*]	Код покрыт тестами — пока только unit, без интеграционных.
[mvc]	Программа имеет графический интерфейс.
[+]	Будет возможность просмотреть для выбранного сотрудника список его подчинённых.
[+]	Будет возможность добавлять новых сотрудников разных видов
[+]	Будет возможность разграничения прав, каждый сотрудник будет иметь свой логин/пароль, имея возможность просматривать только свою зарплату, и зарплату своих подчинённых. Также должен быть супер-пользователь, который имеет доступ ко всем.

# ФБ
Плюсы:
• Попытка использований паттерна фабрика
• Есть оптимизация по методу жадного алгоритма через кэш
• Есть вариант типа dto
• Аккуратный код
Минусы:
[+] ORM в пакетах есть, но использует ado.net
[+] ТЗ не выполнено, добавить/удалить/изменить ничего нельзя
[+] Из коробки не работало (пришлось допиливать проект), не правильно выставлена платформа относительно пакетов, нет копирования БД в папку
[+] Чистые sql запросы
[+] Все захардкожено
[+] Проблема с найменгом почему класс с I
[*] Разбор иерархии только первого уровня
 
# Баллы

[+]	Проект собирается и запускается
[+]	Использование git Есть git-ренпозиторий, Есть коммиты (не один)
[+]	Есть иерархия классов
[+]	Расчет ЗП по всем сотрудникам Сотрудник, менеджер, продажник
[+]	Форма логина Пароль хранится не в открытом виде
[+]	Использование ORM
[+]	Использование DI
[mvc] Валидация вставляемых данных
[+]	Создание БД	+1 балл за наполнение данных
[+]	Редактирование объектов	Добавление, редактирование, удаление
[*]	Форматирование кода	Субъективная оценка (Сахар, форматирование, нэйминг)
[-]	Архитектура	DAL, BL, etc... (разделения приложения на уровни работы с данными, слоя бизнес логики, отображения)
									
# Доп 1

[+] авто развертывание бд с миграцией
[+] использование каких либо паттернов (MVVM/MVC/MVP)
[+] ассинхронность;

# Доп 2

[+] Тесты и двойная реализация зависимостей.
 
Недостатки решения:
[+] Очень тяжело разбираться в проекте, когда много лишнего кода.
[+] Если проект Concole не рабочий, то его можно выделить в отдельную ветку, а в основной удалить.
[+] Эксперименты лучше вывести в отдельную ветку, не вести в рамках основной разработки.

Проблемы:
[*] Разделение на API и MVC предполагает общения через AJAX/REST, а не через БД, т.е. в данном случае я бы рекомендовал реализовать чистый mvc, что проще, чем Rest API + клиент на wpf или js.
[+] MVC\Фронт основан на стандартной генерации, сказать нечего.
[+] Консольное приложение не смотрел, т.к. написано что устарело и не развивается.
[-] API должно быть асинхронное, если выбран такой подход. Получение данных из бд асинхронная (тут как я понимая взято из примеров), а вот расчет ЗП (WebApiProgram) нет - нарушение подхода.
[+] Жаль, что не доделана миграция. Можно было бы все запускать на ms sql.
[*] В расчете используется наследование, но нет полиморфизма для типовых методов, т.е. оно сделано для галочки.
[+] Тесты не должны расшаривать общие данные, а создавать свое окружения самостоятельно, не влияя на другие тесты, статические поля антипаттерн.

- api convert db from sqlite to mssql
- mvc update db with authorization
- api add db seeding with demo data
- api combine repos
- api replace employee id by userid, add role
- api add salary authorization

1.8

- api add authorization controller, classes & repo, middleware, db tables

1.7.1

- delete console & tutorials
- edit api & mvc program & startup
- delete constants from tests
- delete tests by fake

1.7

- add endpoint & unit tests for subs salary
- add unit tests for salary endpoints
- edit api frontend
- repair console
- delete movie mvc tutorial

1.6

- add unit tests by fake class & moq framework
- change db context to repository in controller
- add repository di in startup
- add db migrations
- split db read to console & api methods
- delete tutorial parts of api & mvc

1.5

- add routing for endpoint return of calculations
- model, controller, context etc rename
- class program static to instance change
- extract program methods from application main
- add string interpolations
- add new migrations to mvc
- consoleapp src folder rename to console
- revert project dependency
- add mvcmovie tutorial
- add pic for 17/1/17