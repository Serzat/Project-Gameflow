using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class RanglijstService(GameflowDb db, IStartprofielService startprofielService) : IRanglijstService
{
    public async Task<RanglijstResponse> GetRanglijstAsync(string? eigenResort, CancellationToken ct)
    {
        // Alle standen van alle teams, oudste ronde eerst
        var standen = await db.TeamStanden
            .AsNoTracking()
            .OrderBy(t => t.RondeNummer)
            .ToListAsync(ct);

        // De huidige ronde is de hoogste ronde die al berekend is
        int ronde = standen.Count == 0 ? 1 : standen.Max(t => t.RondeNummer);

        var teams = new List<RanglijstTeam>();

        foreach (var type in Spelopties.ResortTypes)
        {
            var profiel = await startprofielService.GetProfielAsync(type, ct);
            var eigenStanden = standen.Where(t => t.ResortType == type).ToList();

            // Cijfers van nu: de laatste stand, of de startwaarden als er nog niets is gespeeld
            var standNu = eigenStanden.LastOrDefault();
            var nu = standNu is not null ? Cijfers.Van(standNu)
                   : profiel is not null ? Cijfers.Van(profiel)
                   : new Cijfers(0, 0, 0);

            // Cijfers van de vorige ronde, voor de trend
            var standVorige = standNu is null ? null : eigenStanden.LastOrDefault(t => t.RondeNummer < standNu.RondeNummer);
            var vorige = standVorige is not null ? Cijfers.Van(standVorige)
                       : standNu is not null && profiel is not null ? Cijfers.Van(profiel)
                       : nu;

            teams.Add(new RanglijstTeam(
                Naam: char.ToUpper(type[0]) + type[1..] + " Resort",
                Score: nu.Score,
                Bezetting: nu.Bezetting,
                Gast: nu.Gast,
                Duurzaam: nu.Duurzaam,
                Trend: nu.Score - vorige.Score,
                Eigen: type == eigenResort));
        }

        // Hoogste score bovenaan
        var gesorteerd = teams.OrderByDescending(t => t.Score).ToList();

        return new RanglijstResponse(ronde, Spelopties.AantalRondes, gesorteerd);
    }

    // De drie KPI's waar de score uit bestaat
    private record Cijfers(int Bezetting, int Gast, int Duurzaam)
    {
        public int Score => ScoreBerekening.Bereken(Bezetting, Gast, Duurzaam);

        public static Cijfers Van(TeamStand t) =>
            new(t.Bezettingsgraad, t.Gasttevredenheid, t.Duurzaamheidsscore);

        public static Cijfers Van(StartprofielResponse p) =>
            new(p.Bezettingsgraad, p.Gasttevredenheid, p.Duurzaamheidsscore);
    }
}
