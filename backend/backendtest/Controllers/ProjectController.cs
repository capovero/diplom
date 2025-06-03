using System.Security.Claims;
using backendtest.Dtos.ProjectDto;
using backendtest.Interfaces;
using backendtest.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;

[Route("api/projects")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProjects(
        [FromQuery] ProjectFilterDto filter,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var result = await _projectRepository.SearchProjects(
            filter, 
            pageNumber, 
            pageSize, 
            userId,
            isAdmin);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = "UserPolicy")]
    public async Task<ActionResult<ProjectResponseDto>> CreateProject([FromForm] CreateProjectDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var project = await _projectRepository.CreateProjectAsync(dto, Guid.Parse(userId));
        
        return CreatedAtAction(
            nameof(GetProject), 
            new { id = project.Id }, 
            project.ToProjectResponseDto());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
    {
        var project = await _projectRepository.GetProjectAsync(id);
        if (project == null) return NotFound();

        return Ok(project.ToProjectResponseDto());
    }

    [HttpGet("my")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> GetMyProjects()
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var projects = await _projectRepository.GetProjectsByUserAsync(userId);
        return Ok(projects.Select(p => p.ToProjectResponseDto()));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var isAdmin = User.IsInRole("Admin");
        var userId = isAdmin ? null : (Guid?)Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        
        var success = await _projectRepository.DeleteProjectAsync(id, userId, isAdmin);
        return success ? NoContent() : NotFound();
    }
    
    [HttpPut("{id}")]
    [Authorize(Policy = "UserPolicy")]
    public async Task<IActionResult> UpdateProject(int id, [FromForm] UpdateProjectDto dto)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdStr == null) return Unauthorized();

        bool isAdmin = User.IsInRole("Admin");
        Guid? userId = isAdmin ? (Guid?)null : Guid.Parse(userIdStr);

        var success = await _projectRepository.UpdateProjectAsync(id, dto, userId, isAdmin);
        if (!success)
        {
            return Forbid();
        }
        
        var updatedProject = await _projectRepository.GetProjectAsync(id);
        if (updatedProject == null) return NotFound();

        return Ok(updatedProject.ToProjectResponseDto());
    }

    [HttpPatch("{id}/status")]
    [Authorize(Policy = "AdminPolicy")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusUpdateDto dto)
    {
        var success = await _projectRepository.UpdateProjectStatusAsync(id, dto.Status);
        return success ? NoContent() : NotFound();
    }
}