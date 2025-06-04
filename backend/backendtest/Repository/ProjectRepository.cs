using System.Security.Claims;
using backendtest.Data;
using backendtest.Dtos.ProjectDto;
using backendtest.Interfaces;
using backendtest.Mappers;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationContext _context;

    public ProjectRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateProjectAsync(CreateProjectDto dto, Guid userId)
    {
        if (dto.CategoryId == 0)
        {
            dto.CategoryId = null;
        }

        if (dto.CategoryId.HasValue && 
            !await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId))
        {
            throw new ArgumentException("Invalid Category ID");
        }
        var project = new Project
        {
            Title = dto.Title,
            Description = dto.Description,
            GoalAmount = dto.GoalAmount,
            CollectedAmount = 0,
            CreatedAt = DateTime.UtcNow,
            Status = Status.Pending,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        await _context.Projects.AddAsync(project);
        await _context.SaveChangesAsync();

        await SaveProjectMediaFiles(dto.MediaFiles, project.Id);
        return await GetProjectAsync(project.Id);
    }

    private async Task SaveProjectMediaFiles(List<IFormFile> mediaFiles, int projectId)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder);

        foreach (var file in mediaFiles)
        {
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await _context.Medias.AddAsync(new Media
            {
                FilePath = fileName,
                ProjectId = projectId
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<ProjectPaginationDto<ProjectResponseDto>> SearchProjects(
        ProjectFilterDto filter,
        int pageNumber,
        int pageSize,
        string? userId,
        bool isAdmin)
    {
        var query = _context.Projects
            .Include(p => p.User)        //
            .Include(p => p.Category)    //
            .Include(p => p.MediaFiles)
            .AsQueryable();

        
        if (!isAdmin)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var guidUserId = Guid.Parse(userId);
                
                query = query.Where(p => 
                    p.Status == Status.Active || 
                    p.UserId == guidUserId
                );
            }
            else
            {
                query = query.Where(p => p.Status == Status.Active);
            }
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Title))
        {
            var lowerTitle = filter.Title.ToLower();
            query = query.Where(p => p.Title.ToLower().Contains(lowerTitle));
        }


        if (filter.CategoryId.HasValue && filter.CategoryId.Value > 0)
        {
            query = query.Where(p => p.CategoryId == filter.CategoryId);
        }
        
        if (filter.Status.HasValue && isAdmin)
            query = query.Where(p => p.Status == filter.Status.Value);
        
        var totalRecords = await query.CountAsync();
        var projects = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ProjectPaginationDto<ProjectResponseDto>
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalRecords = totalRecords,
            Data = projects.Select(p => p.ToProjectResponseDto()).ToList()
        };
    }

    public async Task<Project> GetProjectAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.User)
            .Include(p => p.MediaFiles)
            .Include(p => p.Category) 
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<Project>> GetProjectsByUserAsync(Guid userId)
    {
        return await _context.Projects
            .Include(p => p.MediaFiles)
            .Include(p => p.Category) 
            .Where(p => p.UserId == userId)
            .Include(p => p.User)
            .AsSplitQuery() 
            .ToListAsync();
    }

    public async Task<bool> DeleteProjectAsync(int projectId, Guid? userId, bool isAdmin)
    {
        var project = await _context.Projects
            .Include(p => p.MediaFiles)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null) return false;

        if (!isAdmin && (userId == null || project.UserId != userId.Value))
            return false;

        _context.Medias.RemoveRange(project.MediaFiles);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> UpdateProjectAsync(int projectId, UpdateProjectDto dto, Guid? userId, bool isAdmin)
    {
        var project = await _context.Projects
            .Include(p => p.MediaFiles)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null) return false;
        
        if (!isAdmin && (userId == null || project.UserId != userId.Value))
        {
            return false;
        }
        
        project.Title = dto.Title;
        project.Description = dto.Description;
        project.GoalAmount = dto.GoalAmount;
        
        if (!dto.CategoryId.HasValue || dto.CategoryId == 0)
        {
            project.CategoryId = null;
        }
        else
        {
            project.CategoryId = dto.CategoryId;
        }

        project.UpdatedAt = DateTime.UtcNow;

        _context.Projects.Update(project);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProjectStatusAsync(int id, Status status)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null) return false;

        project.Status = status;
        project.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}