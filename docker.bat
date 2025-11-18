@echo off
chcp 65001 >nul
title Bárbara Docker Manager

echo.
echo ╔═══════════════════════════════════════╗
echo ║     🎀 Bárbara Docker Manager 🎀     ║
echo ╚═══════════════════════════════════════╝
echo.

if "%1"=="" goto menu
if "%1"=="up" goto up
if "%1"=="down" goto down
if "%1"=="url" goto url
if "%1"=="logs" goto logs
if "%1"=="status" goto status
goto menu

:menu
echo Escolha uma opção:
echo.
echo [1] Iniciar containers
echo [2] Parar containers
echo [3] Ver URL do ngrok
echo [4] Ver logs
echo [5] Ver status
echo [0] Sair
echo.
set /p choice="Digite o número: "

if "%choice%"=="1" goto up
if "%choice%"=="2" goto down
if "%choice%"=="3" goto url
if "%choice%"=="4" goto logs
if "%choice%"=="5" goto status
if "%choice%"=="0" goto end
goto menu

:up
echo.
echo 🚀 Iniciando containers...
docker-compose up -d
timeout /t 5 /nobreak >nul
echo.
echo ✅ Containers iniciados!
echo.
echo 💡 Aguarde alguns segundos e execute:
echo    docker.bat url
echo.
if "%1"=="" pause
goto end

:down
echo.
echo 🛑 Parando containers...
docker-compose down
echo ✅ Containers parados!
if "%1"=="" pause
goto end

:url
echo.
echo 🔗 Obtendo URL pública do ngrok...
echo.
powershell -Command "try { $r = Invoke-RestMethod -Uri 'http://localhost:4040/api/tunnels'; Write-Host '✨ URL Pública:' -ForegroundColor Green; Write-Host $r.tunnels[0].public_url -ForegroundColor Cyan } catch { Write-Host '❌ Erro: Verifique se os containers estão rodando' -ForegroundColor Red }"
echo.
echo 📋 Dashboard: http://localhost:4040
echo.
if "%1"=="" pause
goto end

:logs
echo.
echo 📋 Logs (Ctrl+C para sair)...
docker-compose logs -f
goto end

:status
echo.
echo 📊 Status dos containers:
docker-compose ps
echo.
if "%1"=="" pause
goto end

:end
