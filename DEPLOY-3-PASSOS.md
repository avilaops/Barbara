# ?? DEPLOY EM 3 PASSOS - SUPER SIMPLES

## ? PREPARAÇÃO (JÁ FEITO)

- ? Código migrado para MongoDB
- ? Build OK
- ? GitHub Actions configurado
- ? Publish Profile salvo em: `publish-profile.xml`

---

## ?? PASSO 1: ADICIONAR SECRET NO GITHUB (2 minutos)

### 1.1 Copiar Publish Profile

```powershell
Get-Content publish-profile.xml -Raw | Set-Clipboard
Write-Host "? Copiado!" -ForegroundColor Green
```

### 1.2 Ir para GitHub Secrets

Abra: https://github.com/avilaops/barbara/settings/secrets/actions/new

### 1.3 Adicionar Secret

- **Name:** `AZURE_WEBAPP_PUBLISH_PROFILE`
- **Secret:** Ctrl+V (colar o que copiou)
- Clique em **Add secret**

? **PRONTO!** Secret configurado!

---

## ?? PASSO 2: CONFIGURAR MONGODB NO AZURE (1 minuto)

### 2.1 Copiar MongoDB URI

```powershell
"mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@cluster0.npuhras.mongodb.net/barbara?retryWrites=true&w=majority" | Set-Clipboard
Write-Host "? Copiado!" -ForegroundColor Green
```

### 2.2 Ir para Azure Portal

Abra: https://portal.azure.com/#view/Microsoft_Azure_WApp/WebAppsMenu/~/configuration/resourceId/%2Fsubscriptions%2F3b49f371-dd88-46c7-ba30-aeb54bd5c2f6%2FresourceGroups%2Fbarbara-rg%2Fproviders%2FMicrosoft.Web%2Fsites%2Fbarbara-api

### 2.3 Adicionar Setting

1. Clique em **+ New application setting**
2. **Name:** `MONGODB_URI`
3. **Value:** Ctrl+V (colar o que copiou)
4. Clique em **OK**
5. Clique em **Save** (topo da página)
6. Clique em **Continue** no aviso

? **PRONTO!** MongoDB configurado!

---

## ?? PASSO 3: FAZER DEPLOY (2 minutos)

### 3.1 Commit e Push

```powershell
git add -A -- ':!.vs' ':!**/bin/**' ':!**/obj/**' ':!publish-profile.xml' ':!azure-appsettings.json'
git commit -m "feat: Deploy MongoDB com GitHub Actions"
git push
```

### 3.2 Acompanhar Deploy

Abra: https://github.com/avilaops/barbara/actions

Aguarde 3-5 minutos até ver ? verde

### 3.3 Testar API

Abra: https://barbara-api.azurewebsites.net/swagger

? **PRONTO!** Deploy concluído!

---

## ?? TESTE RÁPIDO

```powershell
# Health Check
Invoke-RestMethod -Uri "https://barbara-api.azurewebsites.net/health"

# Criar categoria
$body = @{
  nome = "Vestidos"
  descricao = "Vestidos femininos"
  ativa = $true
} | ConvertTo-Json

Invoke-RestMethod `
  -Uri "https://barbara-api.azurewebsites.net/api/categorias" `
  -Method POST `
  -ContentType "application/json" `
  -Body $body

# Listar categorias
Invoke-RestMethod -Uri "https://barbara-api.azurewebsites.net/api/categorias"
```

---

## ?? RESULTADO FINAL

? **API:** https://barbara-api.azurewebsites.net  
? **Swagger:** https://barbara-api.azurewebsites.net/swagger  
? **Database:** MongoDB Atlas (Free)  
? **Custo:** USD 0/mês  
? **Deploy:** Automático a cada push!

---

## ?? RESUMO DOS 3 PASSOS

1. **GitHub Secret** ? Cole publish profile
2. **Azure Setting** ? Cole MongoDB URI  
3. **Git Push** ? Deploy automático!

**Tempo total:** 5 minutos

---

## ?? PROBLEMAS?

### API não inicia
- Verifique se o MongoDB URI está correto no Azure
- App Services ? barbara-api ? Configuration ? Application settings

### GitHub Actions falha
- Verifique se o secret `AZURE_WEBAPP_PUBLISH_PROFILE` está correto
- GitHub ? Settings ? Secrets ? Actions

### Timeout MongoDB
- MongoDB Atlas ? Network Access ? Add IP: 0.0.0.0/0

---

## ?? LINKS RÁPIDOS

- **Publish Profile (já gerado):** `publish-profile.xml`
- **GitHub Secrets:** https://github.com/avilaops/barbara/settings/secrets/actions/new
- **Azure Config:** https://portal.azure.com (busque "barbara-api")
- **GitHub Actions:** https://github.com/avilaops/barbara/actions
- **Swagger (após deploy):** https://barbara-api.azurewebsites.net/swagger

---

**?? TUDO PRONTO! Siga os 3 passos acima e em 5 minutos está no ar!**
