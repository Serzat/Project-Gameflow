using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IStartprofielService
{
    // Welke modus heeft de spelbegeleider gekozen?
    Task<string> GetGekozenModusAsync(CancellationToken ct);

    // Alle vier profielen van een modus. Geen modus meegegeven? Dan die van de gekozen modus
    Task<List<StartprofielResponse>> GetProfielenAsync(string? modus, CancellationToken ct);

    // Het profiel van één resorttype, in de gekozen modus
    Task<StartprofielResponse?> GetProfielAsync(string resortType, CancellationToken ct);

    // De spelbegeleider kiest een modus
    Task KiesModusAsync(string modus, CancellationToken ct);
}