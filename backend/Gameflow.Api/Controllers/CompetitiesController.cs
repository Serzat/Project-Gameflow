
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
            // Check the request
            if (verzoek == null)
            {
                return BadRequest("Ongeldig verzoek.");
            }

            // Validate the competition name
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

            // Check the number of teams
            if (verzoek.Teams == null ||
                verzoek.Teams.Count != verzoek.AantalTeams)
            {
                return BadRequest(
                    "Het aantal ingevoerde teams klopt niet.");
            }

            // Every competition must have 4 rounds
            if (verzoek.Rondes == null ||
                verzoek.Rondes.Count != 4)
            {
                return BadRequest(
                    "Er moeten vier rondes worden ingesteld.");
            }

            // Check if a starting profile mode has been selected
            if (string.IsNullOrWhiteSpace(
                SpelGegevens.GekozenModus))
            {
                return BadRequest(
                    "Kies eerst een startprofielmodus.");
            }

            // Check if all starting profiles are ready
            string[] profielen = { "A", "B", "C", "D" };

            foreach (string code in profielen)
            {
                var profiel = SpelGegevens.Startprofielen
                    .FirstOrDefault(p => p.Code == code);

                if (profiel == null || !profiel.IsIngesteld)
                {
                    return BadRequest(
                        "De startprofielen zijn nog niet ingesteld.");
                }
            }

            // Validate team names and selected profiles
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

            // Create the competition with the selected mode
            var competitie = new Competitie
            {
                Id = competitieId,
                Naam = verzoek.Naam,
                AantalTeams = verzoek.AantalTeams,
                Modus = SpelGegevens.GekozenModus!,
                IsGestart = false
            };

            SpelGegevens.Competities.Add(competitie);

            // Create teams with their selected starting values
            foreach (var invoer in verzoek.Teams)
            {
                // Generate a new team ID
                int teamId = SpelGegevens.Teams.Count == 0
                    ? 1
                    : SpelGegevens.Teams.Max(t => t.Id) + 1;

                // Find the selected profile
                var profiel = SpelGegevens.Startprofielen
                    .First(p => p.Code == invoer.Profiel);

                // Copy the starting values for this team
                var startwaarden = new Startprofiel
                {
                    Code = profiel.Code,
                    Locatie = profiel.Locatie,
                    Startbudget = profiel.Startbudget,
                    AantalKamers = profiel.AantalKamers,
                    AantalMedewerkers = profiel.AantalMedewerkers,
                    Bezettingsgraad = profiel.Bezettingsgraad,
                    Gasttevredenheid = profiel.Gasttevredenheid,
                    Duurzaamheidsscore = profiel.Duurzaamheidsscore,
                    IsIngesteld = true
                };

                // Create the team
                var team = new Team
                {
                    Id = teamId,
                    Naam = invoer.Naam,
                    Profiel = invoer.Profiel,
                    CompetitieId = competitieId,
                    Startwaarden = startwaarden
                };

                SpelGegevens.Teams.Add(team);
            }

            // Create four rounds with their selected durations
            for (int i = 0; i < verzoek.Rondes.Count; i++)
            {
                // Generate a new round ID
                int rondeId = SpelGegevens.Rondes.Count == 0
                    ? 1
                    : SpelGegevens.Rondes.Max(r => r.Id) + 1;

                // Create the round
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

        // Get all rounds of a competition
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

        // Start a round manually
        [HttpPost("{competitieId}/rondes/{nummer}/start")]
        public IActionResult StartRonde(
            int competitieId, int nummer)
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

            // Prevent starting the same round twice
            if (ronde.GestartOpUtc != null)
            {
                return BadRequest(
                    "Deze ronde is al gestart.");
            }

            // Check if the previous round has finished
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

                // Calculate the previous round's end time
                DateTime vorigeEindtijd =
                    vorigeRonde.GestartOpUtc.Value
                        .AddMinutes(vorigeRonde.DuurMinuten);

                if (DateTime.UtcNow < vorigeEindtijd)
                {
                    return BadRequest(
                        "De vorige ronde is nog bezig.");
                }
            }

            // Check that teams have starting values
            if (nummer == 1)
            {
                var teams = SpelGegevens.Teams
                    .Where(t => t.CompetitieId == competitieId)
                    .ToList();

                if (teams.Count != competitie.AantalTeams ||
                    teams.Any(t => t.Startwaarden == null))
                {
                    return BadRequest(
                        "Niet alle teams hebben startwaarden.");
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

        // Get the status of a round
        [HttpGet("{competitieId}/rondes/{nummer}/status")]
        public IActionResult GetRondeStatus(
            int competitieId, int nummer)
        {
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

            // Check if the round has started
            if (ronde.GestartOpUtc == null)
            {
                return Ok(new
                {
                    ronde.Nummer,
                    Status = "Nog niet gestart"
                });
            }

            // Calculate the round's end time
            DateTime eindtijd = ronde.GestartOpUtc.Value
                .AddMinutes(ronde.DuurMinuten);

            string status = "Actief";

            // Automatically consider the round finished
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
