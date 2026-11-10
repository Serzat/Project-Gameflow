
using Gameflow.Api.Models;

namespace Gameflow.Api.Data
{
    public static class SpelGegevens
    {
        // All created competitions
        public static List<Competitie> Competities =
            new List<Competitie>();

        // Teams belonging to competitions
        public static List<Team> Teams =
            new List<Team>();

        // Rounds belonging to competitions
        public static List<Ronde> Rondes =
            new List<Ronde>();

        // Starting profiles belonging to competitions
        public static List<Startprofiel> Startprofielen =
            new List<Startprofiel>();
    }
}
