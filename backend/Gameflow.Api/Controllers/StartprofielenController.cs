
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/startprofielen")]
    public class StartprofielenController : ControllerBase
    {
        // Get the available profile modes
        [HttpGet("opties")]
        public IActionResult GetOpties()
        {
            return Ok(new
            {
                Modi = new[] { "Standaard", "Custom" }
            });
        }

        // Get the currently selected mode
        [HttpGet("modus")]
        public IActionResult GetModus()
        {
            return Ok(new
            {
                Modus = SpelGegevens.GekozenModus
            });
        }

        // Get all starting profiles
        [HttpGet]
        public IActionResult GetStartprofielen()
        {
            return Ok(SpelGegevens.Startprofielen);
        }

        // Get one starting profile
        [HttpGet("{code}")]
        public IActionResult GetStartprofiel(string code)
        {
            var profiel = SpelGegevens.Startprofielen
                .FirstOrDefault(p =>
                    p.Code == code.ToUpperInvariant());

            if (profiel == null)
            {
                return NotFound(
                    "Het startprofiel bestaat niet.");
            }

            return Ok(profiel);
        }

        // Select and save the starting profile mode
        [HttpPut("modus")]
        public IActionResult KiesModus(
            [FromBody] StartprofielModusVerzoek verzoek)
        {
            // Check the request
            if (verzoek == null ||
                string.IsNullOrWhiteSpace(verzoek.Modus))
            {
                return BadRequest("Kies een geldige modus.");
            }

            string modus = verzoek.Modus.Trim();

            if (modus != "Standaard" && modus != "Custom")
            {
                return BadRequest(
                    "De modus moet Standaard of Custom zijn.");
            }

            if (modus == "Standaard")
            {
                // Give every profile the same starting values
                var profielen = new List<Startprofiel>();

                string[] codes = { "A", "B", "C", "D" };

                foreach (string code in codes)
                {
                    var profiel = new Startprofiel
                    {
                        Code = code,
                        Locatie = "Neutraal",
                        Startbudget = 5000m,
                        AantalKamers = 15,
                        AantalMedewerkers = 5,
                        Bezettingsgraad = 50,
                        Gasttevredenheid = 60,
                        Duurzaamheidsscore = 50,
                        IsIngesteld = true
                    };

                    profielen.Add(profiel);
                }

                SpelGegevens.Startprofielen = profielen;
            }
            else
            {
                // Give each location different strengths and weaknesses
                SpelGegevens.Startprofielen =
                    new List<Startprofiel>
                    {
                        // Profile A: Island resort
                        new Startprofiel
                        {
                            Code = "A",
                            Locatie = "Eiland",
                            Startbudget = 3000m,
                            AantalKamers = 15,
                            AantalMedewerkers = 5,
                            Bezettingsgraad = 70,
                            Gasttevredenheid = 80,
                            Duurzaamheidsscore = 60,
                            IsIngesteld = true
                        },

                        // Profile B: Winter resort
                        new Startprofiel
                        {
                            Code = "B",
                            Locatie = "Winter",
                            Startbudget = 4000m,
                            AantalKamers = 20,
                            AantalMedewerkers = 4,
                            Bezettingsgraad = 40,
                            Gasttevredenheid = 65,
                            Duurzaamheidsscore = 75,
                            IsIngesteld = true
                        },

                        // Profile C: City resort
                        new Startprofiel
                        {
                            Code = "C",
                            Locatie = "Stad",
                            Startbudget = 6000m,
                            AantalKamers = 12,
                            AantalMedewerkers = 6,
                            Bezettingsgraad = 60,
                            Gasttevredenheid = 60,
                            Duurzaamheidsscore = 40,
                            IsIngesteld = true
                        },

                        // Profile D: Cruise resort
                        new Startprofiel
                        {
                            Code = "D",
                            Locatie = "Cruise",
                            Startbudget = 8000m,
                            AantalKamers = 8,
                            AantalMedewerkers = 3,
                            Bezettingsgraad = 30,
                            Gasttevredenheid = 45,
                            Duurzaamheidsscore = 50,
                            IsIngesteld = true
                        }
                    };
            }

            // Save the selected mode
            SpelGegevens.GekozenModus = modus;

            return Ok(new
            {
                Modus = modus,
                Profielen = SpelGegevens.Startprofielen
            });
        }
    }
}
