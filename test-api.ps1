# Script de teste da API Bárbara
# Uso: .\test-api.ps1

$ErrorActionPreference = "Continue"

function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Error { Write-Host $args -ForegroundColor Red }

Write-Info @"
╔═══════════════════════════════════════╗
║     🧪 Testador API Bárbara 🧪       ║
╔═══════════════════════════════════════╗
"@

$baseUrl = "http://localhost:3000"

# Teste 1: Health Check
Write-Info "`n🏥 Teste 1: Health Check"
try {
    $health = Invoke-RestMethod -Uri "$baseUrl/health" -Method Get
    Write-Success "✅ API está funcionando!"
    Write-Host ($health | ConvertTo-Json -Depth 5)
} catch {
    Write-Error "❌ API não está respondendo. Execute: .\docker.ps1 up"
    exit 1
}

# Teste 2: Criar Job de Avatar
Write-Info "`n🎨 Teste 2: Criar Job de Avatar"
try {
    $avatarRequest = @{
        userId = "teste-usuario"
        frontImageUrl = "https://example.com/pessoa-frente.jpg"
        sideImageUrl = "https://example.com/pessoa-lado.jpg"
    } | ConvertTo-Json

    $response = Invoke-RestMethod `
        -Uri "$baseUrl/avatar/generate" `
        -Method Post `
        -ContentType "application/json" `
        -Body $avatarRequest

    $requestId = $response.requestId
    Write-Success "✅ Job criado com sucesso!"
    Write-Host "   Request ID: $requestId"
    Write-Host "   Status: $($response.status)"
    
    # Teste 3: Verificar Status do Job
    Write-Info "`n🔍 Teste 3: Verificar Status do Job"
    Start-Sleep -Seconds 2
    
    $status = Invoke-RestMethod -Uri "$baseUrl/avatar/$requestId" -Method Get
    Write-Success "✅ Status obtido:"
    Write-Host ($status | ConvertTo-Json -Depth 5)
    
} catch {
    Write-Error "❌ Erro ao criar job: $($_.Exception.Message)"
}

# Teste 4: Listar Jobs do Usuário
Write-Info "`n📋 Teste 4: Listar Todos os Jobs"
try {
    $jobs = Invoke-RestMethod -Uri "$baseUrl/avatar?userId=teste-usuario" -Method Get
    Write-Success "✅ Jobs encontrados: $($jobs.jobs.Count)"
    Write-Host ($jobs | ConvertTo-Json -Depth 5)
} catch {
    Write-Error "❌ Erro ao listar jobs: $($_.Exception.Message)"
}

# Teste 5: Testar Rate Limit
Write-Info "`n⏱️  Teste 5: Testar Rate Limiting"
Write-Host "Enviando 12 requests rápidas (limite é 10)..."
$successCount = 0
$rateLimitCount = 0

for ($i = 1; $i -le 12; $i++) {
    try {
        $response = Invoke-WebRequest `
            -Uri "$baseUrl/avatar/generate" `
            -Method Post `
            -ContentType "application/json" `
            -Body $avatarRequest `
            -ErrorAction Stop
        
        $remaining = $response.Headers['ratelimit-remaining']
        Write-Host "  Request $i`: ✅ Status $($response.StatusCode) - Remaining: $remaining"
        $successCount++
    } catch {
        if ($_.Exception.Response.StatusCode -eq 429) {
            Write-Warning "  Request $i`: ⚠️  Rate limited (429 Too Many Requests)"
            $rateLimitCount++
        } else {
            Write-Error "  Request $i`: ❌ Erro: $($_.Exception.Message)"
        }
    }
    Start-Sleep -Milliseconds 100
}

Write-Info "`nResultado do teste de rate limit:"
Write-Host "  ✅ Sucesso: $successCount"
Write-Host "  ⚠️  Rate limited: $rateLimitCount"

if ($rateLimitCount -gt 0) {
    Write-Success "`n✅ Rate limiting está funcionando corretamente!"
} else {
    Write-Warning "`n⚠️  Rate limiting pode não estar ativo"
}

# Teste 6: Testar Catálogo
Write-Info "`n📦 Teste 6: Testar Catálogo de Produtos"
try {
    $product = @{
        sku = "TEST-$(Get-Random)"
        name = "Camiseta Teste"
        description = "Produto de teste"
        price = 99.90
        category = "roupas"
        imageUrl = "https://example.com/shirt.jpg"
        model3dUrl = "https://example.com/shirt.glb"
    } | ConvertTo-Json

    $newProduct = Invoke-RestMethod `
        -Uri "$baseUrl/catalog" `
        -Method Post `
        -ContentType "application/json" `
        -Body $product

    Write-Success "✅ Produto criado:"
    Write-Host ($newProduct | ConvertTo-Json -Depth 5)
    
    # Listar produtos
    $catalog = Invoke-RestMethod -Uri "$baseUrl/catalog?limit=5" -Method Get
    Write-Info "`n📋 Total de produtos no catálogo: $($catalog.total)"
    
} catch {
    Write-Error "❌ Erro ao testar catálogo: $($_.Exception.Message)"
}

# Resumo Final
Write-Info "`n" + "="*50
Write-Success "🎉 Testes concluídos!"
Write-Info @"

📊 Próximos passos:
   1. Ver logs: .\docker.ps1 logs
   2. Ver URL pública: .\docker.ps1 ngrok-url
   3. Parar containers: .\docker.ps1 down
   
📚 Documentação:
   - CONFIGURACOES-APLICADAS.md
   - docs/DEPLOY-GUIDE.md
   - docs/AVATAR-PROVIDERS.md
"@
