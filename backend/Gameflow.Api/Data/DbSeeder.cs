using Gameflow.Api.Accounts;
using Gameflow.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var db = scope.ServiceProvider.GetRequiredService<GameflowDb>();

        await SeedRollenAsync(userManager, roleManager);
        await SeedStartprofielenAsync(db);
    }

    // ===== Rollen en het spelbegeleider-account =====
    private static async Task SeedRollenAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

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

    // ===== Startprofielen en de gekozen modus =====
    private static async Task SeedStartprofielenAsync(GameflowDb db)
    {
        if (!await db.ResortStartprofielen.AnyAsync())
        {
            // Standaard: elk resort begint met precies dezelfde cijfers.
            // Alleen de locatie hoort bij het type.
            var locaties = new Dictionary<string, string>
            {
                ["beach"] = "Bali",
                ["winter"] = "Zermatt",
                ["cruise"] = "Middellandse Zee",
                ["city"] = "Barcelona"
            };

            foreach (var (resortType, locatie) in locaties)
            {
                db.ResortStartprofielen.Add(new ResortStartprofiel
                {
                    Modus = "standaard",
                    ResortType = resortType,
                    Locatie = locatie,
                    Startbudget = 2500,
                    AantalKamers = 5,
                    AantalMedewerkers = 2,
                    Bezettingsgraad = 40,
                    Kamerprijs = 45,
                    Gasttevredenheid = 50,
                    Personeelstevredenheid = 50,
                    Duurzaamheidsscore = 30,
                    Marktaandeel = 2
                });
            }

            // Custom: elk type resort heeft een eigen startsituatie
            db.ResortStartprofielen.AddRange(
                new ResortStartprofiel
                {
                    Modus = "custom", ResortType = "beach", Locatie = "Bali",
                    Startbudget = 3000, AantalKamers = 6, AantalMedewerkers = 3,
                    Bezettingsgraad = 50, Kamerprijs = 55,
                    Gasttevredenheid = 60, Personeelstevredenheid = 55,
                    Duurzaamheidsscore = 25, Marktaandeel = 3
                },
                new ResortStartprofiel
                {
                    Modus = "custom", ResortType = "winter", Locatie = "Zermatt",
                    Startbudget = 4000, AantalKamers = 4, AantalMedewerkers = 2,
                    Bezettingsgraad = 45, Kamerprijs = 70,
                    Gasttevredenheid = 55, Personeelstevredenheid = 50,
                    Duurzaamheidsscore = 35, Marktaandeel = 2
                },
                new ResortStartprofiel
                {
                    Modus = "custom", ResortType = "cruise", Locatie = "Middellandse Zee",
                    Startbudget = 5000, AantalKamers = 3, AantalMedewerkers = 4,
                    Bezettingsgraad = 60, Kamerprijs = 90,
                    Gasttevredenheid = 50, Personeelstevredenheid = 45,
                    Duurzaamheidsscore = 20, Marktaandeel = 2
                },
                new ResortStartprofiel
                {
                    Modus = "custom", ResortType = "city", Locatie = "Barcelona",
                    Startbudget = 1500, AantalKamers = 8, AantalMedewerkers = 3,
                    Bezettingsgraad = 35, Kamerprijs = 40,
                    Gasttevredenheid = 45, Personeelstevredenheid = 50,
                    Duurzaamheidsscore = 40, Marktaandeel = 4
                });
        }

        // Eén rij met de gekozen modus. Standaard is "standaard"
        if (!await db.Spelinstellingen.AnyAsync())
        {
            db.Spelinstellingen.Add(new Spelinstelling { GekozenModus = "standaard" });;
        }

        await db.SaveChangesAsync();
    }
}