using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IRanglijstService
{
    // De ranglijst van alle resorts. eigenResort is het team van de ingelogde speler (of null)
    Task<RanglijstResponse> GetRanglijstAsync(string? eigenResort, CancellationToken ct);
}
