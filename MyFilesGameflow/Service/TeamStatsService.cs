using Microsoft.EntityFrameworkCore;
using MyFilesGameflow.Dtos;
using MyFilesGameflow.Data;
using MyFilesGameflow.Models;

namespace MyFilesGameflow.Service;

public class TeamStatsService {
    private readonly ApplicationDbContext _context;

    public TeamStatsService(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<TeamsStatsDto?> getTS(int teamNummer, int  rondeNummer) {
        var stats = await _context.teamStats.FirstOrDefaultAsync(i => i.TeamNummer == teamNummer && i.RondeNummer == rondeNummer);

        if (stats == null) {
            return null;
        }

        var DtoStats = new TeamsStatsDto() {
            Id = stats.Id,
            TeamNummer = stats.TeamNummer,
            RondeNummer = stats.RondeNummer,
            StartProfiel = stats.StartProfiel,
            ResortNaam = stats.ResortNaam,
            Locatie = stats.Locatie,
            HuidigBudget = stats.HuidigBudget,
            AantalKamers = stats.AantalKamers,
            AantalMedewerkers = stats.AantalMedewerkers
        };
        
        return DtoStats;
    }

    public async Task postTS(TeamsStatsInputDto dto) {
        var stats = new TeamStats() {
            TeamNummer = dto.TeamNummer,
            RondeNummer = dto.RondeNummer,
            StartProfiel = dto.StartProfiel,
            ResortNaam = dto.ResortNaam,
            Locatie = dto.Locatie,
            HuidigBudget = dto.HuidigBudget,
            AantalKamers = dto.AantalKamers,
            AantalMedewerkers = dto.AantalMedewerkers
        };

        _context.teamStats.Add(stats);
        await _context.SaveChangesAsync();
    }
}