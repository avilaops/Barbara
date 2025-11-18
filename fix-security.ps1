# ?? CORREÇÃO DE SEGURANÇA - PRÉ-DEPLOY

Write-Host "??????????????????????????????????????????????" -ForegroundColor Red
Write-Host "?  ?? CORREÇÕES DE SEGURANÇA (CRÍTICO)    ?" -ForegroundColor Red
Write-Host "??????????????????????????????????????????????" -ForegroundColor Red
Write-Host ""

$erros = 0

# 1. Remover credenciais do Git
Write-Host "[ 1/4 ] Removendo credenciais expostas..." -ForegroundColor Yellow

$arquivos = @(
    "azure-appsettings.json",
    "publish-profile.xml",
 ".env"
)

foreach ($arquivo in $arquivos) {
  if (Test-Path $arquivo) {
   Write-Host "  Removendo do Git: $arquivo" -ForegroundColor Gray
        git rm --cached $arquivo 2>$null
    }
}

# Atualizar .gitignore
$gitignoreItems = @(
    "azure-appsettings.json",
    "publish-profile.xml",
    "*.publishsettings",
    ".env",
    "*.env",
    "publish-profile*.xml"
)

foreach ($item in $gitignoreItems) {
    if (-not (Get-Content .gitignore -ErrorAction SilentlyContinue | Select-String -Pattern "^$item$")) {
    Add-Content .gitignore $item
   Write-Host "  Adicionado ao .gitignore: $item" -ForegroundColor Green
    }
}

Write-Host "  ? Credenciais protegidas" -ForegroundColor Green
Write-Host ""

# 2. Limpar MongoDB URI do appsettings.json
Write-Host "[ 2/4 ] Removendo MongoDB URI do appsettings.json..." -ForegroundColor Yellow

$appsettingsPath = "src\Barbara.API\appsettings.json"
if (Test-Path $appsettingsPath) {
    $appsettings = Get-Content $appsettingsPath | ConvertFrom-Json
    
    if ($appsettings.ConnectionStrings.MongoDB -ne "") {
  $appsettings.ConnectionStrings.MongoDB = ""
        $appsettings | ConvertTo-Json -Depth 10 | Set-Content $appsettingsPath
        Write-Host "  ? MongoDB URI removido do appsettings.json" -ForegroundColor Green
    } else {
        Write-Host "  ? MongoDB URI já estava vazio" -ForegroundColor Gray
    }
} else {
    Write-Host "  ? Arquivo não encontrado: $appsettingsPath" -ForegroundColor Yellow
    $erros++
}

Write-Host ""

# 3. Criar appsettings.Production.json
Write-Host "[ 3/4 ] Criando appsettings.Production.json..." -ForegroundColor Yellow

$productionConfig = @"
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error",
      "Barbara": "Information"
    }
  },
  "AllowedHosts": "barbara.avila.inc;barbara-api.azurewebsites.net;*.azurestaticapps.net",
  "ConnectionStrings": {
    "MongoDB": ""
  }
}
"@

$productionPath = "src\Barbara.API\appsettings.Production.json"
if (-not (Test-Path $productionPath)) {
    $productionConfig | Out-File -FilePath $productionPath -Encoding UTF8
    Write-Host "  ? appsettings.Production.json criado" -ForegroundColor Green
} else {
    Write-Host "  ? appsettings.Production.json já existe" -ForegroundColor Gray
}

Write-Host ""

# 4. Atualizar CORS no Program.cs
Write-Host "[ 4/4 ] Verificando CORS no Program.cs..." -ForegroundColor Yellow

$programPath = "src\Barbara.API\Program.cs"
if (Test-Path $programPath) {
 $programContent = Get-Content $programPath -Raw
    
    if ($programContent -match "AllowAnyOrigin\(\)") {
        Write-Host "  ? CORS permite qualquer origem (INSEGURO)" -ForegroundColor Red
        Write-Host ""
        Write-Host "  ?? AÇÃO MANUAL NECESSÁRIA:" -ForegroundColor Yellow
    Write-Host "  Edite $programPath" -ForegroundColor White
        Write-Host "  Substitua:" -ForegroundColor Gray
        Write-Host "    policy.AllowAnyOrigin()" -ForegroundColor Red
        Write-Host "  Por:" -ForegroundColor Gray
        Write-Host "    policy.WithOrigins(" -ForegroundColor Green
        Write-Host '        "https://barbara.azurestaticapps.net",' -ForegroundColor Green
    Write-Host '        "https://barbara.avila.inc"' -ForegroundColor Green
        Write-Host "    )" -ForegroundColor Green
        Write-Host ""
  $erros++
    } else {
     Write-Host "  ? CORS configurado com origens específicas" -ForegroundColor Green
    }
} else {
    Write-Host "  ? Arquivo não encontrado: $programPath" -ForegroundColor Yellow
    $erros++
}

Write-Host ""
Write-Host "????????????????????????????????????????????" -ForegroundColor Cyan

if ($erros -eq 0) {
  Write-Host "? TODAS AS CORREÇÕES AUTOMÁTICAS CONCLUÍDAS!" -ForegroundColor Green
    Write-Host ""
    Write-Host "?? Próximos passos:" -ForegroundColor Cyan
    Write-Host "  1. Revisar alterações: git status" -ForegroundColor White
    Write-Host "  2. Commit: git commit -m 'security: Correções de segurança'" -ForegroundColor White
    Write-Host "  3. Push: git push" -ForegroundColor White
    Write-Host "  4. Deploy!" -ForegroundColor White
} else {
    Write-Host "??  $erros ações manuais necessárias (ver acima)" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Complete as ações manuais e execute novamente." -ForegroundColor Gray
}

Write-Host ""
Write-Host "?? Documentação completa: AUDITORIA-PRODUCAO.md" -ForegroundColor Cyan
Write-Host ""
