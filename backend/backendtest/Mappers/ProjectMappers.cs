using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Mappers;

public static class ProjectMappers
{
    // Добавляем метод для проектов
    public static ProjectResponseDto ToProjectResponseDto(this Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            GoalAmount = project.GoalAmount,
            CollectedAmount = project.CollectedAmount,
            CreatedAt = project.CreatedAt,
            CategoryId = project.CategoryId,
            status = project.Status,
            MediaFiles = project.MediaFiles?.Select(m => m.FilePath).ToList() ?? new List<string>(),
            AverageRating = project.AverageRating
        };
    }
}