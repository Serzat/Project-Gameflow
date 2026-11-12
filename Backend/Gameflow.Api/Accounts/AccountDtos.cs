using System.ComponentModel.DataAnnotations;

namespace Gameflow.Api.Accounts;

// Wat de frontend stuurt bij registreren
public class RegisterRequest
{
    [Required]
    [StringLength(30, MinimumLength = 3)]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = "";
}

// Wat de frontend stuurt bij inloggen
public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";
}

// Wat de backend terugstuurt (zonder wachtwoord!)
public record AccountResponse(int Id, string Username, string Email);