 using Microsoft.EntityFrameworkCore;
using ProductApi.Models;
using ProductApi.Endpoints;
using ProductApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging to ensure it outputs to console
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Information);

builder.Services.AddDbContext<Db>((sp, options) =>
{
  options.UseNpgsql("Host=localhost;Port=5432;Username=product_user;Password=change_me;Database=productdb");
});

// Scoped because it depends on DbContext which is also scoped to avoid concurrency issues
builder.Services.AddScoped<CategoryService>(); 
builder.Services.AddScoped<ProductService>();


var app = builder.Build();


await using var scope = app.Services.CreateAsyncScope();
var db = scope.ServiceProvider.GetRequiredService<Db>();
var canConnect = await db.Database.CanConnectAsync();
await db.Database.MigrateAsync();

// Get logger and log the connection status
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Can connect to database: {CanConnect}", canConnect);

app.MapGet("/", static () => "Hello World!");

// Map endpoint groups from separate files
app.MapProductEndpoints();
app.MapCategoryEndpoints();

app.Run();
