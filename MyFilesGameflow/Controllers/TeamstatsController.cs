using Microsoft.AspNetCore.Mvc;
using MyFilesGameflow.Service;
using MyFilesGameflow.Dtos;
namespace MyFilesGameflow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamstatsController : ControllerBase{
    
    private readonly TeamStatsService _service;

    public TeamstatsController(TeamStatsService service) {
        _service = service;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<TeamsStatsDto>> GetTeamStats(int teamnummer,  int rondenummer) {
        var stats = await _service.getTS(teamnummer, rondenummer);

        if (stats == null) {
            return NotFound(new {messag = "Teamstats not found"});
        }
        
        return Ok(stats);
    }

    [HttpPost]
    public async Task<IActionResult> PostTeamStats(TeamsStatsInputDto dto) {
        await _service.postTS(dto);
        
        return Ok(new {message = "Stats successfully posted"});
    }
    
}