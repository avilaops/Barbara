# Script de gerenciamento Docker para Bárbara# Script de gerenciamento Docker para Bárbara

# Uso: .\docker.ps1 [comando]# Uso: .\docker.ps1 [comando]



param(param(

    [Parameter(Position=0)]    [Parameter(Position=0)]

    [ValidateSet('build', 'up', 'down', 'restart', 'logs', 'status', 'ngrok-url', 'clean')]    [ValidateSet('build', 'up', 'down', 'restart', 'logs', 'status', 'ngrok-url', 'clean')]

    [string]$Command = 'up'    [string]$Command = 'up'

))



$ErrorActionPreference = "Stop"$ErrorActionPreference = "Stop"



# Cores para output# Cores para output

function Write-Success { Write-Host $args -ForegroundColor Green }function Write-Success { Write-Host $args -ForegroundColor Green }

function Write-Info { Write-Host $args -ForegroundColor Cyan }function Write-Info { Write-Host $args -ForegroundColor Cyan }

function Write-Warning { Write-Host $args -ForegroundColor Yellow }function Write-Warning { Write-Host $args -ForegroundColor Yellow }

function Write-Error { Write-Host $args -ForegroundColor Red }

# Banner

Write-Info @"# Banner

╔═══════════════════════════════════════╗Write-Info @"

║     🎀 Bárbara Docker Manager 🎀     ║╔═══════════════════════════════════════╗

╔═══════════════════════════════════════╗║     🎀 Bárbara Docker Manager 🎀     ║

"@╔═══════════════════════════════════════╗

"@

switch ($Command) {

    'build' {switch ($Command) {

        Write-Info "🔨 Construindo imagens Docker..."    'build' {

        docker-compose build --no-cache        Write-Info "🔨 Construindo imagens Docker..."

        Write-Success "✅ Build concluído!"        docker-compose build --no-cache

    }        Write-Success "✅ Build concluído!"

        }

    'up' {    

        Write-Info "🚀 Iniciando containers..."    'up' {

        docker-compose up -d        Write-Info "🚀 Iniciando containers..."

        Start-Sleep -Seconds 5        docker-compose up -d

        Write-Success "✅ Containers iniciados!"        Start-Sleep -Seconds 5

        Write-Info "`n📊 Status dos serviços:"        Write-Success "✅ Containers iniciados!"

        docker-compose ps        Write-Info "`n📊 Status dos serviços:"

        Write-Info "`n🌐 Aguarde alguns segundos e verifique a URL do ngrok:"        docker-compose ps

        Write-Warning "   http://localhost:4040"        Write-Info "`n🌐 Aguarde alguns segundos e verifique a URL do ngrok:"

    }        Write-Warning "   http://localhost:4040"

        }

    'down' {    

        Write-Info "🛑 Parando containers..."    'down' {

        docker-compose down        Write-Info "🛑 Parando containers..."

        Write-Success "✅ Containers parados!"        docker-compose down

    }        Write-Success "✅ Containers parados!"

        }

    'restart' {    

        Write-Info "🔄 Reiniciando containers..."    'restart' {

        docker-compose restart        Write-Info "🔄 Reiniciando containers..."

        Write-Success "✅ Containers reiniciados!"        docker-compose restart

    }        Write-Success "✅ Containers reiniciados!"

        }

    'logs' {    

        Write-Info "📋 Exibindo logs (Ctrl+C para sair)..."    'logs' {

        docker-compose logs -f        Write-Info "📋 Exibindo logs (Ctrl+C para sair)..."

    }        docker-compose logs -f

        }

    'status' {    

        Write-Info "📊 Status dos containers:"    'status' {

        docker-compose ps        Write-Info "📊 Status dos containers:"

        Write-Info "`n🏥 Health checks:"        docker-compose ps

        docker ps --format "table {{.Names}}\t{{.Status}}"        Write-Info "`n🏥 Health checks:"

    }        docker ps --format "table {{.Names}}\t{{.Status}}"

        }

    'ngrok-url' {    

        Write-Info "🔗 Obtendo URL pública do ngrok..."    'ngrok-url' {

        try {        Write-Info "🔗 Obtendo URL pública do ngrok..."

            $response = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels"        try {

            $url = $response.tunnels[0].public_url            $response = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels"

            Write-Success "`n✨ URL Pública da API Bárbara:"            $url = $response.tunnels[0].public_url

            Write-Host "   $url" -ForegroundColor White -BackgroundColor Blue            Write-Success "`n✨ URL Pública da API Bárbara:"

            Write-Info "`n📋 Interface Web do ngrok:"            Write-Host "   $url" -ForegroundColor White -BackgroundColor Blue

            Write-Host "   http://localhost:4040" -ForegroundColor White            Write-Info "`n📋 Interface Web do ngrok:"

        } catch {            Write-Host "   http://localhost:4040" -ForegroundColor White

            Write-Host "❌ Erro ao obter URL do ngrok. Verifique se os containers estão rodando." -ForegroundColor Red        } catch {

        }            Write-Error "❌ Erro ao obter URL do ngrok. Verifique se os containers estão rodando."

    }        }

        }

    'clean' {    

        Write-Warning "🧹 Limpando todos os containers, volumes e imagens..."    'clean' {

        $confirm = Read-Host "Tem certeza? (s/N)"        Write-Warning "🧹 Limpando todos os containers, volumes e imagens..."

        if ($confirm -eq 's' -or $confirm -eq 'S') {        $confirm = Read-Host "Tem certeza? (s/N)"

            docker-compose down -v --rmi all        if ($confirm -eq 's' -or $confirm -eq 'S') {

            Write-Success "✅ Limpeza concluída!"            docker-compose down -v --rmi all

        } else {            Write-Success "✅ Limpeza concluída!"

            Write-Info "Operação cancelada."        } else {

        }            Write-Info "Operação cancelada."

    }        }

}    }

}

Write-Info "`n💡 Comandos disponíveis:"

Write-Host "   build      - Construir imagens"Write-Info "`n💡 Comandos disponíveis:"

Write-Host "   up         - Iniciar containers"Write-Host "   build      - Construir imagens"

Write-Host "   down       - Parar containers"Write-Host "   up         - Iniciar containers"

Write-Host "   restart    - Reiniciar containers"Write-Host "   down       - Parar containers"

Write-Host "   logs       - Ver logs em tempo real"Write-Host "   restart    - Reiniciar containers"

Write-Host "   status     - Status dos containers"Write-Host "   logs       - Ver logs em tempo real"

Write-Host "   ngrok-url  - Obter URL pública"Write-Host "   status     - Status dos containers"

Write-Host "   clean      - Limpar tudo (CUIDADO!)"Write-Host "   ngrok-url  - Obter URL pública"

Write-Host "   clean      - Limpar tudo (CUIDADO!)"

Write-Info "`n💡 Comandos disponíveis:"
Write-Host "   build      - Construir imagens"
Write-Host "   up         - Iniciar containers"
Write-Host "   down       - Parar containers"
Write-Host "   restart    - Reiniciar containers"
Write-Host "   logs       - Exibir logs"
Write-Host "   status     - Status dos containers"
Write-Host "   ngrok-url  - Obter URL pública"
Write-Host "   clean      - Limpar tudo"
