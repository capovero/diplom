using backendtest.Dtos.UserDto;
using backendtest.Models;
using backendtest.Services;

public interface IUserRepository
{
    Task<Result<User>> RegisterAsync(CreateUserDto dto);
    Task<Result<User>> UpdateAsync(Guid id, UpdateUserDto dto);
    Task<bool> DeleteAsync(Guid id);
    Task<User?> GetProfileAsync(Guid id);
    Task<User?> GetAdminProfileAsync(Guid id);
    Task<List<User>> GetAllAsync();
    
    Task<User?> GetByName(string userName);
    
    Task<bool> DeleteByIdAsync(Guid id);
}