using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sion.DAL.Data.Seeders
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            // Crear rol Admin si no existe
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            // Leer credenciales desde User Secrets (dev) o env vars (prod)
            var adminEmail = config["AdminSeed:Email"] ?? "admin@sioncr.org";
            var adminPassword = config["AdminSeed:Password"] ?? throw new InvalidOperationException("AdminSeed:Password no configurado en User Secrets.");

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Garantizar bloqueo por intentos habilitado (para admins creados antes de configurar Lockout)
            adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser != null && !await userManager.GetLockoutEnabledAsync(adminUser))
                await userManager.SetLockoutEnabledAsync(adminUser, true);
        }
    }
}
