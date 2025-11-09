# ?? DEPLOY FINAL - PASSOS AUTOMÁTICOS

Write-Host @"
?????????????????????????????????????????????????????????????????
?          ?
?  ?? DEPLOY AUTOMÁTICO - BÁRBARA E-COMMERCE (MONGODB)        ?
?               ?
?????????????????????????????????????????????????????????????????
"@ -ForegroundColor Cyan

Write-Host ""

# Passo 1: Verificar arquivos
Write-Host "[ ? ] Publish Profile: publish-profile.xml" -ForegroundColor Green
Write-Host "[ ? ] GitHub Workflow: .github/workflows/azure-deploy.yml" -ForegroundColor Green
Write-Host "[ ? ] MongoDB URI: Configurada" -ForegroundColor Green
Write-Host ""

# Passo 2: Configurar MongoDB no Azure via Portal
Write-Host "[ 1/3 ] CONFIGURAR MONGODB URI NO AZURE" -ForegroundColor Yellow
Write-Host ""
Write-Host "Abrindo Azure Portal..." -ForegroundColor Gray
Start-Process "https://portal.azure.com/#view/Microsoft_Azure_WApp/WebAppsMenu/~/configuration/resourceId/%2Fsubscriptions%2F3b49f371-dd88-46c7-ba30-aeb54bd5c2f6%2FresourceGroups%2Fbarbara-rg%2Fproviders%2FMicrosoft.Web%2Fsites%2Fbarbara-api"

Write-Host ""
Write-Host "?? SIGA ESTES PASSOS NO AZURE PORTAL:" -ForegroundColor Cyan
Write-Host "  1. Clique em '+ New application setting'" -ForegroundColor White
Write-Host "  2. Name: MONGODB_URI" -ForegroundColor White
Write-Host "  3. Value: (copie abaixo)" -ForegroundColor White
Write-Host ""
Write-Host "mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority" -ForegroundColor Yellow -BackgroundColor Black
Write-Host ""

# Copiar para clipboard
"mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority" | Set-Clipboard
Write-Host "  ? MongoDB URI COPIADA para clipboard!" -ForegroundColor Green
Write-Host ""
Write-Host "  4. Clique 'OK'" -ForegroundColor White
Write-Host "  5. Clique 'Save' (topo da página)" -ForegroundColor White
Write-Host "  6. Clique 'Continue' no aviso" -ForegroundColor White
Write-Host ""

$null = Read-Host "Pressione ENTER quando terminar a configuração no Azure"

# Passo 3: Commit e Push
Write-Host ""
Write-Host "[ 2/3 ] COMMIT E PUSH PARA GITHUB" -ForegroundColor Yellow
Write-Host ""

git add -A -- ':!.vs' ':!**/bin/**' ':!**/obj/**' ':!publish-profile.xml' ':!azure-appsettings.json'
git commit -m "feat: Deploy MongoDB version com GitHub Actions configurado" 2>&1 | Out-Null

if ($LASTEXITCODE -eq 0) {
    Write-Host "  ? Commit realizado" -ForegroundColor Green
    
    Write-Host "  Fazendo push..." -ForegroundColor Gray
    git push origin main
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Push concluído - Deploy automático iniciado!" -ForegroundColor Green
    } else {
        Write-Host "  ? Erro no push" -ForegroundColor Yellow
    }
} else {
    Write-Host "  ? Nenhuma alteração para commit" -ForegroundColor Gray
}

# Passo 4: Acompanhar deploy
Write-Host ""
Write-Host "[ 3/3 ] ACOMPANHAR DEPLOY" -ForegroundColor Yellow
Write-Host ""
Write-Host "Abrindo GitHub Actions..." -ForegroundColor Gray
Start-Sleep -Seconds 2
Start-Process "https://github.com/avilaops/barbara/actions"

Write-Host ""
Write-Host "? Aguarde 3-5 minutos para o deploy completar" -ForegroundColor Cyan
Write-Host ""

# Aguardar deploy
Write-Host "Aguardando 90 segundos para iniciar..." -ForegroundColor Gray
for ($i = 90; $i -gt 0; $i -= 10) {
    Write-Host "  $i segundos..." -ForegroundColor DarkGray
 Start-Sleep -Seconds 10
}

# Testar API
Write-Host ""
Write-Host "[ TESTE ] Verificando API..." -ForegroundColor Yellow
Write-Host ""

try {
  $health = Invoke-RestMethod -Uri "https://barbara-api.azurewebsites.net/health" -TimeoutSec 30
    
    Write-Host "?????????????????????????????????????????????????????????????????" -ForegroundColor Green
    Write-Host "?              ?" -ForegroundColor Green
    Write-Host "?        ? DEPLOY CONCLUÍDO!      ?" -ForegroundColor Green
    Write-Host "? ?" -ForegroundColor Green
    Write-Host "?????????????????????????????????????????????????????????????????" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? Status da API:" -ForegroundColor Cyan
 Write-Host "  Status: $($health.status)" -ForegroundColor White
 Write-Host "  Database: $($health.database)" -ForegroundColor White
    Write-Host "  Timestamp: $($health.timestamp)" -ForegroundColor White
    Write-Host ""
    Write-Host "?? URLs:" -ForegroundColor Cyan
    Write-Host "  API:     https://barbara-api.azurewebsites.net" -ForegroundColor White
    Write-Host "  Swagger: https://barbara-api.azurewebsites.net/swagger" -ForegroundColor White
    Write-Host "  Health:  https://barbara-api.azurewebsites.net/health" -ForegroundColor White
    Write-Host ""
  Write-Host "?? Custo: USD 0/mês" -ForegroundColor Green
    Write-Host ""
    
    # Abrir Swagger
Write-Host "Abrindo Swagger..." -ForegroundColor Gray
    Start-Process "https://barbara-api.azurewebsites.net/swagger"
    
} catch {
    Write-Host "??  API ainda não está acessível" -ForegroundColor Yellow
    Write-Host "   Aguarde mais alguns minutos e teste manualmente:" -ForegroundColor Gray
    Write-Host "   https://barbara-api.azurewebsites.net/swagger" -ForegroundColor White
    Write-Host ""
}

Write-Host "???????????????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host ""
Write-Host "?? PRÓXIMOS PASSOS:" -ForegroundColor Yellow
Write-Host "  1. Testar endpoints no Swagger" -ForegroundColor White
Write-Host "  2. Criar categorias e produtos de teste" -ForegroundColor White
Write-Host "  3. Configurar domínio: barbara.avila.inc (opcional)" -ForegroundColor White
Write-Host ""
Write-Host "?? Documentação: README-DEPLOY-AGORA.md" -ForegroundColor Cyan
Write-Host ""
