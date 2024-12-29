using backendtest.Dtos.UserDto;
using backendtest.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using backendtest.HashPassword;

namespace backendtest.Mappers;

public static class UserMappers
{
    public static UserDto ToUserDto(this User usermodel)
    {
        return new UserDto
        {
            Id = usermodel.Id,
            UserName = usermodel.UserName,
            Email = usermodel.Email
        };
    }

    public static CreateUserDto ToCreateUserDto(this CreateUserDto usermodel)
    {
        return new CreateUserDto
        {
            UserName = usermodel.UserName,
            Email = usermodel.Email,
            Password = usermodel.Password  
        };
    }
    
    public static void ToUpdateUserDto(this User user, UpdateUserDto updateUserDto)
    {
        user.UserName = updateUserDto.UserName;
        user.Email = updateUserDto.Email;
    }

    public static UserProfileDto ToUserProfileDto(this User user)
    {
        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Projects = user.Projects.Where(p => p.Status == Status.Active).Select(p => p.ToProjectResponseDto())
                .ToList()
        };
    }

    public static UserProfileDto AdminToUserProfileDto(this User user)
    {
        return new UserProfileDto
        {
            Id = user.Id,
            Name = user.UserName,
            Email = user.Email,
            Projects = user.Projects.Select(p => p.ToProjectResponseDto())
                .ToList()
        };
    }
    

}