
namespace Gameflow.Api.Models
{
    public class Competitie
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public int AantalTeams { get; set; }

        public int RondeDuurMinuten { get; set; }

        public bool IsGestart { get; set; } = false;
    }
}
