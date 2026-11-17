
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/teams")]
    public class TeamsController : ControllerBase
    {
        // Get all teams from a competition
        [HttpGet]
        public IActionResult GetTeams(int competitieId)
        {
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            var teams = SpelGegevens.Teams
                .Where(t => t.CompetitieId == competitieId)
                .ToList();

            return Ok(teams);
        }

        // Get the starting profile of a team
        [HttpGet("{teamId}/startprofiel")]
        public IActionResult GetTeamStartprofiel(
            int competitieId, int teamId)
        {
            // Check if the competition exists
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            // Find the team in the selected competition
            var team = SpelGegevens.Teams
                .FirstOrDefault(t =>
                    t.Id == teamId &&
                    t.CompetitieId == competitieId);

            if (team == null)
            {
                return NotFound("Het team bestaat niet.");
            }

            // Find the starting profile assigned to the team
            var startprofiel = SpelGegevens.Startprofielen
                .FirstOrDefault(p => p.Code == team.Profiel);

            if (startprofiel == null)
            {
                return NotFound("Het startprofiel bestaat niet.");
            }

            // Check if the profile has starting values
            if (!startprofiel.IsIngesteld)
            {
                return BadRequest(
                    "Het startprofiel is nog niet ingesteld.");
            }

            // Return the team and its starting profile
            return Ok(new
            {
                TeamId = team.Id,
                TeamNaam = team.Naam,
                Startprofiel = startprofiel
            });
        }
    }
}
