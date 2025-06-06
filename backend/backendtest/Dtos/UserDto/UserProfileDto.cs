// backendtest/Dtos/UserDto/UserProfileDto.cs
using backendtest.Dtos.ProjectDto;

namespace backendtest.Dtos.UserDto;

public record UserProfileDto(
    Guid Id,
    string UserName,
    string Email,
    List<ProjectResponseDto> Projects
);