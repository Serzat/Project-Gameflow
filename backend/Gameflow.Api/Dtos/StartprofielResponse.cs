namespace Gameflow.Api.Dtos;

public record StartprofielResponse(
    string ResortType,
    string Locatie,
    decimal Startbudget,
    int AantalKamers,
    int AantalMedewerkers,
    int Bezettingsgraad,
    decimal Kamerprijs,
    int Gasttevredenheid,
    int Personeelstevredenheid,
    int Duurzaamheidsscore,
    int Marktaandeel);