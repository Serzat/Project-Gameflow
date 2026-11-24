using Gameflow.Api.Accounts;
using Gameflow.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Data;

public class GameflowDb(DbContextOptions<GameflowDb> options) : IdentityDbContext<User>(options)
{
    public DbSet<ResortStartprofiel> ResortStartprofielen => Set<ResortStartprofiel>();

    public DbSet<Spelinstelling> Spelinstellingen => Set<Spelinstelling>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Nodig voor de tabellen van Identity. Niet weghalen!
        base.OnModelCreating(modelBuilder);

        // Per modus mag elk resorttype maar één keer voorkomen
        modelBuilder.Entity<ResortStartprofiel>()
            .HasIndex(p => new { p.Modus, p.ResortType })
            .IsUnique();

        // Geldbedragen: tot 10 cijfers, waarvan 2 achter de komma
        modelBuilder.Entity<ResortStartprofiel>()
            .Property(p => p.Startbudget)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ResortStartprofiel>()
            .Property(p => p.Kamerprijs)
            .HasPrecision(10, 2);
    }
}