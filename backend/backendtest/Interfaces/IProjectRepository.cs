using backendtest.Dtos.ProjectDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IProjectRepository
{
    Task<Project> CreateProjectAsync(CreateProjectDto createProjectDto);
    
}