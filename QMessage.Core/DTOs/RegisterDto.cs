using System.ComponentModel.DataAnnotations;

namespace QMessage.Core.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = "";
    
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }
}