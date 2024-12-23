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
    
    [HttpGet("all-active-projects")] //метод без авторизации для получения всех проектов
    public async Task<IActionResult> GetAllActiveProjects()
    {
        var result = await _projectRepository.GetProjectsAsyncForUser();
        return Ok(result);
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpPost("create")]  // создание проекта
    public async Task<ActionResult<Project>> Post(CreateProjectDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("Пользователь не аутентифицирован.");
        }

        var isTesting = false;//флаг для тестирования
        
        var project = await _projectRepository.CreateProjectAsync(dto, userId, isTesting);

        var response = new ProjectResponseDto
        {
            Id = project.Id,
            Title = project.Title,
            Description = project.Description,
            GoalAmount = project.GoalAmount,
            CreatedAt = project.CreatedAt,
            MediaFiles = project.MediaFiles.Select(m => m.FilePath).ToList()
        };

        return Ok(response);
    }
    
    [Authorize(Policy = "UserPolicy")]
    [HttpGet("personal-projects")] //получение проектов конкретного юзера
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
    
    [HttpGet("user-projects-search")]
    public async Task<IActionResult> GetSearchProjectsByStatusOrTitle([FromQuery] string? title, [FromQuery] int? categoryId)
    {
        try
        {
            var searchProject = await _projectRepository.UserSearch(title, categoryId);
            return Ok(searchProject);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"{e.Message}");
        }
    }

    [Authorize(Policy = "UserPolicy")]
    [HttpDelete("delete")]
    public async Task<bool> DeleteProject(int projectId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return await _projectRepository.DeleteProjectByIdAsync(projectId, userId);
    }

    
    
    // МЕТОДЫ ДЛЯ АДМИНА
    // [Authorize(Policy = "AdminPolicy")]

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("admin-update-status")]
    public async Task<IActionResult> UpdateStatus(int id, Status status)
    {
        var result = await _projectRepository.UpdateStatusProjectsForAdmin(id, status);
        return Ok(result);
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("admin-projects-search")]
    public async Task<IActionResult> GetSearchProjectsByStatusOrTitleForAdmin([FromQuery] string? title, [FromQuery] int? categoryId)
    {
        try
        {
            var searchProject = await _projectRepository.AdminSearch(title, categoryId);
            return Ok(searchProject);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"{e.Message}");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("Admin-delete-project")]
    public async Task<bool> DeleteProjectAdmin(int projectId)
    {        
        return await _projectRepository.AdminDelete(projectId);
    }

}