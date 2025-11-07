# 🚀 Script de Deploy Bárbara (PowerShell)

Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║   🚀 Deploy Bárbara - Escolha a Opção    ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

Write-Host "1. 🌐 Azure (Completo - Recomendado)" -ForegroundColor Green
Write-Host "2. 🚂 Railway (Rápido e Simples)" -ForegroundColor Yellow
Write-Host "3. ▲ Vercel (Backend Serverless)" -ForegroundColor Blue
Write-Host "4. 🐳 Docker + Ngrok (Público temporário)" -ForegroundColor Magenta
Write-Host "5. ❌ Cancelar" -ForegroundColor Red
Write-Host ""

$option = Read-Host "Escolha uma opção (1-5)"

switch ($option) {
    "1" {
        Write-Host ""
        Write-Host "📦 Preparando deploy para Azure..." -ForegroundColor Cyan
        Write-Host ""
        
        # Verificar Azure CLI
        if (-not (Get-Command az -ErrorAction SilentlyContinue)) {
            Write-Host "❌ Azure CLI não encontrado!" -ForegroundColor Red
            Write-Host "📥 Instale em: https://docs.microsoft.com/cli/azure/install-azure-cli" -ForegroundColor Yellow
            exit 1
        }
        
        # Login
        Write-Host "🔐 Fazendo login no Azure..." -ForegroundColor Yellow
        az login
        
        # Criar Resource Group
        Write-Host "📦 Criando Resource Group..." -ForegroundColor Yellow
        az group create --name barbara-rg --location eastus
        
        # Criar App Service Plan
        Write-Host "📦 Criando App Service Plan..." -ForegroundColor Yellow
        az appservice plan create `
            --name barbara-plan `
            --resource-group barbara-rg `
            --sku F1 `
            --is-linux
        
        # Nome único para Web App
        $timestamp = [int][double]::Parse((Get-Date -UFormat %s))
        $webAppName = "barbara-api-$timestamp"
        
        # Criar Web App
        Write-Host "🌐 Criando Web App: $webAppName..." -ForegroundColor Yellow
        az webapp create `
            --name $webAppName `
            --resource-group barbara-rg `
            --plan barbara-plan `
            --runtime "NODE:20-lts"
        
        Write-Host ""
        Write-Host "✅ Azure configurado!" -ForegroundColor Green
        Write-Host "📝 Próximos passos:" -ForegroundColor Cyan
        Write-Host "   1. Configure variáveis no Azure Portal" -ForegroundColor White
        Write-Host "   2. Faça git push para deploy" -ForegroundColor White
        Write-Host "   3. URL: https://$webAppName.azurewebsites.net" -ForegroundColor Green
        Write-Host ""
    }
    
    "2" {
        Write-Host ""
        Write-Host "🚂 Preparando deploy para Railway..." -ForegroundColor Yellow
        Write-Host ""
        
        # Verificar Railway CLI
        if (-not (Get-Command railway -ErrorAction SilentlyContinue)) {
            Write-Host "📥 Instalando Railway CLI..." -ForegroundColor Yellow
            npm i -g @railway/cli
        }
        
        # Login
        Write-Host "🔐 Fazendo login no Railway..." -ForegroundColor Yellow
        railway login
        
        # Deploy API
        Set-Location api
        Write-Host "🚀 Deploy da API..." -ForegroundColor Yellow
        railway init
        railway up
        
        Write-Host ""
        Write-Host "✅ Deploy Railway concluído!" -ForegroundColor Green
        Write-Host "🌐 Configure variáveis e obtenha URL:" -ForegroundColor Cyan
        Write-Host '   railway variables set MONGODB_URI="<connection-string>"' -ForegroundColor White
        Write-Host "   railway domain" -ForegroundColor White
        Write-Host ""
        Set-Location ..
    }
    
    "3" {
        Write-Host ""
        Write-Host "▲ Preparando deploy para Vercel..." -ForegroundColor Blue
        Write-Host ""
        
        # Verificar Vercel CLI
        if (-not (Get-Command vercel -ErrorAction SilentlyContinue)) {
            Write-Host "📥 Instalando Vercel CLI..." -ForegroundColor Yellow
            npm i -g vercel
        }
        
        # Login
        Write-Host "🔐 Fazendo login no Vercel..." -ForegroundColor Yellow
        vercel login
        
        # Deploy API
        Set-Location api
        Write-Host "🚀 Deploy da API..." -ForegroundColor Yellow
        vercel --prod
        
        Write-Host ""
        Write-Host "✅ Deploy Vercel concluído!" -ForegroundColor Green
        Write-Host "🌐 URL disponível em: https://<seu-projeto>.vercel.app" -ForegroundColor Cyan
        Write-Host ""
        Set-Location ..
    }
    
    "4" {
        Write-Host ""
        Write-Host "🐳 Iniciando Docker + Ngrok (público temporário)..." -ForegroundColor Magenta
        Write-Host ""
        
        # Verificar Docker
        if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
            Write-Host "❌ Docker não encontrado! Instale primeiro." -ForegroundColor Red
            exit 1
        }
        
        # Iniciar containers
        Write-Host "🚀 Iniciando containers..." -ForegroundColor Yellow
        docker-compose up -d
        
        Start-Sleep -Seconds 5
        
        # Verificar ngrok
        Write-Host "🌐 Verificando URL pública do Ngrok..." -ForegroundColor Yellow
        $tunnels = Invoke-RestMethod -Uri "http://localhost:4040/api/tunnels"
        $publicUrl = $tunnels.tunnels | Where-Object { $_.proto -eq "https" } | Select-Object -ExpandProperty public_url
        
        Write-Host ""
        Write-Host "✅ Aplicação rodando!" -ForegroundColor Green
        Write-Host "📝 URLs:" -ForegroundColor Cyan
        Write-Host "   Local:  http://localhost:3000" -ForegroundColor White
        Write-Host "   Público: $publicUrl" -ForegroundColor Green
        Write-Host "   Admin:  http://localhost:4040" -ForegroundColor White
        Write-Host ""
    }
    
    "5" {
        Write-Host ""
        Write-Host "❌ Deploy cancelado." -ForegroundColor Red
        exit 0
    }
    
    default {
        Write-Host ""
        Write-Host "❌ Opção inválida!" -ForegroundColor Red
        exit 1
    }
}

Write-Host ""
Write-Host "╔════════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║         ✅ Deploy Concluído!              ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
