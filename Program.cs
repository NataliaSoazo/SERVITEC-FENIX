using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // Redirigir para login
        options.LogoutPath = "/Home/Logout"; // Redirigir para logout
        options.AccessDeniedPath = "/Home/Index"; // Redirigir para acceso denegado
    });

// Autorización con política de administrador
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador", policy => 
        policy.RequireClaim("Rol", "ADMINISTRADOR"));
});

var app = builder.Build();

// Redirigir HTTP a HTTPS
app.UseHttpsRedirection();

// Servir archivos estáticos
app.UseStaticFiles();
app.UseRouting();
// Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Mapeo de rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=Loguin}/{id?}");

app.MapControllerRoute(
    name: "usuarios",
    pattern: "usuarios/{action=Index}/{id?}",
    defaults: new { controller = "Usuario" });

app.Run();