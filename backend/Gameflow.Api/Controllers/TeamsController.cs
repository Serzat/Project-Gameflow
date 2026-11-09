
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/teams")]
    public class TeamsController : ControllerBase
    {
        // Teams van een bestaande competitie ophalen
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
    }
}
