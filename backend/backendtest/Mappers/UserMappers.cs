using backendtest.Dtos.UserDto;
using backendtest.Models;
using Microsoft.AspNetCore.Http.HttpResults;

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
}