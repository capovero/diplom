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
        var resault = await _projectRepository.GetProjectsAsyncForUser();
        return Ok(resault);
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

    [Authorize(Policy = "UserPolicy")]
    [HttpGet("personal-projects-by-status")] // персональный метод для просмотра своих проектов с любым статусом
    public async Task<IActionResult> GetAllProjectsByStatus([FromQuery] Status status)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        var resault = await _projectRepository.GetUserProjectsByStatusAsync(userId, status);
        return Ok(resault);
    }

    [Authorize(Policy = "UserPolicy")]
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
    
    
    // МЕТОДЫ ДЛЯ АДМИНА
    // [Authorize(Policy = "AdminPolicy")]
    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("admin-get-pending-projects")]
    public async Task<IActionResult> GetAllPendingProjects()
    {
        var pendingProjects = await _projectRepository.GetProjectsAsyncForAdminPending();
        return Ok(pendingProjects);
    }

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

}