using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(CreateProjectDto createProjectDto, string userId, bool isTesting); // +
    
    Task<List<Project>> GetProjectsByIdAsync(string userId);
    
    // Task<List<Project>> GetProjectsAsyncForAdmin();
    // Task<List<Project>> GetProjectsAsyncForAdminAll();
    Task<List<Project>> GetProjectsAsyncForUser();
    Task<List<Project>> GetUserProjectsByStatusAsync(Guid userId, Status newstatus);
    
    
    Task<List<Project>> GetProjectsAsyncForAdminPending();
    Task<bool> UpdateStatusProjectsForAdmin(int id, Status status);


}