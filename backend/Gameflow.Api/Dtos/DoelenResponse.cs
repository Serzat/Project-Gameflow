namespace Gameflow.Api.Dtos;

// De doelen van een team, zoals de frontend ze krijgt
// Level:        het huidige level van het resort (1, 2, ...)
// AllesBehaald: true als ook de laatste set doelen uit doelen.json gehaald is
public record DoelenResponse(
    int Level,
    bool AllesBehaald,
    int AantalBehaald,
    int AantalTotaal,
    List<DoelResponse> Doelen);

// Eén doel. Huidig = de huidige waarde van de kpi, zodat je voortgang kunt tonen.
public record DoelResponse(
    int Id,
    int Nummer,
    string Titel,
    string Kpi,
    decimal Doelwaarde,
    decimal Huidig,
    bool Behaald,
    int? BehaaldInRonde);