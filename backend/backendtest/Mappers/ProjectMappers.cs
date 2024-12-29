using backendtest.Models;
using backendtest.Dtos.ProjectDto;
using System.Linq;

namespace backendtest.Mappers
{
    public static class ProjectMappers
    {
        public static ProjectResponseDto ToProjectResponseDto(this Project project)
        {
            return new ProjectResponseDto
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                GoalAmount = project.GoalAmount,
                CreatedAt = project.CreatedAt,
                CategoryId = project.CategoryId,
                status = project.Status,
                MediaFiles = project.MediaFiles.Select(m => m.FilePath).ToList()
            };
        }
        public static IQueryable<ProjectResponseDto> ToResponseDtoList(this IQueryable<Project> projects)
        {
            return projects.Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                GoalAmount = p.GoalAmount,
                CreatedAt = p.CreatedAt,
                CategoryId = p.CategoryId,
                status = p.Status,
                MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
            });
        }
    }
}