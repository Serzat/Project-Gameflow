using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Dtos;
using Gameflow.Api.Data;
using Gameflow.Api.Models;

namespace Gameflow.Api.Service;

public class TeamStatsService {
    private readonly GameflowDb _context;

    public TeamStatsService(GameflowDb context) {
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

    public async Task<bool> deleteTS(int id) {
        var stats = await _context.teamStats.FindAsync(id);

        if (stats == null) {
            return false;
        }

        _context.teamStats.Remove(stats);
        await _context.SaveChangesAsync();
        return true;
    }
}