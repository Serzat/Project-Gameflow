
namespace Gameflow.Api.Models
{
    public class Competitie
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public int AantalTeams { get; set; }

        // Starting profile mode used by this competition
        public string Modus { get; set; } = string.Empty;

        public bool IsGestart { get; set; } = false;
    }
}
