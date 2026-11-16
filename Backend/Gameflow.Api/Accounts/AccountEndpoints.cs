using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Gameflow.Api.Accounts;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccounts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Accounts");

        group.MapPost("/signup", Signup)
            .Produces<AccountResponse>(StatusCodes.Status200OK)
            .ProducesValidationProblem();

        group.MapGet("/me", Me)
            .RequireAuthorization()
            .Produces<AccountResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized);

        group.MapPost("/logout", Logout)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent);

        return app;
    }

    // POST /api/auth/signup: registreren mét gebruikersnaam en team
       private static async Task<IResult> Signup(SignupRequest request, UserManager<User> userManager)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.Username.Trim(),
            Team = request.Team,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());

            return Results.ValidationProblem(errors);
        }

        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new AccountResponse(user.Id, user.DisplayName, user.Email!, user.Team, roles));
    }

    // GET /api/auth/me: wie is er nu ingelogd?
    private static async Task<IResult> Me(ClaimsPrincipal principal, UserManager<User> userManager)
    {
        var user = await userManager.GetUserAsync(principal);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        var roles = await userManager.GetRolesAsync(user);

        return Results.Ok(new AccountResponse(user.Id, user.DisplayName, user.Email!, user.Team, roles));
    }

    // POST /api/auth/logout: uitloggen (cookie verwijderen)
    private static async Task<IResult> Logout(SignInManager<User> signInManager)
    {
        await signInManager.SignOutAsync();
        return Results.NoContent();
    }
}