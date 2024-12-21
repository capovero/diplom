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
    public async Task<Project> CreateProjectAsync(CreateProjectDto dto, string userId, bool isTesting) //создание
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
        
        if (!isTesting)// Сохраняем медиафайлы, если не тестируем

        {
            await SaveProjectMediaFiles(dto.MediaFiles, project.Id);
        }
        return project;
    }

    private async Task SaveProjectMediaFiles(List<IFormFile> mediaFiles, int projectId) //часть с медиа для создания
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        foreach (var file in mediaFiles)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var media = new Media
            {
                FilePath = fileName,
                ProjectId = projectId
            };
            await _context.Medias.AddAsync(media);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<List<Project>> GetProjectsByIdAsync(string userId)//юзер получает только свои проекты
    {
        var guidUserId = Guid.Parse(userId);
        var projects = _context.Projects.Where(p => p.UserId == guidUserId);
        return await projects.ToListAsync();
    }
    public async Task<List<Project>> GetProjectsAsyncForUser()//получение всех проектов для любой степени авторизации(юзер/аноним)
    {
        return await _context.Projects.Where(p => p.Status == Status.Active).ToListAsync();
    }
    public async Task<List<ProjectResponseDto>> GetUserProjectsByStatusAsync(string userId, Status status) //персональное получение проекта по определнному статусу
    {
        var guidUserId = Guid.Parse(userId);

        return await _context.Projects
            .Where(p => p.UserId == guidUserId && p.Status == status)
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                status = p.Status,
                CategoryId = p.CategoryId,
                GoalAmount = p.GoalAmount,
                CreatedAt = p.CreatedAt,
                MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
            })
            .ToListAsync();
    }

    public async Task<List<ProjectResponseDto>> UserSearch(string title, int? categoryId=null)
    {
        var query = _context.Projects.Where(p => p.Status == Status.Active);
        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        return await query.Select(p => new ProjectResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            status = p.Status,
            CategoryId = p.CategoryId,
            GoalAmount = p.GoalAmount,
            CreatedAt = p.CreatedAt,
            MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
        }).ToListAsync();
    }

    
    
    // МЕТОДЫ ДЛЯ АДМИНА
    public async Task<List<ProjectResponseDto>> GetProjectsAsyncForAdminPending()//метод для админа
    {
        return await _context.Projects.Where(p => p.Status==Status.Pending)
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                status = p.Status,
                CategoryId = p.CategoryId,
                GoalAmount = p.GoalAmount,
                CreatedAt = p.CreatedAt,
                MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
            })
            .ToListAsync();
    }

    public async Task<Project> UpdateStatusProjectsForAdmin(int id, Status newstatus)//метод для админа по обновлению статуса
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return null;
        project.Status = newstatus;  
        project.UpdatedAt = DateTime.UtcNow;
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return project;
    }
    public async Task<List<ProjectResponseDto>> AdminSearch(string title, int? categoryId = null) //админский поиск
    {
        var query = _context.Projects.AsQueryable(); // Все статусы

        if (!string.IsNullOrWhiteSpace(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        return await query
            .Select(p => new ProjectResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                status = p.Status,
                CategoryId = p.CategoryId,
                GoalAmount = p.GoalAmount,
                CreatedAt = p.CreatedAt,
                MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
            })
            .ToListAsync();
    }

    

}