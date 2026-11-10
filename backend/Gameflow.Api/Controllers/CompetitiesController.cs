
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities")]
    public class CompetitiesController : ControllerBase
    {
        // Get all competitions
        [HttpGet]
        public IActionResult GetCompetities()
        {
            return Ok(SpelGegevens.Competities);
        }

        // Create a competition with teams and rounds
        [HttpPost]
        public IActionResult MaakCompetitie(
            [FromBody] CompetitieAanmakenVerzoek verzoek)
        {
            // Check if the request is valid
            if (verzoek == null)
            {
                return BadRequest("Ongeldig verzoek.");
            }

            // Check if a competition name is provided
            if (string.IsNullOrWhiteSpace(verzoek.Naam))
            {
                return BadRequest(
                    "De competitienaam is verplicht.");
            }

            // Allow between 2 and 4 teams
            if (verzoek.AantalTeams < 2 ||
                verzoek.AantalTeams > 4)
            {
                return BadRequest(
                    "Het aantal teams moet tussen 2 en 4 liggen.");
            }

            // Check if the number of teams is correct
            if (verzoek.Teams == null ||
                verzoek.Teams.Count != verzoek.AantalTeams)
            {
                return BadRequest(
                    "Het aantal ingevoerde teams klopt niet.");
            }

            // Every competition must have exactly 4 rounds
            if (verzoek.Rondes == null ||
                verzoek.Rondes.Count != 4)
            {
                return BadRequest(
                    "Er moeten vier rondes worden ingesteld.");
            }

            // Validate team names and starting profiles
            string[] profielen = { "A", "B", "C", "D" };

            foreach (var team in verzoek.Teams)
            {
                if (string.IsNullOrWhiteSpace(team.Naam))
                {
                    return BadRequest(
                        "Alle teams moeten een naam hebben.");
                }

                if (!profielen.Contains(team.Profiel))
                {
                    return BadRequest(
                        "Het profiel moet A, B, C of D zijn.");
                }
            }

            // Only allow 15, 30 or 45 minutes per round
            foreach (var ronde in verzoek.Rondes)
            {
                if (ronde.DuurMinuten != 15 &&
                    ronde.DuurMinuten != 30 &&
                    ronde.DuurMinuten != 45)
                {
                    return BadRequest(
                        "De rondeduur moet 15, 30 of 45 minuten zijn.");
                }
            }

            // Generate a new competition ID
            int competitieId = SpelGegevens.Competities.Count == 0
                ? 1
                : SpelGegevens.Competities.Max(c => c.Id) + 1;

            // Create the competition
            var competitie = new Competitie
            {
                Id = competitieId,
                Naam = verzoek.Naam,
                AantalTeams = verzoek.AantalTeams,
                IsGestart = false
            };

            SpelGegevens.Competities.Add(competitie);

            // Create the teams for this competition
            foreach (var invoer in verzoek.Teams)
            {
                int teamId = SpelGegevens.Teams.Count == 0
                    ? 1
                    : SpelGegevens.Teams.Max(t => t.Id) + 1;

                var team = new Team
                {
                    Id = teamId,
                    Naam = invoer.Naam,
                    Profiel = invoer.Profiel,
                    CompetitieId = competitieId
                };

                SpelGegevens.Teams.Add(team);
            }

            // Create four rounds with their selected durations
            for (int i = 0; i < verzoek.Rondes.Count; i++)
            {
                int rondeId = SpelGegevens.Rondes.Count == 0
                    ? 1
                    : SpelGegevens.Rondes.Max(r => r.Id) + 1;

                var ronde = new Ronde
                {
                    Id = rondeId,
                    Nummer = i + 1,
                    DuurMinuten = verzoek.Rondes[i].DuurMinuten,
                    GestartOpUtc = null,
                    CompetitieId = competitieId
                };

                SpelGegevens.Rondes.Add(ronde);
            }

            return Ok(competitie);
        }

        // Get all rounds for a competition
        [HttpGet("{competitieId}/rondes")]
        public IActionResult GetRondes(int competitieId)
        {
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound(
                    "De competitie bestaat niet.");
            }

            var rondes = SpelGegevens.Rondes
                .Where(r => r.CompetitieId == competitieId)
                .ToList();

            return Ok(rondes);
        }

        // Manually start a round
        [HttpPost("{competitieId}/rondes/{nummer}/start")]
        public IActionResult StartRonde(int competitieId, int nummer)
        {
            // Find the competition
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound(
                    "De competitie bestaat niet.");
            }

            // Find the selected round
            var ronde = SpelGegevens.Rondes
                .FirstOrDefault(r =>
                    r.CompetitieId == competitieId &&
                    r.Nummer == nummer);

            if (ronde == null)
            {
                return NotFound(
                    "De ronde bestaat niet.");
            }

            // Prevent a round from starting twice
            if (ronde.GestartOpUtc != null)
            {
                return BadRequest(
                    "Deze ronde is al gestart.");
            }

            // Make sure the previous round has finished
            if (nummer > 1)
            {
                var vorigeRonde = SpelGegevens.Rondes
                    .FirstOrDefault(r =>
                        r.CompetitieId == competitieId &&
                        r.Nummer == nummer - 1);

                if (vorigeRonde == null ||
                    vorigeRonde.GestartOpUtc == null)
                {
                    return BadRequest(
                        "Start eerst de vorige ronde.");
                }

                // Calculate when the previous round ends
                DateTime vorigeEindtijd =
                    vorigeRonde.GestartOpUtc.Value
                        .AddMinutes(vorigeRonde.DuurMinuten);

                if (DateTime.UtcNow < vorigeEindtijd)
                {
                    return BadRequest(
                        "De vorige ronde is nog bezig.");
                }
            }

            // Save the start time in UTC
            ronde.GestartOpUtc = DateTime.UtcNow;
            competitie.IsGestart = true;

            return Ok(new
            {
                ronde.Nummer,
                ronde.DuurMinuten,
                ronde.GestartOpUtc
            });
        }

        // Check whether a round is active or finished
        [HttpGet("{competitieId}/rondes/{nummer}/status")]
        public IActionResult GetRondeStatus(
            int competitieId, int nummer)
        {
            // Find the requested round
            var ronde = SpelGegevens.Rondes
                .FirstOrDefault(r =>
                    r.CompetitieId == competitieId &&
                    r.Nummer == nummer);

            if (ronde == null)
            {
                return NotFound(
                    "De ronde bestaat niet.");
            }

            // Return not started if there is no start time
            if (ronde.GestartOpUtc == null)
            {
                return Ok(new
                {
                    ronde.Nummer,
                    Status = "Nog niet gestart"
                });
            }

            // Calculate the end time using the selected duration
            DateTime eindtijd = ronde.GestartOpUtc.Value
                .AddMinutes(ronde.DuurMinuten);

            string status = "Actief";

            // Automatically consider the round finished
            // when the selected duration has passed
            if (DateTime.UtcNow >= eindtijd)
            {
                status = "Afgelopen";
            }

            return Ok(new
            {
                ronde.Nummer,
                ronde.DuurMinuten,
                Status = status,
                EindtijdUtc = eindtijd
            });
        }
    }
}
