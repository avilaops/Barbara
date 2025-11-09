# ?? CONFIGURAÇÃO DOS FRONTENDS - Bárbara

## ?? URLs dos Frontends

### 1. Unity WebGL (Frontend Principal)
- **Local:** http://localhost:8080 (após `python -m http.server 8080`)
- **Produção:** https://barbara.azurestaticapps.net (após deploy)
- **Código:** `core/`

### 2. Barbara.Web (Admin/Backoffice)
- **Local:** https://localhost:XXXX (porta aleatória)
- **Produção:** Não deployado ainda
- **Código:** `src/Barbara.Web/`

---

## ?? Configuração da API URL

### Unity Frontend (core/Assets/Scripts/APIClient.cs)

**Desenvolvimento:**
```csharp
[SerializeField] private string baseUrl = "http://localhost:5000";
```

**Produção (após deploy da API):**
```csharp
[SerializeField] private string baseUrl = "https://barbara-api.azurewebsites.net";
```

**OU com variável de ambiente (melhor):**
```csharp
private string baseUrl = 
    #if UNITY_EDITOR
        "http://localhost:5000"
    #else
        "https://barbara-api.azurewebsites.net"
    #endif
;
```

---

## ?? Como Rodar os Frontends

### Unity WebGL (Desenvolvimento)

1. **No Unity Editor:**
   ```
   1. Abra Unity Hub
   2. Adicione projeto: core/
   3. Abra cena: Assets/Scenes/MainScene.unity
   4. Configure API URL no GameObject APIClient
   5. Play
   ```

2. **Build WebGL:**
   ```
   1. File ? Build Settings
   2. Platform: WebGL
   3. Build
   4. Servir:
      cd build/WebGL
python -m http.server 8080
   5. Abrir: http://localhost:8080
   ```

### Barbara.Web (Admin)

```powershell
cd src/Barbara.Web
dotnet run

# Acesse a URL exibida no console (ex: https://localhost:7xxx)
```

---

## ?? Deploy dos Frontends

### Unity WebGL ? Azure Static Web Apps

**Via GitHub Actions (já configurado em `.github/workflows/azure-deploy.yml`):**

1. Build do Unity via GitHub Actions
2. Deploy automático para Azure Static Web Apps
3. URL final: https://barbara.azurestaticapps.net

**Secrets necessários:**
- `UNITY_LICENSE`
- `UNITY_EMAIL`
- `UNITY_PASSWORD`
- `AZURE_STATIC_WEB_APPS_API_TOKEN`

### Barbara.Web ? Azure App Service

```powershell
# Publicar
cd src/Barbara.Web
dotnet publish -c Release -o ./publish

# Deploy via Visual Studio
# Ou via Azure CLI
az webapp deploy --resource-group barbara-rg --name barbara-web --src-path ./publish.zip
```

---

## ?? CORS Configurado

A API já permite **qualquer origem**:

```csharp
// Program.cs
policy.AllowAnyOrigin()
    .AllowAnyMethod()
      .AllowAnyHeader()
```

Para produção, **restringir origens**:

```csharp
policy.WithOrigins(
    "https://barbara.azurestaticapps.net",  // Unity WebGL
    "https://barbara.avila.inc",             // Domínio customizado
    "https://barbara-web.azurewebsites.net"  // Admin
)
```

---

## ?? Arquitetura Completa

```
???????????????????????????????????????????????????
?  Unity WebGL Frontend (Cliente)   ?
?  https://barbara.azurestaticapps.net           ?
?  - Catálogo 3D             ?
?  - Provador Virtual      ?
???????????????????????????????????????????????????
   ?
        ? HTTPS
             ?
???????????????????????????????????????????????????
?  ASP.NET Core 9 API       ?
?  https://barbara-api.azurewebsites.net     ?
?  - Controllers (Categorias, Produtos, etc)      ?
?  - Autenticação         ?
???????????????????????????????????????????????????
     ?
  ? MongoDB Driver
    ?
???????????????????????????????????????????????????
?  MongoDB Atlas (Free M0)             ?
?  cluster0.npuhras.mongodb.net    ?
?  - Database: barbara   ?
???????????????????????????????????????????????????

???????????????????????????????????????????????????
?  Razor Pages Admin (Opcional)        ?
?  https://barbara-web.azurewebsites.net   ?
?  - Gerenciamento de produtos  ?
?  - Pedidos             ?
???????????????????????????????????????????????????
```

---

## ?? URLs Finais Planejadas

| Componente | URL | Status |
|------------|-----|--------|
| **API** | https://barbara.avila.inc/api | ?? Planejado |
| **Unity WebGL** | https://barbara.avila.inc | ?? Planejado |
| **Admin** | https://admin.barbara.avila.inc | ?? Planejado |

**OU separado:**

| Componente | URL | Status |
|------------|-----|--------|
| **API** | https://barbara-api.azurewebsites.net | ? Configurado |
| **Unity WebGL** | https://barbara.azurestaticapps.net | ? Aguardando |
| **Admin** | https://barbara-web.azurewebsites.net | ? Aguardando |

---

## ? Checklist de Configuração

- [x] API rodando localmente
- [x] API no Azure (aguardando deploy)
- [x] MongoDB Atlas configurado
- [x] CORS configurado na API
- [ ] Unity WebGL configurado com URL da API
- [ ] Unity WebGL build
- [ ] Unity WebGL deploy Azure Static Web Apps
- [ ] Barbara.Web deploy (opcional)
- [ ] Domínio customizado barbara.avila.inc
- [ ] SSL/HTTPS configurado

---

## ?? Próximos Passos

1. **Fazer deploy da API** (seguir `DEPLOY-3-PASSOS.md`)
2. **Atualizar URL da API no Unity** (`core/Assets/Scripts/APIClient.cs`)
3. **Build Unity WebGL**
4. **Deploy Unity para Azure Static Web Apps**
5. **Configurar domínio customizado** (opcional)

---

## ?? Arquivos Relacionados

- `core/Assets/Scripts/APIClient.cs` - Cliente HTTP do Unity
- `src/Barbara.Web/Program.cs` - Frontend Admin
- `src/Barbara.API/Program.cs` - Backend API
- `.github/workflows/azure-deploy.yml` - CI/CD GitHub Actions

---

**Atualizado:** 2025-01-09
