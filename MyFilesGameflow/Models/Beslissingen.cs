namespace MyFilesGameflow.Models;

public class Beslissingen {
    public int Id { get; set; }
    
    public int TeamNummer { get; set; }
    public int RondeNummer { get; set; }
    
    public decimal Kamerprijs { get; set; }
    public decimal MarketingBudget  { get; set; }
    public string MarketingFocus { get; set; }
    public int AantalMedewerkers { get; set; }
    public int TypeInvestering { get; set; }
    public decimal InvesteringKosten { get; set; }
    public string DuurzaamheidMaatregel { get; set; }
    public decimal DuurzaamheidKosten { get; set; }
    public string OnderhoudsNiveau { get; set; }
}