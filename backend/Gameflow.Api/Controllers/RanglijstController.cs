using Gameflow.Api.Accounts;
using Gameflow.Api.Dtos;
using Gameflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gameflow.Api.Controllers;

[ApiController]
[Route("api/ranglijst")]
[Authorize]
public class RanglijstController(IRanglijstService ranglijstService, UserManager<User> userManager) : ControllerBase
{
    // GET /api/ranglijst: alle resorts gesorteerd op score.
    // Het team van de ingelogde speler krijgt Eigen = true
    [HttpGet]
    public async Task<ActionResult<RanglijstResponse>> GetRanglijst(CancellationToken ct)
    {
        var gebruiker = await userManager.GetUserAsync(User);
        if (gebruiker is null)
        {
            return Unauthorized();
        }

        // Een spelbegeleider heeft geen team: dan is er geen "eigen" team
        string? eigenResort = string.IsNullOrEmpty(gebruiker.Team) ? null : gebruiker.Team;

        return await ranglijstService.GetRanglijstAsync(eigenResort, ct);
    }
}
