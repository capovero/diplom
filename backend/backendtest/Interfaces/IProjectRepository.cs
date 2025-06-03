using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(CreateProjectDto dto, Guid userId);

    Task<ProjectPaginationDto<ProjectResponseDto>> SearchProjects(
        ProjectFilterDto filter,
        int pageNumber,
        int pageSize,
        string? userId,
        bool isAdmin);

    Task<Project> GetProjectAsync(int id);
    Task<List<Project>> GetProjectsByUserAsync(Guid userId);
    Task<bool> DeleteProjectAsync(int projectId, Guid? userId, bool isAdmin);
    
    Task<bool> UpdateProjectAsync(int projectId, UpdateProjectDto dto, Guid? userId, bool isAdmin);
    Task<bool> UpdateProjectStatusAsync(int id, Status status);

}