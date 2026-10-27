
namespace Gameflow.Api.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        public string Profiel { get; set; } = string.Empty;

        public string Bedrijf { get; set; } = string.Empty;

        public int CompetitieId { get; set; }
    }
}
