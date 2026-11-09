using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    // Alleen om te testen: maakt een competitie met teams en resultaten
    [ApiController]
    [Route("api/testdata")]
    public class TestdataController : ControllerBase
    {
        [HttpPost("feedback")]
        public IActionResult MaakFeedbackTestdata()
        {
            int competitieId = SpelGegevens.Competities.Count == 0
                ? 1
                : SpelGegevens.Competities.Max(c => c.Id) + 1;

            SpelGegevens.Competities.Add(new Competitie
            {
                Id = competitieId,
                Naam = "Testcompetitie",
                AantalTeams = 3,
                IsGestart = true
            });

            // Drie teams
            var teamIds = new List<int>();
            string[] namen = { "Team 1", "Team 2", "Team 3" };
            string[] profielen = { "A", "B", "C" };

            for (int i = 0; i < namen.Length; i++)
            {
                int teamId = SpelGegevens.Teams.Count == 0
                    ? 1
                    : SpelGegevens.Teams.Max(t => t.Id) + 1;

                SpelGegevens.Teams.Add(new Team
                {
                    Id = teamId,
                    Naam = namen[i],
                    Profiel = profielen[i],
                    CompetitieId = competitieId
                });

                teamIds.Add(teamId);
            }

            // Vier rondes, de eerste twee zijn al gespeeld
            for (int nummer = 1; nummer <= 4; nummer++)
            {
                int rondeId = SpelGegevens.Rondes.Count == 0
                    ? 1
                    : SpelGegevens.Rondes.Max(r => r.Id) + 1;

                DateTime? gestartOp = null;

                if (nummer == 1)
                {
                    gestartOp = DateTime.UtcNow.AddHours(-2);
                }
                else if (nummer == 2)
                {
                    gestartOp = DateTime.UtcNow.AddHours(-1);
                }

                SpelGegevens.Rondes.Add(new Ronde
                {
                    Id = rondeId,
                    Nummer = nummer,
                    DuurMinuten = 30,
                    GestartOpUtc = gestartOp,
                    CompetitieId = competitieId
                });
            }

            // Resultaten van ronde 1 en 2
            VoegResultaatToe(competitieId, teamIds[0], 1, 40, 65, 65, 140000, 55);
            VoegResultaatToe(competitieId, teamIds[0], 2, 38, 70, 70, 120000, 60,
                new BeslissingEffect { Beslissing = "Marketing verhoogd", Effect = "meer gasten", Positief = true },
                new BeslissingEffect { Beslissing = "Spa & Wellness", Effect = "lagere cashflow", Positief = false },
                new BeslissingEffect { Beslissing = "Zonnepanelen", Effect = "hogere duurzaamheid", Positief = true });

            VoegResultaatToe(competitieId, teamIds[1], 1, 45, 72, 68, 150000, 50);
            VoegResultaatToe(competitieId, teamIds[1], 2, 40, 75, 66, 155000, 52);

            VoegResultaatToe(competitieId, teamIds[2], 1, 30, 50, 60, 100000, 40);
            VoegResultaatToe(competitieId, teamIds[2], 2, 35, 55, 62, 110000, 45);

            return Ok(new
            {
                CompetitieId = competitieId,
                TeamId = teamIds[0],
                Feedback = $"/api/competities/{competitieId}/teams/{teamIds[0]}/rondes/2/feedback"
            });
        }

        private static void VoegResultaatToe(int competitieId, int teamId, int ronde, int punten,
                                             int bezetting, int tevredenheid, int winst, int duurzaamheid,
                                             params BeslissingEffect[] beslissingen)
        {
            int id = FeedbackGegevens.RondeResultaten.Count == 0
                ? 1
                : FeedbackGegevens.RondeResultaten.Max(r => r.Id) + 1;

            FeedbackGegevens.RondeResultaten.Add(new RondeResultaat
            {
                Id = id,
                CompetitieId = competitieId,
                TeamId = teamId,
                RondeNummer = ronde,
                Punten = punten,
                Bezettingsgraad = bezetting,
                Gasttevredenheid = tevredenheid,
                Winst = winst,
                Duurzaamheid = duurzaamheid,
                Beslissingen = beslissingen.ToList()
            });
        }
    }
}