namespace Gameflow.Api.Models;

// Eén doel, bijv. "Gasttevredenheid naar 80"
public class Doel
{
    public int Id { get; set; }
    public string Modus { get; set; } = "";       // "standaard" of "custom"
    public string ResortType { get; set; } = "";  // "alle" (bij standaard) of beach/winter/cruise/city
    public int Level { get; set; }                 // 1 = doelen om level 2 te halen, 2 = doelen op level 2
    public int Volgorde { get; set; }              // 1, 2, 3... (volgorde in de lijst)
    public string Titel { get; set; } = "";
    public string Kpi { get; set; } = "";          // bijv. "gasttevredenheid"
    public decimal Doelwaarde { get; set; }        // behaald als KPI >= Doelwaarde
}