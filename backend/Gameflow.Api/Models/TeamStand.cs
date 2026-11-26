namespace Gameflow.Api.Models;

public class TeamStand
{
    public int Id { get; set; }

    public required string ResortType { get; set; }   // "beach", "winter", "cruise" of "city"

    public int RondeNummer { get; set; }

    public decimal Budget { get; set; }

    public int AantalKamers { get; set; }

    public int AantalMedewerkers { get; set; }

    public int Bezettingsgraad { get; set; }          // in procenten

    public decimal Kamerprijs { get; set; }

    public decimal Winst { get; set; }

    public int Gasttevredenheid { get; set; }         // 0 - 100

    public int Personeelstevredenheid { get; set; }   // 0 - 100

    public int Duurzaamheidsscore { get; set; }       // 0 - 100

    public int Marktaandeel { get; set; }             // in procenten
}