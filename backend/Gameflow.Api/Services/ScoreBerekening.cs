namespace Gameflow.Api.Services;

// Eén plek voor de score, zodat Feedback en Ranglijst hetzelfde rekenen.
// Afspraak: gemiddelde van bezettingsgraad, gasttevredenheid en duurzaamheid (0 - 100)
public static class ScoreBerekening
{
    public static int Bereken(int bezetting, int gasttevredenheid, int duurzaamheid) =>
        (int)Math.Round((bezetting + gasttevredenheid + duurzaamheid) / 3.0);
}