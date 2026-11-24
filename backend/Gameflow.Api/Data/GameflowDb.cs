using Gameflow.Api.Accounts;
using Gameflow.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Data;

public class GameflowDb(DbContextOptions<GameflowDb> options) : IdentityDbContext<User>(options)
{
    public DbSet<ResortStartprofiel> ResortStartprofielen => Set<ResortStartprofiel>();

    public DbSet<Spelinstelling> Spelinstellingen => Set<Spelinstelling>();

    public DbSet<TeamStand> TeamStanden => Set<TeamStand>();

    public DbSet<SpelEvent> SpelEvents => Set<SpelEvent>();

    public DbSet<SpelEventEffect> SpelEventEffecten => Set<SpelEventEffect>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Nodig voor de tabellen van Identity. Niet weghalen!
        base.OnModelCreating(modelBuilder);

        // ===== Startprofielen =====

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

        // ===== Teamstanden per ronde =====

        // Per resorttype maar één stand per ronde
        modelBuilder.Entity<TeamStand>()
            .HasIndex(t => new { t.ResortType, t.RondeNummer })
            .IsUnique();

        modelBuilder.Entity<TeamStand>().Property(t => t.Budget).HasPrecision(12, 2);
        modelBuilder.Entity<TeamStand>().Property(t => t.Kamerprijs).HasPrecision(10, 2);
        modelBuilder.Entity<TeamStand>().Property(t => t.Winst).HasPrecision(12, 2);

        // ===== Events en hun effecten =====

        // Eén event heeft meerdere effecten. Event weg = effecten ook weg
        modelBuilder.Entity<SpelEvent>()
            .HasMany(e => e.Effecten)
            .WithOne(f => f.SpelEvent)
            .HasForeignKey(f => f.SpelEventId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SpelEvent>().Property(e => e.ImpactBeach).HasPrecision(3, 2);
        modelBuilder.Entity<SpelEvent>().Property(e => e.ImpactWinter).HasPrecision(3, 2);
        modelBuilder.Entity<SpelEvent>().Property(e => e.ImpactCruise).HasPrecision(3, 2);
        modelBuilder.Entity<SpelEvent>().Property(e => e.ImpactCity).HasPrecision(3, 2);

        modelBuilder.Entity<SpelEventEffect>().Property(f => f.Waarde).HasPrecision(12, 2);
    }
}