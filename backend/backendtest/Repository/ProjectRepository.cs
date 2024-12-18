using System.Security.Claims;
using backendtest.Data;
using backendtest.Dtos.ProjectDto;
using backendtest.Interfaces;
using backendtest.Models;
using backendtest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;
    

    public ProjectRepository(ApplicationContext context)
    {
        _context = context;

    }
    public async Task<Project> CreateProjectAsync(CreateProjectDto dto, string userId)
    {
        var guidUserId = Guid.Parse(userId);
        
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            GoalAmount = dto.GoalAmount,
            CreatedAt = DateTime.UtcNow,
            Status = Status.Pending,
            UserId = guidUserId
        };
        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<List<Project>> GetProjectsByIdAsync(string userId)
    {
        var guidUserId = Guid.Parse(userId);
        var projects = _context.Projects.Where(p => p.UserId == guidUserId);
        return await projects.ToListAsync();
    }
    public async Task<List<Project>> GetProjectsAsyncForUser()//метод для юзера
    {
        return await _context.Projects.Where(p => p.Status == Status.Active).ToListAsync();
    }
    public async Task<List<Project>> GetUserProjectsByStatusAsync(Guid userId, Status status)
    {
        return await _context.Projects
            .Where(p => p.UserId == userId && p.Status == status)
            .ToListAsync();
    }
    
    
    // МЕТОДЫ ДЛЯ АДМИНА
    public async Task<List<Project>> GetProjectsAsyncForAdminPending()//метод для админа
    {
        return await _context.Projects.Where(p => p.Status==Status.Pending).ToListAsync();
    }

    public async Task<bool> UpdateStatusProjectsForAdmin(int id, Status newstatus)//метод для админа по обновлению статуса
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }
        project.Status = newstatus; 
        project.UpdatedAt = DateTime.UtcNow;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return true;
    }
    

}