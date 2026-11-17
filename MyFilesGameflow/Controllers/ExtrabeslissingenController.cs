using Microsoft.AspNetCore.Mvc;
using MyFilesGameflow.Service;
using MyFilesGameflow.Dtos;
using MyFilesGameflow.Data;
namespace MyFilesGameflow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExtrabeslissingenController : ControllerBase {
    
    private readonly ExtrabeslissingenService _service;

    public ExtrabeslissingenController(ExtrabeslissingenService service) {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostExtrabeslissingen([FromBody] ExtrabeslissingenDto dto) {
        var text = await _service.PostData(dto);

        if (!text) {
            return BadRequest(new {message = "Sla eerst beslissingen op voordat je extra beslissingen oplsaat."});
        }

        return Ok(new {message = "Extra beslissingen zijn opgeslagen."});
    }
}