using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;

public interface IFeedbackService
{
    // Feedback over de laatst berekende ronde. Null als er nog geen ronde is berekend
    Task<FeedbackResponse?> GetFeedbackAsync(string resortType, CancellationToken ct);
}