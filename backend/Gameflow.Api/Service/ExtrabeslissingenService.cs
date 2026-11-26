using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Dtos;
using Gameflow.Api.Data;
namespace Gameflow.Api.Service;

public class ExtrabeslissingenService {
    private readonly GameflowDb _context;

    public ExtrabeslissingenService(GameflowDb context) {
        _context = context;
    }

    public async Task<bool> PostData(ExtrabeslissingenDto dto) {
        var record = await _context.beslissingen.FirstOrDefaultAsync(e => e.TeamNummer == dto.TeamNummer
        && e.RondeNummer == dto.RondeNummer);

        if (record == null) {
            return false;
        }

        record.DoelGroep = (dto.DoelGroep == "Geen")? null : dto.DoelGroep;
        record.MarketingKanaal = (dto.MarketingKanaal == "Geen")? null : dto.MarketingKanaal;
        record.PersoneelTraining = (dto.PersoneelTraining == "Geen")? null : dto.PersoneelTraining;
        record.Korting = dto.Korting;
        await _context.SaveChangesAsync();

        return true;
    }
}