namespace backendtest.Dtos.UserDto;

public record UserResponseDto(
    Guid Id,
    string UserName,
    string Email);