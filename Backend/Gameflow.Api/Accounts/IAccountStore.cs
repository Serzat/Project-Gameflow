namespace Gameflow.Api.Accounts;

public interface IAccountStore
{
    Task<User?> FindByIdAsync(int id, CancellationToken ct);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct);

    Task<User> CreateAsync(string username, string email, string password, CancellationToken ct);

    Task<User?> CheckLoginAsync(string email, string password, CancellationToken ct);
}