using Gameflow.Api.Accounts;
using Gameflow.Api.Dtos;
using Gameflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gameflow.Api.Controllers;

[ApiController]
[Route("api/overzicht")]
[Authorize]
public class OverzichtController(IOverzichtService overzichtService, UserManager<User> userManager) : ControllerBase
{
    // GET /api/overzicht: het overzicht van het team van de ingelogde speler.
    // De spelbegeleider mag elk team bekijken met ?resortType=beach
    [HttpGet]
    public async Task<ActionResult<OverzichtResponse>> GetOverzicht([FromQuery] string? resortType, CancellationToken ct)
    {
        // Let op: hier is "User" de ingelogde gebruiker van dit verzoek (de cookie)
        var gebruiker = await userManager.GetUserAsync(User);
        if (gebruiker is null)
        {
            return Unauthorized();
        }

        var team = gebruiker.Team;

        if (!string.IsNullOrEmpty(resortType) && User.IsInRole(Roles.Spelbegeleider))
        {
            team = resortType.ToLowerInvariant();
        }

        if (string.IsNullOrEmpty(team))
        {
            return BadRequest("Je hoort nog niet bij een team. Spelbegeleider? Geef ?resortType=beach mee.");
        }

        var overzicht = await overzichtService.GetOverzichtAsync(team, gebruiker.DisplayName, ct);

        if (overzicht is null)
        {
            return NotFound("Dit resorttype bestaat niet.");
        }

        return overzicht;
    }
}