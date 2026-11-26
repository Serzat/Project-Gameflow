using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IEventService
{
    // Alle events die een team tot en met deze ronde heeft gehad (nieuwste eerst)
    Task<List<EventResponse>> GetEventsVoorTeamAsync(string resortType, int huidigeRonde, CancellationToken ct);

    // Het totale effect van de events van één ronde op één team, per KPI.
    // Bedoeld voor het doorrekenen van de rondes.
    Task<Dictionary<string, decimal>> GetEffectenVoorRondeAsync(string resortType, int ronde, CancellationToken ct);
}