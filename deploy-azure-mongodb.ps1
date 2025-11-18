# ?? DEPLOY AZURE - MongoDB Version
# Script otimizado com escape correto de caracteres especiais

param(
    [string]$ResourceGroup = "barbara-rg",
[string]$WebAppName = "barbara-api"
)

$ErrorActionPreference = "Stop"

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "  DEPLOY AZURE - Barbara E-commerce" -ForegroundColor Cyan
Write-Host "  MongoDB Atlas Version" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# MongoDB URI (escapando & corretamente)
$mongoUri = 'mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority'

Write-Host "[ 1/5 ] Configurando variáveis de ambiente..." -ForegroundColor Yellow

# Método 1: Usar arquivo JSON temporário (mais seguro)
$settingsJson = @"
[
  {
    "name": "MONGODB_URI",
    "value": "$mongoUri",
    "slotSetting": false
  },
  {
    "name": "ASPNETCORE_ENVIRONMENT",
    "value": "Production",
    "slotSetting": false
  }
]
"@

$tempFile = [System.IO.Path]::GetTempFileName() + ".json"
$settingsJson | Out-File -FilePath $tempFile -Encoding UTF8

try {
    az webapp config appsettings set `
  --resource-group $ResourceGroup `
--name $WebAppName `
        --settings "@$tempFile" | Out-Null
    
    Write-Host "  ? Variáveis configuradas" -ForegroundColor Green
}
catch {
    Write-Host "  ? Erro ao configurar variáveis" -ForegroundColor Red
    Write-Host $_.Exception.Message
 exit 1
}
finally {
    Remove-Item $tempFile -ErrorAction SilentlyContinue
}

Write-Host ""
Write-Host "[ 2/5 ] Compilando aplicação..." -ForegroundColor Yellow

Push-Location "src\Barbara.API"

try {
    # Limpar build anterior
    Remove-Item -Path ./publish -Recurse -Force -ErrorAction SilentlyContinue
  Remove-Item -Path ./barbara-api.zip -Force -ErrorAction SilentlyContinue
    
    # Publicar
    dotnet publish -c Release -o ./publish --nologo -v q
    
    if ($LASTEXITCODE -ne 0) {
        throw "Falha na compilação"
    }
    
    Write-Host "  ? Compilação concluída" -ForegroundColor Green
}
catch {
    Write-Host "  ? Erro na compilação" -ForegroundColor Red
    Pop-Location
    exit 1
}

Write-Host ""
Write-Host "[ 3/5 ] Criando pacote ZIP..." -ForegroundColor Yellow

try {
    Compress-Archive -Path ./publish/* -DestinationPath ./barbara-api.zip -CompressionLevel Optimal -Force
    
    $zipInfo = Get-Item ./barbara-api.zip
    $sizeMB = [math]::Round($zipInfo.Length / 1MB, 2)
    
 Write-Host "  ? ZIP criado: $sizeMB MB" -ForegroundColor Green
}
catch {
    Write-Host "  ? Erro ao criar ZIP" -ForegroundColor Red
    Pop-Location
    exit 1
}

Write-Host ""
Write-Host "[ 4/5 ] Fazendo upload para Azure..." -ForegroundColor Yellow
Write-Host "  (Isso pode demorar 2-3 minutos)" -ForegroundColor Gray

try {
    # Usar método alternativo de deploy (kudu API)
    $publishProfile = az webapp deployment list-publishing-credentials `
  --resource-group $ResourceGroup `
        --name $WebAppName `
        --query "{username:publishingUserName, password:publishingPassword}" | ConvertFrom-Json
    
  $base64Auth = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("$($publishProfile.username):$($publishProfile.password)"))
    
    $uri = "https://$WebAppName.scm.azurewebsites.net/api/zipdeploy"
    $headers = @{
     Authorization = "Basic $base64Auth"
    }
    
    $response = Invoke-RestMethod -Uri $uri `
     -Method POST `
-Headers $headers `
        -InFile "./barbara-api.zip" `
        -ContentType "application/zip" `
     -TimeoutSec 600
    
    Write-Host "  ? Upload concluído" -ForegroundColor Green
}
catch {
    Write-Host "  ? Tentando método alternativo..." -ForegroundColor Yellow
    
    # Fallback: usar az webapp deployment
    az webapp deployment source config-zip `
     --resource-group $ResourceGroup `
        --name $WebAppName `
        --src ./barbara-api.zip `
        --timeout 600 2>&1 | Out-Null
    
  if ($LASTEXITCODE -eq 0) {
        Write-Host "  ? Upload concluído (método alternativo)" -ForegroundColor Green
}
    else {
        Write-Host "  ? Erro no upload" -ForegroundColor Red
        Write-Host "  Use Visual Studio: Botão direito em Barbara.API ? Publish" -ForegroundColor Yellow
        Pop-Location
        exit 1
    }
}

Pop-Location

Write-Host ""
Write-Host "[ 5/5 ] Reiniciando aplicação..." -ForegroundColor Yellow

az webapp restart --resource-group $ResourceGroup --name $WebAppName | Out-Null
Write-Host "  ? Aplicação reiniciada" -ForegroundColor Green

Write-Host ""
Write-Host "[ Validando ] Aguardando inicialização..." -ForegroundColor Yellow
Start-Sleep -Seconds 15

$apiUrl = "https://$WebAppName.azurewebsites.net"

try {
  $health = Invoke-RestMethod -Uri "$apiUrl/health" -TimeoutSec 30
    Write-Host "  ? API está respondendo!" -ForegroundColor Green
    Write-Host "  Status: $($health.status)" -ForegroundColor Gray
    Write-Host "  Database: $($health.database)" -ForegroundColor Gray
}
catch {
    Write-Host "  ? API ainda não está acessível" -ForegroundColor Yellow
    Write-Host "  Aguarde 1-2 minutos e teste manualmente" -ForegroundColor Gray
}

Write-Host ""
Write-Host "============================================" -ForegroundColor Green
Write-Host "  DEPLOY CONCLUÍDO! ??" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Green
Write-Host ""

Write-Host "?? URLs:" -ForegroundColor Cyan
Write-Host "  API:     $apiUrl" -ForegroundColor White
Write-Host "  Swagger: $apiUrl/swagger" -ForegroundColor White
Write-Host "  Health:  $apiUrl/health" -ForegroundColor White
Write-Host ""

Write-Host "?? Teste rápido:" -ForegroundColor Yellow
Write-Host "  Invoke-RestMethod -Uri '$apiUrl/health'" -ForegroundColor Gray
Write-Host "  Start-Process '$apiUrl/swagger'" -ForegroundColor Gray
Write-Host ""

Write-Host "?? Custo: USD 0/mês (MongoDB Atlas Free + App Service Free)" -ForegroundColor Cyan
Write-Host ""

Write-Host "?? Próximos passos:" -ForegroundColor Yellow
Write-Host "  1. Testar endpoints no Swagger" -ForegroundColor White
Write-Host "  2. Configurar domínio customizado: barbara.avila.inc" -ForegroundColor White
Write-Host "  3. Criar categorias e produtos de teste" -ForegroundColor White
Write-Host ""
