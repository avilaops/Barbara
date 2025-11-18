using Barbara.Infrastructure.Data;
using Barbara.Infrastructure.Repositories;
using Barbara.Domain.Entities;
using Barbara.Application.Services;
using Barbara.API.Middleware;
using MongoDB.Driver;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Carregar variáveis de ambiente do .env
if (File.Exists(".env"))
{
    Env.Load();
}

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB Configuration
var mongoUri = Environment.GetEnvironmentVariable("MONGODB_URI")
    ?? builder.Configuration.GetConnectionString("MongoDB")
    ?? throw new InvalidOperationException("MongoDB connection string not found. Set MONGODB_URI in .env file.");

var mongoClient = new MongoClient(mongoUri);
var mongoDatabase = mongoClient.GetDatabase("barbara");

// Register MongoDB Context
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);
builder.Services.AddSingleton(new MongoDbContext(mongoUri, "barbara"));

// Register Repositories
builder.Services.AddScoped<IRepository<Cliente>>(sp =>
    new MongoRepository<Cliente>(sp.GetRequiredService<IMongoDatabase>(), "clientes"));
builder.Services.AddScoped<IRepository<Produto>>(sp =>
    new MongoRepository<Produto>(sp.GetRequiredService<IMongoDatabase>(), "produtos"));
builder.Services.AddScoped<IRepository<Categoria>>(sp =>
    new MongoRepository<Categoria>(sp.GetRequiredService<IMongoDatabase>(), "categorias"));
builder.Services.AddScoped<IRepository<Pedido>>(sp =>
    new MongoRepository<Pedido>(sp.GetRequiredService<IMongoDatabase>(), "pedidos"));
builder.Services.AddScoped<IRepository<Configuracao>>(sp =>
    new MongoRepository<Configuracao>(sp.GetRequiredService<IMongoDatabase>(), "configuracoes"));

// Register Application Services
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();

// CORS - Configurado para produ��o
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? new[] {
        "https://barbara.azurestaticapps.net",  // Unity WebGL
        "https://barbara.avila.inc",           // Dom�nio customizado
        "https://admin.barbara.avila.inc"        // Admin (futuro)
    };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
 {
     if (builder.Environment.IsDevelopment())
     {
         // Desenvolvimento: permite localhost
         policy.AllowAnyOrigin()
           .AllowAnyMethod()
       .AllowAnyHeader()
    .WithExposedHeaders("X-Total-Count", "X-Page", "X-Page-Size");
     }
     else
     {
         // Produ��o: apenas origens espec�ficas
         policy.WithOrigins(allowedOrigins)
          .AllowAnyMethod()
                     .AllowAnyHeader()
         .AllowCredentials()
         .WithExposedHeaders("X-Total-Count", "X-Page", "X-Page-Size");
     }
 });
});

var app = builder.Build();

// NOTA: Cria��o de �ndices desabilitada temporariamente (executar manualmente se necess�rio)
// Os �ndices ser�o criados automaticamente pelo MongoDB Atlas quando necess�rio
/*
// Create MongoDB indexes on startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
    await context.CreateIndexesAsync();
}
*/

// Configure the HTTP request pipeline.
app.UseErrorHandling(); // Middleware de tratamento de erros

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => new
{
    status = "healthy",
    database = "mongodb",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName
})
.WithName("HealthCheck")
.WithOpenApi();

app.Run();
