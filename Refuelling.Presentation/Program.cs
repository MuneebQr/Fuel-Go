using Microsoft.EntityFrameworkCore;
using Refuelling.DAL.Data;
using Refuelling.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Add SQLite Database Connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=refuelling.db"));

// 2. Register Business Logic Service
builder.Services.AddScoped<RefuellingService>();

// 3. Add Controllers & Static Files support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// Enable serving static files (HTML, CSS, JS from wwwroot)
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Default route to serve index.html
app.MapFallbackToFile("index.html");

app.Run();