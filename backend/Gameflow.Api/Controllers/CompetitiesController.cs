
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompetitiesController : ControllerBase
    {
        // Gezamenlijke lijst met competities
        private static List<Competitie> competities = SpelGegevens.Competities;

        // Alle competities ophalen
        [HttpGet]
        public IActionResult GetCompetities()
        {
            return Ok(competities);
        }

        // Een nieuwe competitie aanmaken
        [HttpPost]
        public IActionResult MaakCompetitie([FromBody] Competitie nieuweCompetitie)
        {
            // Controleren of de naam is ingevuld
            if (string.IsNullOrWhiteSpace(nieuweCompetitie.Naam))
            {
                return BadRequest("De competitienaam is verplicht.");
            }

            // Een competitie moet minimaal 2 en maximaal 4 teams hebben
            if (nieuweCompetitie.AantalTeams < 2 ||
                nieuweCompetitie.AantalTeams > 4)
            {
                return BadRequest("Het aantal teams moet tussen 2 en 4 liggen.");
            }

            // Een uniek ID toekennen
            nieuweCompetitie.Id = competities.Count == 0
                ? 1
                : competities.Max(c => c.Id) + 1;

            nieuweCompetitie.IsGestart = false;

            // Competitie toevoegen aan de lijst
            competities.Add(nieuweCompetitie);

            return Ok(nieuweCompetitie);
        }
    }
}
