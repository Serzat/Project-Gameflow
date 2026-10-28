
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/teams")]
    public class TeamsController : ControllerBase
    {
        private static List<Team> teams = new List<Team>
        {
            new Team
            {
                Id = 1,
                Naam = "Team Horizon",
                Profiel = "A",
                Bedrijf = "B",
                CompetitieId = 1
            },
            new Team
            {
                Id = 2,
                Naam = "Team Nova",
                Profiel = "C",
                Bedrijf = "A",
                CompetitieId = 1
            }
        };

        [HttpGet]
        public IActionResult GetTeams(int competitieId)
        {
            var resultaat = teams
                .Where(t => t.CompetitieId == competitieId)
                .ToList();

            return Ok(resultaat);
        }

        [HttpPost]
        public IActionResult VoegTeamToe(int competitieId, [FromBody] Team nieuwTeam)
        {
            if (string.IsNullOrWhiteSpace(nieuwTeam.Naam))
            {
                return BadRequest("De teamnaam is verplicht.");
            }

            string[] opties = { "A", "B", "C", "D" };

            if (!opties.Contains(nieuwTeam.Profiel) ||
                !opties.Contains(nieuwTeam.Bedrijf))
            {
                return BadRequest("Profiel en bedrijf moeten A, B, C of D zijn.");
            }

            nieuwTeam.Id = teams.Count == 0
                ? 1
                : teams.Max(t => t.Id) + 1;

            nieuwTeam.CompetitieId = competitieId;

            teams.Add(nieuwTeam);

            return Ok(nieuwTeam);
        }
    }
}
