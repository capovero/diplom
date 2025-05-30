using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Mappers;

public static class ProjectMappers
{
    // Маппинг Project -> ProjectResponseDto
    public static ProjectResponseDto ToProjectResponseDto(this Project project)
    {
        return new ProjectResponseDto(
            project.Id,
            project.Title,
            project.Description,
            project.GoalAmount,
            project.CollectedAmount,
            project.CreatedAt,
            project.Category?.Name,
            project.Status,
            project.MediaFiles.Select(m => $"/uploads/{m.FilePath}").ToList(),
            project.AverageRating
        );
    }
}