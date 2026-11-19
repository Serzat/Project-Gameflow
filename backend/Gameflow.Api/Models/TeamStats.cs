namespace MyFilesGameflow.Models;

public class TeamStats {
    public int Id { get; set; }
    public int TeamNummer { get; set; }
    public int RondeNummer { get; set; }
    public char StartProfiel { get; set; }
    public string ResortNaam { get; set; } 
    public string Locatie { get; set; }
    public decimal HuidigBudget { get; set; }
    public int AantalKamers { get; set; }
    public int AantalMedewerkers { get; set; }
}

