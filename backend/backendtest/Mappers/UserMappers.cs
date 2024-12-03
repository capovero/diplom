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
            Password = usermodel.Password  // Просто передаем пароль без хеширования
        };
    }

    //мой ласт маппер - потом проверить правильность
    public static void ToUpdateUserDto(this User user, UpdateUserDto updateUserDto)
    {
        user.UserName = updateUserDto.UserName;
        user.Email = updateUserDto.Email;
    }

}