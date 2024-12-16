using System.ComponentModel.DataAnnotations;

namespace backendtest.Dtos.UserDto;

public class CreateUserDto
{
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty; 
}