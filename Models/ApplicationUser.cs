using Microsoft.AspNetCore.Identity;

namespace HelpDeskSystem.Models;

public class ApplicationUser: IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
