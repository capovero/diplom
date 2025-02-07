using backendtest.Dtos.UserDto;
using backendtest.Services;

namespace backendtest.Interfaces;

public interface IUserService
{
    Task<Result<string>> LoginAsync(LoginUserDto dto);
}