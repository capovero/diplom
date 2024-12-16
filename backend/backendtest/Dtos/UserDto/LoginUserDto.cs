using System.ComponentModel.DataAnnotations;

namespace backendtest.Dtos.UserDto;
public class LoginUserDto
{
    [Required] public string UserName { get; set; } = string.Empty;  // Имя пользователя.
    [Required] public string Password { get; set; } = string.Empty;  // Пароль (нехешированный, вводимый пользователем).
}