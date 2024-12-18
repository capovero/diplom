using System.Security.Claims;
using backendtest.Data;
using backendtest.Dtos.ProjectDto;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;
[Route("api/projects")]
[ApiController]

public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly ApplicationContext _context;

    public ProjectController(IProjectRepository projectRepository, ApplicationContext context)
    {
        _projectRepository = projectRepository;
        _context = context;
    }
    
     
    [Authorize]
    [HttpPost("create")]
    public async Task<ActionResult<Project>> Post(CreateProjectDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Пользователь не аутентифицирован.");
        }

        var project = await _projectRepository.CreateProjectAsync(dto, userId);

        return Ok(project);
    }
    
    
    [HttpGet("personal-projects")]
    [Authorize]
    public async Task<IActionResult> GetUserProjects()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID is not found or invalid.");
        }
        var projects = await _projectRepository.GetProjectsByIdAsync(userId);
        return Ok(projects); 
    }

}