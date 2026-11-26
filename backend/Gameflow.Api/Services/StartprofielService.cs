using Gameflow.Api.Data;
using Gameflow.Api.Dtos;
using Gameflow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Gameflow.Api.Services;

public class StartprofielService(GameflowDb db) : IStartprofielService
{
    public async Task<string> GetGekozenModusAsync(CancellationToken ct)
    {
        var instelling = await db.Spelinstellingen
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);

        return instelling?.GekozenModus ?? "standaard";
    }

    public async Task<List<StartprofielResponse>> GetProfielenAsync(string? modus, CancellationToken ct)
    {
        modus ??= await GetGekozenModusAsync(ct);

        return await db.ResortStartprofielen
            .AsNoTracking()
            .Where(p => p.Modus == modus)
            .Select(p => NaarResponse(p))
            .ToListAsync(ct);
    }

    public async Task<StartprofielResponse?> GetProfielAsync(string resortType, CancellationToken ct)
    {
        var modus = await GetGekozenModusAsync(ct);

        return await db.ResortStartprofielen
            .AsNoTracking()
            .Where(p => p.Modus == modus && p.ResortType == resortType)
            .Select(p => NaarResponse(p))
            .FirstOrDefaultAsync(ct);
    }

    public async Task KiesModusAsync(string modus, CancellationToken ct)
    {
        // Hier géén AsNoTracking: we willen de rij juist aanpassen
        var instelling = await db.Spelinstellingen.FirstOrDefaultAsync(ct);

        if (instelling is null)
        {
            db.Spelinstellingen.Add(new Spelinstelling { GekozenModus = modus });
        }
        else
        {
            instelling.GekozenModus = modus;
        }

        await db.SaveChangesAsync(ct);
    }

    // Van entity (database) naar DTO (wat de API terugstuurt)
    private static StartprofielResponse NaarResponse(ResortStartprofiel p) => new(
        p.ResortType, p.Locatie, p.Startbudget, p.AantalKamers, p.AantalMedewerkers,
        p.Bezettingsgraad, p.Kamerprijs, p.Gasttevredenheid, p.Personeelstevredenheid,
        p.Duurzaamheidsscore, p.Marktaandeel);
}