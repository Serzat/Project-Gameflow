
namespace Gameflow.Api.Models
{
    public class Ronde
    {
        public int Id { get; set; }

        // Het nummer van de ronde
        public int Nummer { get; set; }

        // De duur van de ronde in minuten
        public int DuurMinuten { get; set; }

        // Het moment waarop de ronde is gestart
        public DateTime? GestartOpUtc { get; set; }

        // De competitie waarbij de ronde hoort
        public int CompetitieId { get; set; }
    }
}
