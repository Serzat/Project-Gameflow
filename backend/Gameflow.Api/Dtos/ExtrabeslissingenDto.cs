namespace Gameflow.Api.Dtos;

public class ExtrabeslissingenDto {
    public int TeamNummer { get; set; }
    public int RondeNummer { get; set; }
    
    public string? DoelGroep { get; set; }
    public string? MarketingKanaal { get; set; }
    public string? PersoneelTraining { get; set; }
    public int? Korting { get; set; }
}