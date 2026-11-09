
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities")]
    public class CompetitiesController : ControllerBase
    {
        // Alle competities ophalen
        [HttpGet]
        public IActionResult GetCompetities()
        {
            return Ok(SpelGegevens.Competities);
        }

        // Een competitie met teams en rondes aanmaken
        [HttpPost]
        public IActionResult MaakCompetitie([FromBody] CompetitieAanmakenVerzoek verzoek)
        {
            // Controleren of de competitienaam is ingevuld
            if (string.IsNullOrWhiteSpace(verzoek.Naam))
            {
                return BadRequest("De competitienaam is verplicht.");
            }

            // Er mogen 2 tot 4 teams meedoen
            if (verzoek.AantalTeams < 2 || verzoek.AantalTeams > 4)
            {
                return BadRequest("Het aantal teams moet tussen 2 en 4 liggen.");
            }

            // Het aantal teams controleren
            if (verzoek.Teams == null ||
                verzoek.Teams.Count != verzoek.AantalTeams)
            {
                return BadRequest("Het aantal ingevoerde teams klopt niet.");
            }

            // Er moeten vier rondes zijn
            if (verzoek.Rondes == null || verzoek.Rondes.Count != 4)
            {
                return BadRequest("Er moeten vier rondes worden ingesteld.");
            }

            // Teamnamen en profielen controleren
            string[] profielen = { "A", "B", "C", "D" };

            foreach (var team in verzoek.Teams)
            {
                if (string.IsNullOrWhiteSpace(team.Naam))
                {
                    return BadRequest("Alle teams moeten een naam hebben.");
                }

                if (!profielen.Contains(team.Profiel))
                {
                    return BadRequest("Het profiel moet A, B, C of D zijn.");
                }
            }

            // Controleren of alle rondes een geldige duur hebben
            foreach (var ronde in verzoek.Rondes)
            {
                if (ronde.DuurMinuten <= 0)
                {
                    return BadRequest("De rondeduur moet groter zijn dan 0.");
                }
            }

            // Een nieuw competitie-ID bepalen
            int competitieId = SpelGegevens.Competities.Count == 0
                ? 1
                : SpelGegevens.Competities.Max(c => c.Id) + 1;

            var competitie = new Competitie
            {
                Id = competitieId,
                Naam = verzoek.Naam,
                AantalTeams = verzoek.AantalTeams,
                IsGestart = false
            };

            SpelGegevens.Competities.Add(competitie);

            // Teams aanmaken
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

            // Vier rondes aanmaken met de ingestelde duur
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

        // Rondes van een competitie ophalen
        [HttpGet("{competitieId}/rondes")]
        public IActionResult GetRondes(int competitieId)
        {
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            var rondes = SpelGegevens.Rondes
                .Where(r => r.CompetitieId == competitieId)
                .ToList();

            return Ok(rondes);
        }

        // Een ronde handmatig starten
        [HttpPost("{competitieId}/rondes/{nummer}/start")]
        public IActionResult StartRonde(int competitieId, int nummer)
        {
            // De competitie zoeken
            var competitie = SpelGegevens.Competities
                .FirstOrDefault(c => c.Id == competitieId);

            if (competitie == null)
            {
                return NotFound("De competitie bestaat niet.");
            }

            // De gekozen ronde zoeken
            var ronde = SpelGegevens.Rondes
                .FirstOrDefault(r => r.CompetitieId == competitieId &&
                                     r.Nummer == nummer);

            if (ronde == null)
            {
                return NotFound("De ronde bestaat niet.");
            }

            // Voorkomen dat een ronde twee keer start
            if (ronde.GestartOpUtc != null)
            {
                return BadRequest("Deze ronde is al gestart.");
            }

            // Controleren of de vorige ronde is afgelopen
            if (nummer > 1)
            {
                var vorigeRonde = SpelGegevens.Rondes
                    .FirstOrDefault(r => r.CompetitieId == competitieId &&
                                         r.Nummer == nummer - 1);

                if (vorigeRonde == null ||
                    vorigeRonde.GestartOpUtc == null)
                {
                    return BadRequest("Start eerst de vorige ronde.");
                }

                DateTime vorigeEindtijd = vorigeRonde.GestartOpUtc.Value
                    .AddMinutes(vorigeRonde.DuurMinuten);

                if (DateTime.UtcNow < vorigeEindtijd)
                {
                    return BadRequest("De vorige ronde is nog bezig.");
                }
            }

            // De starttijd opslaan
            ronde.GestartOpUtc = DateTime.UtcNow;
            competitie.IsGestart = true;

            return Ok(new
            {
                ronde.Nummer,
                ronde.DuurMinuten,
                ronde.GestartOpUtc
            });
        }

        // Controleren of een ronde bezig of afgelopen is
        [HttpGet("{competitieId}/rondes/{nummer}/status")]
        public IActionResult GetRondeStatus(int competitieId, int nummer)
        {
            var ronde = SpelGegevens.Rondes
                .FirstOrDefault(r => r.CompetitieId == competitieId &&
                                     r.Nummer == nummer);

            if (ronde == null)
            {
                return NotFound("De ronde bestaat niet.");
            }

            // Een ronde zonder starttijd is nog niet begonnen
            if (ronde.GestartOpUtc == null)
            {
                return Ok(new
                {
                    ronde.Nummer,
                    Status = "Nog niet gestart"
                });
            }

            // De eindtijd berekenen
            DateTime eindtijd = ronde.GestartOpUtc.Value
                .AddMinutes(ronde.DuurMinuten);

            string status = "Actief";

            // Na de ingestelde duur is de ronde afgelopen
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
