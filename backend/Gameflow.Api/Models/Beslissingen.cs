namespace Gameflow.Api.Models;

public class Beslissingen {
    public int Id { get; set; }
    
    public int TeamNummer { get; set; }
    public int RondeNummer { get; set; }
    
    public decimal Kamerprijs { get; set; }
    public decimal MarketingBudget  { get; set; }
    public string MarketingFocus { get; set; }
    public int AantalMedewerkers { get; set; }
    public string TypeInvestering { get; set; }
    public decimal InvesteringKosten { get; set; }
    public string DuurzaamheidMaatregel { get; set; }
    public decimal DuurzaamheidKosten { get; set; }
    public string OnderhoudsNiveau { get; set; }
    
    public string? DoelGroep { get; set; }
    public string? MarketingKanaal { get; set; }
    public string? PersoneelTraining { get; set; }
    public int? Korting { get; set; }
}
