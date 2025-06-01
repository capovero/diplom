// backendtest/Mappers/ProjectMappers.cs

using backendtest.Dtos.ProjectDto;
using backendtest.Dtos.UserDto;
using backendtest.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace backendtest.Mappers;

public static class ProjectMappers
{
    private static IHttpContextAccessor? _httpContextAccessor;
    
    public static void Configure(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public static ProjectResponseDto ToProjectResponseDto(this Project project)
    {
        if (_httpContextAccessor?.HttpContext == null)
        {
            throw new InvalidOperationException("HttpContextAccessor is not configured");
        }

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";
        
        return new ProjectResponseDto(
            project.Id,
            project.Title,
            project.Description,
            project.GoalAmount,
            project.CollectedAmount,
            project.CreatedAt,
            project.Category?.Name,
            project.Status,
            project.MediaFiles.Select(m => $"{baseUrl}/uploads/{m.FilePath}").ToList(),
            project.AverageRating,
            new UserResponseDto(
                project.User.Id,         // 
                project.User.UserName,   // 
                project.User.Email       // 
            )
        );
    }
}