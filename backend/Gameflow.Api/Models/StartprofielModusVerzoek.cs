using System.ComponentModel.DataAnnotations;

namespace Gameflow.Api.Models
{
    public class StartprofielModusVerzoek
    {
        [Required]
        [AllowedValues("standaard", "custom")]
        public string Modus { get; set; } = string.Empty;
    }
}