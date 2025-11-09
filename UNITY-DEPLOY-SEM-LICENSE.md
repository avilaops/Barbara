# ?? DEPLOY UNITY WEBGL - SEM BUILD AUTOMÁTICO

## ?? CENÁRIO

Você **já tem** um build Unity WebGL local e quer fazer deploy **AGORA** sem configurar Unity License.

---

## ?? REQUISITOS

- ? Build Unity WebGL pronto (pasta `Build/`)
- ? Azure Static Web App criado
- ? Secret `AZURE_STATIC_WEB_APPS_API_TOKEN` configurado

---

## ?? OPÇÃO A: Deploy Manual via Azure CLI

### **1. Preparar Build**

```powershell
# Navegar para build Unity
cd core/

# Verificar se build existe
if (Test-Path "Build/WebGL") {
    Write-Host "? Build WebGL encontrado!" -ForegroundColor Green
} else {
    Write-Host "? Build não encontrado. Fazer build no Unity primeiro." -ForegroundColor Red
    exit
}
```

### **2. Deploy para Azure Static Web App**

```powershell
# Instalar Azure Static Web Apps CLI (se não tiver)
npm install -g @azure/static-web-apps-cli

# Deploy
swa deploy `
  --app-location "Build/WebGL" `
  --deployment-token $env:AZURE_STATIC_WEB_APPS_API_TOKEN

# Ou via Azure CLI
az staticwebapp deploy `
  --name barbara-web-app `
  --source Build/WebGL `
  --token $env:AZURE_STATIC_WEB_APPS_API_TOKEN
```

---

## ?? OPÇÃO B: Deploy via GitHub (Sem Build)

### **1. Commit Build Existente**

```powershell
# Adicionar build ao repositório (temporário)
git add core/Build/WebGL/
git commit -m "chore: Adicionar build Unity WebGL para deploy"
git push
```

### **2. Criar Workflow Simplificado**

Criar arquivo `.github/workflows/deploy-unity-manual.yml`:

```yaml
name: ?? Deploy Unity WebGL (Build Manual)

on:
  workflow_dispatch:

jobs:
  deploy:
    name: Deploy Unity WebGL
    runs-on: ubuntu-latest
    
    steps:
    - name: Checkout
  uses: actions/checkout@v4
      
- name: Deploy to Azure Static Web App
      uses: Azure/static-web-apps-deploy@v1
      with:
        azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
        repo_token: ${{ secrets.GITHUB_TOKEN }}
        action: "upload"
     app_location: "core/Build/WebGL"
        skip_app_build: true
```

### **3. Executar Workflow**

1. GitHub ? Actions
2. Deploy Unity WebGL (Build Manual)
3. Run workflow

---

## ?? OPÇÃO C: Build Local + Deploy Automático

### **1. Fazer Build no Unity**

**Unity Editor:**
1. File ? Build Settings
2. Platform: **WebGL**
3. Switch Platform
4. **Build**
5. Pasta de saída: `core/Build/WebGL`

### **2. Deploy**

```powershell
# Copiar build para pasta webgl-build (se preferir)
Copy-Item -Path "core/Build/WebGL/*" -Destination "webgl-build/" -Recurse -Force

# Commit
git add webgl-build/
git commit -m "chore: Unity WebGL build manual"
git push

# Ou usar Azure CLI diretamente
az staticwebapp deploy `
  --name barbara-web-app `
  --source core/Build/WebGL `
  --token $env:AZURE_STATIC_WEB_APPS_API_TOKEN
```

---

## ?? COMPARAÇÃO

| Método | Tempo | Requer Unity License | Deploy Automático |
|--------|-------|---------------------|-------------------|
| **Opção A (Azure CLI)** | 2 min | ? Não | ? Não |
| **Opção B (GitHub Manual)** | 5 min | ? Não | ?? Semi |
| **Opção C (Build + Deploy)** | 10 min | ? Não | ? Não |
| **Workflow Completo** | 15 min | ? Sim | ? Sim |

---

## ? RECOMENDAÇÃO

### **Se você tem build pronto:**
? Use **Opção A (Azure CLI)** - Mais rápido!

### **Se quer automatizar:**
? Configure Unity License (15 min uma vez)
? Depois deploy automático sempre!

---

## ?? PRÓXIMOS PASSOS

### **Deploy Rápido (Agora):**
```powershell
# Fazer build no Unity primeiro
# Depois:
az staticwebapp deploy `
  --name barbara-web-app `
  --source core/Build/WebGL `
  --token $env:AZURE_STATIC_WEB_APPS_API_TOKEN
```

### **Deploy Automático (Futuro):**
1. Seguir `UNITY-LICENSE-GUIA-RAPIDO.md`
2. Configurar secrets Unity
3. Push automático funciona!

---

**Qual opção prefere?**
- ? **Deploy manual agora** ? Usar Opção A
- ? **Deploy automático depois** ? Configurar Unity License

---

**Última atualização:** 2025-01-09 15:00 UTC
