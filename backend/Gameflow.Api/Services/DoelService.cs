using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class DoelService(GameflowDb db) : IDoelService
{
    public async Task<DoelenResponse> GetDoelenAsync(
        string resortType,
        int huidigeRonde,
        IReadOnlyDictionary<string, decimal> huidigeWaarden,
        CancellationToken ct)
    {
        // 1. Welke doelen gelden? Standaard: dezelfde voor iedereen ("alle"), custom: per resorttype
        var instelling = await db.Spelinstellingen.AsNoTracking().FirstAsync(ct);
        var modus = instelling.GekozenModus;
        var doelType = modus == "standaard" ? "alle" : resortType;

        var alleDoelen = await db.Doelen
            .AsNoTracking()
            .Where(d => d.Modus == modus && d.ResortType == doelType)
            .OrderBy(d => d.Level)
            .ThenBy(d => d.Volgorde)
            .ToListAsync(ct);

        // Geen doelen in doelen.json voor deze modus/dit type
        if (alleDoelen.Count == 0)
        {
            return new DoelenResponse(1, false, 0, 0, []);
        }

        // 2. Welke doelen heeft dit team al eerder gehaald? (DoelId -> ronde)
        var behaald = await db.BehaaldeDoelen
            .AsNoTracking()
            .Where(b => b.ResortType == resortType)
            .ToDictionaryAsync(b => b.DoelId, b => b.RondeNummer, ct);

        // 3. Level voor level kijken: zijn alle doelen gehaald, dan door naar het volgende level
        var levels = alleDoelen.Select(d => d.Level).Distinct().ToList();
        var nieuwBehaald = new List<BehaaldDoel>();
        var resortLevel = 1;
        var huidigeSet = new List<Doel>();
        var allesBehaald = false;

        foreach (var level in levels)
        {
            huidigeSet = alleDoelen.Where(d => d.Level == level).ToList();

            // Nieuwe doelen afvinken: is de kpi op of boven de doelwaarde?
            foreach (var doel in huidigeSet)
            {
                if (behaald.ContainsKey(doel.Id))
                {
                    continue; // eenmaal gehaald blijft gehaald
                }

                var waarde = huidigeWaarden.GetValueOrDefault(doel.Kpi);
                if (waarde >= doel.Doelwaarde)
                {
                    behaald[doel.Id] = huidigeRonde;
                    nieuwBehaald.Add(new BehaaldDoel
                    {
                        ResortType = resortType,
                        DoelId = doel.Id,
                        RondeNummer = huidigeRonde
                    });
                }
            }

            // Nog niet alles gehaald? Dan blijft het team op dit level
            if (!huidigeSet.All(d => behaald.ContainsKey(d.Id)))
            {
                break;
            }

            // Alles gehaald: het resort gaat een level omhoog
            resortLevel++;
            allesBehaald = level == levels[^1];
        }

        // 4. Nieuw behaalde doelen opslaan
        if (nieuwBehaald.Count > 0)
        {
            db.BehaaldeDoelen.AddRange(nieuwBehaald);
            try
            {
                await db.SaveChangesAsync(ct);
            }
            catch (DbUpdateException)
            {
                // Tegelijk opgeslagen door een ander verzoek (unieke index): niet erg
                db.ChangeTracker.Clear();
            }
        }

        // 5. De doelen van het huidige level teruggeven
        // (alles gehaald: de laatste set, allemaal afgevinkt)
        var doelen = huidigeSet.Select(d => new DoelResponse(
            d.Id,
            d.Volgorde,
            d.Titel,
            d.Kpi,
            d.Doelwaarde,
            huidigeWaarden.GetValueOrDefault(d.Kpi),
            behaald.ContainsKey(d.Id),
            behaald.TryGetValue(d.Id, out var ronde) ? (int?)ronde : null)).ToList();

        return new DoelenResponse(
            resortLevel,
            allesBehaald,
            doelen.Count(d => d.Behaald),
            doelen.Count,
            doelen);
    }
}