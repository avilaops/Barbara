#!/bin/bash

# 🚀 Script de Deploy Bárbara
# Escolha seu método de deploy

echo ""
echo "╔════════════════════════════════════════════╗"
echo "║   🚀 Deploy Bárbara - Escolha a Opção    ║"
echo "╚════════════════════════════════════════════╝"
echo ""

echo "1. 🌐 Azure (Completo - Recomendado)"
echo "2. 🚂 Railway (Rápido e Simples)"
echo "3. ▲ Vercel (Backend Serverless)"
echo "4. 🐳 Docker + Ngrok (Público temporário)"
echo "5. ❌ Cancelar"
echo ""

read -p "Escolha uma opção (1-5): " option

case $option in
  1)
    echo ""
    echo "📦 Preparando deploy para Azure..."
    echo ""
    
    # Verificar Azure CLI
    if ! command -v az &> /dev/null; then
        echo "❌ Azure CLI não encontrado!"
        echo "📥 Instale em: https://docs.microsoft.com/cli/azure/install-azure-cli"
        exit 1
    fi
    
    # Login
    echo "🔐 Fazendo login no Azure..."
    az login
    
    # Criar Resource Group
    echo "📦 Criando Resource Group..."
    az group create --name barbara-rg --location eastus
    
    # Criar App Service Plan
    echo "📦 Criando App Service Plan..."
    az appservice plan create \
      --name barbara-plan \
      --resource-group barbara-rg \
      --sku F1 \
      --is-linux
    
    # Criar Web App
    echo "🌐 Criando Web App..."
    az webapp create \
      --name barbara-api-$(date +%s) \
      --resource-group barbara-rg \
      --plan barbara-plan \
      --runtime "NODE:20-lts"
    
    echo ""
    echo "✅ Azure configurado!"
    echo "📝 Próximos passos:"
    echo "   1. Configure variáveis de ambiente no Azure Portal"
    echo "   2. Faça git push para deploy"
    echo ""
    ;;
    
  2)
    echo ""
    echo "🚂 Preparando deploy para Railway..."
    echo ""
    
    # Verificar Railway CLI
    if ! command -v railway &> /dev/null; then
        echo "📥 Instalando Railway CLI..."
        npm i -g @railway/cli
    fi
    
    # Login
    echo "🔐 Fazendo login no Railway..."
    railway login
    
    # Deploy API
    cd api
    echo "🚀 Deploy da API..."
    railway init
    railway up
    
    echo ""
    echo "✅ Deploy Railway concluído!"
    echo "🌐 Configure variáveis e obtenha URL:"
    echo "   railway variables set MONGODB_URI=\"<connection-string>\""
    echo "   railway domain"
    echo ""
    ;;
    
  3)
    echo ""
    echo "▲ Preparando deploy para Vercel..."
    echo ""
    
    # Verificar Vercel CLI
    if ! command -v vercel &> /dev/null; then
        echo "📥 Instalando Vercel CLI..."
        npm i -g vercel
    fi
    
    # Login
    echo "🔐 Fazendo login no Vercel..."
    vercel login
    
    # Deploy API
    cd api
    echo "🚀 Deploy da API..."
    vercel --prod
    
    echo ""
    echo "✅ Deploy Vercel concluído!"
    echo "🌐 URL disponível em: https://<seu-projeto>.vercel.app"
    echo ""
    ;;
    
  4)
    echo ""
    echo "🐳 Iniciando Docker + Ngrok (público temporário)..."
    echo ""
    
    # Verificar Docker
    if ! command -v docker &> /dev/null; then
        echo "❌ Docker não encontrado! Instale primeiro."
        exit 1
    fi
    
    # Iniciar containers
    echo "🚀 Iniciando containers..."
    docker-compose up -d
    
    sleep 5
    
    # Verificar ngrok
    echo "🌐 Verificando URL pública do Ngrok..."
    curl -s http://localhost:4040/api/tunnels | grep -o '"public_url":"[^"]*"' | cut -d'"' -f4
    
    echo ""
    echo "✅ Aplicação rodando!"
    echo "📝 URLs:"
    echo "   Local:  http://localhost:3000"
    echo "   Público: (veja acima)"
    echo "   Admin:  http://localhost:4040"
    echo ""
    ;;
    
  5)
    echo ""
    echo "❌ Deploy cancelado."
    exit 0
    ;;
    
  *)
    echo ""
    echo "❌ Opção inválida!"
    exit 1
    ;;
esac

echo ""
echo "╔════════════════════════════════════════════╗"
echo "║         ✅ Deploy Concluído!              ║"
echo "╚════════════════════════════════════════════╝"
echo ""
