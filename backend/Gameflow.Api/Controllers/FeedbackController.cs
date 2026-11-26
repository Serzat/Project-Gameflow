using Gameflow.Api.Accounts;
using Gameflow.Api.Dtos;
using Gameflow.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gameflow.Api.Controllers;

[ApiController]
[Route("api/feedback")]
[Authorize]
public class FeedbackController(IFeedbackService feedbackService, UserManager<User> userManager) : ControllerBase
{
    // GET /api/feedback: feedback over de laatste ronde van het team van de ingelogde speler
    [HttpGet]
    public async Task<ActionResult<FeedbackResponse>> GetFeedback(CancellationToken ct)
    {
        var gebruiker = await userManager.GetUserAsync(User);
        if (gebruiker is null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(gebruiker.Team))
        {
            return BadRequest("Je hoort nog niet bij een team.");
        }

        var feedback = await feedbackService.GetFeedbackAsync(gebruiker.Team, ct);
        if (feedback is null)
        {
            return NotFound("Er is nog geen ronde berekend voor jouw resort.");
        }

        return feedback;
    }
}