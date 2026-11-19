using Microsoft.AspNetCore.Mvc;
using MyFilesGameflow.Service;
using MyFilesGameflow.Dtos;
using MyFilesGameflow.Data;
namespace MyFilesGameflow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BeslissingenController : ControllerBase {
    
    private readonly BeslissingenService _service;

    public BeslissingenController(BeslissingenService service) {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> PostBeslissingen([FromBody] BeslissingInputDto dto) {
        await _service.PostData(dto);
        
        return Ok(new {message = "rij is toegevoegd aan database."});
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBeslissingen(int id) {
        var confirm = await _service.DeleteData(id);

        if (!confirm) {
            return  NotFound();
        }

        return Ok(new { message = "Rij verwijderd." });
    }
}