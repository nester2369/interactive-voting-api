# Как правильно добавить проект на GitHub

## 1. Подготовь папку проекта

В папке проекта должны быть исходники и файлы запуска:

- `Program.cs`;
- `Inter.MixAN.csproj`;
- `Controllers/`;
- `Services/`;
- `Data/`;
- `Domain/`;
- `DTOs/`;
- `appsettings.json`;
- `START_PROJECT.bat`;
- `README.md`;
- `.gitignore`;
- `docs/ERD.md`;
- `docs/UML_MODULES.md`;
- `database/schema.sql`.

Не добавляй в репозиторий:

- `bin/`;
- `obj/`;
- `*.exe`;
- `*.dll`;
- `*.db`;
- временные файлы IDE.

Эти файлы уже закрыты через `.gitignore`.

## 2. Создай репозиторий на GitHub

1. Зайди на GitHub.
2. Нажми `New repository`.
3. Введи название, например `interactive-voting-api`.
4. Выбери `Public`.
5. Не ставь галочку `Add a README file`, потому что README уже есть в проекте.
6. Нажми `Create repository`.

## 3. Загрузи проект через Git Bash или терминал

Открой терминал в папке проекта и выполни команды:

```bash
git init
git add .
git commit -m "Initial commit"
git branch -M main
git remote add origin https://github.com/USERNAME/interactive-voting-api.git
git push -u origin main
```

Замени `USERNAME` на свой логин GitHub.

## 4. Проверь репозиторий после загрузки

На GitHub должны отображаться:

- исходный код `.cs`;
- `README.md`;
- диаграммы в папке `docs/`;
- файл запуска `START_PROJECT.bat`;
- настройки проекта `.csproj` и `appsettings.json`;
- `.gitignore`.

В репозитории не должно быть папок `bin/` и `obj/`. Если они появились, удали их командами:

```bash
git rm -r --cached bin obj
git commit -m "Remove build artifacts"
git push
```

## 5. Что отправить на проверку

В работу или отчет добавь ссылку на публичный репозиторий GitHub. Пример формулировки:

```text
Исходный код практической части дипломного проекта размещен в открытом репозитории GitHub: https://github.com/USERNAME/interactive-voting-api
```

Замени ссылку на свою после публикации.
