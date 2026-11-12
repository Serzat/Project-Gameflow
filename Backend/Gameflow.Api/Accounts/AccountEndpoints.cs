namespace Gameflow.Api.Accounts;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder MapAccounts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/accounts").WithTags("Accounts");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapGet("/{id:int}", GetAccount).WithName("GetAccount");

        return app;
    }

    // POST /accounts/register
    private static async Task<IResult> Register(RegisterRequest request, IAccountStore store, CancellationToken ct)
    {
        if (await store.EmailExistsAsync(request.Email, ct))
        {
            return Results.Conflict(new { message = "Er bestaat al een account met dit e-mailadres." });
        }

        var user = await store.CreateAsync(request.Username, request.Email, request.Password, ct);

        var response = new AccountResponse(user.Id, user.Username, user.Email);

        return Results.CreatedAtRoute("GetAccount", new { id = user.Id }, response);
    }

    // POST /accounts/login
    private static async Task<IResult> Login(LoginRequest request, IAccountStore store, CancellationToken ct)
    {
        var user = await store.CheckLoginAsync(request.Email, request.Password, ct);

        if (user is null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(new AccountResponse(user.Id, user.Username, user.Email));
    }

    // GET /accounts/{id}
    private static async Task<IResult> GetAccount(int id, IAccountStore store, CancellationToken ct)
    {
        var user = await store.FindByIdAsync(id, ct);

        if (user is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(new AccountResponse(user.Id, user.Username, user.Email));
    }
}