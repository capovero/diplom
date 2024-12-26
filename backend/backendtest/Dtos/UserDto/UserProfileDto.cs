using backendtest.Dtos.ProjectDto;

namespace backendtest.Dtos.UserDto;

public class UserProfileDto
{
    public Guid Id { get; set; }         // Уникальный идентификатор пользователя
    public string Name { get; set; }    // Имя пользователя
    public string Email { get; set; }   // Электронная почта пользователя
    public List<ProjectResponseDto> Projects { get; set; } // Список проектов пользователя
}