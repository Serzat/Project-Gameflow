using System.ComponentModel.DataAnnotations;

namespace Gameflow.Api.Accounts;

// Wat de frontend stuurt bij registreren
public class SignupRequest
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [AllowedValues("beach", "winter", "cruise", "city")]
    public string Team { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = "";
}

// Wat de backend terugstuurt over een gebruiker (zonder wachtwoord!)
public record AccountResponse(string Id, string Username, string Email, string Team, IList<string> Roles);