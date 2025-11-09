# ?? OBTER UNITY LICENSE - GUIA RÁPIDO (15 min)

## ?? OBJETIVO

Obter os 3 secrets necessários para build Unity WebGL no GitHub Actions:
- `UNITY_LICENSE` (arquivo .ulf)
- `UNITY_EMAIL`
- `UNITY_PASSWORD`

---

## ?? PASSO A PASSO

### **1. Abrir Unity Hub (2 min)**

```powershell
# Abrir Unity Hub
Start-Process "unityhub://"
```

**Ou:** Abrir manualmente Unity Hub

---

### **2. Gerar Arquivo de Ativação Manual (.alf)** (3 min)

**No Unity Hub:**

1. **Manage** ? **Licenses**
2. **Manual Activation**
3. **Save License Request** ? Salvar como `unity-activation.alf`

**Onde salvar:**
```
C:\Users\nicol\Downloads\unity-activation.alf
```

---

### **3. Ativar Licença Online** (5 min)

1. **Ir para:** https://license.unity3d.com/manual
2. **Upload** do arquivo `unity-activation.alf`
3. **Selecionar:**
   - Unity Personal (se for pessoal/estudante)
   - Unity Plus/Pro (se tiver licença paga)
4. **Confirmar** que não é para uso comercial (se for Personal)
5. **Download** do arquivo `.ulf` (license file)

**Onde salvar:**
```
C:\Users\nicol\Downloads\Unity_v2022.x.ulf
```

---

### **4. Copiar Conteúdo do .ulf** (2 min)

```powershell
# Navegar para Downloads
cd $env:USERPROFILE\Downloads

# Listar arquivos Unity
Get-ChildItem *.ulf

# Copiar conteúdo para clipboard
Get-Content Unity_v*.ulf -Raw | Set-Clipboard

Write-Host "? Licença Unity copiada para clipboard!" -ForegroundColor Green
```

---

### **5. Adicionar Secrets no GitHub** (3 min)

**Abrir GitHub Secrets:**
```powershell
Start-Process "https://github.com/avilaops/barbara/settings/secrets/actions"
```

**Adicionar 3 secrets:**

#### **5.1. UNITY_LICENSE**
- **Name:** `UNITY_LICENSE`
- **Secret:** Ctrl+V (colar o conteúdo do .ulf)
- **Add secret**

#### **5.2. UNITY_EMAIL**
- **Name:** `UNITY_EMAIL`
- **Secret:** seu-email@example.com (email da conta Unity)
- **Add secret**

#### **5.3. UNITY_PASSWORD**
- **Name:** `UNITY_PASSWORD`
- **Secret:** sua-senha-unity
- **Add secret**

---

## ? VERIFICAR SECRETS CONFIGURADOS

```powershell
# Abrir página de secrets
Start-Process "https://github.com/avilaops/barbara/settings/secrets/actions"
```

**Deve ter:**
- ? `AZURE_STATIC_WEB_APPS_API_TOKEN`
- ? `AZURE_WEBAPP_PUBLISH_PROFILE`
- ? `UNITY_LICENSE` ? **NOVO**
- ? `UNITY_EMAIL` ? **NOVO**
- ? `UNITY_PASSWORD` ? **NOVO**

---

## ?? TESTAR BUILD UNITY

### **Opção A: Push Automático**

```powershell
# Fazer alteração em qualquer arquivo Unity
echo "# Test Unity build" >> core/README.md

git add core/
git commit -m "test: Trigger Unity WebGL build"
git push

# Acompanhar
Start-Process "https://github.com/avilaops/barbara/actions"
```

### **Opção B: Manual (Recomendado para primeiro teste)**

1. Ir para: https://github.com/avilaops/barbara/actions
2. Clicar em **Deploy Completo - Bárbara E-commerce**
3. **Run workflow** ? Branch: main ? **Run workflow**
4. Aguardar ~10-15 minutos

---

## ?? O QUE VAI ACONTECER

```
JOB 1: Build API (.NET 9)           ? ~3 min ?
JOB 2: Build Unity WebGL        ? ~8 min ??
JOB 3: Deploy Static Web App   ? ~2 min ??
JOB 4: Azure Functions (skip)    ? ~1 min ??
JOB 5: Health Checks ? ~1 min ??
JOB 6: Notificação         ? ~1 min ??

TOTAL: ~15 minutos
```

---

## ?? RESULTADO FINAL

Após deploy completo, você terá:

| Componente | URL | Status |
|------------|-----|--------|
| **Frontend Unity WebGL** | https://barbara.avila.inc | ? Live |
| **API Backend** | https://barbara-api.azurewebsites.net | ? Live |
| **Swagger** | https://barbara-api.azurewebsites.net/swagger | ? Live |

---

## ?? TROUBLESHOOTING

### ? **Erro: Invalid license**

**Solução:**
1. Verificar se copiou o arquivo `.ulf` completo
2. Não copiar com `notepad` (pode adicionar caracteres)
3. Usar PowerShell: `Get-Content arquivo.ulf -Raw`

### ? **Erro: Unity version mismatch**

**Solução:**
1. Verificar versão do Unity no projeto
2. Atualizar `UNITY_VERSION` no workflow se necessário

### ? **Erro: License expired**

**Solução:**
1. Renovar licença em: https://id.unity.com/
2. Gerar novo arquivo .ulf
3. Atualizar secret `UNITY_LICENSE`

---

## ?? DOCUMENTAÇÃO UNITY

- **Manual Activation:** https://docs.unity3d.com/Manual/ManualActivationGuide.html
- **Unity Licensing:** https://docs.unity3d.com/Manual/LicensingOverview.html
- **Game CI:** https://game.ci/docs/github/activation

---

## ? CHECKLIST

- [ ] Abrir Unity Hub
- [ ] Gerar arquivo .alf (Manual Activation)
- [ ] Ativar em https://license.unity3d.com/manual
- [ ] Download arquivo .ulf
- [ ] Copiar conteúdo do .ulf
- [ ] Adicionar secret `UNITY_LICENSE` no GitHub
- [ ] Adicionar secret `UNITY_EMAIL` no GitHub
- [ ] Adicionar secret `UNITY_PASSWORD` no GitHub
- [ ] Testar workflow manual
- [ ] Verificar deploy em barbara.avila.inc

---

## ?? CONCLUSÃO

**Tempo total:** ~15 minutos

**Depois disso:**
- ? Unity WebGL deploy automático
- ? Frontend acessível em barbara.avila.inc
- ? Deploy completo funcionando

**Próximo passo:**
1. Seguir passos acima
2. Configurar secrets
3. Executar workflow manual
4. Ver Unity WebGL funcionando! ??

---

**Última atualização:** 2025-01-09 15:00 UTC
