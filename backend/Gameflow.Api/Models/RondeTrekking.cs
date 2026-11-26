namespace Gameflow.Api.Models;

// De uitslag van een loting: welk event er in een ronde gebeurde (of niet)
public class RondeTrekking
{
    public int Id { get; set; }

    public int RondeNummer { get; set; }

    // "alle" voor de globale loting, of een resorttype voor de loting van één team
    public required string Doel { get; set; }

    // Het getrokken event, of null als er deze ronde niks gebeurde
    public int? SpelEventId { get; set; }

    public SpelEvent? SpelEvent { get; set; }
}