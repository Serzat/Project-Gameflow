
namespace Gameflow.Api.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        // Selected starting profile: A, B, C or D
        public string Profiel { get; set; } = string.Empty;

        public int CompetitieId { get; set; }

        // Starting values assigned to this team
        public Startprofiel? Startwaarden { get; set; }
    }
}
