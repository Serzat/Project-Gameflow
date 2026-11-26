using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IDoelService
{
    // Controleert welke doelen het team nu haalt, slaat nieuw behaalde doelen op
    // en geeft het level en de doelen van het huidige level terug.
    // huidigeWaarden: kpi-naam (bijv. "winst") -> huidige waarde
    Task<DoelenResponse> GetDoelenAsync(
        string resortType,
        int huidigeRonde,
        IReadOnlyDictionary<string, decimal> huidigeWaarden,
        CancellationToken ct);
}