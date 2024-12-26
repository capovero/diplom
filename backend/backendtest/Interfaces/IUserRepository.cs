using backendtest.Dtos.UserDto;
using backendtest.Models;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    Task<bool> DeleteAsync(string userId);
    Task<bool> RegisterAsync(CreateUserDto createUserDto);
    Task<bool> UpdateAsync(Guid id, UpdateUserDto updateUserDto); // Метод принимает ID и данные для обновления
    Task<User?> GetByName(string userName);
    
    // админские
  Task<bool> DeleteAdmin(Guid userId);
  Task<UserProfileDto> GetUserProfileForAdminAsync(Guid userId);
}
