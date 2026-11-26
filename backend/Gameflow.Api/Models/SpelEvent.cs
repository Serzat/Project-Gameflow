namespace Gameflow.Api.Models;

// Een mogelijk event. Of en wanneer het gebeurt, wordt per ronde geloot
public class SpelEvent
{
    public int Id { get; set; }

    public required string Soort { get; set; }        // "globaal" (alle teams) of "individueel" (één team)

    public required string Categorie { get; set; }    // "weer", "markt", "gasten" of "resort"

    public required string Titel { get; set; }

    public required string Omschrijving { get; set; }

    // Hoe hard het event elk type resort raakt: 1 = volledig, 0.5 = half, 0 = geen invloed.
    // Bij een individueel event betekent 0: dit type kan het event niet krijgen.
    public decimal ImpactBeach { get; set; } = 1;

    public decimal ImpactWinter { get; set; } = 1;

    public decimal ImpactCruise { get; set; } = 1;

    public decimal ImpactCity { get; set; } = 1;

    public List<SpelEventEffect> Effecten { get; set; } = [];
}