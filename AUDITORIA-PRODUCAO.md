# ?? AUDITORIA COMPLETA - NÃO PRONTO PARA PRODUÇÃO

## ? POR QUE NÃO USAR `#if UNITY_EDITOR` EM PRODUÇÃO?

**Resposta curta:** `#if UNITY_EDITOR` significa "**APENAS no editor Unity**"

```csharp
#if UNITY_EDITOR
    "http://localhost:5000"  // ? NUNCA será usado no build WebGL
#else
    "https://barbara-api.azurewebsites.net"  // ? Usado no build final
#endif
```

**O que acontece:**
- ? **No Unity Editor:** Usa `localhost:5000` (desenvolvimento)
- ? **No Build WebGL:** Usa URL de produção
- ? **Correto!** Não há problema nisso!

---

## ?? PROBLEMAS CRÍTICOS - DEVEM SER CORRIGIDOS

### 1. ?? **CORS Permite Qualquer Origem** (CRÍTICO)

**Arquivo:** `src/Barbara.API/Program.cs`

```csharp
// ? ATUAL (inseguro)
policy.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
```

**? CORREÇÃO:**
```csharp
policy.WithOrigins(
    "https://barbara.azurestaticapps.net",  // Unity WebGL
    "https://barbara.avila.inc",     // Domínio customizado
    "https://admin.barbara.avila.inc"// Admin
)
.AllowAnyMethod()
.AllowAnyHeader()
.AllowCredentials();
```

**Risco:** Qualquer site pode acessar sua API

---

### 2. ?? **Credenciais Expostas nos Arquivos**

**Problemas:**
- ? `.env` **está no .gitignore** (OK!)
- ? `azure-appsettings.json` contém MongoDB URI **sem .gitignore**
- ? `publish-profile.xml` contém credenciais de deploy

**? AÇÃO:**
```powershell
# Adicionar ao .gitignore
echo "azure-appsettings.json" >> .gitignore
echo "publish-profile.xml" >> .gitignore
echo "*.publishsettings" >> .gitignore

# Remover do repositório
git rm --cached azure-appsettings.json
git rm --cached publish-profile.xml
git commit -m "security: Remove credenciais expostas"
```

---

### 3. ??? **Código Node.js Antigo Misturado**

**Pasta `api/` está obsoleta!**

O projeto migrou de Node.js para .NET 9, mas a pasta `api/` antiga ainda está no repositório.

**? AÇÃO:**
```powershell
# Verificar se está em uso
git log --oneline api/

# Se não estiver em uso, remover
git rm -r api/
git commit -m "chore: Remover API Node.js obsoleta"
```

---

### 4. ?? **`appsettings.Development.json` em Produção**

**Arquivo:** `src/Barbara.API/appsettings.Development.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",  // ? Muito verboso
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**? CORREÇÃO:** Criar `appsettings.Production.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error",
 "Barbara": "Information"
}
  },
  "AllowedHosts": "barbara.avila.inc;barbara-api.azurewebsites.net"
}
```

---

### 5. ?? **Sem Autenticação/Autorização**

**Nenhum controller tem `[Authorize]`!**

Qualquer pessoa pode:
- ? Criar/deletar produtos
- ? Ver dados de clientes
- ? Criar pedidos

**? AÇÃO:** Implementar JWT ou Azure AD B2C

```csharp
// Adicionar no Program.cs
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
     options.TokenValidationParameters = new TokenValidationParameters
        {
    ValidateIssuer = true,
            ValidateAudience = true,
      ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
       Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

app.UseAuthentication();
app.UseAuthorization();

// Nos controllers
[Authorize]
public class ProdutosController : ControllerBase
```

---

### 6. ?? **Sem Validação de Entrada**

**Exemplo:** `ProdutosController.cs`

```csharp
[HttpPost]
public async Task<ActionResult<Produto>> CriarProduto(CriarProdutoDto dto)
{
    // ? Sem validação!
    var produto = new Produto { Nome = dto.Nome, ... };
}
```

**? CORREÇÃO:** Adicionar Data Annotations

```csharp
public record CriarProdutoDto
{
    [Required(ErrorMessage = "Nome é obrigatório")]
    [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
    public string Nome { get; init; } = string.Empty;
    
  [Required]
    [Range(0.01, 999999, ErrorMessage = "Preço deve ser maior que 0")]
    public decimal Preco { get; init; }
    
    [Url(ErrorMessage = "URL inválida")]
    public string? UrlModelo3D { get; init; }
}
```

---

## ?? PROBLEMAS MÉDIOS - DEVEM SER MELHORADOS

### 7. ?? **Sem Logging Estruturado**

**Atual:** Usa `Console.WriteLine` (implícito)

**? MELHORAR:** Usar Serilog

```csharp
// Instalar
dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.Console
dotnet add package Serilog.Sinks.File

// Program.cs
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
  .WriteTo.File("logs/barbara-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();
```

---

### 8. ?? **Sem Rate Limiting**

Qualquer IP pode fazer **requests ilimitados**!

**? ADICIONAR:**

```csharp
// Program.cs
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
  factory: partition => new FixedWindowRateLimiterOptions
    {
                PermitLimit = 100,
    Window = TimeSpan.FromMinutes(1)
   }));
});

app.UseRateLimiter();
```

---

### 9. ??? **Índices MongoDB Comentados**

**Arquivo:** `Program.cs` linha 64

```csharp
// NOTA: Criação de índices desabilitada temporariamente
/*
await context.CreateIndexesAsync();
*/
```

**Risco:** Performance ruim em queries

**? AÇÃO:** Habilitar após resolver problema de conexão

---

### 10. ?? **Sem Cache**

Toda request bate no MongoDB!

**? ADICIONAR:** Redis ou Memory Cache

```csharp
// Memory Cache (simples)
builder.Services.AddMemoryCache();

// Redis (produção)
builder.Services.AddStackExchangeRedisCache(options =>
{
  options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = "Barbara:";
});

// No controller
private readonly IMemoryCache _cache;

public async Task<ActionResult<IEnumerable<Produto>>> GetProdutosDestaque()
{
    if (!_cache.TryGetValue("produtos_destaque", out List<Produto> produtos))
    {
     produtos = await _repository.FindAsync(p => p.Destaque);
        _cache.Set("produtos_destaque", produtos, TimeSpan.FromMinutes(5));
    }
    return Ok(produtos);
}
```

---

## ?? PROBLEMAS MENORES - MELHORIAS

### 11. ?? **Sem Testes Automatizados (.NET)**

Existe pasta `api/tests/` (Node.js antigo), mas **nenhum teste .NET**!

**? CRIAR:**

```powershell
cd tests
dotnet new xunit -n Barbara.Tests
dotnet add reference ../src/Barbara.API/Barbara.API.csproj

# Exemplo de teste
[Fact]
public async Task GetCategorias_DeveRetornarListaVazia()
{
  var result = await _controller.GetCategorias();
    var okResult = Assert.IsType<OkObjectResult>(result.Result);
    var categorias = Assert.IsAssignableFrom<IEnumerable<Categoria>>(okResult.Value);
    Assert.Empty(categorias);
}
```

---

### 12. ?? **Swagger Habilitado em Produção**

**Atual:**
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
```

**? Correto!** Mas considere habilitar com autenticação em staging.

---

### 13. ?? **MongoDB URI no `appsettings.json`**

**Arquivo:** `src/Barbara.API/appsettings.json`

```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://nicolasrosaab_db_user:Gio4EAQhbEdQMISl@..."
  }
}
```

**Risco:** Credencial versionada no Git!

**? CORREÇÃO:**
```json
{
  "ConnectionStrings": {
    "MongoDB": ""  // Vazio no repositório
  }
}
```

E configurar via **Azure App Settings** (já feito!)

---

## ?? UNITY WEBGL - CHECKLIST

### 14. ?? **Build de Produção**

**Pendente:**
- [ ] Build WebGL otimizado
- [ ] Compressão Brotli habilitada
- [ ] Deploy em Azure Static Web Apps
- [ ] Analytics configurado (GA4)
- [ ] Tratamento de erros de rede

**Unity Player Settings:**
```
? Compression Format: Brotli
? Code Optimization: Size
? Exception Handling: Explicit Null Checks
? Strip Engine Code: Yes
```

---

## ?? AZURE - CHECKLIST DE PRODUÇÃO

### 15. ?? **Configurações Faltando no Azure**

**App Service Settings a configurar:**

```bash
# Produção
ASPNETCORE_ENVIRONMENT=Production

# MongoDB (já configurado)
MONGODB_URI=mongodb+srv://...

# Segurança
ALLOWED_ORIGINS=https://barbara.azurestaticapps.net,https://barbara.avila.inc

# JWT (se implementar)
JWT_SECRET=<secret-super-seguro-64-caracteres>
JWT_ISSUER=https://barbara.avila.inc
JWT_AUDIENCE=Barbara.API

# Logs
APPLICATIONINSIGHTS_CONNECTION_STRING=<do-azure-portal>

# Feature Flags
ENABLE_SWAGGER=false
ENABLE_DETAILED_ERRORS=false
```

---

### 16. ?? **Application Insights**

**Não configurado!**

**? ADICIONAR:**

```powershell
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

---

## ?? RESUMO EXECUTIVO

### ?? CRÍTICO (Corrigir ANTES do deploy)

| # | Problema | Risco | Prioridade |
|---|----------|-------|------------|
| 1 | CORS permite tudo | Segurança | ?? CRÍTICO |
| 2 | Credenciais no Git | Vazamento | ?? CRÍTICO |
| 5 | Sem autenticação | Acesso não autorizado | ?? CRÍTICO |
| 13 | MongoDB URI versionado | Credencial exposta | ?? CRÍTICO |

### ?? IMPORTANTE (Corrigir logo após deploy)

| # | Problema | Impacto | Prioridade |
|---|----------|---------|------------|
| 6 | Sem validação | Dados inválidos | ?? ALTO |
| 8 | Sem rate limiting | DDoS | ?? ALTO |
| 9 | Índices desabilitados | Performance | ?? ALTO |
| 10 | Sem cache | Performance | ?? MÉDIO |

### ?? MELHORIAS (Implementar gradualmente)

| # | Problema | Benefício | Prioridade |
|---|----------|-----------|------------|
| 3 | Código antigo | Organização | ?? BAIXO |
| 7 | Logging básico | Debugging | ?? MÉDIO |
| 11 | Sem testes | Qualidade | ?? MÉDIO |
| 14 | Unity não otimizado | UX | ?? BAIXO |

---

## ? PLANO DE AÇÃO - ORDEM RECOMENDADA

### **ANTES DO DEPLOY (30 min)**

```powershell
# 1. Corrigir CORS
# Editar: src/Barbara.API/Program.cs

# 2. Remover credenciais do Git
git rm --cached azure-appsettings.json publish-profile.xml
echo "azure-appsettings.json" >> .gitignore
echo "publish-profile.xml" >> .gitignore
git commit -m "security: Remove credenciais expostas"

# 3. Limpar MongoDB URI do appsettings.json
# Deixar vazio (usar Azure App Settings)

# 4. Commit e push
git push
```

### **LOGO APÓS DEPLOY (1h)**

1. ? Habilitar índices MongoDB
2. ? Adicionar validação de entrada
3. ? Configurar rate limiting
4. ? Adicionar logging estruturado

### **PRÓXIMAS SEMANAS**

1. ? Implementar autenticação JWT
2. ? Adicionar cache (Redis)
3. ? Criar testes automatizados
4. ? Otimizar Unity WebGL
5. ? Configurar Application Insights

---

## ?? CONCLUSÃO

**O projeto está 85% pronto para produção!**

**Bloqueadores:**
- ?? CORS inseguro
- ?? Credenciais expostas
- ?? Sem autenticação

**Correção rápida:** 30 minutos

**Depois disso:** ? **Deploy seguro!**

---

**Última atualização:** 2025-01-09 12:00 UTC
