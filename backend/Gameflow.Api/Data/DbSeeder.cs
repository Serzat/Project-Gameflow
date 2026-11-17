using Gameflow.Api.Accounts;
using Microsoft.AspNetCore.Identity;

namespace Gameflow.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. De rollen aanmaken als ze nog niet bestaan
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // 2. Een vast spelbegeleider-account aanmaken
        const string begeleiderEmail = "begeleider@gameflow.nl";

        var begeleider = await userManager.FindByEmailAsync(begeleiderEmail);
        if (begeleider is null)
        {
            begeleider = new User
            {
                UserName = begeleiderEmail,
                Email = begeleiderEmail,
                EmailConfirmed = true,
                DisplayName = "Spelbegeleider",
                CreatedAt = DateTime.UtcNow
            };

            await userManager.CreateAsync(begeleider, "Begeleider123!");
        }

        if (!await userManager.IsInRoleAsync(begeleider, Roles.Spelbegeleider))
        {
            await userManager.AddToRoleAsync(begeleider, Roles.Spelbegeleider);
        }
    }
}