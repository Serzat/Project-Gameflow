using System.Text.Json;
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
        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

        await SeedRollenAsync(userManager, roleManager);
        await SeedStartprofielenAsync(db);
        await SeedEventsAsync(db, env);
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

        if (!await db.Spelinstellingen.AnyAsync())
        {
            db.Spelinstellingen.Add(new Spelinstelling { GekozenModus = "standaard" });
        }

        await db.SaveChangesAsync();
    }

    // ===== Events en kansen uit Data/Seed/events.json =====
    // Bij elke start wordt het bestand opnieuw ingelezen:
    // bestaande events (zelfde titel) worden bijgewerkt, nieuwe toegevoegd,
    // en events die niet meer in het bestand staan verwijderd.
    // De lotingen van eerdere rondes blijven bewaard.
    private static async Task SeedEventsAsync(GameflowDb db, IWebHostEnvironment env)
    {
        var pad = Path.Combine(env.ContentRootPath, "Data", "Seed", "events.json");

        if (!File.Exists(pad))
        {
            return;
        }

        var json = await File.ReadAllTextAsync(pad);
        var opties = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var bestand = JsonSerializer.Deserialize<EventsBestand>(json, opties)
            ?? throw new InvalidOperationException("events.json is leeg of ongeldig.");

        // Controleer het bestand, zodat een typfout meteen opvalt
        foreach (var e in bestand.Events)
        {
            if (e.Soort != "globaal" && e.Soort != "individueel")
            {
                throw new InvalidOperationException(
                    $"Soort moet 'globaal' of 'individueel' zijn bij event '{e.Titel}' (events.json).");
            }

            foreach (var effect in e.Effecten)
            {
                if (!Spelopties.Kpis.Contains(effect.Kpi))
                {
                    throw new InvalidOperationException(
                        $"Onbekende kpi '{effect.Kpi}' in event '{e.Titel}' (events.json).");
                }
            }
        }

        // De kansen opslaan in de spelinstelling
        var instelling = await db.Spelinstellingen.FirstAsync();
        instelling.KansGlobaal = bestand.KansGlobaal;
        instelling.KansIndividueel = bestand.KansIndividueel;

        // Bestaande events ophalen, met hun effecten
        var bestaande = await db.SpelEvents.Include(e => e.Effecten).ToListAsync();

        foreach (var e in bestand.Events)
        {
            var spelEvent = bestaande.FirstOrDefault(b => b.Titel == e.Titel);

            if (spelEvent is null)
            {
                spelEvent = new SpelEvent
                {
                    Soort = e.Soort,
                    Categorie = e.Categorie,
                    Titel = e.Titel,
                    Omschrijving = e.Omschrijving
                };
                db.SpelEvents.Add(spelEvent);
            }

            spelEvent.Soort = e.Soort;
            spelEvent.Categorie = e.Categorie;
            spelEvent.Omschrijving = e.Omschrijving;
            spelEvent.ImpactBeach = e.Impact.GetValueOrDefault("beach", 1);
            spelEvent.ImpactWinter = e.Impact.GetValueOrDefault("winter", 1);
            spelEvent.ImpactCruise = e.Impact.GetValueOrDefault("cruise", 1);
            spelEvent.ImpactCity = e.Impact.GetValueOrDefault("city", 1);

            // De effecten opnieuw opbouwen
            spelEvent.Effecten.Clear();
            foreach (var f in e.Effecten)
            {
                spelEvent.Effecten.Add(new SpelEventEffect { Kpi = f.Kpi, Waarde = f.Waarde });
            }
        }

        // Events die niet meer in het bestand staan weghalen
        var titels = bestand.Events.Select(e => e.Titel).ToHashSet();
        db.SpelEvents.RemoveRange(bestaande.Where(b => !titels.Contains(b.Titel)));

        await db.SaveChangesAsync();
    }

    // Hoe events.json eruitziet
    private record EventsBestand(decimal KansGlobaal, decimal KansIndividueel, List<EventInvoer> Events);

    private record EventInvoer(
        string Soort,
        string Categorie,
        string Titel,
        string Omschrijving,
        Dictionary<string, decimal> Impact,
        List<EffectInvoer> Effecten);

    private record EffectInvoer(string Kpi, decimal Waarde);
}