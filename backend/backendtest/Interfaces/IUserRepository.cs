using backendtest.Dtos.UserDto;
using backendtest.Models;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<bool> DeleteAsync(Guid id);

    Task<bool> RegisterAsync(CreateUserDto createUserDto);
    Task<bool> UpdateAsync(Guid id, UpdateUserDto updateUserDto); // Метод принимает ID и данные для обновления
    Task<User?> LoginAsync(string UserName);
}
