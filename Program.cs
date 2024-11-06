using Microsoft.EntityFrameworkCore;
using PowerBI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexión y agrega ApplicationDbContext al contenedor de servicios
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 23))));

// Agrega servicios de controladores con vistas al contenedor
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura el pipeline de solicitud HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Access}/{action=Login}/{id?}");

app.Run();
