using Gameflow.Api.Dtos;

namespace Gameflow.Api.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Naam { get; set; } = string.Empty;

        // Selected resort type: beach, winter, cruise or city
        public string ResortType { get; set; } = string.Empty;

        public int CompetitieId { get; set; }

        // Starting values assigned to this team
        public StartprofielResponse? Startwaarden { get; set; }
    }
}