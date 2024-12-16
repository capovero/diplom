using backendtest.Data;
using backendtest.Dtos.ProjectDto;
using backendtest.Interfaces;
using backendtest.Models;
using backendtest.Services;

namespace backendtest.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;
    // private readonly JwtConverter _jwtConverter;

    public ProjectRepository(ApplicationContext context) // JwtConverter converter)
    {
        _context = context;
        // _jwtConverter = converter;
        
    }
    public async Task<Project> CreateProjectAsync(CreateProjectDto dto)
    {
        // var userId = _jwtConverter.GetCurrentUserId();
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            GoalAmount = dto.GoalAmount,
            CreatedAt = DateTime.UtcNow,
            Status = Status.Pending,
            // UserId = userId
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }
    
}