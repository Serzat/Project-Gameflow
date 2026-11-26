namespace Gameflow.Api.Dtos;

// Alles wat het feedbackscherm nodig heeft (zelfde namen als in feedback.jsx)
public record FeedbackResponse(
    string TeamNaam,
    int RondeNummer,
    int AantalRondes,
    bool RondeAfgerond,
    string? AfgerondOp,
    int TotaalScore,
    int Positie,
    int AantalTeams,
    int PuntenDezeRonde,
    List<KpiVerandering> Kpis,
    List<string> WatGingGoed,
    List<string> WatKanBeter,
    List<string> Advies,
    List<BeslissingEffect> Beslissingen);

// Eén KPI-kaart, bijvoorbeeld Waarde "70%" en Verschil "+5%"
public record KpiVerandering(string Naam, string Waarde, string Verschil, bool Gestegen, int BalkProcent);

// Een beslissing en het gevolg ervan
public record BeslissingEffect(string Beslissing, string Effect, bool Positief);