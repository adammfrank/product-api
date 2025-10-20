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
  var config = sp.GetRequiredService<IConfiguration>();
  var connectionString = config.GetConnectionString("DefaultConnection");
  options.UseNpgsql(connectionString);
});

// Scoped because it depends on DbContext which is also scoped to avoid concurrency issues
builder.Services.AddScoped<CategoryService>(); 
builder.Services.AddScoped<ProductService>();


var app = builder.Build();


await using var scope = app.Services.CreateAsyncScope();

var db = scope.ServiceProvider.GetRequiredService<Db>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
var maxRetries = 10;
var delayMs = 2000;
var connected = false;
for (int i = 0; i < maxRetries; i++)
{
  try
  {
    connected = await db.Database.CanConnectAsync();
    if (connected)
    {
      logger.LogInformation($"Connected to database on attempt {i+1}");
      break;
    }
  }
  catch (Exception ex)
  {
    logger.LogWarning(ex, $"Database connection attempt {i+1} failed");
  }
  await Task.Delay(delayMs);
}
if (!connected)
{
  logger.LogError($"Could not connect to database after {maxRetries} attempts");
  throw new Exception("Could not connect to database");
}
await db.Database.MigrateAsync();

app.MapGet("/", static () => "Hello World!");

// Global exception handler - return ProblemDetails JSON and log errors
app.UseExceptionHandler(errorApp =>
{
  errorApp.Run(async context =>
  {
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
    var ex = feature?.Error;

    int status = StatusCodes.Status500InternalServerError;
    string title = "An unexpected error occurred.";

    logger.LogError(ex, "Unhandled exception while processing request");

    var problem = new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
      Status = status,
      Title = title,
      Detail = null
    };

    context.Response.StatusCode = status;
    context.Response.ContentType = "application/problem+json";
    await context.Response.WriteAsJsonAsync(problem);
  });
});

// Map endpoint groups from separate files
app.MapProductEndpoints();
app.MapCategoryEndpoints();

app.Run();
