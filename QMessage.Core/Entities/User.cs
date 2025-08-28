using Microsoft.AspNetCore.Identity;

namespace QMessage.Core.Entities;

public class User : IdentityUser
{
    public string? DisplayName { get; set; }
    public string? ProfilePictureUrl { get; set; }
}