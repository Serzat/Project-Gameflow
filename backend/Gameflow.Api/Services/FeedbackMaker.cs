using System.Globalization;
using Gameflow.Api.Data;
using Gameflow.Api.Models;

namespace Gameflow.Api.Services
{
    // Maakt van de resultaten van een ronde de feedback voor een team
    public static class FeedbackMaker
    {
        private static readonly CultureInfo Nederlands = new CultureInfo("nl-NL");

        public static Feedback Maak(Team team, Ronde ronde, RondeResultaat resultaat)
        {
            // Het resultaat van de vorige ronde (bestaat niet bij ronde 1)
            var vorige = FeedbackGegevens.RondeResultaten
                .FirstOrDefault(r => r.TeamId == team.Id &&
                                     r.CompetitieId == team.CompetitieId &&
                                     r.RondeNummer == ronde.Nummer - 1);

            var feedback = new Feedback
            {
                TeamNaam = team.Naam,
                RondeNummer = ronde.Nummer,
                AantalRondes = SpelGegevens.Rondes.Count(r => r.CompetitieId == team.CompetitieId),
                PuntenDezeRonde = resultaat.Punten,
                TotaalScore = BerekenTotaalScore(team.Id, team.CompetitieId, ronde.Nummer),
                Beslissingen = resultaat.Beslissingen
            };

            // Positie en datum
            BepaalPositie(feedback, team, ronde.Nummer);
            BepaalRondeStatus(feedback, ronde);

            // De vier KPI's
            int vorigeBezetting = vorige?.Bezettingsgraad ?? resultaat.Bezettingsgraad;
            int vorigeTevredenheid = vorige?.Gasttevredenheid ?? resultaat.Gasttevredenheid;
            int vorigeWinst = vorige?.Winst ?? resultaat.Winst;
            int vorigeDuurzaamheid = vorige?.Duurzaamheid ?? resultaat.Duurzaamheid;

            feedback.Kpis.Add(MaakKpi("Bezettingsgraad",
                resultaat.Bezettingsgraad + "%",
                resultaat.Bezettingsgraad - vorigeBezetting, "%",
                resultaat.Bezettingsgraad));

            feedback.Kpis.Add(MaakKpi("Gasttevredenheid",
                resultaat.Gasttevredenheid + " / 100",
                resultaat.Gasttevredenheid - vorigeTevredenheid, "",
                resultaat.Gasttevredenheid));

            feedback.Kpis.Add(MaakKpi("Winst & Cashflow",
                "€ " + resultaat.Winst.ToString("N0", Nederlands),
                resultaat.Winst - vorigeWinst, "euro",
                Math.Clamp(resultaat.Winst / 2500, 0, 100)));   // € 250.000 = vol balkje

            feedback.Kpis.Add(MaakKpi("Duurzaamheid",
                resultaat.Duurzaamheid + " / 100",
                resultaat.Duurzaamheid - vorigeDuurzaamheid, "",
                resultaat.Duurzaamheid));

            // Teksten voor wat goed ging, wat beter kan en advies
            VulTeksten(feedback, resultaat, vorige);

            return feedback;
        }

        // Alle punten van ronde 1 tot en met deze ronde bij elkaar
        private static int BerekenTotaalScore(int teamId, int competitieId, int totEnMetRonde)
        {
            return FeedbackGegevens.RondeResultaten
                .Where(r => r.TeamId == teamId &&
                            r.CompetitieId == competitieId &&
                            r.RondeNummer <= totEnMetRonde)
                .Sum(r => r.Punten);
        }

        // De plek van het team vergeleken met de andere teams
        private static void BepaalPositie(Feedback feedback, Team team, int rondeNummer)
        {
            var teams = SpelGegevens.Teams
                .Where(t => t.CompetitieId == team.CompetitieId)
                .ToList();

            int aantalHoger = teams.Count(t =>
                BerekenTotaalScore(t.Id, team.CompetitieId, rondeNummer) > feedback.TotaalScore);

            feedback.Positie = aantalHoger + 1;
            feedback.AantalTeams = teams.Count;
        }

        // Of de ronde al voorbij is en wanneer
        private static void BepaalRondeStatus(Feedback feedback, Ronde ronde)
        {
            if (ronde.GestartOpUtc == null)
            {
                feedback.RondeAfgerond = false;
                return;
            }

            DateTime eindtijd = ronde.GestartOpUtc.Value.AddMinutes(ronde.DuurMinuten);

            feedback.RondeAfgerond = DateTime.UtcNow >= eindtijd;
            feedback.AfgerondOp = eindtijd.ToLocalTime().ToString("dd-MM-yyyy");
        }

        private static KpiVerandering MaakKpi(string naam, string waarde, int verschil, string eenheid, int balk)
        {
            string teken = verschil >= 0 ? "+" : "-";
            int absoluut = Math.Abs(verschil);
            string verschilTekst;

            if (eenheid == "euro")
            {
                // Grote bedragen korter maken: 20000 wordt €20k
                verschilTekst = absoluut >= 1000
                    ? teken + "€" + (absoluut / 1000) + "k"
                    : teken + "€" + absoluut;
            }
            else
            {
                verschilTekst = teken + absoluut + eenheid;
            }

            return new KpiVerandering
            {
                Naam = naam,
                Waarde = waarde,
                Verschil = verschilTekst,
                Gestegen = verschil >= 0,
                BalkProcent = Math.Clamp(balk, 0, 100)
            };
        }

        private static void VulTeksten(Feedback feedback, RondeResultaat nu, RondeResultaat? vorige)
        {
            // Per KPI kijken of hij omhoog of omlaag ging
            if (vorige != null)
            {
                VergelijkKpi(feedback, "De bezettingsgraad", nu.Bezettingsgraad, vorige.Bezettingsgraad,
                    "Investeer in marketing om meer gasten aan te trekken.");

                VergelijkKpi(feedback, "De gasttevredenheid", nu.Gasttevredenheid, vorige.Gasttevredenheid,
                    "Verbeter de service of faciliteiten om gasten tevredener te maken.");

                VergelijkKpi(feedback, "De winst", nu.Winst, vorige.Winst,
                    "Kies een goedkopere investering om je cashflow te beschermen.");

                VergelijkKpi(feedback, "De duurzaamheidsscore", nu.Duurzaamheid, vorige.Duurzaamheid,
                    "Investeer in duurzame keuzes, zoals zonnepanelen.");
            }

            // KPI's die nog laag zijn
            if (nu.Bezettingsgraad < 50)
            {
                VoegToe(feedback.WatKanBeter, "De bezettingsgraad is nog laag.");
                VoegToe(feedback.Advies, "Investeer in marketing om meer gasten aan te trekken.");
            }

            if (nu.Gasttevredenheid < 50)
            {
                VoegToe(feedback.WatKanBeter, "De gasttevredenheid is nog laag.");
                VoegToe(feedback.Advies, "Verbeter de service of faciliteiten om gasten tevredener te maken.");
            }

            if (nu.Winst < 0)
            {
                VoegToe(feedback.WatKanBeter, "Het resort maakt verlies.");
                VoegToe(feedback.Advies, "Kies een goedkopere investering om je cashflow te beschermen.");
            }

            if (nu.Duurzaamheid < 50)
            {
                VoegToe(feedback.WatKanBeter, "De duurzaamheidsscore is nog laag.");
                VoegToe(feedback.Advies, "Investeer in duurzame keuzes, zoals zonnepanelen.");
            }

            // Beslissingen met een duidelijk gevolg
            foreach (var beslissing in nu.Beslissingen)
            {
                string zin = beslissing.Beslissing + " zorgde voor " + beslissing.Effect + ".";

                if (beslissing.Positief)
                {
                    VoegToe(feedback.WatGingGoed, zin);
                }
                else
                {
                    VoegToe(feedback.WatKanBeter, zin);
                }
            }

            // Nooit een leeg blok op het scherm
            if (feedback.WatGingGoed.Count == 0)
            {
                feedback.WatGingGoed.Add("Deze ronde zijn er nog geen duidelijke verbeteringen.");
            }

            if (feedback.WatKanBeter.Count == 0)
            {
                feedback.WatKanBeter.Add("Geen grote problemen deze ronde, goed bezig!");
            }

            if (feedback.Advies.Count == 0)
            {
                feedback.Advies.Add("Houd je huidige strategie vast en blijf letten op je budget.");
            }
        }

        private static void VergelijkKpi(Feedback feedback, string naam, int nu, int vorige, string advies)
        {
            if (nu > vorige)
            {
                VoegToe(feedback.WatGingGoed, naam + " is gestegen.");
            }
            else if (nu < vorige)
            {
                VoegToe(feedback.WatKanBeter, naam + " is gedaald.");
                VoegToe(feedback.Advies, advies);
            }
        }

        // Zelfde zin niet twee keer toevoegen
        private static void VoegToe(List<string> lijst, string zin)
        {
            if (!lijst.Contains(zin))
            {
                lijst.Add(zin);
            }
        }
    }
}