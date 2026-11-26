using System.Globalization;
using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class FeedbackService(GameflowDb db, IStartprofielService startprofielService) : IFeedbackService
{
    private static readonly CultureInfo Nederlands = new("nl-NL");

    public async Task<FeedbackResponse?> GetFeedbackAsync(string resortType, CancellationToken ct)
    {
        var profiel = await startprofielService.GetProfielAsync(resortType, ct);
        if (profiel is null)
        {
            return null;
        }

        // Alle standen van alle teams, oudste ronde eerst
        var standen = await db.TeamStanden
            .AsNoTracking()
            .OrderBy(t => t.RondeNummer)
            .ToListAsync(ct);

        // De laatst berekende ronde van dit resort
        var standNu = standen.LastOrDefault(t => t.ResortType == resortType);
        if (standNu is null)
        {
            return null;
        }

        int ronde = standNu.RondeNummer;

        // De vorige ronde. Bij ronde 1 vergelijken we met de startwaarden
        var standVorige = standen.FirstOrDefault(t => t.ResortType == resortType && t.RondeNummer == ronde - 1);
        var nu = Cijfers.Van(standNu);
        var vorige = standVorige is null ? Cijfers.Van(profiel) : Cijfers.Van(standVorige);

        // Positie: hoeveel andere resorts hebben in deze ronde een hogere score?
        int positie = 1 + Spelopties.ResortTypes
            .Where(type => type != resortType)
            .Select(type => standen.LastOrDefault(t => t.ResortType == type && t.RondeNummer <= ronde))
            .Count(stand => stand is not null && Cijfers.Van(stand).Score > nu.Score);

        var kpis = new List<KpiVerandering>
        {
            MaakKpi("Bezettingsgraad", $"{nu.Bezetting}%", nu.Bezetting - vorige.Bezetting, "%", nu.Bezetting),
            MaakKpi("Gasttevredenheid", $"{nu.Gast} / 100", nu.Gast - vorige.Gast, "", nu.Gast),
            MaakKpi("Winst & Cashflow", "€ " + nu.Winst.ToString("N0", Nederlands),
                nu.Winst - vorige.Winst, "euro", (int)Math.Clamp(nu.Winst / 2500, 0, 100)), // € 250.000 = vol balkje
            MaakKpi("Duurzaamheid", $"{nu.Duurzaam} / 100", nu.Duurzaam - vorige.Duurzaam, "", nu.Duurzaam),
        };

        var goed = new List<string>();
        var beter = new List<string>();
        var advies = new List<string>();

        // Per KPI kijken of hij omhoog of omlaag ging
        Vergelijk("De bezettingsgraad", nu.Bezetting, vorige.Bezetting, "Investeer in marketing om meer gasten aan te trekken.");
        Vergelijk("De gasttevredenheid", nu.Gast, vorige.Gast, "Verbeter de service of faciliteiten om gasten tevredener te maken.");
        Vergelijk("De winst", nu.Winst, vorige.Winst, "Kies een goedkopere investering om je cashflow te beschermen.");
        Vergelijk("De duurzaamheidsscore", nu.Duurzaam, vorige.Duurzaam, "Investeer in duurzame keuzes, zoals zonnepanelen.");

        // KPI's die nog laag zijn
        if (nu.Bezetting < 50) { VoegToe(beter, "De bezettingsgraad is nog laag."); VoegToe(advies, "Investeer in marketing om meer gasten aan te trekken."); }
        if (nu.Gast < 50) { VoegToe(beter, "De gasttevredenheid is nog laag."); VoegToe(advies, "Verbeter de service of faciliteiten om gasten tevredener te maken."); }
        if (nu.Winst < 0) { VoegToe(beter, "Het resort maakt verlies."); VoegToe(advies, "Kies een goedkopere investering om je cashflow te beschermen."); }
        if (nu.Duurzaam < 50) { VoegToe(beter, "De duurzaamheidsscore is nog laag."); VoegToe(advies, "Investeer in duurzame keuzes, zoals zonnepanelen."); }

        // Nooit een leeg blok op het scherm
        if (goed.Count == 0) goed.Add("Deze ronde zijn er nog geen duidelijke verbeteringen.");
        if (beter.Count == 0) beter.Add("Geen grote problemen deze ronde, goed bezig!");
        if (advies.Count == 0) advies.Add("Houd je huidige strategie vast en blijf letten op je budget.");

        return new FeedbackResponse(
            TeamNaam: char.ToUpper(resortType[0]) + resortType[1..] + " Resort",
            RondeNummer: ronde,
            AantalRondes: Spelopties.AantalRondes,
            RondeAfgerond: true,          // een stand bestaat pas als de ronde berekend is
            AfgerondOp: null,
            TotaalScore: nu.Score,
            Positie: positie,
            AantalTeams: Spelopties.ResortTypes.Length,
            PuntenDezeRonde: nu.Score - vorige.Score,
            Kpis: kpis,
            WatGingGoed: goed,
            WatKanBeter: beter,
            Advies: advies,
            Beslissingen: []);            // TODO: koppelen zodra Beslissingen per resort is opgeslagen

        void Vergelijk(string naam, decimal waardeNu, decimal waardeVorige, string tip)
        {
            if (waardeNu > waardeVorige) VoegToe(goed, naam + " is gestegen.");
            else if (waardeNu < waardeVorige) { VoegToe(beter, naam + " is gedaald."); VoegToe(advies, tip); }
        }
    }

    // Maakt van een getal een KPI-kaart, bijvoorbeeld verschil 5 wordt "+5%"
    private static KpiVerandering MaakKpi(string naam, string waarde, decimal verschil, string eenheid, int balk)
    {
        string teken = verschil >= 0 ? "+" : "-";
        decimal absoluut = Math.Abs(verschil);

        string tekst = eenheid == "euro"
            ? (absoluut >= 1000 ? $"{teken}€{Math.Round(absoluut / 1000)}k" : $"{teken}€{absoluut:0}")
            : $"{teken}{absoluut:0}{eenheid}";

        return new KpiVerandering(naam, waarde, tekst, verschil >= 0, Math.Clamp(balk, 0, 100));
    }

    // Zelfde zin niet twee keer toevoegen
    private static void VoegToe(List<string> lijst, string zin)
    {
        if (!lijst.Contains(zin)) lijst.Add(zin);
    }

    // De vier KPI's die het feedbackscherm laat zien
    private record Cijfers(int Bezetting, int Gast, decimal Winst, int Duurzaam)
    {
        public int Score => ScoreBerekening.Bereken(Bezetting, Gast, Duurzaam);

        public static Cijfers Van(TeamStand t) =>
            new(t.Bezettingsgraad, t.Gasttevredenheid, t.Winst, t.Duurzaamheidsscore);

        public static Cijfers Van(StartprofielResponse p) =>
            new(p.Bezettingsgraad, p.Gasttevredenheid, 0, p.Duurzaamheidsscore);
    }
}
