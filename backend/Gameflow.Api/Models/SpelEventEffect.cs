namespace Gameflow.Api.Models;

// Eén effect van een event, bijvoorbeeld: bezettingsgraad -20
public class SpelEventEffect
{
    public int Id { get; set; }

    // Foreign key: bij welk event hoort dit effect
    public int SpelEventId { get; set; }

    public SpelEvent SpelEvent { get; set; } = null!;

    // Welk cijfer verandert, bijvoorbeeld "bezettingsgraad" of "winst"
    public required string Kpi { get; set; }

    // Hoeveel: positief is voordelig, negatief is nadelig
    public decimal Waarde { get; set; }
}