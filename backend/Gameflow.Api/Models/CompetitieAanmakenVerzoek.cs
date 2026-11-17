
namespace Gameflow.Api.Models
{
    public class CompetitieAanmakenVerzoek
    {
        public string Naam { get; set; } = string.Empty;

        public int AantalTeams { get; set; }

        public List<TeamInvoer> Teams { get; set; } = new List<TeamInvoer>();

        public List<RondeInvoer> Rondes { get; set; } = new List<RondeInvoer>();
    }

    // Gegevens voor een nieuw team
    public class TeamInvoer
    {
        public string Naam { get; set; } = string.Empty;

        public string Profiel { get; set; } = string.Empty;
    }

    // De ingestelde duur van een ronde
    public class RondeInvoer
    {
        public int DuurMinuten { get; set; }
    }
}
