using Microsoft.AspNetCore.Identity;

namespace Gameflow.Api.Accounts;

public class User : IdentityUser
{
    public string DisplayName { get; set; } = "";

    public string Team { get; set; } = "";

    public DateTime CreatedAt { get; set; }
}