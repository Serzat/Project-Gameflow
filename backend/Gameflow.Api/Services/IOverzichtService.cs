using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IOverzichtService
{
    // Het overzicht van één team. Null als het resorttype niet bestaat
    Task<OverzichtResponse?> GetOverzichtAsync(string resortType, string speler, CancellationToken ct);
}