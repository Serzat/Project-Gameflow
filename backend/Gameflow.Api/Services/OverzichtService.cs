using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class OverzichtService(
    GameflowDb db,
    IStartprofielService startprofielService,
    IEventService eventService,
    IDoelService doelService) : IOverzichtService
{
    public async Task<OverzichtResponse?> GetOverzichtAsync(string resortType, string speler, CancellationToken ct)
    {
        // De startwaarden van dit resorttype (in de gekozen modus)
        var profiel = await startprofielService.GetProfielAsync(resortType, ct);
        if (profiel is null)
        {
            return null;
        }

        // Alle standen van dit team, oudste eerst
        var standen = await db.TeamStanden
            .AsNoTracking()
            .Where(t => t.ResortType == resortType)
            .OrderBy(t => t.RondeNummer)
            .ToListAsync(ct);

        // De huidige ronde is de laatste stand. Nog geen stand? Dan is het ronde 1
        var laatste = standen.LastOrDefault();
        int ronde = laatste?.RondeNummer ?? 1;

        // De cijfers van nu: de laatste stand, of de startwaarden in ronde 1
        var nu = laatste is null ? Cijfers.Van(profiel) : Cijfers.Van(laatste);

        // De cijfers van de vorige ronde, om de verandering te laten zien
        Cijfers? vorige = null;
        if (ronde > 1)
        {
            var standVorige = standen.FirstOrDefault(t => t.RondeNummer == ronde - 1);
            vorige = standVorige is null ? Cijfers.Van(profiel) : Cijfers.Van(standVorige);
        }

        var team = new TeamInfo(
            speler, resortType, profiel.Locatie,
            nu.Budget, nu.AantalKamers, nu.AantalMedewerkers);

        var kpis = new List<KpiResponse>
        {
            Kpi("bezettingsgraad", c => c.Bezettingsgraad),
            Kpi("kamerprijs", c => c.Kamerprijs),
            Kpi("winst", c => c.Winst),
            Kpi("gasttevredenheid", c => c.Gasttevredenheid),
            Kpi("personeelstevredenheid", c => c.Personeelstevredenheid),
            Kpi("duurzaamheidsscore", c => c.Duurzaamheidsscore),
            Kpi("marktaandeel", c => c.Marktaandeel),
            Kpi("bezetteKamers", c => c.BezetteKamers),
        };

        var events = await eventService.GetEventsVoorTeamAsync(resortType, ronde, ct);

        // De doelen: welke zijn gehaald met de cijfers van nu, en op welk level zit het resort?
        var doelen = await doelService.GetDoelenAsync(resortType, ronde, nu.AlsWaarden(), ct);

        return new OverzichtResponse(team, new RondeInfo(ronde, Spelopties.AantalRondes), kpis, events, doelen);

        // Maakt een KPI-kaart met de waarde van nu en die van de vorige ronde
        KpiResponse Kpi(string key, Func<Cijfers, decimal> waarde) =>
            new(key, waarde(nu), vorige is null ? null : waarde(vorige));
    }

    // De cijfers van een team op één moment.
    // Zo kunnen startwaarden en teamstanden op dezelfde manier worden gebruikt.
    private record Cijfers(
        decimal Budget, int AantalKamers, int AantalMedewerkers, int Bezettingsgraad,
        decimal Kamerprijs, decimal Winst, int Gasttevredenheid, int Personeelstevredenheid,
        int Duurzaamheidsscore, int Marktaandeel)
    {
        public int BezetteKamers => (int)Math.Round(AantalKamers * Bezettingsgraad / 100m);

        // Alle cijfers met de kpi-namen uit Spelopties.Kpis (voor de doelen)
        public Dictionary<string, decimal> AlsWaarden() => new()
        {
            ["budget"] = Budget,
            ["aantalKamers"] = AantalKamers,
            ["aantalMedewerkers"] = AantalMedewerkers,
            ["bezettingsgraad"] = Bezettingsgraad,
            ["kamerprijs"] = Kamerprijs,
            ["winst"] = Winst,
            ["gasttevredenheid"] = Gasttevredenheid,
            ["personeelstevredenheid"] = Personeelstevredenheid,
            ["duurzaamheidsscore"] = Duurzaamheidsscore,
            ["marktaandeel"] = Marktaandeel,
        };

        public static Cijfers Van(StartprofielResponse p) => new(
            p.Startbudget, p.AantalKamers, p.AantalMedewerkers, p.Bezettingsgraad,
            p.Kamerprijs, 0, p.Gasttevredenheid, p.Personeelstevredenheid,
            p.Duurzaamheidsscore, p.Marktaandeel);

        public static Cijfers Van(TeamStand t) => new(
            t.Budget, t.AantalKamers, t.AantalMedewerkers, t.Bezettingsgraad,
            t.Kamerprijs, t.Winst, t.Gasttevredenheid, t.Personeelstevredenheid,
            t.Duurzaamheidsscore, t.Marktaandeel);
    }
}