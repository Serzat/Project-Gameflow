namespace Gameflow.Api.Models;

// De startwaarden van één resorttype, in één modus (standaard of custom)
public class ResortStartprofiel
{
    public int Id { get; set; }

    public required string Modus { get; set; }        // "standaard" of "custom"

    public required string ResortType { get; set; }   // "beach", "winter", "cruise" of "city"

    public required string Locatie { get; set; }

    public decimal Startbudget { get; set; }

    public int AantalKamers { get; set; }

    public int AantalMedewerkers { get; set; }

    public int Bezettingsgraad { get; set; }          // in procenten

    public decimal Kamerprijs { get; set; }           // gemiddelde prijs per nacht

    public int Gasttevredenheid { get; set; }         // 0 - 100

    public int Personeelstevredenheid { get; set; }   // 0 - 100

    public int Duurzaamheidsscore { get; set; }       // 0 - 100

    public int Marktaandeel { get; set; }             // in procenten
}