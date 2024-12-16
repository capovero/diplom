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
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var resault = await _projectRepository.CreateProjectAsync(dto);

        return Ok("Проект создан");
    }
}