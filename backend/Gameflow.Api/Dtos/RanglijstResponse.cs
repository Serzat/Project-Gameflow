namespace Gameflow.Api.Dtos;

// Alles wat de ranglijstpagina nodig heeft
public record RanglijstResponse(int Ronde, int AantalRondes, List<RanglijstTeam> Teams);

// Eén regel in de ranglijst (zelfde namen als in ranglijst.jsx)
public record RanglijstTeam(
    string Naam,
    int Score,
    int Bezetting,
    int Gast,
    int Duurzaam,
    int Trend,      // verschil in score met de vorige ronde
    bool Eigen);    // is dit het team van de ingelogde speler?
