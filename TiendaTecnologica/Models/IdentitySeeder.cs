using Microsoft.AspNetCore.Identity;

namespace TiendaTecnologica.Models
{
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            const string adminRole = "Admin";
            const string adminEmail = "tonvasroj060603@gmail.com";
            const string adminPassword = "Admin123";

            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            var user = await userManager.FindByEmailAsync(adminEmail);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, adminPassword);

                if (!result.Succeeded)
                {
                    var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
                    throw new Exception($"No se pudo crear el admin: {errors}");
                }
            }

            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }
    }
}