// backendtest/Dtos/ProjectDto/ProjectResponseDto.cs

using backendtest.Dtos.UserDto;
using backendtest.Models;

namespace backendtest.Dtos.ProjectDto;

public record ProjectResponseDto(
    int Id,
    string Title,
    string Description,
    decimal GoalAmount,
    decimal CollectedAmount,
    DateTime CreatedAt,
    string? CategoryName,
    Status Status,
    List<string> MediaFiles,
    double? AverageRating,
    UserResponseDto Creator // Добавить информацию о создателе
);