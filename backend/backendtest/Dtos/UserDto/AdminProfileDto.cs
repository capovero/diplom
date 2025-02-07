using backendtest.Dtos.ProjectDto;

namespace backendtest.Dtos.UserDto;

public record AdminProfileDto(
    Guid Id,
    string UserName,
    string Email,
    string Role,
    List<ProjectResponseDto> Projects);