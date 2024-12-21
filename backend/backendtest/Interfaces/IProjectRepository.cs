using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(CreateProjectDto createProjectDto, string userId, bool isTesting); //трэба
    
    Task<List<Project>> GetProjectsByIdAsync(string userId);//трэба
    Task<List<Project>> GetProjectsAsyncForUser();//трэба
    Task<List<ProjectResponseDto>> UserSearch(string title, int? categoryId = null);//трэба 
    Task<Project> UpdateStatusProjectsForAdmin(int id, Status status);//трэба
    Task<bool> DeleteProjectByIdAsync(int projectId, string userId);
    Task<bool> AdminDelete(int projectId);

    Task<List<ProjectResponseDto>> AdminSearch(string title, int? categoryId = null);//трэба


}