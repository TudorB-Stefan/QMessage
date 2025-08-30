using System.ComponentModel.DataAnnotations;

namespace QMessage.Core.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Display name is required")]
    public string? DisplayName { get; set; }
    [Required(ErrorMessage = "Email is required")]
    public string? ProfilePictureUrl { get; set; } = "/images/default-avatar.jpg";
    public string? Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string? ConfirmPassword { get; set; }
}