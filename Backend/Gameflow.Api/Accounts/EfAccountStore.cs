using Gameflow.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Accounts;

public class EfAccountStore(GameflowDb db, IPasswordHasher<User> passwordHasher) : IAccountStore
{
    public async Task<User?> FindByIdAsync(int id, CancellationToken ct)
    {
        return await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        var normalized = Normalize(email);
        return await db.Users.AnyAsync(u => u.Email == normalized, ct);
    }

    public async Task<User> CreateAsync(string username, string email, string password, CancellationToken ct)
    {
        var user = new User
        {
            Username = username.Trim(),
            Email = Normalize(email),
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = passwordHasher.HashPassword(user, password);

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        return user;
    }

    public async Task<User?> CheckLoginAsync(string email, string password, CancellationToken ct)
    {
        var normalized = Normalize(email);

        var user = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == normalized, ct);

        if (user is null)
        {
            return null;
        }

        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

        if (result == PasswordVerificationResult.Failed)
        {
            return null;
        }

        return user;
    }

    private static string Normalize(string email) => email.Trim().ToLowerInvariant();
}