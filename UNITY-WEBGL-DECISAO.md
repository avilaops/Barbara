# ?? UNITY WEBGL DEPLOY - DECISÃO AGORA!

## ? SUA PERGUNTA

> "Pq não fazemos o unity-webgl já?"

## ? RESPOSTA: PODEMOS FAZER AGORA!

Você **JÁ TEM** tudo necessário:
- ? Azure Static Web App criado (`barbara-web-app`)
- ? Domínio configurado (`barbara.avila.inc`)
- ? Secret `AZURE_STATIC_WEB_APPS_API_TOKEN` configurado
- ? Workflow `deploy-complete.yml` pronto

**ÚNICO bloqueio:** Unity License (para build automático)

---

## ?? 2 CAMINHOS

### **CAMINHO 1: Deploy Manual (5 min)** ? Rápido

**Se você tem build Unity WebGL pronto:**

```powershell
# 1. Fazer build no Unity Editor
# File ? Build Settings ? WebGL ? Build

# 2. Deploy via Azure CLI
az staticwebapp deploy `
  --name barbara-web-app `
  --source core/Build/WebGL `
  --token $env:AZURE_STATIC_WEB_APPS_API_TOKEN

# 3. Acessar
# https://barbara.avila.inc
```

**Vantagens:**
- ? Funciona AGORA (5 min)
- ? Não precisa Unity License
- ? Deploy imediato

**Desvantagens:**
- ? Manual (precisa fazer build toda vez)
- ? Sem CI/CD

**Guia:** `UNITY-DEPLOY-SEM-LICENSE.md`

---

### **CAMINHO 2: Deploy Automático (15 min uma vez)** ?? Melhor

**Configurar Unity License uma vez:**

```powershell
# 1. Gerar .alf no Unity Hub
# Manage ? Licenses ? Manual Activation

# 2. Ativar em https://license.unity3d.com/manual
# Upload .alf ? Download .ulf

# 3. Adicionar secrets no GitHub
# UNITY_LICENSE (conteúdo do .ulf)
# UNITY_EMAIL
# UNITY_PASSWORD

# 4. Push e pronto!
git push
# Deploy automático de tudo!
```

**Vantagens:**
- ? Deploy automático sempre
- ? CI/CD profissional
- ? Build + Deploy em um push

**Desvantagens:**
- ?? 15 min para configurar (uma vez)
- ?? Precisa gerar Unity License

**Guia:** `UNITY-LICENSE-GUIA-RAPIDO.md`

---

## ?? RECOMENDAÇÃO

### **Se quer VER funcionando AGORA:**
? **CAMINHO 1** (Deploy manual)

### **Se quer AUTOMATIZAR:**
? **CAMINHO 2** (Configurar Unity License)

---

## ?? COMPARAÇÃO

| | Manual | Automático |
|---|--------|------------|
| **Setup** | 0 min | 15 min (uma vez) |
| **Deploy** | 5 min (toda vez) | 0 min (automático) |
| **Build** | Unity Editor | GitHub Actions |
| **CI/CD** | ? Não | ? Sim |
| **Manutenção** | Alta | Baixa |

---

## ?? AÇÃO IMEDIATA

### **OPÇÃO A: Deploy Manual Agora (5 min)**

```powershell
# 1. Abrir Unity
# 2. File ? Build Settings ? WebGL ? Build
# 3. Executar:
cd C:\Users\nicol\source\repos\avilaops\barbara

# Fazer build primeiro no Unity!
# Depois:
az staticwebapp deploy `
  --name barbara-web-app `
  --source core/Build/WebGL `
  --token (Get-Content azure-appsettings.json | ConvertFrom-Json).AZURE_STATIC_WEB_APPS_API_TOKEN

# Abrir
Start-Process "https://barbara.avila.inc"
```

### **OPÇÃO B: Configurar Unity License (15 min)**

```powershell
# Seguir guia:
cat UNITY-LICENSE-GUIA-RAPIDO.md

# Resumo:
# 1. Unity Hub ? Manual Activation
# 2. Ativar em https://license.unity3d.com/manual
# 3. Adicionar 3 secrets no GitHub
# 4. Push e pronto!
```

---

## ? RESPOSTA FINAL

**Por que não fazemos Unity WebGL já?**

**R:** PODEMOS! Escolha um caminho:

1. **Rápido (5 min):** Build manual + deploy via Azure CLI
2. **Automatizado (15 min setup):** Configurar Unity License ? deploy automático

**Eu recomendo:** Começar com **#1** para ver funcionando, depois fazer **#2** para automatizar!

---

## ?? PRÓXIMA AÇÃO

Qual caminho você prefere?

**A)** Deploy manual agora (5 min)
- Fazer build no Unity
- Deploy via Azure CLI
- Ver funcionando em barbara.avila.inc

**B)** Configurar Unity License (15 min)
- Seguir `UNITY-LICENSE-GUIA-RAPIDO.md`
- Deploy automático depois

---

## ?? GUIAS CRIADOS

- ? `UNITY-LICENSE-GUIA-RAPIDO.md` - Configurar Unity License
- ? `UNITY-DEPLOY-SEM-LICENSE.md` - Deploy manual sem license
- ? `UNITY-WEBGL-DECISAO.md` - Este arquivo

---

**?? Escolha um caminho e vamos fazer acontecer!**

---

**Última atualização:** 2025-01-09 15:00 UTC
