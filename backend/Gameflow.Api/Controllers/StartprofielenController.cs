using Gameflow.Api.Accounts;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Gameflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gameflow.Api.Controllers;

[ApiController]
[Route("api/startprofielen")]
public class StartprofielenController(IStartprofielService service) : ControllerBase
{
    // GET /api/startprofielen/opties: welke modi en resorttypes er zijn
    [HttpGet("opties")]
    public IActionResult GetOpties()
    {
        return Ok(new { Modi = Spelopties.Modi, ResortTypes = Spelopties.ResortTypes });
    }

    // GET /api/startprofielen/modus: welke modus is nu gekozen?
    [HttpGet("modus")]
    public async Task<IActionResult> GetModus(CancellationToken ct)
    {
        var modus = await service.GetGekozenModusAsync(ct);
        return Ok(new { Modus = modus });
    }

    // GET /api/startprofielen of /api/startprofielen?modus=custom
    [HttpGet]
    public async Task<ActionResult<List<StartprofielResponse>>> GetStartprofielen(
        [FromQuery] string? modus, CancellationToken ct)
    {
        if (modus is not null && !Spelopties.Modi.Contains(modus))
        {
            return BadRequest("De modus moet standaard of custom zijn.");
        }

        return await service.GetProfielenAsync(modus, ct);
    }

    // GET /api/startprofielen/beach: het profiel van één resorttype
    [HttpGet("{resortType}")]
    public async Task<ActionResult<StartprofielResponse>> GetStartprofiel(string resortType, CancellationToken ct)
    {
        var profiel = await service.GetProfielAsync(resortType.ToLowerInvariant(), ct);

        if (profiel is null)
        {
            return NotFound("Dit resorttype bestaat niet.");
        }

        return profiel;
    }

    // PUT /api/startprofielen/modus: alleen de spelbegeleider mag kiezen
    [HttpPut("modus")]
    [Authorize(Roles = Roles.Spelbegeleider)]
    public async Task<IActionResult> KiesModus(StartprofielModusVerzoek verzoek, CancellationToken ct)
    {
        await service.KiesModusAsync(verzoek.Modus, ct);

        var profielen = await service.GetProfielenAsync(verzoek.Modus, ct);

        return Ok(new { Modus = verzoek.Modus, Profielen = profielen });
    }
}