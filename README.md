## Приложение учёта клиентов и заказов (WinForms + SQLite)

Настольное приложение для сервисной компании на C#/.NET Framework 4.7.2 с использованием **WinForms** и **SQLite**.  
Приложение позволяет вести учёт клиентов и их заказов, работать с ролями пользователей и ограничениями доступа.

### Основные возможности

- **Авторизация пользователей**
  - Логин/пароль
  - Роли: Guest, Operator, Admin
- **Работа с клиентами**
  - Просмотр списка клиентов (`DataGridView`)
  - Добавление / редактирование
- **Работа с заказами**
  - Привязка заказа к клиенту
  - Статусы: *Новый, В работе, Завершён*
  - Просмотр, добавление и изменение статуса
- **Управление пользователями** (только Admin)
  - Создание, редактирование логина/пароля и роли
- **Ограничение доступа по ролям**
  - Guest – только просмотр
  - Operator – работа с клиентами и заказами
  - Admin – полный доступ, в том числе к пользователям
- **Современный тёмный UI**
  - Единая тема `UiTheme` для всех форм (кнопки, таблицы, фон, шрифты)

### Технологии

- .NET Framework 4.7.2
- WinForms
- SQLite (`System.Data.SQLite`)
- `DataGridView` для отображения данных

---

## Структура проекта

- `Program.cs` – точка входа, инициализация SQLite и запуск `LoginForm`
- `Database.cs` – работа с SQLite, создание файла БД и таблиц при первом запуске
- `Models.cs` – модели и перечисления (`User`, `UserRole`)
- `UiTheme.cs` – оформление интерфейса (единая тема для всех форм)
- Формы:
  - `LoginForm` – авторизация
  - `MainForm` – главное окно и навигация
  - `ClientsForm` – управление клиентами
  - `OrdersForm` – управление заказами
  - `UsersForm` – управление пользователями

---

## Подготовка и запуск

### 1. Клонирование репозитория

```bash
git clone https://github.com/johanstt/praktica.git
cd praktica/praktica
```

### 2. Открытие решения

1. Открыть `praktica.sln` в Visual Studio.
2. Убедиться, что выбрана конфигурация **Debug** и платформа **x64** (для поддержки SQLite).

### 3. Установка NuGet-пакетов

Через **Управление пакетами NuGet** для проекта `praktica` убедиться, что установлены:

- `System.Data.SQLite` (2.x, таргет `net472`)
- `SQLitePCLRaw.bundle_e_sqlite3` (та же мажорная версия)

Если пакеты не подтянулись автоматически – установить их вручную.

### 4. Сборка и запуск

1. **Build → Rebuild Solution** / «Пересобрать решение».
2. Запуск (F5).
3. При первом запуске автоматически создаётся файл БД `service_company.db` в папке с приложением и необходимые таблицы.

---

## Данные для входа

Начальные пользователи создаются в `Database.Initialize()`:

- **Гость**
  - Логин: `guest`
  - Пароль: `guest`
  - Роль: `Guest`

- **Оператор**
  - Логин: `operator`
  - Пароль: `operator`
  - Роль: `Operator`

- **Администратор**
  - Логин: `admin`
  - Пароль: `admin`
  - Роль: `Admin`

Рекомендуется после первого входа под `admin` изменить пароль через форму пользователей.

---

## SQL-скрипт создания базы данных

Таблицы создаются автоматически в `Database.Initialize()`, но при необходимости БД можно развернуть вручную.  
Пример SQL-скрипта (SQLite):

```sql
CREATE TABLE IF NOT EXISTS Users (
    Id       INTEGER PRIMARY KEY AUTOINCREMENT,
    UserName TEXT    NOT NULL UNIQUE,
    Password TEXT    NOT NULL,
    Role     TEXT    NOT NULL
);

CREATE TABLE IF NOT EXISTS Clients (
    Id    INTEGER PRIMARY KEY AUTOINCREMENT,
    Name  TEXT,
    Phone TEXT,
    Email TEXT
);

CREATE TABLE IF NOT EXISTS Orders (
    Id          INTEGER PRIMARY KEY AUTOINCREMENT,
    ClientId    INTEGER NOT NULL,
    Description TEXT    NOT NULL,
    Status      TEXT    NOT NULL,
    CreatedAt   TEXT    NOT NULL,
    FOREIGN KEY (ClientId) REFERENCES Clients (Id)
);

INSERT INTO Users (UserName, Password, Role) VALUES
('guest', 'guest', 'Guest'),
('operator', 'operator', 'Operator'),
('admin', 'admin', 'Admin');
```

---

## Как выложить проект в свой GitHub (шаги для lokalного проекта)

1. В корне репозитория (`c:\Users\Admin\source\repos\praktica`) выполнить в терминале:

```bash
git init
git add .
git commit -m "Initial version of WinForms service company app"
```

2. На GitHub под аккаунтом **`johanstt`** создать новый репозиторий, например `praktica` (без README, .gitignore и т.п.).
3. Связать локальный репозиторий с GitHub и запушить:

```bash
git remote add origin https://github.com/johanstt/praktica.git
git branch -M main
git push -u origin main
```

После этого проект будет доступен в вашем репозитории `johanstt/praktica`.


