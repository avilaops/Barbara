# Script de gerenciamento Docker para Bárbara
# Uso: .\docker.ps1 [comando]

param(
    [Parameter(Position=0)]
    [ValidateSet('build', 'up', 'down', 'restart', 'logs', 'status', 'ngrok-url', 'clean')]
    [string]$Command = 'up'
)

$ErrorActionPreference = "Stop"

# Cores para output
function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Warning { Write-Host $args -ForegroundColor Yellow }
function Write-Error { Write-Host $args -ForegroundColor Red }

# Banner
Write-Info @"
╔═══════════════════════════════════════╗
║     🎀 Bárbara Docker Manager 🎀     ║
╔═══════════════════════════════════════╗
"@

switch ($Command) {
    'build' {
        Write-Info "🔨 Construindo imagens Docker..."
        docker-compose build --no-cache
        Write-Success "✅ Build concluído!"
    }
    
    'up' {
        Write-Info "🚀 Iniciando containers..."
        docker-compose up -d
        Start-Sleep -Seconds 5
        Write-Success "✅ Containers iniciados!"
        Write-Info "`n📊 Status dos serviços:"
        docker-compose ps
        Write-Info "`n🌐 Aguarde alguns segundos e verifique a URL do ngrok:"
        Write-Warning "   http://localhost:4040"
    }
    
    'down' {
        Write-Info "🛑 Parando containers..."
        docker-compose down
        Write-Success "✅ Containers parados!"
    }
    
    'restart' {
        Write-Info "🔄 Reiniciando containers..."
        docker-compose restart
        Write-Success "✅ Containers reiniciados!"
    }
    
    'logs' {
        Write-Info "📋 Exibindo logs (Ctrl+C para sair)..."
        docker-compose logs -f
    }
    
    'status' {
        Write-Info "📊 Status dos containers:"
        docker-compose ps
        Write-Info "`n🏥 Health checks:"
        docker ps --format "table {{.Names}}\t{{.Status}}"
    }
    
    'ngrok-url' {
        Write-Info "🔗 Obtendo URL pública do ngrok..."
        try {
            $response = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels"
            $url = $response.tunnels[0].public_url
            Write-Success "`n✨ URL Pública da API Bárbara:"
            Write-Host "   $url" -ForegroundColor White -BackgroundColor Blue
            Write-Info "`n📋 Interface Web do ngrok:"
            Write-Host "   http://localhost:4040" -ForegroundColor White
        } catch {
            Write-Error "❌ Erro ao obter URL do ngrok. Verifique se os containers estão rodando."
        }
    }
    
    'clean' {
        Write-Warning "🧹 Limpando todos os containers, volumes e imagens..."
        $confirm = Read-Host "Tem certeza? (s/N)"
        if ($confirm -eq 's' -or $confirm -eq 'S') {
            docker-compose down -v --rmi all
            Write-Success "✅ Limpeza concluída!"
        } else {
            Write-Info "Operação cancelada."
        }
    }
}

Write-Info "`n💡 Comandos disponíveis:"
Write-Host "   build      - Construir imagens"
Write-Host "   up         - Iniciar containers"
Write-Host "   down       - Parar containers"
Write-Host "   restart    - Reiniciar containers"
Write-Host "   logs       - Exibir logs"
Write-Host "   status     - Status dos containers"
Write-Host "   ngrok-url  - Obter URL pública"
Write-Host "   clean      - Limpar tudo"
