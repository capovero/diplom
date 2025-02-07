// backendtest/Dtos/ProjectDto/ProjectResponseDto.cs

using backendtest.Models;

namespace backendtest.Dtos.ProjectDto;

public record ProjectResponseDto(
    int Id,
    string Title,
    string Description,
    decimal GoalAmount,
    decimal CollectedAmount,
    DateTime CreatedAt,
    int CategoryId,
    Status Status,
    List<string> MediaFiles,
    double? AverageRating
);