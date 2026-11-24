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

    public DbSet<RondeTrekking> RondeTrekkingen => Set<RondeTrekking>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Nodig voor de tabellen van Identity. Niet weghalen!
        base.OnModelCreating(modelBuilder);

        // ===== Startprofielen =====

        modelBuilder.Entity<ResortStartprofiel>()
            .HasIndex(p => new { p.Modus, p.ResortType })
            .IsUnique();

        modelBuilder.Entity<ResortStartprofiel>().Property(p => p.Startbudget).HasPrecision(10, 2);
        modelBuilder.Entity<ResortStartprofiel>().Property(p => p.Kamerprijs).HasPrecision(10, 2);

        // ===== Spelinstelling =====

        modelBuilder.Entity<Spelinstelling>().Property(s => s.KansGlobaal).HasPrecision(3, 2);
        modelBuilder.Entity<Spelinstelling>().Property(s => s.KansIndividueel).HasPrecision(3, 2);

        // ===== Teamstanden per ronde =====

        modelBuilder.Entity<TeamStand>()
            .HasIndex(t => new { t.ResortType, t.RondeNummer })
            .IsUnique();

        modelBuilder.Entity<TeamStand>().Property(t => t.Budget).HasPrecision(12, 2);
        modelBuilder.Entity<TeamStand>().Property(t => t.Kamerprijs).HasPrecision(10, 2);
        modelBuilder.Entity<TeamStand>().Property(t => t.Winst).HasPrecision(12, 2);

        // ===== Events en hun effecten =====

        // De titel is uniek, zo kan de seeder een event terugvinden
        modelBuilder.Entity<SpelEvent>()
            .HasIndex(e => e.Titel)
            .IsUnique();

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

        // ===== Lotingen per ronde =====

        // Per ronde maar één loting per doel ("alle" of een resorttype)
        modelBuilder.Entity<RondeTrekking>()
            .HasIndex(r => new { r.RondeNummer, r.Doel })
            .IsUnique();

        // Wordt een event uit events.json gehaald? Dan blijft de loting bestaan, zonder event
        modelBuilder.Entity<RondeTrekking>()
            .HasOne(r => r.SpelEvent)
            .WithMany()
            .HasForeignKey(r => r.SpelEventId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}