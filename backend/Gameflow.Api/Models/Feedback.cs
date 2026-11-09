namespace Gameflow.Api.Models
{
    // Alles wat het feedbackscherm nodig heeft
    public class Feedback
    {
        public string TeamNaam { get; set; } = string.Empty;

        public int RondeNummer { get; set; }

        public int AantalRondes { get; set; }

        public bool RondeAfgerond { get; set; }

        // Datum waarop de ronde eindigde, bijvoorbeeld "10-10-2026"
        public string? AfgerondOp { get; set; }

        public int TotaalScore { get; set; }

        public int Positie { get; set; }

        public int AantalTeams { get; set; }

        public int PuntenDezeRonde { get; set; }

        public List<KpiVerandering> Kpis { get; set; } = new List<KpiVerandering>();

        public List<string> WatGingGoed { get; set; } = new List<string>();

        public List<string> WatKanBeter { get; set; } = new List<string>();

        public List<string> Advies { get; set; } = new List<string>();

        public List<BeslissingEffect> Beslissingen { get; set; } = new List<BeslissingEffect>();
    }

    // Eén KPI-kaart op het feedbackscherm
    public class KpiVerandering
    {
        public string Naam { get; set; } = string.Empty;

        // Zoals het op het scherm komt, bijvoorbeeld "70%" of "€ 120.000"
        public string Waarde { get; set; } = string.Empty;

        // Bijvoorbeeld "+5%" of "-€20k"
        public string Verschil { get; set; } = string.Empty;

        public bool Gestegen { get; set; }

        // Hoe vol het balkje onder de kaart is (0 - 100)
        public int BalkProcent { get; set; }
    }
}