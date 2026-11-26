namespace Gameflow.Api.Dtos;

// Alles wat de overzichtspagina nodig heeft
public record OverzichtResponse(
    TeamInfo Team,
    RondeInfo Ronde,
    List<KpiResponse> Kpis,
    List<EventResponse> Events,
    DoelenResponse Doelen);

public record TeamInfo(
    string Speler,
    string ResortType,
    string Locatie,
    decimal Budget,
    int AantalKamers,
    int AantalMedewerkers);

public record RondeInfo(int Huidig, int Totaal);

// Eén KPI-kaart. Vorige is null in de eerste ronde
public record KpiResponse(string Key, decimal Waarde, decimal? Vorige);

// Eén event in de nieuwsfeed, met de effecten al omgerekend voor dit team
public record EventResponse(
    int Id,
    string Soort,
    string Categorie,
    string Titel,
    string Omschrijving,
    int Ronde,
    bool Nieuw,
    bool GeenInvloed,
    List<EffectResponse> Effecten);

public record EffectResponse(string Kpi, decimal Waarde);