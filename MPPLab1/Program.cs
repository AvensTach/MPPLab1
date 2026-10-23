using MPPLab1.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbProvider = builder.Configuration["DatabaseSettings:Provider"];
var connectionString = dbProvider == "PostgreSQL"
    ? builder.Configuration.GetConnectionString("PostgreSqlConnection")
    : builder.Configuration.GetConnectionString("SqliteConnection");

if (dbProvider == "PostgreSQL")
{
    builder.Services.AddDbContext<AppDbContext, PostgresDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<AppDbContext, SqliteDbContext>(options =>
        options.UseSqlite(connectionString));
}

builder.Services.AddControllersWithViews();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Wallets}/{action=Index}/{id?}");

app.Run();