
namespace Gameflow.Api.Models
{
    public class Startprofiel
    {
        // Competition this profile belongs to
        public int CompetitieId { get; set; }

        // Profile A, B, C or D
        public string Code { get; set; } = string.Empty;

        // Starting values selected by the game host
        public string? Locatie { get; set; }

        public decimal? Startbudget { get; set; }

        public int? AantalKamers { get; set; }

        public int? AantalMedewerkers { get; set; }

        public int? Bezettingsgraad { get; set; }

        public int? Gasttevredenheid { get; set; }

        public int? Duurzaamheidsscore { get; set; }

        // Check whether the profile is configured
        public bool IsIngesteld { get; set; } = false;
    }
}
