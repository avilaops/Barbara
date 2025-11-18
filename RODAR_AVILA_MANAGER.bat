w@echo off
chcp 65001 >nul
color 0B
cls

echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.
echo      █████╗ ██╗   ██╗██╗██╗      █████╗     ███╗   ███╗ █████╗ ███╗   ██╗ █████╗  ██████╗ ███████╗██████╗ 
echo     ██╔══██╗██║   ██║██║██║     ██╔══██╗    ████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝ ██╔════╝██╔══██╗
echo     ███████║██║   ██║██║██║     ███████║    ██╔████╔██║███████║██╔██╗ ██║███████║██║  ███╗█████╗  ██████╔╝
echo     ██╔══██║╚██╗ ██╔╝██║██║     ██╔══██║    ██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║   ██║██╔══╝  ██╔══██╗
echo     ██║  ██║ ╚████╔╝ ██║███████╗██║  ██║    ██║ ╚═╝ ██║██║  ██║██║ ╚████║██║  ██║╚██████╔╝███████╗██║  ██║
echo     ╚═╝  ╚═╝  ╚═══╝  ╚═╝╚══════╝╚═╝  ╚═╝    ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝
echo.
echo                            🎯 Gerenciador Visual de Aplicações - Framework Avila
echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.
echo  📦 Aplicações Disponíveis:
echo     ⚡ Barbara API      🏗️ Shancrys BIM      🔐 Secreta
echo     🗺️ Geolocation      📊 On Dashboard      🥖 Padaria Vendas
echo     📄 Report Framework 🪟 Windows Optimizer  💼 Camacho
echo.
echo  🌐 Interface Web: http://localhost:8081
echo  🖥️  Tecnologia: Python + NiceGUI
echo.
echo ───────────────────────────────────────────────────────────────────────────────
echo.

echo [1/3] 🛑 Parando processos anteriores...
taskkill /F /IM python.exe >nul 2>&1
timeout /t 1 /nobreak >nul
echo       ✅ Processos limpos!

echo.
echo [2/3] 📂 Navegando para Scripts...
cd /d "%~dp0..\..\Scripts"
echo       ✅ Diretório: %CD%

echo.
echo [3/3] 🚀 Iniciando Avila Manager...
echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.
echo     🔥 AVILA MANAGER ESTÁ SUBINDO!
echo.
echo     📍 Acesse: http://localhost:8081
echo.
echo     💡 O navegador abrirá automaticamente em 3 segundos...
echo     💡 Pressione Ctrl+C para parar o servidor
echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.

timeout /t 3 /nobreak >nul
start http://localhost:8081

python avila-manager.py

echo.
echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.
echo     👋 Avila Manager foi encerrado!
echo.
echo ═══════════════════════════════════════════════════════════════════════════════
echo.
pause
