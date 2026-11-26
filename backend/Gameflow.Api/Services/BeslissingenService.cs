using Microsoft.EntityFrameworkCore;
using Gameflow.Api.Data;
using Gameflow.Api.Models;
using Gameflow.Api.Dtos;

namespace Gameflow.Api.Services;   

public class BeslissingenService {
    private readonly GameflowDb _context;

    public BeslissingenService(GameflowDb context) {
        _context = context;
    }

    public async Task PostData(BeslissingInputDto dto) {
        var beslissingen = await _context.beslissingen.FirstOrDefaultAsync(t => t.TeamNummer == dto.TeamNummer
        && t.RondeNummer == dto.RondeNummer);

        if (beslissingen == null) {
            Beslissingen b = new Beslissingen() {
                TeamNummer = dto.TeamNummer,
                RondeNummer = dto.RondeNummer,
                Kamerprijs = dto.Kamerprijs,
                MarketingBudget = dto.MarketingBudget,
                MarketingFocus = dto.MarketingFocus,
                AantalMedewerkers = dto.AantalMedewerkers,
                TypeInvestering = dto.TypeInvestering,
                InvesteringKosten = dto.InvesteringKosten,
                DuurzaamheidMaatregel = dto.DuurzaamheidMaatregel,
                DuurzaamheidKosten = dto.DuurzaamheidKosten,
                OnderhoudsNiveau = dto.OnderhoudsNiveau
            };
            
            _context.beslissingen.Add(b);
        }
        else {
            beslissingen.Kamerprijs = dto.Kamerprijs;
            beslissingen.MarketingBudget = dto.MarketingBudget;
            beslissingen.MarketingFocus = dto.MarketingFocus;
            beslissingen.AantalMedewerkers = dto.AantalMedewerkers;
            beslissingen.TypeInvestering = dto.TypeInvestering;
            beslissingen.InvesteringKosten = dto.InvesteringKosten;
            beslissingen.DuurzaamheidMaatregel = dto.DuurzaamheidMaatregel;
            beslissingen.DuurzaamheidKosten = dto.DuurzaamheidKosten;
            beslissingen.OnderhoudsNiveau = dto.OnderhoudsNiveau;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteData(int id) {
        var record = await _context.beslissingen.FindAsync(id);

        if (record == null) {
            return false;
        }

        _context.beslissingen.Remove(record);
        await _context.SaveChangesAsync();
        return true;
    }
}