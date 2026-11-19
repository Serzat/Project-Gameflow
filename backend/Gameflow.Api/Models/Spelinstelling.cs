namespace Gameflow.Api.Models;

// Instellingen van het spel. Er is maar één rij (Id = 1)
public class Spelinstelling
{
    public int Id { get; set; }

    public string GekozenModus { get; set; } = "standaard";
}