namespace backendtest.Dtos.UserDto;
public class LoginUserDto
{
    public string UserName { get; set; } = string.Empty;  // Имя пользователя.
    public string Password { get; set; } = string.Empty;  // Пароль (нехешированный, вводимый пользователем).
}
