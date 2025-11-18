# ?? DEPLOY FINAL - Barbara MongoDB

## ? TUDO PRONTO

- ? Código migrado para MongoDB
- ? Build compilando perfeitamente
- ? Código commitado no GitHub
- ? Secrets configuradas no GitHub
- ? Publish criado (barbara-api.zip)

---

## ?? DEPLOY - ESCOLHA UMA OPÇÃO

### ?? OPÇÃO 1: GitHub Actions (RECOMENDADO - Automático)

**Vantagens:**
- ? Deploy automático a cada push
- ? Secrets do MongoDB já configuradas
- ? CI/CD profissional
- ? Sem configuração manual

**Passo a passo:**

1. **Criar workflow do GitHub Actions**

Crie o arquivo: `.github/workflows/azure-deploy.yml`

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
  workflow_dispatch:

env:
  AZURE_WEBAPP_NAME: barbara-api
  DOTNET_VERSION: '9.0.x'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
 uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Publish
      run: dotnet publish src/Barbara.API/Barbara.API.csproj -c Release -o ${{env.DOTNET_ROOT}}/publish
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
    with:
 app-name: ${{ env.AZURE_WEBAPP_NAME }}
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/publish
```

2. **Configurar Publish Profile no GitHub**

```powershell
# Obter o publish profile
az webapp deployment list-publishing-profiles `
  --resource-group barbara-rg `
  --name barbara-api `
--xml > publish-profile.xml

# Copie o conteúdo do arquivo publish-profile.xml
Get-Content publish-profile.xml | Set-Clipboard
```

3. **Adicionar Secret no GitHub**

- Vá em: https://github.com/avilaops/barbara/settings/secrets/actions
- Clique em **New repository secret**
- Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
- Value: **Cole o conteúdo copiado**
- **Add secret**

4. **Configurar MongoDB URI no Azure**

```powershell
# Usar a URI completa das secrets do GitHub
az webapp config appsettings set `
  --resource-group barbara-rg `
  --name barbara-api `
  --settings @settings.json

# Onde settings.json contém:
# {
#   "name": "MONGODB_URI",
#   "value": "mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority"
# }
```

**OU via Portal Azure:**
- App Service ? Configuration ? Application settings
- + New application setting
- Name: `MONGODB_URI`
- Value: (cole a URI completa)
- Save

5. **Fazer push e assistir o deploy**

```powershell
git add .github/workflows/azure-deploy.yml
git commit -m "feat: Adicionar GitHub Actions deploy workflow"
git push

# Acompanhe em:
# https://github.com/avilaops/barbara/actions
```

---

### ?? OPÇÃO 2: Visual Studio (Manual - Mais Simples)

1. **Abra Visual Studio 2022**
2. **Abra o arquivo:** `Barbara.sln`
3. **Botão direito** no projeto `Barbara.API`
4. **Publish...**
5. **Target:** Azure
6. **Specific target:** Azure App Service (Linux)
7. **Sign in:** contato@mrgcaixastermicas.com.br
8. **Subscription:** Padrão
9. **Selecione:** barbara-api
10. **Finish** ? **Publish**
11. Aguarde ~3-5 minutos

**Depois do publish:**

Configurar MongoDB URI no Azure Portal:
1. https://portal.azure.com
2. App Services ? barbara-api
3. Configuration ? Application settings
4. + New application setting
5. Name: `MONGODB_URI`
6. Value: (cole a URI completa das secrets)
7. Save
8. Restart

---

### ? OPÇÃO 3: Azure Portal (Upload Manual)

1. **Acesse:** https://portal.azure.com
2. **App Services** ? barbara-api
3. **Advanced Tools** ? Go
4. **Debug console** ? CMD
5. **Arrastar e soltar** o arquivo `barbara-api.zip` na pasta `site/wwwroot`
6. Aguarde upload
7. Restart app service

---

## ?? CONFIGURAR MONGODB URI (Todas as opções)

### Via PowerShell (escapando caracteres especiais):

```powershell
# Criar arquivo JSON temporário
$settings = @'
[
  {
    "name": "MONGODB_URI",
    "value": "mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority",
    "slotSetting": false
  }
]
'@

$tempFile = New-TemporaryFile
$settings | Out-File $tempFile.FullName -Encoding UTF8

az webapp config appsettings set `
  --resource-group barbara-rg `
  --name barbara-api `
  --settings "@$($tempFile.FullName)"

Remove-Item $tempFile
```

### Via Portal Azure:

1. https://portal.azure.com
2. App Services ? barbara-api
3. Configuration ? Application settings
4. + New application setting
5. Name: `MONGODB_URI`
6. Value: 
   ```
   mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority
   ```
7. OK ? Save
8. Restart

---

## ?? TESTAR APÓS DEPLOY

### 1. Health Check

```powershell
Invoke-RestMethod -Uri "https://barbara-api.azurewebsites.net/health"
```

**Esperado:**
```json
{
  "status": "healthy",
  "database": "mongodb",
  "timestamp": "2025-01-09T11:00:00Z"
}
```

### 2. Swagger

```powershell
Start-Process "https://barbara-api.azurewebsites.net/swagger"
```

### 3. Criar Categoria de Teste

```powershell
$categoria = @{
  nome = "Vestidos"
  descricao = "Vestidos femininos"
  ativa = $true
} | ConvertTo-Json

Invoke-RestMethod `
  -Uri "https://barbara-api.azurewebsites.net/api/categorias" `
  -Method POST `
  -ContentType "application/json" `
  -Body $categoria
```

### 4. Listar Categorias

```powershell
Invoke-RestMethod -Uri "https://barbara-api.azurewebsites.net/api/categorias"
```

---

## ?? RECURSOS CRIADOS

| Recurso | Nome | Custo |
|---------|------|-------|
| MongoDB Atlas | cluster0.npuhras.mongodb.net | USD 0/mês (Free M0) |
| App Service | barbara-api.azurewebsites.net | USD 0/mês (Free F1) |
| Resource Group | barbara-rg | USD 0/mês |
| **TOTAL** | | **USD 0/mês** |

---

## ?? CONFIGURAR DOMÍNIO CUSTOMIZADO

Após deploy bem-sucedido, siga: `SETUP-DOMINIO-CUSTOMIZADO.md`

Resumo:
1. DNS: CNAME barbara ? barbara-api.azurewebsites.net
2. Azure: Adicionar domínio barbara.avila.inc
3. SSL: Auto-managed certificate

---

## ? CHECKLIST FINAL

- [x] Código migrado para MongoDB
- [x] Build OK
- [x] Commitado no GitHub
- [x] Secrets configuradas
- [ ] **Deploy feito** ? ESCOLHA UMA OPÇÃO ACIMA
- [ ] MongoDB URI configurada no Azure
- [ ] Health check OK
- [ ] Swagger acessível
- [ ] Categorias de teste criadas
- [ ] Domínio customizado (opcional)

---

## ?? RECOMENDAÇÃO

Use **OPÇÃO 1 (GitHub Actions)** para:
- ? Deploy automático
- ? CI/CD profissional
- ? Histórico de deploys
- ? Rollback fácil

**OU**

Use **OPÇÃO 2 (Visual Studio)** se preferir simplicidade agora.

---

## ?? DICA DE SEGURANÇA

Sua MongoDB URI está exposta no `.env` local. Para produção:

1. **Remova do .env** (já está no .gitignore)
2. **Use apenas:**
   - GitHub Secrets (CI/CD)
   - Azure App Settings (Runtime)

---

## ?? ARQUIVOS DE REFERÊNCIA

- `DEPLOY-FINAL-COMPLETO.md` - Guia completo
- `SETUP-DOMINIO-CUSTOMIZADO.md` - Domínio customizado
- `deploy-azure-mongodb.ps1` - Script automatizado
- `.github/workflows/azure-deploy.yml` - CI/CD (criar)

---

**?? TUDO PRONTO!**

Escolha uma das 3 opções acima e faça o deploy em 5 minutos! ??

**Recomendação:** Opção 1 (GitHub Actions) para deploy automático profissional.

---

**Última atualização:** 2025-01-09 11:00 UTC
