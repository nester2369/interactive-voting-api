\
@echo off
chcp 65001 >nul
setlocal

REM Файл запускает Web API из исходного кода и открывает Swagger для проверки проекта.
REM Перед запуском на компьютере должен быть установлен .NET 6 SDK или более новая версия.

set PORT=5055
set URL=http://localhost:%PORT%

where dotnet >nul 2>nul
if errorlevel 1 (
    echo .NET SDK не найден. Установи .NET 6 SDK или более новую версию.
    pause
    exit /b 1
)

echo Восстановление NuGet-пакетов...
dotnet restore
if errorlevel 1 (
    echo Ошибка восстановления пакетов.
    pause
    exit /b 1
)

echo Запуск проекта на %URL% ...
start "" %URL%/swagger
dotnet run --urls %URL%

pause
