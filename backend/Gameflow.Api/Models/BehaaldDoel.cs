namespace Gameflow.Api.Models;

// Onthoudt dat een team (resorttype) een doel heeft gehaald, en in welke ronde
public class BehaaldDoel
{
    public int Id { get; set; }
    public string ResortType { get; set; } = "";
    public int DoelId { get; set; }
    public Doel Doel { get; set; } = null!;
    public int RondeNummer { get; set; }
}