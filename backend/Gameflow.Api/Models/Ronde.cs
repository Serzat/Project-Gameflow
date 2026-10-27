
namespace Gameflow.Api.Models
{
    public class Ronde
    {
        public int Id { get; set; }

        public int Nummer { get; set; }

        public TimeOnly Starttijd { get; set; }

        public TimeOnly Eindtijd { get; set; }

        public int CompetitieId { get; set; }
    }
}
