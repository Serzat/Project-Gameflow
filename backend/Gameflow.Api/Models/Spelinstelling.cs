namespace Gameflow.Api.Models;

// Instellingen van het spel. Er is maar één rij
public class Spelinstelling
{
    public int Id { get; set; }

    public string GekozenModus { get; set; } = "standaard";

    // Kans per ronde op een globaal event (0 = nooit, 1 = altijd)
    public decimal KansGlobaal { get; set; } = 0.5m;

    // Kans per ronde, per team, op een individueel event
    public decimal KansIndividueel { get; set; } = 0.4m;
}