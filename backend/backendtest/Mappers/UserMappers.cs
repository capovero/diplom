using backendtest.Dtos.ProjectDto;
using backendtest.Dtos.UserDto;
using backendtest.Models;

namespace backendtest.Mappers;

public static class UserMappers
{
    
    public static UserResponseDto ToResponseDto(this User user)
    {
        return new UserResponseDto(
            user.Id,
            user.UserName,
            user.Email
        );
    }

    
    public static UserProfileDto ToUserProfileDto(this User user)
    {
        return new UserProfileDto(
            user.Id,
            user.UserName,
            user.Email,
            user.Projects
                .Where(p => p.Status == Status.Active)
                .Select(p => p.ToProjectResponseDto()) 
                .ToList()
        );
    }

   
    public static AdminProfileDto ToAdminProfileDto(this User user)
    {
        return new AdminProfileDto(
            user.Id,
            user.UserName,
            user.Email,
            user.Role,
            user.Projects.Select(p => p.ToProjectResponseDto()).ToList() // Маппинг с медиафайлами
        );
    }
}