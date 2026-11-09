using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;
using Gameflow.Api.Services;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/teams/{teamId}/rondes/{nummer}")]
    public class FeedbackController : ControllerBase
    {
        // Het resultaat van een ronde opslaan (later te gebruiken door de simulatie)
        [HttpPost("resultaat")]
        public IActionResult SlaResultaatOp(int competitieId, int teamId, int nummer,
                                            [FromBody] ResultaatInvoer invoer)
        {
            var fout = ControleerTeamEnRonde(competitieId, teamId, nummer, out _, out _);

            if (fout != null)
            {
                return fout;
            }

            // KPI's met een score moeten tussen 0 en 100 liggen
            if (!IsScore(invoer.Bezettingsgraad) ||
                !IsScore(invoer.Gasttevredenheid) ||
                !IsScore(invoer.Duurzaamheid))
            {
                return BadRequest("Bezettingsgraad, gasttevredenheid en duurzaamheid moeten tussen 0 en 100 liggen.");
            }

            // Een bestaand resultaat voor deze ronde vervangen
            FeedbackGegevens.RondeResultaten.RemoveAll(r => r.CompetitieId == competitieId &&
                                                            r.TeamId == teamId &&
                                                            r.RondeNummer == nummer);

            int resultaatId = FeedbackGegevens.RondeResultaten.Count == 0
                ? 1
                : FeedbackGegevens.RondeResultaten.Max(r => r.Id) + 1;

            var resultaat = new RondeResultaat
            {
                Id = resultaatId,
                CompetitieId = competitieId,
                TeamId = teamId,
                RondeNummer = nummer,
                Punten = invoer.Punten,
                Bezettingsgraad = invoer.Bezettingsgraad,
                Gasttevredenheid = invoer.Gasttevredenheid,
                Winst = invoer.Winst,
                Duurzaamheid = invoer.Duurzaamheid,
                Beslissingen = invoer.Beslissingen
            };

            FeedbackGegevens.RondeResultaten.Add(resultaat);

            return Ok(resultaat);
        }

        // De feedback van een team na een ronde ophalen
        [HttpGet("feedback")]
        public IActionResult GetFeedback(int competitieId, int teamId, int nummer)
        {
            var fout = ControleerTeamEnRonde(competitieId, teamId, nummer, out var team, out var ronde);

            if (fout != null)
            {
                return fout;
            }

            var resultaat = FeedbackGegevens.RondeResultaten
                .FirstOrDefault(r => r.CompetitieId == competitieId &&
                                     r.TeamId == teamId &&
                                     r.RondeNummer == nummer);

            if (resultaat == null)
            {
                return NotFound("Er is nog geen resultaat voor deze ronde.");
            }

            var feedback = FeedbackMaker.Maak(team!, ronde!, resultaat);

            return Ok(feedback);
        }

        // Controleert of de competitie, het team en de ronde bestaan
        private IActionResult? ControleerTeamEnRonde(int competitieId, int teamId, int nummer,
                                                     out Team? team, out Ronde? ronde)
        {
            team = null;
            ronde = null;

            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            team = SpelGegevens.Teams
                .FirstOrDefault(t => t.Id == teamId && t.CompetitieId == competitieId);

            if (team == null)
            {
                return NotFound("Het team bestaat niet in deze competitie.");
            }

            ronde = SpelGegevens.Rondes
                .FirstOrDefault(r => r.CompetitieId == competitieId && r.Nummer == nummer);

            if (ronde == null)
            {
                return NotFound("De ronde bestaat niet.");
            }

            return null;
        }

        private static bool IsScore(int waarde)
        {
            return waarde >= 0 && waarde <= 100;
        }
    }
}