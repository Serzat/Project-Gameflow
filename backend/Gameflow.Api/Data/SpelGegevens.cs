
using Gameflow.Api.Models;

namespace Gameflow.Api.Data
{
    public static class SpelGegevens
    {
        public static List<Competitie> Competities = new List<Competitie>
        {
            new Competitie
            {
                Id = 1,
                Naam = "Resort Challenge",
                AantalTeams = 4,
                RondeDuurMinuten = 30,
                IsGestart = false
            }
        };

        public static List<Team> Teams = new List<Team>
        {
            new Team
            {
                Id = 1,
                Naam = "Team Horizon",
                Profiel = "A",
                Bedrijf = "B",
                CompetitieId = 1
            },
            new Team
            {
                Id = 2,
                Naam = "Team Nova",
                Profiel = "C",
                Bedrijf = "A",
                CompetitieId = 1
            }
        };
    }
}
