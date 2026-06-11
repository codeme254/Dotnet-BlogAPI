using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Email address or username is required")]
    public string Identifier { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;
}