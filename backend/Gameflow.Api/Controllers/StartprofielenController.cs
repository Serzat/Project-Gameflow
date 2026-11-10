
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/startprofielen")]
    public class StartprofielenController : ControllerBase
    {
        // Get the available dropdown options
        [HttpGet("opties")]
        public IActionResult GetOpties(int competitieId)
        {
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            return Ok(new
            {
                Locatie = new[] { "Eiland", "Winter", "Stad", "Cruise" },
                Startbudget = new[] { 20000m, 40000m, 60000m, 80000m },
                AantalKamers = new[] { 10, 20, 30, 40 },
                AantalMedewerkers = new[] { 4, 8, 12, 16 },
                Bezettingsgraad = new[] { 25, 40, 55, 70 },
                Gasttevredenheid = new[] { 40, 55, 70, 85 },
                Duurzaamheidsscore = new[] { 25, 45, 65, 85 }
            });
        }

        // Get all starting profiles of a competition
        [HttpGet]
        public IActionResult GetStartprofielen(int competitieId)
        {
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            var profielen = SpelGegevens.Startprofielen
                .Where(p => p.CompetitieId == competitieId)
                .OrderBy(p => p.Code)
                .ToList();

            return Ok(profielen);
        }

        // Get one starting profile
        [HttpGet("{code}")]
        public IActionResult GetStartprofiel(
            int competitieId, string code)
        {
            var profiel = SpelGegevens.Startprofielen
                .FirstOrDefault(p =>
                    p.CompetitieId == competitieId &&
                    p.Code == code.ToUpper());

            if (profiel == null)
            {
                return NotFound("Het startprofiel bestaat niet.");
            }

            return Ok(profiel);
        }

        // Update the selected starting profile
        [HttpPut("{code}")]
        public IActionResult BewerkStartprofiel(
            int competitieId,
            string code,
            [FromBody] StartprofielInvoer verzoek)
        {
            // Check if the competition exists
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            // Prevent changes after the game has started
            if (competitie.IsGestart)
            {
                return BadRequest(
                    "De competitie is al gestart.");
            }

            // Find the selected profile
            var profiel = SpelGegevens.Startprofielen
                .FirstOrDefault(p =>
                    p.CompetitieId == competitieId &&
                    p.Code == code.ToUpper());

            if (profiel == null)
            {
                return NotFound("Het startprofiel bestaat niet.");
            }

            // Check the location
            string[] locaties =
                { "Eiland", "Winter", "Stad", "Cruise" };

            if (!locaties.Contains(verzoek.Locatie))
            {
                return BadRequest("Ongeldige locatie.");
            }

            // Check the starting budget
            decimal[] budgetten =
                { 20000m, 40000m, 60000m, 80000m };

            if (!budgetten.Contains(verzoek.Startbudget))
            {
                return BadRequest("Ongeldig startbudget.");
            }

            // Check the number of rooms
            int[] kamers = { 10, 20, 30, 40 };

            if (!kamers.Contains(verzoek.AantalKamers))
            {
                return BadRequest("Ongeldig aantal kamers.");
            }

            // Check the number of employees
            int[] medewerkers = { 4, 8, 12, 16 };

            if (!medewerkers.Contains(verzoek.AantalMedewerkers))
            {
                return BadRequest(
                    "Ongeldig aantal medewerkers.");
            }

            // Check the occupancy rate
            int[] bezetting = { 25, 40, 55, 70 };

            if (!bezetting.Contains(verzoek.Bezettingsgraad))
            {
                return BadRequest("Ongeldige bezettingsgraad.");
            }

            // Check guest satisfaction
            int[] tevredenheid = { 40, 55, 70, 85 };

            if (!tevredenheid.Contains(verzoek.Gasttevredenheid))
            {
                return BadRequest(
                    "Ongeldige gasttevredenheid.");
            }

            // Check the sustainability score
            int[] duurzaamheid = { 25, 45, 65, 85 };

            if (!duurzaamheid.Contains(verzoek.Duurzaamheidsscore))
            {
                return BadRequest(
                    "Ongeldige duurzaamheidsscore.");
            }

            // Save the selected values
            profiel.Locatie = verzoek.Locatie;
            profiel.Startbudget = verzoek.Startbudget;
            profiel.AantalKamers = verzoek.AantalKamers;
            profiel.AantalMedewerkers = verzoek.AantalMedewerkers;
            profiel.Bezettingsgraad = verzoek.Bezettingsgraad;
            profiel.Gasttevredenheid = verzoek.Gasttevredenheid;
            profiel.Duurzaamheidsscore = verzoek.Duurzaamheidsscore;

            // Mark the profile as configured
            profiel.IsIngesteld = true;

            return Ok(profiel);
        }
    }
}
