namespace Gameflow.Api.Dtos;

public class BeslissingInputDto {
    public int TeamNummer { get; set; }
    public int RondeNummer { get; set; }
    public decimal Kamerprijs { get; set; }
    public decimal MarketingBudget { get; set; }
    public string MarketingFocus { get; set; } = string.Empty;
    public int AantalMedewerkers { get; set; }
    public string TypeInvestering { get; set; } = string.Empty;
    public decimal InvesteringKosten { get; set; }
    public string DuurzaamheidMaatregel { get; set; } = string.Empty;
    public decimal DuurzaamheidKosten { get; set; }
    public string OnderhoudsNiveau { get; set; } = string.Empty;
}