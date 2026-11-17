
namespace Gameflow.Api.Models
{
    public class StartprofielInvoer
    {
        // Values selected by the game host
        public string Locatie { get; set; } = string.Empty;

        public decimal Startbudget { get; set; }

        public int AantalKamers { get; set; }

        public int AantalMedewerkers { get; set; }

        public int Bezettingsgraad { get; set; }

        public int Gasttevredenheid { get; set; }

        public int Duurzaamheidsscore { get; set; }
    }
}
