
using Gameflow.Api.Models;

namespace Gameflow.Api.Data
{
    public static class SpelGegevens
    {
        // Alle aangemaakte competities
        public static List<Competitie> Competities = new List<Competitie>();

        // Teams die bij competities horen
        public static List<Team> Teams = new List<Team>();

        // Rondes die bij competities horen
        public static List<Ronde> Rondes = new List<Ronde>();
    }
}
