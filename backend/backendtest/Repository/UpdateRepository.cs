using backendtest.Data;
using backendtest.Dtos.UpdateDto;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class UpdateRepository : IUpdateRepository
{
    private readonly ApplicationContext _context;

    public UpdateRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<UpdateResponseDto?> CreateUpdateAsync(UpdateDto updateDto, string userId)
    {
        var project = await _context.Projects.FindAsync(updateDto.ProjectId);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found.");
        }

        if (!Guid.TryParse(userId, out var userGuid) || userGuid != project.UserId)
        {
            throw new UnauthorizedAccessException("User is not authorized to update this project.");
        }

        var newUpdate = new Update
        {
            Content = updateDto.Content,
            CreatedAt = DateTime.UtcNow,
            ProjectId = updateDto.ProjectId
        };
        
        _context.Updates.Add(newUpdate);
        await _context.SaveChangesAsync();

        // Подготовить DTO для ответа
        var responseDto = new UpdateResponseDto
        {
            Id = newUpdate.Id,
            Content = newUpdate.Content,
            CreatedAt = newUpdate.CreatedAt
        };

        return responseDto;
    }

    
    public async Task<List<UpdateResponseDto>> GetUpdatesByProjectIdAsync(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            throw new ArgumentException("Project not found.");

        var updates = await _context.Updates
            .Where(u => u.ProjectId == projectId)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UpdateResponseDto
            {
                Id = u.Id,
                Content = u.Content,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync();

        return updates;
    }


    public async Task<bool> DeleteUpdateAsync(int updateId, string userId)
    {
        var update = await _context.Updates.FindAsync(updateId);
        if (update == null) return false;

        var project = await _context.Projects.FindAsync(update.ProjectId);
        if (project == null || Guid.Parse(userId) != project.UserId)
            throw new UnauthorizedAccessException("You do not have permission to delete this update.");

        _context.Updates.Remove(update);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<UpdateResponseDto> UpdateUpdateAsync(int updateId, UpdateDto updateDto, string userId)
    {
        var update = await _context.Updates.FindAsync(updateId);
        if (update == null) throw new ArgumentException("Update not found.");

        var project = await _context.Projects.FindAsync(update.ProjectId);
        if (project == null || Guid.Parse(userId) != project.UserId)
            throw new UnauthorizedAccessException("You do not have permission to edit this update.");

        update.Content = updateDto.Content;
        await _context.SaveChangesAsync();

        return new UpdateResponseDto
        {
            Id = update.Id,
            Content = update.Content,
            CreatedAt = update.CreatedAt
        };
    }

    public async Task<UpdateResponseDto?> CreateUpdateForAdminAsync(UpdateDto updateDto)
    {
        var project = await _context.Projects.FindAsync(updateDto.ProjectId);
        if (project == null)
        {
            throw new KeyNotFoundException("Project not found.");
        }

        var newUpdate = new Update
        {
            Content = updateDto.Content,
            CreatedAt = DateTime.UtcNow,
            ProjectId = updateDto.ProjectId
        };

        _context.Updates.Add(newUpdate);
        await _context.SaveChangesAsync();

        var responseDto = new UpdateResponseDto
        {
            Id = newUpdate.Id,
            Content = newUpdate.Content,
            CreatedAt = newUpdate.CreatedAt
        };

        return responseDto;
    }
    public async Task<bool> DeleteUpdateForAdminAsync(int updateId)
    {
        var update = await _context.Updates.FindAsync(updateId);
        if (update == null) return false;

        var project = await _context.Projects.FindAsync(update.ProjectId);
        if (project == null)
            throw new UnauthorizedAccessException("You do not have permission to delete this update.");

        _context.Updates.Remove(update);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<UpdateResponseDto> UpdateUpdateForAdminAsync(int updateId, UpdateDto updateDto)
    {
        var update = await _context.Updates.FindAsync(updateId);
        if (update == null) throw new ArgumentException("Update not found.");

        var project = await _context.Projects.FindAsync(update.ProjectId);
        if (project == null)
            throw new UnauthorizedAccessException("You do not have permission to edit this update.");

        update.Content = updateDto.Content;
        await _context.SaveChangesAsync();

        return new UpdateResponseDto
        {
            Id = update.Id,
            Content = update.Content,
            CreatedAt = update.CreatedAt
        };
    }


}