using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Sion.DAL.Context;
using Sion.DAL.Data.Seeders;
using Sion.DAL.Interfaces;
using Sion.DAL.Repositories;
using Sion.BLL.Interfaces;
using Sion.BLL.Services;
using Sion.PL.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ??????????????????????????????????????????????????????????????
// SERVICIOS
// ??????????????????????????????????????????????????????????????

// ?? Razor Pages ???????????????????????????????????????????????
builder.Services.AddRazorPages();

// Antiforgery: permite que los POST AJAX (PayPal) envíen el token por header
builder.Services.AddAntiforgery(options => options.HeaderName = "RequestVerificationToken");

// ?? Base de datos ?????????????????????????????????????????????
builder.Services.AddDbContext<SionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SionDb")));

// Repositorios DAL (se agregan en EPIC-003)
builder.Services.AddScoped<ISeccionHomeRepository, SeccionHomeRepository>();
builder.Services.AddScoped<IImagenGaleriaRepository, ImagenGaleriaRepository>();
builder.Services.AddScoped<IDonacionRepository, DonacionRepository>();
builder.Services.AddScoped<IConfiguracionSitioRepository, ConfiguracionSitioRepository>();
builder.Services.AddScoped<ILogAuditoriaRepository, LogAuditoriaRepository>();

//  Servicios BLL (se agregan en EPIC-003) 
builder.Services.AddScoped<ISeccionHomeService, SeccionHomeService>();
builder.Services.AddScoped<IImagenGaleriaService, ImagenGaleriaService>();
builder.Services.AddScoped<IDonacionService, DonacionService>();
builder.Services.AddScoped<IConfiguracionSitioService, ConfiguracionSitioService>();
builder.Services.AddScoped<ILogAuditoriaService, LogAuditoriaService>();
builder.Services.AddScoped<IPayPalService, PayPalService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// Servicio de correo
builder.Services.AddScoped<ICorreoService, CorreoService>();
builder.Services.AddHttpClient();

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    // Bloqueo por intentos fallidos: 5 intentos, 15 minutos de bloqueo
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers      = true;
})
   .AddEntityFrameworkStores<SionDbContext>()
   .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Admin/Account/Login";
    options.AccessDeniedPath = "/Admin/Account/Login";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
});

//Servicio de cache en memoria
builder.Services.AddMemoryCache();
// ??????????????????????????????????????????????????????????????
// PIPELINE HTTP (middleware)
// ??????????????????????????????????????????????????????????????

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
// Seeders
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SionDbContext>();
    await context.Database.MigrateAsync();
    await AdminSeeder.SeedAsync(services);
    await SiteDataSeeder.SeedAsync(context);
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Run();