using Microsoft.EntityFrameworkCore;
using FocusSpace.DatabaseContext;

var builder = WebApplication.CreateBuilder(args);

// Configurar URLs
builder.WebHost.UseUrls("http://localhost:5244");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();

// Adicionar Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "StockFlow API", 
        Version = "v1",
        Description = "API para gerenciamento de estoque"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173", "http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Habilitar Swagger sempre (para desenvolvimento)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StockFlow API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

// CORS deve vir ANTES de qualquer middleware que precisa dele
app.UseCors("AllowAll");

// Aplicar migrations automaticamente (com tratamento de erro mais robusto)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    try
    {
        // Verificar se o banco está acessível
        if (db.Database.CanConnect())
        {
            Console.WriteLine("✓ Database connection successful");
            
            // Tentar aplicar migrations apenas se necessário
            var pendingMigrations = db.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                db.Database.Migrate();
                Console.WriteLine("✓ Database migrations applied");
            }
            else
            {
                Console.WriteLine("✓ Database is up to date");
            }
        }
        else
        {
            Console.WriteLine("⚠ Database connection failed - continuing without DB");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠ Database warning (continuing): {ex.Message}");
        // Continuar mesmo com erro de banco para testar CORS
    }
}

app.UseAuthorization();
app.MapControllers();

Console.WriteLine("🚀 Backend running at: http://localhost:5244");
Console.WriteLine("📊 Database: StockFlow");
Console.WriteLine("📚 Swagger UI: http://localhost:5244");

app.Run();
