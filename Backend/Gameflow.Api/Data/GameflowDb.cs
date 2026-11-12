using Gameflow.Api.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Data;

public class GameflowDb(DbContextOptions<GameflowDb> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}