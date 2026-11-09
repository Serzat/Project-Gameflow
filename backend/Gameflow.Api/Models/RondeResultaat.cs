namespace Gameflow.Api.Models
{
    // De uitkomst van één ronde voor één team
    public class RondeResultaat
    {
        public int Id { get; set; }

        public int CompetitieId { get; set; }

        public int TeamId { get; set; }

        public int RondeNummer { get; set; }

        // Punten die het team in deze ronde heeft verdiend
        public int Punten { get; set; }

        // KPI's aan het einde van de ronde
        public int Bezettingsgraad { get; set; }   // 0 - 100 (%)

        public int Gasttevredenheid { get; set; }  // 0 - 100

        public int Winst { get; set; }             // in euro's

        public int Duurzaamheid { get; set; }      // 0 - 100

        // Beslissingen van deze ronde en wat ze deden
        public List<BeslissingEffect> Beslissingen { get; set; } = new List<BeslissingEffect>();
    }

    // Een beslissing en het gevolg ervan
    public class BeslissingEffect
    {
        public string Beslissing { get; set; } = string.Empty;

        public string Effect { get; set; } = string.Empty;

        public bool Positief { get; set; }
    }

    // Gegevens die binnenkomen als een resultaat wordt opgeslagen
    public class ResultaatInvoer
    {
        public int Punten { get; set; }

        public int Bezettingsgraad { get; set; }

        public int Gasttevredenheid { get; set; }

        public int Winst { get; set; }

        public int Duurzaamheid { get; set; }

        public List<BeslissingEffect> Beslissingen { get; set; } = new List<BeslissingEffect>();
    }
}