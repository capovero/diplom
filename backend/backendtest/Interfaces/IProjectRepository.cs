using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(CreateProjectDto createProjectDto, string userId, bool isTesting); 
    
    Task<List<Project>> GetProjectsByIdAsync(string userId); 
    Task<List<Project>> GetProjectsAsyncForUser();
    Task<List<ProjectResponseDto>> GetUserProjectsByStatusAsync(string userId, Status status);

    Task<List<ProjectResponseDto>> UserSearch(string title, int? categoryId = null);    
    
    Task<List<ProjectResponseDto>> GetProjectsAsyncForAdminPending();
    Task<Project> UpdateStatusProjectsForAdmin(int id, Status status);

    Task<List<ProjectResponseDto>> AdminSearch(string title, int? categoryId = null);


}