
using Microsoft.AspNetCore.Mvc;
using Gameflow.Api.Models;
using Gameflow.Api.Data;

namespace Gameflow.Api.Controllers
{
    [ApiController]
    [Route("api/competities/{competitieId}/teams")]
    public class TeamsController : ControllerBase
    {
        // Gezamenlijke lijst met teams
        private static List<Team> teams = SpelGegevens.Teams;

        // Teams van een competitie ophalen
        [HttpGet]
        public IActionResult GetTeams(int competitieId)
        {
            var resultaat = teams
                .Where(t => t.CompetitieId == competitieId)
                .ToList();

            return Ok(resultaat);
        }

        // Een nieuw team toevoegen
        [HttpPost]
        public IActionResult VoegTeamToe(int competitieId, [FromBody] Team nieuwTeam)
        {
            // Controleren of de teamnaam is ingevuld
            if (string.IsNullOrWhiteSpace(nieuwTeam.Naam))
            {
                return BadRequest("De teamnaam is verplicht.");
            }

            // Alleen profiel en bedrijf A, B, C of D toestaan
            string[] opties = { "A", "B", "C", "D" };

            if (!opties.Contains(nieuwTeam.Profiel) ||
                !opties.Contains(nieuwTeam.Bedrijf))
            {
                return BadRequest("Profiel en bedrijf moeten A, B, C of D zijn.");
            }

            // Een uniek ID toekennen
            nieuwTeam.Id = teams.Count == 0
                ? 1
                : teams.Max(t => t.Id) + 1;

            // Team koppelen aan de competitie
            nieuwTeam.CompetitieId = competitieId;

            teams.Add(nieuwTeam);

            return Ok(nieuwTeam);
        }

        // De gegevens van een bestaand team wijzigen
        [HttpPut("{teamId}")]
        public IActionResult WijzigTeam(int competitieId, int teamId, [FromBody] Team gewijzigdTeam)
        {
            // Het juiste team zoeken
            var team = teams.FirstOrDefault(t =>
                t.Id == teamId && t.CompetitieId == competitieId);

            if (team == null)
            {
                return NotFound("Het team is niet gevonden.");
            }

            // Controleren of de teamnaam geldig is
            if (string.IsNullOrWhiteSpace(gewijzigdTeam.Naam))
            {
                return BadRequest("De teamnaam is verplicht.");
            }

            string[] opties = { "A", "B", "C", "D" };

            if (!opties.Contains(gewijzigdTeam.Profiel) ||
                !opties.Contains(gewijzigdTeam.Bedrijf))
            {
                return BadRequest("Profiel en bedrijf moeten A, B, C of D zijn.");
            }

            // De teamgegevens aanpassen
            team.Naam = gewijzigdTeam.Naam;
            team.Profiel = gewijzigdTeam.Profiel;
            team.Bedrijf = gewijzigdTeam.Bedrijf;

            return Ok(team);
        }

        // Een team uit een competitie verwijderen
        [HttpDelete("{teamId}")]
        public IActionResult VerwijderTeam(int competitieId, int teamId)
        {
            // Het juiste team zoeken
            var team = teams.FirstOrDefault(t =>
                t.Id == teamId && t.CompetitieId == competitieId);

            if (team == null)
            {
                return NotFound("Het team is niet gevonden.");
            }

            // Team verwijderen uit de lijst
            teams.Remove(team);

            return NoContent();
        }
    }
}
