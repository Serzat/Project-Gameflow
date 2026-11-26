using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class EventService(GameflowDb db) : IEventService
{
    private const string Globaal = "alle";

    public async Task<List<EventResponse>> GetEventsVoorTeamAsync(string resortType, int huidigeRonde, CancellationToken ct)
    {
        var events = new List<EventResponse>();

        for (int ronde = huidigeRonde; ronde >= 1; ronde--)
        {
            foreach (var spelEvent in await GetEventsVanRondeAsync(resortType, ronde, ct))
            {
                var impact = Impact(spelEvent, resortType);

                events.Add(new EventResponse(
                    spelEvent.Id,
                    spelEvent.Soort,
                    spelEvent.Categorie,
                    spelEvent.Titel,
                    spelEvent.Omschrijving,
                    ronde,
                    Nieuw: ronde == huidigeRonde,
                    GeenInvloed: impact == 0,
                    spelEvent.Effecten
                        .Select(f => new EffectResponse(f.Kpi, Math.Round(f.Waarde * impact, 1)))
                        .ToList()));
            }
        }

        return events;
    }

    public async Task<Dictionary<string, decimal>> GetEffectenVoorRondeAsync(string resortType, int ronde, CancellationToken ct)
    {
        var totaal = new Dictionary<string, decimal>();

        foreach (var spelEvent in await GetEventsVanRondeAsync(resortType, ronde, ct))
        {
            var impact = Impact(spelEvent, resortType);

            foreach (var effect in spelEvent.Effecten)
            {
                totaal[effect.Kpi] = totaal.GetValueOrDefault(effect.Kpi) + effect.Waarde * impact;
            }
        }

        return totaal;
    }

    // De events van één ronde voor één team: het globale event en het eigen event.
    // Is er voor deze ronde nog niet geloot, dan gebeurt dat nu.
    private async Task<List<SpelEvent>> GetEventsVanRondeAsync(string resortType, int ronde, CancellationToken ct)
    {
        var globaal = await ZorgVoorLotingAsync(ronde, Globaal, ct);
        var eigen = await ZorgVoorLotingAsync(ronde, resortType, ct);

        return new[] { globaal, eigen }
            .Where(t => t.SpelEvent is not null)
            .Select(t => t.SpelEvent!)
            .ToList();
    }

    // Haalt de loting op, of loot als dat nog niet gebeurd is
    private async Task<RondeTrekking> ZorgVoorLotingAsync(int ronde, string doel, CancellationToken ct)
    {
        var bestaand = await HaalLotingOpAsync(ronde, doel, ct);
        if (bestaand is not null)
        {
            return bestaand;
        }

        bool isGlobaal = doel == Globaal;

        var instelling = await db.Spelinstellingen.AsNoTracking().FirstAsync(ct);
        var kans = isGlobaal ? instelling.KansGlobaal : instelling.KansIndividueel;

        SpelEvent? gekozen = null;

        // Gooi de dobbelsteen: valt het getal onder de kans, dan gebeurt er iets
        if ((decimal)Random.Shared.NextDouble() < kans)
        {
            // Events die dit doel al eens gehad heeft, tellen niet mee (geen herhaling)
            var alGehad = await db.RondeTrekkingen
                .Where(r => r.Doel == doel && r.SpelEventId != null)
                .Select(r => r.SpelEventId!.Value)
                .ToListAsync(ct);

            var soort = isGlobaal ? "globaal" : "individueel";

            var kandidaten = await db.SpelEvents
                .Include(e => e.Effecten)
                .Where(e => e.Soort == soort && !alGehad.Contains(e.Id))
                .ToListAsync(ct);

            // Een individueel event met impact 0 past niet bij dit type resort
            if (!isGlobaal)
            {
                kandidaten = kandidaten.Where(e => Impact(e, doel) > 0).ToList();
            }

            if (kandidaten.Count > 0)
            {
                gekozen = kandidaten[Random.Shared.Next(kandidaten.Count)];
            }
        }

        var loting = new RondeTrekking
        {
            RondeNummer = ronde,
            Doel = doel,
            SpelEventId = gekozen?.Id,
            SpelEvent = gekozen
        };

        db.RondeTrekkingen.Add(loting);

        try
        {
            await db.SaveChangesAsync(ct);
            return loting;
        }
        catch (DbUpdateException)
        {
            // Een ander verzoek heeft op precies hetzelfde moment geloot
            // (de unique index hield het tegen). Gebruik die uitslag.
            db.Entry(loting).State = EntityState.Detached;
            return (await HaalLotingOpAsync(ronde, doel, ct))!;
        }
    }

    private Task<RondeTrekking?> HaalLotingOpAsync(int ronde, string doel, CancellationToken ct)
    {
        return db.RondeTrekkingen
            .Include(r => r.SpelEvent)
            .ThenInclude(e => e!.Effecten)
            .FirstOrDefaultAsync(r => r.RondeNummer == ronde && r.Doel == doel, ct);
    }

    // Hoe hard een event een bepaald type resort raakt
    private static decimal Impact(SpelEvent e, string resortType) => resortType switch
    {
        "beach" => e.ImpactBeach,
        "winter" => e.ImpactWinter,
        "cruise" => e.ImpactCruise,
        "city" => e.ImpactCity,
        _ => 1
    };
}