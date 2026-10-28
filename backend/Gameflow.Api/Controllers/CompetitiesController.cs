
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitiesController : ControllerBase
    {
        private static List<Competitie> competities = new List<Competitie>
        {
            new Competitie
            {
                Id = 1,
                Naam = "Resort Challenge"
            }
        };

        [HttpGet]
        public IActionResult GetCompetities()
        {
            return Ok(competities);
        }

        [HttpPost]
        public IActionResult MaakCompetitie([FromBody] Competitie nieuweCompetitie)
        {
            if (string.IsNullOrWhiteSpace(nieuweCompetitie.Naam))
            {
                return BadRequest("De competitienaam is verplicht.");
            }

            nieuweCompetitie.Id = competities.Max(c => c.Id) + 1;
            nieuweCompetitie.IsGestart = false;

            competities.Add(nieuweCompetitie);

            return Ok(nieuweCompetitie);
        }
    }
}
